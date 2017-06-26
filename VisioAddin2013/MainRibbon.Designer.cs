namespace VisioAddin2013
{
    partial class MainRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MainRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainRibbon));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.ModelTargetDropDown = this.Factory.CreateRibbonDropDown();
            this.AboutButton = this.Factory.CreateRibbonButton();
            this.VerifyButton = this.Factory.CreateRibbonButton();
            this.ImportButton = this.Factory.CreateRibbonButton();
            this.ExportButton = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.ModelTargetDropDown);
            this.group1.Items.Add(this.AboutButton);
            this.group1.Items.Add(this.VerifyButton);
            this.group1.Items.Add(this.ImportButton);
            this.group1.Items.Add(this.ExportButton);
            this.group1.Label = "SPES Modelverification";
            this.group1.Name = "group1";
            // 
            // ModelTargetDropDown
            // 
            this.ModelTargetDropDown.Label = "dropDown1";
            this.ModelTargetDropDown.Name = "ModelTargetDropDown";
            this.ModelTargetDropDown.ShowLabel = false;
            this.ModelTargetDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ModelTargetDropDown_SelectionChanged);
            // 
            // AboutButton
            // 
            this.AboutButton.Image = ((System.Drawing.Image)(resources.GetObject("AboutButton.Image")));
            this.AboutButton.Label = "About";
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.ShowImage = true;
            this.AboutButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.AboutButton_Click);
            // 
            // VerifyButton
            // 
            this.VerifyButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.VerifyButton.Image = ((System.Drawing.Image)(resources.GetObject("VerifyButton.Image")));
            this.VerifyButton.Label = "Verify";
            this.VerifyButton.Name = "VerifyButton";
            this.VerifyButton.ShowImage = true;
            this.VerifyButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Verify_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ImportButton.Image = ((System.Drawing.Image)(resources.GetObject("ImportButton.Image")));
            this.ImportButton.Label = "Import";
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.ShowImage = true;
            this.ImportButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ExportButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportButton.Image")));
            this.ExportButton.Label = "Export";
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.ShowImage = true;
            this.ExportButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ExportButton_Click);
            // 
            // MainRibbon
            // 
            this.Name = "MainRibbon";
            this.RibbonType = "Microsoft.Visio.Drawing";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MainRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton VerifyButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ImportButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ExportButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown ModelTargetDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton AboutButton;
    }

    partial class ThisRibbonCollection
    {
        internal MainRibbon MainRibbon
        {
            get { return this.GetRibbon<MainRibbon>(); }
        }
    }
}
