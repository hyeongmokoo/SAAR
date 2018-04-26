namespace VisUncertainty
{
    partial class frmScaleBarProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScaleBarProperties));
            this.label1 = new System.Windows.Forms.Label();
            this.cboDivisionUnits = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudNDivisions = new System.Windows.Forms.NumericUpDown();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudNDivisions)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Division Units:";
            // 
            // cboDivisionUnits
            // 
            this.cboDivisionUnits.FormattingEnabled = true;
            this.cboDivisionUnits.Items.AddRange(new object[] {
            "Feet",
            "Miles",
            "Meters",
            "Kilometers"});
            this.cboDivisionUnits.Location = new System.Drawing.Point(110, 12);
            this.cboDivisionUnits.Name = "cboDivisionUnits";
            this.cboDivisionUnits.Size = new System.Drawing.Size(121, 20);
            this.cboDivisionUnits.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of Divisions:";
            // 
            // nudNDivisions
            // 
            this.nudNDivisions.Location = new System.Drawing.Point(148, 37);
            this.nudNDivisions.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNDivisions.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNDivisions.Name = "nudNDivisions";
            this.nudNDivisions.Size = new System.Drawing.Size(83, 21);
            this.nudNDivisions.TabIndex = 3;
            this.nudNDivisions.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(19, 61);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(87, 21);
            this.btnInsert.TabIndex = 4;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(143, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 21);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmScaleBarProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(248, 91);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.nudNDivisions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboDivisionUnits);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScaleBarProperties";
            this.Text = "Properties";
            ((System.ComponentModel.ISupportInitialize)(this.nudNDivisions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDivisionUnits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudNDivisions;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnCancel;
    }
}