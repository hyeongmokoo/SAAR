namespace VisUncertainty
{
    partial class frmBiLocalSAM
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Statistic",
            "Ii"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Standard Deviate",
            "Zi"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "p-value",
            "Pr"}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "FLAG",
            "flg"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBiLocalSAM));
            this.chkMap = new System.Windows.Forms.CheckBox();
            this.btnOpenSWM = new System.Windows.Forms.Button();
            this.txtSWM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvFields = new System.Windows.Forms.ListView();
            this.colTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.cboAdjustment = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSAM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboFldnm1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.cboFldnm2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudSigLv = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cboSigMethod = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigLv)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMap
            // 
            this.chkMap.AutoSize = true;
            this.chkMap.Checked = true;
            this.chkMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMap.Location = new System.Drawing.Point(15, 417);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(77, 17);
            this.chkMap.TabIndex = 76;
            this.chkMap.Text = "Add a map";
            this.chkMap.UseVisualStyleBackColor = true;
            // 
            // btnOpenSWM
            // 
            this.btnOpenSWM.Enabled = false;
            this.btnOpenSWM.Location = new System.Drawing.Point(367, 419);
            this.btnOpenSWM.Name = "btnOpenSWM";
            this.btnOpenSWM.Size = new System.Drawing.Size(29, 23);
            this.btnOpenSWM.TabIndex = 75;
            this.btnOpenSWM.Text = "...";
            this.btnOpenSWM.UseVisualStyleBackColor = true;
            this.btnOpenSWM.Visible = false;
            // 
            // txtSWM
            // 
            this.txtSWM.Enabled = false;
            this.txtSWM.Location = new System.Drawing.Point(193, 420);
            this.txtSWM.Name = "txtSWM";
            this.txtSWM.Size = new System.Drawing.Size(166, 20);
            this.txtSWM.TabIndex = 74;
            this.txtSWM.Text = "Default";
            this.txtSWM.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(193, 406);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 73;
            this.label5.Text = "Spatial Weight Matrix";
            this.label5.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvFields);
            this.groupBox1.Location = new System.Drawing.Point(10, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 131);
            this.groupBox1.TabIndex = 72;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Save Results";
            // 
            // lvFields
            // 
            this.lvFields.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTypes,
            this.colNames});
            this.lvFields.HoverSelection = true;
            this.lvFields.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.lvFields.LabelEdit = true;
            this.lvFields.Location = new System.Drawing.Point(5, 22);
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(193, 102);
            this.lvFields.TabIndex = 56;
            this.lvFields.UseCompatibleStateImageBehavior = false;
            this.lvFields.View = System.Windows.Forms.View.Details;
            // 
            // colTypes
            // 
            this.colTypes.Text = "Types";
            this.colTypes.Width = 102;
            // 
            // colNames
            // 
            this.colNames.Text = "Field Name";
            this.colNames.Width = 77;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(141, 444);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(9, 444);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 70;
            this.btnRun.Text = "♣";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // cboAdjustment
            // 
            this.cboAdjustment.Enabled = false;
            this.cboAdjustment.FormattingEnabled = true;
            this.cboAdjustment.Items.AddRange(new object[] {
            "None",
            "Bonferroni correction"});
            this.cboAdjustment.Location = new System.Drawing.Point(191, 459);
            this.cboAdjustment.Name = "cboAdjustment";
            this.cboAdjustment.Size = new System.Drawing.Size(208, 21);
            this.cboAdjustment.TabIndex = 69;
            this.cboAdjustment.Text = "None";
            this.cboAdjustment.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(191, 443);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 68;
            this.label4.Text = "Multiple Test Adjustment:";
            this.label4.Visible = false;
            // 
            // cboSAM
            // 
            this.cboSAM.FormattingEnabled = true;
            this.cboSAM.Items.AddRange(new object[] {
            "Lee\'s L"});
            this.cboSAM.Location = new System.Drawing.Point(13, 159);
            this.cboSAM.Name = "cboSAM";
            this.cboSAM.Size = new System.Drawing.Size(208, 21);
            this.cboSAM.TabIndex = 67;
            this.cboSAM.Text = "Lee\'s L";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 13);
            this.label3.TabIndex = 66;
            this.label3.Text = "Select Spatial Autocorrelation Measure:";
            // 
            // cboFldnm1
            // 
            this.cboFldnm1.FormattingEnabled = true;
            this.cboFldnm1.Location = new System.Drawing.Point(15, 67);
            this.cboFldnm1.Name = "cboFldnm1";
            this.cboFldnm1.Size = new System.Drawing.Size(208, 21);
            this.cboFldnm1.TabIndex = 65;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 64;
            this.label2.Text = "Variable 1:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 63;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(13, 26);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(210, 21);
            this.cboTargetLayer.TabIndex = 62;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // cboFldnm2
            // 
            this.cboFldnm2.FormattingEnabled = true;
            this.cboFldnm2.Location = new System.Drawing.Point(15, 109);
            this.cboFldnm2.Name = "cboFldnm2";
            this.cboFldnm2.Size = new System.Drawing.Size(208, 21);
            this.cboFldnm2.TabIndex = 78;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 77;
            this.label6.Text = "Variable 2:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudSigLv);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cboSigMethod);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(13, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 78);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Significance testing";
            // 
            // nudSigLv
            // 
            this.nudSigLv.DecimalPlaces = 3;
            this.nudSigLv.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudSigLv.Location = new System.Drawing.Point(129, 47);
            this.nudSigLv.Name = "nudSigLv";
            this.nudSigLv.Size = new System.Drawing.Size(66, 20);
            this.nudSigLv.TabIndex = 3;
            this.nudSigLv.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Significance level:";
            // 
            // cboSigMethod
            // 
            this.cboSigMethod.FormattingEnabled = true;
            this.cboSigMethod.Items.AddRange(new object[] {
            "Conditional",
            "Total"});
            this.cboSigMethod.Location = new System.Drawing.Point(52, 18);
            this.cboSigMethod.Name = "cboSigMethod";
            this.cboSigMethod.Size = new System.Drawing.Size(144, 21);
            this.cboSigMethod.TabIndex = 1;
            this.cboSigMethod.Text = "Conditional";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Method:";
            // 
            // frmBiLocalSAM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 477);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cboFldnm2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkMap);
            this.Controls.Add(this.btnOpenSWM);
            this.Controls.Add(this.txtSWM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.cboAdjustment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboSAM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboFldnm1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBiLocalSAM";
            this.Text = "Bivariate local SAM";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigLv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkMap;
        private System.Windows.Forms.Button btnOpenSWM;
        private System.Windows.Forms.TextBox txtSWM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader colTypes;
        private System.Windows.Forms.ColumnHeader colNames;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ComboBox cboAdjustment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSAM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboFldnm1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.ComboBox cboFldnm2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudSigLv;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboSigMethod;
        private System.Windows.Forms.Label label7;
    }
}