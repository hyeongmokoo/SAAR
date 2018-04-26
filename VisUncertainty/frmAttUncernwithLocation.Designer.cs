namespace VisUncertainty
{
    partial class frmAttUncernwithLocation
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboTargetLayer = new System.Windows.Forms.ComboBox();
            this.cboFieldName = new System.Windows.Forms.ComboBox();
            this.cboStat = new System.Windows.Forms.ComboBox();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNSimulation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtConcetration = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDirections = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rbtRandom = new System.Windows.Forms.RadioButton();
            this.rbtConstant = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cboX = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboY = new System.Windows.Forms.ComboBox();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input Points:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(111, 308);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(131, 21);
            this.cboTargetLayer.TabIndex = 11;
            // 
            // cboFieldName
            // 
            this.cboFieldName.FormattingEnabled = true;
            this.cboFieldName.Location = new System.Drawing.Point(82, 79);
            this.cboFieldName.Name = "cboFieldName";
            this.cboFieldName.Size = new System.Drawing.Size(164, 21);
            this.cboFieldName.TabIndex = 18;
            // 
            // cboStat
            // 
            this.cboStat.FormattingEnabled = true;
            this.cboStat.Items.AddRange(new object[] {
            "Counts",
            "Mean"});
            this.cboStat.Location = new System.Drawing.Point(111, 337);
            this.cboStat.Name = "cboStat";
            this.cboStat.Size = new System.Drawing.Size(131, 21);
            this.cboStat.TabIndex = 19;
            this.cboStat.Text = "Mean";
            this.cboStat.SelectedIndexChanged += new System.EventHandler(this.cboStat_SelectedIndexChanged);
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(82, 11);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(164, 21);
            this.cboSourceLayer.TabIndex = 20;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 340);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Summary Statistics:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Value Field:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtNSimulation);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtConcetration);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtDirections);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.rbtRandom);
            this.groupBox1.Controls.Add(this.rbtConstant);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtDistance);
            this.groupBox1.Location = new System.Drawing.Point(10, 121);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 182);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location Uncertainty";
            // 
            // txtNSimulation
            // 
            this.txtNSimulation.Location = new System.Drawing.Point(136, 155);
            this.txtNSimulation.Name = "txtNSimulation";
            this.txtNSimulation.Size = new System.Drawing.Size(86, 20);
            this.txtNSimulation.TabIndex = 11;
            this.txtNSimulation.Text = "10";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 158);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Number of Simulation: ";
            // 
            // txtConcetration
            // 
            this.txtConcetration.Location = new System.Drawing.Point(136, 124);
            this.txtConcetration.Name = "txtConcetration";
            this.txtConcetration.Size = new System.Drawing.Size(86, 20);
            this.txtConcetration.TabIndex = 9;
            this.txtConcetration.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(57, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Concentration:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "degrees";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Direction: ";
            // 
            // txtDirections
            // 
            this.txtDirections.Location = new System.Drawing.Point(85, 90);
            this.txtDirections.Name = "txtDirections";
            this.txtDirections.Size = new System.Drawing.Size(89, 20);
            this.txtDirections.TabIndex = 5;
            this.txtDirections.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(178, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Map units";
            // 
            // rbtRandom
            // 
            this.rbtRandom.AutoSize = true;
            this.rbtRandom.Checked = true;
            this.rbtRandom.Location = new System.Drawing.Point(105, 51);
            this.rbtRandom.Name = "rbtRandom";
            this.rbtRandom.Size = new System.Drawing.Size(127, 17);
            this.rbtRandom.TabIndex = 3;
            this.rbtRandom.TabStop = true;
            this.rbtRandom.Text = "Random from Uniform";
            this.rbtRandom.UseVisualStyleBackColor = true;
            this.rbtRandom.CheckedChanged += new System.EventHandler(this.rbtRandom_CheckedChanged);
            // 
            // rbtConstant
            // 
            this.rbtConstant.AutoSize = true;
            this.rbtConstant.Location = new System.Drawing.Point(32, 51);
            this.rbtConstant.Name = "rbtConstant";
            this.rbtConstant.Size = new System.Drawing.Size(67, 17);
            this.rbtConstant.TabIndex = 2;
            this.rbtConstant.Text = "Constant";
            this.rbtConstant.UseVisualStyleBackColor = true;
            this.rbtConstant.CheckedChanged += new System.EventHandler(this.rbtConstant_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Distance Error: ";
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(85, 22);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(89, 20);
            this.txtDistance.TabIndex = 0;
            this.txtDistance.Text = "100";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 311);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Summarized by:";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(10, 379);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(64, 25);
            this.btnGenerate.TabIndex = 25;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(182, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 25);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "X:";
            // 
            // cboX
            // 
            this.cboX.FormattingEnabled = true;
            this.cboX.Location = new System.Drawing.Point(30, 39);
            this.cboX.Name = "cboX";
            this.cboX.Size = new System.Drawing.Size(91, 21);
            this.cboX.TabIndex = 28;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(135, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "Y:";
            // 
            // cboY
            // 
            this.cboY.FormattingEnabled = true;
            this.cboY.Location = new System.Drawing.Point(155, 39);
            this.cboY.Name = "cboY";
            this.cboY.Size = new System.Drawing.Size(91, 21);
            this.cboY.TabIndex = 30;
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Enabled = false;
            this.chkEnable.Location = new System.Drawing.Point(12, 409);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(218, 17);
            this.chkEnable.TabIndex = 31;
            this.chkEnable.Text = "Enable Identify tools for simulation results";
            this.chkEnable.UseVisualStyleBackColor = true;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // frmAttUncernwithLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(266, 438);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.cboY);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cboX);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboSourceLayer);
            this.Controls.Add(this.cboStat);
            this.Controls.Add(this.cboFieldName);
            this.Controls.Add(this.cboTargetLayer);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAttUncernwithLocation";
            this.Text = "Simulation for Locational uncertainty";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.ComboBox cboFieldName;
        private System.Windows.Forms.ComboBox cboStat;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNSimulation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtConcetration;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDirections;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbtRandom;
        private System.Windows.Forms.RadioButton rbtConstant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboY;
        private System.Windows.Forms.CheckBox chkEnable;
    }
}