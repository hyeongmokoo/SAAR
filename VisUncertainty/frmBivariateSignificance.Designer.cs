namespace VisUncertainty
{
    partial class frmBivariateSignificance
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
            "Lee\'s L* Sig",
            "L_sig"}, -1);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.nudSigLv = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cboMeasure = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkDiagZero = new System.Windows.Forms.CheckBox();
            this.txtAlternative = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboFldnm2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboFldnm1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkMap = new System.Windows.Forms.CheckBox();
            this.cboMaptype = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvFields = new System.Windows.Forms.ListView();
            this.colTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigLv)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cboMethod);
            this.groupBox3.Controls.Add(this.nudSigLv);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cboMeasure);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.chkDiagZero);
            this.groupBox3.Location = new System.Drawing.Point(22, 177);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 130);
            this.groupBox3.TabIndex = 129;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Setting";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Method:";
            // 
            // cboMethod
            // 
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "conditional",
            "total"});
            this.cboMethod.Location = new System.Drawing.Point(78, 100);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(114, 21);
            this.cboMethod.TabIndex = 17;
            this.cboMethod.Text = "total";
            // 
            // nudSigLv
            // 
            this.nudSigLv.DecimalPlaces = 3;
            this.nudSigLv.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudSigLv.Location = new System.Drawing.Point(119, 74);
            this.nudSigLv.Name = "nudSigLv";
            this.nudSigLv.Size = new System.Drawing.Size(72, 20);
            this.nudSigLv.TabIndex = 13;
            this.nudSigLv.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Significance level:";
            // 
            // cboMeasure
            // 
            this.cboMeasure.FormattingEnabled = true;
            this.cboMeasure.Items.AddRange(new object[] {
            "Lee\'s L",
            "Local Pearson"});
            this.cboMeasure.Location = new System.Drawing.Point(64, 22);
            this.cboMeasure.Name = "cboMeasure";
            this.cboMeasure.Size = new System.Drawing.Size(127, 21);
            this.cboMeasure.TabIndex = 11;
            this.cboMeasure.Text = "Lee\'s L";
            this.cboMeasure.SelectedIndexChanged += new System.EventHandler(this.cboMeasure_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Measure:";
            // 
            // chkDiagZero
            // 
            this.chkDiagZero.AutoSize = true;
            this.chkDiagZero.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkDiagZero.Checked = true;
            this.chkDiagZero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDiagZero.Location = new System.Drawing.Point(9, 50);
            this.chkDiagZero.Name = "chkDiagZero";
            this.chkDiagZero.Size = new System.Drawing.Size(156, 17);
            this.chkDiagZero.TabIndex = 8;
            this.chkDiagZero.Text = "Non-zero value on diagonal";
            this.chkDiagZero.UseVisualStyleBackColor = true;
            this.chkDiagZero.CheckedChanged += new System.EventHandler(this.chkDiagZero_CheckedChanged);
            // 
            // txtAlternative
            // 
            this.txtAlternative.Location = new System.Drawing.Point(206, 431);
            this.txtAlternative.Name = "txtAlternative";
            this.txtAlternative.Size = new System.Drawing.Size(113, 20);
            this.txtAlternative.TabIndex = 20;
            this.txtAlternative.Text = "two.sided";
            this.txtAlternative.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 431);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Alternative:";
            this.label4.Visible = false;
            // 
            // cboFldnm2
            // 
            this.cboFldnm2.FormattingEnabled = true;
            this.cboFldnm2.Location = new System.Drawing.Point(13, 116);
            this.cboFldnm2.Name = "cboFldnm2";
            this.cboFldnm2.Size = new System.Drawing.Size(179, 21);
            this.cboFldnm2.TabIndex = 102;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 101;
            this.label6.Text = "Variable 2:";
            // 
            // cboFldnm1
            // 
            this.cboFldnm1.FormattingEnabled = true;
            this.cboFldnm1.Location = new System.Drawing.Point(13, 74);
            this.cboFldnm1.Name = "cboFldnm1";
            this.cboFldnm1.Size = new System.Drawing.Size(179, 21);
            this.cboFldnm1.TabIndex = 100;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 99;
            this.label2.Text = "Variable 1:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 98;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(11, 33);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(181, 21);
            this.cboTargetLayer.TabIndex = 97;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboFldnm2);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cboFldnm1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cboTargetLayer);
            this.groupBox4.Location = new System.Drawing.Point(22, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(207, 152);
            this.groupBox4.TabIndex = 130;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Input";
            // 
            // chkMap
            // 
            this.chkMap.AutoSize = true;
            this.chkMap.Checked = true;
            this.chkMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMap.Location = new System.Drawing.Point(6, 94);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(80, 17);
            this.chkMap.TabIndex = 117;
            this.chkMap.Text = "Add a map:";
            this.chkMap.UseVisualStyleBackColor = true;
            // 
            // cboMaptype
            // 
            this.cboMaptype.FormattingEnabled = true;
            this.cboMaptype.Items.AddRange(new object[] {
            "choropleth map",
            "point map"});
            this.cboMaptype.Location = new System.Drawing.Point(86, 91);
            this.cboMaptype.Name = "cboMaptype";
            this.cboMaptype.Size = new System.Drawing.Size(104, 21);
            this.cboMaptype.TabIndex = 119;
            this.cboMaptype.Text = "choropleth map";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboMaptype);
            this.groupBox1.Controls.Add(this.lvFields);
            this.groupBox1.Controls.Add(this.chkMap);
            this.groupBox1.Location = new System.Drawing.Point(22, 313);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 122);
            this.groupBox1.TabIndex = 128;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // lvFields
            // 
            this.lvFields.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTypes,
            this.colNames});
            this.lvFields.HoverSelection = true;
            this.lvFields.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvFields.LabelEdit = true;
            this.lvFields.Location = new System.Drawing.Point(5, 22);
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(193, 61);
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
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(25, 441);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 126;
            this.btnRun.Text = "OK";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(148, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 127;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmBivariateSignificance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 473);
            this.Controls.Add(this.txtAlternative);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmBivariateSignificance";
            this.Text = "Bivariate Spatial Significant Quadrant Maps ";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigLv)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboMeasure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkDiagZero;
        private System.Windows.Forms.ComboBox cboFldnm2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboFldnm1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkMap;
        private System.Windows.Forms.ComboBox cboMaptype;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader colTypes;
        private System.Windows.Forms.ColumnHeader colNames;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudSigLv;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAlternative;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboMethod;
    }
}