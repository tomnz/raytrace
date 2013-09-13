using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class SampleRay
    {
        private Vector3 _source, _d;
        private HDRColor _color;
        private RayTracer _rt;
        private GridSample _parent;
        private GridSample.Corner _corner;
    }
}
