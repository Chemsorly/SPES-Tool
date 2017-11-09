namespace VisioAddin2010
{
    partial class GitIssueWindow
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.IssueTitleTextbox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.IssueBodyTextbox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.IssueAuthorTextbox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.IssueCancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.IssueTitleTextbox);
            this.groupBox1.Location = new System.Drawing.Point(13, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 52);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Issue Title";
            // 
            // IssueTitleTextbox
            // 
            this.IssueTitleTextbox.Location = new System.Drawing.Point(7, 19);
            this.IssueTitleTextbox.Name = "IssueTitleTextbox";
            this.IssueTitleTextbox.Size = new System.Drawing.Size(418, 20);
            this.IssueTitleTextbox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.IssueBodyTextbox);
            this.groupBox2.Location = new System.Drawing.Point(13, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 288);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Issue Body";
            // 
            // IssueBodyTextbox
            // 
            this.IssueBodyTextbox.Location = new System.Drawing.Point(8, 19);
            this.IssueBodyTextbox.Multiline = true;
            this.IssueBodyTextbox.Name = "IssueBodyTextbox";
            this.IssueBodyTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.IssueBodyTextbox.Size = new System.Drawing.Size(418, 257);
            this.IssueBodyTextbox.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.IssueAuthorTextbox);
            this.groupBox3.Location = new System.Drawing.Point(13, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(431, 52);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Author";
            // 
            // IssueAuthorTextbox
            // 
            this.IssueAuthorTextbox.Location = new System.Drawing.Point(7, 19);
            this.IssueAuthorTextbox.Name = "IssueAuthorTextbox";
            this.IssueAuthorTextbox.Size = new System.Drawing.Size(418, 20);
            this.IssueAuthorTextbox.TabIndex = 0;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(369, 410);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 23);
            this.SendButton.TabIndex = 4;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // IssueCancelButton
            // 
            this.IssueCancelButton.Location = new System.Drawing.Point(13, 410);
            this.IssueCancelButton.Name = "IssueCancelButton";
            this.IssueCancelButton.Size = new System.Drawing.Size(75, 23);
            this.IssueCancelButton.TabIndex = 3;
            this.IssueCancelButton.Text = "Cancel";
            this.IssueCancelButton.UseVisualStyleBackColor = true;
            this.IssueCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // GitIssueWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 441);
            this.Controls.Add(this.IssueCancelButton);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GitIssueWindow";
            this.Text = "Create Issue";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox IssueTitleTextbox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox IssueBodyTextbox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox IssueAuthorTextbox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Button IssueCancelButton;
    }
}