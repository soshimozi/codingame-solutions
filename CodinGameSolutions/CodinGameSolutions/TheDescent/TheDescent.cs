using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodinGameSolutions
{
    namespace TheDescent
    {
        /**
         * The while loop represents the game.
         * Each iteration represents a turn of the game
         * where you are given inputs (the heights of the mountains)
         * and where you have to print an output (the index of the mountain to fire on)
         * The inputs you are given are automatically updated according to your last actions.
         **/
        class Player
        {
            static void Mmain(string[] args)
            {
                // game loop
                while (true)
                {
                    // Map of index of mountain and height of mountain
                    Dictionary<int, int> heightMap = new Dictionary<int, int>();

                    //List of mountain heights
                    List<int> heightList = new List<int>();

                    //Iterate through each mountain and fill in the map / list with current heights
                    for (int i = 0; i < 8; i++)
                    {
                        int mountainH = int.Parse(Console.ReadLine()); // represents the height of one mountain.
                        heightMap[i] = mountainH;
                        heightList.Add(mountainH);
                    }

                    //Reorder the heightList map from Highest to Lowest height
                    heightList = heightList.OrderByDescending(x => x).ToList();

                    int highestMountainValue = heightList[0];

                    //Now that we have the highest mountain, find the index of the mountain of that height using the map
                    int indexOfHighestMountain =0;
                    foreach (KeyValuePair<int, int> _kvp in heightMap)
                    {
                        if (_kvp.Value == highestMountainValue) 
                        {
                            indexOfHighestMountain = _kvp.Key;
                        }
                    }

                    //Output the index of the mountain to fire on
                    Console.WriteLine(indexOfHighestMountain);
                }
            }
        }
    }
}
