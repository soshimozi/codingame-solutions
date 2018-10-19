

using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeRoyale
{
    class StrategyManager
    {
        public static void Execute(out string _queenInstructions, out string _trainingInstructions)
        {
            _queenInstructions = "WAIT";
            _trainingInstructions = "TRAIN";

            _queenInstructions = StrategyDirector.GetBasicQueenStrategy();
            _trainingInstructions = StrategyDirector.BuildAllPossibleKnights();
        }

        internal static BuildingType GetDesiredBuildingType()
        {
            List<BuildingLocation> mineList;
            GameData.GetBuildingLocations(BuildingType.MINE, out mineList);
            mineList = mineList.Where(x => x.GetAlliance() == AllianceType.FRIENDLY).ToList();
            int amountOfMines = mineList.Count;

            List<BuildingLocation> towerList;
            GameData.GetBuildingLocations(BuildingType.TOWER, out towerList);
            towerList = towerList.Where(x => x.GetAlliance() == AllianceType.FRIENDLY).ToList();


            int amountOfTowers = towerList.Count;
            float towerToMineRatio =  amountOfMines == 0 ? HeuristicData.IDEAL_TOWER_TO_MINE_RATIO +1 : amountOfTowers / amountOfMines;
            return towerToMineRatio > HeuristicData.IDEAL_TOWER_TO_MINE_RATIO ? BuildingType.MINE : BuildingType.TOWER;
        }

        internal static void OutputTurnData()
        {
            //GameData.m_buildingList.ForEach(x => Console.Error.WriteLine("Id " + x.m_siteId + " Rating:" +x.m_buildingPriorityRating));
        }
    }
}