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
    public partial class frmSaveESF : Form
    {
        public Double[,] arrSelectedEVs;
        public String[] arrSelectedEvsNM;
        public IFeatureClass m_pFClass;

        private int intNSelectedEVs;

        public frmSaveESF()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSaveESF_Load(object sender, EventArgs e)
        {
            intNSelectedEVs = arrSelectedEvsNM.Length;


            for (int i = 0; i < intNSelectedEVs; i++)
            {
                clistFields.Items.Add(arrSelectedEvsNM[i]);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clistFields.Items.Count; i++)
                clistFields.SetItemChecked(i, true);
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clistFields.Items.Count; i++)
                clistFields.SetItemChecked(i, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (clistFields.CheckedItems.Count == 0)
                {
                    MessageBox.Show("EV is not selected.");
                    return;
                }


                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Saving EVs:"; ;
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();

                pfrmProgress.lblStatus.Text = "Saving residuals and spatial filter:";

                for (int k = 0; k < clistFields.CheckedItems.Count; k++)
                {
                    int i = clistFields.CheckedIndices[k];
                    string strEVfieldName = arrSelectedEvsNM[i];
                    pfrmProgress.lblStatus.Text = "Saving EV (" + strEVfieldName + ")";

                    // Create field, if there isn't
                    if (m_pFClass.FindField(strEVfieldName) == -1)
                    {
                        //Add fields
                        IField newField = new FieldClass();
                        IFieldEdit fieldEdit = (IFieldEdit)newField;
                        fieldEdit.Name_2 = strEVfieldName;
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        m_pFClass.AddField(newField);
                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Do you want to overwrite " + strEVfieldName + " field?", "Overwrite", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                    }
                    IFeatureCursor pFCursor = m_pFClass.Update(null, false);
                    IFeature pFeature = pFCursor.NextFeature();

                    int featureIdx = 0;
                    int intValueIdx = m_pFClass.FindField(strEVfieldName);

                    while (pFeature != null)
                    {
                        //Update Residuals
                        pFeature.set_Value(intValueIdx, (object)arrSelectedEVs[featureIdx, i]);

                        pFCursor.UpdateFeature(pFeature);

                        pFeature = pFCursor.NextFeature();
                        featureIdx++;
                    }

                }
                pfrmProgress.Close();
                MessageBox.Show("Complete. The results are stored in the shape file");

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        
        
    }
}
