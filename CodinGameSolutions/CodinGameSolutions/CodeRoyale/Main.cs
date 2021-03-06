using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Utility;
using CodeRoyale;

namespace CRPlayer
{
    class Player
    {

        static void Min(string[] args)
        {
            HeuristicData.Set(args);
            bool isFirstTurn = true;
            string[] inputs;
            int numSites = int.Parse(Console.ReadLine());

            for (int i = 0; i < numSites; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int siteId = int.Parse(inputs[0]);
                int x = int.Parse(inputs[1]);
                int y = int.Parse(inputs[2]);
                int radius = int.Parse(inputs[3]);

                BuildingLocation newLocation = new BuildingLocation(siteId, x, y, radius);
                GameData.m_buildingMap[siteId] = newLocation;
                GameData.m_buildingList.Add(newLocation);
            }

            // game loop
            while (true)
            {
                ++GameData.m_gameTurn;
                inputs = Console.ReadLine().Split(' ');
                int gold = int.Parse(inputs[0]);
                int touchedSite = int.Parse(inputs[1]); // -1 if none
                GameData.m_amountOfGold = gold;
                GameData.m_touchedSite = touchedSite;
                for (int i = 0; i < numSites; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int siteId = int.Parse(inputs[0]);
                    int goldRemaining = int.Parse(inputs[1]); // -1 if unknown
                    int maxMineSize = int.Parse(inputs[2]); // -1 if unknown
                    int structureType = int.Parse(inputs[3]); // -1 = No structure, 0 = Goldmine, 1 = Tower, 2 = Barracks
                    int owner = int.Parse(inputs[4]); // -1 = No structure, 0 = Friendly, 1 = Enemy
                    int param1 = int.Parse(inputs[5]);
                    int param2 = int.Parse(inputs[6]);
                   
                    //Refresh all location data
                    BuildingLocation currentLocation = GameData.m_buildingMap[siteId];
                    currentLocation.m_allianceType = owner == -1 ? AllianceType.NEUTRAL : owner == 0 ? AllianceType.FRIENDLY : AllianceType.HOSTILE;
                    currentLocation.m_remainingBuildTime = param1;
                    currentLocation.m_goldRemaining = goldRemaining;
                    currentLocation.m_maxYield = maxMineSize;
                    currentLocation.m_level = param1;

                    BuildingType updatedType = Helpers.GetBuildingTypeFromString(structureType);
                    bool currentLocationTypeIsBarracks = (int)currentLocation.m_buildingType >= 3;

                    if ((int)updatedType >= 3 && !currentLocationTypeIsBarracks)
                    {
                        currentLocation.m_buildingType = updatedType;
                    }
                    else if ((int)updatedType < 3) { currentLocation.m_buildingType = updatedType; }

                }

                List<Unit> unitList = new List<Unit>();
                int numUnits = int.Parse(Console.ReadLine());
                for (int i = 0; i < numUnits; i++)
                {
                    inputs = Console.ReadLine().Split(' ');
                    int x = int.Parse(inputs[0]);
                    int y = int.Parse(inputs[1]);
                    int owner = int.Parse(inputs[2]); // 0 = FRIENDLY , 1 = ENEMY
                    int unitType = int.Parse(inputs[3]); // -1 = QUEEN, 0 = KNIGHT, 1 = ARCHER
                    int health = int.Parse(inputs[4]);

                    //Create all units that exist this frame
                    AllianceType type = owner == 0 ? AllianceType.FRIENDLY : AllianceType.HOSTILE;

                    UnitType unitTypeEnum = unitType == -1 ? UnitType.QUEEN : unitType == 0 ? UnitType.KNIGHT : UnitType.ARCHER;

                    Unit newUnit = new Unit(x, y, type, unitTypeEnum, health);

                    if (unitTypeEnum == UnitType.QUEEN)
                    {
                        if (type == AllianceType.FRIENDLY)
                        {
                            GameData.m_friendlyQueen = newUnit;
                        }
                        else GameData.m_enemyQueen = newUnit;
                    }
                    unitList.Add(newUnit);
                }

                if (isFirstTurn)
                {
                    Vector2 distanceBetweenQueens = Vector2.GetDistance(GameData.m_friendlyQueen.m_position, GameData.m_enemyQueen.m_position);
                    float lengthBetweenQueens = distanceBetweenQueens.GetLengthSqr();

                    GameData.m_hubPosition = GameData.m_friendlyQueen.m_position + (distanceBetweenQueens.Normalized() * (lengthBetweenQueens /10));
                    StrategyDirector.CreateEvaluationSystem();
                }

                if (GameData.m_gameTurn < HeuristicData.TURNS_FOR_MIDGAME &&
                    Vector2.GetDistance(GameData.m_hubPosition, GameData.m_enemyQueen.m_position).GetLength() < Math.Pow(HeuristicData.QUEEN_IS_RUSHING_DISTANCE, 2))
                {
                    Vector2 distance = Vector2.GetDistance(GameData.m_hubPosition, GameData.m_centerMap);
                    //GameData.m_hubPosition = GameData.m_centerMap + distance;
                    //StrategyDirector.CreateEvaluationSystem();
                }
                GameData.m_unitList = unitList;
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");
                string queenInstructions = "";
                string trainingInstructions = "";
                StrategyManager.Execute(out queenInstructions, out trainingInstructions);

                // First line: A valid queen action
                // Second line: A set of training instructions
                Console.WriteLine(queenInstructions);
                Console.WriteLine(trainingInstructions);

                if (isFirstTurn)
                {
                    isFirstTurn = false;
                }

                StrategyManager.OutputTurnData();
            }
        }
    }
}


