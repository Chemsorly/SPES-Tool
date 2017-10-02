using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisioAddin2013
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        public About(String pVersion) : base()
        {
            this.Text += $" Version:{pVersion}";
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
