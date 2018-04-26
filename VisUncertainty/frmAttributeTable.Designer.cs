namespace VisUncertainty
{
    partial class frmAttributeTable
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAttributeTable));
            this.dgvAttTable = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tableOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldCalculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeadContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sortAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortDescendingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteFieldToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldCalculatorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttTable)).BeginInit();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.columnHeadContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAttTable
            // 
            this.dgvAttTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAttTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAttTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAttTable.Location = new System.Drawing.Point(0, 25);
            this.dgvAttTable.Name = "dgvAttTable";
            this.dgvAttTable.RowTemplate.Height = 23;
            this.dgvAttTable.Size = new System.Drawing.Size(510, 345);
            this.dgvAttTable.TabIndex = 0;
            this.dgvAttTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAttTable_ColumnHeaderMouseClick);
            this.dgvAttTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvAttTable_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(510, 25);
            this.panel1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tableOptionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(510, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tableOptionsToolStripMenuItem
            // 
            this.tableOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFieldToolStripMenuItem,
            this.deleteFieldToolStripMenuItem,
            this.fieldCalculatorToolStripMenuItem});
            this.tableOptionsToolStripMenuItem.Name = "tableOptionsToolStripMenuItem";
            this.tableOptionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.tableOptionsToolStripMenuItem.Text = "Options";
            // 
            // addFieldToolStripMenuItem
            // 
            this.addFieldToolStripMenuItem.Name = "addFieldToolStripMenuItem";
            this.addFieldToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.addFieldToolStripMenuItem.Text = "Add Field";
            this.addFieldToolStripMenuItem.Click += new System.EventHandler(this.addFieldToolStripMenuItem_Click);
            // 
            // deleteFieldToolStripMenuItem
            // 
            this.deleteFieldToolStripMenuItem.Name = "deleteFieldToolStripMenuItem";
            this.deleteFieldToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deleteFieldToolStripMenuItem.Text = "Delete Field";
            this.deleteFieldToolStripMenuItem.Click += new System.EventHandler(this.deleteFieldToolStripMenuItem_Click);
            // 
            // fieldCalculatorToolStripMenuItem
            // 
            this.fieldCalculatorToolStripMenuItem.Name = "fieldCalculatorToolStripMenuItem";
            this.fieldCalculatorToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.fieldCalculatorToolStripMenuItem.Text = "Field Calculator";
            this.fieldCalculatorToolStripMenuItem.Click += new System.EventHandler(this.fieldCalculatorToolStripMenuItem_Click);
            // 
            // columnHeadContextMenu
            // 
            this.columnHeadContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortAsToolStripMenuItem,
            this.sortDescendingToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteFieldToolStripMenuItem1,
            this.fieldCalculatorToolStripMenuItem1,
            this.toolStripSeparator2,
            this.statisticsToolStripMenuItem,
            this.histogramToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.columnHeadContextMenu.Name = "columnHeadContextMenu";
            this.columnHeadContextMenu.Size = new System.Drawing.Size(161, 170);
            // 
            // sortAsToolStripMenuItem
            // 
            this.sortAsToolStripMenuItem.Name = "sortAsToolStripMenuItem";
            this.sortAsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.sortAsToolStripMenuItem.Text = "Sort Ascending";
            this.sortAsToolStripMenuItem.Click += new System.EventHandler(this.sortAsToolStripMenuItem_Click);
            // 
            // sortDescendingToolStripMenuItem
            // 
            this.sortDescendingToolStripMenuItem.Name = "sortDescendingToolStripMenuItem";
            this.sortDescendingToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.sortDescendingToolStripMenuItem.Text = "Sort Descending";
            this.sortDescendingToolStripMenuItem.Click += new System.EventHandler(this.sortDescendingToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(157, 6);
            // 
            // deleteFieldToolStripMenuItem1
            // 
            this.deleteFieldToolStripMenuItem1.Name = "deleteFieldToolStripMenuItem1";
            this.deleteFieldToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.deleteFieldToolStripMenuItem1.Text = "Delete Field";
            this.deleteFieldToolStripMenuItem1.Click += new System.EventHandler(this.deleteFieldToolStripMenuItem1_Click);
            // 
            // fieldCalculatorToolStripMenuItem1
            // 
            this.fieldCalculatorToolStripMenuItem1.Name = "fieldCalculatorToolStripMenuItem1";
            this.fieldCalculatorToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.fieldCalculatorToolStripMenuItem1.Text = "Field Calculator";
            this.fieldCalculatorToolStripMenuItem1.Click += new System.EventHandler(this.fieldCalculatorToolStripMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // histogramToolStripMenuItem
            // 
            this.histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            this.histogramToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.histogramToolStripMenuItem.Text = "Histogram";
            this.histogramToolStripMenuItem.Click += new System.EventHandler(this.histogramToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // frmAttributeTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 370);
            this.Controls.Add(this.dgvAttTable);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmAttributeTable";
            this.Text = "frmAttributeTable";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAttributeTable_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttTable)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.columnHeadContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dgvAttTable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tableOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFieldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fieldCalculatorToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip columnHeadContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteFieldToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fieldCalculatorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sortAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortDescendingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem histogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;

    }
}