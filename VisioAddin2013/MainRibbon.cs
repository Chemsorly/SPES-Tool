using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using SPES_Modelverifier_Base;
using NetOffice.VisioApi;
using ITU_Scenario;
using SPES_Funktionsnetz;
using System.Windows.Forms;
using NetOffice.VisioApi.Enums;
using SPES_App;
using SPES_FunktionellePerspektive;
using SPES_FunktionellerKontext;
using SPES_LogicalViewpoint;
using SPES_StrukturellePerspektive;
using SPES_StrukturellerKontext;
using SPES_SzenarioUseCases;
using SPES_TechnicalViewpoint;
using SPES_Verhaltensperspektive;
using SPES_Wissenskontext;
using SPES_Zielmodell;

namespace VisioAddin2013
{
    public partial class MainRibbon 
    {
        private List<ModelNetwork> modelverifiers = new List<ModelNetwork>();

        private ModelNetwork previousModelverifier = null;
        private ModelNetwork activeModelverifier => modelverifiers.FirstOrDefault(t => t.ToString() == this.ModelTargetDropDown.SelectedItem?.Label);
        private ResultForm activeResultForm { get; set; }
        private bool initialized = false;
        private NetOffice.VisioApi.Application application;
        private SPES_DocumentReferencer documentReferencer;

        private bool IsSPESproject => application.ActiveDocument.Path != "" && Directory.GetFiles(new FileInfo(application.ActiveDocument.Path).Directory.FullName).Contains("spesconfig.xml");
        

        private void MainRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            //get current application
            this.application = NetOffice.VisioApi.Application.GetActiveInstance();
            this.spesapp = new SpesActivities(this.application);

            //add modelverifiers
            modelverifiers.Add(new ScenarioNetwork(application));
            modelverifiers.Add(new FunktionsnetzNetwork(application));
            modelverifiers.Add(new ZielmodellNetwork(application));

            //new ones
            modelverifiers.Add(new WissenskontextNetwork(application));
            modelverifiers.Add(new StrukturellerKontextNetwork(application));
            modelverifiers.Add(new FunktionellerKontextNetwork(application));
            modelverifiers.Add(new SzenarioUseCasesNetwork(application));
            modelverifiers.Add(new StrukturellePerspektiveNetwork(application));
            modelverifiers.Add(new FunktionellePerspektiveNetwork(application));
            modelverifiers.Add(new VerhaltensperspektiveNetwork(application));
            modelverifiers.Add(new LogicalViewpointNetwork(application));
            modelverifiers.Add(new TechnicalViewpointNetwork(application));

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
                    var results = this.activeModelverifier.VerifyModels();
                    if(results.Count > 0)
                    {
                        //show results window
                        activeResultForm?.Dispose();

                        //foreach (var result in results)
                        //{
                        //    if (result.ExceptionObject != null && result.ExceptionObject.Visioshape != null)
                        //    {
                        //        var targetCell = result.ExceptionObject.Visioshape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowFill, (short)VisCellIndices.visFillForegnd);
                        //        targetCell.FormulaU = "RGB(255,0,0)";
                        //    }
                        //}

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
                //check before export. todo remove
                if (!this.activeModelverifier.CanExport())
                    throw new Exception("Verification failed.");

                SaveFileDialog dialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "xml",
                    Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
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

        /// <summary>
        /// creates empty sheets for unreferenced submodels
        /// </summary>
        private void GenerateSubmodelsButton_Click(object sender, RibbonControlEventArgs e)
        {
            if (activeModelverifier != null)
            {
                try
                {
                    activeModelverifier.GenerateSubmodels();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error: " + ex.Message,
                        "ERROR",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }

        private void ModelTargetDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            previousModelverifier?.UnloadShapes();
            activeModelverifier.LoadShapes();
            previousModelverifier = activeModelverifier;

            //update ui
            this.VerifyButton.Enabled = this.activeModelverifier.CanVerify;
            this.ImportButton.Enabled = this.activeModelverifier.CanVerify;
            this.ExportButton.Enabled = this.activeModelverifier.CanVerify;
        }

        private void Application_DocumentLoadedOrCreated(IVDocument pDoc)
        {
            if (!initialized)
            {
                ModelTargetDropDown_SelectionChanged(null, null);
                initialized = true;
            }

            //set ribbon behaviour
            if (IsSPESproject)
            {
                //set SPES specifics
                this.ModelTargetDropDown.Enabled = false;
                this.CreateNewEngineeringPath.Visible = true;
                this.DefineContextEntitiesBehaviour.Visible = true;
                this.DefineContextFunctionsBehaviour.Visible = true;
                this.CompleteInterfaceAutomata.Visible = true;
                this.CreateBMSCs.Visible = true;
                this.CreateNewSPESProject.Visible = false;
            }
            else
            {
                //set normal behaviour
                this.ModelTargetDropDown.Enabled = true;
                this.CreateNewEngineeringPath.Visible = false;
                this.DefineContextEntitiesBehaviour.Visible = false;
                this.DefineContextFunctionsBehaviour.Visible = false;
                this.CompleteInterfaceAutomata.Visible = false;
                this.CreateBMSCs.Visible = false;
                this.CreateNewSPESProject.Visible = true;
            }
        }

        //TODO: aktuell für debug, später raus // durch gitlab api -> new issue ersetzen
        private void AboutButton_Click(object sender, RibbonControlEventArgs e)
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                Version v = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                About about = new About($"{v.Major}.{v.Minor}.{v.Revision}.{v.Build}");
                about.ShowDialog();
            }
            else
            {
                About about = new About();
                about.ShowDialog();
            }
        }

        //kevin teil beginnt hier:
        #region kevin 

        private SpesActivities spesapp;

        private void CreateNewSPESProject_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                // code causing TargetInvocationException

                //Ruft Dialogbox auf, in der der Benutzer den Namen das Systems angibt
                string systemname = Microsoft.VisualBasic.Interaction.InputBox("Type in the name of the system", "Get System name", "System_Name");
                //Zum Starten der Modellierung werden die folgenden Methoden aufgerufen.
                FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.ShowDialog();

                string path = folder.SelectedPath;
                this.application.ActiveDocument.SaveAs(System.IO.Path.Combine(path, systemname + "_Overview.vsdx"));
                this.spesapp.CreateRectangle(systemname);
                this.spesapp.CreateSystem();
                this.spesapp.SetHyperlink();

                //create config file
                File.Create(System.IO.Path.Combine(path, "spesconfig.xml"));

                //this._spesapp.deleteModels();
            }
            //Fange mögliche Fehler ab und informiere Benutzer, dass die Generierung unvollständig ist
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                {
                    System.Windows.Forms.MessageBox.Show("Not all elements could created through Modeling.");
                }
            }
        }

        private void CreateNewEngineeringPath_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {

                //this._application.ActiveDocument.Save();
                this.spesapp.CreateSubsystems();
                System.Windows.Forms.MessageBox.Show("Creation successfully!");
            }
            //Fange mögliche Fehler ab und informiere Benutzer, dass die Generierung unvollständig ist
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                {
                    System.Windows.Forms.MessageBox.Show("Not all elements could created through Modeling." + exc);
                }
            }
        }

        /// <summary>
        /// kommt raus: ist nix anderes als create submodels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefineContextEntitiesBehaviour_Click(object sender, RibbonControlEventArgs e)
        {
            //Button, welches für jede Context Entity auf aktivem Zeichenblatt ein neues Zeichenblatt  erstellt
            try
            {
                this.spesapp.EntitytoPage();
                System.Windows.Forms.MessageBox.Show("Creation successfully!");
            }
            //Fange mögliche Fehler ab und informiere Benutzer, dass die Generierung unvollständig ist
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                {
                    System.Windows.Forms.MessageBox.Show("Not all elements could created through Modeling.");
                }
            }
        }

        /// <summary>
        /// kommt raus: ist nix anderes als create submodels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefineContextFunctionsBehaviour_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                //Button, welches für jede Context Function auf aktivem Zeichenblatt ein neues Zeichenblatt  erstellt
                this.spesapp.FunctiontoPage();
                System.Windows.Forms.MessageBox.Show("Creation successfully!");
            }
            //Fange mögliche Fehler ab und informiere Benutzer, dass die Generierung unvollständig ist
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                {
                    System.Windows.Forms.MessageBox.Show("Not all elements could created through Modeling.");
                }
            }
        }

        private void CreateBMSCs_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                this.spesapp.CreateSheetsforMscReferences();
                System.Windows.Forms.MessageBox.Show("Creation of References successfully done!.");

            }
            //Fange mögliche Fehler ab und informiere Benutzer, dass die Generierung unvollständig ist
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                {
                    System.Windows.Forms.MessageBox.Show("Not all elements could created through Modeling.");
                }
            }
        }

        private void CompleteInterfaceAutomata_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                this.spesapp.CreateInandOutput();
                System.Windows.Forms.MessageBox.Show("Creation of In- and Output finished.");

            }
            //Fange mögliche Fehler ab und informiere Benutzer, dass die Generierung unvollständig ist
            catch (Exception exc)
            {
                if (exc.InnerException != null)
                {
                    System.Windows.Forms.MessageBox.Show("Not all elements could created through Modeling.");
                }
            }
        }
        //neue symbole: new engineering path = appbar.tournament.bracket.up
        // create submodels = appbar.section.expand.all
        // create new project = appbar.folder + photoshop plus
        // complete interfacee automata = appbar.flag.wavy
        #endregion


    }
}
