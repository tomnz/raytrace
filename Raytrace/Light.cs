using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class Light
    {
        Vector3 _position;

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        
        HDRColor _ambient, _diffuse, _specular;

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


        public Light(Vector3 position, HDRColor ambient, HDRColor diffuse, HDRColor specular)
        {
            _position = position;
            _ambient = ambient;
            _diffuse = diffuse;
            _specular = specular;
        }
    }
}
