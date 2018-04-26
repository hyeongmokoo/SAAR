namespace VisUncertainty
{
    partial class frmScatterplot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScatterplot));
            this.chkUseSelected = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnScatterplot = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.cboVariable1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboVariable2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkUseSelected
            // 
            this.chkUseSelected.AutoSize = true;
            this.chkUseSelected.Location = new System.Drawing.Point(139, 152);
            this.chkUseSelected.Name = "chkUseSelected";
            this.chkUseSelected.Size = new System.Drawing.Size(152, 16);
            this.chkUseSelected.TabIndex = 47;
            this.chkUseSelected.Text = "Use Selected Features";
            this.chkUseSelected.UseVisualStyleBackColor = true;
            this.chkUseSelected.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(170, 142);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(119, 23);
            this.btnCancel.TabIndex = 46;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnScatterplot
            // 
            this.btnScatterplot.Location = new System.Drawing.Point(13, 142);
            this.btnScatterplot.Name = "btnScatterplot";
            this.btnScatterplot.Size = new System.Drawing.Size(119, 23);
            this.btnScatterplot.TabIndex = 45;
            this.btnScatterplot.Text = "Create Scatterplot";
            this.btnScatterplot.UseVisualStyleBackColor = true;
            this.btnScatterplot.Click += new System.EventHandler(this.btnScatterplot_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 42;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(15, 23);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(273, 20);
            this.cboTargetLayer.TabIndex = 41;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // cboVariable1
            // 
            this.cboVariable1.FormattingEnabled = true;
            this.cboVariable1.Location = new System.Drawing.Point(14, 65);
            this.cboVariable1.Name = "cboVariable1";
            this.cboVariable1.Size = new System.Drawing.Size(276, 20);
            this.cboVariable1.TabIndex = 58;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 12);
            this.label2.TabIndex = 57;
            this.label2.Text = "Field 1 (X-Axis):";
            // 
            // cboVariable2
            // 
            this.cboVariable2.FormattingEnabled = true;
            this.cboVariable2.Location = new System.Drawing.Point(14, 109);
            this.cboVariable2.Name = "cboVariable2";
            this.cboVariable2.Size = new System.Drawing.Size(275, 20);
            this.cboVariable2.TabIndex = 60;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 12);
            this.label5.TabIndex = 59;
            this.label5.Text = "Field 2 (Y-Axis):";
            // 
            // frmScatterplot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(303, 177);
            this.Controls.Add(this.cboVariable2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboVariable1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnScatterplot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Controls.Add(this.chkUseSelected);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScatterplot";
            this.Text = "Scatterplot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUseSelected;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnScatterplot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.ComboBox cboVariable1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboVariable2;
        private System.Windows.Forms.Label label5;
    }
}