using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Collections;

namespace Raytrace
{
    class RayTracer : IDisposable
    {
        private const double EPSILON = 0.001;

        private int _width;
        private int _height;

        private const double W = 0.5;
        private const double H = 0.5;
        private const double N = 1;

        private const int AA = 3;

        private const int MAX_REFLECTIONS = 3;
        private const int WORKERS = 5;

        private const int STOCHASTIC_STRENGTH = 1;

        private IList<SceneObject> _scene;
        public IList<SceneObject> Scene
        {
            get { return _scene; }
        }

        private IList<Light> _lights;
        public IList<Light> Lights
        {
            get { return _lights; }
        }

        private HDRColor _backgroundColor;
        public HDRColor BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        private Vector3 _cameraPos;
        public Vector3 CameraPosition
        {
            get { return _cameraPos; }
            set { _cameraPos = value; }
        }

        private Vector3 _u, _v, _n;
        public Vector3 u
        {
            get { return _u; }
            set { _u = value; }
        }
        public Vector3 v
        {
            get { return _v; }
            set { _v = value; }
        }
        public Vector3 n
        {
            get { return _n; }
            set { _n = value; }
        }

        private bool _rendering = false;
        private Thread _renderThread;
        private IList<Thread> _renderPool;
        private double _progress = 0;

        private HDRColor[,] _renderGrid;

        public bool Rendering
        {
            get { return _rendering; }
        }

        public double Progress
        {
            get { return _progress; }
        }

        public RayTracer(int width, int height)
        {
            _scene = new List<SceneObject>();
            _lights = new List<Light>();
            _backgroundColor = HDRColor.FromColor(Color.Black);

            _width = width;
            _height = height;

            _renderPool = new List<Thread>();
        }

        public void Render()
        {
            if (_rendering)
                return;
            
            _rendering = true;
            _progress = 0;

            _renderThread = new Thread(new ThreadStart(RenderAsync));

            _renderThread.Start();
        }

        private void RenderAsync()
        {
            try
            {
                int totalSize = _width * _height * AA * AA;
                _renderGrid = new HDRColor[_width * AA, _height * AA];

                Random rnd = new Random();

                for (int r = 0; r < _height * AA; r++)
                {
                    for (int c = 0; c < _width * AA; c++)
                    {
                        double x, y;
                        x = 2.0 * (double)c * W / (double)(_width * AA) - W + rnd.NextDouble() * STOCHASTIC_STRENGTH / (AA * _width);
                        y = 2.0 * (double)r * H / (double)(_height * AA) - H + rnd.NextDouble() * STOCHASTIC_STRENGTH / (AA * _height);

                        Vector3 d = new Vector3();
                        d += -1.0 * N * n;
                        d += W * x * u;
                        d += H * y * v;

                        while (_renderPool.Count >= WORKERS)
                        {
                        }

                        Ray ray = new Ray();
                        ray.r = r;
                        ray.c = c;
                        ray.d = d;
                        ray.origin = _cameraPos;

                        Thread t = new Thread(RenderRay);
                        t.Start(ray);

                        _progress = ((double)r * (double)_width * AA + (double)c) / (double)totalSize;
                    }
                }

                Bitmap b = new Bitmap(_width, _height);

                for (int c = 0; c < _width; c++)
                {
                    for (int r = 0; r < _height; r++)
                    {
                        HDRColor pixel = new HDRColor(0, 0, 0);
                        for (int i = 0; i < AA; i++)
                        {
                            for (int j = 0; j < AA; j++)
                            {
                                pixel += _renderGrid[c * AA + i, r * AA + j];
                            }
                        }
                        pixel *= 1.0 / (double)(AA * AA);

                        b.SetPixel(c, (_height - 1) - r, pixel.ToColor());
                    }
                }

                b.Save("test.jpg");

                System.Diagnostics.Process.Start("test.jpg");

                _rendering = false;
            }
            catch (ThreadAbortException)
            {
                Dispose();
            }
        }

        private delegate void RayFinishedDelegate(Ray ray, HDRColor color);

        private void RayFinished(Ray ray, HDRColor color)
        {
            lock (_renderGrid)
            {
                _renderGrid[ray.c, ray.r] = color;
            }
        }

        private struct Ray
        {
            public int r;
            public int c;
            public Vector3 origin;
            public Vector3 d;
        };

        private void RenderRay(object ray)
        {
            try
            {
                Ray r = (Ray)ray;

                Collision col = Intersect(r.origin, r.d);

                HDRColor color = Shade(col, MAX_REFLECTIONS);

                RayFinished(r, color);
            }
            catch (ThreadAbortException)
            {
            }
        }

        private HDRColor Shade(Collision col, int reflectN)
        {
            if (col.T == -1)
                return _backgroundColor;

            // Phong
            HDRColor color = new HDRColor(0, 0, 0);

            Vector3 hitPoint = col.HitPoint;
            Vector3 m = col.Object.Normal(hitPoint);
            m.Normalise();

            for (int i = 0; i < _lights.Count; i++)
            {
                // Ambient reflection
                color = color + col.Object.Ambient * _lights[i].Ambient;

                // Shadow feeler
                Vector3 lightDir = _lights[i].Position - hitPoint;

                Collision feeler = Intersect(hitPoint, lightDir);

                if (feeler.T == -1 || feeler.T > 1)
                {
                    Vector3 s = _lights[i].Position - hitPoint;
                    Vector3 v = col.D;
                    Vector3 r = s - m * (2.0 * Vector3.DotProduct(m, s));

                    // Diffuse reflection
                    color = color + (col.Object.Diffuse * _lights[i].Diffuse) * (Math.Max(Vector3.DotProduct(s, m), 0) / (s.Length * m.Length));

                    // Specular reflection
                    color = color + (col.Object.Specular * _lights[i].Specular) * Math.Pow(Math.Max(Vector3.DotProduct(v, r), 0) / (v.Length * r.Length), col.Object.Shininess);
                }
            }

            if (reflectN > 0 && col.Object.Reflectivity >= 0.1)
            {
                Vector3 d2 = col.D - m * (2.0 * Vector3.DotProduct(m, col.D));
                Collision reflection = Intersect(hitPoint, d2);
                color = color + Shade(reflection, --reflectN) * col.Object.Reflectivity;
            }

            return color;
        }

        private Collision Intersect(Vector3 source, Vector3 d)
        {
            Collision col = new Collision(source, d, -1, null);

            for (int i = 0; i < _scene.Count; i++)
            {
                Vector3 source2 = source.Clone();
                source2 -= _scene[i].Translation;
                source2 /= _scene[i].Scaling;

                Vector3 d2 = d.Clone();
                d2 /= _scene[i].Scaling;

                double t = _scene[i].Intersect(source2, d2);

                if (t > EPSILON && (col.Object == null || t < col.T))
                {
                    col = new Collision(source, d, t, _scene[i]);
                }
            }

            return col;
        }

        public void CreateDefaultScene()
        {
            _cameraPos = new Vector3(0, 0, 4);
            _u = new Vector3(1, 0, 0);
            _v = new Vector3(0, 1, 0);
            _n = new Vector3(0, 0, 1);

            for (int i = -3; i <= 3; i++)
            {
                for (int j = -3; j <= 3; j++)
                {
                    _scene.Add
                        (
                            new Sphere
                                (
                                    new Vector3(0.4, 0.4, 0.4),
                                    new Vector3(i, j, 0),
                                    new HDRColor(0, 0, 0),
                                    new HDRColor(0, 0, 0),
                                    new HDRColor(0.5, 0.5, 0.5),
                                    50,
                                    1
                                )
                        );
                }
            }
            //_scene.Add
            //    (
            //        new Sphere
            //            (
            //                new Vector3(2, 2, 2),
            //                new Vector3(0, 5, 0),
            //                new HDRColor(0, 0, 0),
            //                new HDRColor(1, 1, 1),
            //                new HDRColor(0, 0, 0),
            //                200,
            //                1
            //            )
            //    );

            _scene.Add
                (
                    new Plane
                        (
                            new Vector3(0, 0, 1),
                            -1,
                            new Vector3(1, 1, 1),
                            new Vector3(0, 0, 0),
                            new HDRColor(0, 0.3, 0),
                            new HDRColor(0, 0.5, 0),
                            new HDRColor(1, 1, 1),
                            50,
                            0.1
                        )
                );

            _lights.Add
                (
                    new Light
                        (
                            new Vector3(2, 2, 5),
                            new HDRColor(0.5, 0.5, 0.5),
                            new HDRColor(0.5, 0.5, 0.5),
                            new HDRColor(0.5, 0.5, 0.5)
                        )
                );

            _lights.Add
                (
                    new Light
                        (
                            new Vector3(-2, -2, 5),
                            new HDRColor(0.5, 0.5, 0.5),
                            new HDRColor(0.5, 0.5, 0.5),
                            new HDRColor(0.5, 0.5, 0.5)
                        )
                );
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (_renderThread != null)
            {
                _renderThread.Abort();
                _renderThread.Join(2);
                _renderThread = null;

                while (_renderPool.Count > 0)
                {
                    _renderPool[0].Abort();
                    _renderPool[0].Join(2);
                    _renderPool.RemoveAt(0);
                }
            }
        }

        #endregion
    }
}
