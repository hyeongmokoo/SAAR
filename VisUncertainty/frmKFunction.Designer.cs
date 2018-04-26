namespace VisUncertainty
{
    partial class frmKFunction
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKFunction));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grbModelling = new System.Windows.Forms.GroupBox();
            this.nudNPermutation = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboModels = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEdgeCorrection = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboStudyArea = new System.Windows.Forms.ComboBox();
            this.nudNDistBands = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.chkConfBnd = new System.Windows.Forms.CheckBox();
            this.nudDistInc = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudBeginDist = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.conMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.grbModelling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNPermutation)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNDistBands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistInc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBeginDist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.conMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.grbModelling);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboTargetLayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 353);
            this.panel1.TabIndex = 39;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(181, 319);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 21);
            this.btnCancel.TabIndex = 45;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(23, 319);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 21);
            this.btnOK.TabIndex = 44;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grbModelling
            // 
            this.grbModelling.Controls.Add(this.nudNPermutation);
            this.grbModelling.Controls.Add(this.label5);
            this.grbModelling.Controls.Add(this.label6);
            this.grbModelling.Controls.Add(this.cboModels);
            this.grbModelling.Enabled = false;
            this.grbModelling.Location = new System.Drawing.Point(13, 233);
            this.grbModelling.Name = "grbModelling";
            this.grbModelling.Size = new System.Drawing.Size(273, 79);
            this.grbModelling.TabIndex = 42;
            this.grbModelling.TabStop = false;
            this.grbModelling.Text = "Models for Spatial Point Patterns";
            // 
            // nudNPermutation
            // 
            this.nudNPermutation.Location = new System.Drawing.Point(168, 53);
            this.nudNPermutation.Name = "nudNPermutation";
            this.nudNPermutation.Size = new System.Drawing.Size(98, 21);
            this.nudNPermutation.TabIndex = 42;
            this.nudNPermutation.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "Models:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 12);
            this.label6.TabIndex = 41;
            this.label6.Text = "Number of Permutation:";
            // 
            // cboModels
            // 
            this.cboModels.FormattingEnabled = true;
            this.cboModels.Items.AddRange(new object[] {
            "Complete Spatial Randomness"});
            this.cboModels.Location = new System.Drawing.Point(10, 30);
            this.cboModels.Name = "cboModels";
            this.cboModels.Size = new System.Drawing.Size(255, 20);
            this.cboModels.TabIndex = 32;
            this.cboModels.Text = "Complete Spatial Randomness";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkEdgeCorrection);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cboStudyArea);
            this.groupBox1.Controls.Add(this.nudNDistBands);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.chkConfBnd);
            this.groupBox1.Controls.Add(this.nudDistInc);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudBeginDist);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(13, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 174);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bandwidth";
            // 
            // chkEdgeCorrection
            // 
            this.chkEdgeCorrection.AutoSize = true;
            this.chkEdgeCorrection.Enabled = false;
            this.chkEdgeCorrection.Location = new System.Drawing.Point(10, 128);
            this.chkEdgeCorrection.Name = "chkEdgeCorrection";
            this.chkEdgeCorrection.Size = new System.Drawing.Size(115, 16);
            this.chkEdgeCorrection.TabIndex = 47;
            this.chkEdgeCorrection.Text = "Edge Correction";
            this.chkEdgeCorrection.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 46;
            this.label7.Text = "Study Area:";
            // 
            // cboStudyArea
            // 
            this.cboStudyArea.FormattingEnabled = true;
            this.cboStudyArea.Items.AddRange(new object[] {
            "Minimum Enclosing Rectangle"});
            this.cboStudyArea.Location = new System.Drawing.Point(7, 103);
            this.cboStudyArea.Name = "cboStudyArea";
            this.cboStudyArea.Size = new System.Drawing.Size(255, 20);
            this.cboStudyArea.TabIndex = 45;
            this.cboStudyArea.Text = "Minimum Enclosing Rectangle ";
            this.cboStudyArea.SelectedIndexChanged += new System.EventHandler(this.cboStudyArea_SelectedIndexChanged);
            // 
            // nudNDistBands
            // 
            this.nudNDistBands.Location = new System.Drawing.Point(168, 66);
            this.nudNDistBands.Name = "nudNDistBands";
            this.nudNDistBands.Size = new System.Drawing.Size(98, 21);
            this.nudNDistBands.TabIndex = 40;
            this.nudNDistBands.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "Number of Distance Band:";
            // 
            // chkConfBnd
            // 
            this.chkConfBnd.AutoSize = true;
            this.chkConfBnd.Location = new System.Drawing.Point(10, 150);
            this.chkConfBnd.Name = "chkConfBnd";
            this.chkConfBnd.Size = new System.Drawing.Size(161, 16);
            this.chkConfBnd.TabIndex = 43;
            this.chkConfBnd.Text = "Add Confidence Bounds";
            this.chkConfBnd.UseVisualStyleBackColor = true;
            this.chkConfBnd.CheckedChanged += new System.EventHandler(this.chkConfBnd_CheckedChanged);
            // 
            // nudDistInc
            // 
            this.nudDistInc.Location = new System.Drawing.Point(131, 42);
            this.nudDistInc.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDistInc.Name = "nudDistInc";
            this.nudDistInc.Size = new System.Drawing.Size(135, 21);
            this.nudDistInc.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "Distance Inclement:";
            // 
            // nudBeginDist
            // 
            this.nudBeginDist.Location = new System.Drawing.Point(131, 18);
            this.nudBeginDist.Name = "nudBeginDist";
            this.nudBeginDist.Size = new System.Drawing.Size(135, 21);
            this.nudBeginDist.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "Beginning Distance:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 40;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(23, 27);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(255, 20);
            this.cboTargetLayer.TabIndex = 39;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // pChart
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.pChart.ChartAreas.Add(chartArea1);
            this.pChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.pChart.Legends.Add(legend1);
            this.pChart.Location = new System.Drawing.Point(304, 0);
            this.pChart.Name = "pChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.pChart.Series.Add(series1);
            this.pChart.Size = new System.Drawing.Size(493, 353);
            this.pChart.TabIndex = 40;
            this.pChart.Text = "chart1";
            this.pChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseDown);
            this.pChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseMove);
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // conMenu
            // 
            this.conMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToImageFileToolStripMenuItem});
            this.conMenu.Name = "conMenu";
            this.conMenu.Size = new System.Drawing.Size(181, 26);
            // 
            // exportToImageFileToolStripMenuItem
            // 
            this.exportToImageFileToolStripMenuItem.Name = "exportToImageFileToolStripMenuItem";
            this.exportToImageFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportToImageFileToolStripMenuItem.Text = "Export to image file";
            this.exportToImageFileToolStripMenuItem.Click += new System.EventHandler(this.exportToImageFileToolStripMenuItem_Click);
            // 
            // frmKFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 353);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKFunction";
            this.Text = "L-Function";
            this.Load += new System.EventHandler(this.frmKFunction_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grbModelling.ResumeLayout(false);
            this.grbModelling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNPermutation)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNDistBands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistInc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBeginDist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.conMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkConfBnd;
        private System.Windows.Forms.GroupBox grbModelling;
        private System.Windows.Forms.NumericUpDown nudNPermutation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboModels;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudNDistBands;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudDistInc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudBeginDist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboStudyArea;
        private System.Windows.Forms.CheckBox chkEdgeCorrection;
        private System.Windows.Forms.ContextMenuStrip conMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToImageFileToolStripMenuItem;

    }
}