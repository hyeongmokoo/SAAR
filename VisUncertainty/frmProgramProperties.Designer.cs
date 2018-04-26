namespace VisUncertainty
{
    partial class frmProgramProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgramProperties));
            this.tbProperties = new System.Windows.Forms.TabControl();
            this.tbpGeneral = new System.Windows.Forms.TabPage();
            this.txtREnviron = new System.Windows.Forms.TextBox();
            this.btnCloseGen = new System.Windows.Forms.Button();
            this.btnApplyGen = new System.Windows.Forms.Button();
            this.nudFeatureCnt = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.fbdLibrariesHome = new System.Windows.Forms.FolderBrowserDialog();
            this.tbProperties.SuspendLayout();
            this.tbpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFeatureCnt)).BeginInit();
            this.SuspendLayout();
            // 
            // tbProperties
            // 
            this.tbProperties.Controls.Add(this.tbpGeneral);
            this.tbProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbProperties.Location = new System.Drawing.Point(0, 0);
            this.tbProperties.Name = "tbProperties";
            this.tbProperties.SelectedIndex = 0;
            this.tbProperties.Size = new System.Drawing.Size(363, 277);
            this.tbProperties.TabIndex = 0;
            // 
            // tbpGeneral
            // 
            this.tbpGeneral.Controls.Add(this.txtREnviron);
            this.tbpGeneral.Controls.Add(this.btnCloseGen);
            this.tbpGeneral.Controls.Add(this.btnApplyGen);
            this.tbpGeneral.Controls.Add(this.nudFeatureCnt);
            this.tbpGeneral.Controls.Add(this.label2);
            this.tbpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbpGeneral.Name = "tbpGeneral";
            this.tbpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbpGeneral.Size = new System.Drawing.Size(355, 251);
            this.tbpGeneral.TabIndex = 0;
            this.tbpGeneral.Text = "General";
            this.tbpGeneral.UseVisualStyleBackColor = true;
            // 
            // txtREnviron
            // 
            this.txtREnviron.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtREnviron.Location = new System.Drawing.Point(7, 226);
            this.txtREnviron.Name = "txtREnviron";
            this.txtREnviron.ReadOnly = true;
            this.txtREnviron.Size = new System.Drawing.Size(76, 14);
            this.txtREnviron.TabIndex = 4;
            this.txtREnviron.DoubleClick += new System.EventHandler(this.txtREnviron_DoubleClick);
            // 
            // btnCloseGen
            // 
            this.btnCloseGen.Location = new System.Drawing.Point(225, 224);
            this.btnCloseGen.Name = "btnCloseGen";
            this.btnCloseGen.Size = new System.Drawing.Size(119, 21);
            this.btnCloseGen.TabIndex = 3;
            this.btnCloseGen.Text = "Close";
            this.btnCloseGen.UseVisualStyleBackColor = true;
            this.btnCloseGen.Click += new System.EventHandler(this.btnCloseGen_Click);
            // 
            // btnApplyGen
            // 
            this.btnApplyGen.Location = new System.Drawing.Point(99, 224);
            this.btnApplyGen.Name = "btnApplyGen";
            this.btnApplyGen.Size = new System.Drawing.Size(119, 21);
            this.btnApplyGen.TabIndex = 2;
            this.btnApplyGen.Text = "Apply";
            this.btnApplyGen.UseVisualStyleBackColor = true;
            // 
            // nudFeatureCnt
            // 
            this.nudFeatureCnt.Location = new System.Drawing.Point(225, 6);
            this.nudFeatureCnt.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudFeatureCnt.Name = "nudFeatureCnt";
            this.nudFeatureCnt.Size = new System.Drawing.Size(119, 21);
            this.nudFeatureCnt.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Minimum Feature Count for Warning:";
            // 
            // frmProgramProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(363, 277);
            this.Controls.Add(this.tbProperties);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgramProperties";
            this.Text = "Program Properties";
            this.tbProperties.ResumeLayout(false);
            this.tbpGeneral.ResumeLayout(false);
            this.tbpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFeatureCnt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbProperties;
        private System.Windows.Forms.TabPage tbpGeneral;
        private System.Windows.Forms.Button btnCloseGen;
        private System.Windows.Forms.Button btnApplyGen;
        private System.Windows.Forms.NumericUpDown nudFeatureCnt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog fbdLibrariesHome;
        private System.Windows.Forms.TextBox txtREnviron;
    }
}