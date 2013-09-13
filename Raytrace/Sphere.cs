using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class Sphere : SceneObject
    {
        public Sphere(Vector3 scaling, Vector3 translation, HDRColor ambient, HDRColor diffuse, HDRColor specular, double shininess, double reflectivity):
            base(scaling, translation, ambient, diffuse, specular, shininess, reflectivity)
        {
        }

        public override double Intersect(Vector3 source, Vector3 d)
        {
            double A = Vector3.DotProduct(d, d);
            double B = 2.0 * Vector3.DotProduct(source, d);
            double C = Vector3.DotProduct(source, source) - 1.0;

            // Calculate B^2 - 4AC
            double discrim = B * B - 4.0 * A * C;
            if (discrim <= -0.01)
                return -1;

            double discrimSqrt = Math.Sqrt(discrim);
            double q;
            if (B > 0)
                q = (-B - discrimSqrt) / (2.0 * A);
            else
                q = (-B + discrimSqrt) / (2.0 * A);

            double t1 = q;
            double t2 = C / (A * q);

            return (t1 < t2) ? t1 : t2;
        }

        public override Vector3 Normal(Vector3 point)
        {
            Vector3 n = Vector3.Add(point, Vector3.Inverse(_translation));
            n /= _scaling;
            n.Normalise();
            return n;
        }
    }
}
