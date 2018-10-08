using System.Linq;
using System.Collections.Generic;
using Utility;

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