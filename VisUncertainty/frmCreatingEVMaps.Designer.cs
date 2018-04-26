namespace VisUncertainty
{
    partial class frmCreatingEVMaps
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
            this.label10 = new System.Windows.Forms.Label();
            this.picGCLineColor = new System.Windows.Forms.PictureBox();
            this.nudGCLinewidth = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.lstSelEVs = new System.Windows.Forms.ListBox();
            this.lstEVMaps = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboColorRamp = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudGCNClasses = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cboGCClassify = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 134);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 71;
            this.label10.Text = "Outline Line Color: ";
            // 
            // picGCLineColor
            // 
            this.picGCLineColor.BackColor = System.Drawing.Color.Black;
            this.picGCLineColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picGCLineColor.Location = new System.Drawing.Point(192, 132);
            this.picGCLineColor.Name = "picGCLineColor";
            this.picGCLineColor.Size = new System.Drawing.Size(55, 21);
            this.picGCLineColor.TabIndex = 70;
            this.picGCLineColor.TabStop = false;
            this.picGCLineColor.Click += new System.EventHandler(this.picGCLineColor_Click);
            // 
            // nudGCLinewidth
            // 
            this.nudGCLinewidth.DecimalPlaces = 1;
            this.nudGCLinewidth.Location = new System.Drawing.Point(192, 106);
            this.nudGCLinewidth.Name = "nudGCLinewidth";
            this.nudGCLinewidth.Size = new System.Drawing.Size(55, 20);
            this.nudGCLinewidth.TabIndex = 69;
            this.nudGCLinewidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 108);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 13);
            this.label12.TabIndex = 68;
            this.label12.Text = "Outline Line Width: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 83;
            this.label1.Text = "EVs for Map";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 82;
            this.label3.Text = "Selected EVs";
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Location = new System.Drawing.Point(134, 74);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(28, 23);
            this.btnMoveLeft.TabIndex = 81;
            this.btnMoveLeft.Text = "<";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Location = new System.Drawing.Point(134, 45);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(28, 23);
            this.btnMoveRight.TabIndex = 80;
            this.btnMoveRight.Text = ">";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // lstSelEVs
            // 
            this.lstSelEVs.FormattingEnabled = true;
            this.lstSelEVs.Location = new System.Drawing.Point(15, 29);
            this.lstSelEVs.Name = "lstSelEVs";
            this.lstSelEVs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelEVs.Size = new System.Drawing.Size(113, 121);
            this.lstSelEVs.TabIndex = 79;
            this.lstSelEVs.DoubleClick += new System.EventHandler(this.lstSelEVs_DoubleClick);
            // 
            // lstEVMaps
            // 
            this.lstEVMaps.FormattingEnabled = true;
            this.lstEVMaps.Location = new System.Drawing.Point(168, 32);
            this.lstEVMaps.Name = "lstEVMaps";
            this.lstEVMaps.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstEVMaps.Size = new System.Drawing.Size(113, 121);
            this.lstEVMaps.TabIndex = 78;
            this.lstEVMaps.DoubleClick += new System.EventHandler(this.lstEVMaps_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboColorRamp);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudGCNClasses);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboGCClassify);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.picGCLineColor);
            this.groupBox1.Controls.Add(this.nudGCLinewidth);
            this.groupBox1.Location = new System.Drawing.Point(15, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 164);
            this.groupBox1.TabIndex = 84;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choropleth map properties";
            // 
            // cboColorRamp
            // 
            this.cboColorRamp.FormattingEnabled = true;
            this.cboColorRamp.Items.AddRange(new object[] {
            "Blue to Red",
            "Green to Purple",
            "Green to Red",
            "Purple to Brown"});
            this.cboColorRamp.Location = new System.Drawing.Point(113, 51);
            this.cboColorRamp.Name = "cboColorRamp";
            this.cboColorRamp.Size = new System.Drawing.Size(134, 21);
            this.cboColorRamp.TabIndex = 90;
            this.cboColorRamp.Text = "Blue to Red";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 89;
            this.label2.Text = "Color Ramp:";
            // 
            // nudGCNClasses
            // 
            this.nudGCNClasses.Location = new System.Drawing.Point(192, 77);
            this.nudGCNClasses.Name = "nudGCNClasses";
            this.nudGCNClasses.Size = new System.Drawing.Size(55, 20);
            this.nudGCNClasses.TabIndex = 71;
            this.nudGCNClasses.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 70;
            this.label6.Text = "Number of Class:";
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
            this.cboGCClassify.Location = new System.Drawing.Point(127, 24);
            this.cboGCClassify.Name = "cboGCClassify";
            this.cboGCClassify.Size = new System.Drawing.Size(120, 21);
            this.cboGCClassify.TabIndex = 69;
            this.cboGCClassify.Text = "Equal Interval";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 68;
            this.label4.Text = "Classification Method:";
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(25, 331);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(103, 23);
            this.btnDraw.TabIndex = 85;
            this.btnDraw.Text = "Creating Maps";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(161, 331);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(103, 23);
            this.btnCancel.TabIndex = 86;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmCreatingEVMaps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 364);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnMoveLeft);
            this.Controls.Add(this.btnMoveRight);
            this.Controls.Add(this.lstSelEVs);
            this.Controls.Add(this.lstEVMaps);
            this.Name = "frmCreatingEVMaps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmCreatingEVMaps";
            this.Load += new System.EventHandler(this.frmCreatingEVMaps_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picGCLineColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCLinewidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGCNClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox picGCLineColor;
        private System.Windows.Forms.NumericUpDown nudGCLinewidth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.ListBox lstSelEVs;
        private System.Windows.Forms.ListBox lstEVMaps;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudGCNClasses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboGCClassify;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDraw;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColorDialog cdColor;
        private System.Windows.Forms.ComboBox cboColorRamp;
        private System.Windows.Forms.Label label2;
    }
}