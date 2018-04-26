namespace VisUncertainty
{
    partial class frmDrawDenPlot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDrawDenPlot));
            this.cboUField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboValueField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudConfiLevel = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.chk3D = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudConfiLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // cboUField
            // 
            this.cboUField.FormattingEnabled = true;
            this.cboUField.Location = new System.Drawing.Point(14, 105);
            this.cboUField.Name = "cboUField";
            this.cboUField.Size = new System.Drawing.Size(164, 21);
            this.cboUField.TabIndex = 45;
            this.cboUField.SelectedIndexChanged += new System.EventHandler(this.cboUField_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 44;
            this.label3.Text = "Uncertainty Field:";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(14, 65);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(164, 21);
            this.cboValueField.TabIndex = 43;
            this.cboValueField.SelectedIndexChanged += new System.EventHandler(this.cboValueField_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Value Field:";
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(15, 25);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(164, 21);
            this.cboSourceLayer.TabIndex = 41;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Layer:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Confidence Level:";
            // 
            // nudConfiLevel
            // 
            this.nudConfiLevel.Location = new System.Drawing.Point(113, 133);
            this.nudConfiLevel.Name = "nudConfiLevel";
            this.nudConfiLevel.Size = new System.Drawing.Size(43, 20);
            this.nudConfiLevel.TabIndex = 47;
            this.nudConfiLevel.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.nudConfiLevel.Click += new System.EventHandler(this.nudConfiLevel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(162, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 48;
            this.label5.Text = "%";
            // 
            // chk3D
            // 
            this.chk3D.AutoSize = true;
            this.chk3D.Location = new System.Drawing.Point(17, 156);
            this.chk3D.Name = "chk3D";
            this.chk3D.Size = new System.Drawing.Size(88, 17);
            this.chk3D.TabIndex = 49;
            this.chk3D.Text = "Display in 3D";
            this.chk3D.UseVisualStyleBackColor = true;
            this.chk3D.CheckedChanged += new System.EventHandler(this.chk3D_CheckedChanged);
            // 
            // frmDrawDenPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(202, 185);
            this.Controls.Add(this.chk3D);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudConfiLevel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboUField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboValueField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDrawDenPlot";
            this.Text = "Variables";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDrawDenPlot_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.nudConfiLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cboUField;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox cboValueField;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.NumericUpDown nudConfiLevel;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox chk3D;
    }
}