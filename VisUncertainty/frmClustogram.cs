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
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using RDotNet;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;

namespace VisUncertainty
{
    public partial class frmClustogram : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        //private double[][] m_arrResults;
        private IFeatureLayer m_pFLayer;
        private clsBrusingLinking m_pBL;
        //private double[] m_arrMaxFromLinks;
        //private double[][] m_arrMaxToLinks;

        private int m_intNFeature;
        private double[,] m_arrXYCoord; //For Brushing and Linking
        private int m_intMaxLag;
        private int[] m_pFIDs;
        private string m_strOIDName;
        private List<Correlogram> m_lstCorrelograms;
        private List<Neighbors> m_lstNeigbors;
        private int m_intTotalNSeries;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

//Additional variable for S
        private double[] m_SZeroZ;

        public frmClustogram()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pBL = new clsBrusingLinking();
                m_pSnippet = new clsSnippet();
                m_pEngine = m_pForm.pEngine;
                try
                {
                    m_pEngine.Evaluate("library(spdep); library(maptools)");
                }
                catch
                {
                    MessageBox.Show("Please checked R packages installed in your local computer.");
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboFieldName.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                        cboFldName2.Items.Add(fields.get_Field(i).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private class Correlogram
        {
            public double GlobalMeasureExp { get; set; }
            //public int[] FIDs { get; set; }
            public int SumLinkCount { get; set; }
            public double GlobalMeasureEst { get; set; }
            public List<double> LocalMeasureEst { get; set; }
        }

        private class Neighbors
        {
            public List<int>[] IDs { get; set; }
        }

        private class BoxPlotStats
        {
            public double LowerAdjecent { get; set; }
            public double FirstQuantile { get; set; }
            public double Median { get; set; }
            public double ThirdQuantile { get; set; }
            public double UpperAdjecent { get; set; }
        }

        private BoxPlotStats GetBoxPlotStats(double[] adblTarget)
        {
            BoxPlotStats pBoxplotStats = new  BoxPlotStats();
            //double[] adblStats = new double[5];
            double[] BPStats = new double[5];
            System.Array.Sort(adblTarget);
            //adblStats[0] = adblTarget.Min();
            //adblStats[4] = adblTarget.Max();
            int intLength = adblTarget.Length;

            pBoxplotStats.Median = GetMedian(adblTarget);
            //Get 1st and 3rd Quantile
            if (intLength % 2 == 0)
            {
                int newLength = intLength / 2;
                double[] lowSubset = new double[newLength];
                double[] upperSubset = new double[newLength];
                System.Array.Copy(adblTarget, 0, lowSubset, 0, newLength);
                System.Array.Copy(adblTarget, newLength, upperSubset, 0, newLength);

                pBoxplotStats.FirstQuantile = GetMedian(lowSubset);
                pBoxplotStats.ThirdQuantile = GetMedian(upperSubset);
            }
            else
            {
                int newLength = (intLength - 1) / 2;
                double[] lowSubset = new double[newLength];
                double[] upperSubset = new double[newLength];
                System.Array.Copy(adblTarget, 0, lowSubset, 0, newLength);
                System.Array.Copy(adblTarget, newLength + 1, upperSubset, 0, newLength);

                pBoxplotStats.FirstQuantile = GetMedian(lowSubset);
                pBoxplotStats.ThirdQuantile = GetMedian(upperSubset);
            }
            double dblIQR = pBoxplotStats.ThirdQuantile - pBoxplotStats.FirstQuantile;

            pBoxplotStats.LowerAdjecent = adblTarget.Min();
            pBoxplotStats.UpperAdjecent = adblTarget.Max();

            //pBoxplotStats.LowerAdjecent = pBoxplotStats.FirstQuantile - (1.5 * dblIQR);
            //pBoxplotStats.UpperAdjecent = pBoxplotStats.ThirdQuantile + (1.5 * dblIQR);

            return pBoxplotStats;
        }

        private double GetMedian(double[] sortedArray)
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
        private void AddLineSeries(frmClustogram pfrmBoxplotResults, string strSeriesName, System.Drawing.Color FillColor, int intWidth, ChartDashStyle BorderDash, double dblXMin, double dblXMax, double dblYMin, double dblYMax)
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

            pfrmBoxplotResults.pChart.Series.Add(pSeries);

            pSeries.Points.AddXY(dblXMin, dblYMin);
            pSeries.Points.AddXY(dblXMax, dblYMax);
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return;
            //}

        }

        private double RuleofThumb(double[] pdblTarget)
        {

            int intLength = pdblTarget.Length;
            double average = pdblTarget.Average();
            double sumOfSquaresOfDifferences = pdblTarget.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / intLength);

            double dblBandwidth = 2.35 * sd * Math.Pow(intLength, -0.2);
            return dblBandwidth;


        }

        private double[,] ViolinPlot(double[] pdblTarget, double dblBandwidth)
        {
            System.Array.Sort(pdblTarget);
            int intLength = pdblTarget.Length;
            double dblMax = pdblTarget[intLength - 1];
            double dblMin = pdblTarget[0];
            double dblRange = dblMax - dblMin;

            //Fixed Counts on Y-axis as 50;
            double dblIncrement = dblRange / 50;
            int intCnt = 51;


            double[,] adblResult = new double[intCnt, 2];

            for (int i = 0; i < intCnt; i++)
            {
                double dblX = dblMin + (i * dblIncrement);
                adblResult[i, 0] = dblX;

                int j = 0;
                double dblU = (dblX - pdblTarget[j]) / dblBandwidth;
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
                        dblU = (dblX - pdblTarget[j]) / dblBandwidth;
                    else
                        dblU = -1;
                }
                //adblResult[i, 1] = dblK / (dblBandwidth * intLength);
                //adblResult[i, 1] = dblK;
                adblResult[i, 1] = dblK / intLength;
            }

            return adblResult;

        }
        private void btnRun_Click(object sender, EventArgs e)
        {

            if (cboFieldName.Text == "")
            {
                MessageBox.Show("Please select target field");
                return;
            }
            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Processing:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            bool blnBivar = false;
            if (cboSAM.Text == "Local L")
                blnBivar = true;
            ////Close Results form, if it is opend.
            //CloseOpendResultForm(m_pHandle);
            //IGraphicsContainer pGraphicContainer = m_pActiveView.GraphicsContainer;
            //pGraphicContainer.DeleteAllElements();

            // Creates the input and output matrices from the shapefile//
            string strLayerName = cboTargetLayer.Text;

            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

            m_pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFClass = m_pFLayer.FeatureClass;
            m_strOIDName = pFClass.OIDFieldName; //For brushing and linking
            m_intNFeature = pFClass.FeatureCount(null);

            IFeatureCursor pFCursor = m_pFLayer.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get index for independent and dependent variables
            //Get variable index            
            string strVarNM = (string)cboFieldName.SelectedItem;
            int intVarIdx = pFClass.FindField(strVarNM);

            string strVarNM2 = null;
            int intVarIdx2 = 0;
            if (blnBivar)
            {
                strVarNM2 = (string)cboFldName2.SelectedItem;
                intVarIdx2 = pFClass.FindField(strVarNM2);
            }


            //Store Variable at Array
            double[] arrVar1 = new double[m_intNFeature];
            double[] arrVar2 = new double[m_intNFeature]; //For L
            m_pFIDs = new int[m_intNFeature]; //For brushing and linking
            m_arrXYCoord = new double[m_intNFeature, 2];

            int i = 0;
            IArea pArea;
            while (pFeature != null)
            {
                pArea = (IArea)pFeature.Shape;
                m_arrXYCoord[i, 0] = pArea.Centroid.X;
                m_arrXYCoord[i, 1] = pArea.Centroid.Y;

                arrVar1[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                m_pFIDs[i] = pFeature.OID;
                if (blnBivar)
                    arrVar2[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx2));

                i++;
                pFeature = pFCursor.NextFeature();
            }

            //Get the file path and name to create spatial weight matrix
            string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

            if (strNameR == null)
                return;

            //Using Local C, load function saved in startup path (AllFunction.R)
            StringBuilder plotCommmand = new StringBuilder();
            if (cboSAM.Text == "Local Geary Ratio")
            {
                string strStartPath = m_pForm.strPath;
                string pathr = strStartPath.Replace(@"\", @"/");
                plotCommmand.Append("source('" + pathr + "/AllFunctions.R')");
                m_pEngine.Evaluate(plotCommmand.ToString());
                plotCommmand.Clear();
            }
            else if (cboSAM.Text == "Local L" || cboSAM.Text == "Local S")
            {
                string strStartPath = m_pForm.strPath;
                string pathr = strStartPath.Replace(@"\", @"/");
                plotCommmand.Append("source('" + pathr + "/clustogram.R')");
                m_pEngine.Evaluate(plotCommmand.ToString());
                plotCommmand.Clear();
            }


            //Create spatial weight matrix in R
            m_pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
            m_pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)"); // Apply for only connectivity

            //Creat Higher spatial lag
            m_intMaxLag = Convert.ToInt32(nudLagOrder.Value);
            try
            {
                //m_pEngine.Evaluate("sample.nblags <- nblag(sample.nb, maxlag = " + intMaxLag.ToString() + ")");
                m_pEngine.Evaluate("sample.nblags <- nblag(sample.nb, " + m_intMaxLag.ToString() + ")");
            }
            catch
            {
                MessageBox.Show("Please reduce the maximum lag order");
            }

            //Create Addictive Spatial Lag

            if (cboSAM.Text == "Local L" || cboSAM.Text == "Local S")
            {
                m_intMaxLag = m_intMaxLag + 1;
                if (chkNonDiag.Checked)
                    m_pEngine.Evaluate("sample.nblags <- add.nblags.star(sample.nblags)");
                else
                    m_pEngine.Evaluate("sample.nblags <- add.nblags.new(sample.nblags)");
            }

            //Get NB for Brushing and Linking
            m_lstNeigbors = new List<Neighbors>();
            for (int j = 0; j < m_intMaxLag; j++)
            {
                Neighbors pNeighbor = new Neighbors();
                List<int>[] IDs = new List<int>[m_intNFeature];

                for (int k = 0; k < m_intNFeature; k++)
                    IDs[k] = m_pEngine.Evaluate("sample.nblags[[" + (j + 1).ToString() + "]][[" + (k + 1).ToString() + "]]").AsInteger().ToList();

                pNeighbor.IDs = IDs;
                m_lstNeigbors.Add(pNeighbor);

            }

            pChart.Series.Clear();

            //Plot command for R

            NumericVector vecVar = m_pEngine.CreateNumericVector(arrVar1);
            m_pEngine.SetSymbol(strVarNM, vecVar);

            NumericVector vecVar2 = null;

            if (blnBivar)
            {
                vecVar2 = m_pEngine.CreateNumericVector(arrVar2);
                m_pEngine.SetSymbol(strVarNM2, vecVar2);
            }

            //Store Information of spatial lag
            m_lstCorrelograms = new List<Correlogram>();
            List<BoxPlotStats> lstBoxPlotStats = new List<BoxPlotStats>();

            for (int j = 0; j < m_intMaxLag; j++)
            {
                Correlogram pCorrelogram = new Correlogram();

                //m_arrResults[j] = new double[5];

                m_pEngine.Evaluate("card.sample <- card(sample.nblags[[" + (j + 1).ToString() + "]])");
                pCorrelogram.SumLinkCount = Convert.ToInt32(m_pEngine.Evaluate("sum(card.sample)").AsNumeric().First());

                //If ther is no link, return
                if (pCorrelogram.SumLinkCount == 0)
                {
                    MessageBox.Show("There is no link at " + (j + 1).ToString() + " order.");
                    return;
                }

                //Functions below are used for brushing on map control, they are under reviewing 080316 HK
                //m_arrMaxFromLinks[j] = m_pEngine.Evaluate("which.max(card.sample)").AsNumeric().First();
                //m_arrMaxToLinks[j] = m_pEngine.Evaluate("sample.nblags[[" + (j + 1).ToString() + "]][[which.max(card.sample)]]").AsNumeric().ToArray();



                //Calculate Local Measures
                //Select method
                if (cboSAM.Text == "Local Moran Coefficient")
                {
                    m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                    plotCommmand.Append("localsam.result <- localmoran(" + strVarNM + ", sample.listw);");
                    plotCommmand.Append("globalsam.result <- moran.test(" + strVarNM + ", sample.listw)");
                }
                else if (cboSAM.Text == "G Statstics")
                {
                    m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='B')");
                    plotCommmand.Append("localsam.result <- localG(" + strVarNM + ", sample.listw);");
                    plotCommmand.Append("globalsam.result <- globalG.test(" + strVarNM + ", sample.listw)");

                }
                else if (cboSAM.Text == "Local Geary Ratio")
                {
                    m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                    plotCommmand.Append("localsam.result <- localgeary(" + strVarNM + ", sample.listw);");
                    plotCommmand.Append("globalsam.result <- geary.test(" + strVarNM + ", sample.listw)");
                }
                else if (cboSAM.Text == "Local S")
                {
                    m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                    plotCommmand.Append("localsam.result <- S.local.mod(" + strVarNM + ", sample.listw);");
                    plotCommmand.Append("globalsam.result <- S.global.test.mod(" + strVarNM + ", sample.listw, method='randomization', alternative='two.sided')");
                    
                    //Not working
                    //if (j == 0)
                    //    m_pEngine.Evaluate("localsam.test <- S.local.test.mod(" + strVarNM + ", sample.listw.fromlags, method='conditional', alternative='two.sided', diag.zero=FALSE)");
             
                }
                else if (cboSAM.Text == "Local L")
                {
                    m_pEngine.Evaluate("sample.listw <- nb2listw(sample.nblags[[" + (j + 1).ToString() + "]], style='W')");
                    plotCommmand.Append("localsam.result <- L.local.mod(" + strVarNM + ", " + strVarNM2 + ", sample.listw);");
                    plotCommmand.Append("globalsam.result <- L.global.test.mod(" + strVarNM + ", " + strVarNM2 + ", sample.listw, alternative='two.sided')");
                }

                //select assumption
                //if (cboAssumption.Text == "Normality")
                //    plotCommmand.Append("randomisation=FALSE, alternative ='two.sided')");
                //else if (cboAssumption.Text == "Randomization")
                //    plotCommmand.Append("randomisation=TRUE, alternative ='two.sided')");
                m_pEngine.Evaluate(plotCommmand.ToString());
                plotCommmand.Clear();


                if (cboSAM.Text == "Local Moran Coefficient")
                {
                    pCorrelogram.LocalMeasureEst = m_pEngine.Evaluate("localsam.result[,1]").AsNumeric().ToList();
                    pCorrelogram.GlobalMeasureEst = m_pEngine.Evaluate("globalsam.result$estimate").AsNumeric().First();
                    pCorrelogram.GlobalMeasureExp = m_pEngine.Evaluate("globalsam.result$estimate[2]").AsNumeric().First();
                }
                else if (cboSAM.Text == "G Statstics")
                {
                    pCorrelogram.LocalMeasureEst = m_pEngine.Evaluate("localsam.result").AsNumeric().ToList();
                    pCorrelogram.GlobalMeasureEst = m_pEngine.Evaluate("globalsam.result$estimate").AsNumeric().First();
                    pCorrelogram.GlobalMeasureExp = m_pEngine.Evaluate("globalsam.result$estimate[2]").AsNumeric().First();
                }
                else if (cboSAM.Text == "Local Geary Ratio")
                {
                    pCorrelogram.LocalMeasureEst = m_pEngine.Evaluate("localsam.result[,1]").AsNumeric().ToList();
                    pCorrelogram.GlobalMeasureEst = m_pEngine.Evaluate("globalsam.result$estimate").AsNumeric().First();
                    pCorrelogram.GlobalMeasureExp = m_pEngine.Evaluate("globalsam.result$estimate[2]").AsNumeric().First();
                }
                else if (cboSAM.Text == "Local L" || cboSAM.Text == "Local S")
                {
                    pCorrelogram.LocalMeasureEst = m_pEngine.Evaluate("localsam.result").AsNumeric().ToList();
                    pCorrelogram.GlobalMeasureEst = m_pEngine.Evaluate("globalsam.result$estimate").AsNumeric().First();
                    pCorrelogram.GlobalMeasureExp = m_pEngine.Evaluate("globalsam.result$estimate[2]").AsNumeric().First();
                    //if (j == 0 && cboSAM.Text == "Local S")
                    //    m_SZeroZ = m_pEngine.Evaluate("localsam.test$sma.z.x").AsNumeric().ToArray();
                }

                BoxPlotStats pBoxplotStats = GetBoxPlotStats(pCorrelogram.LocalMeasureEst.ToArray());

                m_lstCorrelograms.Add(pCorrelogram);
                lstBoxPlotStats.Add(pBoxplotStats);
            }


            for (int j = 0; j < m_intMaxLag; j++)
            {
                int intXvalue = j * 2 + 1;

                double dblXmin = Convert.ToDouble(intXvalue) - 0.6;
                double dblXmax = Convert.ToDouble(intXvalue) + 0.6;
                double dblXBoxMin = Convert.ToDouble(intXvalue) - 0.8;
                double dblXBoxMax = Convert.ToDouble(intXvalue) + 0.8;

                if (chkBoxPlot.Checked)
                {
                    //Add Lines (Draw Boxplot)
                    AddLineSeries(this, "min_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXmin, dblXmax, lstBoxPlotStats[j].LowerAdjecent, lstBoxPlotStats[j].LowerAdjecent);
                    AddLineSeries(this, "BoxBottom_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXBoxMin, dblXBoxMax, lstBoxPlotStats[j].FirstQuantile, lstBoxPlotStats[j].FirstQuantile);
                    AddLineSeries(this, "BoxLeft_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXBoxMin, dblXBoxMin, lstBoxPlotStats[j].FirstQuantile, lstBoxPlotStats[j].ThirdQuantile);
                    AddLineSeries(this, "BoxRight_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXBoxMax, dblXBoxMax, lstBoxPlotStats[j].FirstQuantile, lstBoxPlotStats[j].ThirdQuantile);
                    AddLineSeries(this, "Boxup_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXBoxMin, dblXBoxMax, lstBoxPlotStats[j].ThirdQuantile, lstBoxPlotStats[j].ThirdQuantile);
                    AddLineSeries(this, "median_" + j.ToString(), Color.Black, 2, ChartDashStyle.Solid, dblXBoxMin, dblXBoxMax, m_lstCorrelograms[j].GlobalMeasureEst, m_lstCorrelograms[j].GlobalMeasureEst);
                    AddLineSeries(this, "max_" + j.ToString(), Color.Black, 1, ChartDashStyle.Solid, dblXmin, dblXmax, lstBoxPlotStats[j].UpperAdjecent, lstBoxPlotStats[j].UpperAdjecent);
                    AddLineSeries(this, "verDown_" + j.ToString(), Color.Black, 1, ChartDashStyle.Dash, intXvalue, intXvalue, lstBoxPlotStats[j].LowerAdjecent, lstBoxPlotStats[j].FirstQuantile);
                    AddLineSeries(this, "verUp_" + j.ToString(), Color.Black, 1, ChartDashStyle.Dash, intXvalue, intXvalue, lstBoxPlotStats[j].ThirdQuantile, lstBoxPlotStats[j].UpperAdjecent);
                }

                if (chkViolinPlot.Checked)
                {
                    //Draw Violin Plot
                    AddLineSeries(this, "vMean_" + j.ToString(), Color.Black, 2, ChartDashStyle.Solid, dblXBoxMin, dblXBoxMax, m_lstCorrelograms[j].GlobalMeasureEst, m_lstCorrelograms[j].GlobalMeasureEst);
                    var pviolin = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "vio1_" + j.ToString(),
                        Color = Color.Black,
                        BorderColor = Color.Black,
                        IsVisibleInLegend = false,
                        ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line

                    };
                    this.pChart.Series.Add(pviolin);

                    double dblBandwidth = RuleofThumb(m_lstCorrelograms[j].LocalMeasureEst.ToArray());
                    double[,] adblViolinStats = ViolinPlot(m_lstCorrelograms[j].LocalMeasureEst.ToArray(), dblBandwidth);


                    int intChartLenth = (adblViolinStats.Length) / 2;

                    pviolin.Points.AddXY(intXvalue, adblViolinStats[0, 0]);

                    for (int l = 0; l < intChartLenth; l++)
                        pviolin.Points.AddXY(intXvalue - adblViolinStats[l, 1], adblViolinStats[l, 0]);

                    pviolin.Points.AddXY(intXvalue, adblViolinStats[intChartLenth - 1, 0]);

                    for (int l = intChartLenth - 1; l >= 0; l--)
                        pviolin.Points.AddXY(intXvalue + adblViolinStats[l, 1], adblViolinStats[l, 0]);

                    pviolin.Points.AddXY(intXvalue, adblViolinStats[0, 0]);
                }

            }
            //Add a Expected value line

            //AddLineSeries(this, "ex", Color.Red, 1, ChartDashStyle.Dash, 0.5, m_intMaxLag * 2 + 1, m_lstCorrelograms[0].GlobalMeasureExp, m_lstCorrelograms[0].GlobalMeasureExp);

            var pExpSeries = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "ex",
                Color = Color.Red,
                //BorderColor = Color.Black,
                BorderWidth = 1,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                BorderDashStyle = ChartDashStyle.Dash,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

            };

            this.pChart.Series.Add(pExpSeries);

            for (int k = 0; k < m_intMaxLag; k++)
            {
                int intXvalue = k * 2 + 1;
                pExpSeries.Points.AddXY(intXvalue, m_lstCorrelograms[k].GlobalMeasureExp);
            }


            //Add Points
            System.Drawing.Color pMarkerColor = System.Drawing.Color.Blue;
            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Points",
                Color = pMarkerColor,
                BorderColor = pMarkerColor,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                MarkerSize = 3,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle

            };

            this.pChart.Series.Add(seriesPts);

            for (int k = 0; k < m_intMaxLag; k++)
            {
                int intXvalue = k * 2 + 1;
                foreach (double dblValue in m_lstCorrelograms[k].LocalMeasureEst)
                {
                    //if (dblValue < adbQuantiles[1, k] || dblValue > adbQuantiles[3, k])
                    seriesPts.Points.AddXY(intXvalue, dblValue);
                }
            }

            if (cboSAM.Text == "Local L" || cboSAM.Text == "Local S")
            {
                this.pChart.ChartAreas[0].AxisX.Title = "Spatial Range";
                if (chkNonDiag.Checked)
                    this.pChart.ChartAreas[0].AxisY.Title = cboSAM.Text + "*";
                else
                    this.pChart.ChartAreas[0].AxisY.Title = cboSAM.Text;

                this.pChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
                this.pChart.ChartAreas[0].AxisX.Maximum = m_intMaxLag * 2;
                this.pChart.ChartAreas[0].AxisX.Interval = 2;
                this.pChart.ChartAreas[0].AxisX.Minimum = 0;
                this.pChart.ChartAreas[0].AxisX.IntervalOffset = -1;
                this.pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                for (int n = 0; n < m_intMaxLag; n++)
                {
                    int intXvalue = n * 2 + 1;

                    double dblXmin = Convert.ToDouble(intXvalue) - 0.6;
                    double dblXmax = Convert.ToDouble(intXvalue) + 0.6;

                    CustomLabel pcutsomLabel = new CustomLabel();
                    pcutsomLabel.FromPosition = dblXmin;
                    pcutsomLabel.ToPosition = dblXmax;
                    pcutsomLabel.Text = (n).ToString();

                    this.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
                }
            }
            else
            {
                this.pChart.ChartAreas[0].AxisX.Title = "Spatial Lags";
                this.pChart.ChartAreas[0].AxisY.Title = cboSAM.Text;
                this.pChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
                this.pChart.ChartAreas[0].AxisX.Maximum = m_intMaxLag * 2;
                this.pChart.ChartAreas[0].AxisX.Interval = 2;
                this.pChart.ChartAreas[0].AxisX.Minimum = 0;
                this.pChart.ChartAreas[0].AxisX.IntervalOffset = -1;
                this.pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                for (int n = 0; n < m_intMaxLag; n++)
                {
                    int intXvalue = n * 2 + 1;

                    double dblXmin = Convert.ToDouble(intXvalue) - 0.6;
                    double dblXmax = Convert.ToDouble(intXvalue) + 0.6;

                    CustomLabel pcutsomLabel = new CustomLabel();
                    pcutsomLabel.FromPosition = dblXmin;
                    pcutsomLabel.ToPosition = dblXmax;
                    pcutsomLabel.Text = (n + 1).ToString();

                    this.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
                }
            }

            m_intTotalNSeries = this.pChart.Series.Count;

            if (chkAddMap.Checked)
            {
                //Calculate Value
                double[][] LocalESTbyLag = new double[m_intNFeature][];
                int[] absHLagValue = new int[m_intNFeature];
                bool[] aboveZero = new bool[m_intNFeature];

                for (int k = 0; k < m_intNFeature; k++)
                {
                    LocalESTbyLag[k] = new double[m_intMaxLag];
                    for (int l = 0; l < m_intMaxLag; l++)
                    {
                        LocalESTbyLag[k][l] = Math.Abs(m_lstCorrelograms[l].LocalMeasureEst[k]);
                    }
                    absHLagValue[k] =  System.Array.IndexOf(LocalESTbyLag[k], LocalESTbyLag[k].Max());
                    if (cboSAM.Text == "Local L")
                    {
                        if (m_lstCorrelograms[0].LocalMeasureEst[k] > 0)
                            aboveZero[k] = true;
                        else
                            aboveZero[k] = false;
                    }
                    else if (cboSAM.Text == "Local S")
                    {
                        if (m_SZeroZ[0] > 0)
                            aboveZero[k] = true;
                        else
                            aboveZero[k] = false;
                    }
                }

                
                //Add Fld to store Results
                string strHRangeFld = "Range";
                string strUpZero = "AbvZero";

                if (pFClass.FindField(strHRangeFld) == -1)
                {
                    Field newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strHRangeFld;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                    pFClass.AddField(newField);
                }

                if (pFClass.FindField(strUpZero) == -1)
                {
                    Field newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strUpZero;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                    pFClass.AddField(newField);
                }

                //Update Fields
                int intIdxRange = pFClass.FindField(strHRangeFld);
                int intIdxZero = pFClass.FindField(strUpZero);

                pFCursor = pFClass.Update(null, false);
                pFeature = pFCursor.NextFeature();

                int featureIdx = 0;
                bool blnZero = true;

                while (pFeature != null)
                {
                    pFeature.set_Value(intIdxRange, absHLagValue[featureIdx]);
                    blnZero = aboveZero[featureIdx];
                    if (blnZero)
                        pFeature.set_Value(intIdxZero, 1);
                    else
                        pFeature.set_Value(intIdxZero, 0);
                    
                    pFCursor.UpdateFeature(pFeature);

                    pFeature = pFCursor.NextFeature();
                    featureIdx++;
                }
                #region Add Map
                //Add Positive Map
                string strOutputLayerName = "Highest Range(+)";

                string strDefExp = strUpZero + " > 0";

                IRgbColor pColor1 = new RgbColor();
                IRgbColor pColor2 = new RgbColor();

                //Can Change the color in here!
                pColor1.Red = 165;
                pColor1.Green = 12;
                pColor1.Blue = 21;

                pColor2.Red = 254;
                pColor2.Green = 229;
                pColor2.Blue = 217;

                AddChoroplethMap(pFClass, strOutputLayerName, strDefExp, strHRangeFld, m_intMaxLag, pColor1, pColor2);

                //Add Negative Map
                strOutputLayerName = "Highest Range(-)";

                strDefExp = strUpZero + " = 0";

                pColor1 = new RgbColor();
                pColor2 = new RgbColor();

                //Can Change the color in here!
                pColor1.Red = 8;
                pColor1.Green = 81;
                pColor1.Blue = 156;

                pColor2.Red = 239;
                pColor2.Green = 243;
                pColor2.Blue = 255;

                AddChoroplethMap(pFClass, strOutputLayerName, strDefExp, strHRangeFld, m_intMaxLag, pColor1, pColor2);
 
                #endregion
            }

            
            pfrmProgress.Close();
        }

        private void cboSAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSAM.Text == "Local L" || cboSAM.Text == "Local S")
            {
                lblVar2.Enabled = true;
                chkNonDiag.Enabled = true;
                if (cboSAM.Text == "Local L")
                    cboFldName2.Enabled = true;
                else
                    cboFldName2.Enabled = false;
            }
            else
            {
                lblVar2.Enabled = false;
                cboFldName2.Enabled = false;
                chkNonDiag.Enabled = false;
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

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            ////Export the chart to an image file
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    conMenu.Show(pChart, e.X, e.Y);
            //    return;
            //}

            //Clear previous selection


            //Remove Previous Selection

            m_pActiveView.GraphicsContainer.DeleteAllElements();
            while (pChart.Series.Count != m_intTotalNSeries)
                pChart.Series.RemoveAt(pChart.Series.Count - 1);

            _canDraw = false;

            HitTestResult result = pChart.HitTest(e.X, e.Y);

            int intLastSeriesIdx = pChart.Series.Count - 1;
            //if (result.ChartElementType == ChartElementType.DataPoint)
            //{
            int dblOriPtsSize = pChart.Series[intLastSeriesIdx].MarkerSize;
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
            int intSelLinesCount = 0;
            for (int i = 0; i < pChart.Series[intLastSeriesIdx].Points.Count; i++)
            {
                double dblXChartValue = pChart.Series[intLastSeriesIdx].Points[i].XValue;
                double dblYChartValue = pChart.Series[intLastSeriesIdx].Points[i].YValues[0];
                int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(dblXChartValue);
                int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(dblYChartValue);

                System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);


                if (_rect.Contains(SelPts))
                {
                    int k = Convert.ToInt32((dblXChartValue - 1) / 2);
                    Correlogram pCorrelogram = m_lstCorrelograms[k];
                    int intValueIdx = pCorrelogram.LocalMeasureEst.FindIndex(x => x == dblYChartValue);

                    //int index = result.PointIndex;
                    seriesPts.Points.AddXY(dblXChartValue, dblYChartValue);

                    int r = m_pFIDs[intValueIdx];
                    //int v = Math.DivRem(i, m_intNFeature, out r);
                    //int k = Convert.ToInt32(Math.IEEERemainder(i, m_intMaxLag));
                    plotCommmand.Append(m_strOIDName + " = " + r.ToString() + " Or ");


                    DrawLineOnActiveView(intValueIdx, m_lstNeigbors[k].IDs[intValueIdx], m_arrXYCoord, m_pActiveView);

                    //Add Sel Lines
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

                    for (int l = 0; l < m_intMaxLag; l++)
                    {
                        int intXvalue = l * 2 + 1;
                        seriesLines.Points.AddXY(intXvalue, m_lstCorrelograms[l].LocalMeasureEst[intValueIdx]);
                    }

                }
            }

            //for (int i = 0; i < pChart.Series[intLastSeriesIdx - 1].Points.Count; i++)
            //{
            //    double dblXChartValue = pChart.Series[intLastSeriesIdx - 1].Points[i].XValue;
            //    double dblYChartValue = pChart.Series[intLastSeriesIdx - 1].Points[i].YValues[0];
            //    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(dblXChartValue);
            //    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(dblYChartValue);

            //    Point SelPts = new Point(intX, intY);

            //    if (_rect.Contains(SelPts))
            //    {
            //        int index = result.PointIndex;
            //        seriesPts.Points.AddXY(dblXChartValue, dblYChartValue);
            //        int k = (Convert.ToInt32(dblXChartValue) - 1) / 2;
            //        plotCommmand.Append("(" + strGroupFieldName + " = '" + uvList[k].ToString() + "' And " + strFieldName + " = " + dblYChartValue.ToString() + ") Or ");
            //    }
            //}
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
                m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
            //Brushing to other graphs //The Function should be locatated after MapView Brushing


            m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
        }

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
            IPoint FromP = new PointClass();
            FromP.X = arrXYCoord[intFromIdx, 0]; FromP.Y = arrXYCoord[intFromIdx, 1];

            int intArrLengthCnt = arrToLinks.Count;
            for (int i = 0; i < intArrLengthCnt; i++)
            {
                int intToIdx = arrToLinks[i] - 1;

                IPoint ToP = new PointClass();
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
        private void AddChoroplethMap(IFeatureClass pFClass, string strOutputLayerName, string strDefExp, string strHRangeFld, int intMaxLag, IRgbColor pColor1, IRgbColor pColor2)
        {
            IFeatureLayer pflOutput = new FeatureLayerClass();
            pflOutput.FeatureClass = pFClass;
            pflOutput.Name = strOutputLayerName;
            pflOutput.Visible = true;

            IFeatureLayerDefinition pFLayerDef = (IFeatureLayerDefinition)pflOutput;
            //pFLayerDef.DefinitionExpression = strUpZero + " > 0";
            pFLayerDef.DefinitionExpression = strDefExp;

            IGeoFeatureLayer pGeofeatureLayer;

            pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;

            //IQueryFilter pQFilter = new QueryFilterClass();
            //pQFilter.WhereClause = strUpZero + " > 0";

            //pGeofeatureLayer.SearchDisplayFeatures(pQFilter, false);

            ITable pTable = (ITable)pFClass;
            IClassifyGEN pClassifyGEN = new EqualIntervalClass();


            //Need to be changed 1/29/15
            ITableHistogram pTableHistogram = new BasicTableHistogramClass();
            pTableHistogram.Field = strHRangeFld;
            pTableHistogram.Table = pTable;
            IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram;

            object xVals, frqs;
            pHistogram.GetHistogram(out xVals, out frqs);
            pClassifyGEN.Classify(xVals, frqs, intMaxLag);

            ClassBreaksRenderer pRender = new ClassBreaksRenderer();
            double[] cb = (double[])pClassifyGEN.ClassBreaks;
            pRender.Field = strHRangeFld;
            pRender.BreakCount = intMaxLag;
            pRender.MinimumBreak = cb[0];

            //' create our color ramp
            IAlgorithmicColorRamp pColorRamp = new AlgorithmicColorRampClass();
            pColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;


            Boolean blnOK = true;
            pColorRamp.FromColor = pColor1;
            pColorRamp.ToColor = pColor2;
            pColorRamp.Size = intMaxLag;
            pColorRamp.CreateRamp(out blnOK);

            IEnumColors pEnumColors = pColorRamp.Colors;
            pEnumColors.Reset();

            //IRgbColor pColorOutline = new RgbColor();
            ////Can Change the color in here!
            //pColorOutline.Red = picGCLineColor.BackColor.R;
            //pColorOutline.Green = picGCLineColor.BackColor.G;
            //pColorOutline.Blue = picGCLineColor.BackColor.B;
            //double dblGCOutlineSize = Convert.ToDouble(nudGCLinewidth.Value);

            //ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
            //pOutLines.Width = dblGCOutlineSize;
            //pOutLines.Color = (IColor)pColorOutline;

            //' use this interface to set dialog properties
            IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pRender;
            pUIProperties.ColorRamp = "Custom";

            ISimpleFillSymbol pSimpleFillSym;
            //' be careful, indices are different for the diff lists
            for (int j = 0; j < intMaxLag; j++)
            {
                pRender.Break[j] = cb[j + 1];
                pRender.Label[j] = j.ToString();
                pUIProperties.LowBreak[j] = cb[j];
                pSimpleFillSym = new SimpleFillSymbolClass();
                pSimpleFillSym.Color = pEnumColors.Next();
                //pSimpleFillSym.Outline = pOutLines;
                pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
            }
            pGeofeatureLayer.Renderer = (IFeatureRenderer)pRender;
            m_pForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGeofeatureLayer);
        }
    }
}
