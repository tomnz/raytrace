using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class Collision
    {
        private Vector3 _source;
        private Vector3 _d;
        private double _t;
        private SceneObject _obj;

        public Vector3 Source
        {
            get { return _source; }
        }
        public Vector3 D
        {
            get { return _d; }
        }
        public double T
        {
            get { return _t; }
        }
        public SceneObject Object
        {
            get { return _obj; }
        }

        public Collision(Vector3 source, Vector3 d, double t, SceneObject obj)
        {
            _source = source;
            _d = d;
            _t = t;
            _obj = obj;
        }

        public Vector3 HitPoint
        {
            get
            {
	            return new Vector3(_source.X + _t*_d.X, _source.Y + _t*_d.Y, _source.Z + _t*_d.Z);
            }
        }
    }
}
