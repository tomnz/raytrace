using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Raytrace
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private RayTracer _rt = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (_rt != null && _rt.Rendering) return;

            button1.Enabled = false;

            _rt = new RayTracer(600, 600);
            _rt.CreateDefaultScene();

            _rt.Render();

            timerProgress.Start();
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            if (!_rt.Rendering)
            {
                progressBar1.Value = 0;
                progressBar1.Update();
                timerProgress.Stop();
                button1.Enabled = true;
            }
            else
            {
                progressBar1.Value = (int)(100 * _rt.Progress);
                progressBar1.Update();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_rt != null)
            {
                _rt.Dispose();
                _rt = null;
            }
        }
    }
}
