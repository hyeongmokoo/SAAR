namespace VisUncertainty
{
    partial class frmCreateSWM_inR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateSWM_inR));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboFieldName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.sfdSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkCumulate = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudAdvanced = new System.Windows.Forms.NumericUpDown();
            this.btnSubset = new System.Windows.Forms.Button();
            this.lblAdvanced = new System.Windows.Forms.Label();
            this.lblClip = new System.Windows.Forms.Label();
            this.txtSWM = new System.Windows.Forms.ComboBox();
            this.ofdOpenSWM = new System.Windows.Forms.OpenFileDialog();
            this.cboCoding = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdvanced)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(147, 322);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 23);
            this.btnCancel.TabIndex = 51;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(14, 322);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(100, 23);
            this.btnRun.TabIndex = 50;
            this.btnRun.Text = "Create";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Location = new System.Drawing.Point(210, 111);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(37, 23);
            this.btnSaveFile.TabIndex = 49;
            this.btnSaveFile.Text = "...";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(14, 114);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(190, 20);
            this.txtOutput.TabIndex = 48;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Output file:";
            // 
            // cboFieldName
            // 
            this.cboFieldName.FormattingEnabled = true;
            this.cboFieldName.Location = new System.Drawing.Point(14, 70);
            this.cboFieldName.Name = "cboFieldName";
            this.cboFieldName.Size = new System.Drawing.Size(233, 21);
            this.cboFieldName.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "ID Field:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 44;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(14, 26);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(233, 21);
            this.cboTargetLayer.TabIndex = 43;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // sfdSaveFile
            // 
            this.sfdSaveFile.Filter = "GAL files|*.gal";
            this.sfdSaveFile.Title = "Save GAL file";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboCoding);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkCumulate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nudAdvanced);
            this.groupBox1.Controls.Add(this.btnSubset);
            this.groupBox1.Controls.Add(this.lblAdvanced);
            this.groupBox1.Controls.Add(this.lblClip);
            this.groupBox1.Controls.Add(this.txtSWM);
            this.groupBox1.Location = new System.Drawing.Point(14, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 176);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // chkCumulate
            // 
            this.chkCumulate.AutoSize = true;
            this.chkCumulate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCumulate.Location = new System.Drawing.Point(11, 118);
            this.chkCumulate.Name = "chkCumulate";
            this.chkCumulate.Size = new System.Drawing.Size(122, 17);
            this.chkCumulate.TabIndex = 93;
            this.chkCumulate.Text = "Cumulate neighbors:";
            this.chkCumulate.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 87;
            this.label5.Text = "Definition:";
            // 
            // nudAdvanced
            // 
            this.nudAdvanced.Location = new System.Drawing.Point(117, 89);
            this.nudAdvanced.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nudAdvanced.Name = "nudAdvanced";
            this.nudAdvanced.Size = new System.Drawing.Size(101, 20);
            this.nudAdvanced.TabIndex = 92;
            this.nudAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAdvanced.ValueChanged += new System.EventHandler(this.nudAdvanced_ValueChanged);
            // 
            // btnSubset
            // 
            this.btnSubset.Enabled = false;
            this.btnSubset.Location = new System.Drawing.Point(149, 140);
            this.btnSubset.Name = "btnSubset";
            this.btnSubset.Size = new System.Drawing.Size(69, 23);
            this.btnSubset.TabIndex = 88;
            this.btnSubset.Text = "Clip";
            this.btnSubset.UseVisualStyleBackColor = true;
            this.btnSubset.Click += new System.EventHandler(this.btnSubset_Click);
            // 
            // lblAdvanced
            // 
            this.lblAdvanced.AutoSize = true;
            this.lblAdvanced.Location = new System.Drawing.Point(11, 91);
            this.lblAdvanced.Name = "lblAdvanced";
            this.lblAdvanced.Size = new System.Drawing.Size(100, 13);
            this.lblAdvanced.TabIndex = 91;
            this.lblAdvanced.Text = "Threshold distance:";
            // 
            // lblClip
            // 
            this.lblClip.AutoSize = true;
            this.lblClip.Location = new System.Drawing.Point(11, 145);
            this.lblClip.Name = "lblClip";
            this.lblClip.Size = new System.Drawing.Size(86, 13);
            this.lblClip.TabIndex = 90;
            this.lblClip.Text = "Clip by polygons:";
            // 
            // txtSWM
            // 
            this.txtSWM.FormattingEnabled = true;
            this.txtSWM.Location = new System.Drawing.Point(71, 27);
            this.txtSWM.Name = "txtSWM";
            this.txtSWM.Size = new System.Drawing.Size(147, 21);
            this.txtSWM.TabIndex = 89;
            this.txtSWM.TextChanged += new System.EventHandler(this.txtSWM_TextChanged);
            // 
            // ofdOpenSWM
            // 
            this.ofdOpenSWM.Filter = "GAL files|*.gal|GWT files|*.gwt";
            this.ofdOpenSWM.Title = "Open GAL files";
            // 
            // cboCoding
            // 
            this.cboCoding.FormattingEnabled = true;
            this.cboCoding.Location = new System.Drawing.Point(71, 57);
            this.cboCoding.Name = "cboCoding";
            this.cboCoding.Size = new System.Drawing.Size(147, 21);
            this.cboCoding.TabIndex = 95;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 94;
            this.label4.Text = "Coding:";
            // 
            // frmCreateSWM_inR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(259, 358);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnSaveFile);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboFieldName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateSWM_inR";
            this.Text = "Create Spatial Weights (*.GAL)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdvanced)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboFieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.SaveFileDialog sfdSaveFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCumulate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudAdvanced;
        private System.Windows.Forms.Button btnSubset;
        private System.Windows.Forms.Label lblAdvanced;
        private System.Windows.Forms.Label lblClip;
        public System.Windows.Forms.ComboBox txtSWM;
        private System.Windows.Forms.OpenFileDialog ofdOpenSWM;
        public System.Windows.Forms.ComboBox cboCoding;
        private System.Windows.Forms.Label label4;
    }
}