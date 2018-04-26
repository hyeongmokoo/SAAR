using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesRaster;

namespace VisUncertainty
{
    public partial class frmVisEntropy : Form
    {
        public int[] m_EntUID;
        public double[] m_Entropies;
        public double[] m_laggedEntropies;

        public IActiveView m_pActiveView;
        public int m_intLyrCnt;
        public int m_intClsCnt;
        public int m_intTotalNSeriesUncern;
        public int m_intTotalNSeriesDN;

        public Chart m_pDNChart;
        public Chart m_pUncernChart;

        public List<int[]> m_lstIDsValues;
        public IRasterProps m_pRasterProps;

        public int intOriPtsSizeOnLine;
        private clsSnippet m_pSnippet;
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;

        public frmVisEntropy()
        {
            InitializeComponent();
            m_pSnippet = new clsSnippet();
        }

        private void pChart_MouseDown(object sender, MouseEventArgs e)
        {
            _canDraw = true;
            _startX = e.X;
            _startY = e.Y;
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
            //Clear previous selection
            m_pActiveView.GraphicsContainer.DeleteAllElements();

            //Remove Previous Selection
            int intLastSeriesIdx = pChart.Series.Count - 1;
            if (pChart.Series[intLastSeriesIdx].Name == "SelPoints")
                pChart.Series.RemoveAt(intLastSeriesIdx);

            //Remove the selection in this Chart
            while (m_pUncernChart.Series.Count != m_intTotalNSeriesUncern)
                m_pUncernChart.Series.RemoveAt(m_pUncernChart.Series.Count - 1);

            //Remove the selection in DN Chart
            while (m_pDNChart.Series.Count != m_intTotalNSeriesDN)
                m_pDNChart.Series.RemoveAt(m_pDNChart.Series.Count - 1);

            _canDraw = false;

            HitTestResult result = pChart.HitTest(e.X, e.Y);



            int dblOriPtsSize = pChart.Series[0].MarkerSize;
            //int intTotalSriCount = m_lstPtSeriesID.Count;

            int intSelLinesCount = 0;

            //List<int> lstColIdx = new List<int>();
            //List<int> lstRowIdx = new List<int>();

            //for (int i = 0; i < intTotalSriCount; i++)
            //{
            //    int intSeriesID = m_lstPtSeriesID[i];
                int intPtsCount = pChart.Series[0].Points.Count;
                System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "SelPoints",
                    Color = pMarkerColor,
                    BorderColor = pMarkerColor,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                    MarkerSize = dblOriPtsSize * 4

                };

                pChart.Series.Add(seriesPts);
                for (int j = 0; j < intPtsCount; j++)
                {
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[0].Points[j].XValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[0].Points[j].YValues[0]);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);
                    System.Drawing.Color pBrushingColor = System.Drawing.Color.Red;
                    if (_rect.Contains(SelPts))
                    {

                        var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "SelLines" + intSelLinesCount.ToString(),
                            Color = pBrushingColor,
                            BorderColor = pBrushingColor,
                            IsVisibleInLegend = false,
                            IsXValueIndexed = false,
                            //ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                            MarkerSize = intOriPtsSizeOnLine * 2

                        };
                        m_pDNChart.Series.Add(seriesLines);

                        var seriesUncerLines = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "SelLines" + intSelLinesCount.ToString(),
                            Color = pBrushingColor,
                            BorderColor = pBrushingColor,
                            IsVisibleInLegend = false,
                            IsXValueIndexed = false,
                            //ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                            MarkerSize = intOriPtsSizeOnLine * 2

                        };

                        m_pUncernChart.Series.Add(seriesUncerLines);

                        intSelLinesCount++;

                        int intSelLocationIdx = j;

                        seriesPts.Points.AddXY(m_Entropies[intSelLocationIdx], m_laggedEntropies[intSelLocationIdx]);

                        int[] dblSelValues = m_lstIDsValues[intSelLocationIdx];

                        double dblPlotHalfWidth = 0.08; //Adjust spacing
                        double dblMin = (dblPlotHalfWidth * (m_intClsCnt - 1)) * (-1);
                        double dblXOffset = (dblMin + (dblSelValues[2] * (dblPlotHalfWidth * 2)));

                        //double dblXOffset = (m_pDNChart.Series[0].Points[j].XValue) - Math.Round(m_pDNChart.Series[0].Points[j].XValue, 0); //Checking Again,
                        
                        for (int k = 0; k < m_intLyrCnt; k++)
                        {
                            int intYvalue = m_lstIDsValues[intSelLocationIdx][3 + k];
                            seriesLines.Points.AddXY(k + dblXOffset, intYvalue);
                        }
                        for (int k = 0; k < m_intClsCnt; k++)
                        {
                            int intYvalue = m_lstIDsValues[intSelLocationIdx][3 + m_intLyrCnt + k];
                            seriesUncerLines.Points.AddXY(k + dblXOffset, intYvalue);
                        }

                        DrawPointsOnActiveView(m_lstIDsValues[intSelLocationIdx][0], m_lstIDsValues[intSelLocationIdx][1], m_pRasterProps, m_pActiveView);
                        //lstColIdx.Add(m_lstIDsValues[intSelLocationIdx][0]);
                        //lstRowIdx.Add(m_lstIDsValues[intSelLocationIdx][1]);
                    }

                }
            //}
            frmVisEntropy pfrmVisEntropy = System.Windows.Forms.Application.OpenForms["frmVisEntropy"] as frmVisEntropy;
            //BrushPointsOnMCPlot(lstColIdx, lstRowIdx, m_pRasterProps, pfrmVisEntropy);

            if (intSelLinesCount == 0)
            {
                m_pActiveView.GraphicsContainer.DeleteAllElements();
                m_pActiveView.Refresh();
            }
        }

        private void DrawPointsOnActiveView(int colindex, int rowindex, IRasterProps pRasterProps, IActiveView ActiveView)
        {
            IGraphicsContainer pGraphicContainer = ActiveView.GraphicsContainer;

            IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);

            //ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            //pSimpleLineSymbol.Width = 2;
            //pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            //pSimpleLineSymbol.Color = pRgbColor;

            ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
            pSimpleMarkerSymbol.Size = 8;
            pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pSimpleMarkerSymbol.Color = pRgbColor;

            double dblX = 0, dblY = 0;
            double dblCellSize = Convert.ToDouble(pRasterProps.MeanCellSize().X);
            dblX = pRasterProps.Extent.XMin + (dblCellSize / 2) + dblCellSize * colindex;
            dblY = pRasterProps.Extent.YMax - (dblCellSize / 2) - dblCellSize * rowindex;

            IPoint pPoint = new PointClass();
            pPoint.X = dblX;
            pPoint.Y = dblY;

            IElement pElement = new MarkerElementClass();

            IMarkerElement pMarkerElement = (IMarkerElement)pElement;
            pMarkerElement.Symbol = pSimpleMarkerSymbol;
            pElement.Geometry = pPoint;

            pGraphicContainer.AddElement(pElement, 0);

            ActiveView.Refresh();


        }
    }
}
