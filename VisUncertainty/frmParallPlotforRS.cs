using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using RDotNet;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace VisUncertainty
{
    public partial class frmParallPlotforRS : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private IRasterLayer m_pOriRstLayer;
        private IRasterLayer m_pClsRstLayer;
        private IRasterLayer m_pCnfRstLayer;
        private IRasterLayer m_pPrbRstLayer;

        private int m_intTotalNSeries;

        private List<List<int>> m_lstPtsIdContainer;
        private List<int> m_lstPtSeriesID;
        private List<int[]> m_lstIDsValues;
        private int m_intColumnCnt;
        private IRasterProps m_pRasterProps;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        public frmParallPlotforRS()
        {
            InitializeComponent();
            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

            m_pActiveView = m_pForm.axMapControl1.ActiveView;
            m_pSnippet = new clsSnippet();

            for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
            {
                
                ILayer pLayer = m_pActiveView.FocusMap.get_Layer(i);

                if (pLayer is IRasterLayer)
                {
                    cboOriImage.Items.Add(pLayer.Name);
                    cboClsImage.Items.Add(pLayer.Name);
                    cboProbImage.Items.Add(pLayer.Name);
                }
            }
        }

        private void cboOriImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strRstNM = cboOriImage.Text;
            m_pOriRstLayer = GetRasterLayer(strRstNM, m_pActiveView);
            

            IRaster2 pRaster = (IRaster2)m_pOriRstLayer.Raster;
            
            IRasterBandCollection pBands = pRaster.RasterDataset as IRasterBandCollection;
            int intBandCnt = pBands.Count;

            lstLayers.Items.Clear();
            for(int i = 0; i < intBandCnt; i++)
            {
                lstLayers.Items.Add(pBands.Item(i).Bandname); 
            }

        }

        private IRasterLayer GetRasterLayer(string strRstNM, IActiveView pActiveView)
        {
            IRasterLayer pRstLayer = null;
            //IRasterLayer pTempRLayer = null;
            int intCnt = pActiveView.FocusMap.LayerCount;
            for (int i = 0; i < intCnt; i++)
            {
                if (strRstNM == m_pActiveView.FocusMap.get_Layer(i).Name)
                {
                    pRstLayer = (IRasterLayer)m_pActiveView.FocusMap.get_Layer(i);

                    //pTempRLayer = (IRasterLayer)m_pActiveView.FocusMap.get_Layer(i);
                    //IDataset pDs = (IDataset)pTempRLayer;
                    //string strFullPath = pDs.Workspace.PathName + pDs.Name;
                    //pRstLayer.CreateFromFilePath(pRstLayer.FilePath);
                }
            }
            if (pRstLayer == null)
                return null;
            else
                return pRstLayer;
        }

        private void cboClsImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strRstNM = cboClsImage.Text;
            m_pClsRstLayer = GetRasterLayer(strRstNM, m_pActiveView);


            IRaster2 pRaster = (IRaster2)m_pClsRstLayer.Raster;

            IRasterBandCollection pBands = pRaster.RasterDataset as IRasterBandCollection;
            int intBandCnt = pBands.Count;
            if (intBandCnt > 1)
                return;

            IRasterBand pBand = pBands.Item(0);
            bool blnHasTable = false;
            pBand.HasTable(out blnHasTable);
           
            if (blnHasTable == false)
                return;

            ITable pTable = pBand.AttributeTable;
            
            ICursor pCursor = pTable.Search(null, false);
            IRow pRow = pCursor.NextRow();
            string strClassNM = "Class_name";
            int intNameIdx = pTable.FindField(strClassNM);

            lstClasses.Items.Clear();
            while (pRow != null)
            {
                lstClasses.Items.Add(pRow.get_Value(intNameIdx));
                pRow = pCursor.NextRow();
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
//Class Setting
            int intClsCnt = lstClasses.CheckedItems.Count;
            string[] arrClsNMs = new string[intClsCnt];

            for (int i = 0; i < intClsCnt; i++)
                arrClsNMs[i] = lstClasses.CheckedItems[i].ToString();

            
            int[,] arrClsColor = new int[4, intClsCnt];

            IRaster2 pClsRaster = (IRaster2)m_pClsRstLayer.Raster;

            IRasterBandCollection pClsBands = pClsRaster.RasterDataset as IRasterBandCollection;
            IRasterBand pClsBand = pClsBands.Item(0);
            ITable pClsTable = pClsBand.AttributeTable;

            ICursor pCursor = pClsTable.Search(null, false);
            IRow pRow = pCursor.NextRow();

            string strRed = "Red", strGreen = "Green", strBlue = "Blue", strValue = "Value", strNameFld ="Class_name";

            int intRedIdx = pClsTable.FindField(strRed);
            int intGreenIdx = pClsTable.FindField(strGreen);
            int intBlueIdx = pClsTable.FindField(strBlue);
            int intValueIdx = pClsTable.FindField(strValue);
            int intNameIdx = pClsTable.FindField(strNameFld);

            while (pRow != null)
            {
                for (int i = 0; i < intClsCnt; i++)
                {
                    string strClassName = pRow.get_Value(intNameIdx).ToString();
                    if (arrClsNMs[i] == strClassName)
                    {
                        arrClsColor[0, i] = Convert.ToInt32(pRow.get_Value(intValueIdx));
                        arrClsColor[1, i] = Convert.ToInt32(pRow.get_Value(intRedIdx));
                        arrClsColor[2, i] = Convert.ToInt32(pRow.get_Value(intGreenIdx));
                        arrClsColor[3, i] = Convert.ToInt32(pRow.get_Value(intBlueIdx));
                    }
                }
                pRow = pCursor.NextRow();
            }


            // Get NoDataValues
            IRasterProps pClsrasterProps = (IRasterProps)pClsRaster;
            System.Array pClsNoData = (System.Array)pClsrasterProps.NoDataValue;

            IPnt blocksize = new PntClass();
            blocksize.SetCoords(pClsrasterProps.Width, pClsrasterProps.Height);
            // Create a raster cursor with a system-optimized pixel block size by passing a null.

            IRasterCursor pClsRstCursor = pClsRaster.CreateCursorEx(blocksize);

            IPixelBlock3 pClspBlock3 = (IPixelBlock3)pClsRstCursor.PixelBlock;


            System.Array pClspixels = (System.Array)pClspBlock3.get_PixelData(0);
            
//Layer Setting

            int intLyrCnt = lstLayers.CheckedItems.Count;

            int[] arrLyrIdx = new int[intLyrCnt];
            for (int i = 0; i < intLyrCnt; i++)
                arrLyrIdx[i] = lstLayers.CheckedIndices[i];

            string strOriRSTpath = m_pOriRstLayer.FilePath;
            string strExtentFileSource = System.IO.Path.GetDirectoryName(strOriRSTpath);
            string strExtentFileName = System.IO.Path.GetFileName(strOriRSTpath);

            IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
            IRasterWorkspace rasterWorkspace = (IRasterWorkspace)
                workspaceFactory.OpenFromFile(strExtentFileSource, 0);

            //Open a file raster dataset. 
            IRasterDataset2 rasterDataset = (IRasterDataset2)rasterWorkspace.OpenRasterDataset(strExtentFileName);
            IRaster2 pOriRaster2 = (IRaster2)rasterDataset.CreateFullRaster();
            
            //IRaster2 pOriRaster2 = (IRaster2)m_pOriRstLayer.Raster;
            //Create a raster cursor with a system-optimized pixel block size by passing a null.

            // Get NoDataValues
            m_pRasterProps = (IRasterProps)pOriRaster2;
            System.Array pOriNoData = (System.Array)m_pRasterProps.NoDataValue;

            
            IPnt Oriblocksize = new PntClass();
            Oriblocksize.SetCoords(m_pRasterProps.Width, m_pRasterProps.Height);
            // Create a raster cursor with a system-optimized pixel block size by passing a null.

            IRasterCursor pOriRstCursor = pOriRaster2.CreateCursorEx(Oriblocksize);
            //IRasterCursor pOriRstCursor = pOriRaster2.CreateCursorEx(null);
            IPixelBlock3 pixelblock3 = (IPixelBlock3)pOriRstCursor.PixelBlock;


            System.Array[] OriPixels = new Array[intLyrCnt];
            //UInt16[][,] intArry = new UInt16[intLyrCnt][,];
            for (int i = 0; i < intLyrCnt; i++)
            {
                //intArry[i] = (UInt16[,])pixelblock3.get_PixelData(arrLyrIdx[i]);
                OriPixels[i] = (System.Array)pixelblock3.get_PixelData(arrLyrIdx[i]);
            }

            //Compare the Arrays and Draw
            int intBlockwidth = m_pRasterProps.Width;
            int intBlockHeight = m_pRasterProps.Height;

            //List<int[]>[] lstDN = new List<int[]>[intClsCnt];
            List<int>[][] lstDN = new List<int>[intLyrCnt][];
            for (int j = 0; j < intLyrCnt; j++)
            {
                lstDN[j] = new List<int>[intClsCnt];
            }

            for (int j = 0; j < intLyrCnt; j++)
            {
                for(int i = 0; i < intClsCnt; i++)
                    lstDN[j][i] = new List<int>();

            }

            List<int>[][] lstLyrIDs = new List<int>[intLyrCnt][];
            for (int j = 0; j < intLyrCnt; j++)
            {
                lstLyrIDs[j] = new List<int>[intClsCnt];
            }

            for (int j = 0; j < intLyrCnt; j++)
            {
                for (int i = 0; i < intClsCnt; i++)
                    lstLyrIDs[j][i] = new List<int>();

            }

            //Uncertainty Layer Settings
            System.Array[] ProbPixels = null;
            int intProLyrCnt = 0;
            List<int>[][] lstProb = null;
            List<int>[][] lstProbIDs = null;

            string strProbRst = cboProbImage.Text;

            if (strProbRst != "" && chkAddProb.Checked)
            {
                IRasterLayer pProbRstLyr = GetRasterLayer(strProbRst, m_pActiveView);

                string strProbRSTpath = pProbRstLyr.FilePath;
                strExtentFileSource = System.IO.Path.GetDirectoryName(strProbRSTpath);
                strExtentFileName = System.IO.Path.GetFileName(strProbRSTpath);

                workspaceFactory = new RasterWorkspaceFactoryClass();
                rasterWorkspace = (IRasterWorkspace)
                workspaceFactory.OpenFromFile(strExtentFileSource, 0);

                rasterDataset = (IRasterDataset2)rasterWorkspace.OpenRasterDataset(strExtentFileName);
                IRaster2 pProbRaster2 = (IRaster2)rasterDataset.CreateFullRaster();

                // Get NoDataValues
                IRasterProps pProbrasterProps = (IRasterProps)pProbRaster2;
                System.Array pProbNoData = (System.Array)pProbrasterProps.NoDataValue;

                IRasterCursor pProbRstCursor = pProbRaster2.CreateCursorEx(Oriblocksize); // Using same block size
                IPixelBlock3 Probpixelblock3 = (IPixelBlock3)pProbRstCursor.PixelBlock;

                intProLyrCnt = Probpixelblock3.Planes;

                ProbPixels = new Array[intProLyrCnt];

                for (int i = 0; i < intProLyrCnt; i++)
                {
                    ProbPixels[i] = (System.Array)Probpixelblock3.get_PixelData(i);
                }

                lstProb = new List<int>[intProLyrCnt][];

                for (int j = 0; j < intProLyrCnt; j++)
                {
                    lstProb[j] = new List<int>[intClsCnt];
                }

                for (int j = 0; j < intProLyrCnt; j++)
                {
                    for (int i = 0; i < intClsCnt; i++)
                        lstProb[j][i] = new List<int>();

                }


                lstProbIDs = new List<int>[intProLyrCnt][];

                for (int j = 0; j < intProLyrCnt; j++)
                {
                    lstProbIDs[j] = new List<int>[intClsCnt];
                }

                for (int j = 0; j < intProLyrCnt; j++)
                {
                    for (int i = 0; i < intClsCnt; i++)
                        lstProbIDs[j][i] = new List<int>();

                }

            }

            
            //Store ID and Values for Brushing and Linking
            m_lstIDsValues = new List<int[]>();

            int intID = 0;

            for (int colIndex = 0; colIndex < intBlockwidth; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < intBlockHeight; rowIndex++)
                {
                    int intClass = Convert.ToInt32(pClspixels.GetValue(colIndex, rowIndex));

                    int[] arrValues = new int[intLyrCnt + intProLyrCnt + 3];
                    if (intClass != Convert.ToInt32(pClsNoData.GetValue(0)))
                    {
                        //int[] arrValues = new int[intLyrCnt + intProLyrCnt + 3];

                        arrValues[0] = colIndex;
                        arrValues[1] = rowIndex;
                        arrValues[2] = intClass;

                        for (int i = 0; i < intClsCnt; i++)
                        {

                            if (arrClsColor[0, i] == intClass)
                            {
 
                                //Get DNs
                                for (int j = 0; j < intLyrCnt; j++)
                                {
                                    if (colIndex == 0 && rowIndex == 241)
                                        j = j;
                                    int intValue = Convert.ToInt32(OriPixels[j].GetValue(colIndex, rowIndex));
                                    lstDN[j][i].Add(intValue);
                                    lstLyrIDs[j][i].Add(intID);
                                    arrValues[j + 3] = intValue;
                                    
                                }

                                //Get Probs
                                if (strProbRst != "" && chkAddProb.Checked)
                                {

                                    for (int j = 0; j < intProLyrCnt; j++)
                                    {
                                        int intValue = Convert.ToInt32(Convert.ToInt32(ProbPixels[j].GetValue(colIndex, rowIndex)) * 2.55);
                                        lstProb[j][i].Add(intValue);
                                        lstProbIDs[j][i].Add(intID);
                                        arrValues[j + 3+ intLyrCnt] = intValue;
                                    }
                                }
                            }

                        }
                    }
                    m_lstIDsValues.Add(arrValues);
                    intID++;
                    if (m_lstIDsValues.Count != intID)
                        MessageBox.Show("Diff"); //For deburgging

                }
            }


            pChart.Series.Clear();
            m_intColumnCnt = 0;
            if (strProbRst != "" && chkAddProb.Checked)
                m_intColumnCnt = intLyrCnt + intProLyrCnt;
            else
                m_intColumnCnt = intLyrCnt;

            m_lstPtsIdContainer = new List<List<int>>();
            m_lstPtSeriesID = new List<int>();

            for (int i = 0; i < intClsCnt; i++)
            {
                
                Color FillColor = Color.FromArgb(arrClsColor[1, i], arrClsColor[2, i], arrClsColor[3, i]);

                for (int j = 0; j < m_intColumnCnt; j++)
                {
                    List<int> lstTarget;
                    if (j < intLyrCnt)
                        lstTarget = lstDN[j][i];
                    else
                        lstTarget = lstProb[j - intLyrCnt][i];

                    List<int> lstIDs;
                    if (j < intLyrCnt)
                        lstIDs = lstLyrIDs[j][i];
                    else
                        lstIDs = lstProbIDs[j - intLyrCnt][i];

                    List<int> sortedTarget = new List<int>(lstTarget);

                    //to prevent overlapping of Boxplots
                    double dblPlotHalfWidth = 0.05;
                    double dblMin = (dblPlotHalfWidth * (intClsCnt - 1)) * (-1);
                    double dblRefXvalue = j + (dblMin + (i * (dblPlotHalfWidth * 2)));

                    //Find Max and Min
                    double[] adblStats = BoxPlotStats(sortedTarget);
                    bool blnDrawViolin = true;
                    if (adblStats[0] == adblStats[4])
                        blnDrawViolin = false;

                    //Restrict Plot min and max
                    if (adblStats[0] < 0)
                        adblStats[0] = 0;
                    if (adblStats[4] > 255)
                        adblStats[4] = 255;

                    string strNumbering = i.ToString() + "_" + j.ToString();
                    
                    double dblXmin = dblRefXvalue - (dblPlotHalfWidth/2);
                    double dblXmax = dblRefXvalue + (dblPlotHalfWidth/2);

                    if (blnDrawViolin)
                    {
                        //Draw Lines
                        AddLineSeries(pChart, "m_" + strNumbering, Color.Red, 1, ChartDashStyle.Dash, dblRefXvalue - dblPlotHalfWidth, dblRefXvalue + dblPlotHalfWidth, adblStats[2], adblStats[2]);
                        AddLineSeries(pChart, "v_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[1], adblStats[3]);
                        //AddLineSeries(pChart, "v2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblRefXvalue, dblRefXvalue, adblStats[3], adblStats[4]);
                        AddLineSeries(pChart, "h1_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[1], adblStats[1]);
                        AddLineSeries(pChart, "h2_" + strNumbering, FillColor, 1, ChartDashStyle.Solid, dblXmin, dblXmax, adblStats[3], adblStats[3]);
                    }
                    else
                    {
                        //Draw Outliers
                        var pMedPt = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "mpt_" + strNumbering,
                            Color = Color.Red,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                            MarkerStyle = MarkerStyle.Circle,
                            MarkerSize = 2,
                        };

                        pChart.Series.Add(pMedPt);
                        pMedPt.Points.AddXY(dblRefXvalue, adblStats[2]);
                    }

                    if (blnDrawViolin)
                    {
                        //Draw Violin Plot
                        var pviolin = new System.Windows.Forms.DataVisualization.Charting.Series
                        {
                            Name = "vio1_" + strNumbering,
                            Color = FillColor,
                            BorderColor = FillColor,
                            IsVisibleInLegend = false,
                            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line

                        };
                        pChart.Series.Add(pviolin);

                        double[,] adblViolinStats = ViolinPlot(sortedTarget, 4);

                        int intChartLenth = (adblViolinStats.Length) / 2;

                        
                        for (int k = 0; k < intChartLenth; k++)
                        {
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue - adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        }
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[4]);
                        for (int k = intChartLenth - 1; k >= 0; k--)
                            if (adblViolinStats[k, 0] > adblStats[0] && adblViolinStats[k, 0] < adblStats[4])
                                pviolin.Points.AddXY(dblRefXvalue + adblViolinStats[k, 1], adblViolinStats[k, 0]);
                        pviolin.Points.AddXY(dblRefXvalue, adblStats[0]);
                    }



                    //Draw Outliers
                    var pOutlier = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "out_" + strNumbering,
                        Color = FillColor,
                        BorderColor = FillColor,
                        IsVisibleInLegend = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerSize = 1,
                    };

                    pChart.Series.Add(pOutlier);
                    m_lstPtSeriesID.Add(pChart.Series.Count - 1); //Add Series ID for brushing and linking

                    List<int> lstPTsIds = new List<int>();
                    int intListCnt = lstTarget.Count;
                    for (int k = 0; k < intListCnt; k++)
                    {
                        if (lstTarget[k] < adblStats[0] || lstTarget[k] > adblStats[4])
                        {
                            pOutlier.Points.AddXY(dblRefXvalue, lstTarget[k]);
                            lstPTsIds.Add(lstIDs[k]);
                            //if (m_lstIDsValues[lstIDs[k]][3 + i] != lstTarget[k])
                            //    MessageBox.Show("ddd");
                        }
                    }
                    m_lstPtsIdContainer.Add(lstPTsIds);
                }

            }
            m_intTotalNSeries = pChart.Series.Count;

            //Chart Setting
            pChart.ChartAreas[0].AxisY.Minimum = 0;
            pChart.ChartAreas[0].AxisY.Maximum = 255; 
            
            pChart.ChartAreas[0].AxisY.IsLabelAutoFit = true;
            if (chkAddProb.Checked)
            {
                pChart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                pChart.ChartAreas[0].AxisY.Title = "DN and Probability";
            }
            else
                pChart.ChartAreas[0].AxisY.Title = "DN";

            pChart.ChartAreas[0].AxisX.Maximum = m_intColumnCnt-0.5;
            pChart.ChartAreas[0].AxisX.Minimum = -0.5;

            pChart.ChartAreas[0].AxisX.Title = "Layers";

            pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            pChart.ChartAreas[0].AxisX.CustomLabels.Clear();

            for (int j = 0; j < m_intColumnCnt; j++)
            {
                System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
                pcutsomLabel.FromPosition = j - 0.5;
                pcutsomLabel.ToPosition = j + 0.5;
                if (j < intLyrCnt)
                    pcutsomLabel.Text = "DN of "+lstLayers.CheckedItems[j].ToString()+ "(0-255)";
                else
                    pcutsomLabel.Text = "Prob "+lstClasses.Items[j-intLyrCnt].ToString()+"(0-100)";

                this.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }
        }
        private double[,] ViolinPlot(List<int> plstTarget, double dblBandwidth)
        {
            plstTarget.Sort();
            int intLength = plstTarget.Count;
            int intMax = plstTarget[intLength -1 ];
            int intMin = plstTarget[0];
            int intRange = intMax - intMin;

            double[,] adblResult = new double[intRange + 1, 2];

            for (int i = 0; i < (intRange + 1); i++)
            {
                int intX = intMin + i;
                adblResult[i, 0] = intX;

                int j = 0;
                double dblU = (intX - plstTarget[j]) / dblBandwidth;
                double dblK = 0;
                
                while (dblU > -1) //Apply While to improve efficienty, it can apply only sorted lst or arry
                {
                    //Apply Epanechnikov
                    if (dblU < 1)
                    {
                        dblK += 0.75 * (1 - Math.Pow(dblU, 2));
                    }
                    j++;
                    if (j < intLength)
                        dblU = (intX - plstTarget[j]) / dblBandwidth;
                    else
                        dblU = -1;
                }
                adblResult[i, 1] = dblK / (dblBandwidth * intLength);
            }

            return adblResult;

        }

        private double[] BoxPlotStats(List<int> plstTarget)
        {
            plstTarget.Sort();

            double[] adblStats = new double[5];
            //double[] BPStats = new double[5];

            //adblStats[0] = adblTarget.Min();
            //adblStats[4] = adblTarget.Max();
            int intLength = plstTarget.Count;

            adblStats[2] = GetMedian(plstTarget);
            //Get 1st and 3rd Quantile
            if (intLength % 2 == 0)
            {
                int newLength = intLength / 2;
                List<int> lowSubset = plstTarget.GetRange(0, newLength);
                List<int> upperSubset = plstTarget.GetRange(newLength, newLength);

                adblStats[1] = GetMedian(lowSubset);
                adblStats[3] = GetMedian(upperSubset);
            }
            else
            {
                int newLength = (intLength - 1) / 2;
                List<int> lowSubset = plstTarget.GetRange(0, newLength);
                List<int> upperSubset = plstTarget.GetRange(newLength+1, newLength);

                adblStats[1] = GetMedian(lowSubset);
                adblStats[3] = GetMedian(upperSubset);

            }
            double dblIQR = adblStats[3] - adblStats[1];
            adblStats[0] = adblStats[1] - (1.5 * dblIQR);
            adblStats[4] = adblStats[3] + (1.5 * dblIQR);
            return adblStats;
        }

        private double GetMedian(List<int> sortedArray)
        {
            double dblMedian = 0;
            int intLength = sortedArray.Count;

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

        private void AddLineSeries(Chart pChart, string strSeriesName, System.Drawing.Color FillColor, int intWidth, ChartDashStyle BorderDash, double dblXMin, double dblXMax, double dblYMin, double dblYMax)
        {
            //try
            //{
            var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = strSeriesName,
                Color = FillColor,
                //BorderColor = Color.Black,
                BorderWidth = intWidth,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                BorderDashStyle = BorderDash,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

            };

            pChart.Series.Add(pSeries);

            pSeries.Points.AddXY(dblXMin, dblYMin);
            pSeries.Points.AddXY(dblXMax, dblYMax);
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return;
            //}

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
            while (pChart.Series.Count != m_intTotalNSeries)
                pChart.Series.RemoveAt(pChart.Series.Count - 1);

            _canDraw = false;

            HitTestResult result = pChart.HitTest(e.X, e.Y);



            int dblOriPtsSize = pChart.Series[m_lstPtSeriesID[0]].MarkerSize;
            int intTotalSriCount = m_lstPtSeriesID.Count;

            int intSelLinesCount = 0;
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
                        System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                        var seriesLines = new System.Windows.Forms.DataVisualization.Charting.Series
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
                        intSelLinesCount++;
                        pChart.Series.Add(seriesLines);

                        int intSelLocationIdx = m_lstPtsIdContainer[i][j];
                        int[] dblSelValues = m_lstIDsValues[intSelLocationIdx];
                        for(int k =0; k <m_intColumnCnt; k++)
                        {
                            int intYvalue = m_lstIDsValues[intSelLocationIdx][3 + k];
                            seriesLines.Points.AddXY(k + dblXOffset, intYvalue);
                        }

                        DrawPointsOnActiveView(m_lstIDsValues[intSelLocationIdx][0],  m_lstIDsValues[intSelLocationIdx][1], m_pRasterProps, m_pActiveView);
                    }

                }
            }
            
               
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
            //for (int i = 0; i < intLstCnt; i++)
            //{
            //    int intIdx = lstIndices[i];
            //    double[] arrSelValue = arrValue[intIdx];
            //    //drawing a polyline
            //    IPoint FromP = new PointClass();
            //    FromP.X = arrSelValue[0]; FromP.Y = arrSelValue[1];

            //    IPoint ToP = new PointClass();
            //    ToP.X = arrSelValue[2]; ToP.Y = arrSelValue[3];

            //    IPolyline polyline = new PolylineClass();
            //    IPointCollection pointColl = polyline as IPointCollection;
            //    pointColl.AddPoint(FromP);
            //    pointColl.AddPoint(ToP);

            //    IElement pElement = new LineElementClass();

            //    ILineElement pLineElement = (ILineElement)pElement;
            //    pLineElement.Symbol = pSimpleLineSymbol;
            //    pElement.Geometry = polyline;

            //    pGraphicContainer.AddElement(pElement, 0);
            //}

            //pActiveView.Refresh();

        }

    }
}
