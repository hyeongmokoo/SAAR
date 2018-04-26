namespace VisUncertainty
{
    partial class frmRSSample
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnRun = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.txtEntropyOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtFuzzyOuput = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtClassifiedResult = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkEntropy = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkProb = new System.Windows.Forms.CheckBox();
            this.chkDNs = new System.Windows.Forms.CheckBox();
            this.cboClassificationMethod = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnInputSigfile = new System.Windows.Forms.Button();
            this.txtInputSigFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInputRaster = new System.Windows.Forms.Button();
            this.txtInputRaster = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 424);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cboClassificationMethod);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnInputSigfile);
            this.panel1.Controls.Add(this.txtInputSigFile);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnInputRaster);
            this.panel1.Controls.Add(this.txtInputRaster);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 460);
            this.panel1.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.txtEntropyOutput);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.txtFuzzyOuput);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.txtClassifiedResult);
            this.groupBox3.Location = new System.Drawing.Point(12, 279);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(171, 139);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Save";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Entropy:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(128, 109);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(36, 23);
            this.button3.TabIndex = 19;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txtEntropyOutput
            // 
            this.txtEntropyOutput.Location = new System.Drawing.Point(7, 111);
            this.txtEntropyOutput.Name = "txtEntropyOutput";
            this.txtEntropyOutput.Size = new System.Drawing.Size(116, 20);
            this.txtEntropyOutput.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Fuzzy memberships:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(128, 71);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(36, 23);
            this.button2.TabIndex = 16;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // txtFuzzyOuput
            // 
            this.txtFuzzyOuput.Location = new System.Drawing.Point(7, 73);
            this.txtFuzzyOuput.Name = "txtFuzzyOuput";
            this.txtFuzzyOuput.Size = new System.Drawing.Size(116, 20);
            this.txtFuzzyOuput.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Classified Result:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(128, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 23);
            this.button1.TabIndex = 5;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtClassifiedResult
            // 
            this.txtClassifiedResult.Location = new System.Drawing.Point(7, 33);
            this.txtClassifiedResult.Name = "txtClassifiedResult";
            this.txtClassifiedResult.Size = new System.Drawing.Size(116, 20);
            this.txtClassifiedResult.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(108, 424);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkEntropy);
            this.groupBox2.Location = new System.Drawing.Point(12, 227);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 46);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Moran Scatter Plot";
            // 
            // chkEntropy
            // 
            this.chkEntropy.AutoSize = true;
            this.chkEntropy.Checked = true;
            this.chkEntropy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEntropy.Location = new System.Drawing.Point(7, 20);
            this.chkEntropy.Name = "chkEntropy";
            this.chkEntropy.Size = new System.Drawing.Size(62, 17);
            this.chkEntropy.TabIndex = 0;
            this.chkEntropy.Text = "Entropy";
            this.chkEntropy.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkProb);
            this.groupBox1.Controls.Add(this.chkDNs);
            this.groupBox1.Location = new System.Drawing.Point(12, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 72);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Violin plots";
            // 
            // chkProb
            // 
            this.chkProb.AutoSize = true;
            this.chkProb.Checked = true;
            this.chkProb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProb.Location = new System.Drawing.Point(6, 43);
            this.chkProb.Name = "chkProb";
            this.chkProb.Size = new System.Drawing.Size(117, 17);
            this.chkProb.TabIndex = 1;
            this.chkProb.Text = "Fuzzy memberships";
            this.chkProb.UseVisualStyleBackColor = true;
            // 
            // chkDNs
            // 
            this.chkDNs.AutoSize = true;
            this.chkDNs.Checked = true;
            this.chkDNs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDNs.Location = new System.Drawing.Point(7, 20);
            this.chkDNs.Name = "chkDNs";
            this.chkDNs.Size = new System.Drawing.Size(146, 17);
            this.chkDNs.TabIndex = 0;
            this.chkDNs.Text = "Digital Numbers of Layers";
            this.chkDNs.UseVisualStyleBackColor = true;
            // 
            // cboClassificationMethod
            // 
            this.cboClassificationMethod.FormattingEnabled = true;
            this.cboClassificationMethod.Items.AddRange(new object[] {
            "Maximum likelihood Classifier",
            "Supervised Fuzzy C-Means"});
            this.cboClassificationMethod.Location = new System.Drawing.Point(15, 121);
            this.cboClassificationMethod.Name = "cboClassificationMethod";
            this.cboClassificationMethod.Size = new System.Drawing.Size(168, 21);
            this.cboClassificationMethod.TabIndex = 8;
            this.cboClassificationMethod.Text = "Supervised Fuzzy C-Means";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Classification Method:";
            // 
            // btnInputSigfile
            // 
            this.btnInputSigfile.Location = new System.Drawing.Point(147, 75);
            this.btnInputSigfile.Name = "btnInputSigfile";
            this.btnInputSigfile.Size = new System.Drawing.Size(36, 23);
            this.btnInputSigfile.TabIndex = 6;
            this.btnInputSigfile.UseVisualStyleBackColor = true;
            // 
            // txtInputSigFile
            // 
            this.txtInputSigFile.Location = new System.Drawing.Point(12, 77);
            this.txtInputSigFile.Name = "txtInputSigFile";
            this.txtInputSigFile.Size = new System.Drawing.Size(131, 20);
            this.txtInputSigFile.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Input Signature File:";
            // 
            // btnInputRaster
            // 
            this.btnInputRaster.Location = new System.Drawing.Point(150, 23);
            this.btnInputRaster.Name = "btnInputRaster";
            this.btnInputRaster.Size = new System.Drawing.Size(36, 23);
            this.btnInputRaster.TabIndex = 3;
            this.btnInputRaster.UseVisualStyleBackColor = true;
            // 
            // txtInputRaster
            // 
            this.txtInputRaster.Location = new System.Drawing.Point(15, 25);
            this.txtInputRaster.Name = "txtInputRaster";
            this.txtInputRaster.Size = new System.Drawing.Size(131, 20);
            this.txtInputRaster.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Raster Bands:";
            // 
            // pChart
            // 
            this.pChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea2.AxisX.LabelStyle.Interval = 0D;
            chartArea2.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea2.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea2.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea2.AxisX.LabelStyle.TruncatedLabels = true;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.MajorTickMark.Enabled = false;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisX2.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorTickMark.Enabled = false;
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY2.MajorGrid.Enabled = false;
            chartArea2.Name = "ChartArea1";
            this.pChart.ChartAreas.Add(chartArea2);
            this.pChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.pChart.Legends.Add(legend2);
            this.pChart.Location = new System.Drawing.Point(189, 0);
            this.pChart.Name = "pChart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.YValuesPerPoint = 2;
            this.pChart.Series.Add(series2);
            this.pChart.Size = new System.Drawing.Size(637, 460);
            this.pChart.TabIndex = 153;
            this.pChart.Text = "chart1";
            this.pChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseDown);
            this.pChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseMove);
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // frmRSSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 460);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Name = "frmRSSample";
            this.Text = "frmRSSample";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkEntropy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkProb;
        private System.Windows.Forms.CheckBox chkDNs;
        private System.Windows.Forms.ComboBox cboClassificationMethod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnInputSigfile;
        private System.Windows.Forms.TextBox txtInputSigFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInputRaster;
        private System.Windows.Forms.TextBox txtInputRaster;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtEntropyOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtFuzzyOuput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtClassifiedResult;
    }
}