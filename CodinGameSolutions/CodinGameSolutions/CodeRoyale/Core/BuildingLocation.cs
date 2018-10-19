using System;
using Utility;

namespace CodeRoyale
{
    public class BuildingLocation : Entity, IHeuristicEntity<BuildingLocation>
    {
        public BuildingLocation(int _siteId, int _x, int _y, int _radius)
        {
            m_siteId = _siteId;
            m_position = new Vector2(_x, _y);
            m_radius = _radius;
            m_weight = -1;
        }

        public int m_level = 0;
        public int m_siteId;
        internal int m_goldRemaining;
        internal int m_maxYield;
        public int m_remainingBuildTime =-1;
        public int m_buildingPriorityRating = 0;
        public int m_attackRadius = 0;

        public BuildingType m_idealBuildingType = BuildingType.NONE;
        private EntityType m_entityType = EntityType.LOCATION;
        public BuildingType m_buildingType = BuildingType.NONE;
        public AllianceType m_allianceType = AllianceType.NEUTRAL;


        public BuildingLocation GetEntity() { return this; }
        public Vector2 GetPosition() { return m_position; }
        public AllianceType GetAlliance() { return m_allianceType; }
        public EntityType GetEntityType() { return m_entityType; }
        public int GetLevel(){ return m_level; }
        public float GetRadius() { return m_radius; }

        internal void Reset()
        {
            m_level = 0;
            m_remainingBuildTime = -1;
            m_attackRadius = 0;
            m_allianceType = AllianceType.NEUTRAL;
        }
    }
}