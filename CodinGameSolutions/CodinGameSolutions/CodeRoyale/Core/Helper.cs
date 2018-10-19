using System;
using System.Linq;
using System.Collections.Generic;
using Utility;

namespace CodeRoyale
{
    public static class Helpers
    {
        //move to utility TODO
        public static T Clamp<T>(this T _val, T _min, T _max) where T : IComparable<T>
        {
            if (_val.CompareTo(_min) < 0) return _min;
            else if (_val.CompareTo(_max) > 0) return _max;
            else return _val;
        }


        public static int GetAmountOfFriendlyBarracks()
        {
            List<BuildingLocation> buildingLocations;
            GameData.GetBuildingLocations(AllianceType.FRIENDLY, out buildingLocations);
            int amount = buildingLocations.Count(x => (int)x.m_buildingType >= 3);
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
                float distance = Math.Abs(Vector2.GetDistance(_currentLocation, _location.m_position).GetLength());
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

        internal static Vector2 VerifyAndAvoidCollisions(BuildingLocation _target)
        {
            Unit queen = GameData.m_friendlyQueen;
            Vector2 direction = (Vector2.GetDistance(queen.m_position, _target.m_position).Normalized() * GameData.m_queenAvoidanceRange);
            Vector2 targetPosition = queen.m_position + direction;

            List<Entity> entityList;
            GameData.GetAllEntities(out entityList);

            float lastAvoidanceAngle = 0;
            while (true)
            {
                foreach (Entity _entity in entityList)
                {
                    if (_entity == queen || _entity == _target) { continue; }
                    Vector2 distanceToEntity = Vector2.GetDistance(targetPosition, _entity.m_position);
                    float avoidanceRadius = _entity.m_radius + queen.m_radius;
                    if ( distanceToEntity.GetLength() < avoidanceRadius* avoidanceRadius)
                    {
                        float outAvoidanceAngle = 0;
                        targetPosition = GetAvoidanceDestination(direction, distanceToEntity,avoidanceRadius, lastAvoidanceAngle, out outAvoidanceAngle);
                        lastAvoidanceAngle = outAvoidanceAngle;
                        direction = (Vector2.GetDistance(queen.m_position, targetPosition).Normalized() * (GameData.m_queenAvoidanceRange));
                        break;
                    }
                }
                break;
            }

            return targetPosition;
        }

        private static Vector2 GetAvoidanceDestination(Vector2 _targetDirection, Vector2 _collisionVector, float _avoidanceRadius, float _lastAvoidanceAngle, out float _outAvoidanceAngle)
        {
            Vector2 queenPosition = GameData.m_friendlyQueen.m_position;

            _outAvoidanceAngle = (float)Vector2.GetAngleBetween(_collisionVector, _targetDirection,  true);
            bool clockwise = _outAvoidanceAngle <= 0;
            Vector2 newTarget = (queenPosition + _collisionVector) +
                (Vector2.GetPerpendicularVector(_collisionVector.Normalized(), clockwise) * (_avoidanceRadius + GameData.m_friendlyQueen.m_radius));

            _outAvoidanceAngle = (float)Vector2.GetAngleBetween(Vector2.GetDistance(queenPosition, newTarget), _targetDirection, true);
            if (_lastAvoidanceAngle != 0)
            {
                _outAvoidanceAngle = Math.Abs(_outAvoidanceAngle) * Math.Sign(_lastAvoidanceAngle);
            }

            return (queenPosition + (Vector2.Rotate(_targetDirection, _outAvoidanceAngle).Normalized() * (60)));
        }

        internal static string GetBuildCommand(BuildingLocation _location, BuildingType _override = BuildingType.NONE)
        {
            return string.Format("BUILD {0} {1}", _location.m_siteId, GetNameFromType(_override == BuildingType.NONE? _location.m_idealBuildingType : _override));
        }

        public static string GetQueenMoveCommand(Vector2 _position)
        {
            return string.Format("{0} {1} {2}", "MOVE", (int)_position.x, (int)_position.y);
        }
        public static string GetTrainCommand(List<int> _intList)
        {
            string command = "TRAIN";

            foreach (int _int in _intList)
            {
                command += string.Format(" {0}", _int);
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

        internal static BuildingType GetBuildingTypeFromString(int _structureType)
        {
            switch (_structureType)
            {
                case -1: return BuildingType.NONE;
                case 0: return BuildingType.MINE;
                case 1: return BuildingType.TOWER;
                case 2: return BuildingType.BARRACKS_KNIGHT;
                default: return BuildingType.NONE;
            }
        }

        internal static string GetNameFromType(BuildingType _type)
        {
            switch (_type)
            {
                case BuildingType.NONE:
                    return "NONE";
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