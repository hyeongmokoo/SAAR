using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisUncertainty
{
    public partial class frmConnectivityHistogram : Form
    {
        public clsSnippet.Def_SpatialWeightsMatrix DefSWM;

        public Double[] dblBreaks;
        public Double[] vecMids;
        public Double[] vecCounts;

        public IActiveView pActiveView;
        public IFeatureLayer pFLayer;

        public int intNBins;

        public MainForm m_pForm;

        private string m_strFieldName;
        private clsBrusingLinking m_pBL;

        public frmConnectivityHistogram()
        {
            InitializeComponent();
            m_pBL = new clsBrusingLinking();
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //Export the chart to an image file
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    //conMenu.Show(pChart, e.X, e.Y);
                    return;
                }

                HitTestResult result = pChart.HitTest(e.X, e.Y);

                //Remove Previous Selection
                if (pChart.Series.Count == 2)
                {
                    pChart.Series.RemoveAt(1);
                    pChart.Series.RemoveAt(0);

                    var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Series1",
                        Color = System.Drawing.Color.White,
                        BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Column,

                    };

                    pChart.Series.Add(series1);

                    int intNBins = vecMids.Length;

                    for (int j = 0; j < intNBins; j++)
                    {
                        series1.Points.AddXY(vecMids[j], vecCounts[j]);
                    }
                    pChart.Series[0]["PointWidth"] = "1";
                }

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    //Brushing on histogram
                    int index = result.PointIndex;

                    for (int i = 0; i < pChart.Series[0].Points.Count; i++)
                    {
                        if (i == index)
                            pChart.Series[0].Points[i].Color = System.Drawing.Color.Cyan;
                        else
                            pChart.Series[0].Points[i].Color = System.Drawing.Color.White;
                    }
                    int intSelectedValue = Convert.ToInt32(dblBreaks[index]);
                    //var SelectedFIDs = DefSWM.NeighborCounts.Select(x => x == intSelectedValue);
                    int[] SelectedFIDs = DefSWM.NeighborCounts.Select((b, i) => b == intSelectedValue ? i : -1).Where(i => i != -1).ToArray();
                    StringBuilder whereClause = new StringBuilder();
                    for (int i = 0; i < SelectedFIDs.Length; i++)
                    {
                        whereClause.Append("(" + m_strFieldName + " = " + DefSWM.FIDs[SelectedFIDs[i]].ToString() + ") Or ");
                    }

                    //Brushing on ArcView
                    if (whereClause.Length > 3)
                    {
                        whereClause.Remove(whereClause.Length - 3, 3);
                        m_pBL.FeatureSelectionOnActiveView(whereClause.ToString(), pActiveView, pFLayer);
                    }
                }
                else
                {
                    //Clear Selection Both Histogram and ActiveView
                    for (int i = 0; i < pChart.Series[0].Points.Count; i++)
                    {
                        pChart.Series[0].Points[i].Color = System.Drawing.Color.White;
                    }
                    IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;
                    pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
                //Brushing to ActiveView
                //FeatureSelectionOnActiveView(whereClause, pActiveView, pFLayer);

                //Brushing to other graphs
                // m_pBL.BrushingToOthers(pFLayer, this.Handle);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }




        }
        private void frmConnectivityHistogram_Load(object sender, EventArgs e)
        {
            m_strFieldName = pFLayer.FeatureClass.OIDFieldName;
        }

        private void frmConnectivityHistogram_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                int intFCnt = m_pBL.RemoveBrushing(m_pForm, pFLayer);
                if (intFCnt == -1)
                    return;
                else if (intFCnt == 0)
                {
                    IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}

