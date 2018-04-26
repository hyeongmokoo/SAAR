namespace VisUncertainty
{
    partial class frmGeocoding
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
            this.txtRefStreet = new System.Windows.Forms.TextBox();
            this.cboStreetRef = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenFlow = new System.Windows.Forms.Button();
            this.txtSampleTable = new System.Windows.Forms.TextBox();
            this.cboYRef = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboXRef = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRefStreet
            // 
            this.txtRefStreet.Location = new System.Drawing.Point(15, 40);
            this.txtRefStreet.Name = "txtRefStreet";
            this.txtRefStreet.Size = new System.Drawing.Size(244, 20);
            this.txtRefStreet.TabIndex = 18;
            this.txtRefStreet.Text = "D:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\GeocodingUncertainty\\Data\\Volu" +
    "siaCounty\\0511\\StreetSelected.shp";
            // 
            // cboStreetRef
            // 
            this.cboStreetRef.FormattingEnabled = true;
            this.cboStreetRef.Location = new System.Drawing.Point(138, 70);
            this.cboStreetRef.Name = "cboStreetRef";
            this.cboStreetRef.Size = new System.Drawing.Size(121, 21);
            this.cboStreetRef.TabIndex = 25;
            this.cboStreetRef.Text = "Ref_ID";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Reference Street ID:";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(203, 354);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 36;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Reference File:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Output Shape(or Temp)";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(9, 39);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(244, 20);
            this.txtOutput.TabIndex = 16;
            this.txtOutput.Text = "D:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\GeocodingUncertainty\\Data\\Volu" +
    "siaCounty\\0511\\temp.shp";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtOutput);
            this.groupBox2.Location = new System.Drawing.Point(15, 261);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(263, 80);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output Setting";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtRefStreet);
            this.groupBox3.Controls.Add(this.cboStreetRef);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(15, 153);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(268, 102);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Reference Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Geocoded locations (*.shp): ";
            // 
            // btnOpenFlow
            // 
            this.btnOpenFlow.Location = new System.Drawing.Point(222, 45);
            this.btnOpenFlow.Name = "btnOpenFlow";
            this.btnOpenFlow.Size = new System.Drawing.Size(37, 23);
            this.btnOpenFlow.TabIndex = 5;
            this.btnOpenFlow.Text = "...";
            this.btnOpenFlow.UseVisualStyleBackColor = true;
            // 
            // txtSampleTable
            // 
            this.txtSampleTable.Location = new System.Drawing.Point(9, 47);
            this.txtSampleTable.Name = "txtSampleTable";
            this.txtSampleTable.Size = new System.Drawing.Size(207, 20);
            this.txtSampleTable.TabIndex = 6;
            this.txtSampleTable.Text = "D:\\Dropbox\\Projects\\2014\\NIH_withDrChun_DrGriffith\\GeocodingUncertainty\\Data\\Volu" +
    "siaCounty\\0511\\NewAddresses.shp";
            // 
            // cboYRef
            // 
            this.cboYRef.FormattingEnabled = true;
            this.cboYRef.Location = new System.Drawing.Point(137, 104);
            this.cboYRef.Name = "cboYRef";
            this.cboYRef.Size = new System.Drawing.Size(121, 21);
            this.cboYRef.TabIndex = 23;
            this.cboYRef.Text = "RefY";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.btnOpenFlow);
            this.groupBox4.Controls.Add(this.txtSampleTable);
            this.groupBox4.Controls.Add(this.cboYRef);
            this.groupBox4.Controls.Add(this.cboXRef);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(15, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(268, 135);
            this.groupBox4.TabIndex = 34;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Input";
            // 
            // cboXRef
            // 
            this.cboXRef.FormattingEnabled = true;
            this.cboXRef.Location = new System.Drawing.Point(137, 77);
            this.cboXRef.Name = "cboXRef";
            this.cboXRef.Size = new System.Drawing.Size(121, 21);
            this.cboXRef.TabIndex = 22;
            this.cboXRef.Text = "RefX";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "X Coordinate Reference:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Y Coordinate Reference: ";
            // 
            // frmGeocoding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 389);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Name = "frmGeocoding";
            this.Text = "frmGeocoding";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRefStreet;
        private System.Windows.Forms.ComboBox cboStreetRef;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenFlow;
        private System.Windows.Forms.TextBox txtSampleTable;
        private System.Windows.Forms.ComboBox cboYRef;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboXRef;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}