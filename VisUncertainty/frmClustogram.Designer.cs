namespace VisUncertainty
{
    partial class frmClustogram
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chkNonDiag = new System.Windows.Forms.CheckBox();
            this.cboFldName2 = new System.Windows.Forms.ComboBox();
            this.lblVar2 = new System.Windows.Forms.Label();
            this.chkViolinPlot = new System.Windows.Forms.CheckBox();
            this.chkBoxPlot = new System.Windows.Forms.CheckBox();
            this.nudLagOrder = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cboSAM = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboFieldName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.chkAddMap = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudLagOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkNonDiag
            // 
            this.chkNonDiag.AutoSize = true;
            this.chkNonDiag.Checked = true;
            this.chkNonDiag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNonDiag.Enabled = false;
            this.chkNonDiag.Location = new System.Drawing.Point(19, 220);
            this.chkNonDiag.Name = "chkNonDiag";
            this.chkNonDiag.Size = new System.Drawing.Size(168, 16);
            this.chkNonDiag.TabIndex = 83;
            this.chkNonDiag.Text = "Non-Zero Diagonal Value";
            this.chkNonDiag.UseVisualStyleBackColor = true;
            // 
            // cboFldName2
            // 
            this.cboFldName2.Enabled = false;
            this.cboFldName2.FormattingEnabled = true;
            this.cboFldName2.Location = new System.Drawing.Point(10, 144);
            this.cboFldName2.Name = "cboFldName2";
            this.cboFldName2.Size = new System.Drawing.Size(242, 20);
            this.cboFldName2.TabIndex = 82;
            // 
            // lblVar2
            // 
            this.lblVar2.AutoSize = true;
            this.lblVar2.Enabled = false;
            this.lblVar2.Location = new System.Drawing.Point(10, 128);
            this.lblVar2.Name = "lblVar2";
            this.lblVar2.Size = new System.Drawing.Size(65, 12);
            this.lblVar2.TabIndex = 81;
            this.lblVar2.Text = "Variable 2:";
            // 
            // chkViolinPlot
            // 
            this.chkViolinPlot.AutoSize = true;
            this.chkViolinPlot.Location = new System.Drawing.Point(108, 198);
            this.chkViolinPlot.Name = "chkViolinPlot";
            this.chkViolinPlot.Size = new System.Drawing.Size(80, 16);
            this.chkViolinPlot.TabIndex = 80;
            this.chkViolinPlot.Text = "Violin Plot";
            this.chkViolinPlot.UseVisualStyleBackColor = true;
            // 
            // chkBoxPlot
            // 
            this.chkBoxPlot.AutoSize = true;
            this.chkBoxPlot.Checked = true;
            this.chkBoxPlot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxPlot.Location = new System.Drawing.Point(19, 198);
            this.chkBoxPlot.Name = "chkBoxPlot";
            this.chkBoxPlot.Size = new System.Drawing.Size(71, 16);
            this.chkBoxPlot.TabIndex = 79;
            this.chkBoxPlot.Text = "Box Plot";
            this.chkBoxPlot.UseVisualStyleBackColor = true;
            // 
            // nudLagOrder
            // 
            this.nudLagOrder.Location = new System.Drawing.Point(178, 169);
            this.nudLagOrder.Name = "nudLagOrder";
            this.nudLagOrder.Size = new System.Drawing.Size(72, 21);
            this.nudLagOrder.TabIndex = 78;
            this.nudLagOrder.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 174);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 12);
            this.label6.TabIndex = 77;
            this.label6.Text = "Maximum lag order:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(165, 317);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 21);
            this.btnCancel.TabIndex = 73;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(19, 317);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(87, 21);
            this.btnRun.TabIndex = 72;
            this.btnRun.Text = "OK";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
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
            this.pChart.Size = new System.Drawing.Size(467, 362);
            this.pChart.TabIndex = 48;
            this.pChart.Text = "chart1";
            this.pChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseDown);
            this.pChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseMove);
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // cboSAM
            // 
            this.cboSAM.FormattingEnabled = true;
            this.cboSAM.Items.AddRange(new object[] {
            "Moran Coefficient",
            "Local Geary Ratio",
            "G Statstics",
            "Local S",
            "Local L"});
            this.cboSAM.Location = new System.Drawing.Point(12, 62);
            this.cboSAM.Name = "cboSAM";
            this.cboSAM.Size = new System.Drawing.Size(242, 20);
            this.cboSAM.TabIndex = 69;
            this.cboSAM.Text = "Local Moran Coefficient";
            this.cboSAM.SelectedIndexChanged += new System.EventHandler(this.cboSAM_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 12);
            this.label3.TabIndex = 68;
            this.label3.Text = "Select Spatial Autocorrelation Measure:";
            // 
            // cboFieldName
            // 
            this.cboFieldName.FormattingEnabled = true;
            this.cboFieldName.Location = new System.Drawing.Point(12, 103);
            this.cboFieldName.Name = "cboFieldName";
            this.cboFieldName.Size = new System.Drawing.Size(242, 20);
            this.cboFieldName.TabIndex = 67;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 66;
            this.label2.Text = "Variable 1:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 65;
            this.label1.Text = "Select a Target Layer:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAddMap);
            this.panel1.Controls.Add(this.chkNonDiag);
            this.panel1.Controls.Add(this.cboFldName2);
            this.panel1.Controls.Add(this.lblVar2);
            this.panel1.Controls.Add(this.chkViolinPlot);
            this.panel1.Controls.Add(this.chkBoxPlot);
            this.panel1.Controls.Add(this.nudLagOrder);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Controls.Add(this.cboSAM);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cboFieldName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboTargetLayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 362);
            this.panel1.TabIndex = 47;
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(10, 24);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(244, 20);
            this.cboTargetLayer.TabIndex = 64;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // chkAddMap
            // 
            this.chkAddMap.AutoSize = true;
            this.chkAddMap.Location = new System.Drawing.Point(19, 255);
            this.chkAddMap.Name = "chkAddMap";
            this.chkAddMap.Size = new System.Drawing.Size(75, 16);
            this.chkAddMap.TabIndex = 84;
            this.chkAddMap.Text = "Add Map";
            this.chkAddMap.UseVisualStyleBackColor = true;
            // 
            // frmClustogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 362);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Name = "frmClustogram";
            this.Text = "frmClustogram";
            ((System.ComponentModel.ISupportInitialize)(this.nudLagOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkNonDiag;
        private System.Windows.Forms.ComboBox cboFldName2;
        private System.Windows.Forms.Label lblVar2;
        private System.Windows.Forms.CheckBox chkViolinPlot;
        private System.Windows.Forms.CheckBox chkBoxPlot;
        private System.Windows.Forms.NumericUpDown nudLagOrder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.ComboBox cboSAM;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboFieldName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.CheckBox chkAddMap;
    }
}