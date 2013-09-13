using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Raytrace
{
    abstract class SceneObject
    {
        // Members
        protected Vector3 _scaling, _translation;
        protected HDRColor _ambient, _diffuse, _specular;
        protected double _shininess, _reflectivity;

        // Properties
        public Vector3 Translation
        {
            get { return _translation; }
            set { _translation = value; }
        }

        public Vector3 Scaling
        {
            get { return _scaling; }
            set { _scaling = value; }
        }

        public HDRColor Specular
        {
            get { return _specular; }
            set { _specular = value; }
        }

        public HDRColor Diffuse
        {
            get { return _diffuse; }
            set { _diffuse = value; }
        }

        public HDRColor Ambient
        {
            get { return _ambient; }
            set { _ambient = value; }
        }

        public double Reflectivity
        {
            get { return _reflectivity; }
            set { _reflectivity = value; }
        }

        public double Shininess
        {
            get { return _shininess; }
            set { _shininess = value; }
        }

        public SceneObject():
            this(new Vector3(1, 1, 1), new Vector3())
        {
        }

        public SceneObject(Vector3 scaling, Vector3 translation):
            this(scaling, translation, HDRColor.FromColor(Color.Black), HDRColor.FromColor(Color.White), HDRColor.FromColor(Color.White))
        {
        }

        public SceneObject(Vector3 scaling, Vector3 translation, HDRColor ambient, HDRColor diffuse, HDRColor specular):
            this(scaling, translation, ambient, diffuse, specular, 0, 0)
        {
        }

        public SceneObject(Vector3 scaling, Vector3 translation, HDRColor ambient, HDRColor diffuse, HDRColor specular, double shininess, double reflectivity)
        {
            _scaling = scaling;
            _translation = translation;
            _ambient = ambient;
            _diffuse = diffuse;
            _specular = specular;
            _shininess = shininess;
            _reflectivity = reflectivity;
        }

        public virtual double Intersect(Vector3 source, Vector3 d)
        {
            return 0;
        }

        public virtual Vector3 Normal(Vector3 point)
        {
            return new Vector3();
        }
    }
}
