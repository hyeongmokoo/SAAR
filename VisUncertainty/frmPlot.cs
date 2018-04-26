using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace VisUncertainty
{
    public partial class frmPlot : Form
    {
        //private int pCurrentIdx;
        private MainForm mForm;
        //private List<string> pmultipageImage;
        private clsSnippet pSnippet = new clsSnippet();
        
        public frmPlot()
        {
            InitializeComponent();
            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;    
        }

        private void btnNextPlot_Click(object sender, EventArgs e)
        {
            mForm.intCurrentIdx++;
            pSnippet.drawCurrentChart(mForm.multipageImage, mForm.intCurrentIdx, this);
            
            if (mForm.intCurrentIdx == mForm.multipageImage.Count - 1)
                btnNextPlot.Enabled = false;
            else
                btnNextPlot.Enabled = true;
            
            if (mForm.intCurrentIdx > 0)
                btnPreviousPlot.Enabled = true;
            else
                btnPreviousPlot.Enabled = false;
        }

        private void btnPreviousPlot_Click(object sender, EventArgs e)
        {
            mForm.intCurrentIdx--;
            pSnippet.drawCurrentChart(mForm.multipageImage, mForm.intCurrentIdx, this);

            if (mForm.intCurrentIdx > 0)
                btnPreviousPlot.Enabled = true;
            else
                btnPreviousPlot.Enabled = false;

            if (mForm.intCurrentIdx == mForm.multipageImage.Count - 1)
                btnNextPlot.Enabled = false;
            else
                btnNextPlot.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
