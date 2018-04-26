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
            this.tbpGeneral = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblLayerName = new System.Windows.Forms.TextBox();
            this.lblLayerProjection = new System.Windows.Forms.TextBox();
            this.tbcProperties = new System.Windows.Forms.TabControl();
            this.btnClose = new System.Windows.Forms.Button();
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
            this.tbpGeneral.Size = new System.Drawing.Size(502, 228);
            this.tbpGeneral.TabIndex = 1;
            this.tbpGeneral.Text = "General";
            this.tbpGeneral.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Coordinate:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "Layer Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "Date Source:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.SystemColors.Window;
            this.txtFilePath.Location = new System.Drawing.Point(93, 102);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(383, 78);
            this.txtFilePath.TabIndex = 18;
            // 
            // lblLayerName
            // 
            this.lblLayerName.BackColor = System.Drawing.SystemColors.Window;
            this.lblLayerName.Location = new System.Drawing.Point(93, 11);
            this.lblLayerName.Name = "lblLayerName";
            this.lblLayerName.ReadOnly = true;
            this.lblLayerName.Size = new System.Drawing.Size(295, 21);
            this.lblLayerName.TabIndex = 19;
            // 
            // lblLayerProjection
            // 
            this.lblLayerProjection.BackColor = System.Drawing.SystemColors.Window;
            this.lblLayerProjection.Location = new System.Drawing.Point(93, 40);
            this.lblLayerProjection.Multiline = true;
            this.lblLayerProjection.Name = "lblLayerProjection";
            this.lblLayerProjection.ReadOnly = true;
            this.lblLayerProjection.Size = new System.Drawing.Size(383, 52);
            this.lblLayerProjection.TabIndex = 20;
            // 
            // tbcProperties
            // 
            this.tbcProperties.Controls.Add(this.tbpGeneral);
            this.tbcProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcProperties.Location = new System.Drawing.Point(0, 0);
            this.tbcProperties.Name = "tbcProperties";
            this.tbcProperties.SelectedIndex = 0;
            this.tbcProperties.Size = new System.Drawing.Size(510, 254);
            this.tbcProperties.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(378, 186);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 23);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmProperties2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 254);
            this.Controls.Add(this.tbcProperties);
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