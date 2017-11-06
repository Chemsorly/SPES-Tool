using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPES_App;

namespace VisioAddin2010
{
    public partial class GitIssueWindow : Form
    {
        public GitIssueWindow()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            var api = new GitlabApiManager();
            api.Initialize("https://git.chemsorly.com", "dBxGTw9J_hifAxoMUhBx");

            //projectid hardcoded: 1
            api.CreateIssue(1, IssueTitleTextbox.Text, IssueBodyTextbox.Text, IssueAuthorTextbox.Text);
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
