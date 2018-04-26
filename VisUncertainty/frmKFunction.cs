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
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Display;
using System.Diagnostics;


namespace VisUncertainty
{
    public partial class frmKFunction : Form
    {
        private clsSnippet m_pSnippet;
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;
        private IFeatureLayer m_pFStudyLayer;
        private IFeature m_pFeature;
        
        private double m_dblArea = 0;
        private List<int>[] m_arrResultIdxs;
        private double[][] m_arrValue;
        private int m_intTotalNSeries;
        private double m_dblBeginDist;
        private double m_dblDistInc;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public frmKFunction()
        {
            try
            {
                InitializeComponent();
                m_pSnippet = new clsSnippet();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        cboStudyArea.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                    else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (chkEdgeCorrection.Checked)
                CalKhatWithEgde();
            else
                CalKhatWithoutEgde();

            this.Size = new Size(700, this.Size.Height);

        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                double dblHeight = m_pFLayer.AreaOfInterest.Height;
                double dblWidth = m_pFLayer.AreaOfInterest.Width;

                if (cboStudyArea.Text == "Minimum Enclosing Rectangle")
                    m_dblArea = dblHeight * dblWidth;

                double dblShortLine = 0;
                if (dblHeight > dblWidth)
                {
                    dblShortLine = dblWidth;
                    nudDistInc.Maximum = Convert.ToDecimal(dblHeight);
                }
                else
                {
                    dblShortLine = dblHeight;
                    nudDistInc.Maximum = Convert.ToDecimal(dblWidth);
                }

                double dblNdigits = Math.Pow(10, Math.Ceiling(Math.Log10(dblShortLine)));

                double dblValue = Math.Round(dblShortLine / 4 / dblNdigits, 1) * (dblNdigits / 10);
                nudDistInc.Value = Convert.ToDecimal(dblValue);

                m_pFClass = m_pFLayer.FeatureClass;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
            
        }

        private void frmKFunction_Load(object sender, EventArgs e)
        {
            this.Size = new Size(261, this.Size.Height);
        }
        private double[,] RndPtswithEnvelope(int intNFeatureCount, IEnvelope pEnv)
        {
            double[,] arrTable = new double[intNFeatureCount, 2];

            Random pRandom = new Random();
            double dblWidth = pEnv.Width;
            double dblHeight = pEnv.Height;
            double dblXMin = pEnv.XMin;
            double dblYmin = pEnv.YMin;

            for (int i = 0; i < intNFeatureCount; i++)
            {
                arrTable[i, 0] = (pRandom.NextDouble())*dblWidth + dblXMin;
                arrTable[i, 1] = (pRandom.NextDouble()) * dblHeight + dblYmin;
            }
            return arrTable;

        }
        private double[,] RndPtswithStudyArea(int intNFeatureCount, IEnvelope pEnv, IFeature pFeature)
        {
            double[,] arrTable = new double[intNFeatureCount, 2];

            Random pRandom = new Random();
            double dblWidth = pEnv.Width;
            double dblHeight = pEnv.Height;
            double dblXMin = pEnv.XMin;
            double dblYmin = pEnv.YMin;

            
            IRelationalOperator pRel = pFeature.Shape as IRelationalOperator2;
            
            int intGenPt = 0;
            int intTest = 0;
            while(intGenPt != intNFeatureCount)
            {
                IPoint pPnt = new PointClass();
                pPnt.X = (pRandom.NextDouble()) * dblWidth + dblXMin;
                pPnt.Y = (pRandom.NextDouble()) * dblHeight + dblYmin;
                if (pRel.Contains(pPnt))
                {
                    arrTable[intGenPt, 0] = pPnt.X;
                    arrTable[intGenPt, 1] = pPnt.Y;
                    intGenPt++;
                }
                else
                    intTest++;
            }
            return arrTable;

        }
        private void chkConfBnd_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConfBnd.Checked)
                grbModelling.Enabled = true;
            else
                grbModelling.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Mouse Actions
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

                //Clear previous selection
                while (pChart.Series.Count != m_intTotalNSeries)
                    pChart.Series.RemoveAt(pChart.Series.Count - 1);

                HitTestResult result = pChart.HitTest(e.X, e.Y);

                int dblOriPtsSize = pChart.Series[0].MarkerSize;
                _canDraw = false;

                System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "SelLines",
                    Color = pMarkerColor,
                    BorderColor = pMarkerColor,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                    MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                    MarkerSize = dblOriPtsSize * 2

                };

                pChart.Series.Add(seriesLines);

                StringBuilder plotCommmand = new StringBuilder();
                int intSelPtsCnt = 0;
                for (int i = 0; i < pChart.Series[0].Points.Count; i++)
                {
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[0].Points[i].XValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[0].Points[i].YValues[0]);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        intSelPtsCnt++;

                        int index = result.PointIndex;
                        double dblRefDist = pChart.Series[0].Points[i].XValue;
                        seriesLines.Points.AddXY(pChart.Series[0].Points[i].XValue, pChart.Series[0].Points[i].YValues[0]);
                        int intXIndex = Convert.ToInt32((dblRefDist - m_dblBeginDist) / m_dblDistInc);

                        DrawLineOnActiveView(m_arrResultIdxs[intXIndex - 1], m_arrValue, m_pActiveView);
                    }
                }
                if (intSelPtsCnt == 0)
                {
                    m_pActiveView.GraphicsContainer.DeleteAllElements();
                    m_pActiveView.Refresh();
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
       }
        private void DrawLineOnActiveView(List<int> lstIndices, double[][] arrValue, IActiveView pActiveView)
        {
            int intLstCnt = lstIndices.Count;

            IGraphicsContainer pGraphicContainer = pActiveView.GraphicsContainer;
            pGraphicContainer.DeleteAllElements();

            IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);

            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            pSimpleLineSymbol.Width = 2;
            pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pSimpleLineSymbol.Color = pRgbColor;



            for (int i = 0; i < intLstCnt; i++)
            {
                int intIdx = lstIndices[i];
                double[] arrSelValue = arrValue[intIdx];
                //drawing a polyline
                IPoint FromP = new PointClass();
                FromP.X = arrSelValue[0]; FromP.Y = arrSelValue[1];

                IPoint ToP = new PointClass();
                ToP.X = arrSelValue[2]; ToP.Y = arrSelValue[3];

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
#endregion

        private void cboStudyArea_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboStudyArea.Text == "Minimum Enclosing Rectangle")
            {
                double dblHeight = m_pFLayer.AreaOfInterest.Height;
                double dblWidth = m_pFLayer.AreaOfInterest.Width;
                m_dblArea = dblHeight * dblWidth;
                chkEdgeCorrection.Enabled = false;
            }
            else
            {
                string strAreaName = cboStudyArea.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strAreaName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFStudyLayer = pLayer as IFeatureLayer;
                if (m_pFStudyLayer.FeatureClass.FeatureCount(null) > 1)
                {
                    MessageBox.Show("The Polygon should be consisted with only one feature!");
                    return;
                }
                m_pFeature = m_pFStudyLayer.FeatureClass.GetFeature(0);
                IArea pArea = (IArea)m_pFeature.Shape;
                m_dblArea = pArea.Area;

                chkEdgeCorrection.Enabled = true;
            }
        }

        private double[][] EdgeCorrection(int intNFeatureCount, int intNDistBnd, IFeature pFeature, double[,] arrXYPts, double dblDistInc, double dblBeginDist)
        {
            double[][] dblWeightCnt = new double[intNFeatureCount][];
            ITopologicalOperator2 pTopoOpt = (ITopologicalOperator2)pFeature.ShapeCopy;

            for (int i = 0; i < intNFeatureCount; i++)
            {
                IPoint pFromPts = new PointClass();
                pFromPts.X = arrXYPts[i, 0];
                pFromPts.Y = arrXYPts[i, 1];
                dblWeightCnt[i] = new double[intNDistBnd];
                ITopologicalOperator pTopoBuffer = pFromPts as ITopologicalOperator;

                for (int l = intNDistBnd; l > 0; l--)
                {
                    if (l == intNDistBnd)
                    {
                        double dblWeight = 0;
                        double dblRefDist = l * dblDistInc + dblBeginDist;
                        IPolygon polygon = pTopoBuffer.Buffer(dblRefDist) as IPolygon;
                        IArea pPolyArea = polygon as IArea;
                        IArea pIntersectedArea = (IArea)pTopoOpt.Intersect(polygon, esriGeometryDimension.esriGeometry2Dimension);

                        dblWeight = pIntersectedArea.Area / pPolyArea.Area;
                        dblWeightCnt[i][l - 1] = dblWeight;

                    }
                    else
                    {
                        if (dblWeightCnt[i][l] != 1)
                        {
                            double dblWeight = 0;
                            double dblRefDist = l * dblDistInc + dblBeginDist;
                            IPolygon polygon = pTopoBuffer.Buffer(dblRefDist) as IPolygon;
                            IArea pPolyArea = polygon as IArea;
                            IArea pIntersectedArea = (IArea)pTopoOpt.Intersect(polygon, esriGeometryDimension.esriGeometry2Dimension);

                            dblWeight = pIntersectedArea.Area / pPolyArea.Area;
                            dblWeightCnt[i][l - 1] = dblWeight;
                        }
                        else
                            dblWeightCnt[i][l - 1] = 1;
                    }
                }
            }
            //if (dblWeightCnt[l - 1] < 0.5)
            //    MessageBox.Show("11");
            return dblWeightCnt;
        }

        private void CalKhatWithoutEgde()
        {
            m_dblBeginDist = Convert.ToDouble(nudBeginDist.Value);
            m_dblDistInc = Convert.ToDouble(nudDistInc.Value);
            int intNDistBnd = Convert.ToInt32(nudNDistBands.Value);
            m_arrResultIdxs = new List<int>[intNDistBnd];
            for (int j = 0; j < intNDistBnd; j++)
                m_arrResultIdxs[j] = new List<int>();

            int intNFeatureCount = 0;

            IFeatureCursor pFCursor1 = m_pFClass.Search(null, false);
            intNFeatureCount = m_pFClass.FeatureCount(null);

            if (intNFeatureCount == 0)
                return;

            m_arrValue = new double[intNFeatureCount * (intNFeatureCount - 1)][];

            IFeature pFeature1 = pFCursor1.NextFeature();
            double[,] arrXYPts = new double[intNFeatureCount, 2];
            IPoint pPoint1;
            int i = 0;
            while (pFeature1 != null)
            {
                pPoint1 = pFeature1.Shape as IPoint;

                arrXYPts[i, 0] = pPoint1.X;
                arrXYPts[i, 1] = pPoint1.Y;
                i++;
                pFeature1 = pFCursor1.NextFeature();
            }

            i = 0;
            for (int j = 0; j < intNFeatureCount; j++)
            {
                for (int k = 0; k < intNFeatureCount; k++)
                {
                    if (j != k)
                    {
                        m_arrValue[i] = new double[5];
                        m_arrValue[i][0] = arrXYPts[j, 0];
                        m_arrValue[i][1] = arrXYPts[j, 1];
                        m_arrValue[i][2] = arrXYPts[k, 0];
                        m_arrValue[i][3] = arrXYPts[k, 1];
                        m_arrValue[i][4] = Math.Sqrt(Math.Pow(arrXYPts[j, 0] - arrXYPts[k, 0], 2) + Math.Pow(arrXYPts[j, 1] - arrXYPts[k, 1], 2));

                        for (int l = 1; l <= intNDistBnd; l++)
                        {
                            double dblRefDist = l * m_dblDistInc + m_dblBeginDist;
                            if (m_arrValue[i][4] < dblRefDist)
                            {
                                m_arrResultIdxs[l - 1].Add(i);
                            }
                        }
                        i++;
                    }
                }
            }

            #region previous method
            /* This method is too slow
            //IFeature pFeature1 = pFCursor1.NextFeature();
            ////IFeature pFeature2 = pFCursor2.NextFeature();
            
            //IFeatureCursor pFCursor2;
            //IFeature pFeature2;
            //IPoint pPoint1;
            //IPoint pPoint2;

            //int i = 0;
            //while (pFeature1 != null)
            //{
            //    pPoint1 = pFeature1.Shape as IPoint;
            //    pFCursor2 = m_pFClass.Search(null, false);
            //    pFeature2 = pFCursor2.NextFeature();
                
            //    while (pFeature2 != null)
            //    {
            //        if (pFeature1.OID != pFeature2.OID)
            //        {
            //            m_arrValue[i] = new double[5];
            //            pPoint2 = pFeature2.Shape as IPoint;
                        
            //            m_arrValue[i][0] = pPoint1.X;
            //            m_arrValue[i][1] = pPoint1.Y;
            //            m_arrValue[i][2] = pPoint2.X;
            //            m_arrValue[i][3] = pPoint2.Y;
            //            m_arrValue[i][4] = Math.Sqrt(Math.Pow(m_arrValue[i][0] - m_arrValue[i][2], 2) + Math.Pow(m_arrValue[i][1] - m_arrValue[i][3], 2));
                        
            //            for (int k = 1; k <= intNDistBnd; k++)
            //            {
            //                double dblRefDist = k * dblDistInc + dblBeginDist;
            //                if (m_arrValue[i][4] < dblRefDist)
            //                    m_arrResultIdxs[k - 1].Add(i);
            //            }
            //            i++;
            //        }
            //        pFeature2 = pFCursor2.NextFeature();
            //    }
            //    pFCursor2.Flush();
                
            //    pFeature1 = pFCursor1.NextFeature();
            //}
            */
            #endregion

            pChart.Series.Clear();

            var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ObsLine",
                Color = Color.Red,
                IsVisibleInLegend = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

            };

            pChart.Series.Add(pSeries);

            double dblConstant = m_dblArea / Math.Pow(intNFeatureCount, 2);

            for (int j = 0; j < intNDistBnd; j++)
            {

                double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;
                //double dblDenorm = Math.PI * Math.Pow(dblRefDist, 2);

                double dblKhat = (m_arrResultIdxs[j].Count * dblConstant); // Kfunction
                //pSeries.Points.AddXY(dblRefDist, dblKhat);

                double dblLhat = Math.Sqrt(dblKhat / Math.PI) - dblRefDist;
                pSeries.Points.AddXY(dblRefDist, dblLhat); //LFunction
            }
            m_intTotalNSeries = 1;



            //Permutation
            if (chkConfBnd.Checked)
            {
                IEnvelope pEnv = m_pFLayer.AreaOfInterest;

                int intNPermutation = Convert.ToInt32(nudNPermutation.Value);
                double[,] arrConfBnds = new double[intNDistBnd, 2];
                for (int j = 0; j < intNDistBnd; j++)
                {
                    arrConfBnds[j, 0] = double.MinValue;
                    arrConfBnds[j, 1] = double.MaxValue;
                }

                for (int p = 0; p < intNPermutation; p++)
                {
                    double[,] arrRndPts;
                    if (cboStudyArea.Text == "Minimum Enclosing Rectangle ")
                        arrRndPts = RndPtswithEnvelope(intNFeatureCount, pEnv);
                    else
                        arrRndPts = RndPtswithStudyArea(intNFeatureCount, pEnv, m_pFeature);

                    int[] arrPermCount = new int[intNDistBnd];
                    double dblEstDist = 0;

                    for (int j = 0; j < intNFeatureCount; j++)
                    {
                        for (int k = 0; k < intNFeatureCount; k++)
                        {
                            if (j != k)
                            {
                                dblEstDist = Math.Sqrt(Math.Pow(arrRndPts[j, 0] - arrRndPts[k, 0], 2) + Math.Pow(arrRndPts[j, 1] - arrRndPts[k, 1], 2));
                                for (int l = 1; l <= intNDistBnd; l++)
                                {
                                    double dblRefDist = l * m_dblDistInc + m_dblBeginDist;
                                    if (dblEstDist < dblRefDist)
                                        arrPermCount[l - 1]++;
                                }
                            }
                        }
                    }

                    for (int j = 0; j < intNDistBnd; j++)
                    {

                        double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;
                        double dblKhat = (arrPermCount[j] * dblConstant); // Kfunction
                        double dblLhat = Math.Sqrt(dblKhat / Math.PI) - dblRefDist;

                        if (dblLhat > arrConfBnds[j, 0])
                            arrConfBnds[j, 0] = dblLhat;

                        if (dblLhat < arrConfBnds[j, 1])
                            arrConfBnds[j, 1] = dblLhat;

                    }

                }

                var pMaxSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "MaxLine",
                    Color = Color.Gray,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };
                pChart.Series.Add(pMaxSeries);

                var pMinSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "MinLine",
                    Color = Color.Gray,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };
                pChart.Series.Add(pMinSeries);

                for (int j = 0; j < intNDistBnd; j++)
                {
                    double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;

                    pMaxSeries.Points.AddXY(dblRefDist, arrConfBnds[j, 0]);
                    pMinSeries.Points.AddXY(dblRefDist, arrConfBnds[j, 1]);
                }

                m_intTotalNSeries = 3;
            }
        }

        private void CalKhatWithEgde()
        {
            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.bgWorker.ReportProgress(0);
            pfrmProgress.lblStatus.Text = "Caculate Lhat";
            pfrmProgress.Show();

            m_dblBeginDist = Convert.ToDouble(nudBeginDist.Value);
            m_dblDistInc = Convert.ToDouble(nudDistInc.Value);
            int intNDistBnd = Convert.ToInt32(nudNDistBands.Value);
            m_arrResultIdxs = new List<int>[intNDistBnd];
            for (int j = 0; j < intNDistBnd; j++)
                m_arrResultIdxs[j] = new List<int>();

            int intNFeatureCount = 0;

            IFeatureCursor pFCursor1 = m_pFClass.Search(null, false);
            intNFeatureCount = m_pFClass.FeatureCount(null);

            if (intNFeatureCount == 0)
                return;

            int intTotalFlowCnt = intNFeatureCount * (intNFeatureCount - 1);
            m_arrValue = new double[intTotalFlowCnt][];

            IFeature pFeature1 = pFCursor1.NextFeature();
            double[,] arrXYPts = new double[intNFeatureCount, 2];
            IPoint pPoint1;
            int i = 0;
            while (pFeature1 != null)
            {
                pPoint1 = pFeature1.Shape as IPoint;

                arrXYPts[i, 0] = pPoint1.X;
                arrXYPts[i, 1] = pPoint1.Y;
                i++;
                pFeature1 = pFCursor1.NextFeature();
            }

            //For Edge Correction
            var watch = Stopwatch.StartNew();

            double[][] dblWeightCnt = EdgeCorrection(intNFeatureCount, intNDistBnd, m_pFeature, arrXYPts, m_dblDistInc, m_dblBeginDist);
             
            watch.Stop();
            double dblTime = watch.ElapsedMilliseconds;

            //For Edge Correction


            double[] arrResultCnt = new double[intNDistBnd];
            i = 0;
            for (int j = 0; j < intNFeatureCount; j++)
            {
                for (int k = 0; k < intNFeatureCount; k++)
                {
                    if (j != k)
                    {
                        int intProgress = i * 100 / intTotalFlowCnt;
                        pfrmProgress.bgWorker.ReportProgress(intProgress);

                        m_arrValue[i] = new double[5];
                        m_arrValue[i][0] = arrXYPts[j, 0];
                        m_arrValue[i][1] = arrXYPts[j, 1];
                        m_arrValue[i][2] = arrXYPts[k, 0];
                        m_arrValue[i][3] = arrXYPts[k, 1];
                        m_arrValue[i][4] = Math.Sqrt(Math.Pow(arrXYPts[j, 0] - arrXYPts[k, 0], 2) + Math.Pow(arrXYPts[j, 1] - arrXYPts[k, 1], 2));

                        for (int l = 1; l <= intNDistBnd; l++)
                        {
                            double dblRefDist = l * m_dblDistInc + m_dblBeginDist;
                            if (m_arrValue[i][4] < dblRefDist)
                            {
                                m_arrResultIdxs[l - 1].Add(i);
                                arrResultCnt[l - 1] = arrResultCnt[l - 1] + dblWeightCnt[j][l - 1];
                            }
                        }
                        i++;
                    }
                }
            }

            pChart.Series.Clear();

            var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ObsLine",
                Color = Color.Red,
                IsVisibleInLegend = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

            };

            pChart.Series.Add(pSeries);

            double dblConstant = m_dblArea / Math.Pow(intNFeatureCount, 2);

            for (int j = 0; j < intNDistBnd; j++)
            {

                double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;
                //double dblDenorm = Math.PI * Math.Pow(dblRefDist, 2);

                double dblKhat = (arrResultCnt[j] * dblConstant); // Kfunction
                //pSeries.Points.AddXY(dblRefDist, dblKhat);

                double dblLhat = Math.Sqrt(dblKhat / Math.PI) - dblRefDist;
                pSeries.Points.AddXY(dblRefDist, dblLhat); //LFunction
            }
            m_intTotalNSeries = 1;

            

            //Permutation
            if (chkConfBnd.Checked)
            {
                pfrmProgress.bgWorker.ReportProgress(0);
                pfrmProgress.lblStatus.Text = "Caculate Confidence Bound";

                IEnvelope pEnv = m_pFLayer.AreaOfInterest;

                int intNPermutation = Convert.ToInt32(nudNPermutation.Value);
                double[,] arrConfBnds = new double[intNDistBnd, 2];
                for (int j = 0; j < intNDistBnd; j++)
                {
                    arrConfBnds[j, 0] = double.MinValue;
                    arrConfBnds[j, 1] = double.MaxValue;
                }

                for (int p = 0; p < intNPermutation; p++)
                {
                    int intProgress = p * 100 / intNPermutation;
                    pfrmProgress.bgWorker.ReportProgress(intProgress);

                    double[,] arrRndPts = RndPtswithStudyArea(intNFeatureCount, pEnv, m_pFeature);

                    //EdgeCorrection
                    double[][] dblPermWeightCnt = EdgeCorrection(intNFeatureCount, intNDistBnd, m_pFeature, arrRndPts, m_dblDistInc, m_dblBeginDist);
             
                    double[] arrPermCount = new double[intNDistBnd];
                    double dblEstDist = 0;

                    for (int j = 0; j < intNFeatureCount; j++)
                    {
                        for (int k = 0; k < intNFeatureCount; k++)
                        {
                            if (j != k)
                            {
                                dblEstDist = Math.Sqrt(Math.Pow(arrRndPts[j, 0] - arrRndPts[k, 0], 2) + Math.Pow(arrRndPts[j, 1] - arrRndPts[k, 1], 2));
                                for (int l = 1; l <= intNDistBnd; l++)
                                {
                                    double dblRefDist = l * m_dblDistInc + m_dblBeginDist;
                                    if (dblEstDist < dblRefDist)
                                        arrPermCount[l - 1] = dblPermWeightCnt[j][l - 1] + arrPermCount[l - 1];
                                }
                            }
                        }
                    }

                    for (int j = 0; j < intNDistBnd; j++)
                    {

                        double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;
                        double dblKhat = (arrPermCount[j] * dblConstant); // Kfunction
                        double dblLhat = Math.Sqrt(dblKhat / Math.PI) - dblRefDist;

                        if (dblLhat > arrConfBnds[j, 0])
                            arrConfBnds[j, 0] = dblLhat;

                        if (dblLhat < arrConfBnds[j, 1])
                            arrConfBnds[j, 1] = dblLhat;

                    }

                }

                var pMaxSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "MaxLine",
                    Color = Color.Gray,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };
                pChart.Series.Add(pMaxSeries);

                var pMinSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "MinLine",
                    Color = Color.Gray,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };
                pChart.Series.Add(pMinSeries);

                for (int j = 0; j < intNDistBnd; j++)
                {
                    double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;

                    pMaxSeries.Points.AddXY(dblRefDist, arrConfBnds[j, 0]);
                    pMinSeries.Points.AddXY(dblRefDist, arrConfBnds[j, 1]);
                }

                m_intTotalNSeries = 3;
            }

            pfrmProgress.Close();
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
    }
}
