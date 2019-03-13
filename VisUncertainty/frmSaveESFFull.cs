using ESRI.ArcGIS.Geodatabase;
using RDotNet;
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
    public partial class frmSaveESFFull : Form
    {
        public IFeatureClass m_pFClass;
        public REngine m_pEngine;

        private NumericMatrix nmEVs;
        private CharacterVector cvEVName;
        private NumericVector nvEValue;
        private int intNEVs;
        public frmSaveESFFull()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSaveESFFull_Load(object sender, EventArgs e)
        {
            try
            {
                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Creating EVs:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();

                m_pEngine.Evaluate("sample.n <- length(sample.nb)");
                m_pEngine.Evaluate("sample.listb <- nb2listw(sample.nb, style='B')");
                m_pEngine.Evaluate("B <- listw2mat(sample.listb); M <- diag(sample.n) - matrix(1/sample.n, sample.n, sample.n); MBM <- M%*%B%*%M");
                m_pEngine.Evaluate("eig <- eigen(MBM)");

                nmEVs = m_pEngine.Evaluate("eig$vectors").AsNumericMatrix();
                cvEVName = m_pEngine.Evaluate("paste('EV', 1:sample.n, sep='')").AsCharacter();
                nvEValue = m_pEngine.Evaluate("eig$values").AsNumeric();

                intNEVs = nmEVs.RowCount;


                for (int i = 0; i < intNEVs; i++)
                {
                    string strItemName = cvEVName[i] + " (" + Math.Round(nvEValue[i], 3).ToString() + ")";
                    clistFields.Items.Add(strItemName);
                }
                //m_pEngine.Evaluate("rm(list = ls(all = TRUE))"); //Remove all items from memory.
                pfrmProgress.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < clistFields.Items.Count; i++)
                    clistFields.SetItemChecked(i, false);

                string strEValue = nudEValue.Value.ToString();
                string strDirection = cboDirection.Text;

                if (strDirection == "Positive Only")
                {
                    m_pEngine.Evaluate("np <- length(eig$values[eig$values/eig$values[1]>" + strEValue + "])");
                    int intEnd = Convert.ToInt32(m_pEngine.Evaluate("np").AsNumeric().First());

                    for (int i = 0; i < intEnd; i++)
                        clistFields.SetItemChecked(i, true);
                }
                else if (strDirection == "Negative Only")
                {
                    m_pEngine.Evaluate("n.all <- length(eig$values)");
                    m_pEngine.Evaluate("nn <- length(eig$values[eig$values/eig$values[sample.n] > " + strEValue + "])");
                    m_pEngine.Evaluate("n.start <- n.all-nn");
                    //m_pEngine.Evaluate("n.start <- n.all-nn+1");

                    int intEnd = Convert.ToInt32(m_pEngine.Evaluate("n.all").AsNumeric().First());
                    int intStart = Convert.ToInt32(m_pEngine.Evaluate("n.start").AsNumeric().First());

                    for (int i = intStart; i < intEnd; i++)
                        clistFields.SetItemChecked(i, true);
                }
                else if (strDirection == "Both")
                {
                    m_pEngine.Evaluate("np <- length(eig$values[eig$values/eig$values[1]>" + strEValue + "])");
                    m_pEngine.Evaluate("n.all <- length(eig$values)");
                    m_pEngine.Evaluate("nn <- length(eig$values[eig$values/eig$values[sample.n] > " + strEValue + "])");
                    m_pEngine.Evaluate("n.start <- n.all-nn+1");

                    int intEnd = Convert.ToInt32(m_pEngine.Evaluate("n.all").AsNumeric().First());
                    int intStart = Convert.ToInt32(m_pEngine.Evaluate("n.start").AsNumeric().First());
                    int intPStart = Convert.ToInt32(m_pEngine.Evaluate("np").AsNumeric().First());

                    for (int i = 0; i < intPStart; i++)
                        clistFields.SetItemChecked(i, true);

                    for (int i = intStart; i < intEnd; i++)
                        clistFields.SetItemChecked(i, true);

                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
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

                    string strEVfieldName = cvEVName[i];

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
                        pFeature.set_Value(intValueIdx, (object)nmEVs[featureIdx, i]);

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
