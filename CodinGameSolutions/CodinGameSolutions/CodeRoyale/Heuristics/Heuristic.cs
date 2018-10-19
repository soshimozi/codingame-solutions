using System;
using System.Collections.Generic;
using Utility;

namespace CodeRoyale
{

    public interface IHeuristicEntity<T>
    {
        T GetEntity();
        Vector2 GetPosition();
        AllianceType GetAlliance();
        EntityType GetEntityType();
        int GetLevel();
        float GetRadius();
    }

    public abstract class Heuristic
    {
        public abstract float GetRating<T>(T _evaluatedObject) where T: IHeuristicEntity<T>;
    }

    public class LocationDistanceToQueen : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject) 
        {
            float distance = Vector2.GetDistance(GameData.m_friendlyQueen.m_position, _evaluatedObject.GetPosition()).GetLength() - 
                (float)Math.Pow(_evaluatedObject.GetRadius() ,2) - (float)Math.Pow(GameData.m_friendlyQueen.m_radius,2);
            return Helpers.Clamp((1-(Math.Abs(distance) / (float)Math.Pow(HeuristicData.MAP_MAX_DISTANCE,2))), 0 , 1)* HeuristicData.LOCATION_DISTANCE_TO_QUEEN_RATING;
        }
    }

    public class IsWithinHubTerritoryRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            float distance = Vector2.GetDistance(GameData.m_hubPosition, _evaluatedObject.GetPosition()).GetLength() - (float)Math.Pow(_evaluatedObject.GetRadius(), 2);
            return Helpers.Clamp((1-(Math.Abs(distance) / (float)Math.Pow(HeuristicData.HUB_RADIUS, 2))), 0, 1) * HeuristicData.WITHIN_HUB_RANGE_RATING;
        }
    }

    public class BuildingUpgradeRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            return Helpers.Clamp((1 - (_evaluatedObject.GetLevel() / HeuristicData.MAXIMUM_LEVEL)), 0, 1) * HeuristicData.UPGRADE_RATING;
        }
    }

    public class IdealResourceIsDesiredResourceRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            BuildingLocation location = _evaluatedObject as BuildingLocation;
            if (location == null) { return 0; }
            if (StrategyManager.GetDesiredBuildingType() == location.m_idealBuildingType)
            {
                return HeuristicData.IDEAL_IS_DESIRED_RATING;
            }
            else return 0;
        }
    }

    public class EnemyNearbyRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            List<Unit> enemies;
            GameData.GetUnits(AllianceType.HOSTILE, out enemies);
            int enemiesInRange = 0;
            foreach (Unit _enemy in enemies)
            {
                if (Math.Abs(Vector2.GetDistance(_evaluatedObject.GetPosition(), _enemy.m_position).GetLength() - 
                    Math.Pow(_evaluatedObject.GetRadius(),2) - Math.Pow(_enemy.m_radius,2)) < Math.Pow(HeuristicData.THREAT_RANGE,2))
                {
                    ++enemiesInRange;
                    if (enemiesInRange >= HeuristicData.MAX_ENEMIES_IN_RANGE) { break; }
                }
            }
            return Helpers.Clamp((1-(enemiesInRange / HeuristicData.MAX_ENEMIES_IN_RANGE)), 0, 1) * HeuristicData.ENEMIES_IN_RANGE_RATING;
        }
    }

    public class IsEnemyLocationWithinHubTerritoryRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            float distance = Vector2.GetDistance(GameData.m_hubPosition, _evaluatedObject.GetPosition()).GetLength() - (float)Math.Pow(_evaluatedObject.GetRadius(),2);
            if (_evaluatedObject.GetAlliance() == AllianceType.HOSTILE && _evaluatedObject.GetEntityType() == EntityType.LOCATION &&
                 distance < (float)Math.Pow(HeuristicData.HUB_RADIUS, 2))
            {
                return HeuristicData.ENEMY_LOCATION_WITHIN_HUB_RATING;
            }
            return 0;
        }
    }

    public class LocationUnoccupiedRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            BuildingLocation location = _evaluatedObject as BuildingLocation;
            if (location == null) { return 0; }

            if (location.m_buildingType == BuildingType.NONE)
            {
                return HeuristicData.LOCATION_UNOCCUPIED_RATING;
            }
            else return 0;
        }
    }

    public class DestructionImminentRating : Heuristic
    {
        public override float GetRating<T>(T _evaluatedObject)
        {
            return 0;
        }
    }
}