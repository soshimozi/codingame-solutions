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

        // Vector2 struct to hold x y position and Cardinal equivalent
        struct Vecthor
        {
            public Vecthor(double _x, double _y, string _cardinalDirection = "UNSET")
            {
                x = _x;
                y = _y;
                m_cardinalDirection = _cardinalDirection;
            }

            //Overloading Operator- example
            public static Vecthor operator- (Vecthor lhs, Vecthor rhs)
            {
                return new Vecthor(lhs.x - rhs.x, lhs.y - rhs.y);
            }

            //Overloading Operator+ example
            public static Vecthor operator +(Vecthor lhs, Vecthor rhs)
            {
                return new Vecthor(lhs.x + rhs.x, lhs.y + rhs.y);
            }

            public string m_cardinalDirection;
            public double x;
            public double y;

            //returns vector from initial to final
            public static Vecthor GetDistance(Vecthor _initial, Vecthor _final)
            {
                return _final - _initial;    
            }

            //Normalizes this vector
            public void Normalize()
            {
                Vecthor vectorNormalized = Normalized();
                x = vectorNormalized.x;
                y = vectorNormalized.y;
            }
           
            // Returns a new vector equal to this vector normalized
            public Vecthor Normalized()
            {
                // a^2 + b^2 = c^2
                double length = Math.Sqrt(x * x + y * y);

                Vecthor returnVector = this;

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
                //Create all normalized vector for each cardinal direction
                List<Vecthor> directionList = new List<Vecthor>()
                {
                    { new Vecthor(0,1, "S") },
                    { new Vecthor(1,1, "SE") },
                    { new Vecthor(-1,1, "SW") },
                    { new Vecthor(0,-1, "N") },
                    { new Vecthor(1,-1, "NE") },
                    { new Vecthor(-1,-1, "NW") },
                    { new Vecthor(1,0, "E") },
                    { new Vecthor(-1,0, "W") },
                };

                string[] inputs = Console.ReadLine().Split(' ');
                int lightX = int.Parse(inputs[0]); // the X position of the light of power
                int lightY = int.Parse(inputs[1]); // the Y position of the light of power
                int initialTX = int.Parse(inputs[2]); // Thor's starting X position
                int initialTY = int.Parse(inputs[3]); // Thor's starting Y position

                Vecthor thorPosition = new Vecthor(initialTX, initialTY);
                Vecthor lightPosition = new Vecthor(lightX, lightY);

                // game loop
                while (true)
                {
                    int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.

                    // Write an action using Console.WriteLine()
                    // To debug: Console.Error.WriteLine("Debug messages...");

                    //Get Direction to Thor (normalized Distance vector) and Round for floating point imprecision
                    Vecthor normalizedDistance = Vecthor.GetDistance(thorPosition, lightPosition).Normalized();
                    normalizedDistance.x = Math.Round(normalizedDistance.x);
                    normalizedDistance.y = Math.Round(normalizedDistance.y);

                    //Console.Error.WriteLine("Norm Distance: " + normalizedDistance.ToString());
                    //directionList.ForEach(x => Console.Error.WriteLine(x.ToString()));

                    //Find the vector that corresponds to the normalized direction we want to move Thor in
                    Vecthor finalDirection = directionList.Find( x => x.x == normalizedDistance.x && x.y == normalizedDistance.y);
                     
                    // A single line providing the move to be made: N NE E SE S SW W or NW
                    //Output the cardinal direction set in that vector
                    Console.WriteLine(finalDirection.m_cardinalDirection);

                    //move Thor in the chosen direction
                    thorPosition = thorPosition + finalDirection;
                }
            }
        }
    }
}
