using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CodeRoyale
{
    public static class StrategyDirector 
    {
        public static EvaluationSystem m_evaluationSystem;

        public static void CreateEvaluationSystem()
        {
            m_evaluationSystem = new EvaluationSystem();
            SetAllIdealBuildTypesForLocations();
        }

        private static void SetAllIdealBuildTypesForLocations()
        {
            List<BuildingLocation> locationList = GameData.m_buildingList;

            foreach (BuildingLocation _location in locationList)
            {
                float distanceToHub = Math.Abs(Vector2.GetDistance(_location.m_position, GameData.m_hubPosition ).GetLength() - (float)Math.Pow(_location.m_radius,2));

                _location.m_idealBuildingType = distanceToHub > Math.Pow(HeuristicData.MINE_RANGE, 2) ? BuildingType.TOWER : BuildingType.MINE;
            }
        }

        //Moves to nearest empty location and builds a knight barracks
        public static string GetBasicQueenStrategy()
        {
            string queenInstructions = "WAIT";
            BuildingLocation bestLocation = null;
            if (!GameData.m_upgradingMine)
            {
                bestLocation = m_evaluationSystem.GetBestBuildingLocation();
                queenInstructions = Helpers.GetQueenMoveCommand(Helpers.VerifyAndAvoidCollisions(bestLocation));
            }
            else if (GameData.m_touchedSite == -1) { GameData.m_upgradingMine = false; }
            //The queen is at a building location
            if (GameData.m_touchedSite != -1 && (GameData.m_upgradingMine || GameData.m_touchedSite == bestLocation.m_siteId))
            {
                BuildingLocation touchedLocation = GameData.GetBuilding(GameData.m_touchedSite);
                if (!GameData.m_upgradingMine && touchedLocation.m_buildingType != BuildingType.MINE && m_evaluationSystem.BarracksRequired())
                {
                    queenInstructions = Helpers.GetBuildCommand(touchedLocation, BuildingType.BARRACKS_KNIGHT);
                }
                else
                {
                    if (!GameData.m_upgradingMine && touchedLocation.m_idealBuildingType == BuildingType.MINE)
                    {
                        GameData.m_upgradingMine = true;
                        if (touchedLocation.m_goldRemaining <= 2) { touchedLocation.m_idealBuildingType = BuildingType.TOWER; }
                    }
                    else if (touchedLocation.m_level >= (touchedLocation.m_maxYield-1 < HeuristicData.MAX_MINE_UPGRADE_LEVEL-1 ? 
                        touchedLocation.m_maxYield-1: HeuristicData.MAX_MINE_UPGRADE_LEVEL - 1))
                    {
                        GameData.m_upgradingMine = false;
                    }
                    queenInstructions = Helpers.GetBuildCommand(touchedLocation);
                }
            }
            return queenInstructions;
        }

        public static string BuildAllPossibleKnights()
        {

            List<BuildingLocation> locationList;
            GameData.GetBuildingLocations(AllianceType.FRIENDLY, out locationList);

            locationList = locationList.Where(x => x.m_buildingType == BuildingType.BARRACKS_KNIGHT && x.m_remainingBuildTime >= -1).ToList();

            List<int> idList = new List<int>();
            locationList.ForEach(x => idList.Add(x.m_siteId));
            return Helpers.GetTrainCommand(idList);
        }
    }
}