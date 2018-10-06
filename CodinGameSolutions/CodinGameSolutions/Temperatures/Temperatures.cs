using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CodinGameSolutions
{
    namespace Temperatures
    {
        /**
         * Auto-generated code below aims at helping you parse
         * the standard input according to the problem statement.
         **/
        class Solution
        {
            static void Maain(string[] args)
            {
                // the number of temperatures to analyze
                int n = int.Parse(Console.ReadLine()); 
                string[] inputs = Console.ReadLine().Split(' ');

                //Add all temperature to list including. We add a 0 in case one doesn't exist
                List<int> temperatureList = new List<int>();
                temperatureList.Add(0);
                for (int i = 0; i < n; i++)
                {
                    int t = int.Parse(inputs[i]); // a temperature expressed as an integer ranging from -273 to 5526
                    temperatureList.Add(t);
                }

                //Order the list by Ascending and ensure there are no repeating numbers
                temperatureList = temperatureList.OrderBy(x => x).ToList();
                temperatureList = temperatureList.Distinct().ToList();

                //temperatureList.ForEach(x => Console.Error.WriteLine(x));

                int indexOfZero = temperatureList.FindIndex(x => x == 0);
                //Console.Error.WriteLine("Index Of Zero:" + indexOfZero);
                int finalTemperature = 0;

                //If there are no temperatures in the List return 0
                if (temperatureList.Count != 0)
                {
                    //If zero is the last index, we want the previous value
                    //If zero is not the last index, we want the next value as the game requires the positive integer if 2 are the same
                    if (indexOfZero >= temperatureList.Count-1)
                    {
                        finalTemperature = temperatureList[indexOfZero - 1];
                    }
                    else { finalTemperature = temperatureList[indexOfZero + 1]; }
                }
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");1
                Console.WriteLine(finalTemperature);
            }
        }
    }
}
