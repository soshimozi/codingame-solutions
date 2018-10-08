using System;
using Utility;

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