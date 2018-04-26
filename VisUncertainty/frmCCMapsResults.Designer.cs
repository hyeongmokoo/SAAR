namespace VisUncertainty
{
    partial class frmCCMapsResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCCMapsResults));
            this.pnMapCntrls = new System.Windows.Forms.Panel();
            this.pnMapSetting = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudOutlinewidth = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.picOutlineColor = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picBGColor = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboGCClassify = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudGCNClasses = new System.Windows.Forms.NumericUpDown();
            this.cboColorRamp = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lvSymbol = new System.Windows.Forms.ListView();
            this.colBlank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSymbol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblVariable = new System.Windows.Forms.Label();
            this.pnVert = new System.Windows.Forms.Panel();
            this.lblVerMin = new System.Windows.Forms.Label();
            this.lblVertMax = new System.Windows.Forms.Label();
            this.lblVert = new System.Windows.Forms.Label();
            this.pnHori = new System.Windows.Forms.Panel();
            this.lblHoriMax = new System.Windows.Forms.Label();
            this.lblHori = new System.Windows.Forms.Label();
            this.axTools = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            this.pnMapSetting.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOutlinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOutlineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBGColor)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            this.pnVert.SuspendLayout();
            this.pnHori.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTools)).BeginInit();
            this.SuspendLayout();
            // 
            // pnMapCntrls
            // 
            this.pnMapCntrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMapCntrls.Location = new System.Drawing.Point(0, 58);
            this.pnMapCntrls.Name = "pnMapCntrls";
            this.pnMapCntrls.Size = new System.Drawing.Size(840, 362);
            this.pnMapCntrls.TabIndex = 2;
            this.pnMapCntrls.Resize += new System.EventHandler(this.pnMapCntrls_Resize);
            // 
            // pnMapSetting
            // 
            this.pnMapSetting.Controls.Add(this.btnExport);
            this.pnMapSetting.Controls.Add(this.groupBox2);
            this.pnMapSetting.Controls.Add(this.groupBox1);
            this.pnMapSetting.Controls.Add(this.btnUpdate);
            this.pnMapSetting.Controls.Add(this.lvSymbol);
            this.pnMapSetting.Controls.Add(this.lblVariable);
            this.pnMapSetting.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnMapSetting.Location = new System.Drawing.Point(0, 420);
            this.pnMapSetting.Name = "pnMapSetting";
            this.pnMapSetting.Size = new System.Drawing.Size(951, 139);
            this.pnMapSetting.TabIndex = 3;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(666, 42);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(118, 23);
            this.btnExport.TabIndex = 175;
            this.btnExport.Text = "Export Maps";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nudOutlinewidth);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.picOutlineColor);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.picBGColor);
            this.groupBox2.Location = new System.Drawing.Point(462, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(185, 129);
            this.groupBox2.TabIndex = 174;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Other display options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 170;
            this.label3.Text = "Outline width:";
            // 
            // nudOutlinewidth
            // 
            this.nudOutlinewidth.DecimalPlaces = 1;
            this.nudOutlinewidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudOutlinewidth.Location = new System.Drawing.Point(128, 78);
            this.nudOutlinewidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudOutlinewidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.nudOutlinewidth.Name = "nudOutlinewidth";
            this.nudOutlinewidth.Size = new System.Drawing.Size(46, 20);
            this.nudOutlinewidth.TabIndex = 171;
            this.nudOutlinewidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 168;
            this.label1.Text = "Outline Color: ";
            // 
            // picOutlineColor
            // 
            this.picOutlineColor.BackColor = System.Drawing.Color.DarkGray;
            this.picOutlineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picOutlineColor.Location = new System.Drawing.Point(128, 51);
            this.picOutlineColor.Name = "picOutlineColor";
            this.picOutlineColor.Size = new System.Drawing.Size(46, 21);
            this.picOutlineColor.TabIndex = 169;
            this.picOutlineColor.TabStop = false;
            this.picOutlineColor.Click += new System.EventHandler(this.picOutlineColor_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 13);
            this.label2.TabIndex = 165;
            this.label2.Text = "Background Map Color: ";
            // 
            // picBGColor
            // 
            this.picBGColor.BackColor = System.Drawing.Color.LightGray;
            this.picBGColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picBGColor.Location = new System.Drawing.Point(128, 22);
            this.picBGColor.Name = "picBGColor";
            this.picBGColor.Size = new System.Drawing.Size(46, 21);
            this.picBGColor.TabIndex = 167;
            this.picBGColor.TabStop = false;
            this.picBGColor.Click += new System.EventHandler(this.picBGColor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboGCClassify);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nudGCNClasses);
            this.groupBox1.Controls.Add(this.cboColorRamp);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(282, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 128);
            this.groupBox1.TabIndex = 173;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choropleth map setting";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 168;
            this.label6.Text = "Number of Class:";
            // 
            // cboGCClassify
            // 
            this.cboGCClassify.FormattingEnabled = true;
            this.cboGCClassify.Items.AddRange(new object[] {
            "Equal Interval",
            "Geometrical Interval",
            "Natural Breaks",
            "Quantile",
            "StandardDeviation"});
            this.cboGCClassify.Location = new System.Drawing.Point(9, 58);
            this.cboGCClassify.Name = "cboGCClassify";
            this.cboGCClassify.Size = new System.Drawing.Size(155, 21);
            this.cboGCClassify.TabIndex = 172;
            this.cboGCClassify.Text = "Quantile";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 171;
            this.label5.Text = "Classification Method:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(99, 18);
            this.nudGCNClasses.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudGCNClasses.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudGCNClasses.Name = "nudGCNClasses";
            this.nudGCNClasses.Size = new System.Drawing.Size(55, 20);
            this.nudGCNClasses.TabIndex = 169;
            this.nudGCNClasses.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
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
            this.cboColorRamp.Location = new System.Drawing.Point(11, 99);
            this.cboColorRamp.Name = "cboColorRamp";
            this.cboColorRamp.Size = new System.Drawing.Size(155, 21);
            this.cboColorRamp.TabIndex = 163;
            this.cboColorRamp.Text = "Red Light to Dark";
            this.cboColorRamp.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboColorRamp_DrawItem);
            this.cboColorRamp.SelectedIndexChanged += new System.EventHandler(this.cboColorRamp_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 162;
            this.label8.Text = "Symbol Color Ramps:";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(666, 16);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(118, 23);
            this.btnUpdate.TabIndex = 170;
            this.btnUpdate.Text = "Update Maps";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click_1);
            // 
            // lvSymbol
            // 
            this.lvSymbol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBlank,
            this.colSymbol,
            this.colRange});
            this.lvSymbol.Location = new System.Drawing.Point(12, 27);
            this.lvSymbol.Name = "lvSymbol";
            this.lvSymbol.Size = new System.Drawing.Size(264, 107);
            this.lvSymbol.TabIndex = 161;
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
            // 
            // colRange
            // 
            this.colRange.Text = "Range";
            this.colRange.Width = 172;
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Location = new System.Drawing.Point(12, 7);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(35, 13);
            this.lblVariable.TabIndex = 0;
            this.lblVariable.Text = "label3";
            // 
            // pnVert
            // 
            this.pnVert.Controls.Add(this.lblVerMin);
            this.pnVert.Controls.Add(this.lblVertMax);
            this.pnVert.Controls.Add(this.lblVert);
            this.pnVert.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnVert.Location = new System.Drawing.Point(840, 58);
            this.pnVert.Name = "pnVert";
            this.pnVert.Size = new System.Drawing.Size(111, 362);
            this.pnVert.TabIndex = 4;
            // 
            // lblVerMin
            // 
            this.lblVerMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVerMin.AutoSize = true;
            this.lblVerMin.Location = new System.Drawing.Point(14, 346);
            this.lblVerMin.Name = "lblVerMin";
            this.lblVerMin.Size = new System.Drawing.Size(35, 13);
            this.lblVerMin.TabIndex = 3;
            this.lblVerMin.Text = "label4";
            // 
            // lblVertMax
            // 
            this.lblVertMax.AutoSize = true;
            this.lblVertMax.Location = new System.Drawing.Point(14, 22);
            this.lblVertMax.Name = "lblVertMax";
            this.lblVertMax.Size = new System.Drawing.Size(35, 13);
            this.lblVertMax.TabIndex = 2;
            this.lblVertMax.Text = "label4";
            // 
            // lblVert
            // 
            this.lblVert.AutoSize = true;
            this.lblVert.Location = new System.Drawing.Point(14, 7);
            this.lblVert.Name = "lblVert";
            this.lblVert.Size = new System.Drawing.Size(35, 13);
            this.lblVert.TabIndex = 1;
            this.lblVert.Text = "label4";
            // 
            // pnHori
            // 
            this.pnHori.Controls.Add(this.lblHoriMax);
            this.pnHori.Controls.Add(this.lblHori);
            this.pnHori.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnHori.Location = new System.Drawing.Point(0, 28);
            this.pnHori.Name = "pnHori";
            this.pnHori.Size = new System.Drawing.Size(951, 30);
            this.pnHori.TabIndex = 5;
            // 
            // lblHoriMax
            // 
            this.lblHoriMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHoriMax.AutoSize = true;
            this.lblHoriMax.Location = new System.Drawing.Point(784, 10);
            this.lblHoriMax.Name = "lblHoriMax";
            this.lblHoriMax.Size = new System.Drawing.Size(56, 13);
            this.lblHoriMax.TabIndex = 2;
            this.lblHoriMax.Text = "lblHoriMax";
            // 
            // lblHori
            // 
            this.lblHori.AutoSize = true;
            this.lblHori.Location = new System.Drawing.Point(12, 10);
            this.lblHori.Name = "lblHori";
            this.lblHori.Size = new System.Drawing.Size(35, 13);
            this.lblHori.TabIndex = 1;
            this.lblHori.Text = "label3";
            // 
            // axTools
            // 
            this.axTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.axTools.Location = new System.Drawing.Point(0, 0);
            this.axTools.Name = "axTools";
            this.axTools.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTools.OcxState")));
            this.axTools.Size = new System.Drawing.Size(951, 28);
            this.axTools.TabIndex = 6;
            // 
            // frmCCMapsResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 559);
            this.Controls.Add(this.pnMapCntrls);
            this.Controls.Add(this.pnVert);
            this.Controls.Add(this.pnMapSetting);
            this.Controls.Add(this.pnHori);
            this.Controls.Add(this.axTools);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCCMapsResults";
            this.Text = "frmCCMapsResults";
            this.Load += new System.EventHandler(this.frmCCMapsResults_Load);
            this.pnMapSetting.ResumeLayout(false);
            this.pnMapSetting.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOutlinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOutlineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBGColor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            this.pnVert.ResumeLayout(false);
            this.pnVert.PerformLayout();
            this.pnHori.ResumeLayout(false);
            this.pnHori.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTools)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel pnMapCntrls;
        private System.Windows.Forms.Panel pnMapSetting;
        private System.Windows.Forms.Panel pnVert;
        private System.Windows.Forms.Label lblVariable;
        private System.Windows.Forms.Label lblVert;
        private System.Windows.Forms.ComboBox cboGCClassify;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown nudGCNClasses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picBGColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboColorRamp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView lvSymbol;
        private System.Windows.Forms.ColumnHeader colBlank;
        private System.Windows.Forms.ColumnHeader colSymbol;
        private System.Windows.Forms.ColumnHeader colRange;
        private System.Windows.Forms.Panel pnHori;
        private System.Windows.Forms.Label lblHori;
        private ESRI.ArcGIS.Controls.AxToolbarControl axTools;
        private System.Windows.Forms.ColorDialog cdColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown nudOutlinewidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picOutlineColor;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblVertMax;
        private System.Windows.Forms.Label lblHoriMax;
        private System.Windows.Forms.Label lblVerMin;
        private System.Windows.Forms.Button btnExport;
    }
}