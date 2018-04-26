namespace VisUncertainty
{
    partial class frmNewClassSeparbility
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewClassSeparbility));
            this.pChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.nudConfidenceLevel = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboValueField = new System.Windows.Forms.ComboBox();
            this.lblUncerfld = new System.Windows.Forms.Label();
            this.cboUncernFld = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboColorRamp = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboAlgorithm = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nudGCLinewidth = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.picGCLineColor = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listFormcolor = new System.Windows.Forms.ListView();
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtMinSep = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trbpValue = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudGCNClasses = new System.Windows.Forms.NumericUpDown();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.conMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToImageFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbpValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            this.conMenu.SuspendLayout();
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
            this.pChart.Location = new System.Drawing.Point(345, 0);
            this.pChart.Name = "pChart";
            this.pChart.Size = new System.Drawing.Size(600, 448);
            this.pChart.TabIndex = 149;
            this.pChart.Text = "chart1";
            this.pChart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pChart_MouseUp_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(345, 448);
            this.panel1.TabIndex = 150;
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
            this.groupBox1.Location = new System.Drawing.Point(14, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 115);
            this.groupBox1.TabIndex = 144;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Layer";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(272, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 12);
            this.label13.TabIndex = 138;
            this.label13.Text = "%";
            // 
            // nudConfidenceLevel
            // 
            this.nudConfidenceLevel.Location = new System.Drawing.Point(213, 90);
            this.nudConfidenceLevel.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            131072});
            this.nudConfidenceLevel.Name = "nudConfidenceLevel";
            this.nudConfidenceLevel.Size = new System.Drawing.Size(51, 21);
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
            this.label7.Location = new System.Drawing.Point(8, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 12);
            this.label7.TabIndex = 136;
            this.label7.Text = "Confidence Level:";
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(59, 18);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(229, 20);
            this.cboSourceLayer.TabIndex = 135;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 12);
            this.label5.TabIndex = 119;
            this.label5.Text = "Estimates Field:";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(148, 42);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(140, 20);
            this.cboValueField.TabIndex = 120;
            this.cboValueField.SelectedIndexChanged += new System.EventHandler(this.cboValueField_SelectedIndexChanged);
            // 
            // lblUncerfld
            // 
            this.lblUncerfld.AutoSize = true;
            this.lblUncerfld.Location = new System.Drawing.Point(8, 68);
            this.lblUncerfld.Name = "lblUncerfld";
            this.lblUncerfld.Size = new System.Drawing.Size(103, 12);
            this.lblUncerfld.TabIndex = 132;
            this.lblUncerfld.Text = "Uncertainty Field:";
            // 
            // cboUncernFld
            // 
            this.cboUncernFld.FormattingEnabled = true;
            this.cboUncernFld.Location = new System.Drawing.Point(148, 66);
            this.cboUncernFld.Name = "cboUncernFld";
            this.cboUncernFld.Size = new System.Drawing.Size(140, 20);
            this.cboUncernFld.TabIndex = 133;
            this.cboUncernFld.SelectedIndexChanged += new System.EventHandler(this.cboUncernFld_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 134;
            this.label1.Text = "Layer:";
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
            this.groupBox3.Location = new System.Drawing.Point(14, 299);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 116);
            this.groupBox3.TabIndex = 146;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Symbology";
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
            this.cboColorRamp.Location = new System.Drawing.Point(148, 18);
            this.cboColorRamp.Name = "cboColorRamp";
            this.cboColorRamp.Size = new System.Drawing.Size(158, 20);
            this.cboColorRamp.TabIndex = 131;
            this.cboColorRamp.Text = "Red Light to Dark";
            this.cboColorRamp.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboColorRamp_DrawItem);
            this.cboColorRamp.SelectedIndexChanged += new System.EventHandler(this.cboColorRamp_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 12);
            this.label8.TabIndex = 125;
            this.label8.Text = "Symbol Color Ramps:";
            // 
            // cboAlgorithm
            // 
            this.cboAlgorithm.FormattingEnabled = true;
            this.cboAlgorithm.Items.AddRange(new object[] {
            "HSV",
            "CIE Lab",
            "Lab LCh"});
            this.cboAlgorithm.Location = new System.Drawing.Point(148, 39);
            this.cboAlgorithm.Name = "cboAlgorithm";
            this.cboAlgorithm.Size = new System.Drawing.Size(70, 20);
            this.cboAlgorithm.TabIndex = 127;
            this.cboAlgorithm.Text = "CIE Lab";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 12);
            this.label9.TabIndex = 126;
            this.label9.Text = "Color ramp algorithm:";
            // 
            // nudGCLinewidth
            // 
            this.nudGCLinewidth.DecimalPlaces = 1;
            this.nudGCLinewidth.Location = new System.Drawing.Point(148, 64);
            this.nudGCLinewidth.Name = "nudGCLinewidth";
            this.nudGCLinewidth.Size = new System.Drawing.Size(64, 21);
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
            this.label12.Location = new System.Drawing.Point(10, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 12);
            this.label12.TabIndex = 121;
            this.label12.Text = "Outline Line Width: ";
            // 
            // picGCLineColor
            // 
            this.picGCLineColor.BackColor = System.Drawing.Color.Black;
            this.picGCLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGCLineColor.Location = new System.Drawing.Point(148, 88);
            this.picGCLineColor.Name = "picGCLineColor";
            this.picGCLineColor.Size = new System.Drawing.Size(63, 20);
            this.picGCLineColor.TabIndex = 123;
            this.picGCLineColor.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(114, 12);
            this.label10.TabIndex = 124;
            this.label10.Text = "Outline Line Color: ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listFormcolor);
            this.groupBox2.Controls.Add(this.txtMinSep);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.trbpValue);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cboMethod);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.nudGCNClasses);
            this.groupBox2.Location = new System.Drawing.Point(14, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 162);
            this.groupBox2.TabIndex = 145;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Method";
            // 
            // listFormcolor
            // 
            this.listFormcolor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23,
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26,
            this.columnHeader1});
            this.listFormcolor.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listFormcolor.Location = new System.Drawing.Point(13, 138);
            this.listFormcolor.Name = "listFormcolor";
            this.listFormcolor.Size = new System.Drawing.Size(282, 19);
            this.listFormcolor.TabIndex = 121;
            this.listFormcolor.UseCompatibleStateImageBehavior = false;
            this.listFormcolor.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "";
            this.columnHeader17.Width = 0;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "";
            this.columnHeader18.Width = 24;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "";
            this.columnHeader19.Width = 24;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "";
            this.columnHeader20.Width = 24;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "";
            this.columnHeader21.Width = 24;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "";
            this.columnHeader22.Width = 23;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "";
            this.columnHeader23.Width = 23;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "";
            this.columnHeader24.Width = 24;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "";
            this.columnHeader25.Width = 24;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "";
            this.columnHeader26.Width = 24;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 24;
            // 
            // txtMinSep
            // 
            this.txtMinSep.Location = new System.Drawing.Point(143, 76);
            this.txtMinSep.Name = "txtMinSep";
            this.txtMinSep.ReadOnly = true;
            this.txtMinSep.Size = new System.Drawing.Size(158, 21);
            this.txtMinSep.TabIndex = 146;
            this.txtMinSep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 81);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(100, 12);
            this.label14.TabIndex = 126;
            this.label14.Text = "Min Separability:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(276, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 12);
            this.label11.TabIndex = 125;
            this.label11.Text = "1.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 12);
            this.label4.TabIndex = 124;
            this.label4.Text = "0.5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 12);
            this.label3.TabIndex = 123;
            this.label3.Text = "0.0";
            // 
            // trbpValue
            // 
            this.trbpValue.Location = new System.Drawing.Point(3, 110);
            this.trbpValue.Maximum = 100;
            this.trbpValue.Name = "trbpValue";
            this.trbpValue.Size = new System.Drawing.Size(301, 45);
            this.trbpValue.TabIndex = 122;
            this.trbpValue.TickFrequency = 10;
            this.trbpValue.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trbpValue.Scroll += new System.EventHandler(this.trbpValue_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 120;
            this.label2.Text = "Classification Method:";
            // 
            // cboMethod
            // 
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "Class Seperability",
            "Bhattacharyya distance"});
            this.cboMethod.Location = new System.Drawing.Point(143, 22);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(161, 20);
            this.cboMethod.TabIndex = 119;
            this.cboMethod.Text = "Class Seperability";
            this.cboMethod.SelectedIndexChanged += new System.EventHandler(this.cboMethod_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 12);
            this.label6.TabIndex = 117;
            this.label6.Text = "Number of Class:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(240, 48);
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
            this.nudGCNClasses.Size = new System.Drawing.Size(64, 21);
            this.nudGCNClasses.TabIndex = 118;
            this.nudGCNClasses.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.nudGCNClasses.ValueChanged += new System.EventHandler(this.nudGCNClasses_ValueChanged);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(28, 418);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(108, 21);
            this.btnApply.TabIndex = 129;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(213, 418);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 21);
            this.btnCancel.TabIndex = 130;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
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
            // frmNewClassSeparbility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 448);
            this.Controls.Add(this.pChart);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmNewClassSeparbility";
            this.Text = "Class separability";
            this.Load += new System.EventHandler(this.frmNewClassSeparbility_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbpValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            this.conMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart pChart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.NumericUpDown nudConfidenceLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboValueField;
        private System.Windows.Forms.Label lblUncerfld;
        private System.Windows.Forms.ComboBox cboUncernFld;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboColorRamp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboAlgorithm;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudGCLinewidth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox picGCLineColor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.NumericUpDown nudGCNClasses;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.TrackBar trbpValue;
        private System.Windows.Forms.ListView listFormcolor;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader26;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMinSep;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip conMenu;
        private System.Windows.Forms.ToolStripMenuItem exportToImageFileToolStripMenuItem;
    }
}