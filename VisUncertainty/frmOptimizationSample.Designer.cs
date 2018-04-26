namespace VisUncertainty
{
    partial class frmOptimizationSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptimizationSample));
            this.label2 = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboUncernFld = new System.Windows.Forms.ComboBox();
            this.lblUncerfld = new System.Windows.Forms.Label();
            this.cboColorRamp = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMeasures = new System.Windows.Forms.ComboBox();
            this.btnGraph = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nudConfidenceLevel = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.conMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdExportImage = new System.Windows.Forms.SaveFileDialog();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.conMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 137;
            this.label2.Text = "Minimizing";
            // 
            // cboMethod
            // 
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "Sum of within groups",
            "Sum of the maximum within groups",
            "Maximum sum of within groups",
            "Maximum within groups"});
            this.cboMethod.Location = new System.Drawing.Point(80, 24);
            this.cboMethod.Margin = new System.Windows.Forms.Padding(5);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(159, 21);
            this.cboMethod.TabIndex = 136;
            this.cboMethod.Text = "Sum of within groups";
            this.cboMethod.SelectedIndexChanged += new System.EventHandler(this.cboMethod_SelectedIndexChanged);
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(105, 16);
            this.cboSourceLayer.Margin = new System.Windows.Forms.Padding(5);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(134, 21);
            this.cboSourceLayer.TabIndex = 135;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 134;
            this.label1.Text = "Layer:";
            // 
            // cboUncernFld
            // 
            this.cboUncernFld.FormattingEnabled = true;
            this.cboUncernFld.Location = new System.Drawing.Point(105, 72);
            this.cboUncernFld.Margin = new System.Windows.Forms.Padding(5);
            this.cboUncernFld.Name = "cboUncernFld";
            this.cboUncernFld.Size = new System.Drawing.Size(134, 21);
            this.cboUncernFld.TabIndex = 133;
            this.cboUncernFld.SelectedIndexChanged += new System.EventHandler(this.cboUncernFld_SelectedIndexChanged);
            // 
            // lblUncerfld
            // 
            this.lblUncerfld.AutoSize = true;
            this.lblUncerfld.Location = new System.Drawing.Point(15, 76);
            this.lblUncerfld.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUncerfld.Name = "lblUncerfld";
            this.lblUncerfld.Size = new System.Drawing.Size(89, 13);
            this.lblUncerfld.TabIndex = 132;
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
            this.cboColorRamp.Location = new System.Drawing.Point(122, 22);
            this.cboColorRamp.Margin = new System.Windows.Forms.Padding(5);
            this.cboColorRamp.Name = "cboColorRamp";
            this.cboColorRamp.Size = new System.Drawing.Size(115, 21);
            this.cboColorRamp.TabIndex = 131;
            this.cboColorRamp.Text = "Red Light to Dark";
            this.cboColorRamp.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboColorRamp_DrawItem);
            this.cboColorRamp.SelectedIndexChanged += new System.EventHandler(this.cboColorRamp_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(148, 420);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 25);
            this.btnCancel.TabIndex = 130;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(10, 420);
            this.btnApply.Margin = new System.Windows.Forms.Padding(5);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(116, 25);
            this.btnApply.TabIndex = 129;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cboAlgorithm
            // 
            this.cboAlgorithm.FormattingEnabled = true;
            this.cboAlgorithm.Items.AddRange(new object[] {
            "HSV",
            "CIE Lab",
            "Lab LCh"});
            this.cboAlgorithm.Location = new System.Drawing.Point(147, 50);
            this.cboAlgorithm.Margin = new System.Windows.Forms.Padding(5);
            this.cboAlgorithm.Name = "cboAlgorithm";
            this.cboAlgorithm.Size = new System.Drawing.Size(90, 21);
            this.cboAlgorithm.TabIndex = 127;
            this.cboAlgorithm.Text = "CIE Lab";
            this.cboAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cboAlgorithm_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 52);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 13);
            this.label9.TabIndex = 126;
            this.label9.Text = "Color ramp algorithm:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 125;
            this.label8.Text = "Symbol Color Ramps:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 106);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 124;
            this.label10.Text = "Outline Line Color: ";
            // 
            // picGCLineColor
            // 
            this.picGCLineColor.BackColor = System.Drawing.Color.Black;
            this.picGCLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGCLineColor.Location = new System.Drawing.Point(156, 106);
            this.picGCLineColor.Margin = new System.Windows.Forms.Padding(5);
            this.picGCLineColor.Name = "picGCLineColor";
            this.picGCLineColor.Size = new System.Drawing.Size(81, 18);
            this.picGCLineColor.TabIndex = 123;
            this.picGCLineColor.TabStop = false;
            this.picGCLineColor.Click += new System.EventHandler(this.picGCLineColor_Click);
            // 
            // nudGCLinewidth
            // 
            this.nudGCLinewidth.DecimalPlaces = 1;
            this.nudGCLinewidth.Location = new System.Drawing.Point(154, 79);
            this.nudGCLinewidth.Margin = new System.Windows.Forms.Padding(5);
            this.nudGCLinewidth.Name = "nudGCLinewidth";
            this.nudGCLinewidth.Size = new System.Drawing.Size(83, 20);
            this.nudGCLinewidth.TabIndex = 122;
            this.nudGCLinewidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 79);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 121;
            this.label12.Text = "Outline Line Width: ";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(105, 44);
            this.cboValueField.Margin = new System.Windows.Forms.Padding(5);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(134, 21);
            this.cboValueField.TabIndex = 120;
            this.cboValueField.SelectedIndexChanged += new System.EventHandler(this.cboValueField_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 48);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 119;
            this.label5.Text = "Estimates Field:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(156, 76);
            this.nudGCNClasses.Margin = new System.Windows.Forms.Padding(5);
            this.nudGCNClasses.Name = "nudGCNClasses";
            this.nudGCNClasses.Size = new System.Drawing.Size(83, 20);
            this.nudGCNClasses.TabIndex = 118;
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
            this.label6.Location = new System.Drawing.Point(15, 83);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 117;
            this.label6.Text = "Number of Class:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(155, 643);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 24);
            this.label3.TabIndex = 138;
            this.label3.Text = "Objective Value:";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 141;
            this.label4.Text = "Measure:";
            // 
            // cboMeasures
            // 
            this.cboMeasures.FormattingEnabled = true;
            this.cboMeasures.Items.AddRange(new object[] {
            "Class Separability",
            "Bhattacharyya distance "});
            this.cboMeasures.Location = new System.Drawing.Point(80, 52);
            this.cboMeasures.Margin = new System.Windows.Forms.Padding(5);
            this.cboMeasures.Name = "cboMeasures";
            this.cboMeasures.Size = new System.Drawing.Size(159, 21);
            this.cboMeasures.TabIndex = 140;
            this.cboMeasures.Text = "Class Separability";
            this.cboMeasures.SelectedIndexChanged += new System.EventHandler(this.cboMeasures_SelectedIndexChanged);
            // 
            // btnGraph
            // 
            this.btnGraph.Enabled = false;
            this.btnGraph.Location = new System.Drawing.Point(151, 667);
            this.btnGraph.Margin = new System.Windows.Forms.Padding(5);
            this.btnGraph.Name = "btnGraph";
            this.btnGraph.Size = new System.Drawing.Size(113, 35);
            this.btnGraph.TabIndex = 142;
            this.btnGraph.Text = "Show Graph";
            this.btnGraph.UseVisualStyleBackColor = true;
            this.btnGraph.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.nudConfidenceLevel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cboSourceLayer);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboValueField);
            this.groupBox1.Controls.Add(this.lblUncerfld);
            this.groupBox1.Controls.Add(this.cboUncernFld);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(252, 137);
            this.groupBox1.TabIndex = 144;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Layer";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(219, 106);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 13);
            this.label13.TabIndex = 138;
            this.label13.Text = "%";
            // 
            // nudConfidenceLevel
            // 
            this.nudConfidenceLevel.Location = new System.Drawing.Point(149, 102);
            this.nudConfidenceLevel.Margin = new System.Windows.Forms.Padding(5);
            this.nudConfidenceLevel.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.nudConfidenceLevel.Name = "nudConfidenceLevel";
            this.nudConfidenceLevel.Size = new System.Drawing.Size(66, 20);
            this.nudConfidenceLevel.TabIndex = 137;
            this.nudConfidenceLevel.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.nudConfidenceLevel.ValueChanged += new System.EventHandler(this.nudConfidenceLevel_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 104);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 136;
            this.label7.Text = "Confidence Level:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboMethod);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.nudGCNClasses);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cboMeasures);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(10, 161);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox2.Size = new System.Drawing.Size(252, 106);
            this.groupBox2.TabIndex = 145;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optimization method";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboColorRamp);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cboAlgorithm);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.nudGCLinewidth);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.picGCLineColor);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(10, 274);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox3.Size = new System.Drawing.Size(252, 136);
            this.groupBox3.TabIndex = 146;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Symbology";
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
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY2.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.pChart.ChartAreas.Add(chartArea1);
            this.pChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.pChart.Legends.Add(legend1);
            this.pChart.Location = new System.Drawing.Point(279, 0);
            this.pChart.Margin = new System.Windows.Forms.Padding(5);
            this.pChart.Name = "pChart";
            this.pChart.Size = new System.Drawing.Size(526, 460);
            this.pChart.TabIndex = 147;
            this.pChart.Text = "chart1";
            this.pChart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseDown);
            this.pChart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseMove);
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnGraph);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 460);
            this.panel1.TabIndex = 148;
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
            // frmOptimizationSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 460);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmOptimizationSample";
            this.Text = "Optimal Classification with Uncertainty";
            this.Load += new System.EventHandler(this.frmOptimizationSample_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.conMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboUncernFld;
        private System.Windows.Forms.Label lblUncerfld;
        private System.Windows.Forms.ComboBox cboColorRamp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMeasures;
        private System.Windows.Forms.Button btnGraph;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.NumericUpDown nudConfidenceLevel;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip conMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToImageFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdExportImage;
        private System.Windows.Forms.ColorDialog cdColor;
    }
}