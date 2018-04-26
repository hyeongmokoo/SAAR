using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace VisUncertainty
{
    public partial class frmVisProbEnt : Form
    {
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        private clsSnippet m_pSnippet;
        //private MainForm m_pForm;
        //private REngine m_pEngine;
        public IRasterProps m_pRasterProps;

        public IActiveView m_pActiveView;
        public int m_intLyrCnt;
        public int m_intClsCnt;
        public int m_intTotalNSeries;
        public int m_intTotalNSeriesDN;

        //Check These variables later!
        public List<List<int>> m_lstPtsIdContainer;
        public List<int> m_lstPtSeriesID;
        public List<int[]> m_lstIDsValues;

        //private ClassificationResults[,] m_arrClsResults;
        public double[] m_Entropies;
        public double[] m_laggedEntropies;

        public Chart m_pDNChart;

        public frmVisProbEnt()
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
            
            //Remove the selection in this Chart
            while (pChart.Series.Count != m_intTotalNSeries)
                pChart.Series.RemoveAt(pChart.Series.Count - 1);

            //Remove the selection in DN Chart
            while (m_pDNChart.Series.Count != m_intTotalNSeriesDN)
                m_pDNChart.Series.RemoveAt(m_pDNChart.Series.Count - 1);

            _canDraw = false;

            HitTestResult result = pChart.HitTest(e.X, e.Y);



            int dblOriPtsSize = pChart.Series[m_lstPtSeriesID[0]].MarkerSize;
            int intTotalSriCount = m_lstPtSeriesID.Count;

            int intSelLinesCount = 0;

            List<int> lstColIdx = new List<int>();
            List<int> lstRowIdx = new List<int>();

            for (int i = 0; i < intTotalSriCount; i++)
            {
                int intSeriesID = m_lstPtSeriesID[i];
                int intPtsCount = pChart.Series[intSeriesID].Points.Count;

                for (int j = 0; j < intPtsCount; j++)
                {
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[intSeriesID].Points[j].XValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[intSeriesID].Points[j].YValues[0]);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        double dblXOffset = (pChart.Series[intSeriesID].Points[j].XValue) - Math.Round(pChart.Series[intSeriesID].Points[j].XValue, 0);
                        
                        System.Drawing.Color BrushingColor = System.Drawing.Color.Red;
                        var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "SelLines" + intSelLinesCount.ToString(),
                            Color = BrushingColor,
                            BorderColor = BrushingColor,
                            IsVisibleInLegend = false,
                            IsXValueIndexed = false,
                            //ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                            MarkerSize = dblOriPtsSize * 2

                        };
                        m_pDNChart.Series.Add(seriesLines);

                        System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                        var seriesUncerLines = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "SelLines" + intSelLinesCount.ToString(),
                            Color = pMarkerColor,
                            BorderColor = pMarkerColor,
                            IsVisibleInLegend = false,
                            IsXValueIndexed = false,
                            //ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                            MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                            MarkerSize = dblOriPtsSize * 2

                        };

                        pChart.Series.Add(seriesUncerLines);

                        intSelLinesCount++;
                        int intSelLocationIdx = m_lstPtsIdContainer[i][j];
                        int[] dblSelValues = m_lstIDsValues[intSelLocationIdx];
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
                        lstColIdx.Add(m_lstIDsValues[intSelLocationIdx][0]);
                        lstRowIdx.Add(m_lstIDsValues[intSelLocationIdx][1]);
                    }

                }
            }
            frmVisEntropy pfrmVisEntropy = System.Windows.Forms.Application.OpenForms["frmVisEntropy"] as frmVisEntropy;
            BrushPointsOnMCPlot(lstColIdx, lstRowIdx, m_pRasterProps, pfrmVisEntropy);

            if (intSelLinesCount == 0)
            {
                m_pActiveView.GraphicsContainer.DeleteAllElements();
                m_pActiveView.Refresh();
            }
        }
        private void BrushPointsOnMCPlot(List<int> colindex, List<int> rowindex, IRasterProps pRasterProps, frmVisEntropy pfrmVisEntropy)
        {

            int intLength = colindex.Count;
            Chart pVisEntChart = pfrmVisEntropy.pChart;
            int intLastSeriesIdx = pVisEntChart.Series.Count - 1;

            //Remove Previous Selection
            if (pVisEntChart.Series[intLastSeriesIdx].Name == "SelPoints")
                pVisEntChart.Series.RemoveAt(intLastSeriesIdx);

            if (intLength == 0)
                return;

            int dblOriPtsSize = pVisEntChart.Series[0].MarkerSize;

            System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "SelPoints",
                Color = pMarkerColor,
                BorderColor = pMarkerColor,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                MarkerSize = dblOriPtsSize * 2

            };

            pVisEntChart.Series.Add(seriesPts);

            for (int i = 0; i < intLength; i++)
            {
                int intSelID = (colindex[i] * pRasterProps.Height) + rowindex[i];
                seriesPts.Points.AddXY(pfrmVisEntropy.m_Entropies[intSelID], pfrmVisEntropy.m_laggedEntropies[intSelID]);
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
