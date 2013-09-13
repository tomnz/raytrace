using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raytrace
{
    class GridSample
    {
        // Four corners
        private SampleRay _bl, _br, _tl, _tr;

        // Super samples
        private GridSample _blS, _brS, _tlS, _trS;

        public enum Corner
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        };
    }
}
