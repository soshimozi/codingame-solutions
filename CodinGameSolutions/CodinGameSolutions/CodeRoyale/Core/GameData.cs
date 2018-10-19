using System.Linq;
using System.Collections.Generic;
using Utility;
using System;

namespace CodeRoyale
{
    class GameData
    {
        public static Vector2 m_centerMap = new Vector2(960, 500);
        public const int KNIGHT_COST = 80;
        public const int ARCHER_COST = 100;
        public const int GIANT_COST = 140;

        public static Dictionary<int, BuildingLocation> m_buildingMap = new Dictionary<int, BuildingLocation>();
        public static List<BuildingLocation> m_buildingList = new List<BuildingLocation>();
        public static List<Unit> m_unitList;

        public static Unit m_friendlyQueen;
        public static Unit m_enemyQueen;

        public static Vector2 m_hubPosition;

        public static int m_currentGold = 0;
        public static int m_touchedSite = 0;
        public static int m_amountOfGold = 0;
        internal static float m_queenAvoidanceRange = 80;
        internal static float m_queenFleeRange = 80;
        internal static bool m_upgradingMine = false;
        internal static int m_gameTurn =0;

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

        internal static void GetAllEntities(out List<Entity> _entityList)
        {
            _entityList = new List<Entity>();

            _entityList.AddRange(m_buildingList);
            _entityList.AddRange(m_unitList);
        }
    }
}