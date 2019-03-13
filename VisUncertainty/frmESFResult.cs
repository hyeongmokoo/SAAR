using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisUncertainty
{
    public partial class frmESFResult : Form
    {
        public Double[,] arrSelectedEVs;
        public String[] arrSelectedEvsNM;
        public IFeatureClass m_pFClass;

        public frmESFResult()
        {
            InitializeComponent();
        }

        private void btnESFSave_Click(object sender, EventArgs e)
        {
            frmSaveESF pfrmSaveESF = new frmSaveESF();
            pfrmSaveESF.arrSelectedEVs = arrSelectedEVs;
            pfrmSaveESF.arrSelectedEvsNM = arrSelectedEvsNM;
            pfrmSaveESF.m_pFClass = m_pFClass;

            pfrmSaveESF.Show();
        }

        private void frmESFResult_Load(object sender, EventArgs e)
        {
            if (m_pFClass == null)
            {
                btnESFSave.Enabled = false;
            }
        }
    }
}
