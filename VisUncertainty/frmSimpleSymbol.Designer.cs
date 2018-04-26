namespace VisUncertainty
{
    partial class frmSimpleSymbol
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
            this.lblSimSymColor = new System.Windows.Forms.Label();
            this.picSymColor = new System.Windows.Forms.PictureBox();
            this.lblSimOutColor = new System.Windows.Forms.Label();
            this.nudOutWidth = new System.Windows.Forms.NumericUpDown();
            this.lblSimOutWidth = new System.Windows.Forms.Label();
            this.picOutColor = new System.Windows.Forms.PictureBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cdColor = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picSymColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOutWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOutColor)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSimSymColor
            // 
            this.lblSimSymColor.AutoSize = true;
            this.lblSimSymColor.Location = new System.Drawing.Point(16, 21);
            this.lblSimSymColor.Name = "lblSimSymColor";
            this.lblSimSymColor.Size = new System.Drawing.Size(86, 12);
            this.lblSimSymColor.TabIndex = 69;
            this.lblSimSymColor.Text = "Symbol Color:";
            // 
            // picSymColor
            // 
            this.picSymColor.BackColor = System.Drawing.Color.Beige;
            this.picSymColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picSymColor.Location = new System.Drawing.Point(141, 17);
            this.picSymColor.Name = "picSymColor";
            this.picSymColor.Size = new System.Drawing.Size(63, 20);
            this.picSymColor.TabIndex = 68;
            this.picSymColor.TabStop = false;
            this.picSymColor.Click += new System.EventHandler(this.picSymColor_Click);
            // 
            // lblSimOutColor
            // 
            this.lblSimOutColor.AutoSize = true;
            this.lblSimOutColor.Location = new System.Drawing.Point(16, 73);
            this.lblSimOutColor.Name = "lblSimOutColor";
            this.lblSimOutColor.Size = new System.Drawing.Size(110, 12);
            this.lblSimOutColor.TabIndex = 67;
            this.lblSimOutColor.Text = "Outline Line Color:";
            // 
            // nudOutWidth
            // 
            this.nudOutWidth.DecimalPlaces = 1;
            this.nudOutWidth.Location = new System.Drawing.Point(140, 43);
            this.nudOutWidth.Name = "nudOutWidth";
            this.nudOutWidth.Size = new System.Drawing.Size(64, 21);
            this.nudOutWidth.TabIndex = 65;
            this.nudOutWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // lblSimOutWidth
            // 
            this.lblSimOutWidth.AutoSize = true;
            this.lblSimOutWidth.Location = new System.Drawing.Point(16, 46);
            this.lblSimOutWidth.Name = "lblSimOutWidth";
            this.lblSimOutWidth.Size = new System.Drawing.Size(110, 12);
            this.lblSimOutWidth.TabIndex = 64;
            this.lblSimOutWidth.Text = "Outline Line Width:";
            // 
            // picOutColor
            // 
            this.picOutColor.BackColor = System.Drawing.Color.Gray;
            this.picOutColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picOutColor.Location = new System.Drawing.Point(141, 69);
            this.picOutColor.Name = "picOutColor";
            this.picOutColor.Size = new System.Drawing.Size(63, 20);
            this.picOutColor.TabIndex = 66;
            this.picOutColor.TabStop = false;
            this.picOutColor.Click += new System.EventHandler(this.picOutColor_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(18, 99);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 70;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(130, 99);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 71;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSimpleSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 142);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblSimSymColor);
            this.Controls.Add(this.picSymColor);
            this.Controls.Add(this.lblSimOutColor);
            this.Controls.Add(this.nudOutWidth);
            this.Controls.Add(this.lblSimOutWidth);
            this.Controls.Add(this.picOutColor);
            this.Name = "frmSimpleSymbol";
            this.Text = "frmSimpleSymbol";
            ((System.ComponentModel.ISupportInitialize)(this.picSymColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOutWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOutColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSimSymColor;
        public System.Windows.Forms.PictureBox picSymColor;
        private System.Windows.Forms.Label lblSimOutColor;
        public System.Windows.Forms.NumericUpDown nudOutWidth;
        private System.Windows.Forms.Label lblSimOutWidth;
        public System.Windows.Forms.PictureBox picOutColor;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColorDialog cdColor;
    }
}