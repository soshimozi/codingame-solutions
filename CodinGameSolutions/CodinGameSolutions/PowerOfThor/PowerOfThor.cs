using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CodinGameSolutions
{
    namespace PowerOfThor
    {
        /**
   * Auto-generated code below aims at helping you parse
   * the standard input according to the problem statement.
   * ---
   * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
   **/

        struct Vector2
        {
            public Vector2(double _x, double _y, string _cardinalDirection = "UNSET")
            {
                x = _x;
                y = _y;
                m_cardinalDirection = _cardinalDirection;
            }

            public static Vector2 operator- (Vector2 lhs, Vector2 rhs)
            {
                return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
            }

            public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
            {
                return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
            }

            public string m_cardinalDirection;
            public double x;
            public double y;


            public static Vector2 GetDistance(Vector2 _initial, Vector2 _final)
            {
                return _final - _initial;    
            }

            public void Normalize()
            {
                Vector2 vectorNormalized = Normalized();
                x = vectorNormalized.x;
                y = vectorNormalized.y;
            }

            public Vector2 Normalized()
            {
                // a^2 + b^2 = c^2
                double length = Math.Sqrt(x * x + y * y);

                Vector2 returnVector = this;

                returnVector.x = x / length;
                returnVector.y = y / length;

                return returnVector;
            }

            public override string ToString()
            {
                return x.ToString() + ", " + y.ToString() + "      - CD: " + m_cardinalDirection;
            }
            
        }

        class Player
        {
            static void Mmain(string[] args)
            {
                List<Vector2> directionList = new List<Vector2>()
                {
                    { new Vector2(0,1, "S") },
                    { new Vector2(1,1, "SE") },
                    { new Vector2(-1,1, "SW") },
                    { new Vector2(0,-1, "N") },
                    { new Vector2(1,-1, "NE") },
                    { new Vector2(-1,-1, "NW") },
                    { new Vector2(1,0, "E") },
                    { new Vector2(-1,0, "W") },
                };

                string[] inputs = Console.ReadLine().Split(' ');
                int lightX = int.Parse(inputs[0]); // the X position of the light of power
                int lightY = int.Parse(inputs[1]); // the Y position of the light of power
                int initialTX = int.Parse(inputs[2]); // Thor's starting X position
                int initialTY = int.Parse(inputs[3]); // Thor's starting Y position

                Vector2 thorPosition = new Vector2(initialTX, initialTY);
                Vector2 lightPosition = new Vector2(lightX, lightY);

                // game loop
                while (true)
                {
                    int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.

                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");

                    Vector2 normalizedDistance = Vector2.GetDistance(thorPosition, lightPosition).Normalized();
                    normalizedDistance.x = Math.Round(normalizedDistance.x);
                    normalizedDistance.y = Math.Round(normalizedDistance.y);

                    //Console.Error.WriteLine("Norm Distance: " + normalizedDistance.ToString());
                    //directionList.ForEach(x => Console.Error.WriteLine(x.ToString()));

                    Vector2 finalDirection = directionList.Find( x => x.x == normalizedDistance.x && x.y == normalizedDistance.y);
                     
                    // A single line providing the move to be made: N NE E SE S SW W or NW
                    Console.WriteLine(finalDirection.m_cardinalDirection);
                    thorPosition = thorPosition + finalDirection;
                }
            }
        }
    }
}
