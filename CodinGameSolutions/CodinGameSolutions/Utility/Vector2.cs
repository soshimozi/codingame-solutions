using System;

namespace Utility
{
    // Vector2 struct to hold x y position and Cardinal equivalent
    public struct Vector2
    {
        const double DegToRadians = Math.PI / 180;
        const double RadToDegrees = 360.0 / (2 * Math.PI);
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

        public double this[double _key]
        {
            get
            {
                return _key == 0 ? x : y;
            }
            set
            {
                if (_key == 0) { x = value; }
                else y = value;
            }
        }

        public double x;
        public double y;

        //returns vector from initial to final
        public static Vector2 GetDistance(Vector2 _initial, Vector2 _final)
        {
            return _final - _initial;
        }

        public static float DotProduct(Vector2 _base, Vector2 _other)
        {
            _base.Normalize();
            _other.Normalize();
            return (float)((_base.x * _other.x) + (_base.y * _other.y));
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
            return (float)Math.Sqrt(Math.Abs(x * x + y * y));
        }

        public float GetLength()
        {
            return Math.Abs((float)(x * x + y * y));
        }

        public override string ToString()
        {
            return x.ToString() + ", " + y.ToString();
        }

        internal static Vector2 GetPerpendicularVector(Vector2 _vector, bool _clockwise = true)
        {
            Vector2 returnVector = new Vector2(_vector.y, _vector.x);
            if (_clockwise)
            {
                returnVector.y *= -1;
            }
            else returnVector.x *= -1;
            return returnVector;
        }

        public static Vector2 Rotate(Vector2 _vectorToRotate, double _degrees)
        {
            return RotateUsingRadians(_vectorToRotate, _degrees * DegToRadians);
        }

        private static Vector2 RotateUsingRadians(Vector2 _v, double _radians)
        {
            double c = Math.Cos(_radians);
            double s = Math.Sin(_radians);
            return new Vector2(c*_v.x - s*_v.y, s* _v.x + c * _v.y);
        }


        public static double GetAngleBetween(Vector2 _v1, Vector2 _v2, bool _returnInDegrees)
        {
            double top = 0;
            for (int i = 0; i < 2; i++)
            {
                top += _v1[i] * _v2[i];
            }

            double _v1Squared = 0;
            double _v2Squared = 0; 
            for (int i = 0; i < 2; i++)
            {
                _v1Squared += _v1[i] * _v1[i];
                _v2Squared += _v2[i] * _v2[i];
            }

            double bottom = 0;
            bottom = Math.Sqrt(_v1Squared * _v2Squared);

            double returnValue = Math.Acos(top / bottom);
            if (_returnInDegrees) returnValue *= RadToDegrees;
            return returnValue;
        }
    }
}