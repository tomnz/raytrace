using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Raytrace
{
    class HDRColor
    {
        private double _r;
        private double _g;
        private double _b;
        private double _a;

        public double R
        {
            get { return _r; }
            set { _r = value; }
        }
        public double G
        {
            get { return _g; }
            set { _g = value; }
        }
        public double B
        {
            get { return _b; }
            set { _b = value; }
        }
        public double A
        {
            get { return _a; }
            set { _a = value; }
        }

        public double SafeR
        {
            get { return Clamp(_r); }
        }
        public double SafeG
        {
            get { return Clamp(_g); }
        }
        public double SafeB
        {
            get { return Clamp(_b); }
        }
        public double SafeA
        {
            get { return Clamp(_a); }
        }

        private static double Clamp(double val)
        {
            return Math.Min(1, Math.Max(val, 0));
        }

        public HDRColor(double r, double g, double b):
            this(r, g, b, 1)
        {
        }

        public HDRColor(double r, double g, double b, double a)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }

        public Color ToColor()
        {
            return Color.FromArgb((int)(SafeA * 255), (int)(SafeR * 255), (int)(SafeG * 255), (int)(SafeB * 255));
        }

        public static HDRColor FromColor(Color c)
        {
            return new HDRColor(c.R, c.G, c.B, c.A);
        }

        public static HDRColor Add(HDRColor a, HDRColor b)
        {
            return new HDRColor
                (
                    a.R + b.R,
                    a.G + b.G,
                    a.B + b.B,
                    Clamp(a.A + b.A)
                );
        }

        public static HDRColor Multiply(HDRColor a, HDRColor b)
        {
            return new HDRColor
                (
                    a.R * b.R,
                    a.G * b.G,
                    a.B * b.B,
                    Clamp(a.A * b.A)
                );
        }

        public static HDRColor Multiply(HDRColor a, double f)
        {
            return new HDRColor
                (
                    a.R * f,
                    a.G * f,
                    a.B * f,
                    a.A
                );
        }

        public static HDRColor operator +(HDRColor a, HDRColor b)
        {
            return Add(a, b);
        }
        public static HDRColor operator *(HDRColor a, HDRColor b)
        {
            return Multiply(a, b);
        }
        public static HDRColor operator *(HDRColor a, double f)
        {
            return Multiply(a, f);
        }
    }
}
