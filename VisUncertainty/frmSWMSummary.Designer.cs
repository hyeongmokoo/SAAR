namespace VisUncertainty
{
    partial class frmSWMSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSWMSummary));
            this.txtResult = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLayer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEVs = new System.Windows.Forms.Button();
            this.btnConnectivity = new System.Windows.Forms.Button();
            this.btnHistogram = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.SystemColors.Control;
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(0, 45);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(334, 242);
            this.txtResult.TabIndex = 8;
            this.txtResult.Text = "Count:\r\nMinimum:\r\nMaximum:\r\nSum:\r\nMean:\r\nStandard deviation:\r\nMedian:\r\nIQR:";
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.txtLayer);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(334, 45);
            this.panel1.TabIndex = 11;
            // 
            // txtLayer
            // 
            this.txtLayer.BackColor = System.Drawing.SystemColors.Control;
            this.txtLayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLayer.Location = new System.Drawing.Point(55, 12);
            this.txtLayer.Name = "txtLayer";
            this.txtLayer.ReadOnly = true;
            this.txtLayer.Size = new System.Drawing.Size(206, 20);
            this.txtLayer.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Layer:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnEVs);
            this.panel2.Controls.Add(this.btnConnectivity);
            this.panel2.Controls.Add(this.btnHistogram);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 235);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(334, 52);
            this.panel2.TabIndex = 12;
            // 
            // btnEVs
            // 
            this.btnEVs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEVs.Location = new System.Drawing.Point(227, 17);
            this.btnEVs.Name = "btnEVs";
            this.btnEVs.Size = new System.Drawing.Size(100, 23);
            this.btnEVs.TabIndex = 2;
            this.btnEVs.Text = "Save EVs";
            this.btnEVs.UseVisualStyleBackColor = true;
            this.btnEVs.Click += new System.EventHandler(this.btnEVs_Click);
            // 
            // btnConnectivity
            // 
            this.btnConnectivity.Location = new System.Drawing.Point(117, 17);
            this.btnConnectivity.Name = "btnConnectivity";
            this.btnConnectivity.Size = new System.Drawing.Size(100, 23);
            this.btnConnectivity.TabIndex = 1;
            this.btnConnectivity.Text = "Connectivity Map";
            this.btnConnectivity.UseVisualStyleBackColor = true;
            this.btnConnectivity.Click += new System.EventHandler(this.btnConnectivity_Click);
            // 
            // btnHistogram
            // 
            this.btnHistogram.Location = new System.Drawing.Point(7, 17);
            this.btnHistogram.Name = "btnHistogram";
            this.btnHistogram.Size = new System.Drawing.Size(100, 23);
            this.btnHistogram.TabIndex = 0;
            this.btnHistogram.Text = "Histogram";
            this.btnHistogram.UseVisualStyleBackColor = true;
            this.btnHistogram.Click += new System.EventHandler(this.btnHistogram_Click);
            // 
            // frmSWMSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(334, 287);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSWMSummary";
            this.Text = "SWM Summary";
            this.Load += new System.EventHandler(this.frmSWMSummary_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.TextBox txtLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnHistogram;
        private System.Windows.Forms.Button btnConnectivity;
        private System.Windows.Forms.Button btnEVs;
    }
}