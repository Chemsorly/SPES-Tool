using System;
using System.Collections.Generic;
using NetOffice.VisioApi;
using SPES_App.Utility;

namespace SPES_App
{
    public class SpesActivities
    {
        Application _application;

        List<IVMaster> ActiveMasters
        {
            get
            {
                List<IVMaster> masters = new List<IVMaster>();
                foreach (IVDocument doc in _application.Documents)
                    foreach (IVMaster master in doc.Masters)
                        masters.Add(master);
                return masters;
            }
        }

        public SpesActivities(Application a)
        {
            this._application = a;
        }

        public void CreateSystem()
        {
            IVShape systemshape = null;
            foreach (Shape s in this._application.ActivePage.Shapes)
            {
                systemshape = s;
            }
            IVPage p = this._application.ActiveDocument.Pages.Add();
            p.Name = systemshape.Text;
            CreateSystemElements(p);
            System.Windows.Forms.MessageBox.Show("Artifact Creation for Level 0 finished!");
        }

        public void CreateRectangle(string name)
        {
            this._application.ActivePage.Name = "System Overview";
            IVShape s= this._application.ActivePage.DrawRectangle(1, 1, 3, 1.5);
            s.Text = name; s.SetCenter((10 / 2.54), (27.5 / 2.54));
            s.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
        }

        private void CreateSystemElements( IVPage p)
        {
            //CellsSRC(1,11,4) gibt an, wo der Text positioniert werden soll
            //setCenter (double x, double y) positioniert das ausgewaehlte Shape an die gewuenschte Stelle, angegebene Werte
            //sind in Zoll , um von cm auf Zoll zu kommen, muss durch 2.54 dividiert werden.
            IVShape header, systemName, rvp, fvp, lvp, tvp, statusRvp, statusFvp, statusLvp, statusTvp;
            IVHyperlink rvphl, fvphl, lvphl, tvphl;
            header = p.DrawRectangle(1, 1, 8, 1.5); header.LineStyle = "none"; header.Text ="Artifacts of " + p.Name;
            header.SetCenter(4, (28/2.54));  header.CellsSRC(3, 0, 7).FormulaU = "24 pt"; header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

            systemName = p.DrawRectangle(1, 1, 8, 4); systemName.Text = p.Name; systemName.SetCenter(4, (23.2/2.54));
            systemName.CellsSRC(1, 11, 4).Formula = "0"; systemName.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

            rvp = p.DrawRectangle(1, 1, 2.5, 3);rvp.Text = "Requirements Engineering Viewpoint";
            rvp.SetCenter(4.2/2.54, (22.8 / 2.54)); rvp.CellsSRC(1, 11, 4).Formula = "0";
            rvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusRvp = p.DrawOval(1, 1, 1.16, 1.16); statusRvp.SetCenter(4.2 / 2.54, 21.3 / 2.54); statusRvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            fvp = p.DrawRectangle(1, 1, 2.5, 3); fvp.Text = "Functional Viewpoint"; fvp.SetCenter(8.2/2.54, (22.8 / 2.54));
            fvp.CellsSRC(1, 11, 4).Formula = "0"; fvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusFvp = p.DrawOval(1, 1, 1.16, 1.16); statusFvp.SetCenter(8.2 / 2.54, 21.3 / 2.54); statusFvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            lvp = p.DrawRectangle(1, 1, 2.5, 3); lvp.Text = "Logical Viewpoint"; lvp.SetCenter(12.2/2.54, (22.8 / 2.54));
            lvp.CellsSRC(1, 11, 4).Formula = "0"; lvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusLvp = p.DrawOval(1, 1, 1.16, 1.16); statusLvp.SetCenter(12.2 / 2.54, 21.3 / 2.54); statusLvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            tvp = p.DrawRectangle(1, 1, 2.5, 3); tvp.Text = "Technical Viewpoint"; tvp.SetCenter(16.2/2.54, (22.8 / 2.54));
            tvp.CellsSRC(1, 11, 4).Formula = "0"; tvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusTvp = p.DrawOval(1, 1, 1.16, 1.16); statusTvp.SetCenter(16.2 / 2.54, 21.3 / 2.54); statusTvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            System.Windows.Forms.MessageBox.Show(("Create Documents?"));
            CreateViewPointDocs(p.Name, this._application.ActiveDocument.Path);

            rvphl=rvp.AddHyperlink();
            rvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_RVP.vsdx"));
            fvphl = fvp.AddHyperlink();
            fvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_FVP.vsdx"));
            lvphl = lvp.AddHyperlink();
            lvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_LVP.vsdx"));
            tvphl = tvp.AddHyperlink();
            tvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_TVP.vsdx"));
 
        }

        public void CreateSheetsforMscReferences()
        {
            List<IVShape> references = new List<IVShape>();

            foreach (var s in this._application.ActivePage.Shapes)
            {
                if (s.Name.Contains("MSC Reference"))
                {
                    bool exists = false;
                    foreach (var sh in references)
                    {
                        if (sh.Text == s.Text)
                        {
                            exists = true; System.Windows.Forms.MessageBox.Show(sh.Text +
                                        " exists twice ore more as a MSC Reference.");
                        }
                    }
                    if (exists == false) { references.Add(s); }
                }
            }

            if (references.Count>=1)
            {
                this._application.Documents.OpenEx("basic Message Sequence Chart.vssx",4);
                foreach (var r in references)
                {
                    bool exist = false;
                    foreach(var p in this._application.ActiveDocument.Pages)
                    {
                        if (p.Name == r.Text)
                        {
                            exist = true;
                            System.Windows.Forms.MessageBox.Show(r.Text +
                                        " already exists.");
                        }
                    }
                    if (exist==false)
                    {
                        IVPage p = this._application.ActiveDocument.Pages.Add();
                        p.Name = r.Text;
                        IVHyperlink hl = r.Hyperlinks.Add();
                        hl.SubAddress = p.Name;
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No MSC Reference found.");
            };
        }

        private void CreateViewPointDocs(string systemname, string path)
        {

            using (Application app = new Application())
            {
                Application subapplic = this._application;
                IntPtr appkey = new IntPtr(0);
                IntPtr helpappkey = new IntPtr(0);
                Application applic = null;
                foreach (var window in OpenWindowGetter.GetOpenWindows())
                {
                    if (window.Value.Contains("Visio Professional"))
                    {                      
                        OpenWindowGetter.SetForegroundWindow(window.Key);
                        applic = NetOffice.VisioApi.Application.GetActiveInstance();                     
                        if (app == applic) { helpappkey = window.Key; }
                        else if (applic == this._application) { appkey = window.Key; };
                    };
                }
                OpenWindowGetter.SetForegroundWindow(helpappkey);
                CreateemptyModels(app, path, systemname);
                var doc = app.Documents.Add("");
                CreateRvp(systemname, doc);
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP.vsdx"));
                doc.Close();
                doc = app.Documents.Add("");
                CreateFvp(systemname, doc);
                app.Documents.OpenEx("Functional Design - Function Network.vssx", 4); app.Documents.OpenEx("Interface Automata.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_FVP.vsdx"));
                doc.Close();
                doc = app.Documents.Add("");
                CreateLvp(systemname, doc);
                app.Documents.OpenEx("Class Diagram.vssx", 4); 
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_LVP.vsdx"));
                doc.Close();
                doc = app.Documents.Add("");
                CreateTvp(systemname, doc);
                app.Documents.OpenEx("State Machine.vssx", 4);
                app.Documents.OpenEx("Interface Automata.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(path, systemname + "_TVP.vsdx"));
                doc.Close();
                OpenWindowGetter.SetForegroundWindow(appkey);
                app.Quit();
            }
            
        }
        private void CreateTvp(string system, IVDocument doc)
        {

            foreach (Page page in doc.Pages)
            {
                page.Name = "TVP_" + system;

                IVShape header;
                IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";

                header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Technical Viewpoint: " + system; header.SetCenter(4, (28 / 2.54));
                header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            }
        }

        private void CreateLvp(string system, IVDocument doc)
        {
            foreach (Page page in doc.Pages)
            {
                page.Name = "LVP_" + system;

                IVShape header;
                IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";

                header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Logical Viewpoint: " + system; header.SetCenter(4, (28 / 2.54));
                header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            }
        }

        private void CreateFvp(string system, IVDocument doc)
        {
            foreach (Page page in doc.Pages)
            {
                page.Name = "FVP_" + system;

                IVShape header;
                IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";


                header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Functional Viewpoint: " + system; header.SetCenter(4, (28 / 2.54));
                header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

            }
        }
        

        private void CreateRvp(string systemname, IVDocument doc)
        {
            //TODO neu erstellte Diagramme mit Shapesheet oeffnen
            IVPage page = new IVPage();
            IVShape header, kontext, neutral, bezogen;
            IVShape wissenskontext, funktKontext, struktKontext;
            IVShape goals, useMap, szenario;
            IVShape struktPerspektive, funktPerspektive, verhaltensPerspektive;
            IVHyperlink wkhl, skhl, fkhl, ghl, uchl, mschl, sphl, fphl, vphl;
            IVShape statusWk, statusfK, statussK, statusG, statusUcm, statusSz, statussP, statusfP, statusVp;
            foreach (Page p in doc.Pages)
            {
                page = p;
            }

            page.Name = "RVP_" + systemname;
            IVShape boundary = page.DrawRectangle(0, 12, 9, 1); boundary.CellsSRC(1, 3, 1).FormulaU = "THEMEGUARD(RGB(0,0,0))";
            header = page.DrawRectangle(1, 1, 8, 1.5); header.Text = "Requirements Viewpoint: " + systemname; header.SetCenter(4, (28 / 2.54));
            header.CellsSRC(1, 11, 4).Formula = "0"; header.LineStyle = "none"; header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
            header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            //Kontextmodelle
            kontext = page.DrawRectangle(1, 1, 3, 1.5); kontext.Text = "Context Models"; kontext.SetCenter((3 / 2.54), (25 / 2.54));
            kontext.CellsSRC(1, 11, 4).Formula = "0"; kontext.LineStyle = "none"; kontext.CellsSRC(3, 0, 7).FormulaU = "18 pt";
            kontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            wissenskontext = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); wissenskontext.Text = "Context of Knowledge";
            wissenskontext.SetCenter((3 / 2.54), (22 / 2.54)); wissenskontext.CellsSRC(1, 11, 4).Formula = "0";
            wissenskontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusWk = page.DrawOval(1, 1, 1.16, 1.16); statusWk.SetCenter(3 / 2.54, 20.5 / 2.54); statusWk.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
           
            struktKontext = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); struktKontext.Text = "Structural operational Context";
            struktKontext.SetCenter((8 / 2.54), (22 / 2.54)); struktKontext.CellsSRC(1, 11, 4).Formula = "0";
            struktKontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statussK = page.DrawOval(1, 1, 1.16, 1.16); statussK.SetCenter(8 / 2.54, 20.5 / 2.54); statussK.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            funktKontext = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); funktKontext.Text = "Functional operational Context";
            funktKontext.SetCenter((13 / 2.54), (22 / 2.54)); funktKontext.CellsSRC(1, 11, 4).Formula = "0";
            funktKontext.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusfK = page.DrawOval(1, 1, 1.16, 1.16); statusfK.SetCenter(13 / 2.54, 20.5 / 2.54); statusfK.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            //Loesungsneutrale Modelle
            neutral = page.DrawRectangle(1, 1, 4, 1.5); neutral.Text = "Solution-unaware Models"; neutral.SetCenter((4 / 2.54), (18 / 2.54));
            neutral.CellsSRC(1, 11, 4).Formula = "0"; neutral.LineStyle = "none"; neutral.CellsSRC(3, 0, 7).FormulaU = "18 pt";
            neutral.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            goals = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); goals.Text = "Goals";
            goals.SetCenter((3 / 2.54), (15 / 2.54)); goals.CellsSRC(1, 11, 4).Formula = "0";
            goals.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusG = page.DrawOval(1, 1, 1.16, 1.16); statusG.SetCenter(3 / 2.54, 13.5 / 2.54); statusG.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            useMap = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); useMap.Text = "Use-Case Maps";
            useMap.SetCenter((8 / 2.54), (15 / 2.54)); useMap.CellsSRC(1, 11, 4).Formula = "0";
            useMap.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusUcm = page.DrawOval(1, 1, 1.16, 1.16); statusUcm.SetCenter(8 / 2.54, 13.5 / 2.54); statusUcm.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            szenario = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); szenario.Text = "Scenarios";
            szenario.SetCenter((13 / 2.54), (15 / 2.54)); szenario.CellsSRC(1, 11, 4).Formula = "0";
            szenario.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusSz = page.DrawOval(1, 1, 1.16, 1.16); statusSz.SetCenter(13 / 2.54, 13.5 / 2.54); statusSz.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            //Loesungsbezogene Modelle
            bezogen = page.DrawRectangle(1, 1, 4, 1.5); bezogen.Text = "Solution-oriented Models"; bezogen.SetCenter((4 / 2.54), (11 / 2.54));
            bezogen.CellsSRC(1, 11, 4).Formula = "0"; bezogen.LineStyle = "none"; bezogen.CellsSRC(3, 0, 7).FormulaU = "18 pt";
            bezogen.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            struktPerspektive = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); struktPerspektive.Text = "Structural Perspective";
            struktPerspektive.SetCenter((3 / 2.54), (8 / 2.54)); struktPerspektive.CellsSRC(1, 11, 4).Formula = "0";
            struktPerspektive.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statussP = page.DrawOval(1, 1, 1.16, 1.16); statussP.SetCenter(3 / 2.54, 6.5 / 2.54); statussP.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            funktPerspektive = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); funktPerspektive.Text = "Function Perspective";
            funktPerspektive.SetCenter((8 / 2.54), (8 / 2.54)); funktPerspektive.CellsSRC(1, 11, 4).Formula = "0";
            funktPerspektive.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusfP = page.DrawOval(1, 1, 1.16, 1.16); statusfP.SetCenter(8 / 2.54, 6.5 / 2.54); statusfP.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";
            verhaltensPerspektive = page.DrawRectangle(2.5, 2.5, 4.2, 4.2); verhaltensPerspektive.Text = "Behavioral Perspective";
            verhaltensPerspektive.SetCenter((13 / 2.54), (8 / 2.54)); verhaltensPerspektive.CellsSRC(1, 11, 4).Formula = "0";
            verhaltensPerspektive.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
            statusVp = page.DrawOval(1, 1, 1.16, 1.16); statusVp.SetCenter(13 / 2.54, 6.5 / 2.54); statusVp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

            //Weise erstellte Objekte zu
            wkhl=wissenskontext.AddHyperlink();
            wkhl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_CoK.vsdx"));
            skhl= struktKontext.AddHyperlink();
            skhl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_soC.vsdx"));
            fkhl= funktKontext.AddHyperlink();
            fkhl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_foC.vsdx"));

            ghl=goals.AddHyperlink();
            ghl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_Goals.vsdx"));
            uchl=useMap.AddHyperlink();
            uchl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_UCM.vsdx"));
            mschl= szenario.AddHyperlink();
            mschl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_MSC.vsdx"));

            sphl= struktPerspektive.AddHyperlink();
            sphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_stP.vsdx"));
            fphl = funktPerspektive.AddHyperlink();
            fphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_fuP.vsdx"));
            vphl = verhaltensPerspektive.AddHyperlink();
            vphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemname + "_RVP_BP.vsdx"));

        }

        private void CreateemptyModels(Application subapp, string path, string systemname)
        {
            var doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Context of Knowledge.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_CoK.vsdx"));
            doct.Close();
            
            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Functional Context.vssx", 4); 
            subapp.ActivePage.Name = "funktional Perspective";
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_foC.vsdx"));
            doct.Close();
            
            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Structural Context.vssx", 4); 
            subapp.ActivePage.Name = "static Perspective";
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_soC.vsdx"));
            doct.Close();
            
            //Dokumente für Loesungsneutrale Modelle
            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Goal-oriented Requirements Language.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_Goals.vsdx"));
            doct.Close();
            
            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Use Case Maps.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_UCM.vsdx"));
            doct.Close();
            

            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("high Level Message Sequence Chart.vssx", 4); 
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_MSC.vsdx"));
            doct.Close();
            

            //Dokumente für Loesungsbezogene Modelle
            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Class Diagram.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_stP.vsdx"));
            doct.Close();
            

            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("Activity Diagram.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path, systemname + "_RVP_fuP.vsdx"));
            doct.Close();
            

            doct = subapp.Documents.Add("");
            subapp.Documents.OpenEx("State Machine.vssx", 4);
            doct.SaveAs(System.IO.Path.Combine(path,systemname+  "_RVP_BP.vsdx"));
            doct.Close();
            
        }

        public void SetHyperlink()
        {
            //NUr verwenden, wenn noch keine Hyperlinks gesetzt sind
            IVPage overview = new IVPage();
            foreach (IVPage p in this._application.ActiveDocument.Pages)
            {
                if (p.Name=="System Overview")
                {
                    overview = p;
                }
            }
            foreach (IVShape s in overview.Shapes)
            {
                foreach (IVPage p in this._application.ActiveDocument.Pages)
                {
                    if (s.Text == p.Name)
                    {
                        IVHyperlink hl = s.Hyperlinks.Add();
                        hl.SubAddress = p.Name;

                    }
                }
            }

        }
        public void FunctiontoPage()
        {
            List<IVShape> shapes = new List<IVShape>();
            foreach (IVShape shape in this._application.ActivePage.Shapes)
            {
                if (shape.Name.Contains("Context Function") )
                {
                    bool exists = false;
                    foreach (var s in shapes)
                    {
                        if (s.Text == shape.Text) { exists = true; System.Windows.Forms.MessageBox.Show(shape.Text +
                            " already exists"); }
                    }
                    if (exists == false) { shapes.Add(shape); }
                }

            }
            if (shapes.Count >= 1)
            {
                IVDocument stencil = this._application.Documents.OpenEx("Behavioral Context.vssx", 4);
                IVMaster masterfunction = new IVMaster();
                foreach (var m in stencil.Masters)
                {
                    if (m.Name == "Context Entity/ Function")
                    {
                        masterfunction = m;
                    }
                }
                foreach (var shape in shapes)
                {
                    IVPage page = this._application.ActiveDocument.Pages.Add();
                    IVShape shapeh = page.Drop(masterfunction, 10.3 / 2.54, 20.5 / 2.54);
                    shapeh.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                    
                    //Gruppe auslesen und <<Boundary Name>> umbenennen
                    foreach (var subshape in shapeh.Shapes)
                    {
                        if (subshape.Text == "Context Entity/ Function") { subshape.Text = shape.Text; };
                    }


                    page.Name = shape.Text;
                    IVHyperlink hl = shape.Hyperlinks.Add();
                    hl.SubAddress = page.Name;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Context Function found.");
            };

        }

        public void EntitytoPage()
        {
            List<IVShape> shapes = new List<IVShape>();
            foreach( IVShape shape in this._application.ActivePage.Shapes)
            {
                if (shape.Name.Contains("Context Entity (CE)"))
                {
                    bool exists = false;
                    foreach (var s in shapes)
                    {
                        if (s.Text==shape.Text) { exists = true; System.Windows.Forms.MessageBox.Show(shape.Text +
                             " already exists.");}
                    }
                    if (exists == false) { shapes.Add(shape); }
                    
                }
                
            }
            if (shapes.Count >= 1)
            {
                foreach (var shape in shapes)
                {
                    IVPage page = this._application.ActiveDocument.Pages.Add();
                    page.Name = shape.Text;
                    IVDocument stencil = this._application.Documents.OpenEx("Behavioral Context.vssx", 4);
                    IVMaster masterentity = new IVMaster();
                    foreach (var m in stencil.Masters)
                    {
                        if (m.Name == "Context Entity/ Function")
                        {
                            masterentity = m;
                        }       
                    }
                    IVShape shapeh = page.Drop(masterentity, 10.3/2.54, 20.5 / 2.54);
                    shapeh.Text = shape.Text;
                    shapeh.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                    IVHyperlink hl = shape.Hyperlinks.Add();
                    hl.SubAddress = page.Name;
                    
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Context Entity found.");
            };

        }

        public void CreateSubsystems()
        {
            //bestimme Namen des übergeordneten Systems anhand des Seitennamens

            IVPage active = this._application.ActivePage;
            string systemname = active.Name.Substring(4);

            IVSelection selects = this._application.ActiveWindow.Selection;
            List<IVShape> shapes = new List<IVShape>();

            foreach (var shape in selects.SelectionForDragCopy)
            {
                if (shape.Shapes != null)
                {
                    bool firstshape = true;
                    int count = 0;
                    foreach (var subshape in shape.Shapes)
                    {
                        count++;
                        if (count % 2 == 1)
                        {
                            if (firstshape == true)
                            {
                                shapes.Add(subshape);
                            }
                            firstshape = false;
                        }

                    }
                }
                else
                {
                    shapes.Add(shape);
                }
            }
            System.Windows.Forms.MessageBox.Show("Create Documents?");
            //getPage "Systemübersicht"--> dazu Document holen mit passender Page
            //speichere aktuelle Applikation ab und suche Applikation mit der Page "Systemübersicht"
            IVDocument systemdoc = null;
            IVPage systemoverview = null;
            Application subapplic = this._application;
            IntPtr subapplickey= new IntPtr(0);
            IntPtr applickey = new IntPtr(0);
            Application applic = null; ;
            bool found = false;
            foreach (var window in OpenWindowGetter.GetOpenWindows())
            {
                if (found == false)
                {
                    if (window.Value.Contains("Visio Professional"))
                    {
                        OpenWindowGetter.SetForegroundWindow(window.Key);
                        applic = NetOffice.VisioApi.Application.GetActiveInstance();
                        if (subapplic == applic) { subapplickey = window.Key; };
                        foreach (var doc in applic.Documents)
                        {
                            foreach (var page in doc.Pages)
                            {
                                if (page.Name == "System Overview")
                                {
                                    systemdoc = doc;
                                    systemoverview = page;
                                    applickey = window.Key;
                                    found = true;
                                }
                            }
                        }
                    }
                }
            }

            //erstelle für jede ausgewählte Shape/Subsystem auf dem Zeichenblatt "Systemübersicht" ein Rechteck und verbinde dieses mit dem höher gelegenen System
            int counter = 0;
            int sum = shapes.Count;
            IVShape preshape = null;
            foreach (var shape in systemoverview.Shapes)
            {
                if (shape.Text == systemname)
                {
                    preshape = shape;
                }
            }
            double xvalue = (Convert.ToDouble(preshape.CellsSRC(1, 1, 0).FormulaU.Substring(0, preshape.CellsSRC(1, 1, 0).FormulaU.IndexOf(' '))))/10;
            double yvalue = (Convert.ToDouble(preshape.CellsSRC(1, 1, 1).FormulaU.Substring(0, preshape.CellsSRC(1, 1, 1).FormulaU.IndexOf(' '))))/10;
           
            foreach (var shape in shapes)
            {

                IVShape subsystem = systemoverview.DrawRectangle(1, 1, 3, 1.5);
                subsystem.Text = shape.Text;
                subsystem.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                subsystem.SetCenter(BerechneXPosition(xvalue, sum, counter)/2.54, (yvalue - 3.0)/2.54);
                counter++;
                //verbinde zu übergeordnetem System
                preshape.AutoConnect(subsystem, 0);
                //erstelle neues Zeichenblatt und erstelle Hyperlink
                IVPage shapePage = systemdoc.Pages.Add(); 
                shapePage.Name = shape.Text;
                IVHyperlink hl = subsystem.Hyperlinks.Add();
                hl.SubAddress = shapePage.Name; //geht nur wenn, Page in selber Dokumentebene ist.

                //rufe Methode auf, die für die gespeicherten Pages, die benötigten Dokumente erstellt und einbindet
                CreateSubSystemElements(shapePage, applickey);
            }
            //setze Verbinder als gerade/straight
            foreach (var connects in systemoverview.Shapes)
            {
                if (connects.Name.Contains("Dynamic connector") || connects.Name.Contains("Dynamischer Verbinder"))
                {
                    connects.CellsSRC(1, 23, 10).Formula = "16";
                }

            }
            OpenWindowGetter.SetForegroundWindow(subapplickey);
        }

        private void CreateSubSystemElements(IVPage p, IntPtr appkey)
        {
            using (Application app = new Application())
            {
                Application subapplic = this._application;
                IntPtr helpappkey = new IntPtr(0);
                Application applic = null; ;

                foreach (var window in OpenWindowGetter.GetOpenWindows())
                {
                    if (window.Value.Contains("Visio Professional"))
                    {
                        OpenWindowGetter.SetForegroundWindow(window.Key);
                        applic = NetOffice.VisioApi.Application.GetActiveInstance();
                        if (app == applic) { helpappkey = window.Key;};
                    };

                }
                OpenWindowGetter.SetForegroundWindow(helpappkey);
                CreateemptyModels(app, this._application.ActiveDocument.Path, p.Name);
                IVShape header, systemName, rvp, fvp, lvp, tvp, statusRvp, statusFvp, statusLvp, statusTvp;
                IVHyperlink rvphl, fvphl, lvphl, tvphl;
                header = p.DrawRectangle(1, 1, 8, 1.5); header.LineStyle = "none"; header.Text = "Artifacts of " + p.Name;
                header.SetCenter(4, (28 / 2.54)); header.CellsSRC(3, 0, 7).FormulaU = "24 pt";
                header.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

                systemName = p.DrawRectangle(1, 1, 8, 4); systemName.Text = p.Name; systemName.SetCenter(4, (23.2 / 2.54));
                systemName.CellsSRC(1, 11, 4).Formula = "0"; systemName.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";

                rvp = p.DrawRectangle(1, 1, 2.5, 3); rvp.Text = "Requirements Engineering Viewpoint";
                rvp.SetCenter(4.2 / 2.54, (22.8 / 2.54)); rvp.CellsSRC(1, 11, 4).Formula = "0";
                rvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusRvp = p.DrawOval(1, 1, 1.16, 1.16); statusRvp.SetCenter(4.2 / 2.54, 23.5 / 2.54); statusRvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                fvp = p.DrawRectangle(1, 1, 2.5, 3); fvp.Text = "Functional Viewpoint"; fvp.SetCenter(8.2 / 2.54, (22.8 / 2.54));
                fvp.CellsSRC(1, 11, 4).Formula = "0"; fvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusFvp = p.DrawOval(1, 1, 1.16, 1.16); statusFvp.SetCenter(8.2 / 2.54, 23.5 / 2.54); statusFvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                lvp = p.DrawRectangle(1, 1, 2.5, 3); lvp.Text = "Logical Viewpoint"; lvp.SetCenter(12.2 / 2.54, (22.8 / 2.54));
                lvp.CellsSRC(1, 11, 4).Formula = "0"; lvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusLvp = p.DrawOval(1, 1, 1.16, 1.16); statusLvp.SetCenter(12.2 / 2.54, 23.5 / 2.54); statusLvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                tvp = p.DrawRectangle(1, 1, 2.5, 3); tvp.Text = "Technical Viewpoint"; tvp.SetCenter(16.2 / 2.54, (22.8 / 2.54));
                tvp.CellsSRC(1, 11, 4).Formula = "0"; tvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,255,255))";
                statusTvp = p.DrawOval(1, 1, 1.16, 1.16); statusTvp.SetCenter(16.2 / 2.54, 23.5 / 2.54); statusTvp.CellsSRC(1, 3, 0).FormulaU = "THEMEGUARD(RGB(255,0,0))";

                var doc = app.Documents.Add("");
                CreateRvp(p.Name, doc);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_RVP.vsdx"));
                    
                doc.Close();
                rvphl = rvp.AddHyperlink();
                rvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_RVP.vsdx"));

                doc = app.Documents.Add("");
                CreateFvp(p.Name, doc);
                app.Documents.OpenEx("Functional Design - Function Network.vssx", 4); app.Documents.OpenEx("Interface Automata.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_FVP.vsdx"));
                    doc.Close();
                fvphl = fvp.AddHyperlink();
                fvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_FVP.vsdx"));

                doc = app.Documents.Add("");
                CreateLvp(p.Name, doc);
                app.Documents.OpenEx("Class Diagram.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_LVP.vsdx"));
                    doc.Close();
                lvphl = lvp.AddHyperlink();
                lvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_LVP.vsdx"));

                doc = app.Documents.Add("");
                CreateTvp(p.Name, doc);
                app.Documents.OpenEx("State Machine.vssx", 4);
                app.Documents.OpenEx("Interface Automata.vssx", 4);
                doc.SaveAs(System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_TVP.vsdx"));
                    doc.Close();


                tvphl = tvp.AddHyperlink();
                tvphl.Address = (System.IO.Path.Combine(this._application.ActiveDocument.Path, systemName.Text + "_TVP.vsdx"));
                app.Quit();
                OpenWindowGetter.SetForegroundWindow(appkey);
            }

        }

        private double BerechneXPosition(double x, int sum, int counter)
        {
            // ermittle Position des Vorgängers
            double xwert = 0;
            int range = 10;
            int distance = 0;
                if (sum-1 >0) { distance = range / (sum - 1); }
            xwert = x + (counter * distance - (x / 2));
            return xwert;
        }

        //createfor each Connection a Entry or Exit Point at the Boundary Shape
        /// <summary>
        /// erstellt input/output knoten am rand des interface automaten
        /// </summary>
        public void CreateInandOutput()
        {
            List<IVPage> pagesBound = new List<IVPage>();
            List<IVShape> cons;
            IVMaster input = new IVMaster();
            IVMaster output = new IVMaster();

            foreach (var item in this.ActiveMasters)
            {
                if (item.Name == "Input")
                    input = item;
                else if (item.Name == "Output")
                    output = item;
            }
            foreach (var item in this._application.ActiveDocument.Pages)
            {
                foreach (var shape in item.Shapes)
                {
                    if (shape.Name.Contains("Interface"))  pagesBound.Add(item);

                }
            }
            foreach (var item in pagesBound)
            {

                
                cons = new List<IVShape>();
                IVShape boundary = new IVShape();
                List<IVShape> deleted = new List<IVShape>(); ;
                foreach (var connects in item.Shapes)
                {
                    if (connects.Name.Contains("Connection"))
                    {
                        bool exists = false;
                        foreach (var c in cons)
                        {
                            if (c.Text == connects.Text)
                                exists = true;
                        }
                        if (exists == false)
                            cons.Add(connects);
                    };
                    if (connects.Name.Contains("Interface"))
                        boundary = connects;
                    if (connects.Name.Contains("Output"))
                        deleted.Add(connects);
                    if (connects.Name.Contains("Input"))
                        deleted.Add(connects);
                }
                foreach (var d in deleted)
                {
                    d.Delete();
                }
                string xs = boundary.CellsSRC(1, 1, 0).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 0).FormulaU.IndexOf(' '));
                string ys = boundary.CellsSRC(1, 1, 1).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 1).FormulaU.IndexOf(' '));
                string ws = boundary.CellsSRC(1, 1, 2).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 2).FormulaU.IndexOf(' '));
                string hs = boundary.CellsSRC(1, 1, 3).FormulaU.Substring(0, boundary.CellsSRC(1, 1, 3).FormulaU.IndexOf(' '));

                double xvalue = (Convert.ToDouble(xs.Replace('.', ','))) / 10;
                double yvalue = (Convert.ToDouble(ys.Replace('.', ','))) / 10;
                double weight = (Convert.ToDouble(ws.Replace('.', ','))) / 10;
                double height = (Convert.ToDouble(hs.Replace('.', ','))) / 10;
                int count = 1;

                foreach (var inout in cons)
                {

                    {
                        if (inout.Text.Contains("?"))
                        {
                            double x = (((xvalue - (weight / 2.05)) + ((weight / (cons.Count + 1)) * count)));
                            double y = (yvalue + (height / 2) + 0.25);
                            IVShape inputshape = item.Drop(input, x / 2.54, y / 2.54);
                            foreach (var g in inputshape.Shapes)
                            {
                                if (g.Text.Contains("Input"))  g.Text = inout.Text.Substring(0, inout.Text.IndexOf("?")); 
                            }
                        }
                        else if (inout.Text.Contains("!"))
                        {
                            double x = (((xvalue - (weight / 2.05)) + ((weight / (cons.Count + 1)) * count)));
                            double y = (yvalue + (height / 2) + 0.25);
                            IVShape outputshape = item.Drop(output, x / 2.54, y / 2.54);
                            foreach (var g in outputshape.Shapes)
                            {
                                if (g.Text.Contains("Output")) g.Text = inout.Text.Substring(0, inout.Text.IndexOf("!"));
                            }
                        }
                    }
                    count++;
                }

            }

        }
    }  
}

