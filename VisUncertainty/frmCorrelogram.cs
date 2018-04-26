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

using RDotNet;
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Display;


namespace VisUncertainty
{
    public partial class frmCorrelogram : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        private double[][] m_arrResults;
        private double[] m_arrMaxFromLinks;
        private double[][] m_arrMaxToLinks;
        private int m_intNFeature;
        private int m_intDeciPlaces = 4;
        private double[,] m_arrXYCoord;
        private int m_intTotalNSeries = 0;

        private IntPtr m_pHandle = IntPtr.Zero;

        private IFeatureClass m_pFClass;

        //Varaibles for SWM
        private bool m_blnCreateSWM = false;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public frmCorrelogram()
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

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                m_pFClass = pFLayer.FeatureClass;

                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                    txtSWM.Text = pSWMType.strPolySWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    txtSWM.Text = pSWMType.strPointSWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    MessageBox.Show("Spatial weight matrix for polyline is not supported.");

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

        private void btnOpenSWM_Click(object sender, EventArgs e)
        {
            if (ofdOpenSWM.ShowDialog() == DialogResult.OK)
                txtSWM.Text = string.Concat(ofdOpenSWM.FileName);
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

                //Close Results form, if it is opend.
                CloseOpendResultForm(m_pHandle);
                IGraphicsContainer pGraphicContainer = m_pActiveView.GraphicsContainer;
                pGraphicContainer.DeleteAllElements();

                // Creates the input and output matrices from the shapefile//
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                //IFeatureClass pFClass = pFLayer.FeatureClass;
                m_intNFeature = m_pFClass.FeatureCount(null);

                IFeatureCursor pFCursor = m_pFClass.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Get index for independent and dependent variables
                //Get variable index            
                string strVarNM = (string)cboFieldName.SelectedItem;
                int intVarIdx = m_pFClass.FindField(strVarNM);

                //Store Variable at Array
                double[] arrVar = new double[m_intNFeature];
                m_arrXYCoord = new double[m_intNFeature, 2];

                int i = 0;
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

                    arrVar[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                    i++;
                    pFeature = pFCursor.NextFeature();
                }

                if (!m_blnCreateSWM)
                {
                    //Get the file path and name to create spatial weight matrix
                    string strNameR = m_pSnippet.FilePathinRfromLayer(pFLayer);

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
                //string strNameR = m_pSnippet.FilePathinRfromLayer(pFLayer);

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

                //Creat Higher spatial lag
                int intMaxLag = Convert.ToInt32(nudLagOrder.Value);
                try
                {
                    m_pEngine.Evaluate("sample.nblags <- nblag(sample.nb, maxlag = " + intMaxLag.ToString() + ")");
                }
                catch
                {
                    MessageBox.Show("Please reduce the maximum lag order");
                }

                m_arrResults = new double[intMaxLag][];
                m_arrMaxToLinks = new double[intMaxLag][];
                m_arrMaxFromLinks = new double[intMaxLag];
                pChart.Series.Clear();

                //Plot command for R
                StringBuilder plotCommmand = new StringBuilder();
                NumericVector vecVar = m_pEngine.CreateNumericVector(arrVar);
                m_pEngine.SetSymbol(strVarNM, vecVar);

                bool blnZeroPolicy = false;
                //Store Information of spatial lag
                for (int j = 0; j < intMaxLag; j++)
                {
                    m_arrResults[j] = new double[5];
                    m_pEngine.Evaluate("card.sample <- card(sample.nblags[[" + (j + 1).ToString() + "]])");
                    m_arrResults[j][0] = m_pEngine.Evaluate("sum(card.sample)").AsNumeric().First();

                    //If ther is no link, return
                    if (m_arrResults[j][0] == 0)
                    {
                        MessageBox.Show("There is no link at " + (j + 1).ToString() + " order.");
                        return;
                    }

                    //Functions below are used for brushing on map control, they are under reviewing 080316 HK
                    //m_arrMaxFromLinks[j] = m_pEngine.Evaluate("which.max(card.sample)").AsNumeric().First();
                    //m_arrMaxToLinks[j] = m_pEngine.Evaluate("sample.nblags[[" + (j + 1).ToString() + "]][[which.max(card.sample)]]").AsNumeric().ToArray();


                    //Select method
                    if (cboSAM.Text == "Moran Coefficient")
                    {
                        try
                        {
                            m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                        }
                        catch
                        {
                            DialogResult dialogResult = new DialogResult() ;
                            if (blnZeroPolicy == false)
                                dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                            if (dialogResult == DialogResult.Yes||blnZeroPolicy==true)
                            {
                                m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W', zero.policy=TRUE)");
                                blnZeroPolicy = true;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                pfrmProgress.Close();
                                return;
                            }
                        }

                        plotCommmand.Append("sam.result <- moran.test(" + strVarNM + ", sample.listw, ");
                        
                        //select assumption
                        if (cboAssumption.Text == "Normality")
                            plotCommmand.Append("randomisation=FALSE, alternative ='two.sided', zero.policy=TRUE)");
                        else if (cboAssumption.Text == "Randomization")
                            plotCommmand.Append("randomisation=TRUE, alternative ='two.sided', zero.policy=TRUE)");

                    }
                    else if (cboSAM.Text == "Geary Ratio")
                    {
                        try
                        {
                            m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                        }
                        catch
                        {
                            DialogResult dialogResult = new DialogResult();
                            if (blnZeroPolicy == false)
                                dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                            if (dialogResult == DialogResult.Yes || blnZeroPolicy == true)
                            {
                                m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W', zero.policy=TRUE)");
                                blnZeroPolicy = true;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                pfrmProgress.Close();
                                return;
                            }
                        }

                        plotCommmand.Append("sam.result <- geary.test(" + strVarNM + ", sample.listw, ");

                        //select assumption
                        if (cboAssumption.Text == "Normality")
                            plotCommmand.Append("randomisation=FALSE, alternative ='two.sided', zero.policy=TRUE)");
                        else if (cboAssumption.Text == "Randomization")
                            plotCommmand.Append("randomisation=TRUE, alternative ='two.sided', zero.policy=TRUE)");

                    }
                    else if (cboSAM.Text == "Global G Statistic")
                    {
                        try
                        {
                            m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                        }
                        catch
                        {
                            DialogResult dialogResult = new DialogResult();
                            if (blnZeroPolicy == false)
                                dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                            if (dialogResult == DialogResult.Yes || blnZeroPolicy == true)
                            {
                                m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='B', zero.policy=TRUE)");
                                blnZeroPolicy = true;
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                pfrmProgress.Close();
                                return;
                            }
                        }
                        plotCommmand.Append("sam.result <- globalG.test(" + strVarNM + ", sample.listw, alternative ='two.sided', zero.policy=TRUE)");                       
                    }
                    
                    m_pEngine.Evaluate(plotCommmand.ToString());
                    plotCommmand.Clear();
                    NumericVector vecResults = m_pEngine.Evaluate("sam.result$estimate").AsNumeric();
                    m_arrResults[j][1] = vecResults[0];
                    m_arrResults[j][2] = vecResults[1];
                    m_arrResults[j][3] = vecResults[2];
                    m_arrResults[j][4] = m_pEngine.Evaluate("sam.result$p.value").AsNumeric().First();

                    double dblXmin = (j + 1) - 0.2;
                    double dblXmax = (j + 1) + 0.2;
                    double dblYmax = vecResults[0] + (Math.Sqrt(vecResults[2]) * 1.96);
                    double dblYmin = vecResults[0] - (Math.Sqrt(vecResults[2]) * 1.96);


                    AddLineSeries(pChart, "min_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXmin, dblXmax, dblYmin, dblYmin);
                    AddLineSeries(pChart, "max_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXmin, dblXmax, dblYmax, dblYmax);
                    AddLineSeries(pChart, "rg_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, (j + 1), (j + 1), dblYmin, dblYmax);
                }

                //pEngine.Evaluate("sample.nb <- poly2nb(sample.shp);sample.listw <- nb2listw(sample.nb, style='W')");

                double dblExp = m_arrResults[0][2];
                AddLineSeries(pChart, "ex", Color.Red, 1, ChartDashStyle.Dash, 0.5, intMaxLag + 0.5, dblExp, dblExp);

                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Point",
                    Color = Color.Black,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,

                };
                pChart.Series.Add(pSeries);


                for (int j = 0; j < intMaxLag; j++)
                {
                    pSeries.Points.AddXY((j + 1), m_arrResults[j][1]);
                }

                m_intTotalNSeries = pChart.Series.Count;

                //Chart Design
                pChart.ChartAreas[0].AxisX.Title = "Spatial Lag";
                pChart.ChartAreas[0].AxisY.Title = cboSAM.Text;
                pChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
                pChart.ChartAreas[0].AxisX.Maximum = intMaxLag + 0.5;
                pChart.ChartAreas[0].AxisX.Minimum = 0.5;
                pChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false; //need to be updated 042816 HK

                pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;

                for (int j = 0; j < intMaxLag; j++)
                {
                    int intXvalue = j + 1;

                    double dblXmin = Convert.ToDouble(intXvalue) - 0.5;
                    double dblXmax = Convert.ToDouble(intXvalue) + 0.5;

                    CustomLabel pcutsomLabel = new CustomLabel();
                    pcutsomLabel.FromPosition = dblXmin;
                    pcutsomLabel.ToPosition = dblXmax;
                    pcutsomLabel.Text = intXvalue.ToString();

                    pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
                }
                pfrmProgress.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void AddLineSeries(Chart pChart, string strSeriesName, System.Drawing.Color FillColor, int intWidth, ChartDashStyle BorderDash, double dblXMin, double dblXMax, double dblYMin, double dblYMax)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    //BorderColor = Color.Black,
                    BorderWidth = intWidth,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    BorderDashStyle = BorderDash,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };

                pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblXMin, dblYMin);
                pSeries.Points.AddXY(dblXMax, dblYMax);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void pChart_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                _canDraw = false;
            else
            {
                _canDraw = true;
                _startX = e.X;
                _startY = e.Y;
            }
        }

        private void pChart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_canDraw) return;

                int x = Math.Min(_startX, e.X);
                int y = Math.Min(_startY, e.Y);

                int width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);

                int height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);
                _rect = new Rectangle(x, y, width, height);
                Refresh();

                Pen pen = new Pen(Color.Cyan, 1);
                pGraphics = pChart.CreateGraphics();
                pGraphics.DrawRectangle(pen, _rect);
                pGraphics.Dispose();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //Export the chart to an image file
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    conMenu.Show(pChart, e.X, e.Y);
                    return;
                }

                if (m_intTotalNSeries == 0)
                    return;
                //Clear previous selection
                while (pChart.Series.Count != m_intTotalNSeries)
                    pChart.Series.RemoveAt(pChart.Series.Count - 1);

                int intPtsSeriesIdx = m_intTotalNSeries - 1;
                _canDraw = false;

                HitTestResult result = pChart.HitTest(e.X, e.Y);

                int dblOriPtsSize = pChart.Series[intPtsSeriesIdx].MarkerSize;

                System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "SelPts",
                    Color = pMarkerColor,
                    BorderColor = pMarkerColor,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    //ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                    MarkerSize = dblOriPtsSize * 2

                };

                pChart.Series.Add(seriesLines);

                int intSelPtsCnt = 0;
                for (int i = 0; i < pChart.Series[intPtsSeriesIdx].Points.Count; i++)
                {
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[intPtsSeriesIdx].Points[i].XValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[intPtsSeriesIdx].Points[i].YValues[0]);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        if (intSelPtsCnt == 0)
                        {
                            seriesLines.Points.AddXY(pChart.Series[intPtsSeriesIdx].Points[i].XValue, pChart.Series[intPtsSeriesIdx].Points[i].YValues[0]);

                            //int index = result.PointIndex;
                            string strDecimalPlaces = "N" + m_intDeciPlaces.ToString();
                            string[] strResults = new string[8];
                            strResults[0] = cboSAM.Text + " under " + cboAssumption.Text;
                            strResults[1] = (i + 1).ToString() + " order result";
                            strResults[2] = "Number of none-zero links: " + m_arrResults[i][0].ToString("N0");
                            strResults[3] = "Average number of links: " + (m_arrResults[i][0] / m_intNFeature).ToString(strDecimalPlaces);
                            strResults[4] = "Statistic: " + m_arrResults[i][1].ToString(strDecimalPlaces);
                            strResults[5] = "Expectation: " + m_arrResults[i][2].ToString(strDecimalPlaces);
                            strResults[6] = "Variance: " + m_arrResults[i][3].ToString(strDecimalPlaces);
                            strResults[7] = "p-value: " + m_arrResults[i][4].ToString(strDecimalPlaces);

                            CloseOpendResultForm(m_pHandle);
                            frmGenResult pfrmResult = new frmGenResult();
                            pfrmResult.Text = "Summary";
                            pfrmResult.txtField.Text = cboFieldName.Text;
                            pfrmResult.txtStatistics.Lines = strResults;
                            pfrmResult.Show();

                            m_pHandle = pfrmResult.Handle;

                            //Brushing on Map Control is under reviewing. 080316 HK 
                            //DrawLineOnActiveView(m_arrMaxFromLinks[i], m_arrMaxToLinks[i], m_arrXYCoord, m_pActiveView);
                        }

                        intSelPtsCnt++;
                    }
                }
                if (intSelPtsCnt == 0)
                {
                    m_pActiveView.GraphicsContainer.DeleteAllElements();
                    m_pActiveView.Refresh();
                }
                if (intSelPtsCnt > 1)
                    MessageBox.Show("Can select only one point");
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        //The function is under reviewing.
        private void DrawLineOnActiveView(double dblFromLink,double[] arrToLinks, double[,] arrXYCoord, IActiveView pActiveView)
        {
            IGraphicsContainer pGraphicContainer = pActiveView.GraphicsContainer;
            pGraphicContainer.DeleteAllElements();

            IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);

            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            pSimpleLineSymbol.Width = 2;
            pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pSimpleLineSymbol.Color = pRgbColor;
            int intFromIdx = Convert.ToInt32(dblFromLink) - 1;
            IPoint FromP = new PointClass();
            FromP.X = arrXYCoord[intFromIdx, 0]; FromP.Y = arrXYCoord[intFromIdx, 1];

            int intArrLengthCnt = arrToLinks.Length;
            for (int i = 0; i < intArrLengthCnt; i++)
            {
                int intToIdx = Convert.ToInt32(arrToLinks[i]) - 1;
                
                IPoint ToP = new PointClass();
                ToP.X = arrXYCoord[intToIdx, 0]; ToP.Y = arrXYCoord[intToIdx, 1];

                IPolyline polyline = new PolylineClass();
                IPointCollection pointColl = polyline as IPointCollection;
                pointColl.AddPoint(FromP);
                pointColl.AddPoint(ToP);

                IElement pElement = new LineElementClass();
                ILineElement pLineElement = (ILineElement)pElement;
                pLineElement.Symbol = pSimpleLineSymbol;
                pElement.Geometry = polyline;

                pGraphicContainer.AddElement(pElement, 0);
            }

            pActiveView.Refresh();

        }
        private void CloseOpendResultForm(IntPtr pHandle)
        {
            if(pHandle == IntPtr.Zero)
                return;
            
            FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
            for (int i = 0; i < pFormCollection.Count; i++)
            {
                if (pFormCollection[i].Handle == pHandle)
                    pFormCollection[i].Close();
            }
        }

        private void frmCorrelogram_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseOpendResultForm(m_pHandle);
            IGraphicsContainer pGraphicContainer = m_pActiveView.GraphicsContainer;
            pGraphicContainer.DeleteAllElements();
            m_pActiveView.Refresh();
        }

        private void exportToImageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExportChart pfrmExportChart = new frmExportChart();
            pfrmExportChart.thisChart = pChart;
            pfrmExportChart.nudHeight.Value = pChart.Height;
            pfrmExportChart.nudHeight.Maximum = pChart.Height * 5; //Restriction of maximum size
            pfrmExportChart.nudWidth.Value = pChart.Width;
            pfrmExportChart.nudWidth.Maximum = pChart.Width * 5;
            pfrmExportChart.Show();
        }

        private void btnOpenSWM_Click_2(object sender, EventArgs e)
        {
            if (m_pFClass == null)
            {
                MessageBox.Show("Please select a target layer");
                return;
            }

            frmAdvSWM pfrmAdvSWM = new frmAdvSWM();
            pfrmAdvSWM.m_pFClass = m_pFClass;
            pfrmAdvSWM.blnCorrelogram = true;
            pfrmAdvSWM.ShowDialog();
            m_blnCreateSWM = pfrmAdvSWM.blnSWMCreation;
            txtSWM.Text = pfrmAdvSWM.txtSWM.Text;
        }

        private void cboSAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSAM.Text == "Global G Statistic")
                cboAssumption.Enabled = false;
            else
                cboAssumption.Enabled = true;
        }
        
    }
}
