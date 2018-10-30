using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fortnite_Music
{

    public partial class ResolutionSetter : Form
    {
        public int resX;
        public int resY;
        public ResolutionSetter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Debug.WriteLine(numericUpDown1.Value.ToString());
            resX = (int)numericUpDown1.Value;
            resY = (int)numericUpDown2.Value;
            this.Close();
        }
    }
}
