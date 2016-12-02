namespace VisioAddin2010
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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.ModelTargetDropDown = this.Factory.CreateRibbonDropDown();
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
            this.group1.Items.Add(this.VerifyButton);
            this.group1.Items.Add(this.ImportButton);
            this.group1.Items.Add(this.ExportButton);
            this.group1.Label = "SPES";
            this.group1.Name = "group1";
            // 
            // ModelTargetDropDown
            // 
            this.ModelTargetDropDown.Label = "dropDown1";
            this.ModelTargetDropDown.Name = "ModelTargetDropDown";
            this.ModelTargetDropDown.ShowLabel = false;
            this.ModelTargetDropDown.SelectionChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ModelTargetDropDown_SelectionChanged);
            // 
            // VerifyButton
            // 
            this.VerifyButton.Label = "Verify";
            this.VerifyButton.Name = "VerifyButton";
            this.VerifyButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.VerifyButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Label = "Import";
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ImportButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Label = "Export";
            this.ExportButton.Name = "ExportButton";
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
        internal Microsoft.Office.Tools.Ribbon.RibbonDropDown ModelTargetDropDown;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton VerifyButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ImportButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ExportButton;
    }

    partial class ThisRibbonCollection
    {
        internal MainRibbon MainRibbon
        {
            get { return this.GetRibbon<MainRibbon>(); }
        }
    }
}
