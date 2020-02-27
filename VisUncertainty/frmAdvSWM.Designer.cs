namespace VisUncertainty
{
    partial class frmAdvSWM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdvSWM));
            this.chkCumulate = new System.Windows.Forms.CheckBox();
            this.nudAdvanced = new System.Windows.Forms.NumericUpDown();
            this.lblAdvanced = new System.Windows.Forms.Label();
            this.txtSWM = new System.Windows.Forms.ComboBox();
            this.lblClip = new System.Windows.Forms.Label();
            this.lblGeoDa = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSubset = new System.Windows.Forms.Button();
            this.btnOpenSWM = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ofdOpenSWM = new System.Windows.Forms.OpenFileDialog();
            this.cboCoding = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudAdvanced)).BeginInit();
            this.SuspendLayout();
            // 
            // chkCumulate
            // 
            this.chkCumulate.AutoSize = true;
            this.chkCumulate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCumulate.Location = new System.Drawing.Point(12, 108);
            this.chkCumulate.Name = "chkCumulate";
            this.chkCumulate.Size = new System.Drawing.Size(122, 17);
            this.chkCumulate.TabIndex = 86;
            this.chkCumulate.Text = "Cumulate neighbors:";
            this.chkCumulate.UseVisualStyleBackColor = true;
            // 
            // nudAdvanced
            // 
            this.nudAdvanced.Location = new System.Drawing.Point(118, 80);
            this.nudAdvanced.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.nudAdvanced.Name = "nudAdvanced";
            this.nudAdvanced.Size = new System.Drawing.Size(85, 20);
            this.nudAdvanced.TabIndex = 85;
            this.nudAdvanced.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAdvanced.ValueChanged += new System.EventHandler(this.nudAdvanced_ValueChanged);
            // 
            // lblAdvanced
            // 
            this.lblAdvanced.AutoSize = true;
            this.lblAdvanced.Location = new System.Drawing.Point(12, 81);
            this.lblAdvanced.Name = "lblAdvanced";
            this.lblAdvanced.Size = new System.Drawing.Size(100, 13);
            this.lblAdvanced.TabIndex = 84;
            this.lblAdvanced.Text = "Threshold distance:";
            // 
            // txtSWM
            // 
            this.txtSWM.FormattingEnabled = true;
            this.txtSWM.Location = new System.Drawing.Point(72, 19);
            this.txtSWM.Name = "txtSWM";
            this.txtSWM.Size = new System.Drawing.Size(131, 21);
            this.txtSWM.TabIndex = 82;
            this.txtSWM.TextChanged += new System.EventHandler(this.txtSWM_TextChanged);
            // 
            // lblClip
            // 
            this.lblClip.AutoSize = true;
            this.lblClip.Location = new System.Drawing.Point(12, 135);
            this.lblClip.Name = "lblClip";
            this.lblClip.Size = new System.Drawing.Size(86, 13);
            this.lblClip.TabIndex = 83;
            this.lblClip.Text = "Clip by polygons:";
            // 
            // lblGeoDa
            // 
            this.lblGeoDa.AutoSize = true;
            this.lblGeoDa.Location = new System.Drawing.Point(12, 166);
            this.lblGeoDa.Name = "lblGeoDa";
            this.lblGeoDa.Size = new System.Drawing.Size(145, 13);
            this.lblGeoDa.TabIndex = 81;
            this.lblGeoDa.Text = "Load SWM from GeoDa files:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 78;
            this.label5.Text = "Definition:";
            // 
            // btnSubset
            // 
            this.btnSubset.Enabled = false;
            this.btnSubset.Location = new System.Drawing.Point(151, 130);
            this.btnSubset.Name = "btnSubset";
            this.btnSubset.Size = new System.Drawing.Size(52, 23);
            this.btnSubset.TabIndex = 80;
            this.btnSubset.Text = "Clip";
            this.btnSubset.UseVisualStyleBackColor = true;
            this.btnSubset.Click += new System.EventHandler(this.btnSubset_Click);
            // 
            // btnOpenSWM
            // 
            this.btnOpenSWM.Location = new System.Drawing.Point(172, 161);
            this.btnOpenSWM.Name = "btnOpenSWM";
            this.btnOpenSWM.Size = new System.Drawing.Size(31, 23);
            this.btnOpenSWM.TabIndex = 79;
            this.btnOpenSWM.Text = "...";
            this.btnOpenSWM.UseVisualStyleBackColor = true;
            this.btnOpenSWM.Click += new System.EventHandler(this.btnOpenSWM_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(22, 201);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 87;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(128, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 88;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ofdOpenSWM
            // 
            this.ofdOpenSWM.Filter = "GAL files|*.gal|GWT files|*.gwt";
            this.ofdOpenSWM.Title = "Open GAL files";
            // 
            // cboCoding
            // 
            this.cboCoding.FormattingEnabled = true;
            this.cboCoding.Location = new System.Drawing.Point(72, 48);
            this.cboCoding.Name = "cboCoding";
            this.cboCoding.Size = new System.Drawing.Size(131, 21);
            this.cboCoding.TabIndex = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 89;
            this.label1.Text = "Coding:";
            // 
            // frmAdvSWM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(223, 234);
            this.Controls.Add(this.cboCoding);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.chkCumulate);
            this.Controls.Add(this.nudAdvanced);
            this.Controls.Add(this.lblAdvanced);
            this.Controls.Add(this.txtSWM);
            this.Controls.Add(this.lblClip);
            this.Controls.Add(this.lblGeoDa);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSubset);
            this.Controls.Add(this.btnOpenSWM);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAdvSWM";
            this.Text = "Spatial Weights";
            this.Load += new System.EventHandler(this.frmAdvSWM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudAdvanced)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCumulate;
        private System.Windows.Forms.NumericUpDown nudAdvanced;
        private System.Windows.Forms.Label lblAdvanced;
        private System.Windows.Forms.Label lblClip;
        private System.Windows.Forms.Label lblGeoDa;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSubset;
        private System.Windows.Forms.Button btnOpenSWM;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog ofdOpenSWM;
        public System.Windows.Forms.ComboBox txtSWM;
        public System.Windows.Forms.ComboBox cboCoding;
        private System.Windows.Forms.Label label1;
    }
}