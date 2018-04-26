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
    public partial class frmVariogramCloud : Form
    {
        private clsSnippet m_pSnippet;
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

                
        //private double m_dblArea = 0;
        private List<int>[] m_arrResultIdxs;
        private double[][] m_arrValue;
        private int m_intTotalNSeries;
        private double m_dblBeginDist;
        private double m_dblDistInc;
        private int m_intNDistBnd;
        private int m_intTotalFlowCnt = 0;

        private double m_dblOldXMax = 0;
        private double m_dblOldYMax = 0;

        private double m_dblSemiXMax = 0;
        private double m_dblSemiYMax = 0;
        private bool m_blnZoomIn = true;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public frmVariogramCloud()
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

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        #region Actions
        private void rdbCloud_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCloud.Checked)
                rdbSemiVariogram.Checked = false;
            else
                rdbSemiVariogram.Checked = true;
        }

        private void rdbSemiVariogram_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSemiVariogram.Checked)
                rdbCloud.Checked = false;
            else
                rdbCloud.Checked = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            VarioGramCloud();
            rdbCloud.Enabled = true;
            //this.Size = new Size(700, this.Size.Height);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmVariogramCloud_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(261, this.Size.Height);
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLayerName = cboTargetLayer.Text;

            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

            m_pFLayer = pLayer as IFeatureLayer;
            double dblHeight = m_pFLayer.AreaOfInterest.Height;
            double dblWidth = m_pFLayer.AreaOfInterest.Width;

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

            //double dblNdigits = Math.Pow(10, Math.Ceiling(Math.Log10(dblShortLine)));
            double dblNdigits = Math.Pow(10, Math.Ceiling(Math.Log10(dblShortLine / 2)));

            double dblValue = Math.Round(dblShortLine / 2 / dblNdigits, 1) * (dblNdigits / 10);
            nudDistInc.Value = Convert.ToDecimal(dblValue);

            m_pFClass = m_pFLayer.FeatureClass;

            IFields fields = m_pFClass.Fields;

            cboValueFld.Items.Clear();

            for (int i = 0; i < fields.FieldCount; i++)
            {
                if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                {
                    cboValueFld.Items.Add(fields.get_Field(i).Name);
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSemiVariogram();
            rdbSemiVariogram.Enabled = true;
            btnZoom.Enabled = true;
        }
        #endregion

        private void VarioGramCloud()
        {
            try
            {
                string strValueFldNM = cboValueFld.Text;
                int intValueFldIdx = m_pFClass.FindField(strValueFldNM);

                int intNFeatureCount = 0;

                IFeatureCursor pFCursor1 = m_pFClass.Search(null, false);
                intNFeatureCount = m_pFClass.FeatureCount(null);

                if (intNFeatureCount == 0)
                    return;

                m_arrValue = new double[intNFeatureCount * (intNFeatureCount - 1)][];

                IFeature pFeature1 = pFCursor1.NextFeature();
                double[,] arrXYPts = new double[intNFeatureCount, 3];
                IPoint pPoint1;
                int i = 0;
                while (pFeature1 != null)
                {
                    pPoint1 = pFeature1.Shape as IPoint;

                    arrXYPts[i, 0] = pPoint1.X;
                    arrXYPts[i, 1] = pPoint1.Y;
                    arrXYPts[i, 2] = Convert.ToDouble(pFeature1.get_Value(intValueFldIdx));
                    i++;
                    pFeature1 = pFCursor1.NextFeature();
                }

                pChart.Series.Clear();

                //Add Cloud point
                var pPtsCloud = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Cloud",
                    Color = Color.Blue,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,

                };

                pChart.Series.Add(pPtsCloud);
                m_intTotalNSeries = pChart.Series.Count;

                i = 0;
                for (int j = 0; j < intNFeatureCount; j++)
                {
                    for (int k = 0; k < intNFeatureCount; k++)
                    {
                        if (j != k)
                        {
                            m_arrValue[i] = new double[6];
                            m_arrValue[i][0] = arrXYPts[j, 0];
                            m_arrValue[i][1] = arrXYPts[j, 1];
                            m_arrValue[i][2] = arrXYPts[k, 0];
                            m_arrValue[i][3] = arrXYPts[k, 1];
                            m_arrValue[i][4] = Math.Sqrt(Math.Pow(arrXYPts[j, 0] - arrXYPts[k, 0], 2) + Math.Pow(arrXYPts[j, 1] - arrXYPts[k, 1], 2));
                            //m_arrValue[i][5] = Math.Abs(arrXYPts[k, 2] - arrXYPts[j, 2]);
                            m_arrValue[i][5] = Math.Pow(arrXYPts[k, 2] - arrXYPts[j, 2], 2);

                            //pPtsCloud.Points.AddXY(m_arrValue[i][4], m_arrValue[i][5]);
                            pPtsCloud.Points.AddXY(m_arrValue[i][4], m_arrValue[i][5] / 2);

                            //for (int l = 1; l <= m_intNDistBnd; l++)
                            //{
                            //    double dblUpperRefDist = l * m_dblDistInc + m_dblBeginDist;
                            //    double dblLowerRefDist = (l - 1) * m_dblDistInc + m_dblBeginDist;
                            //    if (m_arrValue[i][4] < dblUpperRefDist && m_arrValue[i][4] >= dblLowerRefDist)
                            //    {
                            //        m_arrResultIdxs[l - 1].Add(i);
                            //    }
                            //}
                            i++;
                        }
                    }
                }
                m_intTotalFlowCnt = i;
                if (m_intTotalFlowCnt != 0)
                {
                    grbVariogram.Enabled = true;
                }

                pChart.ChartAreas[0].AxisY.Title = "Gamma";
                pChart.ChartAreas[0].AxisX.LabelStyle.Format = "{0:###0.###}";
                pChart.ChartAreas[0].AxisX.IsStartedFromZero = true;
                pChart.Update();
                m_dblOldXMax = pChart.ChartAreas[0].AxisX.Maximum;
                m_dblOldYMax = pChart.ChartAreas[0].AxisY.Maximum;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void AddSemiVariogram()
        {
            try
            {
                m_dblBeginDist = Convert.ToDouble(nudBeginDist.Value);
                m_dblDistInc = Convert.ToDouble(nudDistInc.Value);
                m_intNDistBnd = Convert.ToInt32(nudNDistBands.Value);

                m_arrResultIdxs = new List<int>[m_intNDistBnd];
                for (int j = 0; j < m_intNDistBnd; j++)
                    m_arrResultIdxs[j] = new List<int>();

                while (pChart.Series.Count > 1)
                    pChart.Series.RemoveAt(pChart.Series.Count - 1);

                for (int i = 0; i < m_intTotalFlowCnt; i++)
                {
                    for (int l = 1; l <= m_intNDistBnd; l++)
                    {
                        double dblUpperRefDist = l * m_dblDistInc + m_dblBeginDist;
                        double dblLowerRefDist = (l - 1) * m_dblDistInc + m_dblBeginDist;
                        if (m_arrValue[i][4] < dblUpperRefDist && m_arrValue[i][4] >= dblLowerRefDist)
                        {
                            m_arrResultIdxs[l - 1].Add(i);
                        }
                    }
                }

                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "MeanPts",
                    Color = Color.Red,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,

                };

                pChart.Series.Add(pSeries);
                m_intTotalNSeries = pChart.Series.Count;
                for (int j = 0; j < m_intNDistBnd; j++)
                {
                    double dblRefDist = (j + 1) * m_dblDistInc + m_dblBeginDist;
                    int intCnt = m_arrResultIdxs[j].Count;
                    double dblSum = 0;

                    for (int k = 0; k < intCnt; k++)
                    {
                        dblSum += m_arrValue[m_arrResultIdxs[j][k]][5];
                    }

                    pSeries.Points.AddXY(dblRefDist, dblSum / intCnt / 2);
                }

                double dblMinValue = double.MaxValue;
                m_dblSemiYMax = double.MinValue;

                for (int j = 0; j < pSeries.Points.Count; j++)
                {
                    double dblValue = pSeries.Points[j].YValues[0];

                    if (dblValue > m_dblSemiYMax)
                        m_dblSemiYMax = dblValue;

                    if (dblValue < dblMinValue)
                        dblMinValue = dblValue;

                }

                m_dblSemiYMax = m_dblSemiYMax + ((m_dblSemiYMax - dblMinValue) / 20);
                m_dblSemiXMax = m_dblBeginDist + (m_dblDistInc * (m_intNDistBnd + 1));
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
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

                _canDraw = false;

                HitTestResult result = pChart.HitTest(e.X, e.Y);

                if (rdbSemiVariogram.Checked)
                {
                    int dblOriPtsSize = pChart.Series[1].MarkerSize;

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

                    StringBuilder plotCommmand = new StringBuilder();
                    int intSelPtsCnt = 0;
                    for (int i = 0; i < pChart.Series[1].Points.Count; i++)
                    {
                        int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[1].Points[i].XValue);
                        int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[1].Points[i].YValues[0]);

                        System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                        if (_rect.Contains(SelPts))
                        {
                            intSelPtsCnt++;

                            int index = result.PointIndex;
                            double dblRefDist = pChart.Series[1].Points[i].XValue;
                            seriesLines.Points.AddXY(pChart.Series[1].Points[i].XValue, pChart.Series[1].Points[i].YValues[0]);
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
                else
                {
                    int dblOriPtsSize = pChart.Series[0].MarkerSize;


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

                    StringBuilder plotCommmand = new StringBuilder();
                    int intSelPtsCnt = 0;
                    List<int> lstSelIdx = new List<int>();

                    for (int i = 0; i < pChart.Series[0].Points.Count; i++)
                    {
                        int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[0].Points[i].XValue);
                        int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[0].Points[i].YValues[0]);

                        System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                        if (_rect.Contains(SelPts))
                        {
                            intSelPtsCnt++;
                            seriesLines.Points.AddXY(pChart.Series[0].Points[i].XValue, pChart.Series[0].Points[i].YValues[0]);
                            lstSelIdx.Add(i);
                        }
                    }
                    if (intSelPtsCnt == 0)
                    {
                        m_pActiveView.GraphicsContainer.DeleteAllElements();
                        m_pActiveView.Refresh();
                    }
                    else
                    {
                        DrawLineOnActiveView(lstSelIdx, m_arrValue, m_pActiveView);
                    }
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
            try
            {
                int intLstCnt = lstIndices.Count;

                IGraphicsContainer pGraphicContainer = pActiveView.GraphicsContainer;
                pGraphicContainer.DeleteAllElements();

                IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);
                //IRgbColor pRgbColor = new RgbColorClass();
                //pRgbColor.Red = 0;
                //pRgbColor.Green = 255;
                //pRgbColor.Blue = 255;
                //pRgbColor.Transparency = 255;


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
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        #endregion

        private void btnZoom_Click(object sender, EventArgs e)
        {
            if (m_blnZoomIn)
            {
                pChart.ChartAreas[0].AxisX.Maximum = m_dblSemiXMax;
                pChart.ChartAreas[0].AxisY.Maximum = m_dblSemiYMax;
                m_blnZoomIn = false;
                btnZoom.Text = "Zoom Out";
            }
            else
            {
                pChart.ChartAreas[0].AxisX.Maximum = m_dblOldXMax;
                pChart.ChartAreas[0].AxisY.Maximum = m_dblOldYMax;
                m_blnZoomIn = true;
                btnZoom.Text = "Zoom In";
            }


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
