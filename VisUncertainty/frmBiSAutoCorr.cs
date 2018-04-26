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
using ESRI.ArcGIS.Display;

using RDotNet;
using ESRI.ArcGIS.esriSystem;

namespace VisUncertainty
{
    public partial class frmBiSAutoCorr : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        public frmBiSAutoCorr()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Only polygon to make spatial weight matrix 10/9/15 HK
                        cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboFldnm1.Items.Clear();
                cboFldnm2.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFldnm1.Items.Add(fields.get_Field(i).Name);
                        cboFldnm2.Items.Add(fields.get_Field(i).Name);
                    }
                }
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

        private void btnRun_Click(object sender, EventArgs e)
        {
            int intDeciPlaces = 5;

            if (cboFldnm1.Text == "" || cboFldnm2.Text == "")
            {
                MessageBox.Show("Please select target field");
                return;
            }

            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Processing:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            REngine pEngine = m_pForm.pEngine;
            // Creates the input and output matrices from the shapefile//
            string strLayerName = cboTargetLayer.Text;

            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFClass = pFLayer.FeatureClass;
            int nFeature = pFClass.FeatureCount(null);

            IFeatureCursor pFCursor = pFLayer.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get index for independent and dependent variables
            //Get variable index            
            string strVarNM1 = (string)cboFldnm1.SelectedItem;
            string strVarNM2 = (string)cboFldnm2.SelectedItem;
            int intVarIdx1 = pFClass.FindField(strVarNM1);
            int intVarIdx2 = pFClass.FindField(strVarNM2);

            //Store Variable at Array
            double[] arrVar1 = new double[nFeature];
            double[] arrVar2 = new double[nFeature];

            int i = 0;

            while (pFeature != null)
            {
                arrVar1[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx1));
                arrVar2[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx2));
                i++;
                pFeature = pFCursor.NextFeature();
            }

            //Plot command for R
            StringBuilder plotCommmand = new StringBuilder();

            string strStartPath = m_pForm.strPath;
            string pathr = strStartPath.Replace(@"\", @"/");
            pEngine.Evaluate("source('" + pathr + "/AllFunctions_LeeL.R')");

            //Get the file path and name to create spatial weight matrix
            string strNameR = m_pSnippet.FilePathinRfromLayer(pFLayer);

            if (strNameR == null)
                return;

            //Create spatial weight matrix in R
            pEngine.Evaluate("library(spdep); library(maptools)");
            pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
            pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)");

            NumericVector vecVar1 = pEngine.CreateNumericVector(arrVar1);
            pEngine.SetSymbol("sample.v1", vecVar1);
            NumericVector vecVar2 = pEngine.CreateNumericVector(arrVar2);
            pEngine.SetSymbol("sample.v2", vecVar2);

            string strNonZeroDiag = null;
            if(chkNonZeroDiag.Checked)
                strNonZeroDiag = "FALSE";
            else
                strNonZeroDiag = "TRUE";

            if (cboSAM.Text == "Lee's L")
            {
                pEngine.Evaluate("sample.g <- L.global.test(sample.v1, sample.v2, sample.nb, style='W', alternative='two.sided', diag.zero=" + strNonZeroDiag + ")");


                //Print Output
                string strDecimalPlaces = "N" + intDeciPlaces.ToString();
                string[] strResults = new string[7];
                if(chkNonZeroDiag.Checked)
                    strResults[0] = cboSAM.Text + "* under " + cboAssumption.Text;
                else
                    strResults[0] = cboSAM.Text + "0 under " + cboAssumption.Text;

                strResults[1] = "";
                NumericVector vecResults = pEngine.Evaluate("sample.g$estimate").AsNumeric();
                strResults[2] = "Statistic: " + vecResults[0].ToString(strDecimalPlaces);
                strResults[3] = "Expectation: " + vecResults[1].ToString(strDecimalPlaces);
                strResults[4] = "Variance: " + vecResults[2].ToString(strDecimalPlaces);
                double dblStd = pEngine.Evaluate("sample.g$statistic").AsNumeric().First();
                double dblPval = pEngine.Evaluate("sample.g$p.value").AsNumeric().First();
                strResults[5] = "Standard deviate: " + dblStd.ToString(strDecimalPlaces);
                strResults[6] = "p-value: " + dblPval.ToString(strDecimalPlaces);

                frmGenResult pfrmResult = new frmGenResult();
                pfrmResult.Text = "Summary";
                pfrmResult.txtField.Text = strVarNM1 + " & " + strVarNM2;
                pfrmResult.txtStatistics.Lines = strResults;

                pfrmResult.Show();
            }
                pfrmProgress.Close();
        }
    }
}
