namespace VisUncertainty
{
    partial class frmQQplot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQQplot));
            this.chkUseSelected = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnQQplot = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.txtDf2 = new System.Windows.Forms.TextBox();
            this.lblDf2 = new System.Windows.Forms.Label();
            this.txtDf1 = new System.Windows.Forms.TextBox();
            this.lblDf1 = new System.Windows.Forms.Label();
            this.cboDistribution = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboVariable1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUseSelected
            // 
            this.chkUseSelected.AutoSize = true;
            this.chkUseSelected.Location = new System.Drawing.Point(11, 211);
            this.chkUseSelected.Name = "chkUseSelected";
            this.chkUseSelected.Size = new System.Drawing.Size(134, 17);
            this.chkUseSelected.TabIndex = 61;
            this.chkUseSelected.Text = "Use Selected Features";
            this.chkUseSelected.UseVisualStyleBackColor = true;
            this.chkUseSelected.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(148, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 25);
            this.btnCancel.TabIndex = 60;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnQQplot
            // 
            this.btnQQplot.Location = new System.Drawing.Point(15, 211);
            this.btnQQplot.Name = "btnQQplot";
            this.btnQQplot.Size = new System.Drawing.Size(102, 25);
            this.btnQQplot.TabIndex = 59;
            this.btnQQplot.Text = "Create Q-Q Plot";
            this.btnQQplot.UseVisualStyleBackColor = true;
            this.btnQQplot.Click += new System.EventHandler(this.btnQQplot_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 58;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(13, 25);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(235, 21);
            this.cboTargetLayer.TabIndex = 57;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // txtDf2
            // 
            this.txtDf2.Enabled = false;
            this.txtDf2.Location = new System.Drawing.Point(172, 60);
            this.txtDf2.Name = "txtDf2";
            this.txtDf2.Size = new System.Drawing.Size(52, 20);
            this.txtDf2.TabIndex = 69;
            this.txtDf2.Visible = false;
            // 
            // lblDf2
            // 
            this.lblDf2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDf2.AutoSize = true;
            this.lblDf2.Location = new System.Drawing.Point(115, 66);
            this.lblDf2.Name = "lblDf2";
            this.lblDf2.Size = new System.Drawing.Size(28, 13);
            this.lblDf2.TabIndex = 68;
            this.lblDf2.Text = "df2=";
            this.lblDf2.Visible = false;
            // 
            // txtDf1
            // 
            this.txtDf1.Enabled = false;
            this.txtDf1.Location = new System.Drawing.Point(58, 62);
            this.txtDf1.Name = "txtDf1";
            this.txtDf1.Size = new System.Drawing.Size(52, 20);
            this.txtDf1.TabIndex = 67;
            this.txtDf1.Visible = false;
            // 
            // lblDf1
            // 
            this.lblDf1.AutoSize = true;
            this.lblDf1.Location = new System.Drawing.Point(9, 66);
            this.lblDf1.Name = "lblDf1";
            this.lblDf1.Size = new System.Drawing.Size(28, 13);
            this.lblDf1.TabIndex = 66;
            this.lblDf1.Text = "df1=";
            this.lblDf1.Visible = false;
            // 
            // cboDistribution
            // 
            this.cboDistribution.FormattingEnabled = true;
            this.cboDistribution.Items.AddRange(new object[] {
            "Normal",
            "t",
            "F",
            "Chi-square",
            "Beta",
            "Gamma",
            "Binomial",
            "Negative binomial",
            "Poisson"});
            this.cboDistribution.Location = new System.Drawing.Point(10, 33);
            this.cboDistribution.Name = "cboDistribution";
            this.cboDistribution.Size = new System.Drawing.Size(214, 21);
            this.cboDistribution.TabIndex = 65;
            this.cboDistribution.SelectedIndexChanged += new System.EventHandler(this.cboDistribution_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "Distribution:";
            // 
            // cboVariable1
            // 
            this.cboVariable1.FormattingEnabled = true;
            this.cboVariable1.Location = new System.Drawing.Point(12, 75);
            this.cboVariable1.Name = "cboVariable1";
            this.cboVariable1.Size = new System.Drawing.Size(236, 21);
            this.cboVariable1.TabIndex = 63;
            this.cboVariable1.SelectedIndexChanged += new System.EventHandler(this.cboVariable1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Value Field:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboDistribution);
            this.groupBox1.Controls.Add(this.txtDf2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblDf2);
            this.groupBox1.Controls.Add(this.lblDf1);
            this.groupBox1.Controls.Add(this.txtDf1);
            this.groupBox1.Location = new System.Drawing.Point(13, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 100);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // frmQQplot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(262, 252);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboVariable1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnQQplot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Controls.Add(this.chkUseSelected);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQQplot";
            this.Text = "Quantile comparison plot";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseSelected;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnQQplot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.TextBox txtDf2;
        private System.Windows.Forms.Label lblDf2;
        private System.Windows.Forms.TextBox txtDf1;
        private System.Windows.Forms.Label lblDf1;
        private System.Windows.Forms.ComboBox cboDistribution;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboVariable1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}