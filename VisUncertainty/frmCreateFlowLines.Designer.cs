namespace VisUncertainty
{
    partial class frmCreateFlowLines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateFlowLines));
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenFlow = new System.Windows.Forms.Button();
            this.txtFlowFile = new System.Windows.Forms.TextBox();
            this.grbSpInfo = new System.Windows.Forms.GroupBox();
            this.cbo4 = new System.Windows.Forms.ComboBox();
            this.lbl4 = new System.Windows.Forms.Label();
            this.cbo3 = new System.Windows.Forms.ComboBox();
            this.lbl3 = new System.Windows.Forms.Label();
            this.cbo2 = new System.Windows.Forms.ComboBox();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.cbo1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSpatialInfo = new System.Windows.Forms.ComboBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ofdOpenDBF = new System.Windows.Forms.OpenFileDialog();
            this.sfdSaveShp = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUnselect = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.clistFields = new System.Windows.Forms.CheckedListBox();
            this.grbSpInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Flow File (*.dbf): ";
            // 
            // btnOpenFlow
            // 
            this.btnOpenFlow.Location = new System.Drawing.Point(228, 25);
            this.btnOpenFlow.Name = "btnOpenFlow";
            this.btnOpenFlow.Size = new System.Drawing.Size(37, 23);
            this.btnOpenFlow.TabIndex = 2;
            this.btnOpenFlow.Text = "...";
            this.btnOpenFlow.UseVisualStyleBackColor = true;
            this.btnOpenFlow.Click += new System.EventHandler(this.btnOpenFlow_Click);
            // 
            // txtFlowFile
            // 
            this.txtFlowFile.Location = new System.Drawing.Point(15, 27);
            this.txtFlowFile.Name = "txtFlowFile";
            this.txtFlowFile.Size = new System.Drawing.Size(207, 20);
            this.txtFlowFile.TabIndex = 3;
            this.txtFlowFile.TextChanged += new System.EventHandler(this.txtFlowFile_TextChanged);
            // 
            // grbSpInfo
            // 
            this.grbSpInfo.Controls.Add(this.cbo4);
            this.grbSpInfo.Controls.Add(this.lbl4);
            this.grbSpInfo.Controls.Add(this.cbo3);
            this.grbSpInfo.Controls.Add(this.lbl3);
            this.grbSpInfo.Controls.Add(this.cbo2);
            this.grbSpInfo.Controls.Add(this.lbl2);
            this.grbSpInfo.Controls.Add(this.lbl1);
            this.grbSpInfo.Controls.Add(this.cbo1);
            this.grbSpInfo.Location = new System.Drawing.Point(15, 81);
            this.grbSpInfo.Name = "grbSpInfo";
            this.grbSpInfo.Size = new System.Drawing.Size(250, 131);
            this.grbSpInfo.TabIndex = 4;
            this.grbSpInfo.TabStop = false;
            this.grbSpInfo.Text = "From Table";
            // 
            // cbo4
            // 
            this.cbo4.FormattingEnabled = true;
            this.cbo4.Location = new System.Drawing.Point(125, 100);
            this.cbo4.Name = "cbo4";
            this.cbo4.Size = new System.Drawing.Size(115, 21);
            this.cbo4.TabIndex = 7;
            // 
            // lbl4
            // 
            this.lbl4.AutoSize = true;
            this.lbl4.Location = new System.Drawing.Point(6, 103);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(104, 13);
            this.lbl4.TabIndex = 6;
            this.lbl4.Text = "Destination Y Coord:";
            // 
            // cbo3
            // 
            this.cbo3.FormattingEnabled = true;
            this.cbo3.Location = new System.Drawing.Point(125, 73);
            this.cbo3.Name = "cbo3";
            this.cbo3.Size = new System.Drawing.Size(115, 21);
            this.cbo3.TabIndex = 5;
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.Location = new System.Drawing.Point(6, 76);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(104, 13);
            this.lbl3.TabIndex = 4;
            this.lbl3.Text = "Destination X Coord:";
            // 
            // cbo2
            // 
            this.cbo2.FormattingEnabled = true;
            this.cbo2.Location = new System.Drawing.Point(125, 47);
            this.cbo2.Name = "cbo2";
            this.cbo2.Size = new System.Drawing.Size(115, 21);
            this.cbo2.TabIndex = 3;
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(6, 50);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(78, 13);
            this.lbl2.TabIndex = 2;
            this.lbl2.Text = "Origin Y Coord:";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(6, 23);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(78, 13);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "Origin X Coord:";
            // 
            // cbo1
            // 
            this.cbo1.FormattingEnabled = true;
            this.cbo1.Location = new System.Drawing.Point(125, 20);
            this.cbo1.Name = "cbo1";
            this.cbo1.Size = new System.Drawing.Size(115, 21);
            this.cbo1.TabIndex = 0;
            this.cbo1.SelectedIndexChanged += new System.EventHandler(this.cbo1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Spatial Information:";
            // 
            // cboSpatialInfo
            // 
            this.cboSpatialInfo.FormattingEnabled = true;
            this.cboSpatialInfo.Items.AddRange(new object[] {
            "From ShapeFile",
            "From Table"});
            this.cboSpatialInfo.Location = new System.Drawing.Point(116, 54);
            this.cboSpatialInfo.Name = "cboSpatialInfo";
            this.cboSpatialInfo.Size = new System.Drawing.Size(149, 21);
            this.cboSpatialInfo.TabIndex = 6;
            this.cboSpatialInfo.Text = "From Table";
            this.cboSpatialInfo.SelectedIndexChanged += new System.EventHandler(this.cboSpatialInfo_SelectedIndexChanged);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(15, 234);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(216, 20);
            this.txtOutput.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(237, 232);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Output file (*.shp): ";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(24, 423);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 10;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ofdOpenDBF
            // 
            this.ofdOpenDBF.Filter = "dBase file|*.dbf";
            this.ofdOpenDBF.Title = "Open Flow File (*.dbf)";
            // 
            // sfdSaveShp
            // 
            this.sfdSaveShp.Filter = "Shapefiles | *.shp";
            this.sfdSaveShp.Title = "Save Output Shape File";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUnselect);
            this.groupBox1.Controls.Add(this.btnSelectAll);
            this.groupBox1.Controls.Add(this.clistFields);
            this.groupBox1.Location = new System.Drawing.Point(12, 260);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 150);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Fields";
            // 
            // btnUnselect
            // 
            this.btnUnselect.Location = new System.Drawing.Point(92, 119);
            this.btnUnselect.Name = "btnUnselect";
            this.btnUnselect.Size = new System.Drawing.Size(75, 23);
            this.btnUnselect.TabIndex = 26;
            this.btnUnselect.Text = "Unselect All";
            this.btnUnselect.UseVisualStyleBackColor = true;
            this.btnUnselect.Click += new System.EventHandler(this.btnUnselect_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(11, 119);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 25;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // clistFields
            // 
            this.clistFields.FormattingEnabled = true;
            this.clistFields.Location = new System.Drawing.Point(6, 19);
            this.clistFields.Name = "clistFields";
            this.clistFields.Size = new System.Drawing.Size(237, 94);
            this.clistFields.TabIndex = 24;
            // 
            // frmCreateFlowLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 458);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboSpatialInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grbSpInfo);
            this.Controls.Add(this.txtFlowFile);
            this.Controls.Add(this.btnOpenFlow);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCreateFlowLines";
            this.Text = "Create O-D Flow lines";
            this.grbSpInfo.ResumeLayout(false);
            this.grbSpInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFlow;
        private System.Windows.Forms.TextBox txtFlowFile;
        private System.Windows.Forms.GroupBox grbSpInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSpatialInfo;
        private System.Windows.Forms.ComboBox cbo4;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.ComboBox cbo3;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.ComboBox cbo2;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.ComboBox cbo1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog ofdOpenDBF;
        private System.Windows.Forms.SaveFileDialog sfdSaveShp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnUnselect;
        private System.Windows.Forms.Button btnSelectAll;
        public System.Windows.Forms.CheckedListBox clistFields;
    }
}