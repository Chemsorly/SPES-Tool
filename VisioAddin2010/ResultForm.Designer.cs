namespace VisioAddin2010
{
    partial class ResultForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ResultsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.processLevelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExceptionObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.validationFailedMessageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.validationFailedMessageBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ResultsDataGridView
            // 
            this.ResultsDataGridView.AllowUserToAddRows = false;
            this.ResultsDataGridView.AllowUserToDeleteRows = false;
            this.ResultsDataGridView.AutoGenerateColumns = false;
            this.ResultsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.ResultsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.processLevelDataGridViewTextBoxColumn,
            this.ExceptionObject,
            this.messageDataGridViewTextBoxColumn});
            this.ResultsDataGridView.DataSource = this.validationFailedMessageBindingSource;
            this.ResultsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.ResultsDataGridView.Name = "ResultsDataGridView";
            this.ResultsDataGridView.ReadOnly = true;
            this.ResultsDataGridView.Size = new System.Drawing.Size(584, 261);
            this.ResultsDataGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ExceptionObject";
            this.dataGridViewTextBoxColumn1.HeaderText = "Item";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 52;
            // 
            // processLevelDataGridViewTextBoxColumn
            // 
            this.processLevelDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.processLevelDataGridViewTextBoxColumn.DataPropertyName = "ProcessLevel";
            this.processLevelDataGridViewTextBoxColumn.HeaderText = "Level";
            this.processLevelDataGridViewTextBoxColumn.Name = "processLevelDataGridViewTextBoxColumn";
            this.processLevelDataGridViewTextBoxColumn.ReadOnly = true;
            this.processLevelDataGridViewTextBoxColumn.Width = 58;
            // 
            // ExceptionObject
            // 
            this.ExceptionObject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ExceptionObject.DataPropertyName = "ExceptionObject";
            this.ExceptionObject.HeaderText = "Item";
            this.ExceptionObject.Name = "ExceptionObject";
            this.ExceptionObject.ReadOnly = true;
            this.ExceptionObject.Width = 52;
            // 
            // messageDataGridViewTextBoxColumn
            // 
            this.messageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.messageDataGridViewTextBoxColumn.DataPropertyName = "Message";
            this.messageDataGridViewTextBoxColumn.HeaderText = "Message";
            this.messageDataGridViewTextBoxColumn.Name = "messageDataGridViewTextBoxColumn";
            this.messageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // validationFailedMessageBindingSource
            // 
            this.validationFailedMessageBindingSource.DataSource = typeof(SPES_Modelverifier_Base.ValidationFailedMessage);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.ResultsDataGridView);
            this.Name = "ResultForm";
            this.Text = "ResultForm";
            ((System.ComponentModel.ISupportInitialize)(this.ResultsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.validationFailedMessageBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource validationFailedMessageBindingSource;
        private System.Windows.Forms.DataGridView ResultsDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn processLevelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExceptionObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    }
}