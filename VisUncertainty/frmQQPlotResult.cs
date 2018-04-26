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
using ESRI.ArcGIS.Geodatabase;
/*
 * Chart Setting(Default)
 */

namespace VisUncertainty
{
    public partial class frmQQPlotResults : Form
    {
        public MainForm m_pForm;
        public IActiveView m_pActiveView;
        public IFeatureLayer m_pFLayer;

        //private clsSnippet m_pSnippet;
        private clsBrusingLinking m_pBL;
        //public int intGraphType;

        //Variables for QQPlot
        public Double[] adblVar1;
        public Double[] adblVar2;
        //public string strVar2Name;
        public string strVar1Name;
        public System.Drawing.Color pMakerColor;
        public string strDistribution;
        public string strDF1;
        public string strDF2;

        //Private variables
        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public frmQQPlotResults()
        {
            InitializeComponent();
            //m_pSnippet = new clsSnippet();
            m_pBL = new clsBrusingLinking();
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

                m_pActiveView.GraphicsContainer.DeleteAllElements();
                //Clear previous selection
                if (pChart.Series[pChart.Series.Count - 1].Name == "SelPoints")
                    pChart.Series.RemoveAt(pChart.Series.Count - 1);


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

                    Point SelPts = new Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        int index = result.PointIndex;
                        seriesPts.Points.AddXY(pChart.Series[0].Points[i].XValue, pChart.Series[0].Points[i].YValues[0]);
                        plotCommmand.Append(strVar1Name + " = " + adblVar1[i].ToString() + " Or ");
                    }
                }

                //Brushing on ArcView
                if (plotCommmand.Length > 3)
                {
                    plotCommmand.Remove(plotCommmand.Length - 3, 3);
                    string whereClause = plotCommmand.ToString();
                    FeatureSelectionOnActiveView(whereClause, m_pActiveView, m_pFLayer);
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
        
        private void FeatureSelectionOnActiveView(string whereClause, IActiveView pActiveView, IFeatureLayer pFLayer)
        {
            try
            {
                ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast

                // Set up the query
                ESRI.ArcGIS.Geodatabase.IQueryFilter queryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
                queryFilter.WhereClause = whereClause;

                // Invalidate only the selection cache. Flag the original selection
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                // Perform the selection
                featureSelection.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

                // Flag the new selection
                //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                pActiveView.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void frmTemp_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                int intFCnt = m_pBL.RemoveBrushing(m_pForm, m_pFLayer);
                if (intFCnt == -1)
                    return;
                else if (intFCnt == 0)
                {
                    IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
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
