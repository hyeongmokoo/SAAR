namespace VisUncertainty
{
    partial class frmSWMAnalysis
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
            this.chkCumulate = new System.Windows.Forms.CheckBox();
            this.nudAdvanced = new System.Windows.Forms.NumericUpDown();
            this.lblAdvanced = new System.Windows.Forms.Label();
            this.txtSWM = new System.Windows.Forms.ComboBox();
            this.lblClip = new System.Windows.Forms.Label();
            this.lblGeoDa = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSubset = new System.Windows.Forms.Button();
            this.btnOpenSWM = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.ofdOpenSWM = new System.Windows.Forms.OpenFileDialog();
            this.btnSummary = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdvanced)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkCumulate);
            this.groupBox1.Controls.Add(this.nudAdvanced);
            this.groupBox1.Controls.Add(this.lblAdvanced);
            this.groupBox1.Controls.Add(this.txtSWM);
            this.groupBox1.Controls.Add(this.lblClip);
            this.groupBox1.Controls.Add(this.lblGeoDa);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnSubset);
            this.groupBox1.Controls.Add(this.btnOpenSWM);
            this.groupBox1.Location = new System.Drawing.Point(12, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 153);
            this.groupBox1.TabIndex = 76;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spatial Weights Matrix";
            // 
            // chkCumulate
            // 
            this.chkCumulate.AutoSize = true;
            this.chkCumulate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCumulate.Location = new System.Drawing.Point(9, 73);
            this.chkCumulate.Name = "chkCumulate";
            this.chkCumulate.Size = new System.Drawing.Size(122, 17);
            this.chkCumulate.TabIndex = 77;
            this.chkCumulate.Text = "Cumulate neighbors:";
            this.chkCumulate.UseVisualStyleBackColor = true;
            // 
            // nudAdvanced
            // 
            this.nudAdvanced.Location = new System.Drawing.Point(115, 47);
            this.nudAdvanced.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nudAdvanced.Name = "nudAdvanced";
            this.nudAdvanced.Size = new System.Drawing.Size(85, 20);
            this.nudAdvanced.TabIndex = 76;
            this.nudAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAdvanced.ValueChanged += new System.EventHandler(this.nudAdvanced_ValueChanged);
            // 
            // lblAdvanced
            // 
            this.lblAdvanced.AutoSize = true;
            this.lblAdvanced.Location = new System.Drawing.Point(9, 48);
            this.lblAdvanced.Name = "lblAdvanced";
            this.lblAdvanced.Size = new System.Drawing.Size(100, 13);
            this.lblAdvanced.TabIndex = 75;
            this.lblAdvanced.Text = "Threshold distance:";
            // 
            // txtSWM
            // 
            this.txtSWM.FormattingEnabled = true;
            this.txtSWM.Location = new System.Drawing.Point(69, 20);
            this.txtSWM.Name = "txtSWM";
            this.txtSWM.Size = new System.Drawing.Size(131, 21);
            this.txtSWM.TabIndex = 74;
            this.txtSWM.TextChanged += new System.EventHandler(this.txtSWM_TextChanged);
            // 
            // lblClip
            // 
            this.lblClip.AutoSize = true;
            this.lblClip.Location = new System.Drawing.Point(9, 98);
            this.lblClip.Name = "lblClip";
            this.lblClip.Size = new System.Drawing.Size(86, 13);
            this.lblClip.TabIndex = 74;
            this.lblClip.Text = "Clip by polygons:";
            // 
            // lblGeoDa
            // 
            this.lblGeoDa.AutoSize = true;
            this.lblGeoDa.Location = new System.Drawing.Point(9, 126);
            this.lblGeoDa.Name = "lblGeoDa";
            this.lblGeoDa.Size = new System.Drawing.Size(145, 13);
            this.lblGeoDa.TabIndex = 73;
            this.lblGeoDa.Text = "Load SWM from GeoDa files:";
            this.lblGeoDa.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 64;
            this.label5.Text = "Definition:";
            // 
            // btnSubset
            // 
            this.btnSubset.Enabled = false;
            this.btnSubset.Location = new System.Drawing.Point(148, 93);
            this.btnSubset.Name = "btnSubset";
            this.btnSubset.Size = new System.Drawing.Size(52, 23);
            this.btnSubset.TabIndex = 72;
            this.btnSubset.Text = "Clip";
            this.btnSubset.UseVisualStyleBackColor = true;
            this.btnSubset.Click += new System.EventHandler(this.btnSubset_Click);
            // 
            // btnOpenSWM
            // 
            this.btnOpenSWM.Location = new System.Drawing.Point(169, 121);
            this.btnOpenSWM.Name = "btnOpenSWM";
            this.btnOpenSWM.Size = new System.Drawing.Size(31, 23);
            this.btnOpenSWM.TabIndex = 66;
            this.btnOpenSWM.Text = "...";
            this.btnOpenSWM.UseVisualStyleBackColor = true;
            this.btnOpenSWM.Visible = false;
            this.btnOpenSWM.Click += new System.EventHandler(this.btnOpenSWM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 75;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(12, 28);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(210, 21);
            this.cboTargetLayer.TabIndex = 74;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // ofdOpenSWM
            // 
            this.ofdOpenSWM.Filter = "GAL files|*.gal|GWT files|*.gwt";
            this.ofdOpenSWM.Title = "Open GAL files";
            // 
            // btnSummary
            // 
            this.btnSummary.Location = new System.Drawing.Point(21, 214);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(75, 23);
            this.btnSummary.TabIndex = 77;
            this.btnSummary.Text = "Summary";
            this.btnSummary.UseVisualStyleBackColor = true;
            this.btnSummary.Click += new System.EventHandler(this.btnSummary_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(137, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 78;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSWMAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 249);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSummary);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Name = "frmSWMAnalysis";
            this.Text = "SWM Analysis";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdvanced)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCumulate;
        private System.Windows.Forms.NumericUpDown nudAdvanced;
        private System.Windows.Forms.Label lblAdvanced;
        private System.Windows.Forms.ComboBox txtSWM;
        private System.Windows.Forms.Label lblClip;
        private System.Windows.Forms.Label lblGeoDa;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSubset;
        private System.Windows.Forms.Button btnOpenSWM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.OpenFileDialog ofdOpenSWM;
        private System.Windows.Forms.Button btnSummary;
        private System.Windows.Forms.Button btnCancel;
    }
}