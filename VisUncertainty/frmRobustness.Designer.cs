namespace VisUncertainty
{
    partial class frmRobustness
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.cboUField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboValueField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grbVisualization = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboGCClassify = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.picGCLineColor = new System.Windows.Forms.PictureBox();
            this.nudGCLinewidth = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.picSymbolTo = new System.Windows.Forms.PictureBox();
            this.picSymolfrom = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudGCNClasses = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudTeNClasses = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudTeLinewidth = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.picTeLineColor = new System.Windows.Forms.PictureBox();
            this.label19 = new System.Windows.Forms.Label();
            this.nudSeperationTo = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.nudSeperationFrom = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.nudAngleFrom = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            this.chkRobustness = new System.Windows.Forms.CheckBox();
            this.txtFldName = new System.Windows.Forms.TextBox();
            this.lblFldName = new System.Windows.Forms.Label();
            this.grbVisualization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSymbolTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSymolfrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTeNClasses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTeLinewidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTeLineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeperationTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeperationFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(113, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 70;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(15, 211);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 69;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cboUField
            // 
            this.cboUField.FormattingEnabled = true;
            this.cboUField.Location = new System.Drawing.Point(15, 106);
            this.cboUField.Name = "cboUField";
            this.cboUField.Size = new System.Drawing.Size(164, 21);
            this.cboUField.TabIndex = 65;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 64;
            this.label3.Text = "Standard Error Field:";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(15, 66);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(164, 21);
            this.cboValueField.TabIndex = 63;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Value Field:";
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(16, 26);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(164, 21);
            this.cboSourceLayer.TabIndex = 61;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Layer:";
            // 
            // grbVisualization
            // 
            this.grbVisualization.Controls.Add(this.label10);
            this.grbVisualization.Controls.Add(this.cboGCClassify);
            this.grbVisualization.Controls.Add(this.label4);
            this.grbVisualization.Controls.Add(this.picGCLineColor);
            this.grbVisualization.Controls.Add(this.nudGCLinewidth);
            this.grbVisualization.Controls.Add(this.label12);
            this.grbVisualization.Controls.Add(this.picSymbolTo);
            this.grbVisualization.Controls.Add(this.picSymolfrom);
            this.grbVisualization.Controls.Add(this.label8);
            this.grbVisualization.Controls.Add(this.nudGCNClasses);
            this.grbVisualization.Controls.Add(this.label6);
            this.grbVisualization.Location = new System.Drawing.Point(198, 10);
            this.grbVisualization.Name = "grbVisualization";
            this.grbVisualization.Size = new System.Drawing.Size(172, 221);
            this.grbVisualization.TabIndex = 59;
            this.grbVisualization.TabStop = false;
            this.grbVisualization.Text = "Choropleth Map";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 183);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "Outline Line Color: ";
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
            this.cboGCClassify.Location = new System.Drawing.Point(10, 39);
            this.cboGCClassify.Name = "cboGCClassify";
            this.cboGCClassify.Size = new System.Drawing.Size(148, 21);
            this.cboGCClassify.TabIndex = 39;
            this.cboGCClassify.Text = "Natural Breaks";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Classification Method";
            // 
            // picGCLineColor
            // 
            this.picGCLineColor.BackColor = System.Drawing.Color.Black;
            this.picGCLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGCLineColor.Location = new System.Drawing.Point(102, 178);
            this.picGCLineColor.Name = "picGCLineColor";
            this.picGCLineColor.Size = new System.Drawing.Size(55, 21);
            this.picGCLineColor.TabIndex = 14;
            this.picGCLineColor.TabStop = false;
            this.picGCLineColor.Click += new System.EventHandler(this.picGCLineColor_Click);
            // 
            // nudGCLinewidth
            // 
            this.nudGCLinewidth.DecimalPlaces = 1;
            this.nudGCLinewidth.Location = new System.Drawing.Point(103, 148);
            this.nudGCLinewidth.Name = "nudGCLinewidth";
            this.nudGCLinewidth.Size = new System.Drawing.Size(55, 20);
            this.nudGCLinewidth.TabIndex = 13;
            this.nudGCLinewidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 150);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Outline Line Width: ";
            // 
            // picSymbolTo
            // 
            this.picSymbolTo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.picSymbolTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSymbolTo.Location = new System.Drawing.Point(102, 122);
            this.picSymbolTo.Name = "picSymbolTo";
            this.picSymbolTo.Size = new System.Drawing.Size(55, 21);
            this.picSymbolTo.TabIndex = 11;
            this.picSymbolTo.TabStop = false;
            this.picSymbolTo.Click += new System.EventHandler(this.picSymbolTo_Click);
            // 
            // picSymolfrom
            // 
            this.picSymolfrom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.picSymolfrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSymolfrom.Location = new System.Drawing.Point(23, 122);
            this.picSymolfrom.Name = "picSymolfrom";
            this.picSymolfrom.Size = new System.Drawing.Size(55, 21);
            this.picSymolfrom.TabIndex = 10;
            this.picSymolfrom.TabStop = false;
            this.picSymolfrom.Click += new System.EventHandler(this.picSymolfrom_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Symbol Color:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(103, 64);
            this.nudGCNClasses.Name = "nudGCNClasses";
            this.nudGCNClasses.Size = new System.Drawing.Size(55, 20);
            this.nudGCNClasses.TabIndex = 2;
            this.nudGCNClasses.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Number of Class:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudTeNClasses);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.nudTeLinewidth);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.picTeLineColor);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.nudSeperationTo);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.nudSeperationFrom);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.nudAngleFrom);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Location = new System.Drawing.Point(376, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(193, 219);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "The Robustness";
            // 
            // nudTeNClasses
            // 
            this.nudTeNClasses.Location = new System.Drawing.Point(101, 21);
            this.nudTeNClasses.Name = "nudTeNClasses";
            this.nudTeNClasses.Size = new System.Drawing.Size(47, 20);
            this.nudTeNClasses.TabIndex = 74;
            this.nudTeNClasses.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "Number of Class:";
            // 
            // nudTeLinewidth
            // 
            this.nudTeLinewidth.DecimalPlaces = 1;
            this.nudTeLinewidth.Location = new System.Drawing.Point(93, 73);
            this.nudTeLinewidth.Name = "nudTeLinewidth";
            this.nudTeLinewidth.Size = new System.Drawing.Size(55, 20);
            this.nudTeLinewidth.TabIndex = 72;
            this.nudTeLinewidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(5, 76);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(61, 13);
            this.label20.TabIndex = 71;
            this.label20.Text = "Line width: ";
            // 
            // picTeLineColor
            // 
            this.picTeLineColor.BackColor = System.Drawing.Color.Black;
            this.picTeLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picTeLineColor.Location = new System.Drawing.Point(93, 47);
            this.picTeLineColor.Name = "picTeLineColor";
            this.picTeLineColor.Size = new System.Drawing.Size(55, 21);
            this.picTeLineColor.TabIndex = 69;
            this.picTeLineColor.TabStop = false;
            this.picTeLineColor.Click += new System.EventHandler(this.picTeLineColor_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(4, 51);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 13);
            this.label19.TabIndex = 67;
            this.label19.Text = "Symbol Color:";
            // 
            // nudSeperationTo
            // 
            this.nudSeperationTo.Location = new System.Drawing.Point(105, 114);
            this.nudSeperationTo.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSeperationTo.Name = "nudSeperationTo";
            this.nudSeperationTo.Size = new System.Drawing.Size(43, 20);
            this.nudSeperationTo.TabIndex = 66;
            this.nudSeperationTo.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(83, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 13);
            this.label13.TabIndex = 65;
            this.label13.Text = "To:";
            // 
            // nudSeperationFrom
            // 
            this.nudSeperationFrom.Location = new System.Drawing.Point(40, 114);
            this.nudSeperationFrom.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSeperationFrom.Name = "nudSeperationFrom";
            this.nudSeperationFrom.Size = new System.Drawing.Size(43, 20);
            this.nudSeperationFrom.TabIndex = 64;
            this.nudSeperationFrom.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 116);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 63;
            this.label14.Text = "From:";
            // 
            // nudAngleFrom
            // 
            this.nudAngleFrom.Location = new System.Drawing.Point(105, 142);
            this.nudAngleFrom.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudAngleFrom.Name = "nudAngleFrom";
            this.nudAngleFrom.Size = new System.Drawing.Size(43, 20);
            this.nudAngleFrom.TabIndex = 70;
            this.nudAngleFrom.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 98);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 13);
            this.label15.TabIndex = 62;
            this.label15.Text = "Seperations:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 144);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(42, 13);
            this.label18.TabIndex = 68;
            this.label18.Text = "Angles:";
            // 
            // chkRobustness
            // 
            this.chkRobustness.AutoSize = true;
            this.chkRobustness.Location = new System.Drawing.Point(15, 151);
            this.chkRobustness.Name = "chkRobustness";
            this.chkRobustness.Size = new System.Drawing.Size(123, 17);
            this.chkRobustness.TabIndex = 72;
            this.chkRobustness.Text = "Save the robustness";
            this.chkRobustness.UseVisualStyleBackColor = true;
            this.chkRobustness.CheckedChanged += new System.EventHandler(this.chkRobustness_CheckedChanged);
            // 
            // txtFldName
            // 
            this.txtFldName.Enabled = false;
            this.txtFldName.Location = new System.Drawing.Point(80, 171);
            this.txtFldName.Name = "txtFldName";
            this.txtFldName.Size = new System.Drawing.Size(99, 20);
            this.txtFldName.TabIndex = 73;
            this.txtFldName.Text = "Rob";
            // 
            // lblFldName
            // 
            this.lblFldName.AutoSize = true;
            this.lblFldName.Enabled = false;
            this.lblFldName.Location = new System.Drawing.Point(16, 174);
            this.lblFldName.Name = "lblFldName";
            this.lblFldName.Size = new System.Drawing.Size(63, 13);
            this.lblFldName.TabIndex = 74;
            this.lblFldName.Text = "Field Name:";
            // 
            // frmRobustness
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 243);
            this.Controls.Add(this.lblFldName);
            this.Controls.Add(this.txtFldName);
            this.Controls.Add(this.chkRobustness);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.cboUField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboValueField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grbVisualization);
            this.Name = "frmRobustness";
            this.Text = "frmRobustness";
            this.grbVisualization.ResumeLayout(false);
            this.grbVisualization.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSymbolTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSymolfrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTeNClasses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTeLinewidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTeLineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeperationTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeperationFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngleFrom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ComboBox cboUField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboValueField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grbVisualization;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboGCClassify;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picGCLineColor;
        private System.Windows.Forms.NumericUpDown nudGCLinewidth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox picSymbolTo;
        private System.Windows.Forms.PictureBox picSymolfrom;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudGCNClasses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudTeNClasses;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudTeLinewidth;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.PictureBox picTeLineColor;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.NumericUpDown nudSeperationTo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudSeperationFrom;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudAngleFrom;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ColorDialog cdColor;
        private System.Windows.Forms.CheckBox chkRobustness;
        private System.Windows.Forms.TextBox txtFldName;
        private System.Windows.Forms.Label lblFldName;
    }
}