
using CodeRoyale;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CRReferee
{
    public static class EntityManager
    {
        public static int m_siteIdCount=0;
        static int m_maxLocationCount = 24;
        static int m_minLocationCount = 16;
        static int m_locationMaxRadius = 80;
        static int m_locationMinRadius = 50;
        static int m_borderSize = 160;
        static int m_amountOfRows = 4;

        static public int m_finalLocationCount = 0;
        static public List<Unit> m_unitList = new List<Unit>();
        static public List<BuildingLocation> m_locationList = new List<BuildingLocation>();

        public static Unit m_player1Queen;
        public static Unit m_player2Queen;

        static Vector2 m_initialCursorPosition = new Vector2();
        static public void Update()
        {

        }

        static internal void CreateAllEntities()
        {
            m_initialCursorPosition = new Vector2(Console.CursorLeft,Console.CursorTop);
            CreateInitialBuildingLocations();
            CreateQueens();
        }

        static private void CreateQueens()
        {
            BuildingLocation queenSpawnLocation = m_locationList[0];
            Vector2 firstQueenPos = queenSpawnLocation.m_position;
            firstQueenPos.y = queenSpawnLocation.m_position.y + (queenSpawnLocation.m_radius);
            m_player1Queen = new Unit((int)firstQueenPos.x, (int)firstQueenPos.y, AllianceType.FRIENDLY, UnitType.QUEEN);
            m_player1Queen.m_radius = 30;
            m_unitList.Add(m_player1Queen);

            BuildingLocation enemySpawnLocation = m_locationList.Last();
            Vector2 secondQueenPos = enemySpawnLocation.m_position;
            secondQueenPos.y = enemySpawnLocation.m_position.y - (enemySpawnLocation.m_radius);
            m_player2Queen = new Unit((int)secondQueenPos.x, (int)secondQueenPos.y, AllianceType.HOSTILE, UnitType.QUEEN);
            m_player2Queen.m_radius = 30;
            m_unitList.Add(m_player2Queen);

        }


     
        static private void CreateInitialBuildingLocations()
        {
            m_finalLocationCount = RefereeData.m_random.Next(m_minLocationCount, m_maxLocationCount);
            if (m_finalLocationCount % 2 != 0) { ++m_finalLocationCount; }

            float columns = (float)Math.Ceiling((double)m_finalLocationCount / (m_amountOfRows));
            float xIncrement = (RefereeData.m_fieldWidth-m_borderSize) / columns;
            float yIncrement = (RefereeData.m_fieldHeight-m_borderSize) / m_amountOfRows;

            int locationsPerRow = m_finalLocationCount / 4;
            int firstRowCount = RefereeData.m_random.Next((int)Math.Ceiling((double)locationsPerRow / 2) + 1, locationsPerRow);

            float nextSpawnX = xIncrement + m_borderSize/2;
            float nextSpawnY = yIncrement + m_borderSize/2;
            for (int i = 1; i <= m_finalLocationCount / 2; ++i)
            {
                if (i == m_finalLocationCount/2 - firstRowCount)
                {
                    nextSpawnY += yIncrement;
                    nextSpawnX = (int)(xIncrement*1.5);
                }

                SpawnTwinLocationsWithOffset((int)nextSpawnX - (int)(xIncrement/2),(int)nextSpawnY - (int)(yIncrement/2));
                nextSpawnX += xIncrement;
            }


            //DrawAllEntities(); 
            m_locationList = m_locationList.OrderBy(x => x.m_siteId).ToList();
        }

        internal static void AgeCreeps()
        {
            m_unitList.Where(x => x.m_unitType != UnitType.QUEEN).ToList().ForEach(x => --x.m_health);
        }

        internal static void ProcessStructureTurn()
        {
            foreach (BuildingLocation _location in m_locationList)
            {
                switch (_location.m_buildingType)
                {
                    case BuildingType.BARRACKS_KNIGHT:
                    case BuildingType.BARRACKS_ARCHER:
                    case BuildingType.BARRACKS_GIANT:
                        int buildTime = _location.m_remainingBuildTime;

                        if (buildTime != -1)
                        {
                            --_location.m_remainingBuildTime;
                        }
                        if (buildTime == 0)
                        {
                            Spawn(_location);
                            --_location.m_remainingBuildTime; 
                        }
                        if (_location.m_remainingBuildTime < -1) { _location.m_remainingBuildTime = -1;}
                        break;
                    case BuildingType.MINE:
                        PlayerData player = _location.m_allianceType == AllianceType.FRIENDLY ? CodeRoyaleReferee.m_player1 : CodeRoyaleReferee.m_player2;
                        if (_location.m_level >= 0){player.m_gold += _location.m_level;}
                        if (_location.m_goldRemaining > 0){ --_location.m_goldRemaining; }
                        break;

                    case BuildingType.TOWER:
                        _location.m_level -=4;

                        float attackRadius = (float)((_location.m_level * 1000 + (Math.PI*(_location.m_radius * _location.m_radius))) / Math.PI);

                        List<Unit> unitsInRange = m_unitList.Where(x => x.m_allianceType != _location.m_allianceType && 
                        Vector2.GetDistance(_location.m_position, x.m_position).GetLength() <= attackRadius).ToList();

                        unitsInRange = unitsInRange.OrderBy(x => Vector2.GetDistance(_location.m_position, x.m_position).GetLength()).ToList();

                        Unit target = null;
                        if (unitsInRange.Count > 2 && unitsInRange[0].m_unitType == UnitType.QUEEN)
                        {
                            target = unitsInRange[1];
                        }
                        else if (unitsInRange.Count != 0)
                        {
                            target = unitsInRange[0];
                        }
                        
                        if (target != null)
                        {
                            float distanceToTower = Vector2.GetDistance(target.m_position, _location.m_position).GetLength();
                            float distanceFromEdge = attackRadius - distanceToTower;

                            int damage = (int)(distanceFromEdge / 200);
                            target.m_health -= damage = target.m_unitType == UnitType.QUEEN ? 1 : 3;
                        }

                        List<Unit> giantsInRange = m_unitList.Where(x => x.m_unitType == UnitType.GIANT && x.m_allianceType != _location.m_allianceType).ToList();
                        giantsInRange.ForEach(x => _location.m_level -= 80);
                        break;
                    default:
                        break;
                }
            }
        }

        internal static void ResolveAllQueenDamage()
        {
            List<BuildingLocation> buildingList = m_locationList.Where(x => 
            Vector2.GetDistance(m_player1Queen.m_position, x.m_position).GetLength() - Math.Pow(m_player1Queen.m_radius,2) - Math.Pow(x.m_radius,2) <= 25).ToList();
            CodeRoyaleReferee.m_player1.m_touchedSite = -1;
            CodeRoyaleReferee.m_player2.m_touchedSite = -1;
            foreach (BuildingLocation _location in buildingList)
            {
                CodeRoyaleReferee.m_player1.m_touchedSite = _location.m_siteId;
                if (_location.m_allianceType != m_player1Queen.m_allianceType && _location.m_buildingType == BuildingType.MINE)
                {
                    //Destroy location
                    _location.Reset();
                }
                break;
            }

            buildingList = m_locationList.Where(x =>
            Vector2.GetDistance(m_player2Queen.m_position, x.m_position).GetLength() - Math.Pow(m_player2Queen.m_radius,2) - Math.Pow(x.m_radius, 2) <= 25).ToList();
            foreach (BuildingLocation _location in buildingList)
            {
                CodeRoyaleReferee.m_player2.m_touchedSite = _location.m_siteId;
                if (_location.m_allianceType != m_player2Queen.m_allianceType && _location.m_buildingType == BuildingType.MINE)
                {
                    //Destroy location
                    _location.Reset();
                }
                break;
            }
        }

        internal static void ResolveCreepDamage()
        {
            foreach (Unit _unit in m_unitList)
            {
                switch (_unit.m_unitType)
                {
                    case UnitType.KNIGHT:
                        Unit enemyQueen = _unit.m_allianceType == AllianceType.FRIENDLY ? m_player2Queen : m_player1Queen;
                        if (Vector2.GetDistance(enemyQueen.m_position, _unit.m_position).GetLength() - Math.Pow(enemyQueen.m_radius, 2) - Math.Pow(_unit.m_radius, 2) <= 25)
                        {
                            --enemyQueen.m_health;
                        }
                        List<BuildingLocation> locationsInRange = m_locationList.Where(x => x.m_buildingType == BuildingType.MINE &&
                        Vector2.GetDistance(x.m_position, _unit.m_position).GetLength() - Math.Pow(x.m_radius,2) - Math.Pow(_unit.m_radius,2) <= 25).ToList();
                        break;
                    case UnitType.ARCHER:
                        //Creeps in range

                        break;
                    default:
                        //Towers
                        break;
                }
            }
        }

        internal static void MoveAllEntities()
        {
            foreach (Unit _unit in m_unitList)
            {
                AllianceType allianceType = _unit.m_allianceType;

                switch (_unit.m_unitType)
                {
                    case UnitType.KNIGHT:
                        Unit opposingQueen = allianceType == AllianceType.FRIENDLY ? m_player2Queen : m_player1Queen;
                        MoveEntity(_unit, opposingQueen.m_position);
                        break;
                    case UnitType.ARCHER:
                        List<Unit> unitRangeList = m_unitList.OrderBy(x => Vector2.GetDistance(_unit.m_position,x.m_position).GetLength() - 
                        Math.Pow(_unit.m_radius,2) - Math.Pow(x.m_radius,2)).ToList();
                        MoveEntity(_unit, unitRangeList[0].m_position);
                        break;
                    case UnitType.GIANT:
                        List<BuildingLocation> enemyTowerRangeList = m_locationList.Where(x => x.m_buildingType == BuildingType.TOWER &&
                        x.m_allianceType != allianceType).OrderBy(x => Vector2.GetDistance(_unit.m_position, x.m_position).GetLength() -
                        Math.Pow(_unit.m_radius, 2) - Math.Pow(x.m_radius, 2)).ToList();
                        MoveEntity(_unit, enemyTowerRangeList[0].m_position);
                        break;
                }
            }
        }

        internal static void ResolveAllCollisions()
        {
            List<Entity> entityList = GetAllEntities();

            int count = 0;
            bool entitiesColliding = true;
      
            while (entitiesColliding)
            {
                ++count;
                SnapToBounds(entityList);
                entitiesColliding = false;
                bool collisionFound = true;

                foreach (Entity _entity in entityList)
                {
                    int loopCount = 0;
                    while (collisionFound)
                    {
                        ++loopCount;
                        collisionFound = false;

                        List<Entity> entitiesInRange = entityList.Where(x => x != _entity && Vector2.GetDistance(x.m_position, _entity.m_position).GetLength() -
                        Math.Pow(x.m_radius, 2) - Math.Pow(_entity.m_radius, 2) <= 25).ToList();

                        if (entitiesInRange.Count != 0)
                        {
                            entitiesColliding = true;
                            collisionFound = true;
                            entitiesInRange = entitiesInRange.OrderBy(x => Vector2.GetDistance(x.m_position, _entity.m_position).GetLength() -
                            Math.Pow(x.m_radius, 2) - Math.Pow(_entity.m_radius, 2)).ToList();
                            entitiesInRange.ForEach(x => RepositionOutOfRange(x, _entity));
                        }

                        if (loopCount > 100) { break; } 
                    }
                }
                if (count > 100) { break; }
            }
        }

        private static void SnapToBounds(List<Entity> _entityList)
        {
            foreach (Entity _entity in _entityList)
            {
                if (_entity as BuildingLocation != null) { continue; }
                Vector2 position = _entity.m_position;
                float radius = _entity.m_radius +5;
                if (position.x - radius <= 0) { position.x = radius; }
                if (position.x + radius >= RefereeData.m_fieldWidth) { position.x = RefereeData.m_fieldWidth - radius; }
                if (position.y - radius <= 0) { position.y = radius; }
                if (position.y + radius >= RefereeData.m_fieldHeight) { position.y = RefereeData.m_fieldHeight - radius; }
            }
        }

        private static void RepositionOutOfRange(Entity _entity, Entity _entityInRange)
        {
            float totalWeight = _entity.m_weight + _entityInRange.m_weight;
            float entityWeightRatio = 0;
            float entityInRangeWeightRatio = 0;

            if (totalWeight <= 0) { return; }
            
            entityWeightRatio = _entity.m_weight / totalWeight;
            entityInRangeWeightRatio = 1 - entityWeightRatio;

            Vector2 distance = Vector2.GetDistance(_entity.m_position, _entityInRange.m_position);
            float distanceToCenterSqr = distance.GetLengthSqr() / 2;
            if (distanceToCenterSqr < 1) { distanceToCenterSqr = 1; }

            float totalDistance = _entity.m_radius + _entityInRange.m_radius + 10;

            if (_entity as BuildingLocation == null) 
            {
                _entity.m_position = _entity.m_position - (distance.Normalized() * Math.Abs(((entityWeightRatio * (totalDistance / 2)) - distanceToCenterSqr)));
            }

            if (_entity as BuildingLocation == null)
            {
                _entityInRange.m_position = _entityInRange.m_position + (distance.Normalized() * Math.Abs(((entityInRangeWeightRatio * (totalDistance / 2)) - distanceToCenterSqr)));
            }
        }

        private static List<Entity> GetAllEntities()
        {
            List<Entity> entityList = new List<Entity>();

            entityList.AddRange(m_locationList);
            entityList.AddRange(m_unitList);

            return entityList;
        }

        internal static void Build(AllianceType _allianceType, int siteId, BuildingType _type)
        {
            BuildingLocation location = m_locationList[siteId];

            if (_allianceType != location.m_allianceType && location.m_buildingType == BuildingType.TOWER)
            {
                return;
            }


            if (_allianceType == location.m_allianceType && _type == location.m_buildingType)
            {
                if (location.m_buildingType == BuildingType.MINE)
                {
                    ++location.m_level;
                    if (location.m_level > location.m_maxYield) { location.m_level = location.m_maxYield; }
                }
                else if (location.m_buildingType == BuildingType.TOWER)
                {
                    location.m_level += 100;
                    if (location.m_level > 800) { location.m_level = 800; }
                }
            }
            else 
            {
                location.Reset();
                location.m_allianceType = _allianceType;
                location.m_buildingType = _type;

                if (location.m_buildingType == BuildingType.TOWER) { location.m_level = 100; }
            }
        }

        internal static void MoveEntity(Unit _unit, Vector2 _position)
        {
            Vector2 direction = Vector2.GetDistance(_unit.m_position, _position);
            direction = direction.Normalized() * _unit.m_movementSpeed;
            _unit.m_position = _unit.m_position + direction;
        }

        static private void SpawnTwinLocationsWithOffset(int _x, int _y)
        {
            Random random = RefereeData.m_random;

            _x = random.Next(_x - 10, _x + 10);
            _y = random.Next(_y - 10, _y + 10);
            Vector2 centerOfMap = new Vector2(RefereeData.m_fieldWidth/2, RefereeData.m_fieldHeight/2);
            float distanceToCenter = Vector2.GetDistance(centerOfMap, new Vector2(_x, _y)).GetLength();

            int distanceBonus = 0;
            if (distanceToCenter <= 500 * 500) { ++distanceBonus; }
            if (distanceToCenter <= 200 * 200) { ++distanceBonus; }

           

            int randomRadius = random.Next(m_locationMinRadius, m_locationMaxRadius);
            int randomGoldValue = random.Next(200,250) + (50*distanceBonus);
            int maximumMiningRate = random.Next(1,3) + distanceBonus;

            BuildingLocation newBuildingLocation = new BuildingLocation(m_siteIdCount, _x, _y, randomRadius);
            newBuildingLocation.m_buildingType = BuildingType.NONE;
            newBuildingLocation.m_allianceType = AllianceType.NEUTRAL;
            newBuildingLocation.m_goldRemaining = randomGoldValue;
            newBuildingLocation.m_maxYield = maximumMiningRate;
            m_locationList.Add(newBuildingLocation);

            ++m_siteIdCount;
            

            BuildingLocation twinBuildingLocation = new BuildingLocation(m_finalLocationCount -m_siteIdCount, 
                RefereeData.m_fieldWidth - _x, RefereeData.m_fieldHeight - _y, randomRadius);

            twinBuildingLocation.m_buildingType = BuildingType.NONE;
            twinBuildingLocation.m_allianceType = AllianceType.NEUTRAL;
            twinBuildingLocation.m_goldRemaining = randomGoldValue;
            twinBuildingLocation.m_maxYield = maximumMiningRate;
            m_locationList.Add(twinBuildingLocation);

        }

        internal static void StartTraining(PlayerData player, string buildOutputPlayer)
        {
            List<string> args = buildOutputPlayer.Split(' ').ToList();

            //First is TRAIN command
            for (int i = 1; i < args.Count; ++i)
            {
                int id = int.Parse(args[i]);
                TryToTrainCreepsFromBuilding(player, id);
            }

        }

        private static void TryToTrainCreepsFromBuilding(PlayerData _player, int id)
        {
            BuildingLocation location = m_locationList[id];

            if (location.m_remainingBuildTime > 0) { return; }
            int price = GetPriceFromBuildingType(location.m_buildingType);
            if (_player.m_gold >= price)
            {
                location.m_remainingBuildTime = GetBuildTimeFromType(location);
                _player.m_gold -= price;
            }
        }

        private static int GetBuildTimeFromType(BuildingLocation location)
        {
            switch (location.m_buildingType)
            {
                case BuildingType.BARRACKS_KNIGHT:
                    return 5;
                case BuildingType.BARRACKS_ARCHER:
                    return 8;
                case BuildingType.BARRACKS_GIANT:
                    return 10;
                default:
                    return 0;
            }
        }

        private static void Spawn(BuildingLocation _location)
        {
            switch (_location.m_buildingType)
            {
                case BuildingType.BARRACKS_KNIGHT:
                    for (int i = 0; i < 4; ++i)
                    {
                        m_unitList.Add(new Unit((int)_location.m_position.x,(int)_location.m_position.y, _location.m_allianceType, UnitType.KNIGHT));
                    }
                    break;
                case BuildingType.BARRACKS_ARCHER:
                    for (int i = 0; i < 2; ++i)
                    {
                        m_unitList.Add(new Unit((int)_location.m_position.x, (int)_location.m_position.y, _location.m_allianceType, UnitType.ARCHER));
                    }
                    break;
                case BuildingType.BARRACKS_GIANT:
                    for (int i = 0; i < 1; ++i)
                    {
                        m_unitList.Add(new Unit((int)_location.m_position.x, (int)_location.m_position.y, _location.m_allianceType, UnitType.GIANT));
                    }
                    break;
                default:
                    break;
            }
        }

        private static int GetPriceFromBuildingType(BuildingType _buildingType)
        {
            switch (_buildingType)
            {
                case BuildingType.BARRACKS_KNIGHT:
                    return GameData.KNIGHT_COST;
                case BuildingType.BARRACKS_ARCHER:
                    return GameData.ARCHER_COST;
                case BuildingType.BARRACKS_GIANT:
                    return GameData.GIANT_COST;
                default:
                    return 0;
            }
        }

        public static void DrawAllEntities()
        {
            foreach (BuildingLocation _location in m_locationList)
            {
                DrawEntity(_location.m_position, (int)_location.m_radius, _location.m_allianceType == AllianceType.FRIENDLY ? ConsoleColor.Green : ConsoleColor.Red);
            }
            foreach (Unit _unit in m_unitList)
            {
                DrawEntity(_unit.m_position, (int)_unit.m_radius, _unit.m_allianceType ==  AllianceType.FRIENDLY? ConsoleColor.Green : ConsoleColor.Red);
            }

            for (int i = 0; i < RefereeData.m_fieldHeight; ++i)
            {
                for (int t=0; t < RefereeData.m_fieldWidth; ++t)
                {
                    if (i == 0 || i == RefereeData.m_fieldHeight-1)
                    {
                        DrawPoint(new Vector2(t, i), ConsoleColor.Green);
                    }
                    else
                    {
                        if (t == 0 || t == RefereeData.m_fieldWidth-1)
                        {
                            DrawPoint(new Vector2(t, i), ConsoleColor.Green);
                        }
                    }
                }
            }
            Console.ReadKey();
        }


        static void DrawEntity(Vector2 _position, int _radius, ConsoleColor _color = ConsoleColor.White)
        {
            List<Vector2> offsets = new List<Vector2>();
            int threshold = _radius * _radius;
            for (int i = -_radius; i < _radius; i++)
            {
                for (int j = -_radius; j < _radius; j++)
                {
                    if (i * i + j * j < threshold)
                        offsets.Add(new Vector2(i, j));
                }
            }

            foreach (Vector2 _vector2 in offsets)
            {
                DrawPoint(_position + _vector2, _color);
            }
        }

        static private void DrawPoint(Vector2 vector2, ConsoleColor _color)
        {
            Console.ForegroundColor = _color;
            Console.SetCursorPosition(Math.Abs((int)vector2.x / 20), Math.Abs((int)vector2.y / 40));
            Console.Write('*');
        }
    }
}