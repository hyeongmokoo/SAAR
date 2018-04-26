using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace VisUncertainty
{
    public partial class frmMScatterResults : Form
    {
        public MainForm m_pForm;
        public IActiveView m_pActiveView;
        public IFeatureLayer m_pFLayer;
        //private clsSnippet pSnippet;
        private clsBrusingLinking m_pBL;

        //Variables for ScatterPlot
        public double[] arrVar;
        public int[] arrFID;
        //public double[] arrWeightVar;
        public string strVarNM;
        public string strFIDNM;
        //public string strVar1Name;
        public System.Drawing.Color pMakerColor;

        //Private variables
        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public double[,] m_arrXYCoord; //For Brushing and Linking
        public List<int>[] m_NBIDs;


        private clsSnippet m_pSnippet;
        public class Neighbors
        {
            public List<int>[] IDs { get; set; }
        }
        public frmMScatterResults()
        {
            InitializeComponent();
            //pSnippet = new clsSnippet();
            m_pBL = new clsBrusingLinking();
            m_pSnippet = new clsSnippet();
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                m_pActiveView.GraphicsContainer.DeleteAllElements();
                //Export the chart to an image file
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    conMenu.Show(pChart, e.X, e.Y);
                    return;
                }

                //Clear previous selection
                int intLastSeriesIdx = pChart.Series.Count - 1;

                //Remove Previous Selection
                if (pChart.Series[intLastSeriesIdx].Name == "SelPoints")
                    pChart.Series.RemoveAt(intLastSeriesIdx);


                HitTestResult result = pChart.HitTest(e.X, e.Y);

                int dblOriPtsSize = pChart.Series[0].MarkerSize;
                _canDraw = false;

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
                    MarkerSize = dblOriPtsSize * 2

                };

                pChart.Series.Add(seriesPts);

                StringBuilder plotCommmand = new StringBuilder();

                for (int i = 0; i < pChart.Series[0].Points.Count; i++)
                {
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[0].Points[i].XValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(pChart.Series[0].Points[i].YValues[0]);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        int intValueIdx = arrFID[i];
                        int index = result.PointIndex;
                        seriesPts.Points.AddXY(pChart.Series[0].Points[i].XValue, pChart.Series[0].Points[i].YValues[0]);
                        //plotCommmand.Append("(" + strVarNM + " = " + arrVar[i].ToString() + ") Or ");
                        plotCommmand.Append("(" + strFIDNM + " = " + intValueIdx.ToString() + ") Or ");

                        DrawLineOnActiveView(intValueIdx, m_NBIDs[intValueIdx], m_arrXYCoord, m_pActiveView);
                    }
                }

                //Brushing on ArcView
                if (plotCommmand.Length > 3)
                {
                    plotCommmand.Remove(plotCommmand.Length - 3, 3);
                    string whereClause = plotCommmand.ToString();
                    m_pBL.FeatureSelectionOnActiveView(whereClause, m_pActiveView, m_pFLayer);
                }
                else
                {
                    IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                    m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    //m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                    m_pActiveView.Refresh();
                }
                //Brushing to other graphs //The Function should be locatated after MapView Brushing
                m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void pChart_MouseDown(object sender, MouseEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
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

        private void frmMScatterResults_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                int intFCnt = m_pBL.RemoveBrushing(m_pForm, m_pFLayer);
                if (intFCnt == -1)
                    return;
                else if (intFCnt == 0)
                {
                    m_pActiveView.GraphicsContainer.DeleteAllElements();
                    IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.Refresh();

                }
                else
                    return;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
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

        //The function is under reviewing.
        private void DrawLineOnActiveView(int intFromLinkID, List<int> arrToLinks, double[,] arrXYCoord, IActiveView pActiveView)
        {
            IGraphicsContainer pGraphicContainer = pActiveView.GraphicsContainer;
            //pGraphicContainer.DeleteAllElements();

            IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);

            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
            pSimpleLineSymbol.Width = 2;
            pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pSimpleLineSymbol.Color = pRgbColor;
            int intFromIdx = intFromLinkID;
            ESRI.ArcGIS.Geometry.IPoint FromP = new PointClass();
            FromP.X = arrXYCoord[intFromIdx, 0]; FromP.Y = arrXYCoord[intFromIdx, 1];

            int intArrLengthCnt = arrToLinks.Count;
            for (int i = 0; i < intArrLengthCnt; i++)
            {
                int intToIdx = arrToLinks[i] - 1;

                ESRI.ArcGIS.Geometry.IPoint ToP = new PointClass();
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

    }
}
