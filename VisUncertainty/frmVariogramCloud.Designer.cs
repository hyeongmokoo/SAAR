namespace VisUncertainty
{
    partial class frmVariogramCloud
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVariogramCloud));
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbSemiVariogram = new System.Windows.Forms.RadioButton();
            this.rdbCloud = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.cboValueFld = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.grbVariogram = new System.Windows.Forms.GroupBox();
            this.btnZoom = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.nudNDistBands = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudDistInc = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudBeginDist = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.conMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbVariogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNDistBands)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistInc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBeginDist)).BeginInit();
            this.conMenu.SuspendLayout();
            this.SuspendLayout();
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
            this.pChart.Location = new System.Drawing.Point(269, 0);
            this.pChart.Name = "pChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.pChart.Series.Add(series1);
            this.pChart.Size = new System.Drawing.Size(528, 384);
            this.pChart.TabIndex = 42;
            this.pChart.Text = "chart1";
            this.pChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseDown);
            this.pChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseMove);
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cboValueFld);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.grbVariogram);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboTargetLayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 384);
            this.panel1.TabIndex = 41;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbSemiVariogram);
            this.groupBox1.Controls.Add(this.rdbCloud);
            this.groupBox1.Location = new System.Drawing.Point(15, 280);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 68);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Brushing with";
            // 
            // rdbSemiVariogram
            // 
            this.rdbSemiVariogram.AutoSize = true;
            this.rdbSemiVariogram.Enabled = false;
            this.rdbSemiVariogram.Location = new System.Drawing.Point(20, 40);
            this.rdbSemiVariogram.Name = "rdbSemiVariogram";
            this.rdbSemiVariogram.Size = new System.Drawing.Size(114, 16);
            this.rdbSemiVariogram.TabIndex = 1;
            this.rdbSemiVariogram.Text = "Semi-variogram";
            this.rdbSemiVariogram.UseVisualStyleBackColor = true;
            this.rdbSemiVariogram.CheckedChanged += new System.EventHandler(this.rdbSemiVariogram_CheckedChanged);
            // 
            // rdbCloud
            // 
            this.rdbCloud.AutoSize = true;
            this.rdbCloud.Checked = true;
            this.rdbCloud.Enabled = false;
            this.rdbCloud.Location = new System.Drawing.Point(20, 18);
            this.rdbCloud.Name = "rdbCloud";
            this.rdbCloud.Size = new System.Drawing.Size(88, 16);
            this.rdbCloud.TabIndex = 0;
            this.rdbCloud.TabStop = true;
            this.rdbCloud.Text = "Point Cloud";
            this.rdbCloud.UseVisualStyleBackColor = true;
            this.rdbCloud.CheckedChanged += new System.EventHandler(this.rdbCloud_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 12);
            this.label5.TabIndex = 47;
            this.label5.Text = "Select a value field:";
            // 
            // cboValueFld
            // 
            this.cboValueFld.FormattingEnabled = true;
            this.cboValueFld.Location = new System.Drawing.Point(23, 67);
            this.cboValueFld.Name = "cboValueFld";
            this.cboValueFld.Size = new System.Drawing.Size(202, 20);
            this.cboValueFld.TabIndex = 46;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(79, 92);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(146, 21);
            this.btnOK.TabIndex = 44;
            this.btnOK.Text = "Add Cloud";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grbVariogram
            // 
            this.grbVariogram.Controls.Add(this.btnZoom);
            this.grbVariogram.Controls.Add(this.btnAdd);
            this.grbVariogram.Controls.Add(this.nudNDistBands);
            this.grbVariogram.Controls.Add(this.label4);
            this.grbVariogram.Controls.Add(this.nudDistInc);
            this.grbVariogram.Controls.Add(this.label3);
            this.grbVariogram.Controls.Add(this.nudBeginDist);
            this.grbVariogram.Controls.Add(this.label2);
            this.grbVariogram.Enabled = false;
            this.grbVariogram.Location = new System.Drawing.Point(13, 124);
            this.grbVariogram.Name = "grbVariogram";
            this.grbVariogram.Size = new System.Drawing.Size(227, 150);
            this.grbVariogram.TabIndex = 41;
            this.grbVariogram.TabStop = false;
            this.grbVariogram.Text = "Semi-variogram";
            // 
            // btnZoom
            // 
            this.btnZoom.Enabled = false;
            this.btnZoom.Location = new System.Drawing.Point(66, 118);
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Size = new System.Drawing.Size(146, 21);
            this.btnZoom.TabIndex = 42;
            this.btnZoom.Text = "Zoom In";
            this.btnZoom.UseVisualStyleBackColor = true;
            this.btnZoom.Click += new System.EventHandler(this.btnZoom_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(66, 89);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(146, 23);
            this.btnAdd.TabIndex = 41;
            this.btnAdd.Text = "Add Semi-variogram";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // nudNDistBands
            // 
            this.nudNDistBands.Location = new System.Drawing.Point(107, 65);
            this.nudNDistBands.Name = "nudNDistBands";
            this.nudNDistBands.Size = new System.Drawing.Size(105, 21);
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
            this.label4.Size = new System.Drawing.Size(96, 12);
            this.label4.TabIndex = 39;
            this.label4.Text = "Number of lags:";
            // 
            // nudDistInc
            // 
            this.nudDistInc.Location = new System.Drawing.Point(107, 41);
            this.nudDistInc.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDistInc.Name = "nudDistInc";
            this.nudDistInc.Size = new System.Drawing.Size(105, 21);
            this.nudDistInc.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "Lag size:";
            // 
            // nudBeginDist
            // 
            this.nudBeginDist.Location = new System.Drawing.Point(107, 17);
            this.nudBeginDist.Name = "nudBeginDist";
            this.nudBeginDist.Size = new System.Drawing.Size(105, 21);
            this.nudBeginDist.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "Nugget:";
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
            this.cboTargetLayer.Size = new System.Drawing.Size(202, 20);
            this.cboTargetLayer.TabIndex = 39;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
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
            // frmVariogramCloud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 384);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmVariogramCloud";
            this.Text = "Variogram Cloud";
            this.Load += new System.EventHandler(this.frmVariogramCloud_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbVariogram.ResumeLayout(false);
            this.grbVariogram.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNDistBands)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistInc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBeginDist)).EndInit();
            this.conMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grbVariogram;
        private System.Windows.Forms.NumericUpDown nudNDistBands;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudDistInc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudBeginDist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboValueFld;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbSemiVariogram;
        private System.Windows.Forms.RadioButton rdbCloud;
        private System.Windows.Forms.Button btnZoom;
        private System.Windows.Forms.ContextMenuStrip conMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToImageFileToolStripMenuItem;
    }
}