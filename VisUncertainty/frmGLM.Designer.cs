namespace VisUncertainty
{
    partial class frmGLM
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
            "Residuals",
            "glm_resi"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGLM));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboFieldName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.lstIndeVar = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.cboFamily = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboNormalization = new System.Windows.Forms.ComboBox();
            this.lblNorm = new System.Windows.Forms.Label();
            this.grbSave = new System.Windows.Forms.GroupBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cboResiType = new System.Windows.Forms.ComboBox();
            this.lstSave = new System.Windows.Forms.ListView();
            this.colTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkResiAuto = new System.Windows.Forms.CheckBox();
            this.btnOpenSWM = new System.Windows.Forms.Button();
            this.txtSWM = new System.Windows.Forms.TextBox();
            this.lblSWM = new System.Windows.Forms.Label();
            this.grbSave.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(462, 285);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 23);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(303, 285);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(115, 23);
            this.btnRun.TabIndex = 34;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(163, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "Independent Variables";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Fields";
            // 
            // cboFieldName
            // 
            this.cboFieldName.FormattingEnabled = true;
            this.cboFieldName.Location = new System.Drawing.Point(13, 109);
            this.cboFieldName.Name = "cboFieldName";
            this.cboFieldName.Size = new System.Drawing.Size(265, 21);
            this.cboFieldName.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Dependent Variable:";
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Location = new System.Drawing.Point(132, 243);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(28, 23);
            this.btnMoveLeft.TabIndex = 29;
            this.btnMoveLeft.Text = "<";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Location = new System.Drawing.Point(132, 214);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(28, 23);
            this.btnMoveRight.TabIndex = 28;
            this.btnMoveRight.Text = ">";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // lstFields
            // 
            this.lstFields.FormattingEnabled = true;
            this.lstFields.Location = new System.Drawing.Point(13, 198);
            this.lstFields.Name = "lstFields";
            this.lstFields.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFields.Size = new System.Drawing.Size(113, 108);
            this.lstFields.TabIndex = 27;
            this.lstFields.DoubleClick += new System.EventHandler(this.lstFields_DoubleClick);
            // 
            // lstIndeVar
            // 
            this.lstIndeVar.FormattingEnabled = true;
            this.lstIndeVar.Location = new System.Drawing.Point(166, 201);
            this.lstIndeVar.Name = "lstIndeVar";
            this.lstIndeVar.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstIndeVar.Size = new System.Drawing.Size(113, 108);
            this.lstIndeVar.TabIndex = 26;
            this.lstIndeVar.DoubleClick += new System.EventHandler(this.lstIndeVar_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(13, 25);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(266, 21);
            this.cboTargetLayer.TabIndex = 24;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // cboFamily
            // 
            this.cboFamily.FormattingEnabled = true;
            this.cboFamily.Items.AddRange(new object[] {
            "Poisson",
            "Binomial",
            "Logistic"});
            this.cboFamily.Location = new System.Drawing.Point(13, 66);
            this.cboFamily.Name = "cboFamily";
            this.cboFamily.Size = new System.Drawing.Size(265, 21);
            this.cboFamily.TabIndex = 60;
            this.cboFamily.Text = "Poisson";
            this.cboFamily.SelectedIndexChanged += new System.EventHandler(this.cboFamily_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 59;
            this.label5.Text = "Family:";
            // 
            // cboNormalization
            // 
            this.cboNormalization.FormattingEnabled = true;
            this.cboNormalization.Location = new System.Drawing.Point(13, 152);
            this.cboNormalization.Name = "cboNormalization";
            this.cboNormalization.Size = new System.Drawing.Size(265, 21);
            this.cboNormalization.TabIndex = 62;
            // 
            // lblNorm
            // 
            this.lblNorm.AutoSize = true;
            this.lblNorm.Location = new System.Drawing.Point(14, 136);
            this.lblNorm.Name = "lblNorm";
            this.lblNorm.Size = new System.Drawing.Size(86, 13);
            this.lblNorm.TabIndex = 61;
            this.lblNorm.Text = "Offset (Optional):";
            // 
            // grbSave
            // 
            this.grbSave.Controls.Add(this.lblType);
            this.grbSave.Controls.Add(this.cboResiType);
            this.grbSave.Controls.Add(this.lstSave);
            this.grbSave.Controls.Add(this.chkSave);
            this.grbSave.Location = new System.Drawing.Point(303, 140);
            this.grbSave.Name = "grbSave";
            this.grbSave.Size = new System.Drawing.Size(274, 130);
            this.grbSave.TabIndex = 71;
            this.grbSave.TabStop = false;
            this.grbSave.Text = "Save";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Enabled = false;
            this.lblType.Location = new System.Drawing.Point(13, 41);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 59;
            this.lblType.Text = "Type:";
            // 
            // cboResiType
            // 
            this.cboResiType.Enabled = false;
            this.cboResiType.FormattingEnabled = true;
            this.cboResiType.Items.AddRange(new object[] {
            "deviance",
            "pearson"});
            this.cboResiType.Location = new System.Drawing.Point(61, 37);
            this.cboResiType.Name = "cboResiType";
            this.cboResiType.Size = new System.Drawing.Size(194, 21);
            this.cboResiType.TabIndex = 58;
            this.cboResiType.Text = "pearson";
            // 
            // lstSave
            // 
            this.lstSave.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTypes,
            this.colNames});
            this.lstSave.Enabled = false;
            this.lstSave.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstSave.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lstSave.LabelEdit = true;
            this.lstSave.Location = new System.Drawing.Point(12, 64);
            this.lstSave.Name = "lstSave";
            this.lstSave.Size = new System.Drawing.Size(244, 55);
            this.lstSave.TabIndex = 57;
            this.lstSave.UseCompatibleStateImageBehavior = false;
            this.lstSave.View = System.Windows.Forms.View.Details;
            this.lstSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstSave_MouseUp);
            // 
            // colTypes
            // 
            this.colTypes.Text = "Types";
            this.colTypes.Width = 103;
            // 
            // colNames
            // 
            this.colNames.Text = "Field Name";
            this.colNames.Width = 105;
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(12, 15);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(146, 17);
            this.chkSave.TabIndex = 56;
            this.chkSave.Text = "Save regression residuals";
            this.chkSave.UseVisualStyleBackColor = true;
            this.chkSave.CheckedChanged += new System.EventHandler(this.chkSave_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkResiAuto);
            this.groupBox1.Controls.Add(this.btnOpenSWM);
            this.groupBox1.Controls.Add(this.txtSWM);
            this.groupBox1.Controls.Add(this.lblSWM);
            this.groupBox1.Location = new System.Drawing.Point(303, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 90);
            this.groupBox1.TabIndex = 91;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Residual analysis";
            // 
            // chkResiAuto
            // 
            this.chkResiAuto.AutoSize = true;
            this.chkResiAuto.Checked = true;
            this.chkResiAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResiAuto.Location = new System.Drawing.Point(12, 22);
            this.chkResiAuto.Name = "chkResiAuto";
            this.chkResiAuto.Size = new System.Drawing.Size(220, 17);
            this.chkResiAuto.TabIndex = 24;
            this.chkResiAuto.Text = "Calculate Moran Coefficient for Residuals";
            this.chkResiAuto.UseVisualStyleBackColor = true;
            this.chkResiAuto.CheckedChanged += new System.EventHandler(this.chkResiAuto_CheckedChanged);
            // 
            // btnOpenSWM
            // 
            this.btnOpenSWM.Location = new System.Drawing.Point(226, 57);
            this.btnOpenSWM.Name = "btnOpenSWM";
            this.btnOpenSWM.Size = new System.Drawing.Size(29, 23);
            this.btnOpenSWM.TabIndex = 89;
            this.btnOpenSWM.Text = "...";
            this.btnOpenSWM.UseVisualStyleBackColor = true;
            this.btnOpenSWM.Click += new System.EventHandler(this.btnOpenSWM_Click);
            // 
            // txtSWM
            // 
            this.txtSWM.Location = new System.Drawing.Point(12, 60);
            this.txtSWM.Name = "txtSWM";
            this.txtSWM.Size = new System.Drawing.Size(208, 20);
            this.txtSWM.TabIndex = 88;
            this.txtSWM.Text = "Default";
            // 
            // lblSWM
            // 
            this.lblSWM.AutoSize = true;
            this.lblSWM.Location = new System.Drawing.Point(12, 44);
            this.lblSWM.Name = "lblSWM";
            this.lblSWM.Size = new System.Drawing.Size(107, 13);
            this.lblSWM.TabIndex = 87;
            this.lblSWM.Text = "Spatial Weight Matrix";
            // 
            // frmGLM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(596, 321);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grbSave);
            this.Controls.Add(this.cboNormalization);
            this.Controls.Add(this.lblNorm);
            this.Controls.Add(this.cboFamily);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboFieldName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnMoveLeft);
            this.Controls.Add(this.btnMoveRight);
            this.Controls.Add(this.lstFields);
            this.Controls.Add(this.lstIndeVar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGLM";
            this.Text = "Generalized Linear Models";
            this.grbSave.ResumeLayout(false);
            this.grbSave.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboFieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.ListBox lstFields;
        private System.Windows.Forms.ListBox lstIndeVar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.ComboBox cboFamily;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboNormalization;
        private System.Windows.Forms.Label lblNorm;
        private System.Windows.Forms.GroupBox grbSave;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboResiType;
        private System.Windows.Forms.ListView lstSave;
        private System.Windows.Forms.ColumnHeader colTypes;
        private System.Windows.Forms.ColumnHeader colNames;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkResiAuto;
        private System.Windows.Forms.Button btnOpenSWM;
        private System.Windows.Forms.TextBox txtSWM;
        private System.Windows.Forms.Label lblSWM;
    }
}