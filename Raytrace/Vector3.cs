using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class Vector3
    {
        private double _x;
        private double _y;
        private double _z;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public double Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public Vector3() :
            this(0, 0, 0)
        {
        }

        public Vector3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public Vector3 Clone()
        {
            return new Vector3(_x, _y, _z);
        }

        public static double DotProduct(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public double Length
        {
            get { return Math.Sqrt(DotProduct(this, this)); }
        }

        public void Normalise()
        {
            double length = Length;
            _x /= length;
            _y /= length;
            _z /= length;
        }

        // Static modifiers
        public static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3
                (
                    a.X * b.X,
                    a.Y * b.Y,
                    a.Z * b.Z
                );
        }

        public static Vector3 Multiply(Vector3 a, double f)
        {
            return new Vector3
                (
                    a.X * f,
                    a.Y * f,
                    a.Z * f
                );
        }

        public static Vector3 Divide(Vector3 a, Vector3 b)
        {
            return new Vector3
                (
                    a.X / b.X,
                    a.Y / b.Y,
                    a.Z / b.Z
                );
        }

        public static Vector3 Divide(Vector3 a, double f)
        {
            return new Vector3
                (
                    a.X / f,
                    a.Y / f,
                    a.Z / f
                );
        }

        public static Vector3 Add(Vector3 a, Vector3 b)
        {
            return new Vector3
                (
                    a.X + b.X,
                    a.Y + b.Y,
                    a.Z + b.Z
                );
        }

        public static Vector3 Inverse(Vector3 v)
        {
            return Multiply(v, -1);
        }

        // Operators
        public static Vector3 operator -(Vector3 v)
        {
            return Inverse(v);
        }
        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return Multiply(a, b);
        }
        public static Vector3 operator *(Vector3 a, double f)
        {
            return Multiply(a, f);
        }
        public static Vector3 operator *(double f, Vector3 a)
        {
            return Multiply(a, f);
        }
        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return Divide(a, b);
        }
        public static Vector3 operator /(Vector3 a, double f)
        {
            return Divide(a, f);
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return Add(a, b);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return Add(a, Inverse(b));
        }
    }
}
