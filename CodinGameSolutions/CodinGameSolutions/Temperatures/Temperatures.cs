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
                int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
                string[] inputs = Console.ReadLine().Split(' ');

                List<int> temperatureList = new List<int>();
                for (int i = 0; i < n; i++)
                {
                    temperatureList.Add(0);
                    int t = int.Parse(inputs[i]); // a temperature expressed as an integer ranging from -273 to 5526
                    temperatureList.Add(t);
                }

                temperatureList = temperatureList.OrderBy(x => x).ToList();
                temperatureList = temperatureList.Distinct().ToList();

                temperatureList.ForEach(x => Console.Error.WriteLine(x));

                int indexOfZero = temperatureList.FindIndex(x => x == 0);
                Console.Error.WriteLine("Index Of Zero:" + indexOfZero);
                
                int finalTemperature = 0;

                if (temperatureList.Count != 0)
                {
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
