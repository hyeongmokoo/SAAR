namespace VisUncertainty
{
    partial class frmProperties2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProperties2));
            this.tbpGeneral = new System.Windows.Forms.TabPage();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblLayerProjection = new System.Windows.Forms.TextBox();
            this.lblLayerName = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbcProperties = new System.Windows.Forms.TabControl();
            this.tbpGeneral.SuspendLayout();
            this.tbcProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbpGeneral
            // 
            this.tbpGeneral.Controls.Add(this.btnClose);
            this.tbpGeneral.Controls.Add(this.lblLayerProjection);
            this.tbpGeneral.Controls.Add(this.lblLayerName);
            this.tbpGeneral.Controls.Add(this.txtFilePath);
            this.tbpGeneral.Controls.Add(this.label2);
            this.tbpGeneral.Controls.Add(this.label1);
            this.tbpGeneral.Controls.Add(this.label3);
            this.tbpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbpGeneral.Name = "tbpGeneral";
            this.tbpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbpGeneral.Size = new System.Drawing.Size(429, 249);
            this.tbpGeneral.TabIndex = 1;
            this.tbpGeneral.Text = "General";
            this.tbpGeneral.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(324, 202);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 25);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblLayerProjection
            // 
            this.lblLayerProjection.BackColor = System.Drawing.SystemColors.Window;
            this.lblLayerProjection.Location = new System.Drawing.Point(80, 43);
            this.lblLayerProjection.Multiline = true;
            this.lblLayerProjection.Name = "lblLayerProjection";
            this.lblLayerProjection.ReadOnly = true;
            this.lblLayerProjection.Size = new System.Drawing.Size(329, 56);
            this.lblLayerProjection.TabIndex = 20;
            // 
            // lblLayerName
            // 
            this.lblLayerName.BackColor = System.Drawing.SystemColors.Window;
            this.lblLayerName.Location = new System.Drawing.Point(80, 12);
            this.lblLayerName.Name = "lblLayerName";
            this.lblLayerName.ReadOnly = true;
            this.lblLayerName.Size = new System.Drawing.Size(253, 20);
            this.lblLayerName.TabIndex = 19;
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.SystemColors.Window;
            this.txtFilePath.Location = new System.Drawing.Point(80, 111);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(329, 84);
            this.txtFilePath.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Date Source:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Layer Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Coordinate:";
            // 
            // tbcProperties
            // 
            this.tbcProperties.Controls.Add(this.tbpGeneral);
            this.tbcProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcProperties.Location = new System.Drawing.Point(0, 0);
            this.tbcProperties.Name = "tbcProperties";
            this.tbcProperties.SelectedIndex = 0;
            this.tbcProperties.Size = new System.Drawing.Size(437, 275);
            this.tbcProperties.TabIndex = 1;
            // 
            // frmProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 275);
            this.Controls.Add(this.tbcProperties);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProperties2";
            this.Text = "Properties";
            this.Load += new System.EventHandler(this.frmProperties2_Load);
            this.tbpGeneral.ResumeLayout(false);
            this.tbpGeneral.PerformLayout();
            this.tbcProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tbpGeneral;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox lblLayerProjection;
        private System.Windows.Forms.TextBox lblLayerName;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tbcProperties;
    }
}