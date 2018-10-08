using Utility;

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