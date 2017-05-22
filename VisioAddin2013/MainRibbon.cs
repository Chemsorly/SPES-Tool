using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using SPES_Modelverifier_Base;
using NetOffice.VisioApi;
using ITU_Scenario;
using SPES_Funktionsnetz;
using System.Windows.Forms;

namespace VisioAddin2013
{
    public partial class MainRibbon 
    {
        private List<ModelNetwork> modelverifiers = new List<ModelNetwork>();

        private ModelNetwork previousModelverifier = null;
        private ModelNetwork activeModelverifier => modelverifiers.FirstOrDefault(t => t.ToString() == this.ModelTargetDropDown.SelectedItem?.Label);
        private ResultForm activeResultForm { get; set; }
        private bool initialized = false;

        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            //get current application
            var application = NetOffice.VisioApi.Application.GetActiveInstance();

            //add modelverifiers
            modelverifiers.Add(new ScenarioNetwork(application));
            modelverifiers.Add(new FunktionsnetzNetwork(application));

            //add modelverifiers to dropdown menu and subscribe to events
            foreach (var obj in modelverifiers)
            {
                //dropdown
                var item = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                item.Label = obj.ToString();
                this.ModelTargetDropDown.Items.Add(item);

                //sub to log messages etc.
                obj.OnErrorReceivedEvent += delegate (Exception pEx) {
                    //move to most inner exception
                    while (pEx.InnerException != null) pEx = pEx.InnerException;
                    System.Windows.Forms.MessageBox.Show(pEx.Message);
                };
                //obj.OnLogMessageReceivedEvent += delegate (String pMessage) { System.Windows.Forms.MessageBox.Show(pMessage); };
            }

            //init stencils for modelverifiers
            modelverifiers.ForEach(t => t.CheckStencils());

            //call selection changed for init shape load (only if document is loaded)
            if (application.ActiveDocument != null)
                ModelTargetDropDown_SelectionChanged(null, null);

            //subscribe to application events
            application.DocumentCreatedEvent += Application_DocumentLoadedOrCreated;
            application.DocumentOpenedEvent += Application_DocumentLoadedOrCreated;
        }

        private void Verify_Click(object sender, RibbonControlEventArgs e)
        {
            if (activeModelverifier != null)
            {
                try
                {
                    var results = this.activeModelverifier.Validate();
                    if(results.Count > 0)
                    {
                        //show results window
                        activeResultForm?.Dispose();

                        ResultForm window = new ResultForm(results);
                        activeResultForm = window;
                        window.Show();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Verification successful!", "Success!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Verification failed: " + ex.Message,
                        "ERROR",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        private void ExportButton_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "xml"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.activeModelverifier.Export(dialog.FileName);
                    MessageBox.Show("Export successful");
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Export failed: " + ex.Message,
                        "ERROR",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void ImportButton_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog {DefaultExt = "xml"};
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.activeModelverifier.Import(dialog.FileName);
                    MessageBox.Show("Import successful");
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Import failed: " + ex.Message,
                        "ERROR",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void ModelTargetDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            previousModelverifier?.UnloadShapes();
            activeModelverifier.LoadShapes();
            previousModelverifier = activeModelverifier;
        }

        private void Application_DocumentLoadedOrCreated(IVDocument pDoc)
        {
            if (!initialized)
            {
                ModelTargetDropDown_SelectionChanged(null, null);
                initialized = true;
            }
        }

        //TODO: aktuell für debug, später raus
        private void AboutButton_Click(object sender, RibbonControlEventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                Version v = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                System.Windows.Forms.MessageBox.Show("Version: " + String.Format(@" v{0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build, v.Revision));
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("not deployed");
            }
        }
    }
}
