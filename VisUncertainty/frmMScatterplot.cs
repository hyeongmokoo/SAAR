using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

using RDotNet;
using System.Collections.Generic;

//spdep, and maptools packages in R are required


namespace VisUncertainty
{
    public partial class frmMScatterplot : Form
    {
        
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private double[,] m_arrXYCoord; //For Brushing and Linking
        
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;
        private REngine m_pEngine;

        //Varaibles for SWM
        private bool m_blnCreateSWM = false;

        public frmMScatterplot()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);

                    //IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    //if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    //    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
                m_pEngine = m_pForm.pEngine;
                try
                {
                    m_pEngine.Evaluate("library(spdep); library(maptools)");
                }
                catch
                {
                    MessageBox.Show("Please checked R packages installed in your local computer.");
                }

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

                int intFeatureCnt = m_pFClass.FeatureCount(null);
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

                cboFldNm1.Items.Clear();
                cboFldNm1.Text = "";
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFldNm1.Items.Add(fields.get_Field(i).Name);
                        cboFldNm2.Items.Add(fields.get_Field(i).Name);
                        //cboLabel.Items.Add(fields.get_Field(i).Name);
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
            try
            {

                    frmProgress pfrmProgress = new frmProgress();
                    pfrmProgress.lblStatus.Text = "Processing:";
                    pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                    pfrmProgress.Show();


                if (cboFldNm1.Text == "")
                    {
                        MessageBox.Show("Please select a proper field");
                        return;
                    }

                    // Creates the input and output matrices from the shapefile//
                    //string strLayerName = cboTargetLayer.Text;

                    //int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                    //ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                    //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    //pFClass = pFLayer.FeatureClass;
                    int nFeature = m_pFClass.FeatureCount(null);

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

                ////Get the file path and name to create spatial weight matrix
                //string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

                //if (strNameR == null)
                //    return;

                //int intSuccess = 0;


                ////Create spatial weight matrix in R
                //if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                //{
                //    m_pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
                //    intSuccess = m_pSnippet.CreateSpatialWeightMatrix1(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, Convert.ToDouble(nudAdvanced.Value), chkCumulate.Checked);

                //}
                //else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                //{
                //    m_pEngine.Evaluate("sample.shp <- readShapePoints('" + strNameR + "')");
                //    //intSuccess = m_pSnippet.ExploreSpatialWeightMatrix1(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, Convert.ToDouble(nudAdvanced.Value), chkCumulate.Checked);
                //    intSuccess = m_pSnippet.CreateSpatialWeightMatrixPts(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, Convert.ToDouble(nudAdvanced.Value), chkCumulate.Checked, m_pClippedPolygon);

                //    //chkCumulate.Visible = false;
                //}
                //else
                //{
                //    MessageBox.Show("This geometry type is not supported");
                //    pfrmProgress.Close();
                //    this.Close();
                //}

                //if (intSuccess == 0)
                //    return;


                IFeatureCursor pFCursor = m_pFClass.Search(null, true);
                    IFeature pFeature = pFCursor.NextFeature();

                    //Get index for independent and dependent variables
                    //Get variable index            
                    string strVarNM = (string)cboFldNm1.SelectedItem;
                    int intVarIdx = m_pFClass.FindField(strVarNM);
                    int intFIDIdx = m_pFClass.FindField(m_pFClass.OIDFieldName); // Collect FIDs to apply Brushing and Linking

                    //Store Variable at Array
                    double[] arrVar = new double[nFeature];
                    int[] arrFID = new int[nFeature];

                    int i = 0;

                m_arrXYCoord = new double[nFeature, 2];
                List<int>[] NBIDs = new List<int>[nFeature];

                IArea pArea;
                IPoint pPoint;

                while (pFeature != null)
                {
                    if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pArea = (IArea)pFeature.Shape;
                        m_arrXYCoord[i, 0] = pArea.Centroid.X;
                        m_arrXYCoord[i, 1] = pArea.Centroid.Y;
                    }
                    else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pPoint = (IPoint)pFeature.Shape;
                        m_arrXYCoord[i, 0] = pPoint.X;
                        m_arrXYCoord[i, 1] = pPoint.Y;
                    }
                    NBIDs[i] = m_pEngine.Evaluate("sample.nb[[" + (i + 1).ToString() + "]]").AsInteger().ToList();

                    arrVar[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                    arrFID[i] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
                    i++;
                    pFeature = pFCursor.NextFeature();
                }
                    pFCursor.Flush();


                    //Collect NB for Brushing and linking

                NumericVector vecVar = m_pEngine.CreateNumericVector(arrVar);
                    m_pEngine.SetSymbol(strVarNM, vecVar);

                    if (chkStd.Checked)
                    {
                        m_pEngine.Evaluate(strVarNM + " <- scale(" + strVarNM + ")"); //Scaled
                        vecVar = m_pEngine.Evaluate(strVarNM).AsNumeric();
                    }

                    NumericVector vecWeightVar = null;
                    if (cboMethod.Text == "MC")
                        vecWeightVar = m_pEngine.Evaluate("wx.sample <- lag.listw(sample.listw, " + strVarNM + ", zero.policy=TRUE)").AsNumeric();
                    else if (cboMethod.Text == "GR")
                    {
                        string strStartPath = m_pForm.strPath;
                        string pathr = strStartPath.Replace(@"\", @"/");
                        m_pEngine.Evaluate("source('" + pathr + "/AllFunctions.R')");

                        vecWeightVar = m_pEngine.Evaluate("wx.sample <- diff.lag.listw(sample.listw, " + strVarNM + ")").AsNumeric();
                    }
                    else if (cboMethod.Text == "L")
                    {
                        string strStartPath = m_pForm.strPath;
                        string pathr = strStartPath.Replace(@"\", @"/");
                        m_pEngine.Evaluate("source('" + pathr + "/AllFunctions.R')");

                        vecWeightVar = m_pEngine.Evaluate("wx.sample <- diff.lag.listw(sample.listw, " + strVarNM + ")").AsNumeric();
                    }

                    m_pEngine.SetSymbol("WVar.sample", vecWeightVar);
                    //double[] arrWeightVar = vecWeightVar.ToArray();
                    NumericVector vecCoeff = m_pEngine.Evaluate("lm(WVar.sample~" + strVarNM + ")$coefficients").AsNumeric();

                    frmMScatterResults pfrmMScatterResult = new frmMScatterResults();
                pfrmMScatterResult.m_arrXYCoord = m_arrXYCoord;
                pfrmMScatterResult.m_NBIDs = NBIDs;
                    pfrmMScatterResult.pChart.ChartAreas[0].AxisX.IsStartedFromZero = false;
                    pfrmMScatterResult.pChart.ChartAreas[0].AxisX.IsMarginVisible = true;

                    pfrmMScatterResult.pChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

                    pfrmMScatterResult.Text = "Moran Scatter Plot of " + m_pFLayer.Name;
                    pfrmMScatterResult.pChart.Series.Clear();
                    System.Drawing.Color pMarkerColor = System.Drawing.Color.Blue;
                    var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Points",
                        Color = pMarkerColor,
                        BorderColor = pMarkerColor,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                        MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle

                    };

                    pfrmMScatterResult.pChart.Series.Add(seriesPts);

                    for (int j = 0; j < vecVar.Length; j++)
                        seriesPts.Points.AddXY(vecVar[j], vecWeightVar[j]);



                    var VLine = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "VLine",
                        Color = System.Drawing.Color.Black,
                        BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                        //BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                    };
                    pfrmMScatterResult.pChart.Series.Add(VLine);

                    VLine.Points.AddXY(vecVar.Average(), vecWeightVar.Min());
                    VLine.Points.AddXY(vecVar.Average(), vecWeightVar.Max());

                    var HLine = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "HLine",
                        Color = System.Drawing.Color.Black,
                        BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                        //BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                    };
                    pfrmMScatterResult.pChart.Series.Add(HLine);

                    HLine.Points.AddXY(vecVar.Min(), vecWeightVar.Average());
                    HLine.Points.AddXY(vecVar.Max(), vecWeightVar.Average());

                    var seriesLine = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "RegLine",
                        Color = System.Drawing.Color.Red,
                        //BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                    };

                    pfrmMScatterResult.pChart.Series.Add(seriesLine);

                    seriesLine.Points.AddXY(vecVar.Min(), vecVar.Min() * vecCoeff[1] + vecCoeff[0]);
                    seriesLine.Points.AddXY(vecVar.Max(), vecVar.Max() * vecCoeff[1] + vecCoeff[0]);

                    if (chkStd.Checked)
                    {
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisX.Title = "standardized " + strVarNM;
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisY.Title = "Spatially lagged standardized " + strVarNM;
                        pfrmMScatterResult.lblRegression.Text = "Spatially lagged standardized " + strVarNM + " = " + vecCoeff[1].ToString("N3") + " * " + "standardized " + strVarNM;
                    }
                    else
                    {
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisX.Title = strVarNM;
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisY.Title = "Spatially lagged " + strVarNM;
                        pfrmMScatterResult.lblRegression.Text = "Spatially lagged " + strVarNM + " = " + vecCoeff[1].ToString("N3") + " * " + strVarNM + " + " + vecCoeff[0].ToString("N3");
                    }

                    pfrmMScatterResult.m_pForm = m_pForm;
                    pfrmMScatterResult.m_pFLayer = m_pFLayer;
                    pfrmMScatterResult.m_pActiveView = m_pActiveView;
                    pfrmMScatterResult.arrVar = arrVar;
                    pfrmMScatterResult.arrFID = arrFID;
                    pfrmMScatterResult.strFIDNM = m_pFClass.OIDFieldName;
                    //pfrmMScatterResult.arrWeightVar = arrWeightVar;
                    pfrmMScatterResult.pMakerColor = pMarkerColor;
                    pfrmMScatterResult.strVarNM = strVarNM;

                    if (chkStd.Checked)
                    {
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisX.CustomLabels.Clear();
                        pfrmMScatterResult.pChart.ChartAreas[0].AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.None;
                        //pfrmMScatterResult.pChart.ChartAreas[0].AxisX.MajorTickMark.Interval = 1;
                        //pfrmMScatterResult.pChart.ChartAreas[0].AxisX.MajorTickMark.IntervalOffset = -2;

                        int intMin = Convert.ToInt32(Math.Floor(vecVar.Min()));
                        int intMax = Convert.ToInt32(Math.Ceiling(vecVar.Max()));
                        for (int n = intMin; n < intMax; n++)
                        {
                            System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
                            pcutsomLabel.FromPosition = n - 0.5;
                            pcutsomLabel.ToPosition = n + 0.5;
                            pcutsomLabel.Text = n.ToString();
                            pfrmMScatterResult.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);

                        }
                    }

                    pfrmMScatterResult.Show();
                    pfrmProgress.Close();
                    //this.Close();

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private class Neighbors
        {
            public List<int>[] IDs { get; set; }
        }

        private void btnOpenSWM_Click_1(object sender, EventArgs e)
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
