namespace VisUncertainty
{
    partial class frmFieldProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFieldProperties));
            this.lblOption2 = new System.Windows.Forms.Label();
            this.lblOption1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtOption1 = new System.Windows.Forms.TextBox();
            this.txtOption2 = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblOption2
            // 
            this.lblOption2.AutoSize = true;
            this.lblOption2.Location = new System.Drawing.Point(16, 92);
            this.lblOption2.Name = "lblOption2";
            this.lblOption2.Size = new System.Drawing.Size(41, 12);
            this.lblOption2.TabIndex = 21;
            this.lblOption2.Text = "Scale:";
            this.lblOption2.Visible = false;
            // 
            // lblOption1
            // 
            this.lblOption1.AutoSize = true;
            this.lblOption1.Location = new System.Drawing.Point(16, 66);
            this.lblOption1.Name = "lblOption1";
            this.lblOption1.Size = new System.Drawing.Size(62, 12);
            this.lblOption1.TabIndex = 20;
            this.lblOption1.Text = "Precision:";
            this.lblOption1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 26;
            this.label2.Text = "Type:";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.Control;
            this.txtName.Location = new System.Drawing.Point(63, 7);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(159, 21);
            this.txtName.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "Name:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(136, 114);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 21);
            this.btnClose.TabIndex = 28;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtOption1
            // 
            this.txtOption1.BackColor = System.Drawing.SystemColors.Control;
            this.txtOption1.Location = new System.Drawing.Point(106, 64);
            this.txtOption1.Name = "txtOption1";
            this.txtOption1.ReadOnly = true;
            this.txtOption1.Size = new System.Drawing.Size(116, 21);
            this.txtOption1.TabIndex = 29;
            // 
            // txtOption2
            // 
            this.txtOption2.BackColor = System.Drawing.SystemColors.Control;
            this.txtOption2.Location = new System.Drawing.Point(106, 90);
            this.txtOption2.Name = "txtOption2";
            this.txtOption2.ReadOnly = true;
            this.txtOption2.Size = new System.Drawing.Size(116, 21);
            this.txtOption2.TabIndex = 30;
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.SystemColors.Control;
            this.txtType.Location = new System.Drawing.Point(63, 35);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(160, 21);
            this.txtType.TabIndex = 31;
            // 
            // frmFieldProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(238, 142);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.txtOption2);
            this.Controls.Add(this.txtOption1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblOption2);
            this.Controls.Add(this.lblOption1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFieldProperties";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.frmFieldProperties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOption2;
        private System.Windows.Forms.Label lblOption1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtOption1;
        private System.Windows.Forms.TextBox txtOption2;
        private System.Windows.Forms.TextBox txtType;
    }
}