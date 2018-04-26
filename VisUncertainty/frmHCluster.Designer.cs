namespace VisUncertainty
{
    partial class frmHCluster
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
            this.cboDistance = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.var = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.error = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cboAgglmethod = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nudNClasses = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputFld = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Select a Target Layer:";
            // 
            // cboTargetLayer
            // 
            this.cboTargetLayer.FormattingEnabled = true;
            this.cboTargetLayer.Location = new System.Drawing.Point(12, 29);
            this.cboTargetLayer.Name = "cboTargetLayer";
            this.cboTargetLayer.Size = new System.Drawing.Size(297, 21);
            this.cboTargetLayer.TabIndex = 20;
            this.cboTargetLayer.SelectedIndexChanged += new System.EventHandler(this.cboTargetLayer_SelectedIndexChanged);
            // 
            // cboDistance
            // 
            this.cboDistance.FormattingEnabled = true;
            this.cboDistance.Items.AddRange(new object[] {
            "Bhattacharyya distance",
            "Euclidean distance"});
            this.cboDistance.Location = new System.Drawing.Point(12, 73);
            this.cboDistance.Name = "cboDistance";
            this.cboDistance.Size = new System.Drawing.Size(297, 21);
            this.cboDistance.TabIndex = 29;
            this.cboDistance.Text = "Bhattacharyya distance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Distance measure:";
            // 
            // dgvVariables
            // 
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.var,
            this.error});
            this.dgvVariables.Location = new System.Drawing.Point(12, 166);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.Size = new System.Drawing.Size(297, 150);
            this.dgvVariables.TabIndex = 30;
            // 
            // var
            // 
            this.var.HeaderText = "Variables";
            this.var.Name = "var";
            this.var.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.var.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.var.Width = 120;
            // 
            // error
            // 
            this.error.HeaderText = "Standard errors";
            this.error.Name = "error";
            this.error.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.error.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.error.Width = 120;
            // 
            // cboAgglmethod
            // 
            this.cboAgglmethod.FormattingEnabled = true;
            this.cboAgglmethod.Items.AddRange(new object[] {
            "Single linkage",
            "Complete linkage"});
            this.cboAgglmethod.Location = new System.Drawing.Point(12, 117);
            this.cboAgglmethod.Name = "cboAgglmethod";
            this.cboAgglmethod.Size = new System.Drawing.Size(297, 21);
            this.cboAgglmethod.TabIndex = 32;
            this.cboAgglmethod.Text = "Complete linkage";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Agglomeration method:";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(194, 389);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 23);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(14, 389);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(115, 23);
            this.btnRun.TabIndex = 33;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Select variables:";
            // 
            // nudNClasses
            // 
            this.nudNClasses.Location = new System.Drawing.Point(182, 326);
            this.nudNClasses.Name = "nudNClasses";
            this.nudNClasses.Size = new System.Drawing.Size(120, 20);
            this.nudNClasses.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 329);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Number of classes:";
            // 
            // txtOutputFld
            // 
            this.txtOutputFld.Location = new System.Drawing.Point(168, 360);
            this.txtOutputFld.Name = "txtOutputFld";
            this.txtOutputFld.Size = new System.Drawing.Size(141, 20);
            this.txtOutputFld.TabIndex = 38;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 363);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Output Field Name:";
            // 
            // frmHCluster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtOutputFld);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudNClasses);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.cboAgglmethod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvVariables);
            this.Controls.Add(this.cboDistance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTargetLayer);
            this.Name = "frmHCluster";
            this.Text = "frmHCluster";
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTargetLayer;
        private System.Windows.Forms.ComboBox cboDistance;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvVariables;
        private System.Windows.Forms.ComboBox cboAgglmethod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.DataGridViewComboBoxColumn var;
        private System.Windows.Forms.DataGridViewComboBoxColumn error;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudNClasses;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOutputFld;
        private System.Windows.Forms.Label label6;
    }
}