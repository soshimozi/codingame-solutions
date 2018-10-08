using System;

namespace Utility
{
    // Vector2 struct to hold x y position and Cardinal equivalent
    public struct Vector2
    {
        public static Vector2 zero;

        public Vector2(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        //Overloading Operator- example
        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        //Overloading Operator+ example
        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2 operator *(Vector2 lsh, float _length)
        {
            return new Vector2(lsh.x * _length, lsh.y * _length);
        }

        public double x;
        public double y;

        //returns vector from initial to final
        public static Vector2 GetDistance(Vector2 _initial, Vector2 _final)
        {
            return _final - _initial;
        }

        //Normalizes this vector
        public void Normalize()
        {
            Vector2 vectorNormalized = Normalized();
            x = vectorNormalized.x;
            y = vectorNormalized.y;
        }

        // Returns a new vector equal to this vector normalized
        public Vector2 Normalized()
        {
            // a^2 + b^2 = c^2
            double length = GetLengthSqr();

            Vector2 returnVector = this;

            returnVector.x = x / length;
            returnVector.y = y / length;

            return returnVector;
        }

        public float GetLengthSqr()
        {
            // a^2 + b^2 = c^2
            return (float)Math.Sqrt(x * x + y * y);
        }

        public float GetLength()
        {
            return (float)(x * x + y * y);
        }

        public override string ToString()
        {
            return x.ToString() + ", " + y.ToString();
        }
    }
}