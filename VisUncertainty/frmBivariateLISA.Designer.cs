namespace VisUncertainty
{
    partial class frmBivariateLISA
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
            "Lee\'s L*",
            "LocalL"}, -1);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboFldnm2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboFldnm1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboMeasure = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkDiagZero = new System.Windows.Forms.CheckBox();
            this.chkMap = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboMapOption = new System.Windows.Forms.ComboBox();
            this.lvFields = new System.Windows.Forms.ListView();
            this.colTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboFldnm2);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cboFldnm1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.cboTargetLayer);
            this.groupBox4.Location = new System.Drawing.Point(21, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(207, 152);
            this.groupBox4.TabIndex = 120;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Input";
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboMeasure);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.chkDiagZero);
            this.groupBox3.Location = new System.Drawing.Point(21, 177);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(207, 72);
            this.groupBox3.TabIndex = 119;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Setting";
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
            // chkMap
            // 
            this.chkMap.AutoSize = true;
            this.chkMap.Checked = true;
            this.chkMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMap.Location = new System.Drawing.Point(6, 94);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(77, 17);
            this.chkMap.TabIndex = 117;
            this.chkMap.Text = "Add a map";
            this.chkMap.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboMapOption);
            this.groupBox1.Controls.Add(this.lvFields);
            this.groupBox1.Controls.Add(this.chkMap);
            this.groupBox1.Location = new System.Drawing.Point(21, 260);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 145);
            this.groupBox1.TabIndex = 116;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // cboMapOption
            // 
            this.cboMapOption.Enabled = false;
            this.cboMapOption.FormattingEnabled = true;
            this.cboMapOption.Items.AddRange(new object[] {
            "Local Pearson",
            "z-score of variable 1",
            "z-score of variable 2"});
            this.cboMapOption.Location = new System.Drawing.Point(85, 91);
            this.cboMapOption.Name = "cboMapOption";
            this.cboMapOption.Size = new System.Drawing.Size(112, 21);
            this.cboMapOption.TabIndex = 118;
            this.cboMapOption.SelectedIndexChanged += new System.EventHandler(this.cboMapOption_SelectedIndexChanged);
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
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(144, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 115;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(27, 415);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 114;
            this.btnRun.Text = "OK";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmBivariateLISA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 449);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Name = "frmBivariateLISA";
            this.Text = "Bivariate LISA Maps";
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboFldnm2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboFldnm1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboMeasure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkDiagZero;
        private System.Windows.Forms.CheckBox chkMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader colTypes;
        private System.Windows.Forms.ColumnHeader colNames;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ComboBox cboMapOption;
    }
}