using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

using RDotNet;
using RDotNet.NativeLibrary;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisUncertainty
{
    public partial class frmQQplot : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private double[] m_adblVar1;
        private REngine m_pEngine;
        private string m_strVar1Name;
        private IFeatureLayer m_pTargetFLayer;
        private int m_intNFeatureCount;

        #region variables for Brushing and linking
        private int[] m_arrFID;
        private int[] m_arrSortedFID;
        #endregion
        public frmQQplot()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
                
                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }
                m_pSnippet = new clsSnippet();

                m_pEngine = m_pForm.pEngine;
                m_pEngine.Evaluate("library(MASS)");
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

                cboVariable1.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        cboVariable1.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnQQplot_Click(object sender, EventArgs e)
        {
            try
            {
                //string strTargetLayerName = cboTargetLayer.Text;
                //string strVar1Name = cboVariable1.Text;
                //int intTargetIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strTargetLayerName);
                //int intNFeatureCount = 0;

                //ILayer pTargetLayer = m_pForm.axMapControl1.get_Layer(intTargetIndex);

                //IFeatureLayer pTargetFLayer = (IFeatureLayer)pTargetLayer;

                ////Extract geometry from selected feature
                //IFeatureCursor pFCursor = null;
                //pFCursor = pTargetFLayer.Search(null, true);
                //intNFeatureCount = pTargetFLayer.FeatureClass.FeatureCount(null);
                //#region deprecated function
                ////Considering only all features 032317 HK
                ////IFeatureSelection pFeatureSelection = pTargetFLayer as IFeatureSelection;
                ////intNFeatureCount = pFeatureSelection.SelectionSet.Count;
                ////if (intNFeatureCount > 0 && chkUseSelected.Checked == true)
                ////{
                ////    ICursor pCursor = null;

                ////    pFeatureSelection.SelectionSet.Search(null, true, out pCursor);
                ////    pFCursor = (IFeatureCursor)pCursor;

                ////}
                ////else if (intNFeatureCount == 0 && chkUseSelected.Checked == true)
                ////{
                ////    MessageBox.Show("Select at least one feature");
                ////    return;
                ////}
                ////else
                ////{
                ////    pFCursor = pTargetFLayer.Search(null, true);
                ////    intNFeatureCount = pTargetFLayer.FeatureClass.FeatureCount(null);
                ////}
                //#endregion

                //IFeature pFeature = pFCursor.NextFeature();

                ////Source and Group Field Index
                //int intVar1Idx = pTargetFLayer.FeatureClass.Fields.FindField(strVar1Name);
                //int intFIDIdx = pTargetFLayer.FeatureClass.FindField(pTargetFLayer.FeatureClass.OIDFieldName);

                //m_adblVar1 = new double[intNFeatureCount];
                //m_arrFID = new int[intNFeatureCount];

                ////Feature Value to Array
                //int i = 0;
                //while (pFeature != null)
                //{
                //    m_adblVar1[i] = Convert.ToDouble(pFeature.get_Value(intVar1Idx));
                //    m_arrFID[i] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
                //    i++;
                //    pFeature = pFCursor.NextFeature();
                //}

                ////Draw Boxplot at the Form
                ////Array to R vector.
                //NumericVector vecVar1 = m_pEngine.CreateNumericVector(m_adblVar1);
                //m_pEngine.SetSymbol(strVar1Name, vecVar1);
                //IntegerVector vecFID = m_pEngine.CreateIntegerVector(m_arrFID);
                //m_pEngine.SetSymbol("FID", vecFID);

                //Sorting the variables
                m_pEngine.Evaluate("n <- length(" + m_strVar1Name + "); p <- ((1:n)-0.5)/(n)");
                m_pEngine.Evaluate("y <- quantile(" + m_strVar1Name + ", c(0.25, 0.75))");
                m_pEngine.Evaluate("sample.bind <- cbind(" + m_strVar1Name + ", FID)");
                Double[] dblSortedVariable = m_pEngine.Evaluate("sample.bind[order(sample.bind[,1]),1]").AsNumeric().ToArray();
                m_arrSortedFID = m_pEngine.Evaluate("sample.bind[order(sample.bind[,1]),2]").AsInteger().ToArray();

                string strTitle = null;

                Double[] dblTransformedX = null;
                double dblSlope = 0;
                double dblyInt = 0;

                switch (cboDistribution.Text)
                {
                    case "Normal":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='norm');");
                        strTitle = "Normal Quantile";
                        m_pEngine.Evaluate("q <- qnorm(p)");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                        m_pEngine.Evaluate("x <- qnorm(c(0.25, 0.75))");
                        
                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();
                       
                        //Calculate Low and upper bound
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        m_pEngine.Evaluate("SE<-(slope/dnorm(q))*sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");

                        break;
                    case "t":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='t', df=" + txtDf1.Text + ");");
                        strTitle = "t Quantile";
                        m_pEngine.Evaluate("q <- qt(p, df=" + txtDf1.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                        m_pEngine.Evaluate("x <- qt(c(0.25, 0.75), df=" + txtDf1.Text + ")");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        //pEngine.Evaluate("conf <- 0.95; zz<-qt(1-(1-conf)/2, df=" + txtDf1.Text + ")");
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        m_pEngine.Evaluate("SE<-(slope/dt(q, df=" + txtDf1.Text + "))*sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");

                        break;
                    case "F":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='f', df1=" + txtDf1.Text + ", df2=" + txtDf2.Text + ");");
                        strTitle = "F Quantile";
                        m_pEngine.Evaluate("q <- qf(p, df1=" + txtDf1.Text + ", df2=" + txtDf2.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                        m_pEngine.Evaluate("x <- qf(c(0.25, 0.75), df1=" + txtDf1.Text + ", df2=" + txtDf2.Text + ")");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        //pEngine.Evaluate("conf <- 0.95; zz<-qf(1-(1-conf)/2, df1=" + txtDf1.Text + ", df2=" + txtDf2.Text + ")");
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        m_pEngine.Evaluate("SE<-(slope/df(q, df1=" + txtDf1.Text + ", df2=" + txtDf2.Text + ")) *sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");


                        break;
                    case "Chi-square":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='chisq', df=" + txtDf1.Text + ");");
                        strTitle = "Chi square Quantile";
                        m_pEngine.Evaluate("q <- qchisq(p, df=" + txtDf1.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();

                        m_pEngine.Evaluate("x <- qchisq(c(0.25, 0.75), df=" + txtDf1.Text + ")");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        //pEngine.Evaluate("conf <- 0.95; zz<-qchisq(1-(1-conf)/2, df=" + txtDf1.Text + ")");
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        m_pEngine.Evaluate("SE<-(slope/dchisq(q, df=" + txtDf1.Text + "))*sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");

                        break;
                    case "Binomial":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='binom', size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ");");
                        strTitle = "Binomial Quantile";
                        m_pEngine.Evaluate("q <- qbinom(p, size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                        
                        m_pEngine.Evaluate("x <- qbinom(c(0.25, 0.75), size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ")");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        //pEngine.Evaluate("conf <- 0.95; zz<-qbinom(1-(1-conf)/2, size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ")");
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");

                        m_pEngine.Evaluate("SE<-(slope/dbinom(q, size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ")) *sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");

                        break;
                    case "Negative binomial":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='nbinom', size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ");");
                        strTitle = "Negative Binomial Quantile";
                        m_pEngine.Evaluate("q <- qnbinom(p, size=" + txtDf1.Text + ", mu=" + txtDf2.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();

                        m_pEngine.Evaluate("x <- qnbinom(c(0.25, 0.75), size=" + txtDf1.Text + ", mu=" + txtDf2.Text + ")");
                        
                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        //pEngine.Evaluate("conf <- 0.95; zz<-qnbinom(1-(1-conf)/2, size=" + txtDf1.Text + ", prob=" + txtDf2.Text + ")");
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        m_pEngine.Evaluate("SE<-(slope/dnbinom(q, size=" + txtDf1.Text + ", mu=" + txtDf2.Text + ")) *sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");


                        break;
                    case "Beta":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='beta', shape1=" + txtDf1.Text + ", shape2=" + txtDf2.Text + ");");
                        strTitle = "Beta Quantile";
                        m_pEngine.Evaluate("q <- qbeta(p, shape1=" + txtDf1.Text + ", shape2=" + txtDf2.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();

                        m_pEngine.Evaluate("x <- qbeta(c(0.25, 0.75), shape1=" + txtDf1.Text + ", shape2=" + txtDf2.Text + ")");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        //pEngine.Evaluate("conf <- 0.95; zz<-qbeta(1-(1-conf)/2,  shape1=" + txtDf1.Text + ", shape2=" + txtDf2.Text + ")");
                        m_pEngine.Evaluate("SE<-(slope/dbeta(q,  shape1=" + txtDf1.Text + ", shape2=" + txtDf2.Text + ")) *sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");

                        break;
                    case "Gamma":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='gamma', shape=" + txtDf1.Text + ");");
                        strTitle = "Gamma Quantile";
                        m_pEngine.Evaluate("q <- qgamma(p, shape=" + txtDf1.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                        m_pEngine.Evaluate("x <- qgamma(c(0.25, 0.75), shape=" + txtDf1.Text + ")");
                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        //pEngine.Evaluate("conf <- 0.95; zz<-qgamma(1-(1-conf)/2,  shape=" + txtDf1.Text + ")");
                        m_pEngine.Evaluate("SE<-(slope/dgamma(q,  shape=" + txtDf1.Text + ")) *sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");

                        break;
                    case "Poisson":
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='pois', lambda=" + txtDf1.Text + ");");
                        strTitle = "Poisson Quantile";
                        m_pEngine.Evaluate("q <- qpois(p, lambda=" + txtDf1.Text + ")");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();

                        m_pEngine.Evaluate("x <- qpois(c(0.25, 0.75), lambda=" + txtDf1.Text + ")");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                       // pEngine.Evaluate("conf <- 0.95; zz<-qpois(1-(1-conf)/2,  lambda=" + txtDf1.Text + ")");
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");

                        m_pEngine.Evaluate("SE<-(slope/dpois(q,  lambda=" + txtDf1.Text + ")) *sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");
                        break;
                    default:
                        //sbCommand.Append("qqPlot(" + strVar1Name + ", dist='norm');");
                        strTitle = "Normal Quantile";
                        m_pEngine.Evaluate("q <- qnorm(p)");
                        dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                        m_pEngine.Evaluate("x <- qnorm(c(0.25, 0.75))");

                        //Calculate Slope and y intercept
                        m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                        m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                        dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                        dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();

                        //Calculate Low and upper bound
                        m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                        m_pEngine.Evaluate("SE<-(slope/dnorm(q))*sqrt(p*(1-p)/n)");
                        m_pEngine.Evaluate("fit.value<-int+slope*q");
                        break;
                }
                //Calculate Slope and y intercept
                //pEngine.Evaluate("slope <- diff(y)/diff(x)");
                //double dblSlope = pEngine.Evaluate("diff(y)/diff(x)").AsNumeric().ToArray().First();
                //double dblyInt = pEngine.Evaluate("y[1L] - slope * x[1L]").AsNumeric().ToArray().First();
                //pSnippet.drawPlottoForm(strTitle, sbCommand.ToString());

                //Calculate Upper-lower bounds
                double[] arrUpper = m_pEngine.Evaluate("fit.value+zz*SE").AsNumeric().ToArray();
                double[] arrLower = m_pEngine.Evaluate("fit.value-zz*SE").AsNumeric().ToArray();

                //QQ Plot
                frmQQPlotResults pfrmQQPlotResults = new frmQQPlotResults();
                pfrmQQPlotResults.Text = "Quantile comparison plot of " + m_pTargetFLayer.Name;
                pfrmQQPlotResults.pChart.Series.Clear();
                System.Drawing.Color pMarkerColor = System.Drawing.Color.Blue;
                var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Points",
                    Color = pMarkerColor,
                    BorderColor = pMarkerColor,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle

                };

                pfrmQQPlotResults.pChart.Series.Add(seriesPts);

                //Feature Value to Array
                //int i = 0;
                for (int j = 0; j < m_intNFeatureCount; j++)
                {
                    //adblVar1[i] = Convert.ToDouble(pFeature.get_Value(intVar1Idx));
                    //adblVar2[i] = Convert.ToDouble(pFeature.get_Value(intVar2Idx));
                    //Add Pts
                    seriesPts.Points.AddXY(dblTransformedX[j], dblSortedVariable[j]);
                }

                pfrmQQPlotResults.pChart.ChartAreas[0].AxisX.Title = strTitle;
                pfrmQQPlotResults.pChart.ChartAreas[0].AxisY.Title = m_strVar1Name;

                //Add Line
                var seriesLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Line",
                    Color = System.Drawing.Color.Red,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };
                
                pfrmQQPlotResults.pChart.Series.Add(seriesLine);
                seriesLine.Points.AddXY(dblTransformedX.Min(), dblTransformedX.Min() * dblSlope + dblyInt);
                seriesLine.Points.AddXY(dblTransformedX.Max(), dblTransformedX.Max() * dblSlope + dblyInt);


                //Confidence bounds is not working properly HK 042817
                var upperLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "UpperLine",
                    Color = System.Drawing.Color.Red,
                    BorderDashStyle = ChartDashStyle.Dash,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };
                pfrmQQPlotResults.pChart.Series.Add(upperLine);

                var lowerLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "LowerLine",
                    Color = System.Drawing.Color.Red,
                    BorderDashStyle = ChartDashStyle.Dash,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };
                pfrmQQPlotResults.pChart.Series.Add(lowerLine);

                for (int j = 0; j < dblTransformedX.Length; j++)
                {
                    lowerLine.Points.AddXY(dblTransformedX[j], arrLower[j]);
                    upperLine.Points.AddXY(dblTransformedX[j], arrUpper[j]);

                }




                //pfrmTemp.dblBreaks = dblBreaks;
                //pfrmPtsPlot.intGraphType = 2;
                pfrmQQPlotResults.strDistribution = cboDistribution.Text;
                pfrmQQPlotResults.strDF1 = txtDf1.Text;
                pfrmQQPlotResults.strDF2 = txtDf2.Text;
                pfrmQQPlotResults.adblVar1 = dblSortedVariable;
                pfrmQQPlotResults.adblVar2 = dblTransformedX;
                pfrmQQPlotResults.strVar1Name = m_strVar1Name;
                pfrmQQPlotResults.m_pForm = m_pForm;
                pfrmQQPlotResults.m_pActiveView = m_pActiveView;
                pfrmQQPlotResults.m_pFLayer = m_pTargetFLayer;
                pfrmQQPlotResults.pMakerColor = pMarkerColor;
                //pfrmTemp.strFieldName = strFieldName;



                pfrmQQPlotResults.Show();
                //this.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

       
        private void cboDistribution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtDf1.Text = string.Empty;
                txtDf2.Text = string.Empty;

                if (cboDistribution.Text == "F") //Required initial value
                {
                    MessageBox.Show("The parameters for the distribution is not estimated. Please manually input the parameters");

                    lblDf1.Text = "df1=";
                    lblDf2.Text = "df2=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = true;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = true;
                    txtDf2.Visible = true;
                }
                else if (cboDistribution.Text == "Normal")
                {
                    lblDf1.Visible = false;
                    lblDf2.Visible = false;

                    txtDf1.Enabled = false;
                    txtDf1.Visible = false;
                    txtDf2.Enabled = false;
                    txtDf2.Visible = false;
                }
                else if (cboDistribution.Text == "Chi-square")//Required initial value
                {
                    MessageBox.Show("The parameters for the distribution is not estimated. Please manually input the parameters");

                    lblDf1.Text = "df=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = false;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = false;
                    txtDf2.Visible = false;
                }
                else if (cboDistribution.Text == "t")
                {
                    lblDf1.Text = "df=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = false;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = false;
                    txtDf2.Visible = false;

                    m_pEngine.Evaluate("sample.fit <- fitdistr(" + m_strVar1Name + ", 't')");
                    txtDf1.Text = m_pEngine.Evaluate("round(sample.fit$estimate[3], 5)").AsNumeric().First().ToString();
                }
                else if (cboDistribution.Text == "Binomial")//Fitdistr does not support Binomial distribution
                {
                    MessageBox.Show("The parameters for the distribution is not estimated. Please manually input the parameters");

                    lblDf1.Text = "size=";
                    lblDf2.Text = "prob=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = true;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = true;
                    txtDf2.Visible = true;
                }
                else if (cboDistribution.Text == "Negative binomial")
                {
                    lblDf1.Text = "size=";
                    lblDf2.Text = "mu=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = true;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = true;
                    txtDf2.Visible = true;
                    m_pEngine.Evaluate("sample.fit <- fitdistr(" + m_strVar1Name + ", 'negative binomial')");
                    txtDf1.Text = m_pEngine.Evaluate("round(sample.fit$estimate[1], 5)").AsNumeric().First().ToString();
                    txtDf2.Text = m_pEngine.Evaluate("round(sample.fit$estimate[2], 5)").AsNumeric().First().ToString();

                }
                else if (cboDistribution.Text == "Beta")//Required initial value
                {
                    MessageBox.Show("The parameters for the distribution is not estimated. Please manually input the parameters");

                    lblDf1.Text = "shape1=";
                    lblDf2.Text = "shape2=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = true;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = true;
                    txtDf2.Visible = true;
                }
                else if (cboDistribution.Text == "Gamma")
                {
                    lblDf1.Text = "shape=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = false;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = false;
                    txtDf2.Visible = false;

                    m_pEngine.Evaluate("sample.fit <- fitdistr(" + m_strVar1Name + ", 'gamma')");
                    txtDf1.Text = m_pEngine.Evaluate("round(sample.fit$estimate[1], 5)").AsNumeric().First().ToString();
                }
                else if (cboDistribution.Text == "Poisson")
                {
                    lblDf1.Text = "lambda=";

                    lblDf1.Visible = true;
                    lblDf2.Visible = false;

                    txtDf1.Enabled = true;
                    txtDf1.Visible = true;
                    txtDf2.Enabled = false;
                    txtDf2.Visible = false;
                    m_pEngine.Evaluate("sample.fit <- fitdistr(" + m_strVar1Name + ", 'Poisson')");
                    txtDf1.Text = m_pEngine.Evaluate("round(sample.fit$estimate[1], 5)").AsNumeric().First().ToString();

                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232832)
                    MessageBox.Show("The parameters for the distribution is not estimated. Please manually input the parameters");
                else
                {
                    frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                }
                return;
            }
        }

        private void cboVariable1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTargetLayerName = cboTargetLayer.Text;
            m_strVar1Name = cboVariable1.Text;
            int intTargetIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strTargetLayerName);
            m_intNFeatureCount = 0;

            ILayer pTargetLayer = m_pForm.axMapControl1.get_Layer(intTargetIndex);

            m_pTargetFLayer = (IFeatureLayer)pTargetLayer;

            //Extract geometry from selected feature
            IFeatureCursor pFCursor = null;
            pFCursor = m_pTargetFLayer.Search(null, true);
            m_intNFeatureCount = m_pTargetFLayer.FeatureClass.FeatureCount(null);

            IFeature pFeature = pFCursor.NextFeature();

            //Source and Group Field Index
            int intVar1Idx = m_pTargetFLayer.FeatureClass.Fields.FindField(m_strVar1Name);
            int intFIDIdx = m_pTargetFLayer.FeatureClass.FindField(m_pTargetFLayer.FeatureClass.OIDFieldName);

            m_adblVar1 = new double[m_intNFeatureCount];
            m_arrFID = new int[m_intNFeatureCount];

            //Feature Value to Array
            int i = 0;
            while (pFeature != null)
            {
                m_adblVar1[i] = Convert.ToDouble(pFeature.get_Value(intVar1Idx));
                m_arrFID[i] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
                i++;
                pFeature = pFCursor.NextFeature();
            }

            //Draw Boxplot at the Form
            //Array to R vector.
            NumericVector vecVar1 = m_pEngine.CreateNumericVector(m_adblVar1);
            m_pEngine.SetSymbol(m_strVar1Name, vecVar1);
            IntegerVector vecFID = m_pEngine.CreateIntegerVector(m_arrFID);
            m_pEngine.SetSymbol("FID", vecFID);
        }
    }
}