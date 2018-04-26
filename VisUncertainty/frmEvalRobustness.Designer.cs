namespace VisUncertainty
{
    partial class frmEvalRobustness
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEvalRobustness));
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cboUncernFld = new System.Windows.Forms.ComboBox();
            this.lblUncernNM = new System.Windows.Forms.Label();
            this.lblValueName = new System.Windows.Forms.Label();
            this.lvSymbol = new System.Windows.Forms.ListView();
            this.colBlank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEval = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCnt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEval = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grbSave = new System.Windows.Forms.GroupBox();
            this.chkAddMap = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboMeasure = new System.Windows.Forms.ComboBox();
            this.lblFldName = new System.Windows.Forms.Label();
            this.txtFldName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.grbSave.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(54, 6);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(164, 21);
            this.cboSourceLayer.TabIndex = 63;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 62;
            this.label1.Text = "Layer:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMethod);
            this.groupBox1.Controls.Add(this.cboUncernFld);
            this.groupBox1.Controls.Add(this.lblUncernNM);
            this.groupBox1.Controls.Add(this.lblValueName);
            this.groupBox1.Location = new System.Drawing.Point(12, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 98);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Classification Information";
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(17, 62);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(108, 13);
            this.lblMethod.TabIndex = 3;
            this.lblMethod.Text = "Classfication Method:";
            // 
            // cboUncernFld
            // 
            this.cboUncernFld.FormattingEnabled = true;
            this.cboUncernFld.Location = new System.Drawing.Point(110, 37);
            this.cboUncernFld.Name = "cboUncernFld";
            this.cboUncernFld.Size = new System.Drawing.Size(127, 21);
            this.cboUncernFld.TabIndex = 2;
            // 
            // lblUncernNM
            // 
            this.lblUncernNM.AutoSize = true;
            this.lblUncernNM.Location = new System.Drawing.Point(17, 41);
            this.lblUncernNM.Name = "lblUncernNM";
            this.lblUncernNM.Size = new System.Drawing.Size(92, 13);
            this.lblUncernNM.TabIndex = 1;
            this.lblUncernNM.Text = "Uncertainty Field: ";
            // 
            // lblValueName
            // 
            this.lblValueName.AutoSize = true;
            this.lblValueName.Location = new System.Drawing.Point(17, 22);
            this.lblValueName.Name = "lblValueName";
            this.lblValueName.Size = new System.Drawing.Size(83, 13);
            this.lblValueName.TabIndex = 0;
            this.lblValueName.Text = "Estimates Field: ";
            // 
            // lvSymbol
            // 
            this.lvSymbol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBlank,
            this.colRange,
            this.colEval,
            this.colCnt,
            this.colSS,
            this.colSB,
            this.colMS,
            this.colMB});
            this.lvSymbol.Location = new System.Drawing.Point(255, 34);
            this.lvSymbol.Name = "lvSymbol";
            this.lvSymbol.Size = new System.Drawing.Size(492, 119);
            this.lvSymbol.TabIndex = 130;
            this.lvSymbol.UseCompatibleStateImageBehavior = false;
            this.lvSymbol.View = System.Windows.Forms.View.Details;
            this.lvSymbol.Visible = false;
            // 
            // colBlank
            // 
            this.colBlank.Text = "";
            this.colBlank.Width = 10;
            // 
            // colRange
            // 
            this.colRange.Text = "Range";
            this.colRange.Width = 69;
            // 
            // colEval
            // 
            this.colEval.Text = "Robustness";
            this.colEval.Width = 76;
            // 
            // colCnt
            // 
            this.colCnt.Text = "Count";
            // 
            // colSS
            // 
            this.colSS.Text = "SS";
            // 
            // colSB
            // 
            this.colSB.Text = "SB";
            // 
            // colMS
            // 
            this.colMS.Text = "MS";
            // 
            // colMB
            // 
            this.colMB.Text = "MB";
            this.colMB.Width = 78;
            // 
            // btnEval
            // 
            this.btnEval.Location = new System.Drawing.Point(12, 256);
            this.btnEval.Name = "btnEval";
            this.btnEval.Size = new System.Drawing.Size(96, 23);
            this.btnEval.TabIndex = 131;
            this.btnEval.Text = "Evaluation";
            this.btnEval.UseVisualStyleBackColor = true;
            this.btnEval.Click += new System.EventHandler(this.btnEval_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(168, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 23);
            this.btnCancel.TabIndex = 132;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grbSave
            // 
            this.grbSave.Controls.Add(this.chkAddMap);
            this.grbSave.Controls.Add(this.label2);
            this.grbSave.Controls.Add(this.cboMeasure);
            this.grbSave.Controls.Add(this.lblFldName);
            this.grbSave.Controls.Add(this.txtFldName);
            this.grbSave.Location = new System.Drawing.Point(12, 148);
            this.grbSave.Name = "grbSave";
            this.grbSave.Size = new System.Drawing.Size(252, 102);
            this.grbSave.TabIndex = 138;
            this.grbSave.TabStop = false;
            this.grbSave.Text = "Setting";
            // 
            // chkAddMap
            // 
            this.chkAddMap.AutoSize = true;
            this.chkAddMap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAddMap.Checked = true;
            this.chkAddMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddMap.Location = new System.Drawing.Point(18, 79);
            this.chkAddMap.Name = "chkAddMap";
            this.chkAddMap.Size = new System.Drawing.Size(69, 17);
            this.chkAddMap.TabIndex = 141;
            this.chkAddMap.Text = "Add Map";
            this.chkAddMap.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 140;
            this.label2.Text = "Measure :";
            // 
            // cboMeasure
            // 
            this.cboMeasure.FormattingEnabled = true;
            this.cboMeasure.Items.AddRange(new object[] {
            "Entropy",
            "Robustness"});
            this.cboMeasure.Location = new System.Drawing.Point(89, 51);
            this.cboMeasure.Name = "cboMeasure";
            this.cboMeasure.Size = new System.Drawing.Size(142, 21);
            this.cboMeasure.TabIndex = 139;
            this.cboMeasure.Text = "Entropy";
            this.cboMeasure.SelectedIndexChanged += new System.EventHandler(this.cboMeasure_SelectedIndexChanged);
            // 
            // lblFldName
            // 
            this.lblFldName.AutoSize = true;
            this.lblFldName.Location = new System.Drawing.Point(15, 25);
            this.lblFldName.Name = "lblFldName";
            this.lblFldName.Size = new System.Drawing.Size(63, 13);
            this.lblFldName.TabIndex = 138;
            this.lblFldName.Text = "Save Field :";
            // 
            // txtFldName
            // 
            this.txtFldName.Location = new System.Drawing.Point(89, 22);
            this.txtFldName.Name = "txtFldName";
            this.txtFldName.Size = new System.Drawing.Size(142, 20);
            this.txtFldName.TabIndex = 137;
            this.txtFldName.Text = "Rob";
            // 
            // frmEvalRobustness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(290, 302);
            this.Controls.Add(this.grbSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvSymbol);
            this.Controls.Add(this.btnEval);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEvalRobustness";
            this.Text = "Evaluation of a choropleth map";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbSave.ResumeLayout(false);
            this.grbSave.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblUncernNM;
        private System.Windows.Forms.Label lblValueName;
        private System.Windows.Forms.ListView lvSymbol;
        private System.Windows.Forms.ColumnHeader colBlank;
        private System.Windows.Forms.ColumnHeader colRange;
        private System.Windows.Forms.ColumnHeader colEval;
        private System.Windows.Forms.Button btnEval;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cboUncernFld;
        private System.Windows.Forms.ColumnHeader colCnt;
        private System.Windows.Forms.ColumnHeader colSS;
        private System.Windows.Forms.ColumnHeader colSB;
        private System.Windows.Forms.ColumnHeader colMS;
        private System.Windows.Forms.ColumnHeader colMB;
        private System.Windows.Forms.GroupBox grbSave;
        private System.Windows.Forms.CheckBox chkAddMap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboMeasure;
        private System.Windows.Forms.Label lblFldName;
        private System.Windows.Forms.TextBox txtFldName;
    }
}