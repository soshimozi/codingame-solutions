using System;
using System.Collections.Generic;
using System.Linq;


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