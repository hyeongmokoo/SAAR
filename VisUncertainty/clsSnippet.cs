using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using RDotNet;
using RDotNet.NativeLibrary;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;

namespace VisUncertainty
{
    public class clsSnippet
    {
        public System.Int32 GetIndexNumberFromLayerName(ESRI.ArcGIS.Carto.IActiveView activeView, System.String layerName)
        {
            try
            {
                if (activeView == null || layerName == null)
                {
                    return -1;
                }
                ESRI.ArcGIS.Carto.IMap map = activeView.FocusMap;

                // Get the number of layers
                int numberOfLayers = map.LayerCount;

                // Loop through the layers and get the correct layer index
                for (System.Int32 i = 0; i < numberOfLayers; i++)
                {
                    if (layerName == map.get_Layer(i).Name)
                    {

                        // Layer was found
                        return i;
                    }
                }

                // No layer was found
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return -1;
            }

        }

        public void drawCurrentChart(System.Collections.Generic.List<string> multipageImage, int currIndex, frmPlot mPlot)
        {
            try
            {
                if (multipageImage.Count > 0)
                {
                    using (System.IO.StreamReader str = new System.IO.StreamReader(multipageImage[currIndex]))
                    {
                        //mPlot.picPlot.Image = new System.Drawing.Bitmap(str.BaseStream);
                        mPlot.picPlot.Image = new System.Drawing.Imaging.Metafile(str.BaseStream);
                        str.Close();
                    }
                    mPlot.picPlot.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }

        }
        public void enableButtons(System.Collections.Generic.List<string> multipageImage, int intCurrentIdx, frmPlot pfrmPlot)
        {
            try
            {
                if (intCurrentIdx > 0)
                    pfrmPlot.btnPreviousPlot.Enabled = true;
                else
                    pfrmPlot.btnPreviousPlot.Enabled = false;

                if (intCurrentIdx >= multipageImage.Count - 1)
                    pfrmPlot.btnNextPlot.Enabled = false;
                else
                    pfrmPlot.btnNextPlot.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void drawPlottoForm(string strTitle, string strCommand)
        {
            try
            {
                MainForm mForm = Application.OpenForms["MainForm"] as MainForm;
                REngine pEngine = mForm.pEngine;
                //Create Plots in R
                StringBuilder CommandPlot = new StringBuilder();

                //Plots are saved in temporary folders. 
                string path = Path.GetTempPath();
                //Have to assing pathes differently at R and ArcObject
                string pathr = path.Replace(@"\", @"/");
                //Remove existing image file pathes
                if (mForm.multipageImage == null)
                    mForm.multipageImage = new List<string>();
                else
                    mForm.multipageImage.Clear();

                //Delete existing image files
                mForm.multipageImage.AddRange(Directory.GetFiles(path, "rnet*.wmf"));

                for (int j = 0; j < mForm.multipageImage.Count; j++)
                {
                    FileInfo pinfo = new FileInfo(mForm.multipageImage[j]);
                    if (pinfo.Exists)
                        pinfo.Delete();
                    pinfo.Refresh();
                }

                //Load Form and assign the settings
                frmPlot pfrmPlot = new frmPlot();
                pfrmPlot.Text = strTitle;
                pfrmPlot.Show();
                string strwidth = pfrmPlot.picPlot.Size.Width.ToString();
                string strHeight = pfrmPlot.picPlot.Size.Height.ToString();

                //Create Plots in R
                CommandPlot.Append("win.metafile('" + pathr + "rnet%01d.wmf');");
                CommandPlot.Append(strCommand);
                CommandPlot.Append("graphics.off()");
                pEngine.Evaluate(CommandPlot.ToString());

                //Add Plot pathes at List
                mForm.multipageImage.Clear();
                mForm.multipageImage.AddRange(Directory.GetFiles(path, "rnet*.wmf"));

                //Draw plots at the Form
                mForm.intCurrentIdx = 0;
                drawCurrentChart(mForm.multipageImage, mForm.intCurrentIdx, pfrmPlot);
                enableButtons(mForm.multipageImage, mForm.intCurrentIdx, pfrmPlot);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public string FilePathinRfromLayer(ESRI.ArcGIS.Carto.IFeatureLayer pFLayer)
        {
            try
            {
                ESRI.ArcGIS.Geodatabase.IDataset dataset = (ESRI.ArcGIS.Geodatabase.IDataset)(pFLayer);
                string strfullname = dataset.Workspace.PathName + "\\" + dataset.BrowseName;
                if (dataset.Category == "Shapefile Feature Class")
                    strfullname = strfullname + ".shp";
                string strNameR = strfullname.Replace(@"\", @"/");
                return strNameR;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return null;
            }
        }
        public ESRI.ArcGIS.Display.IRgbColor getRGB(int R, int G, int B)
        {
            try
            {
                IRgbColor pRgbColor = new RgbColorClass();
                pRgbColor.Red = R;
                pRgbColor.Green = G;
                pRgbColor.Blue = B;

                return pRgbColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return null;
            }

        }
        public ESRI.ArcGIS.Geometry.IEnvelope DrawRectangle(ESRI.ArcGIS.Carto.IActiveView activeView)
        {
            try
            {
                if (activeView == null)
                {
                    return null;
                }
                else
                {

                    ESRI.ArcGIS.Display.IScreenDisplay screenDisplay = activeView.ScreenDisplay;

                    // Constant
                    screenDisplay.StartDrawing(screenDisplay.hDC, (System.Int16)ESRI.ArcGIS.Display.esriScreenCache.esriNoScreenCache); // Explicit Cast
                    //ESRI.ArcGIS.Display.IRgbColor rgbColor = new ESRI.ArcGIS.Display.RgbColorClass();
                    //rgbColor.Red = 255;

                    //ESRI.ArcGIS.Display.IColor color = rgbColor; // Implicit Cast
                    ESRI.ArcGIS.Display.ISimpleFillSymbol simpleFillSymbol = new ESRI.ArcGIS.Display.SimpleFillSymbolClass();
                    //simpleFillSymbol.Color = color;
                    simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSHollow;

                    ESRI.ArcGIS.Display.ISymbol symbol = simpleFillSymbol as ESRI.ArcGIS.Display.ISymbol; // Dynamic Cast
                    ESRI.ArcGIS.Display.IRubberBand rubberBand = new ESRI.ArcGIS.Display.RubberEnvelopeClass();
                    ESRI.ArcGIS.Geometry.IGeometry geometry = rubberBand.TrackNew(screenDisplay, symbol);
                    screenDisplay.SetSymbol(symbol);
                    ESRI.ArcGIS.Geometry.IEnvelope pEnvelope = geometry as ESRI.ArcGIS.Geometry.IEnvelope;
                    screenDisplay.DrawRectangle(pEnvelope); // Dynamic Cast
                    screenDisplay.FinishDrawing();

                    return pEnvelope;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return null;
            }

        }
        public void AddLayerToCbo(MainForm mForm, ComboBox cboSourceLayer)
        {
            try
            {
                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void AddFieldsForTwoCbo(string strLayerName, ESRI.ArcGIS.Carto.IActiveView pActiveView, MainForm mForm, ComboBox cboValueField, ComboBox cboUField)
        {
            try
            {
                int intLIndex = GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;


                cboValueField.Items.Clear();
                cboUField.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                    cboUField.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void ClearSelectedMapFeatures(ESRI.ArcGIS.Carto.IActiveView activeView, ESRI.ArcGIS.Carto.IFeatureLayer featureLayer) //dhncho 01.10
        {
            try
            {
                if (activeView == null || featureLayer == null)
                {
                    return;
                }
                ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = featureLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast

                // Invalidate only the selection cache. Flag the original selection
                activeView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                // Clear the selection
                featureSelection.Clear();

                // Flag the new selection
                activeView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        //The function below is not used now. 012516 HK
        //public IFeatureCursor FeatureCursorFromSelection(IFeatureLayer pFLayer, bool blnUseSelected)
        //{
        //    try
        //    {
        //        IFeatureCursor pFCursor = null;
        //        IFeatureSelection pFeatureSelection = pFLayer as IFeatureSelection;
        //        int intNFeatureCount = pFeatureSelection.SelectionSet.Count;
        //        if (intNFeatureCount > 0 && blnUseSelected == true)
        //        {
        //            ICursor pCursor = null;

        //            pFeatureSelection.SelectionSet.Search(null, true, out pCursor);
        //            pFCursor = (IFeatureCursor)pCursor;

        //        }
        //        else if (intNFeatureCount == 0 && blnUseSelected == true)
        //        {
        //            MessageBox.Show("Select at least one feature");

        //        }
        //        else
        //        {
        //            pFCursor = pFLayer.Search(null, true);
        //            intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);
        //        }
        //        return pFCursor;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return null;
        //    }
        //}
        public void AddFieldsForOneCombo(string strLayerName, ESRI.ArcGIS.Carto.IActiveView pActiveView, ComboBox cboValueField)
        {
            try
            {
                int intLIndex = GetIndexNumberFromLayerName(pActiveView, strLayerName);
                if (intLIndex == -1)
                    return;
                ILayer pLayer = pActiveView.FocusMap.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;


                cboValueField.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }

        public bool FindNumberFieldType(IField pField)
        {
            try
            {
                bool blnNField = true;
                if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeOID || pField.Type == esriFieldType.esriFieldTypeString)
                    blnNField = false;

                return blnNField;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return false;
            }
        }
        //The method below is based on R Graphic, Not used in now 012516 HK
        //public void drawHistogram(string strLayerName, string strFieldName, bool blnChkSelected)
        //{
        //    MainForm mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
        //    REngine pEngine = mForm.pEngine;
        //    IActiveView pActiveView = mForm.axMapControl1.ActiveView;

        //    int intLIndex = GetIndexNumberFromLayerName(pActiveView, strLayerName);
        //    int intNFeatureCount = 0;

        //    ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);
        //    IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

        //    IFeatureCursor pFCursor = null;
        //    IFeatureSelection pFeatureSelection = pFLayer as IFeatureSelection;
        //    intNFeatureCount = pFeatureSelection.SelectionSet.Count;
        //    if (intNFeatureCount > 0 && blnChkSelected == true)
        //    {
        //        ICursor pCursor = null;

        //        pFeatureSelection.SelectionSet.Search(null, true, out pCursor);
        //        pFCursor = (IFeatureCursor)pCursor;

        //    }
        //    else if (intNFeatureCount == 0 && blnChkSelected == true)
        //    {
        //        MessageBox.Show("Select at least one feature");
        //        return;
        //    }
        //    else
        //    {
        //        pFCursor = pFLayer.Search(null, true);
        //        intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);
        //    }

        //    IFeature pFeature = pFCursor.NextFeature();

        //    int intFieldIdx = pFLayer.FeatureClass.Fields.FindField(strFieldName);

        //    double[] arrValue = new double[intNFeatureCount];

        //    int i = 0;
        //    while (pFeature != null)
        //    {

        //        arrValue[i] = Convert.ToDouble(pFeature.get_Value(intFieldIdx));
        //        i++;
        //        pFeature = pFCursor.NextFeature();
        //    }

        //    NumericVector vecValue = pEngine.CreateNumericVector(arrValue);
        //    pEngine.SetSymbol(strFieldName, vecValue);

        //    string strCommand = "hist(" + strFieldName + ", freq=FALSE);";
        //    string strTitle = "Histogram";

        //    drawPlottoForm(strTitle, strCommand);
        //}
        public void drawHistogram(string strLayerName, string strFieldName)
        {
            try
            {
                MainForm mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                REngine pEngine = mForm.pEngine;
                IActiveView pActiveView = mForm.axMapControl1.ActiveView;

                int intLIndex = GetIndexNumberFromLayerName(pActiveView, strLayerName);
                if (intLIndex == -1)
                    return;

                int intNFeatureCount = 0;

                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

                IFeatureCursor pFCursor = null;
                IFeatureSelection pFeatureSelection = pFLayer as IFeatureSelection;
                intNFeatureCount = pFeatureSelection.SelectionSet.Count;

                pFCursor = pFLayer.Search(null, true);
                intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);

                if (intNFeatureCount == 0)
                    return;

                IFeature pFeature = pFCursor.NextFeature();

                int intFieldIdx = pFLayer.FeatureClass.Fields.FindField(strFieldName);

                double[] arrValue = new double[intNFeatureCount];

                int i = 0;
                while (pFeature != null)
                {

                    arrValue[i] = Convert.ToDouble(pFeature.get_Value(intFieldIdx));
                    i++;
                    pFeature = pFCursor.NextFeature();
                }

                //IDataHistogram pDataHistogram = new DataHistogramClass();

                //pDataHistogram.SetData(arrValue);

                //IHistogram pHistogram = (IHistogram)pDataHistogram;

                //object xVals, frqs;
                //pHistogram.GetHistogram(out xVals, out frqs);

                ////Manually Select bin width: Not used.
                //double dblIQR = GetIQR(arrValue);
                // //Use Freedman–Diaconis' choice to get bin width of histogram
                //double dblBinWidth = 2 * dblIQR * Math.Pow(intNFeatureCount, -1 / 3);
                //double dblMax = arrValue.Max();
                //double dblMin = arrValue.Min();
                //int intBreakCount = Convert.ToInt32(Math.Floor((dblMax - dblMin) / dblBinWidth));
                                
                //IClassifyGEN pClassifyGEN = new EqualIntervalClass();

                //pClassifyGEN.Classify(xVals, frqs, intBreakCount);

                var watch = System.Diagnostics.Stopwatch.StartNew();

                NumericVector vecValue = pEngine.CreateNumericVector(arrValue);
                pEngine.SetSymbol(strFieldName, vecValue);
                pEngine.Evaluate("hist.sample <- hist(" + strFieldName + ", plot = FALSE)");

                frmHistResults pfrmTemp = new frmHistResults();
                pfrmTemp.Text = "Histogram of " + pFLayer.Name;
                pfrmTemp.pChart.Series.Clear();

                Double[] vecMids = pEngine.Evaluate("hist.sample$mids").AsNumeric().ToArray();
                Double[] vecCounts = pEngine.Evaluate("hist.sample$counts").AsNumeric().ToArray();
                Double[] dblBreaks = pEngine.Evaluate("hist.sample$breaks").AsNumeric().ToArray();

                watch.Stop();
                double dblTime = watch.ElapsedMilliseconds;

                var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Series1",
                    Color = System.Drawing.Color.White,
                    BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Column,

                };

                pfrmTemp.pChart.Series.Add(series1);

                int intNBins = vecMids.Length;

                for (int j = 0; j < intNBins; j++)
                {
                    series1.Points.AddXY(vecMids[j], vecCounts[j]);
                }

                //pfrmTemp.pChart.Invalidate();
                pfrmTemp.pChart.Series[0]["PointWidth"] = "1";
                pfrmTemp.pChart.ChartAreas[0].AxisX.Title = strFieldName;
                pfrmTemp.pChart.ChartAreas[0].AxisY.Title = "Frequency";
                //pfrmTemp.pChart.ChartAreas[0].AxisX.IntervalOffset = -0.5;
                
                pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
                //pfrmTemp.pChart.ChartAreas[0].AxisX.IsStartedFromZero = false;
                //pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.IntervalOffset = -0.5;
                //pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.IntervalOffsetType = DateTimeIntervalType.Number;

                //pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.Interval = 2;
                //pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.IntervalType = DateTimeIntervalType.Number;

                pfrmTemp.dblBreaks = dblBreaks;
                pfrmTemp.m_pForm = mForm;
                pfrmTemp.pActiveView = pActiveView;
                pfrmTemp.pFLayer = pFLayer;
                pfrmTemp.strFieldName = strFieldName;
                pfrmTemp.intNBins = intNBins;
                pfrmTemp.vecCounts = vecCounts;
                pfrmTemp.vecMids = vecMids;

                double dblInterval = dblBreaks[1] - dblBreaks[0];
                for (int n = 0; n < dblBreaks.Length; n++)
                {
                    CustomLabel pcutsomLabel = new CustomLabel();
                    //pcutsomLabel.FromPosition = dblBreaks[n] - dblInterval;
                    //pcutsomLabel.ToPosition = dblBreaks[n] + dblInterval;
                    pcutsomLabel.FromPosition = n;
                    pcutsomLabel.ToPosition = n + 1;
                    pcutsomLabel.Text = dblBreaks[n].ToString();

                    pfrmTemp.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
                }

                pfrmTemp.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void DeleteField(IFeatureClass featureClass, string fieldName)
        {
            try
            {
                IFields fields = featureClass.Fields;
                IField field = fields.get_Field(fields.FindField(fieldName));
                featureClass.DeleteField(field);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void MoveSelectedItemsinListBoxtoOtherCheckedListBox(ListBox FromListBox, CheckedListBox ToListBox)
        {
            try
            {
                int intSelectedItems = FromListBox.SelectedItems.Count;
                int intToItemCount = ToListBox.Items.Count;
                if (intSelectedItems > 0)
                {
                    for (int i = 0; i < intSelectedItems; i++)
                    {
                        ToListBox.Items.Add(FromListBox.SelectedItems[0]);
                        FromListBox.Items.Remove(FromListBox.SelectedItems[0]);

                    }
                    FromListBox.ClearSelected();
                }
                else
                    return;
                ToListBox.SetItemChecked(intToItemCount, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void MoveSelectedItemsinCheckedListBoxtoOtherListBox(CheckedListBox FromListBox, ListBox ToListBox)
        {
            try
            {
                int intSelectedItems = FromListBox.SelectedItems.Count;
                if (intSelectedItems > 0)
                {
                    for (int i = 0; i < intSelectedItems; i++)
                    {
                        ToListBox.Items.Add(FromListBox.SelectedItems[0]);
                        FromListBox.Items.Remove(FromListBox.SelectedItems[0]);
                    }
                    FromListBox.ClearSelected();
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

        public void MoveSelectedItemsinListBoxtoOtherListBox(ListBox FromListBox, ListBox ToListBox)
        {
            try
            {
                int intSelectedItems = FromListBox.SelectedItems.Count;
                if (intSelectedItems > 0)
                {
                    for (int i = 0; i < intSelectedItems; i++)
                    {
                        ToListBox.Items.Add(FromListBox.SelectedItems[0]);
                        FromListBox.Items.Remove(FromListBox.SelectedItems[0]);
                    }
                    FromListBox.ClearSelected();
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


        public void LoadingAttributeTable(ILayer pLayer, frmAttributeTable pfrmAttTable)
        {
            try
            {
                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                string strLayerName = pFLayer.Name;
                IAttributeTable pAttributeTable = pFLayer as IAttributeTable;
                ITable pTable = pAttributeTable as ITable;

                IQueryFilter pQFilter = new QueryFilterClass();
                pQFilter = null;

                System.Data.DataTable shpDataTable;
                shpDataTable = new System.Data.DataTable("shpDataTable");

                ICursor pCursor = pTable.Search(pQFilter, true);
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {

                        System.Data.DataColumn column;
                        column = new System.Data.DataColumn();
                        column.ColumnName = pRow.Fields.get_Field(i).Name;
                        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeOID) column.DataType = System.Type.GetType("System.Int32");
                        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeInteger) column.DataType = System.Type.GetType("System.Int32");
                        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeSingle) column.DataType = System.Type.GetType("System.Single");
                        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeDouble) column.DataType = System.Type.GetType("System.Double");
                        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeSmallInteger) column.DataType = System.Type.GetType("System.Int16");
                        shpDataTable.Columns.Add(column);

                    }
                    while (pRow != null)
                    {
                        System.Data.DataRow pDataRow = shpDataTable.NewRow();
                        for (int j = 0; j < pCursor.Fields.FieldCount; j++)
                            pDataRow[j] = pRow.get_Value(j);
                        shpDataTable.Rows.Add(pDataRow);
                        pRow = pCursor.NextRow();
                    }
                }

                pfrmAttTable.dgvAttTable.DataSource = shpDataTable;
                pfrmAttTable.Text = "Attribute of " + strLayerName;

                for (int i = 0; i < shpDataTable.Columns.Count; i++)
                {
                    if (i != 1)
                    {
                        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Int16")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Int32")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Single")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Double")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }

                pfrmAttTable.dgvAttTable.Columns["Shape"].Visible = false;
                pfrmAttTable.dgvAttTable.AllowUserToAddRows = false;
                pfrmAttTable.m_pLayer = pLayer;
                pfrmAttTable.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public frmAttributeTable returnAttTable(IntPtr intHandle)
        {
            try
            {
                FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
                frmAttributeTable pfrmAttributeTable = null;

                for (int j = 0; j < pFormCollection.Count; j++)
                {
                    if (pFormCollection[j].Handle == intHandle)
                    {
                        pfrmAttributeTable = pFormCollection[j] as frmAttributeTable;

                    }
                }
                return pfrmAttributeTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return null;
            }
        }
        //public int RemoveBrushing(MainForm mForm, IFeatureLayer pFLayer)
        //{
        //    try
        //    {
        //        FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
        //        IActiveView pActiveView = mForm.axMapControl1.ActiveView;
        //        int intFormCnt = 0;
        //        if (pFLayer.Visible)
        //        {
        //            //Brushing to Mapcontrol
        //            string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
        //            //IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;


        //            for (int j = 0; j < pFormCollection.Count; j++)
        //            {

        //                if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
        //                {
        //                    frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
        //                    if (pfrmHistResults.pFLayer == pFLayer)
        //                        intFormCnt++;
        //                }
        //                if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
        //                {
        //                    frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
        //                    if (pfrmPointsPlot.pFLayer == pFLayer)
        //                        intFormCnt++;

        //                }
        //                if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
        //                {
        //                    frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
        //                    if (pfrmQQPlotResult.pFLayer == pFLayer)
        //                        intFormCnt++;
        //                }
        //                if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
        //                {
        //                    frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
        //                    if ((IFeatureLayer)pfrmAttTable.pLayer == pFLayer)
        //                        intFormCnt++;
        //                }
        //                if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
        //                {
        //                    frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
        //                    if (pfrmBoxPlotResult.pFLayer == pFLayer)
        //                        intFormCnt++;
        //                }
        //                if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
        //                {
        //                    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
        //                    if (pfrmClassGraph.pFLayer == pFLayer)
        //                        intFormCnt++;
        //                }
        //                if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
        //                {
        //                    frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
        //                    if (pfrmMScatterPlot.pFLayer == pFLayer)
        //                        intFormCnt++;
        //                }


        //            }
        //            return intFormCnt;
        //        }
        //        else
        //            return -1;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return -1;
        //    }
        //}
        //public string RemoveWarningByBrushingTechnique(IFeatureLayer pFLayer)
        //{
        //    //try
        //    //{
        //        FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
        //        //IActiveView pActiveView = mForm.axMapControl1.ActiveView;
        //        string strRelatedPlotName = string.Empty;
        //        for (int j = 0; j < pFormCollection.Count; j++)
        //        {
        //            if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
        //            {
        //                frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
        //                if (pfrmHistResults.pFLayer == pFLayer)
        //                    strRelatedPlotName += "Histogram, ";
        //            }
        //            if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
        //            {
        //                frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
        //                if (pfrmPointsPlot.pFLayer == pFLayer)
        //                    strRelatedPlotName += "Scatter plot, ";

        //            }
        //            if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
        //            {
        //                frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
        //                if (pfrmQQPlotResult.pFLayer == pFLayer)
        //                    strRelatedPlotName += "QQ-plot, ";
        //            }
        //            //if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
        //            //{
        //            //    frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
        //            //    BrushingToAttTable(pfrmAttTable, pFLayer, featureSelection);
        //            //}
        //            if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
        //            {
        //                frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
        //                if (pfrmBoxPlotResult.pFLayer == pFLayer)
        //                    strRelatedPlotName += "Boxplot, ";
        //            }
        //            //if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
        //            //{
        //            //    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
        //            //    BrushingToOptimizeGraph(pfrmClassGraph, pFLayer, featureSelection);
        //            //}
        //            //if (pFormCollection[j].Name == "frmCSDesign")//Brushing to CS Graph
        //            //{
        //            //    frmCSDesign pfrmCSDesign = pFormCollection[j] as frmCSDesign;
        //            //    if (pfrmCSDesign.pFLayer == pFLayer)
        //            //        strRelatedPlotName += "Uncertainty Classification, ";
        //            //}
        //            if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
        //            {
        //                frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
        //                if (pfrmMScatterPlot.pFLayer == pFLayer)
        //                    strRelatedPlotName += "Moran Scatter Plot, ";
        //            }

        //        }
        //    if(strRelatedPlotName != string.Empty)
        //        strRelatedPlotName = strRelatedPlotName.Remove(strRelatedPlotName.Length - 2);
        //        return strRelatedPlotName;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Exception:" + ex.Message);
        //    //    return;
        //    //}
        //}
        //public void CloseAllRelatedPlots(IFeatureLayer pFLayer)
        //{
        //    //try
        //    //{
        //    FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
        //    List<int> lstPlotIdx = new List<int>();

        //    for (int j = 0; j < pFormCollection.Count; j++)
        //    {
        //        if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
        //        {
        //            frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
        //            if (pfrmHistResults.pFLayer == pFLayer)
        //                lstPlotIdx.Add(j);
        //        }
        //        if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
        //        {
        //            frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
        //            if (pfrmPointsPlot.pFLayer == pFLayer)
        //                lstPlotIdx.Add(j);

        //        }
        //        if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
        //        {
        //            frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
        //            if (pfrmQQPlotResult.pFLayer == pFLayer)
        //                lstPlotIdx.Add(j);
        //        }
        //        if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
        //        {
        //            frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
        //            if (pfrmAttTable.pLayer == (ILayer)pFLayer)
        //                lstPlotIdx.Add(j);
        //        }
        //        if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
        //        {
        //            frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
        //            if (pfrmBoxPlotResult.pFLayer == pFLayer)
        //                lstPlotIdx.Add(j);
        //        }
        //        //if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
        //        //{
        //        //    frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
        //        //    BrushingToOptimizeGraph(pfrmClassGraph, pFLayer, featureSelection);
        //        //}
        //        //if (pFormCollection[j].Name == "frmCSDesign")//Brushing to CS Graph
        //        //{
        //        //    frmCSDesign pfrmCSDesign = pFormCollection[j] as frmCSDesign;
        //        //    if (pfrmCSDesign.pFLayer == pFLayer)
        //        //        lstPlotIdx.Add(j);
        //        //}
        //        if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
        //        {
        //            frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
        //            if (pfrmMScatterPlot.pFLayer == pFLayer)
        //                lstPlotIdx.Add(j);
        //        }

        //    }

        //    lstPlotIdx.Sort();
        //    for (int j = lstPlotIdx.Count - 1; j >= 0; j--)
        //    {
        //        pFormCollection[lstPlotIdx[j]].Close();
        //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Exception:" + ex.Message);
        //    //    return;
        //    //}
        //}
        //public void BrushingToOthers(IFeatureLayer pFLayer, IntPtr intPtrParent)
        //{
        //    try
        //    {
        //        FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
        //        //IActiveView pActiveView = mForm.axMapControl1.ActiveView;

        //        if (pFLayer.Visible)
        //        {
        //            //Brushing to Mapcontrol
        //            string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
        //            IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;


        //            for (int j = 0; j < pFormCollection.Count; j++)
        //            {
        //                if (pFormCollection[j].Handle != intPtrParent) // Brushing to Others
        //                {

        //                    if (pFormCollection[j].Name == "frmHistResults")//Brushing to Histogram
        //                    {
        //                        frmHistResults pfrmHistResults = pFormCollection[j] as frmHistResults;
        //                        BrushingToHistogram(pfrmHistResults, pFLayer, featureSelection);
        //                    }
        //                    if (pFormCollection[j].Name == "frmScatterPlotResults") //Brushing to Scatterplot
        //                    {
        //                        frmScatterPlotResults pfrmPointsPlot = pFormCollection[j] as frmScatterPlotResults;
        //                        BrushingToScatterPlot(pfrmPointsPlot, pFLayer, featureSelection);

        //                    }
        //                    if (pFormCollection[j].Name == "frmQQPlotResults") //Brushing to QQPlot
        //                    {
        //                        frmQQPlotResults pfrmQQPlotResult = pFormCollection[j] as frmQQPlotResults;
        //                        BrushingToQQPlot(pfrmQQPlotResult, pFLayer, featureSelection);
        //                    }
        //                    if (pFormCollection[j].Name == "frmAttributeTable")//Brushing to AttributeTable
        //                    {
        //                        frmAttributeTable pfrmAttTable = pFormCollection[j] as frmAttributeTable;
        //                        BrushingToAttTable(pfrmAttTable, pFLayer, featureSelection);
        //                    }
        //                    if (pFormCollection[j].Name == "frmBoxPlotResults")//Brushing to AttributeTable
        //                    {
        //                        frmBoxPlotResults pfrmBoxPlotResult = pFormCollection[j] as frmBoxPlotResults;
        //                        BrushingToBoxPlot(pfrmBoxPlotResult, pFLayer, featureSelection);
        //                    }
        //                    if (pFormCollection[j].Name == "frmClassificationGraph")//Brushing to Optiize Graph
        //                    {
        //                        frmClassificationGraph pfrmClassGraph = pFormCollection[j] as frmClassificationGraph;
        //                        BrushingToOptimizeGraph(pfrmClassGraph, pFLayer, featureSelection);
        //                    }
        //                    //if (pFormCollection[j].Name == "frmCSDesign")//Brushing to CS Graph
        //                    //{
        //                    //    frmCSDesign pfrmCSDesign = pFormCollection[j] as frmCSDesign;
        //                    //    BrushingToClassSepGraph(pfrmCSDesign, pFLayer, featureSelection);
        //                    //}
        //                    if (pFormCollection[j].Name == "frmMScatterResults")//Brushing to Moran ScatterPlot
        //                    {
        //                        frmMScatterResults pfrmMScatterPlot = pFormCollection[j] as frmMScatterResults;
        //                        BrushingToMScatterPlot(pfrmMScatterPlot, pFLayer, featureSelection);
        //                    }
        //                    if (pFormCollection[j].Name == "frmCCMapsResults")//Brushing to Moran ScatterPlot
        //                    {
        //                        frmCCMapsResults pfrmCCMApsResults = pFormCollection[j] as frmCCMapsResults;
        //                        BrushingToCCMaps(pfrmCCMApsResults, pFLayer, featureSelection);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return;
        //    }
        //}
        //private void BrushingToCCMaps(frmCCMapsResults pfrmCCMapResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    if (pfrmCCMapResults.pFLayer == pFLayer)
        //    {
               

        //        pfrmCCMapResults.blnBrushingFromMapControl = true;

        //        IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                
        //        string strFIDName = pFeatureClass.OIDFieldName;
        //        int intFIDIdx = pFeatureClass.FindField(strFIDName);

        //        ICursor pCursor = null;
        //        featureSelection.SelectionSet.Search(null, false, out pCursor);

        //        IRow pRow = pCursor.NextRow();
        //        StringBuilder plotCommmand = new StringBuilder();

        //        string whereClause = null;
        //        while (pRow != null)
        //        {
        //            plotCommmand.Append("(" + strFIDName + " = " + pRow.get_Value(intFIDIdx).ToString() + ") Or ");

        //            pRow = pCursor.NextRow();
        //        }

        //        //Brushing on ArcView
        //        if (plotCommmand.Length > 3)
        //        {
        //            plotCommmand.Remove(plotCommmand.Length - 3, 3);
        //            whereClause = plotCommmand.ToString();
                    
        //        }

        //        int intMapCtrlCnts = pfrmCCMapResults.intHorCnt * pfrmCCMapResults.intVerCnt;

        //        for (int i = 0; i < intMapCtrlCnts; i++)
        //        {
        //            string strName = "axMapControl" + i.ToString();
        //            //AxMapControl MapCntrl = this.m_axMapControls[i].Object;
        //            IMapControl3 pMapCntrl = (IMapControl3)((AxMapControl)pfrmCCMapResults.pnMapCntrls.Controls[strName]).Object;
        //            IFeatureLayer pSelLayer = pMapCntrl.get_Layer(0) as IFeatureLayer;
        //            FeatureSelectionOnActiveView(whereClause, pMapCntrl.ActiveView, pSelLayer);
        //        }


        //    }
        //    pfrmCCMapResults.blnBrushingFromMapControl = false;
        //}
        //private void BrushingToQQPlot(frmQQPlotResults pfrmPointsPlot, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmPointsPlot.pFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            int intVar1Idx = pFeatureClass.Fields.FindField(pfrmPointsPlot.strVar1Name);
        //            string strVar1Name = pfrmPointsPlot.strVar1Name;

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);

        //            IRow pRow = pCursor.NextRow();

        //            int dblOriPtsSize = pfrmPointsPlot.pChart.Series[0].MarkerSize;

        //            System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        //            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelPoints",
        //                Color = pMarkerColor,
        //                BorderColor = pMarkerColor,
        //                IsVisibleInLegend = false,
        //                IsXValueIndexed = false,
        //                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
        //                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
        //                MarkerSize = dblOriPtsSize * 2

        //            };

        //            var checkDup = pfrmPointsPlot.pChart.Series.FindByName("SelPoints");
        //            if (checkDup != null)
        //                pfrmPointsPlot.pChart.Series.RemoveAt(2);

        //            pfrmPointsPlot.pChart.Series.Add(seriesPts);
        //            int intNfeature = featureSelection.SelectionSet.Count;
        //            //int i = 0;
        //            Double[] adblVar1 = new Double[intNfeature];

        //            while (pRow != null)
        //            {
        //                for (int j = 0; j < pfrmPointsPlot.adblVar1.Length; j++)
        //                {
        //                    if (pfrmPointsPlot.adblVar1[j] == Convert.ToDouble(pRow.get_Value(intVar1Idx)))
        //                        seriesPts.Points.AddXY(pfrmPointsPlot.adblVar2[j], pfrmPointsPlot.adblVar1[j]);
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
        //private void BrushingToHistogram(frmHistResults pfrmHistResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmHistResults.pFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            int intVarIdx = pFeatureClass.FindField(pfrmHistResults.strFieldName);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);
        //            IRow pRow = pCursor.NextRow();

        //            var series2 = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelSeries",
        //                Color = System.Drawing.Color.Red,
        //                BorderColor = System.Drawing.Color.Black,
        //                BackHatchStyle = ChartHatchStyle.DiagonalCross,
        //                IsVisibleInLegend = false,
        //                IsXValueIndexed = true,
        //                ChartType = SeriesChartType.StackedColumn,
        //            };

        //            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "Series1",
        //                Color = System.Drawing.Color.White,
        //                BorderColor = System.Drawing.Color.Black,
        //                IsVisibleInLegend = false,
        //                IsXValueIndexed = true,
        //                ChartType = SeriesChartType.StackedColumn,

        //            };
        //            var checkDup1 = pfrmHistResults.pChart.Series.FindByName("Series1");
        //            if (checkDup1 != null)
        //            {
        //                if (pfrmHistResults.pChart.Series.Count == 1)
        //                    pfrmHistResults.pChart.Series.RemoveAt(0);
        //                else
        //                    pfrmHistResults.pChart.Series.RemoveAt(1);
        //            }

        //            var checkDup2 = pfrmHistResults.pChart.Series.FindByName("SelSeries");
        //            if (checkDup2 != null)
        //                pfrmHistResults.pChart.Series.RemoveAt(0);

        //            pfrmHistResults.pChart.Series.Add(series2);
        //            pfrmHistResults.pChart.Series.Add(series1);

        //            int intNBins = pfrmHistResults.intNBins;
        //            int[] intFrequencies = new int[intNBins];
        //            Double[] vecMids = pfrmHistResults.vecMids;
        //            Double[] dblBreaks = pfrmHistResults.dblBreaks;
        //            Double[] vecCounts = pfrmHistResults.vecCounts;

        //            while (pRow != null)
        //            {
        //                double dblValue = Convert.ToDouble(pRow.get_Value(intVarIdx));
        //                for (int j = 0; j < intNBins; j++)
        //                {
        //                    if (dblValue > dblBreaks[j] && dblValue <= dblBreaks[j + 1])
        //                    {
        //                        intFrequencies[j] = intFrequencies[j] + 1;
        //                    }
        //                }
        //                pRow = pCursor.NextRow();
        //            }

        //            for (int j = 0; j < intNBins; j++)
        //            {
        //                series1.Points.AddXY(vecMids[j], vecCounts[j] - intFrequencies[j]);
        //                series2.Points.AddXY(vecMids[j], intFrequencies[j]);
        //            }

        //            pfrmHistResults.pChart.Series[1]["PointWidth"] = "1";
        //            pfrmHistResults.pChart.Series[0]["PointWidth"] = "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return;
        //    }
        //}
        //private void BrushingToScatterPlot(frmScatterPlotResults pfrmPointsPlot, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmPointsPlot.pFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            int intVar1Idx = pFeatureClass.Fields.FindField(pfrmPointsPlot.strVar1Name);
        //            int intVar2Idx = pFeatureClass.Fields.FindField(pfrmPointsPlot.strVar2Name);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);

        //            IRow pRow = pCursor.NextRow();
        //            //IFeature pFeature = pFCursor.NextFeature();

        //            int dblOriPtsSize = pfrmPointsPlot.pChart.Series[0].MarkerSize;

        //            System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        //            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelPoints",
        //                Color = pMarkerColor,
        //                BorderColor = pMarkerColor,
        //                IsVisibleInLegend = false,
        //                IsXValueIndexed = false,
        //                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
        //                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
        //                MarkerSize = dblOriPtsSize * 2

        //            };

        //            var checkDup = pfrmPointsPlot.pChart.Series.FindByName("SelPoints");
        //            if (checkDup != null)
        //                pfrmPointsPlot.pChart.Series.RemoveAt(2);

        //            pfrmPointsPlot.pChart.Series.Add(seriesPts);

        //            while (pRow != null)
        //            {
        //                //Add Pts
        //                seriesPts.Points.AddXY(pRow.get_Value(intVar1Idx), pRow.get_Value(intVar2Idx));
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
        //private void BrushingToMScatterPlot(frmMScatterResults pfrmMScatterResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmMScatterResults.pFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            //int intVarIdx = pFeatureClass.Fields.FindField(pfrmMScatterResults.strVarNM);
        //            int intFIDIdx = pFeatureClass.FindField(pFeatureClass.OIDFieldName);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);

        //            IRow pRow = pCursor.NextRow();
        //            //IFeature pFeature = pFCursor.NextFeature();

        //            int dblOriPtsSize = pfrmMScatterResults.pChart.Series[0].MarkerSize;

        //            System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        //            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelPoints",
        //                Color = pMarkerColor,
        //                BorderColor = pMarkerColor,
        //                IsVisibleInLegend = false,
        //                IsXValueIndexed = false,
        //                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
        //                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
        //                MarkerSize = dblOriPtsSize * 2

        //            };

        //            //Clear previous selection
        //            int intLastSeriesIdx = pfrmMScatterResults.pChart.Series.Count - 1;

        //            //Remove Previous Selection
        //            if (pfrmMScatterResults.pChart.Series[intLastSeriesIdx].Name == "SelPoints")
        //                pfrmMScatterResults.pChart.Series.RemoveAt(intLastSeriesIdx);


        //            pfrmMScatterResults.pChart.Series.Add(seriesPts);

        //            while (pRow != null)
        //            {
        //                //double dblSelVar = Convert.ToDouble(pRow.get_Value(intVarIdx));
        //                //for (int i = 0; i < pfrmMScatterResults.pChart.Series[0].Points.Count; i++)
        //                //{
        //                //    if (dblSelVar == pfrmMScatterResults.pChart.Series[0].Points[i].XValue)
        //                //    {
        //                //        double dblWeightVar = pfrmMScatterResults.pChart.Series[0].Points[i].YValues[0];
        //                //        seriesPts.Points.AddXY(dblSelVar, dblWeightVar);
        //                //    }
        //                //}

        //                int intSelFID = Convert.ToInt32(pRow.get_Value(intFIDIdx));
        //                for (int i = 0; i < pfrmMScatterResults.arrFID.Length; i++)
        //                {
        //                    if (intSelFID == pfrmMScatterResults.arrFID[i])
        //                    {
        //                        seriesPts.Points.AddXY(pfrmMScatterResults.pChart.Series[0].Points[i].XValue, pfrmMScatterResults.pChart.Series[0].Points[i].YValues[0]); 
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
        //private void BrushingToOptimizeGraph(frmClassificationGraph pfrmClassGraph, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmClassGraph.pFLayer == pFLayer)
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
        ////private void BrushingToClassSepGraph(frmCSDesign pfrmCSDesign, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        ////{
        ////    try
        ////    {
        ////        if (pfrmCSDesign.pFLayer == pFLayer)
        ////        {
        ////            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        ////            int intVar1Idx = pFeatureClass.Fields.FindField(pfrmCSDesign.strValueFldName);

        ////            int intLastSeriesIdx = pfrmCSDesign.pChart.Series.Count - 1;

        ////            //Remove Previous Selection
        ////            if (pfrmCSDesign.pChart.Series[intLastSeriesIdx].Name == "SelSeries")
        ////                pfrmCSDesign.pChart.Series.RemoveAt(intLastSeriesIdx);

        ////            ICursor pCursor = null;
        ////            featureSelection.SelectionSet.Search(null, false, out pCursor);
        ////            int selCnties = featureSelection.SelectionSet.Count;

        ////            IRow pRow = pCursor.NextRow();
        ////            //IFeature pFeature = pFCursor.NextFeature();

        ////            //int dblOriPtsSize = pfrmClassGraph.pChart.Series[0].MarkerSize;

        ////            //System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        ////            var Selseries = new System.Windows.Forms.DataVisualization.Charting.Series
        ////            {
        ////                Name = "SelSeries",
        ////                Color = System.Drawing.Color.Red,
        ////                BorderColor = System.Drawing.Color.Black,
        ////                BackHatchStyle = ChartHatchStyle.DiagonalCross,
        ////                IsVisibleInLegend = false,
        ////                ChartType = SeriesChartType.Column,
        ////            };


        ////            pfrmCSDesign.pChart.Series.Add(Selseries);

        ////            while (pRow != null)
        ////            {
        ////                double dblEstValue = Convert.ToDouble(pRow.get_Value(intVar1Idx));
        ////                for (int i = 0; i < pfrmCSDesign.pChart.Series[0].Points.Count; i++)
        ////                {
        ////                    double dblYValue = pfrmCSDesign.pChart.Series[0].Points[i].YValues[0] + pfrmCSDesign.pChart.Series[1].Points[i].YValues[0];
        ////                    if (dblEstValue == dblYValue)
        ////                    {
        ////                        double dblSelYValue = pfrmCSDesign.pChart.Series[0].Points[i].YValues[0] + pfrmCSDesign.pChart.Series[1].Points[i].YValues[0] + pfrmCSDesign.pChart.Series[2].Points[i].YValues[0];
        ////                        Selseries.Points.AddXY(pfrmCSDesign.pChart.Series[0].Points[i].XValue, dblSelYValue);
        ////                    }
        ////                }
        ////                pRow = pCursor.NextRow();
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        MessageBox.Show("Exception:" + ex.Message);
        ////        return;
        ////    }
        ////}
        
        //private void BrushingToAttTable(frmAttributeTable pfrmAttTable, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    try
        //    {
        //        if (pfrmAttTable.dgvAttTable.SelectedRows.Count > 0)
        //            pfrmAttTable.dgvAttTable.ClearSelection();
        //        IFeatureLayer pFormFLayer = pfrmAttTable.pLayer as IFeatureLayer;
        //        if (pFormFLayer == pFLayer)
        //        {
        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;

        //            int intFIDIdx = pFeatureClass.FindField(pFeatureClass.OIDFieldName);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);

        //            IRow pRow = pCursor.NextRow();

        //            int intNfeature = featureSelection.SelectionSet.Count;
        //            int i = 0;
        //            int[] intFIDs = new int[intNfeature];
        //            while (pRow != null)
        //            {
        //                intFIDs[i] = Convert.ToInt32(pRow.get_Value(intFIDIdx));
        //                pfrmAttTable.dgvAttTable.Rows[intFIDs[i]].Selected = true;
        //                i++;
        //                pRow = pCursor.NextRow();
        //            }
        //        }
        //        else
        //            return;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return;
        //    }
        //}
        //private void BrushingToBoxPlot(frmBoxPlotResults pfrmBoxPlotResults, IFeatureLayer pFLayer, IFeatureSelection featureSelection)
        //{
        //    //try
        //    //{
        //        if (pfrmBoxPlotResults.pFLayer == pFLayer)
        //        {
        //            //Remove Previous Selection
        //            int intLastSeriesIdx = pfrmBoxPlotResults.pChart.Series.Count - 1;

        //            if (pfrmBoxPlotResults.pChart.Series[intLastSeriesIdx].Name == "SelPoints")
        //                pfrmBoxPlotResults.pChart.Series.RemoveAt(intLastSeriesIdx);

        //            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
        //            int intVarIdx = pFeatureClass.Fields.FindField(pfrmBoxPlotResults.strFieldName);
        //            int intGrpIdx = pFeatureClass.Fields.FindField(pfrmBoxPlotResults.strGroupFieldName);

        //            ICursor pCursor = null;
        //            featureSelection.SelectionSet.Search(null, false, out pCursor);

        //            IRow pRow = pCursor.NextRow();

        //            intLastSeriesIdx = pfrmBoxPlotResults.pChart.Series.Count - 1;

        //            int dblOriPtsSize = pfrmBoxPlotResults.pChart.Series[intLastSeriesIdx].MarkerSize;

        //            System.Drawing.Color pMarkerColor = System.Drawing.Color.Red;
        //            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
        //            {
        //                Name = "SelPoints",
        //                Color = pMarkerColor,
        //                BorderColor = pMarkerColor,
        //                IsVisibleInLegend = false,
        //                IsXValueIndexed = false,
        //                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
        //                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
        //                MarkerSize = dblOriPtsSize * 2
        //            };

        //            pfrmBoxPlotResults.pChart.Series.Add(seriesPts);

        //            int intNGroups = pfrmBoxPlotResults.uvList.Count;

        //            while (pRow != null)
        //            {
        //                string strGrp = pRow.get_Value(intGrpIdx).ToString();

        //                for (int k = 0; k < intNGroups; k++)
        //                {
        //                    if (strGrp == pfrmBoxPlotResults.uvList[k].ToString())
        //                    {
        //                        int intXvalue = k * 2 + 1;
        //                        seriesPts.Points.AddXY(intXvalue, Convert.ToDouble(pRow.get_Value(intVarIdx)));
        //                    }
        //                }
        //                pRow = pCursor.NextRow();
        //            }
        //        }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show("Exception:" + ex.Message);
        //    //    return;
        //    //}
        //}
        public void AddRenderInfo(MainForm mForm, IFeatureLayer pFLayer, string strValueFldName, string strUncernFldName, double[] ClassBreaks, int[,] arrColors, string strClassificationType)
        {
            try
            {
                clsRenderedLayers pRenderedLayer = new clsRenderedLayers();
                pRenderedLayer.strLayerName = pFLayer.Name;
                if (mForm.lstRenderedLayers.Count != 0)
                {
                    bool blnNotFindID = true;
                    for (int i = 0; i < mForm.lstRenderedLayers.Count; i++)
                    {
                        if (pRenderedLayer.strLayerName == mForm.lstRenderedLayers[i].strLayerName)
                        {
                            mForm.lstRenderedLayers[i].strValueFldName = strValueFldName;
                            mForm.lstRenderedLayers[i].strUncernFldName = strUncernFldName;
                            mForm.lstRenderedLayers[i].ClassBreaks = ClassBreaks;
                            mForm.lstRenderedLayers[i].arrColors = arrColors;
                            mForm.lstRenderedLayers[i].strClassificationType = strClassificationType;
                            blnNotFindID = false;
                        }
                    }
                    if (blnNotFindID)
                    {
                        pRenderedLayer.strValueFldName = strValueFldName;
                        pRenderedLayer.strUncernFldName = strUncernFldName;
                        pRenderedLayer.ClassBreaks = ClassBreaks;
                        pRenderedLayer.arrColors = arrColors;
                        pRenderedLayer.strClassificationType = strClassificationType;
                        mForm.lstRenderedLayers.Add(pRenderedLayer);
                    }
                }
                else
                {
                    pRenderedLayer.strValueFldName = strValueFldName;
                    pRenderedLayer.strUncernFldName = strUncernFldName;
                    pRenderedLayer.ClassBreaks = ClassBreaks;
                    pRenderedLayer.arrColors = arrColors;
                    pRenderedLayer.strClassificationType = strClassificationType;
                    mForm.lstRenderedLayers.Add(pRenderedLayer);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        public void AddField(IFeatureClass fClass, string name, esriFieldType fieldType)
        {
            try
            {
                IField newField = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)newField;
                fieldEdit.Name_2 = name;
                fieldEdit.Type_2 = fieldType;

                fClass.AddField(newField);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }
        //public void FeatureSelectionOnActiveView(string whereClause, IActiveView pActiveView, IFeatureLayer pFLayer)
        //{
        //    try
        //    {
        //        ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast

        //        // Set up the query
        //        ESRI.ArcGIS.Geodatabase.IQueryFilter queryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
        //        queryFilter.WhereClause = whereClause;

        //        // Invalidate only the selection cache. Flag the original selection
        //        pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

        //        // Perform the selection
        //        featureSelection.SelectFeatures(queryFilter, ESRI.ArcGIS.Carto.esriSelectionResultEnum.esriSelectionResultNew, false);

        //        // Flag the new selection
        //        pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Exception:" + ex.Message);
        //        return;
        //    }
        //}

        public void CalculateDesStat(IFeatureLayer pFLayer, string strFieldName, int intDeciPlaces)
        {
            try
            {
                //IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

                IFeatureCursor pFCursor = pFLayer.Search(null, true);
                int intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);
                if (intNFeatureCount == 0)
                    return;

                IFeature pFeature = pFCursor.NextFeature();

                int intFieldIdx = pFLayer.FeatureClass.Fields.FindField(strFieldName);

                double[] arrValue = new double[intNFeatureCount];

                int i = 0;
                while (pFeature != null)
                {

                    arrValue[i] = Convert.ToDouble(pFeature.get_Value(intFieldIdx));
                    i++;
                    pFeature = pFCursor.NextFeature();
                }

                string strDecimalPlaces = "N" + intDeciPlaces.ToString();
                string[] strResults = new string[8];
                strResults[0] = "Count: " + intNFeatureCount.ToString();
                strResults[1] = "Minimum: " + Math.Round(arrValue.Min(), intDeciPlaces).ToString();
                strResults[2] = "Maximum: " + Math.Round(arrValue.Max(), intDeciPlaces).ToString();
                strResults[3] = "Sum: " + Math.Round(arrValue.Sum(), intDeciPlaces).ToString();
                strResults[4] = "Mean: " + Math.Round(arrValue.Average(), intDeciPlaces).ToString();
                strResults[5] = "Standard deviation: " + Math.Round(CalSDfromArray(arrValue), intDeciPlaces).ToString();

                double[] medianIQR = new double[2];
                medianIQR = getMedian_IQR(arrValue);
                strResults[6] = "Median: " + Math.Round(medianIQR[0], intDeciPlaces).ToString();
                strResults[7] = "IQR: " + Math.Round(medianIQR[1], intDeciPlaces).ToString();

                frmGenResult pfrmResult = new frmGenResult();
                pfrmResult.Text = "Descriptive statistics of " + pFLayer.Name;
                pfrmResult.txtField.Text = strFieldName;
                pfrmResult.txtStatistics.Lines = strResults;
                pfrmResult.Show();
            }
            catch (Exception ex)



            {
                MessageBox.Show("Exception:" + ex.Message);
                return;
            }
        }

        private double CalSDfromArray(double[] arrSubset)
        {
            try
            {
                double average = arrSubset.Average();
                double sumOfSquaresOfDifferences = arrSubset.Select(val => (val - average) * (val - average)).Sum();
                double sd = Math.Sqrt(sumOfSquaresOfDifferences / arrSubset.Length);

                return sd;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return -9999;
            }
        }
        private double[] getMedian_IQR(double[] adblTarget)
        {
            try
            {
                double[] Stats = new double[2];
                System.Array.Sort(adblTarget);
                int intLength = adblTarget.Length;

                Stats[0] = GetMedian(adblTarget);
                //Get 1st and 3rd Quantile
                if (intLength % 2 == 0)
                {
                    int newLength = intLength / 2;
                    double[] lowSubset = new double[newLength];
                    double[] upperSubset = new double[newLength];
                    System.Array.Copy(adblTarget, 0, lowSubset, 0, newLength);
                    System.Array.Copy(adblTarget, newLength, upperSubset, 0, newLength);

                    Stats[1] = GetMedian(upperSubset) - GetMedian(lowSubset);
                }
                else
                {
                    int newLength = (intLength - 1) / 2;
                    double[] lowSubset = new double[newLength];
                    double[] upperSubset = new double[newLength];
                    System.Array.Copy(adblTarget, 0, lowSubset, 0, newLength);
                    System.Array.Copy(adblTarget, newLength + 1, upperSubset, 0, newLength);

                    Stats[1] = GetMedian(upperSubset) - GetMedian(lowSubset);
                }
                return Stats;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return null;
            }
        }
        private double GetMedian(double[] sortedArray)
        {
            try
            {
                double dblMedian = 0;
                int intLength = sortedArray.Length;

                if (intLength % 2 == 0)
                {
                    // count is even, average two middle elements
                    double a = sortedArray[intLength / 2 - 1];
                    double b = sortedArray[intLength / 2];
                    return dblMedian = (a + b) / 2;
                }
                else
                {
                    // count is odd, return the middle element
                    return dblMedian = sortedArray[(intLength - 1) / 2];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return -9999;
            }
        }
        public int SWMusingGAL(REngine pEngine, IFeatureClass pFClass, string strGALPath)
        {
            try
            {
                int intResult = 0;
                string strExtension = strGALPath.Substring(strGALPath.Length - 3);
                string strRegionFld = null;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(strGALPath))
                {
                    String line = sr.ReadLine();
                    int inttemp = line.LastIndexOf(" ");
                    strRegionFld = line.Substring(inttemp + 1);
                    Console.WriteLine(line);
                }


                if (pFClass.FindField(strRegionFld) == -1)
                {
                    if (strExtension == "gal")
                    {
                        if (strRegionFld == "POLY_ID")
                            MessageBox.Show("Unique IDs are overrided.");
                        else
                        {
                            MessageBox.Show("GAL file is invalid; When you create spatial weight matrix in GeoDa, you need to specify ID field, or use default ID field name (POLY_ID)");

                        }
                    }
                    else if(strExtension == "gwt")
                    {
                        MessageBox.Show("GWT file is invalid; When you create spatial weight matrix in GeoDa, you should specify ID field");
                        intResult = -1;
                        return intResult;
                    }
                }
                

                string strGALpathR = strGALPath.Replace(@"\", @"/");
                if(strExtension == "gal")
                {
                    string strTemp = null;
                    if (strRegionFld == "FID" || strRegionFld == "POLY_ID")
                    {
                        strTemp = "sample.nb <- read.gal('" + strGALpathR + "', override.id = TRUE)";
                    }
                    else
                    {
                        strTemp = "sample.nb <- read.gal('" + strGALpathR + "', region.id = sample.shp@data$" + strRegionFld + ")";
                    }
                    pEngine.Evaluate(strTemp);

                    bool blnSymmetric = pEngine.Evaluate("is.symmetric.nb(sample.nb)").AsLogical().First();
                    if (blnSymmetric == false)
                    {
                        DialogResult dialogResult = MessageBox.Show("This spatial weight matrix is asymmetric. Some functions are restricted in an asymmetric matrix. Do you want to continue?", "Asymmetric spatial weight matrix", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            intResult = -1;
                            return intResult;
                        }
                    }

                    pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");

                }
                else if (strExtension == "gwt")
                {
                    pEngine.Evaluate("sample.nb <- read.gwt2nb('" + strGALpathR + "', region.id = sample.shp@data$" + strRegionFld + ")");

                    bool blnSymmetric = pEngine.Evaluate("is.symmetric.nb(sample.nb)").AsLogical().First();
                    if(blnSymmetric == false)
                    {
                        DialogResult dialogResult = MessageBox.Show("This spatial weight matrix is asymmetric. Some functions are restricted in an asymmetric matrix. Do you want to continue?", "Asymmetric spatial weight matrix", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            intResult = -1;
                            return intResult;
                        }
                    }
                    
                    pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                }


                return intResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
                return -1;
            }
        }
        private double GetIQR(double[] adblTarget)
        {
            System.Array.Sort(adblTarget);
            int intLength = adblTarget.Length;
            double dblLowSubset = 0, dblUpperSubset = 0;

            if (intLength % 2 == 0)
            {
                int newLength = intLength / 2;
                double[] lowSubset = new double[newLength];
                double[] upperSubset = new double[newLength];
                System.Array.Copy(adblTarget, 0, lowSubset, 0, newLength);
                System.Array.Copy(adblTarget, newLength, upperSubset, 0, newLength);

                dblLowSubset = GetMedian(lowSubset);
                dblUpperSubset = GetMedian(upperSubset);
            }
            else
            {
                int newLength = (intLength - 1) / 2;
                double[] lowSubset = new double[newLength];
                double[] upperSubset = new double[newLength];
                System.Array.Copy(adblTarget, 0, lowSubset, 0, newLength);
                System.Array.Copy(adblTarget, newLength + 1, upperSubset, 0, newLength);

                dblLowSubset = GetMedian(lowSubset);
                dblUpperSubset = GetMedian(upperSubset);
            }
            double dblIQR = dblUpperSubset - dblLowSubset;

            return dblIQR;
        }

        //Creating Spatial Weight Matrix
        public int CreateSpatialWeightMatrix(REngine pEngine, IFeatureClass pFClass, string strSWMtype, frmProgress pfrmProgress)
        {
            //Return 0, means fails to create spatial weight matrix, 1 means success.

            SpatialWeightMatrixType pSWMType = new SpatialWeightMatrixType();

            if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {

                if (strSWMtype == pSWMType.strPolyDefs[0])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=T)");
                }
                else if (strSWMtype == pSWMType.strPolyDefs[1])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=F)");
                }
                else
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=T)");
                }
                //For dealing empty neighbors
                try
                {
                    pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W');sample.listb <- nb2listw(sample.nb, style='B')");
                }
                catch
                {
                    DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                    if (dialogResult == DialogResult.Yes)
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        pfrmProgress.Close();
                        return 0;
                    }
                }
            }
            else if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
                bool blnOpen = false;
                int intIdx = 0;

                for (int j = 0; j < pFormCollection.Count; j++)
                {
                    if (pFormCollection[j].Name == "frmSubsetPoly")//Brushing to Histogram
                    {
                        intIdx = j;

                        blnOpen = true;
                    }
                }

                if (blnOpen) //Delaunay with clipping
                {
                    frmSubsetPoly pfrmSubsetPoly1 = pFormCollection[intIdx] as frmSubsetPoly;
                    if (pfrmSubsetPoly1.m_blnSubset)
                    {
                        string strPolypathR = FilePathinRfromLayer(pfrmSubsetPoly1.m_pFLayer);

                        pEngine.Evaluate("sample.sub.shp <- readShapePoly('" + strPolypathR + "')");

                        pEngine.Evaluate("sample.nb <- del.subset(sample.shp, sample.sub.shp)");
                        bool blnError = pEngine.Evaluate("nrow(sample.shp) == length(sample.nb)").AsLogical().First();

                        if (blnError == false)
                        {
                            MessageBox.Show("The number of features in points and the rows of neighbors is not matched.", "Error");
                            return 0;
                        }
                        else
                        {
                            try
                            {
                                pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                            }
                            catch
                            {
                                DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                                if (dialogResult == DialogResult.Yes)
                                {
                                    pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                                }
                                else if (dialogResult == DialogResult.No)
                                {
                                    pfrmProgress.Close();
                                    return 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    pEngine.Evaluate("sample.nb <- tri2nb(coordinates(sample.shp))");
                    //For dealing empty neighbors

                    try
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                    }
                    catch
                    {
                        DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                        if (dialogResult == DialogResult.Yes)
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            pfrmProgress.Close();
                            return 0;
                        }
                    }
                }
            }
            else
            {
                int intResult = SWMusingGAL(pEngine, pFClass, strSWMtype);
                if (intResult == -1)
                {
                    pfrmProgress.Close();
                    return 0;
                }

            }
            return 1;
        }
        public int CreateSpatialWeightMatrix1(REngine pEngine, IFeatureClass pFClass, string strSWMtype, frmProgress pfrmProgress, double dblAdvancedValue, bool blnCumul)
        {
            //Return 0, means fails to create spatial weight matrix, 1 means success.

            SpatialWeightMatrixType pSWMType = new SpatialWeightMatrixType();

            if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                if (strSWMtype == pSWMType.strPolyDefs[0])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=T)");
                }
                else if (strSWMtype == pSWMType.strPolyDefs[1])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=F)");
                }
                
                if(dblAdvancedValue > 1)
                {
                    try
                    {
                        pEngine.Evaluate("sample.nblags <- nblag(sample.nb, maxlag = " + dblAdvancedValue.ToString() + ")");
                    }
                    catch
                    {
                        MessageBox.Show("Please reduce the maximum lag order");
                    }

                    if (blnCumul)
                        pEngine.Evaluate("sample.nb <- nblag_cumul(sample.nblags)");
                    else
                        pEngine.Evaluate("sample.nb <- sample.nblags[[" + dblAdvancedValue.ToString() + "]]");

                }

                //For dealing empty neighbors
                try
                {
                    pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W');sample.listb <- nb2listw(sample.nb, style='B')");
                }
                catch
                {
                    DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                    if (dialogResult == DialogResult.Yes)
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        pfrmProgress.Close();
                        return 0;
                    }
                }
            }
            else if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                #region Delaunay
                if(strSWMtype == pSWMType.strPointDef[0])
                {

                    if (blnCumul == false)
                    {
                        pEngine.Evaluate("sample.nb <- tri2nb(coordinates(sample.shp))");
                        //For dealing empty neighbors

                        try
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                        }
                        catch
                        {
                            DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                            if (dialogResult == DialogResult.Yes)
                            {
                                pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                pfrmProgress.Close();
                                return 0;
                            }
                        }
                    }
                }
                #endregion
                else if(strSWMtype == pSWMType.strPointDef[1])
                {
                    pEngine.Evaluate("sample.nb <- dnearneigh(coordinates(sample.shp), 0, " + dblAdvancedValue.ToString() + ")");
                    //For dealing empty neighbors

                    try
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                    }
                    catch
                    {
                        DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                        if (dialogResult == DialogResult.Yes)
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            pfrmProgress.Close();
                            return 0;
                        }
                    }
                    finally
                    {
                        pfrmProgress.Close();
                        MessageBox.Show("Fail to create spatial weights matrix");
                    }
                }
                else if (strSWMtype == pSWMType.strPointDef[2])
                {
                    pEngine.Evaluate("col.knn <- knearneigh(coordinates(sample.shp), k=" + dblAdvancedValue.ToString() + ")");
                    pEngine.Evaluate("sample.nb <- knn2nb(col.knn)");
                    //For dealing empty neighbors

                    try
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                    }
                    catch
                    {
                        DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                        if (dialogResult == DialogResult.Yes)
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            pfrmProgress.Close();
                            return 0;
                        }
                    }
                }

            }
            else
            {
                int intResult = SWMusingGAL(pEngine, pFClass, strSWMtype);
                if (intResult == -1)
                {
                    pfrmProgress.Close();
                    return 0;
                }

            }
            return 1;
        }//For only polygons

        public int CreateSpatialWeightMatrixPts(REngine pEngine, IFeatureClass pFClass, string strSWMtype, frmProgress pfrmProgress, double dblAdvancedValue, bool blnCumul, IFeatureLayer pFLayer)//For only point dataset
        {
            //Return 0, means fails to create spatial weight matrix, 1 means success.

            SpatialWeightMatrixType pSWMType = new SpatialWeightMatrixType();
            
            if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                #region Delaunay
                if (strSWMtype == pSWMType.strPointDef[0])
                {

                    if (blnCumul == false)
                    {
                        pEngine.Evaluate("sample.nb <- tri2nb(coordinates(sample.shp))");
                        //For dealing empty neighbors

                        try
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                        }
                        catch
                        {
                            DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                            if (dialogResult == DialogResult.Yes)
                            {
                                pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                pfrmProgress.Close();
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        if (pFLayer == null)
                            return 0;

                        MainForm pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                        
                        string strStartPath = pForm.strPath;
                        string pathr = strStartPath.Replace(@"\", @"/");
                        pEngine.Evaluate("source('" + pathr + "/del.subset.R')");

                        try
                        {
                            pEngine.Evaluate("library(deldir); library(rgeos)");

                        }
                        catch
                        {
                            MessageBox.Show("Please checked R packages installed in your local computer.");
                            return 0;
                        }


                        string strPolypathR = FilePathinRfromLayer(pFLayer);

                        pEngine.Evaluate("sample.sub.shp <- readShapePoly('" + strPolypathR + "')");

                        pEngine.Evaluate("sample.nb <- del.subset(sample.shp, sample.sub.shp)");
                        bool blnError = pEngine.Evaluate("nrow(sample.shp) == length(sample.nb)").AsLogical().First();

                        if (blnError == false)
                        {
                            MessageBox.Show("The number of features in points and the rows of neighbors is not matched.", "Error");
                            return 0;
                        }
                        else
                        {
                            try
                            {
                                pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                            }
                            catch
                            {
                                DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                                if (dialogResult == DialogResult.Yes)
                                {
                                    pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                                }
                                else if (dialogResult == DialogResult.No)
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                }
                #endregion
                else if (strSWMtype == pSWMType.strPointDef[1])
                {
                    pEngine.Evaluate("sample.nb <- dnearneigh(coordinates(sample.shp), 0, " + dblAdvancedValue.ToString() + ")");
                    //For dealing empty neighbors
                    if(pEngine.Evaluate("sum(card(sample.nb)) < 1").AsLogical().First())
                    {
                        MessageBox.Show("There are too many empty neighbors");
                        pfrmProgress.Close();
                        return 0;
                    }

                    try
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                    }
                    catch
                    {
                        DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                        if (dialogResult == DialogResult.Yes)
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            pfrmProgress.Close();
                            return 0;
                        }
                    }
                    finally
                    {
                        pfrmProgress.Close();
                        MessageBox.Show("Fail to create spatial weights matrix");
                    }
                }
                else if (strSWMtype == pSWMType.strPointDef[2])
                {
                    pEngine.Evaluate("col.knn <- knearneigh(coordinates(sample.shp), k=" + dblAdvancedValue.ToString() + ")");
                    pEngine.Evaluate("sample.nb <- knn2nb(col.knn)");
                    //For dealing empty neighbors

                    bool blnSymmetric = pEngine.Evaluate("is.symmetric.nb(sample.nb)").AsLogical().First();
                    if (blnSymmetric == false)
                    {
                        DialogResult dialogResult = MessageBox.Show("This spatial weight matrix is asymmetric. Some functions are restricted in an asymmetric matrix. Do you want to continue?", "Asymmetric spatial weight matrix", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            pfrmProgress.Close();
                            return 0;
                        }
                    }

                    try
                    {
                        pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W'); sample.listb <- nb2listw(sample.nb, style='B')");
                    }
                    catch
                    {
                        DialogResult dialogResult = MessageBox.Show("Empty neighbor sets are founded. Do you want to continue?", "Empty neighbor", MessageBoxButtons.YesNo);


                        if (dialogResult == DialogResult.Yes)
                        {
                            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE);sample.listb <- nb2listw(sample.nb, style='B', zero.policy=TRUE)");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            pfrmProgress.Close();
                            return 0;
                        }
                    }
                }

            }
            else
            {
                int intResult = SWMusingGAL(pEngine, pFClass, strSWMtype);
                if (intResult == -1)
                {
                    pfrmProgress.Close();
                    return 0;
                }

            }
            return 1;
        }

        public int ExploreSpatialWeightMatrix1(REngine pEngine, IFeatureClass pFClass, string strSWMtype, frmProgress pfrmProgress, double dblAdvancedValue, bool blnCumul)
        {
            //Return 0, means fails to create spatial weight matrix, 1 means success.

            SpatialWeightMatrixType pSWMType = new SpatialWeightMatrixType();

            if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                if (strSWMtype == pSWMType.strPolyDefs[0])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=T)");
                }
                else if (strSWMtype == pSWMType.strPolyDefs[1])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=F)");
                }

                if (dblAdvancedValue > 1)
                {
                    try
                    {
                        pEngine.Evaluate("sample.nblags <- nblag(sample.nb, maxlag = " + dblAdvancedValue.ToString() + ")");
                    }
                    catch
                    {
                        MessageBox.Show("Please reduce the maximum lag order");
                    }

                    if (blnCumul)
                        pEngine.Evaluate("sample.nb <- nblag_cumul(sample.nblags)");
                    else
                        pEngine.Evaluate("sample.nb <- sample.nblags[[" + dblAdvancedValue.ToString() + "]]");

                }

                
            }
            else if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                #region Delaunay
                if (strSWMtype == pSWMType.strPointDef[0])
                {

                    if (blnCumul == false)
                    {
                        pEngine.Evaluate("sample.nb <- tri2nb(coordinates(sample.shp))");
                        //For dealing empty neighbors
                        
                    }

                }
                #endregion
                else if (strSWMtype == pSWMType.strPointDef[1])
                {
                    pEngine.Evaluate("sample.nb <- dnearneigh(coordinates(sample.shp), 0, " + dblAdvancedValue.ToString() + ")");
                    
                }
                else if (strSWMtype == pSWMType.strPointDef[2])
                {
                    pEngine.Evaluate("col.knn <- knearneigh(coordinates(sample.shp), k=" + dblAdvancedValue.ToString() + ")");
                    pEngine.Evaluate("sample.nb <- knn2nb(col.knn)");
                    //For dealing empty neighbors

                   
                }

            }
            else
            {
                int intResult = SWMusingGAL(pEngine, pFClass, strSWMtype);
                if (intResult == -1)
                {
                    pfrmProgress.Close();
                    return 0;
                }

            }
            return 1;
        }

        public int ExploreSpatialWeightMatrixPts(REngine pEngine, IFeatureClass pFClass, string strSWMtype, frmProgress pfrmProgress, double dblAdvancedValue, bool blnCumul, IFeatureLayer pFLayer)
        {
            //Return 0, means fails to create spatial weight matrix, 1 means success.

            SpatialWeightMatrixType pSWMType = new SpatialWeightMatrixType();

            if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                if (strSWMtype == pSWMType.strPolyDefs[0])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=T)");
                }
                else if (strSWMtype == pSWMType.strPolyDefs[1])
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=F)");
                }

                if (dblAdvancedValue > 1)
                {
                    try
                    {
                        pEngine.Evaluate("sample.nblags <- nblag(sample.nb, maxlag = " + dblAdvancedValue.ToString() + ")");
                    }
                    catch
                    {
                        MessageBox.Show("Please reduce the maximum lag order");
                    }

                    if (blnCumul)
                        pEngine.Evaluate("sample.nb <- nblag_cumul(sample.nblags)");
                    else
                        pEngine.Evaluate("sample.nb <- sample.nblags[[" + dblAdvancedValue.ToString() + "]]");

                }


            }
            else if (pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint)
            {
                #region Delaunay
                if (strSWMtype == pSWMType.strPointDef[0])
                {

                    if (blnCumul == false)
                    {
                        pEngine.Evaluate("sample.nb <- tri2nb(coordinates(sample.shp))");
                        //For dealing empty neighbors

                    }
                    else
                    {
                        if (pFLayer == null)
                            return 0;

                        MainForm pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

                        string strStartPath = pForm.strPath;
                        string pathr = strStartPath.Replace(@"\", @"/");
                        pEngine.Evaluate("source('" + pathr + "/del.subset.R')");

                        try
                        {
                            pEngine.Evaluate("library(deldir); library(rgeos)");

                        }
                        catch
                        {
                            MessageBox.Show("Please checked R packages installed in your local computer.");
                            return 0;
                        }


                        string strPolypathR = FilePathinRfromLayer(pFLayer);

                        pEngine.Evaluate("sample.sub.shp <- readShapePoly('" + strPolypathR + "')");

                        pEngine.Evaluate("sample.nb <- del.subset(sample.shp, sample.sub.shp)");
                        bool blnError = pEngine.Evaluate("nrow(sample.shp) == length(sample.nb)").AsLogical().First();

                        if (blnError == false)
                        {
                            MessageBox.Show("The number of features in points and the rows of neighbors is not matched.", "Error");
                            return 0;
                        }


                    }
                }
                #endregion
                else if (strSWMtype == pSWMType.strPointDef[1])
                {
                    pEngine.Evaluate("sample.nb <- dnearneigh(coordinates(sample.shp), 0, " + dblAdvancedValue.ToString() + ")");

                }
                else if (strSWMtype == pSWMType.strPointDef[2])
                {
                    pEngine.Evaluate("col.knn <- knearneigh(coordinates(sample.shp), k=" + dblAdvancedValue.ToString() + ")");
                    pEngine.Evaluate("sample.nb <- knn2nb(col.knn)");
                    //For dealing empty neighbors


                }

            }
            else
            {
                int intResult = SWMusingGAL(pEngine, pFClass, strSWMtype);
                if (intResult == -1)
                {
                    pfrmProgress.Close();
                    return 0;
                }

            }
            return 1;
        }


        public class SpatialWeightMatrixType
        {
            //Polygon 
            public string strPolySWM = "Contiguity (Queen)"; //Default
            public string[] strPolyDefs = new string[] { "Contiguity (Queen)", "Contiguity (Rook)" };

            //Points
            public string strPointSWM = "Delaunay triangulation";
            public string[] strPointDef = new string[] { "Delaunay triangulation", "Distance", "k-Nearest neigbors" } ;
        }
        public class SWMCodingScheme
        {
            public string[] strSchemes = new string[] { "Row standardized", "Binary", "Globally standardized", "Variance-stabilizing" };
        }
        public class Def_SpatialWeightsMatrix
        {
            //Basic information;
            public ESRI.ArcGIS.Geometry.esriGeometryType Geometry;
            public string Definition;
            public double AdvancedValue; //It contains the number of higher order for polygons and threshold distance or number of k for points
            public bool Cumulative; //Higher orders neighbors; Cumulative (True)
            public bool Subset; //Delaunay triangulation are clipped by study area polygon (True) 

            //FID and NID for Brushing and Linking
            public int[] FIDs;
            public List<int>[] NBIDs;
            public double[,] XYCoord;

            //NB information
            public int FeatureCount;
            public int NonZeroLinkCount;
            public double PercentNonZeroWeight;
            public double AverageNumberofLink;
            public bool Symmetric;

            public double[] NeighborCounts;

            //For Higher lags
            public double[] LinkCountforHigher;
            public double[] AverageforHigher;
        }
    }
}
