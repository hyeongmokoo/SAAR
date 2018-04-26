using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

using RDotNet;

//using Accord.Math;
//spdep, and maptools packages in R are required

namespace VisUncertainty
{
    public partial class frmSAutoCorr : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        //Varaibles for SWM
        private bool m_blnCreateSWM = false;

        public frmSAutoCorr()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    //IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);
                    //if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    //Deprecated: Updated to accept point and polygon dataset. 

                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
                m_pEngine = m_pForm.pEngine;
                m_pEngine.Evaluate("rm(list=ls(all=TRUE))");
                m_pEngine.Evaluate("library(spdep); library(maptools)");
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

                 m_pFLayer = pLayer as IFeatureLayer;
                 m_pFClass = m_pFLayer.FeatureClass;

                //New Spatial Weight matrix function 080317
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                    txtSWM.Text = pSWMType.strPolySWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    txtSWM.Text = pSWMType.strPointSWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    MessageBox.Show("Spatial weight matrix for polyline is not supported.");
                //

                IFields fields = m_pFClass.Fields;

                cboFieldName.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
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
            try
            {

                if (cboFieldName.Text == "")
                {
                    MessageBox.Show("Please select target field");
                    return;
                }
                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Processing:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();

                //Decimal places
                int intDeciPlaces = 5;

                // Creates the input and output matrices from the shapefile//
                //string strLayerName = cboTargetLayer.Text;

                //int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                //ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                 //pFLayer = pLayer as IFeatureLayer;
                 //pFClass = pFLayer.FeatureClass;
                int nFeature = m_pFClass.FeatureCount(null);

                IFeatureCursor pFCursor = m_pFLayer.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Get index for independent and dependent variables
                //Get variable index            
                string strVarNM = cboFieldName.Text;
                int intVarIdx = m_pFClass.FindField(strVarNM);

                //Store Variable at Array
                double[] arrVar = new double[nFeature];

                int i = 0;

                while (pFeature != null)
                {
                    arrVar[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                    i++;
                    pFeature = pFCursor.NextFeature();
                }

                //Plot command for R
                StringBuilder plotCommmand = new StringBuilder();

                if (!m_blnCreateSWM)
                {
                    //Get the file path and name to create spatial weight matrix
                    string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

                    if (strNameR == null)
                        return;

                    //Create spatial weight matrix in R
                    if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        m_pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
                    else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        m_pEngine.Evaluate("sample.shp <- readShapePoints('" + strNameR + "')");
                    else
                    {
                        MessageBox.Show("This geometry type is not supported");
                        pfrmProgress.Close();
                        this.Close();
                    }


                    int intSuccess = m_pSnippet.CreateSpatialWeightMatrix(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress);
                    if (intSuccess == 0)
                        return;
                }

                //pEngine.Evaluate("sample.nb <- poly2nb(sample.shp);sample.listw <- nb2listw(sample.nb, style='W')");
                NumericVector vecVar = m_pEngine.CreateNumericVector(arrVar);
                m_pEngine.SetSymbol(strVarNM, vecVar);
                plotCommmand.Append("sam.result <- ");

                //Select method
                if (cboSAM.Text == "Moran Coefficient")
                    plotCommmand.Append("moran.test");
                else if (cboSAM.Text == "Geary Ratio")
                    plotCommmand.Append("geary.test");

                plotCommmand.Append("(" + strVarNM + ", sample.listw, ");

                //select assumption
                if (cboAssumption.Text == "Normality")
                    plotCommmand.Append("randomisation=FALSE, , zero.policy=TRUE)");
                else if (cboAssumption.Text == "Randomization")
                    plotCommmand.Append("randomisation=TRUE, , zero.policy=TRUE)");

                m_pEngine.Evaluate(plotCommmand.ToString());
                //Print Output
                string strDecimalPlaces = "N" + intDeciPlaces.ToString();
                string[] strResults = new string[7];
                strResults[0] = cboSAM.Text + " under " + cboAssumption.Text;
                strResults[1] = "";
                NumericVector vecResults = m_pEngine.Evaluate("sam.result$estimate").AsNumeric();
                strResults[2] = "Statistic: " + vecResults[0].ToString(strDecimalPlaces);
                strResults[3] = "Expectation: " + vecResults[1].ToString(strDecimalPlaces);
                strResults[4] = "Variance: " + vecResults[2].ToString(strDecimalPlaces);
                double dblStd = m_pEngine.Evaluate("sam.result$statistic").AsNumeric().First();
                double dblPval = m_pEngine.Evaluate("sam.result$p.value").AsNumeric().First();
                strResults[5] = "Standard deviate: " + dblStd.ToString(strDecimalPlaces);
                if(dblPval < Math.Pow(0.1, intDeciPlaces))
                    strResults[6] = "p-value: < 0.001";
                else
                    strResults[6] = "p-value: " + dblPval.ToString(strDecimalPlaces);

                frmGenResult pfrmResult = new frmGenResult();
                pfrmResult.Text = "Summary";
                pfrmResult.txtField.Text = strVarNM;
                pfrmResult.txtStatistics.Lines = strResults;
                pfrmProgress.Close();
                pfrmResult.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                
                return;
            }
        }

        private void btnOpenSWM_Click(object sender, EventArgs e)
        {
            if (m_pFClass == null)
            {
                MessageBox.Show("Please select a target layer");
                return;
            }
            frmAdvSWM pfrmAdvSWM = new frmAdvSWM();
            pfrmAdvSWM.m_pFClass = m_pFClass;
            pfrmAdvSWM.blnCorrelogram = false;
            pfrmAdvSWM.ShowDialog();
            m_blnCreateSWM = pfrmAdvSWM.blnSWMCreation;
            txtSWM.Text = pfrmAdvSWM.txtSWM.Text;
        }
    }
}
