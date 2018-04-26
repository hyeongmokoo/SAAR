namespace VisUncertainty
{
    partial class frmUncernSAM
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
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "B-distance",
            "BD"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "B-Coefficient",
            "BC"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "Hellinger distance",
            "HD"}, -1);
            this.cboUField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboValueField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSourceLayer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboMaptype = new System.Windows.Forms.ComboBox();
            this.chkScatterplot = new System.Windows.Forms.CheckBox();
            this.lvFields = new System.Windows.Forms.ListView();
            this.colTypes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkMap = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboUField
            // 
            this.cboUField.FormattingEnabled = true;
            this.cboUField.Location = new System.Drawing.Point(18, 112);
            this.cboUField.Name = "cboUField";
            this.cboUField.Size = new System.Drawing.Size(180, 21);
            this.cboUField.TabIndex = 85;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 84;
            this.label3.Text = "Uncertainty Field:";
            // 
            // cboValueField
            // 
            this.cboValueField.FormattingEnabled = true;
            this.cboValueField.Location = new System.Drawing.Point(18, 72);
            this.cboValueField.Name = "cboValueField";
            this.cboValueField.Size = new System.Drawing.Size(180, 21);
            this.cboValueField.TabIndex = 83;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 82;
            this.label2.Text = "Value Field:";
            // 
            // cboSourceLayer
            // 
            this.cboSourceLayer.FormattingEnabled = true;
            this.cboSourceLayer.Location = new System.Drawing.Point(19, 32);
            this.cboSourceLayer.Name = "cboSourceLayer";
            this.cboSourceLayer.Size = new System.Drawing.Size(179, 21);
            this.cboSourceLayer.TabIndex = 81;
            this.cboSourceLayer.SelectedIndexChanged += new System.EventHandler(this.cboSourceLayer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 80;
            this.label1.Text = "Layer:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboUField);
            this.groupBox1.Controls.Add(this.cboSourceLayer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboValueField);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 148);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboMaptype);
            this.groupBox2.Controls.Add(this.chkScatterplot);
            this.groupBox2.Controls.Add(this.lvFields);
            this.groupBox2.Controls.Add(this.chkMap);
            this.groupBox2.Location = new System.Drawing.Point(12, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(207, 188);
            this.groupBox2.TabIndex = 124;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // cboMaptype
            // 
            this.cboMaptype.FormattingEnabled = true;
            this.cboMaptype.Items.AddRange(new object[] {
            "choropleth map",
            "point map"});
            this.cboMaptype.Location = new System.Drawing.Point(86, 129);
            this.cboMaptype.Name = "cboMaptype";
            this.cboMaptype.Size = new System.Drawing.Size(104, 21);
            this.cboMaptype.TabIndex = 119;
            this.cboMaptype.Text = "choropleth map";
            // 
            // chkScatterplot
            // 
            this.chkScatterplot.AutoSize = true;
            this.chkScatterplot.Checked = true;
            this.chkScatterplot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScatterplot.Location = new System.Drawing.Point(6, 156);
            this.chkScatterplot.Name = "chkScatterplot";
            this.chkScatterplot.Size = new System.Drawing.Size(134, 17);
            this.chkScatterplot.TabIndex = 118;
            this.chkScatterplot.Text = "Generate a scatter plot";
            this.chkScatterplot.UseVisualStyleBackColor = true;
            // 
            // lvFields
            // 
            this.lvFields.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTypes,
            this.colNames});
            this.lvFields.HoverSelection = true;
            this.lvFields.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.lvFields.LabelEdit = true;
            this.lvFields.Location = new System.Drawing.Point(5, 22);
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(193, 101);
            this.lvFields.TabIndex = 56;
            this.lvFields.UseCompatibleStateImageBehavior = false;
            this.lvFields.View = System.Windows.Forms.View.Details;
            // 
            // colTypes
            // 
            this.colTypes.Text = "Types";
            this.colTypes.Width = 102;
            // 
            // colNames
            // 
            this.colNames.Text = "Field Name";
            this.colNames.Width = 77;
            // 
            // chkMap
            // 
            this.chkMap.AutoSize = true;
            this.chkMap.Checked = true;
            this.chkMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMap.Location = new System.Drawing.Point(6, 132);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(80, 17);
            this.chkMap.TabIndex = 117;
            this.chkMap.Text = "Add a map:";
            this.chkMap.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 361);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 125;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(144, 361);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 126;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // frmUncernSAM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 416);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmUncernSAM";
            this.Text = "frmUncernSAM";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboUField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboValueField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSourceLayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboMaptype;
        private System.Windows.Forms.CheckBox chkScatterplot;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader colTypes;
        private System.Windows.Forms.ColumnHeader colNames;
        private System.Windows.Forms.CheckBox chkMap;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}