using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using RDotNet;
using ESRI.ArcGIS.esriSystem;
//spdep, and maptools packages in R are required

namespace VisUncertainty
{
    public partial class frmBivariateGlobal: Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        public frmBivariateGlobal()
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
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
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

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                IFields fields = m_pFClass.Fields;

                cboFldnm1.Text = "";
                cboFldnm2.Text = "";

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
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMeasure.Text == "Pearson's r")
                chkDiagZero.Enabled = false;
            else
                chkDiagZero.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
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

            int nFeature = m_pFClass.FeatureCount(null);

            IFeatureCursor pFCursor = m_pFClass.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get index for independent and dependent variables
            //Get variable index            
            string strVarNM1 = (string)cboFldnm1.SelectedItem;
            string strVarNM2 = (string)cboFldnm2.SelectedItem;
            int intVarIdx1 = m_pFClass.FindField(strVarNM1);
            int intVarIdx2 = m_pFClass.FindField(strVarNM2);

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

            pFCursor.Flush();

            //Plot command for R
            StringBuilder plotCommmand = new StringBuilder();

            string strStartPath = m_pForm.strPath;
            string pathr = strStartPath.Replace(@"\", @"/");
            pEngine.Evaluate("source('" + pathr + "/ESDA_LEE/AllFunctions_LARRY.R')");
            pEngine.Evaluate("source('" + pathr + "/ESDA_LEE/AllFunctions_neighbor.R')");
            pEngine.Evaluate("source('" + pathr + "/ESDA_LEE/AllFunctions_SASbi.R')");

            //Get the file path and name to create spatial weight matrix
            string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

            if (strNameR == null)
                return;

            //Create spatial weight matrix in R
            pEngine.Evaluate("library(spdep); library(maptools)");
            pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
            //pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=FALSE)");
            pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)");

            NumericVector vecVar1 = pEngine.CreateNumericVector(arrVar1);
            pEngine.SetSymbol("sample.v1", vecVar1);
            NumericVector vecVar2 = pEngine.CreateNumericVector(arrVar2);
            pEngine.SetSymbol("sample.v2", vecVar2);

            string strNonZero = null;
            if (chkDiagZero.Checked)
                strNonZero = "FALSE";
            else
                strNonZero = "TRUE";

            if (cboMeasure.Text == "Lee's L")
            {
                pEngine.Evaluate("sample.result <- test.global.lee.L(sample.v1, sample.v2, sample.nb, style = 'W', alternative='two.sided', diag.zero = " + strNonZero + ")");
            }
            else if (cboMeasure.Text == "Pearson's r")
            {
                pEngine.Evaluate("sample.result <- test.global.pearson(sample.v1, sample.v2, alternative='two.sided')");
            }
            else if (cboMeasure.Text == "Bivariate Moran")
            {
                pEngine.Evaluate("sample.result <- test.global.bMoran(sample.v1, sample.v2, sample.nb, style = 'W', alternative='two.sided', diag.zero = " + strNonZero + ")");
            }
            else if (cboMeasure.Text == "Bivariate Geary")
            {
                pEngine.Evaluate("sample.result <- test.global.bGeary(sample.v1, sample.v2, sample.nb, style = 'W', alternative='two.sided', diag.zero = " + strNonZero + ")");
            }
            int intDeciPlaces = 4;
            //Print Output
            string strDecimalPlaces = "N" + intDeciPlaces.ToString();
            string[] strResults = new string[8];
            strResults[0] = pEngine.Evaluate("sample.result$method").AsCharacter().First();
            strResults[1] = "";
            if (cboMeasure.Text != "Pearson's r")
            {
                if (chkDiagZero.Checked)
                    strResults[2] = "Weights: W*";
                else
                    strResults[2] = "Weights: W";
            }
            else
                strResults[2] = string.Empty;

            NumericVector vecResults = pEngine.Evaluate("sample.result$estimate").AsNumeric();
            strResults[3] = "Observed: " + vecResults[0].ToString(strDecimalPlaces);
            strResults[4] = "Expectation: " + vecResults[1].ToString(strDecimalPlaces);
            strResults[5] = "Variance: " + vecResults[2].ToString(strDecimalPlaces);
            double dblStd = pEngine.Evaluate("sample.result$statistic").AsNumeric().First();
            double dblPval = pEngine.Evaluate("sample.result$p.value").AsNumeric().First();
            strResults[6] = "Standard deviate: " + dblStd.ToString(strDecimalPlaces);
            strResults[7] = "p-value: " + dblPval.ToString(strDecimalPlaces);

            frmGenResult pfrmResult = new frmGenResult();
            pfrmResult.Text = "Summary";
            pfrmResult.txtField.Text = cboFldnm1.Text + " & " + cboFldnm2.Text;
            pfrmResult.txtStatistics.Lines = strResults;
            pfrmProgress.Close();
            pfrmResult.Show();
        }
    }
}
