using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using SPES_Modelverifier_Base;
using NetOffice.VisioApi;
using ITU_Scenario;
using SPES_Funktionsnetz;

namespace VisioAddin2013
{
    public partial class MainRibbon 
    {
        List<ModelNetwork> modelverifiers = new List<ModelNetwork>();

        ModelNetwork previousModelverifier = null;
        ModelNetwork activeModelverifier => modelverifiers.FirstOrDefault(t => t.ToString() == this.ModelTargetDropDown.SelectedItem?.Label);

        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            //get current application
            var application = Application.GetActiveInstance();

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
                obj.OnErrorReceivedEvent += delegate (Exception pEx) { System.Windows.Forms.MessageBox.Show(pEx.Message); };
                obj.OnLogMessageReceivedEvent += delegate (String pMessage) { System.Windows.Forms.MessageBox.Show(pMessage); };
            }

            //call selection changed for init shape load (only if document is loaded)
            if(application.ActiveDocument != null)
                ModelTargetDropDown_SelectionChanged(null, null);
        }

        private void Verify_Click(object sender, RibbonControlEventArgs e)
        {
            if (activeModelverifier != null)
            {
                try
                {
                    this.activeModelverifier.Verify();
                    System.Windows.Forms.MessageBox.Show("Verification successful!", "Success!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                catch (ValidationFailedException ex)
                {
                    System.Windows.Forms.MessageBox.Show(String.Format("Sheet: {0} \nShape: {1}\nMessage: {2}", ex.ExceptionObject.visiopage, ex.ExceptionObject.uniquename, ex.Message),
                        "Validation failed!",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
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
            //throw new NotImplementedException();
        }

        private void ImportButton_Click(object sender, RibbonControlEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ModelTargetDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            if (previousModelverifier != null)
                previousModelverifier.UnloadShapes();

            activeModelverifier.LoadShapes();
            previousModelverifier = activeModelverifier;
        }

        // not needed atm, maybe later for dynamic shape loading
        //private void ModelTarget_SelectionChanged(object sender, RibbonControlEventArgs e)
        //{
        //    RibbonDropDownItem sel = (e.Control as RibbonDropDown).SelectedItem;
        //    if (modelverifiers.Exists(t => t.ToString() == sel.Label))
        //        activeModelverifier = modelverifiers.First(t => t.ToString() == sel.Label);
        //}
    }
}
