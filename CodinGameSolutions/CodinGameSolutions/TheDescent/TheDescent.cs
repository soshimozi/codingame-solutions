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
                    Dictionary<int, int> heightMap = new Dictionary<int, int>();
                    List<int> heightList = new List<int>();
                    for (int i = 0; i < 8; i++)
                    {
                        int mountainH = int.Parse(Console.ReadLine()); // represents the height of one mountain.
                        heightMap[i] = mountainH;
                        heightList.Add(mountainH);
                    }
                    heightList = heightList.OrderByDescending(x => x).ToList();

                    int highestMountainValue = heightList[0];

                    int indexOfHighestMountain =0;
                    foreach (KeyValuePair<int, int> _kvp in heightMap)
                    {
                        if (_kvp.Value == highestMountainValue) 
                        {
                            indexOfHighestMountain = _kvp.Key;
                        }
                    }

                    Console.WriteLine(indexOfHighestMountain); // The index of the mountain to fire on.
                }
            }
        }
    }
}
