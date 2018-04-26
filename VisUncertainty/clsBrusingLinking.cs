using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisUncertainty
{
    class clsBrusingLinking
    {
        public int RemoveBrushing(MainForm mForm, IFeatureLayer pFLayer)
        {
            try
            {
                FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
                IActiveView pActiveView = mForm.axMapControl1.ActiveView;
                int intFormCnt = 0;
                if (pFLayer.Visible)
                {
                    //Brushing to Mapcontrol
                    string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                    //IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;


                    for (int j = 0; j < pFormCollection.Count; j++)
                    {

                        if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
                        {
                            frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
                            if (pfrmHistResults.pFLayer == pFLayer)
                                intFormCnt++;
                        }
                        if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
                        {
                            frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
                            if (pfrmPointsPlot.m_pFLayer == pFLayer)
                                intFormCnt++;

                        }
                        if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
                        {
                            frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
                            if (pfrmQQPlotResult.m_pFLayer == pFLayer)
                                intFormCnt++;
                        }
                        if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
                        {
                            frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
                            if ((IFeatureLayer)pfrmAttTable.m_pLayer == pFLayer)
                                intFormCnt++;
                        }
                        if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
                        {
                            frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
                            if (pfrmBoxPlotResult.pFLayer == pFLayer)
                                intFormCnt++;
                        }
                        //if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
                        //{
                        //    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
                        //    if (pfrmClassGraph.m_pFLayer == pFLayer)
                        //        intFormCnt++;
                        //}
                        if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
                        {
                            frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
                            if (pfrmMScatterPlot.m_pFLayer == pFLayer)
                                intFormCnt++;
                        }


                    }
                    return intFormCnt;
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return -1;
            }
        }
        public string RemoveWarningByBrushingTechnique(IFeatureLayer pFLayer)
        {
            //try
            //{
            FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
            //IActiveView pActiveView = mForm.axMapControl1.ActiveView;
            string strRelatedPlotName = string.Empty;
            for (int j = 0; j < pFormCollection.Count; j++)
            {
                if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
                {
                    frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
                    if (pfrmHistResults.pFLayer == pFLayer)
                        strRelatedPlotName += "Histogram, ";
                }
                if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
                {
                    frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
                    if (pfrmPointsPlot.m_pFLayer == pFLayer)
                        strRelatedPlotName += "Scatter plot, ";

                }
                if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
                {
                    frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
                    if (pfrmQQPlotResult.m_pFLayer == pFLayer)
                        strRelatedPlotName += "QQ-plot, ";
                }
                //if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
                //{
                //    frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
                //    BrushingToAttTable(pfrmAttTable, pFLayer, featureSelection);
                //}
                if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
                {
                    frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
                    if (pfrmBoxPlotResult.pFLayer == pFLayer)
                        strRelatedPlotName += "Boxplot, ";
                }
                //if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
                //{
                //    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
                //    BrushingToOptimizeGraph(pfrmClassGraph, pFLayer, featureSelection);
                //}
                //if (pFormCollection[j].Name == "frmCSDesign")//Brushing to CS Graph
                //{
                //    frmCSDesign pfrmCSDesign = pFormCollection[j] as frmCSDesign;
                //    if (pfrmCSDesign.pFLayer == pFLayer)
                //        strRelatedPlotName += "Uncertainty Classification, ";
                //}
                if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
                {
                    frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
                    if (pfrmMScatterPlot.m_pFLayer == pFLayer)
                        strRelatedPlotName += "Moran Scatter Plot, ";
                }

            }
            if (strRelatedPlotName != string.Empty)
                strRelatedPlotName = strRelatedPlotName.Remove(strRelatedPlotName.Length - 2);
            return strRelatedPlotName;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Exception:" + ex.Message);
            //    return;
            //}
        }
        public void CloseAllRelatedPlots(IFeatureLayer pFLayer)
        {
            //try
            //{
            FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
            List<int> lstPlotIdx = new List<int>();

            for (int j = 0; j < pFormCollection.Count; j++)
            {
                if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
                {
                    frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
                    if (pfrmHistResults.pFLayer == pFLayer)
                        lstPlotIdx.Add(j);
                }
                if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
                {
                    frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
                    if (pfrmPointsPlot.m_pFLayer == pFLayer)
                        lstPlotIdx.Add(j);

                }
                if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
                {
                    frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
                    if (pfrmQQPlotResult.m_pFLayer == pFLayer)
                        lstPlotIdx.Add(j);
                }
                if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
                {
                    frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
                    if (pfrmAttTable.m_pLayer == (ILayer)pFLayer)
                        lstPlotIdx.Add(j);
                }
                if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
                {
                    frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
                    if (pfrmBoxPlotResult.pFLayer == pFLayer)
                        lstPlotIdx.Add(j);
                }
                //if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
                //{
                //    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
                //    BrushingToOptimizeGraph(pfrmClassGraph, pFLayer, featureSelection);
                //}
                //if (pFormCollection[j].Name == "frmCSDesign")//Brushing to CS Graph
                //{
                //    frmCSDesign pfrmCSDesign = pFormCollection[j] as frmCSDesign;
                //    if (pfrmCSDesign.pFLayer == pFLayer)
                //        lstPlotIdx.Add(j);
                //}
                if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
                {
                    frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
                    if (pfrmMScatterPlot.m_pFLayer == pFLayer)
                        lstPlotIdx.Add(j);
                }
                if (pFormCollection[j].Name == "frmCorrelogram_local")//Brushing to Correlogram
                {
                    frmCorrelogram_local pfrmCorrelogram = pFormCollection[j] as frmCorrelogram_local;
                    if (pfrmCorrelogram.m_pFLayer == pFLayer)
                        lstPlotIdx.Add(j);
                }
                
            }

            lstPlotIdx.Sort();
            for (int j = lstPlotIdx.Count - 1; j >= 0; j--)
            {
                pFormCollection[lstPlotIdx[j]].Close();
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Exception:" + ex.Message);
            //    return;
            //}
        }

        public void BrushingToOthers(IFeatureLayer pFLayer, IntPtr intPtrParent)
        {
            try
            {
                FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
                //IActiveView pActiveView = mForm.axMapControl1.ActiveView;

                if (pFLayer.Visible)
                {
                    //Brushing to Mapcontrol
                    string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                    IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;


                    for (int j = 0; j < pFormCollection.Count; j++)
                    {
                        if (pFormCollection[j].Handle != intPtrParent) // Brushing to Others
                        {

                            if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
                            {
                                frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
                                BrushingToHistogram(pfrmHistResults, pFLayer, featureSelection);
                            }
                            if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
                            {
                                frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
                                BrushingToScatterPlot(pfrmPointsPlot, pFLayer, featureSelection);

                            }
                            if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
                            {
                                frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
                                BrushingToQQPlot(pfrmQQPlotResult, pFLayer, featureSelection);
                            }
                            if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
                            {
                                frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
                                BrushingToAttTable(pfrmAttTable, pFLayer, featureSelection);
                            }
                            if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
                            {
                                frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
                                BrushingToBoxPlot(pfrmBoxPlotResult, pFLayer, featureSelection);
                            }
                            //if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
                            //{
                            //    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
                            //    BrushingToOptimizeGraph(pfrmClassGraph, pFLayer, featureSelection);
                            //}
                            //if (pFormCollection[j].Name == "frmCSDesign")//Brushing to CS Graph
                            //{
                            //    frmCSDesign pfrmCSDesign = pFormCollection[j] as frmCSDesign;
                            //    BrushingToClassSepGraph(pfrmCSDesign, pFLayer, featureSelection);
                            //}
                            if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
                            {
                                frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
                                BrushingToMScatterPlot(pfrmMScatterPlot, pFLayer, featureSelection);
                            }
                            if (pFormCollection[j].Name == "frmCCMapsResults")//Brushing to Moran ScatterPlot
                            {
                                frmCCMapsResults pfrmCCMApsResults = pFormCollection[j] as frmCCMapsResults;
                                BrushingToCCMaps(pfrmCCMApsResults, pFLayer, featureSelection);
                            }
                            if (pFormCollection[j].Name == "frmBoxCox")//Brushing to Box-Cox transformation tool
                            {
                                frmBoxCox pfrmBoxCox =  pFormCollection[j] as frmBoxCox;
                                BrushingToBoxCox(pfrmBoxCox, pFLayer, featureSelection);
                            }
                            if (pFormCollection[j].Name == "frmCorrelogram_local")//Brushing to Correlogram
                            {
                                frmCorrelogram_local pfrmCorrelogram = pFormCollection[j] as frmCorrelogram_local;
                                BrushingToCorrelogram(pfrmCorrelogram, pFLayer, featureSelection);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        private void BrushingToCorrelogram(frmCorrelogram_local pfrmCorrelogram, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            if (pfrmCorrelogram.m_pFLayer == pFLayer)
            {
                IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                //int intVarIdx = pFeatureClass.Fields.FindField(pfrmMScatterResults.strVarNM);
                int intFIDIdx = pFeatureClass.FindField(pFeatureClass.OIDFieldName);

                ICursor pCursor = null;
                featureSelection.SelectionSet.Search(null, false, out pCursor);

                IRow pRow = pCursor.NextRow();
                //IFeature pFeature = pFCursor.NextFeature();

                int dblOriPtsSize = pfrmCorrelogram.pChart.Series[0].MarkerSize;

                //Remove Previous Selection
                while (pfrmCorrelogram.pChart.Series.Count != pfrmCorrelogram.m_intTotalNSeries)
                    pfrmCorrelogram.pChart.Series.RemoveAt(pfrmCorrelogram.pChart.Series.Count - 1);

                int intSelLinesCount = 0;

                while (pRow != null)
                {
                    //Add Sel Lines
                    var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "SelLines" + intSelLinesCount.ToString(),
                        Color = System.Drawing.Color.Red,
                        BorderColor = System.Drawing.Color.Red,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = false,
                        ChartType = SeriesChartType.Line,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = dblOriPtsSize * 2
                    };

                    intSelLinesCount++;
                    pfrmCorrelogram.pChart.Series.Add(seriesLines);

                    int intSelFID = Convert.ToInt32(pRow.get_Value(intFIDIdx));
                    int intValueIdx = Array.IndexOf(pfrmCorrelogram.m_pFIDs, intSelFID);

                    for (int l = 0; l < pfrmCorrelogram.m_intMaxLag; l++)
                    {
                        int intXvalue = l * 2 + 1;
                        seriesLines.Points.AddXY(intXvalue, pfrmCorrelogram.m_lstCorrelograms[l].LocalMeasureEst[intValueIdx]);
                    }

                    
                    pRow = pCursor.NextRow();
                }
            }

        }
        private void BrushingToBoxCox(frmBoxCox pfrmBoxCox, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            if (pfrmBoxCox.m_pFLayer == pFLayer)
            {
                pfrmBoxCox.BrushingToQQPlot(pFLayer);
                pfrmBoxCox.BrushingToHistogram(pFLayer);
            }
        }
        private void BrushingToCCMaps(frmCCMapsResults pfrmCCMapResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            if (pfrmCCMapResults.pFLayer == pFLayer)
            {


                pfrmCCMapResults.blnBrushingFromMapControl = true;

                IFeatureClass pFeatureClass = pFLayer.FeatureClass;

                string strFIDName = pFeatureClass.OIDFieldName;
                int intFIDIdx = pFeatureClass.FindField(strFIDName);

                ICursor pCursor = null;
                featureSelection.SelectionSet.Search(null, false, out pCursor);

                IRow pRow = pCursor.NextRow();
                StringBuilder plotCommmand = new StringBuilder();

                string whereClause = null;
                while (pRow != null)
                {
                    plotCommmand.Append("(" + strFIDName + " = " + pRow.get_Value(intFIDIdx).ToString() + ") Or ");

                    pRow = pCursor.NextRow();
                }

                //Brushing on ArcView
                if (plotCommmand.Length > 3)
                {
                    plotCommmand.Remove(plotCommmand.Length - 3, 3);
                    whereClause = plotCommmand.ToString();

                }
                else
                    return;

                int intMapCtrlCnts = pfrmCCMapResults.intHorCnt * pfrmCCMapResults.intVerCnt;

                for (int i = 0; i < intMapCtrlCnts; i++)
                {
                    string strName = "axMapControl" + i.ToString();
                    //AxMapControl MapCntrl = this.m_axMapControls[i].Object;
                    IMapControl3 pMapCntrl = (IMapControl3)((AxMapControl)pfrmCCMapResults.pnMapCntrls.Controls[strName]).Object;
                    IFeatureLayer pSelLayer = pMapCntrl.get_Layer(0) as IFeatureLayer;
                    FeatureSelectionOnActiveView(whereClause, pMapCntrl.ActiveView, pSelLayer);
                }


            }
            pfrmCCMapResults.blnBrushingFromMapControl = false;
        }
        private void BrushingToQQPlot(frmQQPlotResults pfrmPointsPlot, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            try
            {
                if (pfrmPointsPlot.m_pFLayer == pFLayer)
                {
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                    int intVar1Idx = pFeatureClass.Fields.FindField(pfrmPointsPlot.strVar1Name);
                    string strVar1Name = pfrmPointsPlot.strVar1Name;

                    ICursor pCursor = null;
                    featureSelection.SelectionSet.Search(null, false, out pCursor);

                    IRow pRow = pCursor.NextRow();

                    int dblOriPtsSize = pfrmPointsPlot.pChart.Series[0].MarkerSize;

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

                    var checkDup = pfrmPointsPlot.pChart.Series.FindByName("SelPoints");
                    if (checkDup != null)
                        pfrmPointsPlot.pChart.Series.RemoveAt(2);

                    pfrmPointsPlot.pChart.Series.Add(seriesPts);
                    int intNfeature = featureSelection.SelectionSet.Count;
                    //int i = 0;
                    Double[] adblVar1 = new Double[intNfeature];

                    while (pRow != null)
                    {
                        for (int j = 0; j < pfrmPointsPlot.adblVar1.Length; j++)
                        {
                            if (pfrmPointsPlot.adblVar1[j] == Convert.ToDouble(pRow.get_Value(intVar1Idx)))
                                seriesPts.Points.AddXY(pfrmPointsPlot.adblVar2[j], pfrmPointsPlot.adblVar1[j]);
                        }
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        private void BrushingToHistogram(frmHistResults pfrmHistResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            try
            {
                if (pfrmHistResults.pFLayer == pFLayer)
                {
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                    int intVarIdx = pFeatureClass.FindField(pfrmHistResults.strFieldName);

                    ICursor pCursor = null;
                    featureSelection.SelectionSet.Search(null, false, out pCursor);
                    IRow pRow = pCursor.NextRow();

                    var series2 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "SelSeries",
                        Color = System.Drawing.Color.Red,
                        BorderColor = System.Drawing.Color.Black,
                        BackHatchStyle = ChartHatchStyle.DiagonalCross,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.StackedColumn,
                    };

                    var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Series1",
                        Color = System.Drawing.Color.White,
                        BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.StackedColumn,

                    };
                    var checkDup1 = pfrmHistResults.pChart.Series.FindByName("Series1");
                    if (checkDup1 != null)
                    {
                        if (pfrmHistResults.pChart.Series.Count == 1)
                            pfrmHistResults.pChart.Series.RemoveAt(0);
                        else
                            pfrmHistResults.pChart.Series.RemoveAt(1);
                    }

                    var checkDup2 = pfrmHistResults.pChart.Series.FindByName("SelSeries");
                    if (checkDup2 != null)
                        pfrmHistResults.pChart.Series.RemoveAt(0);

                    pfrmHistResults.pChart.Series.Add(series2);
                    pfrmHistResults.pChart.Series.Add(series1);

                    int intNBins = pfrmHistResults.intNBins;
                    int[] intFrequencies = new int[intNBins];
                    Double[] vecMids = pfrmHistResults.vecMids;
                    Double[] dblBreaks = pfrmHistResults.dblBreaks;
                    Double[] vecCounts = pfrmHistResults.vecCounts;

                    while (pRow != null)
                    {
                        double dblValue = Convert.ToDouble(pRow.get_Value(intVarIdx));
                        for (int j = 0; j < intNBins; j++)
                        {
                            if (dblValue > dblBreaks[j] && dblValue <= dblBreaks[j + 1])
                            {
                                intFrequencies[j] = intFrequencies[j] + 1;
                            }
                        }
                        pRow = pCursor.NextRow();
                    }

                    for (int j = 0; j < intNBins; j++)
                    {
                        series1.Points.AddXY(vecMids[j], vecCounts[j] - intFrequencies[j]);
                        series2.Points.AddXY(vecMids[j], intFrequencies[j]);
                    }

                    pfrmHistResults.pChart.Series[1]["PointWidth"] = "1";
                    pfrmHistResults.pChart.Series[0]["PointWidth"] = "1";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        private void BrushingToScatterPlot(frmScatterPlotResults pfrmPointsPlot, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            try
            {
                if (pfrmPointsPlot.m_pFLayer == pFLayer)
                {
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                    int intVar1Idx = pFeatureClass.Fields.FindField(pfrmPointsPlot.strVar1Name);
                    int intVar2Idx = pFeatureClass.Fields.FindField(pfrmPointsPlot.strVar2Name);

                    ICursor pCursor = null;
                    featureSelection.SelectionSet.Search(null, false, out pCursor);

                    IRow pRow = pCursor.NextRow();
                    //IFeature pFeature = pFCursor.NextFeature();

                    int dblOriPtsSize = pfrmPointsPlot.pChart.Series[0].MarkerSize;

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

                    var checkDup = pfrmPointsPlot.pChart.Series.FindByName("SelPoints");
                    if (checkDup != null)
                        pfrmPointsPlot.pChart.Series.RemoveAt(2);

                    pfrmPointsPlot.pChart.Series.Add(seriesPts);

                    while (pRow != null)
                    {
                        //Add Pts
                        seriesPts.Points.AddXY(pRow.get_Value(intVar1Idx), pRow.get_Value(intVar2Idx));
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        private void BrushingToMScatterPlot(frmMScatterResults pfrmMScatterResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            try
            {
                if (pfrmMScatterResults.m_pFLayer == pFLayer)
                {
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                    //int intVarIdx = pFeatureClass.Fields.FindField(pfrmMScatterResults.strVarNM);
                    int intFIDIdx = pFeatureClass.FindField(pFeatureClass.OIDFieldName);

                    ICursor pCursor = null;
                    featureSelection.SelectionSet.Search(null, false, out pCursor);

                    IRow pRow = pCursor.NextRow();
                    //IFeature pFeature = pFCursor.NextFeature();

                    int dblOriPtsSize = pfrmMScatterResults.pChart.Series[0].MarkerSize;

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

                    //Clear previous selection
                    int intLastSeriesIdx = pfrmMScatterResults.pChart.Series.Count - 1;

                    //Remove Previous Selection
                    if (pfrmMScatterResults.pChart.Series[intLastSeriesIdx].Name == "SelPoints")
                        pfrmMScatterResults.pChart.Series.RemoveAt(intLastSeriesIdx);


                    pfrmMScatterResults.pChart.Series.Add(seriesPts);

                    while (pRow != null)
                    {
                        //double dblSelVar = Convert.ToDouble(pRow.get_Value(intVarIdx));
                        //for (int i = 0; i < pfrmMScatterResults.pChart.Series[0].Points.Count; i++)
                        //{
                        //    if (dblSelVar == pfrmMScatterResults.pChart.Series[0].Points[i].XValue)
                        //    {
                        //        double dblWeightVar = pfrmMScatterResults.pChart.Series[0].Points[i].YValues[0];
                        //        seriesPts.Points.AddXY(dblSelVar, dblWeightVar);
                        //    }
                        //}

                        int intSelFID = Convert.ToInt32(pRow.get_Value(intFIDIdx));
                        for (int i = 0; i < pfrmMScatterResults.arrFID.Length; i++)
                        {
                            if (intSelFID == pfrmMScatterResults.arrFID[i])
                            {
                                seriesPts.Points.AddXY(pfrmMScatterResults.pChart.Series[0].Points[i].XValue, pfrmMScatterResults.pChart.Series[0].Points[i].YValues[0]);
                            }
                        }
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        //private void BrushingToOptimizeGraph(frmClassificationGraph pfrmClassGraph, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmClassGraph.m_pFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            int intVar1Idx = pFeatureClass.Fields.FindField(pfrmClassGraph.strValueFldName);

        //            int intLastSeriesIdx = pfrmClassGraph.pChart.Series.Count - 1;

        //            //Remove Previous Selection
        //            if (pfrmClassGraph.pChart.Series[intLastSeriesIdx].Name == "SelSeries")
        //                pfrmClassGraph.pChart.Series.RemoveAt(intLastSeriesIdx);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);
        //            int selCnties = featureSelection.SelectionSet.Count;

        //            IRow pRow = pCursor.NextRow();
        //            //IFeature pFeature = pFCursor.NextFeature();

        //            //int dblOriPtsSize = pfrmClassGraph.pChart.Series[0].MarkerSize;

        //            //System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        //            var Selseries = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelSeries",
        //                Color = System.Drawing.Color.Red,
        //                BorderColor = System.Drawing.Color.Black,
        //                BackHatchStyle = ChartHatchStyle.DiagonalCross,
        //                IsVisibleInLegend = false,
        //                ChartType = SeriesChartType.Column,
        //            };


        //            pfrmClassGraph.pChart.Series.Add(Selseries);

        //            while (pRow != null)
        //            {
        //                double dblEstValue = Convert.ToDouble(pRow.get_Value(intVar1Idx));
        //                for (int i = 0; i < pfrmClassGraph.pChart.Series[0].Points.Count; i++)
        //                {
        //                    double dblYValue = pfrmClassGraph.pChart.Series[0].Points[i].YValues[0] + pfrmClassGraph.pChart.Series[1].Points[i].YValues[0];
        //                    if (dblEstValue == dblYValue)
        //                    {
        //                        double dblSelYValue = pfrmClassGraph.pChart.Series[0].Points[i].YValues[0] + pfrmClassGraph.pChart.Series[1].Points[i].YValues[0] + pfrmClassGraph.pChart.Series[2].Points[i].YValues[0];
        //                        Selseries.Points.AddXY(pfrmClassGraph.pChart.Series[0].Points[i].XValue, dblSelYValue);
        //                    }
        //                }
        //                pRow = pCursor.NextRow();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return;
        //    }
        //}
        //private void BrushingToClassSepGraph(frmCSDesign pfrmCSDesign, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmCSDesign.pFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            int intVar1Idx = pFeatureClass.Fields.FindField(pfrmCSDesign.strValueFldName);

        //            int intLastSeriesIdx = pfrmCSDesign.pChart.Series.Count - 1;

        //            //Remove Previous Selection
        //            if (pfrmCSDesign.pChart.Series[intLastSeriesIdx].Name == "SelSeries")
        //                pfrmCSDesign.pChart.Series.RemoveAt(intLastSeriesIdx);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);
        //            int selCnties = featureSelection.SelectionSet.Count;

        //            IRow pRow = pCursor.NextRow();
        //            //IFeature pFeature = pFCursor.NextFeature();

        //            //int dblOriPtsSize = pfrmClassGraph.pChart.Series[0].MarkerSize;

        //            //System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        //            var Selseries = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelSeries",
        //                Color = System.Drawing.Color.Red,
        //                BorderColor = System.Drawing.Color.Black,
        //                BackHatchStyle = ChartHatchStyle.DiagonalCross,
        //                IsVisibleInLegend = false,
        //                ChartType = SeriesChartType.Column,
        //            };


        //            pfrmCSDesign.pChart.Series.Add(Selseries);

        //            while (pRow != null)
        //            {
        //                double dblEstValue = Convert.ToDouble(pRow.get_Value(intVar1Idx));
        //                for (int i = 0; i < pfrmCSDesign.pChart.Series[0].Points.Count; i++)
        //                {
        //                    double dblYValue = pfrmCSDesign.pChart.Series[0].Points[i].YValues[0] + pfrmCSDesign.pChart.Series[1].Points[i].YValues[0];
        //                    if (dblEstValue == dblYValue)
        //                    {
        //                        double dblSelYValue = pfrmCSDesign.pChart.Series[0].Points[i].YValues[0] + pfrmCSDesign.pChart.Series[1].Points[i].YValues[0] + pfrmCSDesign.pChart.Series[2].Points[i].YValues[0];
        //                        Selseries.Points.AddXY(pfrmCSDesign.pChart.Series[0].Points[i].XValue, dblSelYValue);
        //                    }
        //                }
        //                pRow = pCursor.NextRow();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return;
        //    }
        //}

        private void BrushingToAttTable(frmAttributeTable pfrmAttTable, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            try
            {
                if (pfrmAttTable.dgvAttTable.SelectedRows.Count > 0)
                    pfrmAttTable.dgvAttTable.ClearSelection();
                IFeatureLayer pFormFLayer = pfrmAttTable.m_pLayer as IFeatureLayer;
                if (pFormFLayer == pFLayer)
                {
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;

                    int intFIDIdx = pFeatureClass.FindField(pFeatureClass.OIDFieldName);

                    ICursor pCursor = null;
                    featureSelection.SelectionSet.Search(null, false, out pCursor);

                    IRow pRow = pCursor.NextRow();

                    int intNfeature = featureSelection.SelectionSet.Count;
                    int i = 0;
                    int[] intFIDs = new int[intNfeature];
                    while (pRow != null)
                    {
                        intFIDs[i] = Convert.ToInt32(pRow.get_Value(intFIDIdx));
                        pfrmAttTable.dgvAttTable.Rows[intFIDs[i]].Selected = true;
                        i++;
                        pRow = pCursor.NextRow();
                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        private void BrushingToBoxPlot(frmBoxPlotResults pfrmBoxPlotResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        {
            //try
            //{
            if (pfrmBoxPlotResults.pFLayer == pFLayer)
            {
                //Remove Previous Selection
                int intLastSeriesIdx = pfrmBoxPlotResults.pChart.Series.Count - 1;

                if (pfrmBoxPlotResults.pChart.Series[intLastSeriesIdx].Name == "SelPoints")
                    pfrmBoxPlotResults.pChart.Series.RemoveAt(intLastSeriesIdx);

                IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                int intVarIdx = pFeatureClass.Fields.FindField(pfrmBoxPlotResults.strFieldName);
                int intGrpIdx = pFeatureClass.Fields.FindField(pfrmBoxPlotResults.strGroupFieldName);

                ICursor pCursor = null;
                featureSelection.SelectionSet.Search(null, false, out pCursor);

                IRow pRow = pCursor.NextRow();

                intLastSeriesIdx = pfrmBoxPlotResults.pChart.Series.Count - 1;

                int dblOriPtsSize = pfrmBoxPlotResults.pChart.Series[intLastSeriesIdx].MarkerSize;

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

                pfrmBoxPlotResults.pChart.Series.Add(seriesPts);

                int intNGroups = pfrmBoxPlotResults.uvList.Count;

                while (pRow != null)
                {
                    string strGrp = pRow.get_Value(intGrpIdx).ToString();

                    for (int k = 0; k < intNGroups; k++)
                    {
                        if (strGrp == pfrmBoxPlotResults.uvList[k].ToString())
                        {
                            int intXvalue = k * 2 + 1;
                            seriesPts.Points.AddXY(intXvalue, Convert.ToDouble(pRow.get_Value(intVarIdx)));
                        }
                    }
                    pRow = pCursor.NextRow();
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Exception:" + ex.Message);
            //    return;
            //}
        }

        public void FeatureSelectionOnActiveView(string whereClause, IActiveView pActiveView, IFeatureLayer pFLayer)
        {
            try
            {
                //pActiveView.GraphicsContainer.DeleteAllElements();
                ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast

                // Set up the query
                ESRI.ArcGIS.Geodatabase.IQueryFilter queryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
                queryFilter.WhereClause = whereClause;

                // Invalidate only the selection cache. Flag the original selection
                pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                // Perform the selection
                featureSelection.SelectFeatures(queryFilter, ESRI.ArcGIS.Carto.esriSelectionResultEnum.esriSelectionResultNew, false);

                // Flag the new selection
                //pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                pActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
    }
}
