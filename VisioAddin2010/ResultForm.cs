using SPES_Modelverifier_Base;
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
    public partial class ResultForm : Form
    {
        public List<ValidationFailedMessage> Results { get; }

        public ResultForm(List<ValidationFailedMessage> pResults)
        {
            Results = pResults;

            InitializeComponent();
            this.ResultsDataGridView.DataSource = Results;
        }
    }
}
