namespace VisUncertainty
{
    partial class frmBlinking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlinking));
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudFlikerRate = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudTransparency = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboWeight = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboCoClassify = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.nudCoNClasses = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlikerRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoNClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(15, 25);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(187, 21);
            this.cboSourceLayer.TabIndex = 23;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Layer:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Flicker Rate (Second):";
            // 
            // nudFlikerRate
            // 
            this.nudFlikerRate.DecimalPlaces = 2;
            this.nudFlikerRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudFlikerRate.Location = new System.Drawing.Point(128, 51);
            this.nudFlikerRate.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nudFlikerRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFlikerRate.Name = "nudFlikerRate";
            this.nudFlikerRate.Size = new System.Drawing.Size(74, 20);
            this.nudFlikerRate.TabIndex = 25;
            this.nudFlikerRate.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Transparency (%):";
            // 
            // nudTransparency
            // 
            this.nudTransparency.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudTransparency.Location = new System.Drawing.Point(128, 79);
            this.nudTransparency.Name = "nudTransparency";
            this.nudTransparency.Size = new System.Drawing.Size(74, 20);
            this.nudTransparency.TabIndex = 27;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(18, 266);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(111, 23);
            this.btnStart.TabIndex = 28;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(143, 266);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(59, 23);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboWeight);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboCoClassify);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.nudCoNClasses);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Location = new System.Drawing.Point(18, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 142);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Uncertainty";
            // 
            // cboWeight
            // 
            this.cboWeight.FormattingEnabled = true;
            this.cboWeight.Location = new System.Drawing.Point(19, 34);
            this.cboWeight.Name = "cboWeight";
            this.cboWeight.Size = new System.Drawing.Size(148, 21);
            this.cboWeight.TabIndex = 62;
            this.cboWeight.Text = "None";
            this.cboWeight.SelectedIndexChanged += new System.EventHandler(this.cboWeight_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 61;
            this.label4.Text = "Field:";
            // 
            // cboCoClassify
            // 
            this.cboCoClassify.Enabled = false;
            this.cboCoClassify.FormattingEnabled = true;
            this.cboCoClassify.Items.AddRange(new object[] {
            "Equal Interval",
            "Geometrical Interval",
            "Natural Breaks",
            "Quantile",
            "StandardDeviation"});
            this.cboCoClassify.Location = new System.Drawing.Point(19, 82);
            this.cboCoClassify.Name = "cboCoClassify";
            this.cboCoClassify.Size = new System.Drawing.Size(148, 21);
            this.cboCoClassify.TabIndex = 60;
            this.cboCoClassify.Text = "Natural Breaks";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 66);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 13);
            this.label16.TabIndex = 59;
            this.label16.Text = "Classification Method";
            // 
            // nudCoNClasses
            // 
            this.nudCoNClasses.Enabled = false;
            this.nudCoNClasses.Location = new System.Drawing.Point(112, 114);
            this.nudCoNClasses.Name = "nudCoNClasses";
            this.nudCoNClasses.Size = new System.Drawing.Size(55, 20);
            this.nudCoNClasses.TabIndex = 58;
            this.nudCoNClasses.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(16, 116);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(87, 13);
            this.label22.TabIndex = 57;
            this.label22.Text = "Number of Class:";
            // 
            // frmBlinking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(225, 301);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.nudTransparency);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudFlikerRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBlinking";
            this.Text = "Blinking";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBlinking_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nudFlikerRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTransparency)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoNClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudFlikerRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudTransparency;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboWeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboCoClassify;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown nudCoNClasses;
        private System.Windows.Forms.Label label22;
    }
}