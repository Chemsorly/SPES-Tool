using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using SPES_Modelverifier_Base.Items;
using SPES_Modelverifier_Base.Utility;
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

        private String documentReferencerFile => System.IO.Path.Combine(new FileInfo(application.ActiveDocument.Path).Directory.FullName, "spesconfig.xml");

        private bool IsSPESproject => application.ActiveDocument.Path != "" && Directory.GetFiles(new FileInfo(application.ActiveDocument.Path).Directory.FullName).Any(t => t.Contains("spesconfig.xml"));
        

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
            var defaultitem = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
            defaultitem.Label = "none";
            this.ModelTargetDropDown.Items.Add(defaultitem);
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
            application.EnterScopeEvent += delegate(IVApplication app, int id, string description)
                {
                    //4490 = hyperlink event
                    if(id == 4490)
                        this.initialized = false;
                };
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
                    //special case: StrukturellerKontext
                    if (activeModelverifier.GetType() == typeof(StrukturellerKontextNetwork))
                    {
                        this.spesapp.EntitytoPage();
                    }
                    //special case: FunktionellerKontext
                    else if (activeModelverifier.GetType() == typeof(FunktionellerKontextNetwork))
                    {
                        this.spesapp.FunctiontoPage();
                    }
                    else
                    {
                        activeModelverifier.GenerateSubmodels();
                    }
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
            activeModelverifier?.LoadShapes();
            previousModelverifier = activeModelverifier;

            //update ui
            if (activeModelverifier != null)
            {
                this.VerifyButton.Enabled = this.activeModelverifier.CanVerify;
                this.ImportButton.Enabled = this.activeModelverifier.CanVerify;
                this.ExportButton.Enabled = this.activeModelverifier.CanVerify;
                this.CreateNewSPESProject.Visible = false;
                this.GenerateSubmodelsButton.Visible = Reflection.GetAllModelreferenceTypesInModule(activeModelverifier.GetType()).Any();

                //special cases:
                if (activeModelverifier.GetType() == typeof(LogicalViewpointNetwork))
                {
                    this.CreateNewEngineeringPath.Visible = true;
                    this.CompleteInterfaceAutomata.Visible = false;
                }
                else if (activeModelverifier.GetType() == typeof(FunktionsnetzNetwork) || activeModelverifier.GetType() == typeof(TechnicalViewpointNetwork))
                {
                    this.CreateNewEngineeringPath.Visible = false;
                    this.CompleteInterfaceAutomata.Visible = true;
                }
                else
                {
                    this.CompleteInterfaceAutomata.Visible = false;
                    this.CreateNewEngineeringPath.Visible = false;
                }
            }
            else
            {
                //none selected
                this.VerifyButton.Enabled = false;
                this.ImportButton.Enabled = false;
                this.ExportButton.Enabled = false;
                this.CreateNewSPESProject.Visible = true;
                this.GenerateSubmodelsButton.Visible = false;
            }
        }

        private void Application_DocumentLoadedOrCreated(IVDocument pDoc)
        {
            if (!initialized)
            {
                //set ribbon behaviour
                if (IsSPESproject)
                {
                    //set document referencer
                    documentReferencer = new SPES_DocumentReferencer();
                    documentReferencer.LoadConfigFromFile(documentReferencerFile);

                    //set SPES specifics
                    this.ModelTargetDropDown.Enabled = false;
                    this.CreateNewSPESProject.Visible = false;

                    //load module based on definition
                    var type = documentReferencer.GetTypeFromFile(application.ActiveDocument.Name);
                    if (type != null)
                    {
                        ModelTargetDropDown.SelectedItem = ModelTargetDropDown.Items.Where(t => t.Label == type.ToString()).First();
                    }
                    else
                    {
                        ModelTargetDropDown.SelectedItem = ModelTargetDropDown.Items.Where(t => t.Label == "none").First();
                    }
                }
                else
                {
                    //set normal behaviour
                    this.ModelTargetDropDown.Enabled = true;
                    this.CreateNewSPESProject.Visible = true;
                }

                ModelTargetDropDown_SelectionChanged(null, null);
                initialized = true;
            }
        }

        private void AboutButton_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version v = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    About about = new About($"{v.Major}.{v.Minor}.{v.Revision}");
                    about.ShowDialog();
                }
                else
                {
                    About about = new About();
                    about.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                documentReferencer = new SPES_DocumentReferencer();

                string path = folder.SelectedPath;
                this.application.ActiveDocument.SaveAs(System.IO.Path.Combine(path, systemname + "_Overview.vsdx"));
                this.spesapp.CreateRectangle(systemname);
                this.spesapp.CreateSystem(documentReferencer);
                this.spesapp.SetHyperlink();

                //create config file
                documentReferencer.SaveConfigToFile(documentReferencerFile);

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
                this.spesapp.CreateSubsystems(documentReferencer);
                documentReferencer.SaveConfigToFile(documentReferencerFile);

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
        #endregion


    }
}
