namespace VisUncertainty
{
    partial class frmCorrelogram
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCorrelogram));
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOpenSWM = new System.Windows.Forms.Button();
            this.txtSWM = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudLagOrder = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.cboAssumption = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSAM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboFieldName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.ofdOpenSWM = new System.Windows.Forms.OpenFileDialog();
            this.conMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLagOrder)).BeginInit();
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
            this.pChart.Location = new System.Drawing.Point(231, 0);
            this.pChart.Name = "pChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.pChart.Series.Add(series1);
            this.pChart.Size = new System.Drawing.Size(400, 303);
            this.pChart.TabIndex = 44;
            this.pChart.Text = "chart1";
            this.pChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseDown);
            this.pChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseMove);
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOpenSWM);
            this.panel1.Controls.Add(this.txtSWM);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nudLagOrder);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.cboAssumption);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cboSAM);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cboFieldName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboTargetLayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 303);
            this.panel1.TabIndex = 43;
            // 
            // btnOpenSWM
            // 
            this.btnOpenSWM.Location = new System.Drawing.Point(189, 226);
            this.btnOpenSWM.Name = "btnOpenSWM";
            this.btnOpenSWM.Size = new System.Drawing.Size(29, 23);
            this.btnOpenSWM.TabIndex = 81;
            this.btnOpenSWM.Text = "...";
            this.btnOpenSWM.UseVisualStyleBackColor = true;
            this.btnOpenSWM.Click += new System.EventHandler(this.btnOpenSWM_Click_2);
            // 
            // txtSWM
            // 
            this.txtSWM.Location = new System.Drawing.Point(10, 228);
            this.txtSWM.Name = "txtSWM";
            this.txtSWM.Size = new System.Drawing.Size(166, 20);
            this.txtSWM.TabIndex = 80;
            this.txtSWM.Text = "Default";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 79;
            this.label5.Text = "Spatial Weights Matrix";
            // 
            // nudLagOrder
            // 
            this.nudLagOrder.Location = new System.Drawing.Point(153, 181);
            this.nudLagOrder.Name = "nudLagOrder";
            this.nudLagOrder.Size = new System.Drawing.Size(62, 20);
            this.nudLagOrder.TabIndex = 78;
            this.nudLagOrder.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 77;
            this.label6.Text = "Maximum lag order:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(128, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 73;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(18, 257);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 72;
            this.btnRun.Text = "OK";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // cboAssumption
            // 
            this.cboAssumption.FormattingEnabled = true;
            this.cboAssumption.Items.AddRange(new object[] {
            "Normality",
            "Randomization"});
            this.cboAssumption.Location = new System.Drawing.Point(10, 154);
            this.cboAssumption.Name = "cboAssumption";
            this.cboAssumption.Size = new System.Drawing.Size(208, 21);
            this.cboAssumption.TabIndex = 71;
            this.cboAssumption.Text = "Normality";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 13);
            this.label4.TabIndex = 70;
            this.label4.Text = "Inference Assumption:";
            // 
            // cboSAM
            // 
            this.cboSAM.FormattingEnabled = true;
            this.cboSAM.Items.AddRange(new object[] {
            "Moran Coefficient",
            "Geary Ratio",
            "Global G Statistic"});
            this.cboSAM.Location = new System.Drawing.Point(10, 111);
            this.cboSAM.Name = "cboSAM";
            this.cboSAM.Size = new System.Drawing.Size(208, 21);
            this.cboSAM.TabIndex = 69;
            this.cboSAM.Text = "Moran Coefficient";
            this.cboSAM.SelectedIndexChanged += new System.EventHandler(this.cboSAM_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Select Spatial Autocorrelation Measure:";
            // 
            // cboFieldName
            // 
            this.cboFieldName.FormattingEnabled = true;
            this.cboFieldName.Location = new System.Drawing.Point(10, 67);
            this.cboFieldName.Name = "cboFieldName";
            this.cboFieldName.Size = new System.Drawing.Size(208, 21);
            this.cboFieldName.TabIndex = 67;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 66;
            this.label2.Text = "Field:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 65;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(9, 26);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(210, 21);
            this.cboTargetLayer.TabIndex = 64;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // ofdOpenSWM
            // 
            this.ofdOpenSWM.Filter = "GAL files|*.gal|GWT files|*.gwt";
            this.ofdOpenSWM.Title = "Open GAL files";
            // 
            // conMenu
            // 
            this.conMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToImageFileToolStripMenuItem});
            this.conMenu.Name = "conMenu";
            this.conMenu.Size = new System.Drawing.Size(177, 26);
            // 
            // exportToImageFileToolStripMenuItem
            // 
            this.exportToImageFileToolStripMenuItem.Name = "exportToImageFileToolStripMenuItem";
            this.exportToImageFileToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exportToImageFileToolStripMenuItem.Text = "Export to image file";
            this.exportToImageFileToolStripMenuItem.Click += new System.EventHandler(this.exportToImageFileToolStripMenuItem_Click);
            // 
            // frmCorrelogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 303);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCorrelogram";
            this.Text = "Spatial Correlogram";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCorrelogram_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLagOrder)).EndInit();
            this.conMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown nudLagOrder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ComboBox cboAssumption;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSAM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboFieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.OpenFileDialog ofdOpenSWM;
        private System.Windows.Forms.ContextMenuStrip conMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToImageFileToolStripMenuItem;
        private System.Windows.Forms.Button btnOpenSWM;
        private System.Windows.Forms.TextBox txtSWM;
        private System.Windows.Forms.Label label5;
    }
}