namespace VisUncertainty
{
    partial class frmClassSeparability
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new System.Windows.Forms.ListViewItem.ListViewSubItem[] {
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "aa", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 3F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)))),
            new System.Windows.Forms.ListViewItem.ListViewSubItem(null, "", System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 3.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))))}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("aa");
            this.cboUncernFld = new System.Windows.Forms.ComboBox();
            this.lblUncerfld = new System.Windows.Forms.Label();
            this.cboColorRamp = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lvSymbol = new System.Windows.Forms.ListView();
            this.colBlank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSymbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cboAlgorithm = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.picGCLineColor = new System.Windows.Forms.PictureBox();
            this.nudGCLinewidth = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.cboValueField = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudGCNClasses = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGraph = new System.Windows.Forms.Button();
            this.lvSep = new System.Windows.Forms.ListView();
            this.colBlan2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSep = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // cboUncernFld
            // 
            this.cboUncernFld.FormattingEnabled = true;
            this.cboUncernFld.Location = new System.Drawing.Point(370, 68);
            this.cboUncernFld.Name = "cboUncernFld";
            this.cboUncernFld.Size = new System.Drawing.Size(136, 20);
            this.cboUncernFld.TabIndex = 112;
            this.cboUncernFld.SelectedIndexChanged += new System.EventHandler(this.cboUncernFld_SelectedIndexChanged);
            // 
            // lblUncerfld
            // 
            this.lblUncerfld.AutoSize = true;
            this.lblUncerfld.Location = new System.Drawing.Point(264, 72);
            this.lblUncerfld.Name = "lblUncerfld";
            this.lblUncerfld.Size = new System.Drawing.Size(103, 12);
            this.lblUncerfld.TabIndex = 111;
            this.lblUncerfld.Text = "Uncertainty Field:";
            // 
            // cboColorRamp
            // 
            this.cboColorRamp.FormattingEnabled = true;
            this.cboColorRamp.Items.AddRange(new object[] {
            "Blue Light to Dark",
            "Green Light to Dark",
            "Orange Light to Dark",
            "Red Light to Dark",
            "Blue to Red",
            "Green to Purple",
            "Green to Red",
            "Purple to Brown",
            "Custom"});
            this.cboColorRamp.Location = new System.Drawing.Point(143, 96);
            this.cboColorRamp.Name = "cboColorRamp";
            this.cboColorRamp.Size = new System.Drawing.Size(158, 20);
            this.cboColorRamp.TabIndex = 110;
            this.cboColorRamp.Text = "Red Light to Dark";
            this.cboColorRamp.SelectedIndexChanged += new System.EventHandler(this.cboColorRamp_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(420, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 21);
            this.btnCancel.TabIndex = 109;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(325, 320);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(87, 21);
            this.btnApply.TabIndex = 108;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lvSymbol
            // 
            this.lvSymbol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBlank,
            this.colSymbol,
            this.colRange,
            this.colLabel});
            this.lvSymbol.Location = new System.Drawing.Point(17, 151);
            this.lvSymbol.Name = "lvSymbol";
            this.lvSymbol.Size = new System.Drawing.Size(368, 164);
            this.lvSymbol.TabIndex = 107;
            this.lvSymbol.UseCompatibleStateImageBehavior = false;
            this.lvSymbol.View = System.Windows.Forms.View.Details;
            // 
            // colBlank
            // 
            this.colBlank.Text = "";
            this.colBlank.Width = 10;
            // 
            // colSymbol
            // 
            this.colSymbol.Text = "Symbol";
            this.colSymbol.Width = 52;
            // 
            // colRange
            // 
            this.colRange.Text = "Range";
            this.colRange.Width = 117;
            // 
            // colLabel
            // 
            this.colLabel.Text = "Label";
            this.colLabel.Width = 128;
            // 
            // cboAlgorithm
            // 
            this.cboAlgorithm.FormattingEnabled = true;
            this.cboAlgorithm.Items.AddRange(new object[] {
            "HSV",
            "CIE Lab",
            "Lab LCh"});
            this.cboAlgorithm.Location = new System.Drawing.Point(433, 96);
            this.cboAlgorithm.Name = "cboAlgorithm";
            this.cboAlgorithm.Size = new System.Drawing.Size(70, 20);
            this.cboAlgorithm.TabIndex = 106;
            this.cboAlgorithm.Text = "CIE Lab";
            this.cboAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cboAlgorithm_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(309, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 12);
            this.label9.TabIndex = 105;
            this.label9.Text = "Color ramp algorithm:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 12);
            this.label8.TabIndex = 104;
            this.label8.Text = "Symbol Color Ramps:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(199, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 12);
            this.label10.TabIndex = 103;
            this.label10.Text = "Outline Line Color: ";
            // 
            // picGCLineColor
            // 
            this.picGCLineColor.BackColor = System.Drawing.Color.Black;
            this.picGCLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGCLineColor.Location = new System.Drawing.Point(310, 119);
            this.picGCLineColor.Name = "picGCLineColor";
            this.picGCLineColor.Size = new System.Drawing.Size(63, 20);
            this.picGCLineColor.TabIndex = 102;
            this.picGCLineColor.TabStop = false;
            this.picGCLineColor.Click += new System.EventHandler(this.picGCLineColor_Click);
            // 
            // nudGCLinewidth
            // 
            this.nudGCLinewidth.DecimalPlaces = 1;
            this.nudGCLinewidth.Location = new System.Drawing.Point(127, 122);
            this.nudGCLinewidth.Name = "nudGCLinewidth";
            this.nudGCLinewidth.Size = new System.Drawing.Size(64, 21);
            this.nudGCLinewidth.TabIndex = 101;
            this.nudGCLinewidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 125);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 12);
            this.label12.TabIndex = 100;
            this.label12.Text = "Outline Line Width: ";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(120, 69);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(136, 20);
            this.cboValueField.TabIndex = 99;
            this.cboValueField.SelectedIndexChanged += new System.EventHandler(this.cboValueField_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 12);
            this.label5.TabIndex = 98;
            this.label5.Text = "Estimates Field:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(435, 41);
            this.nudGCNClasses.Name = "nudGCNClasses";
            this.nudGCNClasses.Size = new System.Drawing.Size(64, 21);
            this.nudGCNClasses.TabIndex = 95;
            this.nudGCNClasses.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nudGCNClasses.ValueChanged += new System.EventHandler(this.nudGCNClasses_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(334, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 12);
            this.label6.TabIndex = 94;
            this.label6.Text = "Number of Class:";
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(65, 15);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(191, 20);
            this.cboSourceLayer.TabIndex = 114;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 113;
            this.label1.Text = "Layer:";
            // 
            // cboMethod
            // 
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "Class Seperability",
            "Bhattacharyya distance"});
            this.cboMethod.Location = new System.Drawing.Point(149, 44);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(161, 20);
            this.cboMethod.TabIndex = 115;
            this.cboMethod.Text = "Class Seperability";
            this.cboMethod.SelectedIndexChanged += new System.EventHandler(this.cboMethod_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 116;
            this.label2.Text = "Classification Method:";
            // 
            // btnGraph
            // 
            this.btnGraph.Location = new System.Drawing.Point(13, 320);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(87, 21);
            this.btnGraph.TabIndex = 143;
            this.btnGraph.Text = "Show Graph";
            this.btnGraph.UseVisualStyleBackColor = true;
            this.btnGraph.Click += new System.EventHandler(this.btnGraph_Click);
            // 
            // lvSep
            // 
            this.lvSep.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBlan2,
            this.colSep});
            this.lvSep.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvSep.Location = new System.Drawing.Point(391, 157);
            this.lvSep.Name = "lvSep";
            this.lvSep.Size = new System.Drawing.Size(116, 157);
            this.lvSep.TabIndex = 144;
            this.lvSep.UseCompatibleStateImageBehavior = false;
            this.lvSep.View = System.Windows.Forms.View.Details;
            // 
            // colBlan2
            // 
            this.colBlan2.Text = "";
            this.colBlan2.Width = 3;
            // 
            // colSep
            // 
            this.colSep.Text = "Separability";
            this.colSep.Width = 90;
            // 
            // frmClassSeparability
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(520, 353);
            this.Controls.Add(this.lvSep);
            this.Controls.Add(this.btnGraph);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboMethod);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboUncernFld);
            this.Controls.Add(this.lblUncerfld);
            this.Controls.Add(this.cboColorRamp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lvSymbol);
            this.Controls.Add(this.cboAlgorithm);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.picGCLineColor);
            this.Controls.Add(this.nudGCLinewidth);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cboValueField);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudGCNClasses);
            this.Controls.Add(this.label6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClassSeparability";
            this.Text = "Heuristic Classification with Uncertainty";
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboUncernFld;
        private System.Windows.Forms.Label lblUncerfld;
        private System.Windows.Forms.ComboBox cboColorRamp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ListView lvSymbol;
        private System.Windows.Forms.ColumnHeader colBlank;
        private System.Windows.Forms.ColumnHeader colSymbol;
        private System.Windows.Forms.ColumnHeader colRange;
        private System.Windows.Forms.ColumnHeader colLabel;
        private System.Windows.Forms.ComboBox cboAlgorithm;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox picGCLineColor;
        private System.Windows.Forms.NumericUpDown nudGCLinewidth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboValueField;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown nudGCNClasses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColorDialog cdColor;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.ListView lvSep;
        private System.Windows.Forms.ColumnHeader colBlan2;
        private System.Windows.Forms.ColumnHeader colSep;
    }
}