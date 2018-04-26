namespace VisUncertainty
{
    partial class frmAttGraduatedSymbols
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAttGraduatedSymbols));
            this.label1 = new System.Windows.Forms.Label();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboValueField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CboUField = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkNewLayer = new System.Windows.Forms.CheckBox();
            this.txtNewLayer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            this.nudConfidenceLevel = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.tcUncer = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.nudLinewidth = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.picSymbolColor = new System.Windows.Forms.PictureBox();
            this.picLineColor = new System.Windows.Forms.PictureBox();
            this.nudTransTo = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.nudTransFrom = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.nudSymbolSize = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudThickness = new System.Windows.Forms.NumericUpDown();
            this.chk3D = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudChartSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nudChartWidth = new System.Windows.Forms.NumericUpDown();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.picLineConColor = new System.Windows.Forms.PictureBox();
            this.picLineCntColor = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.nudMinWidth = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).BeginInit();
            this.tcUncer.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSymbolColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSymbolSize)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThickness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChartSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChartWidth)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLineConColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLineCntColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Layer:";
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(16, 29);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(164, 21);
            this.cboSourceLayer.TabIndex = 21;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Estimated Value Field:";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(15, 69);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(164, 21);
            this.cboValueField.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Uncertainty Field:";
            // 
            // CboUField
            // 
            this.CboUField.FormattingEnabled = true;
            this.CboUField.Location = new System.Drawing.Point(15, 109);
            this.CboUField.Name = "CboUField";
            this.CboUField.Size = new System.Drawing.Size(164, 21);
            this.CboUField.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Confidence Level:";
            // 
            // chkNewLayer
            // 
            this.chkNewLayer.AutoSize = true;
            this.chkNewLayer.Location = new System.Drawing.Point(16, 177);
            this.chkNewLayer.Name = "chkNewLayer";
            this.chkNewLayer.Size = new System.Drawing.Size(111, 17);
            this.chkNewLayer.TabIndex = 28;
            this.chkNewLayer.Text = "Create New Layer";
            this.chkNewLayer.UseVisualStyleBackColor = true;
            this.chkNewLayer.CheckedChanged += new System.EventHandler(this.chkNewLayer_CheckedChanged);
            // 
            // txtNewLayer
            // 
            this.txtNewLayer.Enabled = false;
            this.txtNewLayer.Location = new System.Drawing.Point(15, 213);
            this.txtNewLayer.Name = "txtNewLayer";
            this.txtNewLayer.Size = new System.Drawing.Size(164, 20);
            this.txtNewLayer.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "New Layer Name:";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(205, 213);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 32;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(299, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // nudConfidenceLevel
            // 
            this.nudConfidenceLevel.Location = new System.Drawing.Point(112, 139);
            this.nudConfidenceLevel.Name = "nudConfidenceLevel";
            this.nudConfidenceLevel.Size = new System.Drawing.Size(44, 20);
            this.nudConfidenceLevel.TabIndex = 34;
            this.nudConfidenceLevel.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(162, 147);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(15, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "%";
            // 
            // tcUncer
            // 
            this.tcUncer.Controls.Add(this.tabPage1);
            this.tcUncer.Controls.Add(this.tabPage2);
            this.tcUncer.Controls.Add(this.tabPage3);
            this.tcUncer.Location = new System.Drawing.Point(205, 13);
            this.tcUncer.Name = "tcUncer";
            this.tcUncer.SelectedIndex = 0;
            this.tcUncer.Size = new System.Drawing.Size(200, 194);
            this.tcUncer.TabIndex = 36;
            this.tcUncer.SelectedIndexChanged += new System.EventHandler(this.tcUncer_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.nudLinewidth);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.picSymbolColor);
            this.tabPage1.Controls.Add(this.picLineColor);
            this.tabPage1.Controls.Add(this.nudTransTo);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.nudTransFrom);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.label19);
            this.tabPage1.Controls.Add(this.nudSymbolSize);
            this.tabPage1.Controls.Add(this.label20);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(192, 168);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Circles";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // nudLinewidth
            // 
            this.nudLinewidth.DecimalPlaces = 1;
            this.nudLinewidth.Location = new System.Drawing.Point(108, 83);
            this.nudLinewidth.Name = "nudLinewidth";
            this.nudLinewidth.Size = new System.Drawing.Size(55, 20);
            this.nudLinewidth.TabIndex = 26;
            this.nudLinewidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 85);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "Line width: ";
            // 
            // picSymbolColor
            // 
            this.picSymbolColor.BackColor = System.Drawing.Color.DodgerBlue;
            this.picSymbolColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSymbolColor.Location = new System.Drawing.Point(107, 50);
            this.picSymbolColor.Name = "picSymbolColor";
            this.picSymbolColor.Size = new System.Drawing.Size(55, 21);
            this.picSymbolColor.TabIndex = 24;
            this.picSymbolColor.TabStop = false;
            this.picSymbolColor.Click += new System.EventHandler(this.picSymbolColor_Click);
            // 
            // picLineColor
            // 
            this.picLineColor.BackColor = System.Drawing.Color.LightCoral;
            this.picLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLineColor.Location = new System.Drawing.Point(107, 111);
            this.picLineColor.Name = "picLineColor";
            this.picLineColor.Size = new System.Drawing.Size(55, 21);
            this.picLineColor.TabIndex = 23;
            this.picLineColor.TabStop = false;
            this.picLineColor.Click += new System.EventHandler(this.picLineColor_Click);
            // 
            // nudTransTo
            // 
            this.nudTransTo.Location = new System.Drawing.Point(126, 161);
            this.nudTransTo.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTransTo.Name = "nudTransTo";
            this.nudTransTo.Size = new System.Drawing.Size(43, 20);
            this.nudTransTo.TabIndex = 22;
            this.nudTransTo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudTransTo.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(104, 163);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 13);
            this.label15.TabIndex = 21;
            this.label15.Text = "To:";
            this.label15.Visible = false;
            // 
            // nudTransFrom
            // 
            this.nudTransFrom.Location = new System.Drawing.Point(55, 161);
            this.nudTransFrom.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTransFrom.Name = "nudTransFrom";
            this.nudTransFrom.Size = new System.Drawing.Size(43, 20);
            this.nudTransFrom.TabIndex = 20;
            this.nudTransFrom.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTransFrom.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(22, 163);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "From:";
            this.label16.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 145);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(72, 13);
            this.label17.TabIndex = 18;
            this.label17.Text = "Transparency";
            this.label17.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 13);
            this.label18.TabIndex = 17;
            this.label18.Text = "Symbol Color:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 114);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(57, 13);
            this.label19.TabIndex = 16;
            this.label19.Text = "Line Color:";
            // 
            // nudSymbolSize
            // 
            this.nudSymbolSize.DecimalPlaces = 1;
            this.nudSymbolSize.Location = new System.Drawing.Point(108, 16);
            this.nudSymbolSize.Name = "nudSymbolSize";
            this.nudSymbolSize.Size = new System.Drawing.Size(55, 20);
            this.nudSymbolSize.TabIndex = 15;
            this.nudSymbolSize.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 18);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(90, 13);
            this.label20.TabIndex = 14;
            this.label20.Text = "Min Symbol Size: ";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.nudChartSize);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.nudChartWidth);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 168);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Bar";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.nudThickness);
            this.groupBox1.Controls.Add(this.chk3D);
            this.groupBox1.Location = new System.Drawing.Point(9, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 93);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "3D Chart";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 54;
            this.label8.Text = "Thickness:";
            // 
            // nudThickness
            // 
            this.nudThickness.Enabled = false;
            this.nudThickness.Location = new System.Drawing.Point(104, 44);
            this.nudThickness.Name = "nudThickness";
            this.nudThickness.Size = new System.Drawing.Size(59, 20);
            this.nudThickness.TabIndex = 53;
            this.nudThickness.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // chk3D
            // 
            this.chk3D.AutoSize = true;
            this.chk3D.Location = new System.Drawing.Point(9, 21);
            this.chk3D.Name = "chk3D";
            this.chk3D.Size = new System.Drawing.Size(88, 17);
            this.chk3D.TabIndex = 50;
            this.chk3D.Text = "Display in 3D";
            this.chk3D.UseVisualStyleBackColor = true;
            this.chk3D.CheckedChanged += new System.EventHandler(this.chk3D_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 54;
            this.label7.Text = "Chart Size:";
            // 
            // nudChartSize
            // 
            this.nudChartSize.Location = new System.Drawing.Point(113, 40);
            this.nudChartSize.Name = "nudChartSize";
            this.nudChartSize.Size = new System.Drawing.Size(59, 20);
            this.nudChartSize.TabIndex = 53;
            this.nudChartSize.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 52;
            this.label6.Text = "Chart Width:";
            // 
            // nudChartWidth
            // 
            this.nudChartWidth.Location = new System.Drawing.Point(113, 9);
            this.nudChartWidth.Name = "nudChartWidth";
            this.nudChartWidth.Size = new System.Drawing.Size(59, 20);
            this.nudChartWidth.TabIndex = 51;
            this.nudChartWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.picLineConColor);
            this.tabPage3.Controls.Add(this.picLineCntColor);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.nudMinWidth);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(192, 168);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "LineWidth";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // picLineConColor
            // 
            this.picLineConColor.BackColor = System.Drawing.Color.DodgerBlue;
            this.picLineConColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLineConColor.Location = new System.Drawing.Point(117, 50);
            this.picLineConColor.Name = "picLineConColor";
            this.picLineConColor.Size = new System.Drawing.Size(55, 21);
            this.picLineConColor.TabIndex = 32;
            this.picLineConColor.TabStop = false;
            // 
            // picLineCntColor
            // 
            this.picLineCntColor.BackColor = System.Drawing.Color.LightCoral;
            this.picLineCntColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picLineCntColor.Location = new System.Drawing.Point(117, 81);
            this.picLineCntColor.Name = "picLineCntColor";
            this.picLineCntColor.Size = new System.Drawing.Size(55, 21);
            this.picLineCntColor.TabIndex = 31;
            this.picLineCntColor.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Upper bound Color:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Lower bound Color:";
            // 
            // nudMinWidth
            // 
            this.nudMinWidth.DecimalPlaces = 1;
            this.nudMinWidth.Location = new System.Drawing.Point(117, 16);
            this.nudMinWidth.Name = "nudMinWidth";
            this.nudMinWidth.Size = new System.Drawing.Size(55, 20);
            this.nudMinWidth.TabIndex = 28;
            this.nudMinWidth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Min Symbol width: ";
            // 
            // frmAttGraduatedSymbols
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(408, 247);
            this.Controls.Add(this.tcUncer);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.nudConfidenceLevel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtNewLayer);
            this.Controls.Add(this.chkNewLayer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CboUField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboValueField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAttGraduatedSymbols";
            this.Text = "Proportional Symbol with Confidence Bounds";
            ((System.ComponentModel.ISupportInitialize)(this.nudConfidenceLevel)).EndInit();
            this.tcUncer.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSymbolColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSymbolSize)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThickness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChartSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChartWidth)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLineConColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLineCntColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboValueField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CboUField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkNewLayer;
        private System.Windows.Forms.TextBox txtNewLayer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColorDialog cdColor;
        private System.Windows.Forms.NumericUpDown nudConfidenceLevel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabControl tcUncer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.NumericUpDown nudLinewidth;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox picSymbolColor;
        private System.Windows.Forms.PictureBox picLineColor;
        private System.Windows.Forms.NumericUpDown nudTransTo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudTransFrom;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown nudSymbolSize;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chk3D;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudThickness;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudChartSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudChartWidth;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox picLineConColor;
        private System.Windows.Forms.PictureBox picLineCntColor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudMinWidth;
        private System.Windows.Forms.Label label12;
    }
}