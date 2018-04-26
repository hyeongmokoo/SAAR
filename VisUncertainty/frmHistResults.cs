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
 * Chart Setting
 * Grid Setting
Chart Area > Axes > GridTickMarks : All MajorGrid Set to Disable

 * Legend Setting- Disable
 */



namespace VisUncertainty
{
    public partial class frmHistResults : Form
    {
        public Double[] dblBreaks;
        public Double[] vecMids;
        public Double[] vecCounts;
        public IActiveView pActiveView;
        public IFeatureLayer pFLayer;
        public string strFieldName;
        public int intNBins;
        public Double[] dblMins;

        public MainForm m_pForm;

        //private clsSnippet m_pSnippet;
        private clsBrusingLinking m_pBL;

        public frmHistResults()
        {
            InitializeComponent();
            //m_pSnippet = new clsSnippet();
            m_pBL = new clsBrusingLinking();
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                pActiveView.GraphicsContainer.DeleteAllElements();

                //Export the chart to an image file
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    conMenu.Show(pChart, e.X, e.Y);
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

                    string whereClause = strFieldName + " > " + dblBreaks[index].ToString() + " And " + strFieldName + " <= " + dblBreaks[index + 1].ToString();

                    //Brushing to ActiveView
                    FeatureSelectionOnActiveView(whereClause, pActiveView, pFLayer);

                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(pFLayer, this.Handle);
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
                    //pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                    pActiveView.Refresh();
                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(pFLayer, this.Handle);
                }
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
                pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                // Perform the selection
                featureSelection.SelectFeatures(queryFilter, ESRI.ArcGIS.Carto.esriSelectionResultEnum.esriSelectionResultNew, false);

                // Flag the new selection
                pActiveView.Refresh();
                //pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
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
            
            
            //string strOutputPath = null;
            //if (sfdExportImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    strOutputPath = string.Concat(sfdExportImage.FileName);

            //    switch (sfdExportImage.FilterIndex)
            //    {
            //        case 1:
            //            pChart.SaveImage(strOutputPath, ChartImageFormat.Bmp);
            //            break;
            //        case 2:
            //            pChart.SaveImage(strOutputPath, ChartImageFormat.Emf);
            //            break;
            //        case 3:
            //            pChart.SaveImage(strOutputPath, ChartImageFormat.Gif);
            //            break;
            //        case 4:
            //            pChart.SaveImage(strOutputPath, ChartImageFormat.Jpeg);
            //            break;
            //        case 5:
            //            pChart.SaveImage(strOutputPath, ChartImageFormat.Png);
            //            break;
            //        case 6:
            //            pChart.SaveImage(strOutputPath, ChartImageFormat.Tiff);
            //            break;
            //    }

            //    //// save to a bitmap
            //    //Bitmap bmp = new Bitmap(pChart.Size.Width, pChart.Size.Height);
            //    //bmp.SetResolution(300, 300);
            //    //bmp.SetResolution(96, 96);
            //    //pChart.DrawToBitmap(bmp, new Rectangle(0, 0, pChart.Size.Width, pChart.Size.Height));
            //    //bmp.Save(strOutputPath);

            //    MessageBox.Show("The export completed successfully (" + strOutputPath + ")");
            //}
        }
    }
}
