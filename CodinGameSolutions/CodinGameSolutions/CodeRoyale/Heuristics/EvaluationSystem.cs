using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CodeRoyale
{
    public class EvaluationSystem
    {
        public static int m_idealAmountOfTowers =0;
        public static int m_idealAmountOfMines = 0;

        public EvaluationSystem()
        {
            m_buildingHeursticList = new List<Heuristic>() {
                new LocationDistanceToQueen(), new IsWithinHubTerritoryRating(),
                new BuildingUpgradeRating(), new IdealResourceIsDesiredResourceRating(),
                new EnemyNearbyRating(), new IsEnemyLocationWithinHubTerritoryRating(),
                new LocationUnoccupiedRating(), new DestructionImminentRating()};
        }

        List<Heuristic> m_buildingHeursticList = new List<Heuristic>();


        public void EvaluateBuildingLocation(BuildingLocation _location)
        {
            float totalRating = 0;
            foreach (Heuristic _heuristic in m_buildingHeursticList)
            {
                totalRating += _heuristic.GetRating(_location);
            }
            _location.m_buildingPriorityRating = (int)totalRating;
        }

        public BuildingLocation GetBestBuildingLocation()
        {
            GameData.m_buildingList.ForEach(x => EvaluateBuildingLocation(x));

            GameData.m_buildingList = GameData.m_buildingList.OrderByDescending(x => x.m_buildingPriorityRating).ToList();
            //GameData.m_buildingList.ForEach(x => Console.Error.WriteLine(x.m_siteId + "  " + x.m_rating));
            return GameData.m_buildingList[0];
        }


        public bool BarracksRequired()
        {
            int amountOfGold = GameData.m_amountOfGold;
            return Helpers.GetAmountOfFriendlyBarracks() ==0 && amountOfGold > GameData.KNIGHT_COST ;
        }
    }
}