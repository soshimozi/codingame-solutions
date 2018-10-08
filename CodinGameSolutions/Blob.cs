/*This file was automatically generated at 9:56 AM UTC by SharpNamespaceBlobber! Made by EngiHero

*Ignored directories: 
*bin
*obj
*Properties

*Namespaces used: 
*Utility
*CodeRoyale


Visit www.bitbucket.org/engigamesbitbucket for more codinGame Solutions!*/


using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Utility;

namespace CodeRoyale
{
    class Player
    {

        static void Main(string[] args)
        {
            bool isFirstTurn = true;
            string[] inputs;
            int numSites = int.Parse(Console.ReadLine());

            for (int i = 0; i < numSites; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int siteId = int.Parse(inputs[0]);
                int x = int.Parse(inputs[1]);
                int y = int.Parse(inputs[2]);
                int radius = int.Parse(inputs[3]);

                BuildingLocation newLocation = new BuildingLocation(siteId, x, y, radius);
                GameData.m_buildingMap[siteId] = newLocation;
                GameData.m_buildingList.Add(newLocation);
            }

            // game loop
            while (true)
            {

                inputs = Console.ReadLine().Split(' ');
                int gold = int.Parse(inputs[0]);
                int touchedSite = int.Parse(inputs[1]); // -1 if none
                GameData.m_amountOfGold = gold;
                GameData.m_touchedSite = touchedSite;
                for (int i = 0; i < numSites; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int siteId = int.Parse(inputs[0]);
                    int ignore1 = int.Parse(inputs[1]); // used in future leagues
                    int ignore2 = int.Parse(inputs[2]); // used in future leagues
                    int structureType = int.Parse(inputs[3]); // -1 = No structure, 2 = Barracks
                    int owner = int.Parse(inputs[4]); // -1 = No structure, 0 = Friendly, 1 = Enemy
                    int param1 = int.Parse(inputs[5]);
                    int param2 = int.Parse(inputs[6]);

                    //Refresh all location data
                    BuildingLocation currentLocation = GameData.m_buildingMap[siteId];
                    currentLocation.m_buildingType = structureType == -1 ? BuildingType.NONE : BuildingType.BARRACKS;
                    currentLocation.m_allianceType = owner == -1 ? AllianceType.NEUTRAL : owner == 0 ? AllianceType.FRIENDLY : AllianceType.HOSTILE;
                    currentLocation.m_remainingBuildTime = param1;
                }

                List<Unit> unitList = new List<Unit>();
                int numUnits = int.Parse(Console.ReadLine());
                for (int i = 0; i < numUnits; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int x = int.Parse(inputs[0]);
                    int y = int.Parse(inputs[1]);
                    int owner = int.Parse(inputs[2]); // 0 = FRIENDLY , 1 = ENEMY
                    int unitType = int.Parse(inputs[3]); // -1 = QUEEN, 0 = KNIGHT, 1 = ARCHER
                    int health = int.Parse(inputs[4]);

                    //Create all units that exist this frame
                    AllianceType type = owner == 0 ? AllianceType.FRIENDLY : AllianceType.HOSTILE;

                    UnitType unitTypeEnum = unitType == -1 ? UnitType.QUEEN : unitType == 0 ? UnitType.KNIGHT : UnitType.ARCHER;
                    Unit newUnit = new Unit(x, y, type, unitTypeEnum, health);

                    if (unitTypeEnum == UnitType.QUEEN)
                    {
                        if (type == AllianceType.FRIENDLY)
                        {
                            GameData.m_friendlyQueen = newUnit;
                        }
                        else GameData.m_enemyQueen = newUnit;
                    }
                    unitList.Add(newUnit);
                }

                if (isFirstTurn)
                {
                    Vector2 distanceBetweenQueens = Vector2.GetDistance(GameData.m_friendlyQueen.m_position, GameData.m_enemyQueen.m_position);
                    float lengthBetweenQueens = distanceBetweenQueens.GetLengthSqr();

                    GameData.m_hubPosition = GameData.m_friendlyQueen.m_position + (distanceBetweenQueens.Normalized() * (lengthBetweenQueens /8));
                    Console.Error.WriteLine(GameData.m_hubPosition.ToString());
                }

                GameData.m_unitList = unitList;
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                string queenInstructions = "";
                string trainingInstructions = "";
                StrategyManager.Execute(out queenInstructions, out trainingInstructions);

                // First line: A valid queen action
                // Second line: A set of training instructions
                Console.WriteLine(queenInstructions);
                Console.WriteLine(trainingInstructions);
                if (isFirstTurn)
                {
                    isFirstTurn = false;
                }
            }
        }
    }
}



namespace CodeRoyale
{
    public class BuildingLocation
    {
        public BuildingLocation(int _siteId, int _x, int _y, int _radius)
        {
            m_siteId = _siteId;
            m_position = new Vector2(_x, _y);
            m_radius = _radius;
        }

        public int m_siteId;
        public Vector2 m_position;
        public int m_radius;
        public BuildingType m_buildingType = BuildingType.NONE;
        public AllianceType m_allianceType = AllianceType.NEUTRAL;
        public int m_remainingBuildTime;
        public int m_rating = 0;
        public BuildingType m_desiredBuildingType = BuildingType.NONE;
    }
}

namespace CodeRoyale
{
    class GameData
    {
        public const int KNIGHT_COST = 80;
        public static Dictionary<int, BuildingLocation> m_buildingMap = new Dictionary<int, BuildingLocation>();
        public static List<BuildingLocation> m_buildingList = new List<BuildingLocation>();
        public static List<Unit> m_unitList;

        public static Unit m_friendlyQueen;
        public static Unit m_enemyQueen;

        public static Vector2 m_hubPosition;

        public static int m_currentGold = 0;
        public static int m_touchedSite = 0;
        public static int m_amountOfGold = 0;

        public static void GetBuildingLocations(AllianceType _type, out List<BuildingLocation> _locationList)
        {
            _locationList = m_buildingList.Where(x => x.m_allianceType == _type).ToList();
        }
        public static void GetBuildingLocations(BuildingType _type, out List<BuildingLocation> _locationList)
        {
            _locationList = m_buildingList.Where(x => x.m_buildingType == _type).ToList();
        }

        public static void GetUnits(AllianceType _type, out List<Unit> _unitList)
        {
            _unitList = m_unitList.Where(x => x.m_allianceType == _type).ToList();
        }
        public static void GetUnits(UnitType _type, out List<Unit> _unitList)
        {
            _unitList = m_unitList.Where(x => x.m_unitType == _type).ToList();
        }

        internal static BuildingLocation GetBuilding(int _touchedSite)
        {
            return m_buildingMap[_touchedSite];
        }
    }
}
namespace CodeRoyale
{
    public enum UnitType
    {
        UNSET,
        QUEEN,
        KNIGHT,
        ARCHER
    }

    public enum BuildingType
    {
        NONE,
        BARRACKS,
        BARRACKS_KNIGHT,
        BARRACKS_ARCHER,
        TOWER,
        MINE
    }

    public enum AllianceType
    {
        NEUTRAL,
        FRIENDLY,
        HOSTILE
    }
}

namespace CodeRoyale
{
    class Helpers
    {
        public static int GetAmountOfFriendlyBarracks()
        {
            List<BuildingLocation> buildingLocations;
            GameData.GetBuildingLocations(AllianceType.FRIENDLY, out buildingLocations);
            int amount = buildingLocations.Count(x => x.m_buildingType != BuildingType.NONE);
            return amount;
        }

        public static Vector2 GetNearestEmptyBuildingLocation(Vector2 _currentLocation)
        {
            List<BuildingLocation> emptyLocations;
            GameData.GetBuildingLocations(BuildingType.NONE, out emptyLocations);
            int smallestDistance = 9999999;
            BuildingLocation nearestLocation = null;

            foreach (BuildingLocation _location in emptyLocations)
            {
                float distance = Vector2.GetDistance(_currentLocation, _location.m_position).GetLength();
                if (distance < smallestDistance)
                {
                    smallestDistance = (int)distance;
                    nearestLocation = _location;
                }
            }

            if (nearestLocation != null)
            {
                return nearestLocation.m_position;
            }
            else return Vector2.zero;
        }

        internal static string GetBuildCommand(BuildingLocation _location)
        {
            return string.Format("BUILD {0} {1}", _location.m_siteId, Helpers.GetNameFromType(_location.m_desiredBuildingType));
        }

        public static string GetQueenMoveCommand(Vector2 _position)
        {
            return string.Format("{0} {1} {2}", "MOVE", _position.x, _position.y);
        }
        public static string GetTrainCommand(List<int> _intList)
        {
            string command = "TRAIN";

            foreach (int _int in _intList)
            {
                command += String.Format(" {0}", _int);
            }

            return command;
        }
        internal static string GetNameFromType(UnitType _type)
        {
            switch (_type)
            {
                case UnitType.ARCHER:
                    return "ARCHER";
                case UnitType.KNIGHT:
                    return "KNIGHT";
                case UnitType.QUEEN:
                    return "QUEEN";
                default:
                    return "UNSET";
            }
        }

        internal static string GetNameFromType(BuildingType _type)
        {
            switch (_type)
            {
                case BuildingType.NONE:
                    return "NONE";
                case BuildingType.BARRACKS:
                    return "BARRACKS";
                case BuildingType.BARRACKS_KNIGHT:
                    return "BARRACKS-KNIGHT";
                case BuildingType.BARRACKS_ARCHER:
                    return "BARRACKS-ARCHER";
                case BuildingType.TOWER:
                    return "TOWER";
                case BuildingType.MINE:
                    return "MINE";
                default:
                    return "UNSET";
            }
        }
    }
}

namespace CodeRoyale
{
    struct Unit
    {
        public Unit(int _x, int _y, AllianceType _allianceType, UnitType _unitType, int _health)
        {
            m_position = new Vector2(_x, _y);
            m_allianceType = _allianceType;
            m_unitType = _unitType;
            m_health = _health;
        }
        public Vector2 m_position;
        public AllianceType m_allianceType;
        public UnitType m_unitType;
        public int m_health;
    }
}

namespace CodeRoyale
{
    public class EvaluationSystem
    {
        public EvaluationSystem()
        {
            m_buildingHeursticList.Add(new DistanceToHubPositionHeuristic(2000));
            m_buildingHeursticList.Add(new DistanceToEnemyQueenHeuristic(500));
            m_buildingHeursticList.Add(new IsEmptyHeuristic(2000));
        }

        List<BuildingHeuristic> m_buildingHeursticList = new List<BuildingHeuristic>();


        public void EvaluateBuildingLocation(BuildingLocation _location)
        {
            float totalRating = 0;
            foreach (BuildingHeuristic _heuristic in m_buildingHeursticList)
            {
                totalRating += _heuristic.GetRating(_location);
            }
            _location.m_rating = (int)totalRating;
        }

        public BuildingLocation GetBestBuildingLocation()
        {
            GameData.m_buildingList.ForEach(x => EvaluateBuildingLocation(x));

            GameData.m_buildingList = GameData.m_buildingList.OrderByDescending(x => x.m_rating).ToList();
            //GameData.m_buildingList.ForEach(x => Console.Error.WriteLine(x.m_siteId + "  " + x.m_rating));
            return GameData.m_buildingList[0];
        }

        // If far from hub,we probably want to build a tower
        // Do we need barracks ? 
        // Build mine
        public void EvaluateBestTypeToBuild(BuildingLocation _location)
        {
            Vector2 hubPosition = GameData.m_hubPosition;
            float absDistance = Math.Abs(Vector2.GetDistance(_location.m_position, hubPosition).GetLength());

            if (absDistance > 400 * 400 && !BarracksRequired())
            {
                _location.m_desiredBuildingType = BuildingType.TOWER;
            }
            else if (BarracksRequired())
            {
                _location.m_desiredBuildingType = BuildingType.BARRACKS_KNIGHT;
            }
            else
            {
                _location.m_desiredBuildingType = BuildingType.MINE;
            }
        }


        public bool BarracksRequired()
        {
            int amountOfGold = GameData.m_amountOfGold;
            int maximumSpendingNextTurn = Helpers.GetAmountOfFriendlyBarracks() * GameData.KNIGHT_COST;

            return amountOfGold > maximumSpendingNextTurn * 2;
        }
    }
}

namespace CodeRoyale
{
    public abstract class BuildingHeuristic
    {
        protected float m_maxRating = 0;
        public abstract float GetRating(BuildingLocation _location);
    }

    public class DistanceToHubPositionHeuristic : BuildingHeuristic
    {
        public DistanceToHubPositionHeuristic(float _maxRating)
        {
            m_maxRating = _maxRating;
        }

        const float MAX_DISTANCE = 2200.0f;

        public override float GetRating(BuildingLocation _location)
        {
            float distance = Vector2.GetDistance(_location.m_position, GameData.m_hubPosition).GetLength();
            return (1 - (Math.Abs(distance) / (MAX_DISTANCE * MAX_DISTANCE))) * m_maxRating;
        }
    }

    public class DistanceToEnemyQueenHeuristic : BuildingHeuristic
    {
        const float MAX_DISTANCE = 2200.0f;

        public DistanceToEnemyQueenHeuristic(float _maxRating)
        {
            m_maxRating = _maxRating;
        }

        public override float GetRating(BuildingLocation _location)
        {
            float distance = Vector2.GetDistance(_location.m_position, GameData.m_enemyQueen.m_position).GetLength();
            return (Math.Abs(distance) / (MAX_DISTANCE * MAX_DISTANCE)) * m_maxRating;
        }
    }

    public class IsEmptyHeuristic : BuildingHeuristic
    {
        public IsEmptyHeuristic(float _maxRating)
        {
            m_maxRating = _maxRating;
        }

        public override float GetRating(BuildingLocation _location)
        {
            return _location.m_buildingType == BuildingType.NONE ? m_maxRating : 0;
        }
    }
}


namespace CodeRoyale
{
    public static class Strategies
    {
        public static EvaluationSystem m_evaluationSystem = new EvaluationSystem();

        //Moves to nearest empty location and builds a knight barracks
        public static string GetBasicQueenStrategy()
        {
            string queenInstructions = "WAIT";

            BuildingLocation bestLocation = m_evaluationSystem.GetBestBuildingLocation();
            queenInstructions = Helpers.GetQueenMoveCommand(bestLocation.m_position);

            //The queen is at a building location
            if (GameData.m_touchedSite != -1)
            {
                BuildingLocation touchedLocation = GameData.GetBuilding(GameData.m_touchedSite);

                if (touchedLocation.m_buildingType == BuildingType.NONE)
                {
                    m_evaluationSystem.EvaluateBestTypeToBuild(touchedLocation);
                    queenInstructions = Helpers.GetBuildCommand(touchedLocation);
                }
            }
            return queenInstructions;
        }

        public static string BuildAllPossibleKnights()
        {

            List<BuildingLocation> locationList;
            GameData.GetBuildingLocations(AllianceType.FRIENDLY, out locationList);

            locationList = locationList.Where(x => x.m_buildingType != BuildingType.NONE && x.m_remainingBuildTime == 0).ToList();
            int maxAmountOfUnits = (int)Math.Floor((decimal)(GameData.m_amountOfGold / GameData.KNIGHT_COST));

            List<int> idList = new List<int>();
            locationList.ForEach(x => idList.Add(x.m_siteId));

            int amountOfBarrack = idList.Count < maxAmountOfUnits ? idList.Count : maxAmountOfUnits;
            idList = idList.GetRange(0, amountOfBarrack);
            return Helpers.GetTrainCommand(idList);
        }
    }
}


namespace CodeRoyale
{
    class StrategyManager
    {
        public static void Execute(out string _queenInstructions, out string _trainingInstructions)
        {
            _queenInstructions = "WAIT";
            _trainingInstructions = "TRAIN";

            _queenInstructions = Strategies.GetBasicQueenStrategy();
            _trainingInstructions = Strategies.BuildAllPossibleKnights();
        }
    }
}

namespace Utility
{
    // Vector2 struct to hold x y position and Cardinal equivalent
    public struct Vector2
    {
        public static Vector2 zero;

        public Vector2(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        //Overloading Operator- example
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        //Overloading Operator+ example
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2 operator *(Vector2 lsh, float _length)
        {
            return new Vector2(lsh.x * _length, lsh.y * _length);
        }

        public double x;
        public double y;

        //returns vector from initial to final
        public static Vector2 GetDistance(Vector2 _initial, Vector2 _final)
        {
            return _final - _initial;
        }

        //Normalizes this vector
        public void Normalize()
        {
            Vector2 vectorNormalized = Normalized();
            x = vectorNormalized.x;
            y = vectorNormalized.y;
        }

        // Returns a new vector equal to this vector normalized
        public Vector2 Normalized()
        {
            // a^2 + b^2 = c^2
            double length = GetLengthSqr();

            Vector2 returnVector = this;

            returnVector.x = x / length;
            returnVector.y = y / length;

            return returnVector;
        }

        public float GetLengthSqr()
        {
            // a^2 + b^2 = c^2
            return (float)Math.Sqrt(x * x + y * y);
        }

        public float GetLength()
        {
            return (float)(x * x + y * y);
        }

        public override string ToString()
        {
            return x.ToString() + ", " + y.ToString();
        }
    }
}
