namespace VisUncertainty
{
    partial class frmClassification
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
            this.cboCoClassify = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.nudCoNClasses = new System.Windows.Forms.NumericUpDown();
            this.label22 = new System.Windows.Forms.Label();
            this.cboWeight = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudCoNClasses)).BeginInit();
            this.SuspendLayout();
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
            this.cboCoClassify.Location = new System.Drawing.Point(15, 76);
            this.cboCoClassify.Name = "cboCoClassify";
            this.cboCoClassify.Size = new System.Drawing.Size(148, 21);
            this.cboCoClassify.TabIndex = 54;
            this.cboCoClassify.Text = "Natural Breaks";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 60);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 13);
            this.label16.TabIndex = 53;
            this.label16.Text = "Classification Method";
            // 
            // nudCoNClasses
            // 
            this.nudCoNClasses.Enabled = false;
            this.nudCoNClasses.Location = new System.Drawing.Point(108, 101);
            this.nudCoNClasses.Name = "nudCoNClasses";
            this.nudCoNClasses.Size = new System.Drawing.Size(55, 20);
            this.nudCoNClasses.TabIndex = 52;
            this.nudCoNClasses.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 103);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(87, 13);
            this.label22.TabIndex = 51;
            this.label22.Text = "Number of Class:";
            // 
            // cboWeight
            // 
            this.cboWeight.FormattingEnabled = true;
            this.cboWeight.Location = new System.Drawing.Point(15, 25);
            this.cboWeight.Name = "cboWeight";
            this.cboWeight.Size = new System.Drawing.Size(148, 21);
            this.cboWeight.TabIndex = 56;
            this.cboWeight.Text = "None";
            this.cboWeight.SelectedIndexChanged += new System.EventHandler(this.cboWeight_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Weight Field:";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(88, 135);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 57;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // frmClassification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(180, 170);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.cboWeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboCoClassify);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.nudCoNClasses);
            this.Controls.Add(this.label22);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClassification";
            this.Text = "frmClassification";
            this.Shown += new System.EventHandler(this.frmClassification_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudCoNClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboCoClassify;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown nudCoNClasses;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ComboBox cboWeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnApply;
    }
}