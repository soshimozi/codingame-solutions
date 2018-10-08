using System;
using System.Linq;
using System.Collections.Generic;
using Utility;

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