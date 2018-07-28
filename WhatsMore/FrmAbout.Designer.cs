namespace WhatsMore
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.btnOK = new System.Windows.Forms.Button();
            this.lblTitleVer = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtxtLicense = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lnkGitHub = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlButtonImage = new System.Windows.Forms.Panel();
            this.lblButton = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.pnlButtonImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // lblTitleVer
            // 
            resources.ApplyResources(this.lblTitleVer, "lblTitleVer");
            this.lblTitleVer.Name = "lblTitleVer";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.rtxtLicense);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // rtxtLicense
            // 
            resources.ApplyResources(this.rtxtLicense, "rtxtLicense");
            this.rtxtLicense.BackColor = System.Drawing.Color.White;
            this.rtxtLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtLicense.Name = "rtxtLicense";
            this.rtxtLicense.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lnkGitHub
            // 
            resources.ApplyResources(this.lnkGitHub, "lnkGitHub");
            this.lnkGitHub.Name = "lnkGitHub";
            this.lnkGitHub.TabStop = true;
            this.lnkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkGitHub_LinkClicked);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // pnlButtonImage
            // 
            resources.ApplyResources(this.pnlButtonImage, "pnlButtonImage");
            this.pnlButtonImage.BackgroundImage = global::WhatsMore.Properties.Resources.donation_button;
            this.pnlButtonImage.Controls.Add(this.lblButton);
            this.pnlButtonImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlButtonImage.Name = "pnlButtonImage";
            this.pnlButtonImage.Click += new System.EventHandler(this.PnlButtonImage_Click);
            // 
            // lblButton
            // 
            resources.ApplyResources(this.lblButton, "lblButton");
            this.lblButton.BackColor = System.Drawing.Color.Orange;
            this.lblButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblButton.Name = "lblButton";
            this.lblButton.Click += new System.EventHandler(this.LblButton_Click);
            // 
            // FrmAbout
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlButtonImage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lnkGitHub);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitleVer);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FrmAbout_Load);
            this.groupBox1.ResumeLayout(false);
            this.pnlButtonImage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblTitleVer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lnkGitHub;
        private System.Windows.Forms.RichTextBox rtxtLicense;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlButtonImage;
        private System.Windows.Forms.Label lblButton;
    }
}