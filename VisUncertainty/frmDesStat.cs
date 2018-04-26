using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

using RDotNet;
using RDotNet.NativeLibrary;

namespace VisUncertainty
{
    public partial class frmDesStat : Form
    {
        private MainForm mForm; 
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public frmDesStat()
        {
            try
            {
                InitializeComponent();
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboFieldName.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnHistogram_Click(object sender, EventArgs e)
        {

            try
            {
                string strLayerName = cboTargetLayer.Text;
                string strFieldName = cboFieldName.Text;
                pSnippet.drawHistogram(strLayerName, strFieldName);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            try
            {
                REngine pEngine = mForm.pEngine;
                int intDeciPlaces = 3;

                string strLayerName = cboTargetLayer.Text;
                string strFieldName = cboFieldName.Text;
                bool blnChkSelected = chkUseSelected.Checked;

                if (strLayerName == "" || strFieldName == "")
                {
                    MessageBox.Show("Please select a layer and field");
                    return;
                }
                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                if (intLIndex == -1)
                {
                    MessageBox.Show("Please select a proper layer");
                    return;
                }

                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

                pSnippet.CalculateDesStat(pFLayer, strFieldName, intDeciPlaces);

                //The funtion below is moved to Snippet! 012616 HK
                //IFeatureCursor pFCursor = null;
                //IFeatureSelection pFeatureSelection = pFLayer as IFeatureSelection;
                //intNFeatureCount = pFeatureSelection.SelectionSet.Count;
                //if (intNFeatureCount > 0 && blnChkSelected == true)
                //{
                //    ICursor pCursor = null;

                //    pFeatureSelection.SelectionSet.Search(null, true, out pCursor);
                //    pFCursor = (IFeatureCursor)pCursor;

                //}
                //else if (intNFeatureCount == 0 && blnChkSelected == true)
                //{
                //    MessageBox.Show("Select at least one feature");
                //    return;
                //}
                //else
                //{
                //    pFCursor = pFLayer.Search(null, true);
                //    intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);
                //}

                //IFeature pFeature = pFCursor.NextFeature();

                //int intFieldIdx = pFLayer.FeatureClass.Fields.FindField(strFieldName);

                //double[] arrValue = new double[intNFeatureCount];

                //int i = 0;
                //while (pFeature != null)
                //{

                //    arrValue[i] = Convert.ToDouble(pFeature.get_Value(intFieldIdx));
                //    i++;
                //    pFeature = pFCursor.NextFeature();
                //}
                
                ////IDataStatistics pDataStat = new DataStatisticsClass();
                ////pDataStat.Field = strFieldName;
                ////pDataStat.Cursor = (ICursor)pFCursor;
                ////IStatisticsResults pStatResults = pDataStat.Statistics;

                //string strDecimalPlaces = "N" + intDeciPlaces.ToString();
                //string[] strResults = new string[8];
                //strResults[0] = "Count: " + intNFeatureCount.ToString();
                //strResults[1] = "Minimum: " + Math.Round(arrValue.Min(), intDeciPlaces).ToString();
                //strResults[2] = "Maximum: " + Math.Round(arrValue.Max(), intDeciPlaces).ToString();
                //strResults[3] = "Sum: " + Math.Round(arrValue.Sum(), intDeciPlaces).ToString();
                //strResults[4] = "Mean: " + Math.Round(arrValue.Average(), intDeciPlaces).ToString();
                //strResults[5] = "Standard deviation: " + Math.Round(CalSDfromArray(arrValue), intDeciPlaces).ToString();

                //double[] medianIQR = new double[2];
                //medianIQR = getMedian_IQR(arrValue);
                //strResults[6] = "Median: " + Math.Round(medianIQR[0], intDeciPlaces).ToString();
                //strResults[7] = "IQR: " + Math.Round(medianIQR[1], intDeciPlaces).ToString();

                //frmGenResult pfrmResult = new frmGenResult();
                //pfrmResult.Text = "Descriptive statistics of " + strLayerName;
                //pfrmResult.txtField.Text = strFieldName;
                //pfrmResult.txtStatistics.Lines = strResults;
                //pfrmResult.Show();

                    //Using R.net: is not used from 012316 HK
                //double[] arrValue = new double[intNFeatureCount];

                //int i = 0;
                //while (pFeature != null)
                //{

                //    arrValue[i] = Convert.ToDouble(pFeature.get_Value(intFieldIdx));
                //    i++;
                //    pFeature = pFCursor.NextFeature();
                //}

                //NumericVector vecValue = pEngine.CreateNumericVector(arrValue);
                //pEngine.SetSymbol(strFieldName, vecValue);

                //double dblMean = pEngine.Evaluate("mean(" + strFieldName + ")").AsNumeric().First();
                //NumericVector nvMedian = pEngine.Evaluate("median(" + strFieldName + ")").AsNumeric();
                //NumericVector nvSd = pEngine.Evaluate("sd(" + strFieldName + ")").AsNumeric();
                ////NumericVector nvSkew = pEngine.Evaluate("skewness(" + strFieldName + ")").AsNumeric();
                ////NumericVector nvKur = pEngine.Evaluate("kurtosis(" + strFieldName + ")").AsNumeric();
                //NumericVector nvIQR = pEngine.Evaluate("IQR(" + strFieldName + ")").AsNumeric();

                //MessageBox.Show("Mean: " + Math.Round(dblMean, 2).ToString() + "\n"
                //    + "Median: " + Math.Round(nvMedian[0], 2).ToString() + "\n"
                //    + "SD: " + Math.Round(nvSd[0], 2).ToString() + "\n"
                //    //+ "skewness:" + Math.Round(nvSkew[0], 2).ToString() + "\n"
                //    //+ "kurtosis:" + Math.Round(nvKur[0], 2).ToString() + "\n"
                //    + "IQR: " + Math.Round(nvIQR[0], 2).ToString() + "\n");

                //                string strDecimalPlaces = "N" + intDeciPlaces.ToString();
                //string[] strResults = new string[6];
                //strResults[0] = "Count: " + intNFeatureCount.ToString();
                //strResults[1] = "Minimum: " + arrValue.Min().ToString();
                //strResults[2] = "Maximum: " + arrValue.Max().ToString();
                //strResults[1] = "Sum: " + arrValue.Sum().ToString();
                //strResults[1] = "Mean: " + arrValue.Average().ToString();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

    }
}
