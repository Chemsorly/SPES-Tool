using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisioAddin2010
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        public About(String pVersion)
        {
            this.Text += $" Version:{pVersion}";
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateIssueButton_Click(object sender, EventArgs e)
        {
            using (var form = new GitIssueWindow())
                form.ShowDialog();
        }
    }
}
