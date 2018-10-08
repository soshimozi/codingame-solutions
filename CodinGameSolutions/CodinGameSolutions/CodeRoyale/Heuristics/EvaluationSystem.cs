using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CodeRoyale
{
    public class EvaluationSystem
    {
        public EvaluationSystem()
        {
            m_buildingHeursticList.Add(new DistanceToHubPositionHeuristic(2000));
            m_buildingHeursticList.Add(new DistanceToEnemyQueenHeuristic(500));
            m_buildingHeursticList.Add(new IsEmptyHeuristic(2000));
        }

        List<BuildingHeuristic> m_buildingHeursticList = new List<BuildingHeuristic>();


        public void EvaluateBuildingLocation(BuildingLocation _location)
        {
            float totalRating = 0;
            foreach (BuildingHeuristic _heuristic in m_buildingHeursticList)
            {
                totalRating += _heuristic.GetRating(_location);
            }
            _location.m_rating = (int)totalRating;
        }

        public BuildingLocation GetBestBuildingLocation()
        {
            GameData.m_buildingList.ForEach(x => EvaluateBuildingLocation(x));

            GameData.m_buildingList = GameData.m_buildingList.OrderByDescending(x => x.m_rating).ToList();
            //GameData.m_buildingList.ForEach(x => Console.Error.WriteLine(x.m_siteId + "  " + x.m_rating));
            return GameData.m_buildingList[0];
        }

        // If far from hub,we probably want to build a tower
        // Do we need barracks ? 
        // Build mine
        public void EvaluateBestTypeToBuild(BuildingLocation _location)
        {
            Vector2 hubPosition = GameData.m_hubPosition;
            float absDistance = Math.Abs(Vector2.GetDistance(_location.m_position, hubPosition).GetLength());

            if (absDistance > 400 * 400 && !BarracksRequired())
            {
                _location.m_desiredBuildingType = BuildingType.TOWER;
            }
            else if (BarracksRequired())
            {
                _location.m_desiredBuildingType = BuildingType.BARRACKS_KNIGHT;
            }
            else
            {
                _location.m_desiredBuildingType = BuildingType.MINE;
            }
        }


        public bool BarracksRequired()
        {
            int amountOfGold = GameData.m_amountOfGold;
            int maximumSpendingNextTurn = Helpers.GetAmountOfFriendlyBarracks() * GameData.KNIGHT_COST;

            return amountOfGold > maximumSpendingNextTurn * 2;
        }
    }
}