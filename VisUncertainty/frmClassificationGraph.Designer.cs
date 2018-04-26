namespace VisUncertainty
{
    partial class frmClassificationGraph
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
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtObjValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudGCNClasses = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.nudConfidenceLevel = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMethod = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pChart
            // 
            this.pChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LabelStyle.TruncatedLabels = true;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.pChart.ChartAreas.Add(chartArea1);
            this.pChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.pChart.Legends.Add(legend1);
            this.pChart.Location = new System.Drawing.Point(0, 29);
            this.pChart.Name = "pChart";
            this.pChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen;
            this.pChart.Size = new System.Drawing.Size(584, 402);
            this.pChart.TabIndex = 1;
            this.pChart.Text = "chart1";
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.txtObjValue);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nudGCNClasses);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.nudConfidenceLevel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(584, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(132, 431);
            this.panel1.TabIndex = 2;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(19, 391);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(101, 28);
            this.btnApply.TabIndex = 142;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtObjValue
            // 
            this.txtObjValue.Location = new System.Drawing.Point(12, 147);
            this.txtObjValue.Name = "txtObjValue";
            this.txtObjValue.Size = new System.Drawing.Size(108, 20);
            this.txtObjValue.TabIndex = 141;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 140;
            this.label3.Text = "Objective Value:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(60, 93);
            this.nudGCNClasses.Name = "nudGCNClasses";
            this.nudGCNClasses.Size = new System.Drawing.Size(55, 20);
            this.nudGCNClasses.TabIndex = 120;
            this.nudGCNClasses.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudGCNClasses.ValueChanged += new System.EventHandler(this.nudGCNClasses_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 119;
            this.label6.Text = "Number of Classes:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(110, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "%";
            // 
            // nudConfidenceLevel
            // 
            this.nudConfidenceLevel.Location = new System.Drawing.Point(60, 43);
            this.nudConfidenceLevel.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.nudConfidenceLevel.Name = "nudConfidenceLevel";
            this.nudConfidenceLevel.Size = new System.Drawing.Size(44, 20);
            this.nudConfidenceLevel.TabIndex = 37;
            this.nudConfidenceLevel.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudConfidenceLevel.ValueChanged += new System.EventHandler(this.nudConfidenceLevel_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Confidence Level:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblMethod);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(584, 29);
            this.panel2.TabIndex = 3;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(12, 8);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(53, 13);
            this.lblMethod.TabIndex = 0;
            this.lblMethod.Text = "lblMethod";
            // 
            // frmClassificationGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 431);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmClassificationGraph";
            this.Text = "Optimal Classification";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmClassificationGraph_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.NumericUpDown nudConfidenceLevel;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown nudGCNClasses;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtObjValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.Button btnApply;

    }
}