using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;


namespace VisUncertainty
{
    public partial class frmSlider : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private double dblMinValue;
        private double dblMaxValue;
        private ILayer pLayer;
        private IFeatureLayer pFLayer;
        private IFeatureClass pFClass;
        private string strUncernRenderField;
        private int intNDecimal = 3;

        public frmSlider()
        {
            InitializeComponent();
            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            pActiveView = mForm.axMapControl1.ActiveView;

            for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
            {
                cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
            }
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsSnippet pSnippet = new clsSnippet();
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                pFLayer = pLayer as IFeatureLayer;
                pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                CboUField.Items.Clear();
                CboUField.Text = "";

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    CboUField.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void CboUField_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                strUncernRenderField = CboUField.Text;

                ITable pTable = (ITable)pFClass;
                int intUncernIdx = pTable.FindField(strUncernRenderField);
                IField pUncernField = pTable.Fields.get_Field(intUncernIdx);
                ICursor pCursor = pTable.Search(null, false);
                IDataStatistics pDataStat = new DataStatisticsClass();
                pDataStat.Field = pUncernField.Name;
                pDataStat.Cursor = pCursor;
                IStatisticsResults pStatResults = pDataStat.Statistics;

                dblMinValue = pStatResults.Minimum;
                dblMaxValue = pStatResults.Maximum;

                int intX = lblMax.Location.X + lblMax.Size.Width;
                lblMin.Text = "MIN: " + Math.Round(pStatResults.Minimum, intNDecimal).ToString();
                lblMax.Text = "MAX: " + Math.Round(pStatResults.Maximum, intNDecimal).ToString();
                lblMax.SetBounds(intX - lblMax.Size.Width, lblMax.Location.Y, lblMax.Size.Width, lblMax.Size.Height);
                txtValue.Text = Math.Round(dblMinValue, intNDecimal).ToString();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void DrawOnlySelectedFeatures(double dblValue)
        {
            try
            {
                IFeatureLayerDefinition2 pFDefinition = (IFeatureLayerDefinition2)pFLayer;
                pFDefinition.DefinitionExpression = strUncernRenderField + ">=" + dblValue.ToString();
                pActiveView.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void frmGlider_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (pFLayer != null)
                {
                    ILayerEffects pLayerEffect = (ILayerEffects)pFLayer;
                    pLayerEffect.Transparency = 0;
                    DrawOnlySelectedFeatures(dblMinValue - 1);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }


        private void trbUncern_Scroll(object sender, EventArgs e)
        {
            try
            {
                double dblInterval = (dblMaxValue - dblMinValue) / Convert.ToDouble(trbUncern.Maximum);
                double dblValue = dblMinValue + dblInterval * Convert.ToDouble(trbUncern.Value);
                txtValue.Text = Math.Round(dblValue, intNDecimal).ToString();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double dblValue = 0;
                if (txtValue.Text != Math.Round(dblMinValue, intNDecimal).ToString())
                {
                    try
                    {
                        dblValue = Convert.ToDouble(txtValue.Text);
                    }
                    catch
                    {
                        return;
                    }
                    if (dblValue < (dblMinValue-1) || dblValue > (dblMaxValue+1)) // For Rounding Errors
                    {
                        MessageBox.Show("please input number between MIN and MAX");
                        return;
                    }
                }
                else
                    dblValue = dblMinValue;
                double dblInterval = (dblMaxValue - dblMinValue) / Convert.ToDouble(trbUncern.Maximum);
                trbUncern.Value = Convert.ToInt32((dblValue - dblMinValue) / dblInterval);
                trbUncern.Refresh();

                DrawOnlySelectedFeatures(dblValue);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }


        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trbTransparent_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (pFLayer == null) return;
                ILayerEffects pLayerEffect = (ILayerEffects)pFLayer;
                pLayerEffect.Transparency = Convert.ToInt16(trbTransparent.Value);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
