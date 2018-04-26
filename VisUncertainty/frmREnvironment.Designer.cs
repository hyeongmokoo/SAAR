namespace VisUncertainty
{
    partial class frmREnvironment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmREnvironment));
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAddLibHome = new System.Windows.Forms.Button();
            this.lvReqPacks = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInstalled = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.txtRHomePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fbdLibrariesHome = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(195, 224);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(152, 21);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.Text = "Update Availability";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAddLibHome
            // 
            this.btnAddLibHome.Location = new System.Drawing.Point(195, 71);
            this.btnAddLibHome.Name = "btnAddLibHome";
            this.btnAddLibHome.Size = new System.Drawing.Size(152, 21);
            this.btnAddLibHome.TabIndex = 11;
            this.btnAddLibHome.Text = "Add Libraries Home";
            this.btnAddLibHome.UseVisualStyleBackColor = true;
            this.btnAddLibHome.Click += new System.EventHandler(this.btnAddLibHome_Click);
            // 
            // lvReqPacks
            // 
            this.lvReqPacks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colInstalled});
            this.lvReqPacks.Location = new System.Drawing.Point(15, 114);
            this.lvReqPacks.Name = "lvReqPacks";
            this.lvReqPacks.Size = new System.Drawing.Size(331, 106);
            this.lvReqPacks.TabIndex = 10;
            this.lvReqPacks.UseCompatibleStateImageBehavior = false;
            this.lvReqPacks.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 110;
            // 
            // colInstalled
            // 
            this.colInstalled.Text = "Installed";
            this.colInstalled.Width = 134;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "Required Packages:";
            // 
            // txtRHomePath
            // 
            this.txtRHomePath.Location = new System.Drawing.Point(14, 22);
            this.txtRHomePath.Multiline = true;
            this.txtRHomePath.Name = "txtRHomePath";
            this.txtRHomePath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRHomePath.Size = new System.Drawing.Size(332, 44);
            this.txtRHomePath.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "R Libraries Home:";
            // 
            // frmREnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 253);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAddLibHome);
            this.Controls.Add(this.lvReqPacks);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRHomePath);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmREnvironment";
            this.Text = "R Environment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAddLibHome;
        private System.Windows.Forms.ListView lvReqPacks;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colInstalled;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRHomePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog fbdLibrariesHome;
    }
}