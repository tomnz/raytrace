using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class Plane : SceneObject
    {
        Vector3 _n;
        double _a;

        public Plane(Vector3 n, double a):
            base()
        {
            _n = n;
            _a = a;
        }

        public Plane(Vector3 n, double a, Vector3 scaling, Vector3 translation, HDRColor ambient, HDRColor diffuse, HDRColor specular, double shininess, double reflectivity):
            base(scaling, translation, ambient, diffuse, specular, shininess, reflectivity)
        {
            _n = n;
            _a = a;
        }

        public override double Intersect(Vector3 source, Vector3 d)
        {
            if (Vector3.DotProduct(d, _n) != 0)
            {
                double t = (_a - Vector3.DotProduct(source, _n)) / Vector3.DotProduct(d, _n);
                return t;
            }
            return -1;
        }

        public override Vector3 Normal(Vector3 point)
        {
            return _n;
        }
    }
}
