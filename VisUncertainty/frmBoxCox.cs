using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RDotNet;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisUncertainty
{
    public partial class frmBoxCox : Form
    {
        private clsSnippet m_pSnippet;
        private MainForm m_pForm;
        private REngine m_pEngine;
        private IActiveView m_pActiveView;

        private double[] m_arrValue;
        private double m_dblIinValue = 0;
        private double m_dblLogSum = 0;
        private double[] m_arrTrValue;
        private IFeatureClass m_pFClass;

        private double m_dblLambda;
        private double m_dblGamma;

        private string m_strFieldName;

        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        #region variables for Brushing and linking
        private int[] m_arrFID;
        private string m_strFIDNM;
        private clsBrusingLinking m_pBL;
        public IFeatureLayer m_pFLayer; //Brushing from others
        private int[] m_arrSortedFID;

        private Double[] m_vecMids;
        //Double[] vecCounts = m_pEngine.Evaluate("hist.sample$density*10").AsNumeric().ToArray();
        private Double[] m_vecCounts;
        private Double[] m_dblBreaks;

        private bool m_blnTransformed = false;
        #endregion

        public frmBoxCox()
        {
            InitializeComponent();
            m_pSnippet = new clsSnippet();
            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            m_pEngine = m_pForm.pEngine;
            m_pActiveView = m_pForm.axMapControl1.ActiveView;
            m_pBL = new clsBrusingLinking();

            for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
            {
                cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
            }

            //Loading R packages
            try
            {
                m_pEngine.Evaluate("library(MASS); library(geoR);library(car)");
            }
            catch
            {
                MessageBox.Show("This tool will not be working properly because the required R package for this tool is not installed.");
                return;
            }
        }

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strFieldName = txtSaveResult.Text;
                int intFieldIdx = m_pFClass.Fields.FindField(strFieldName);
                if (intFieldIdx == -1)
                {
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strFieldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    m_pFClass.AddField(newField);
                    intFieldIdx = m_pFClass.Fields.FindField(strFieldName);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to overwrite an existing field?", "Overwrite", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (m_pFClass.Fields.get_Field(intFieldIdx).Type != esriFieldType.esriFieldTypeDouble && m_pFClass.Fields.get_Field(intFieldIdx).Type != esriFieldType.esriFieldTypeSingle)
                        {
                            MessageBox.Show("Field type is not suitable, this function supports only 'DOUBLE' type.");
                            return;
                        }

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }

                IFeatureCursor pFCursor = m_pFClass.Update(null, true);
                int intNFeatureCount = m_pFClass.FeatureCount(null);

                if (intNFeatureCount == 0)
                    return;

                IFeature pFeature = pFCursor.NextFeature();

                int i = 0;
                while (pFeature != null)
                {
                    pFeature.set_Value(intFieldIdx, m_arrTrValue[i]);

                    pFCursor.UpdateFeature(pFeature);

                    pFeature = pFCursor.NextFeature();
                    i++;
                }

                MessageBox.Show("Complete");
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void trbLambda_MouseUp(object sender, MouseEventArgs e)
        {
            nudLambda.Value = trbLambda.Value * 2 / Convert.ToDecimal(100);
        }

        private void trbGamma_MouseUp(object sender, MouseEventArgs e)
        {
            if (trbGamma.Value == -100)
                trbGamma.Value = -99; //To avoid Error occured by zero value transformation
            else if (trbGamma.Value == 100)
                trbGamma.Value = 99; //To avoid Error occured by zero value transformation

            //nudGamma.Value = Convert.ToDecimal((Convert.ToDouble(trbGamma.Value) * m_dblIinValue) / 200);
            nudGamma.Value = nudGamma.Minimum + Convert.ToDecimal((Convert.ToDouble(trbGamma.Value + 100) * (2*m_dblIinValue)) / 200) ;
        }

        private void btnAddPlot_Click(object sender, EventArgs e)
        {
            try
            {

                NumericVector vecVar1 = m_pEngine.CreateNumericVector(m_arrValue);
                m_pEngine.SetSymbol(m_strFieldName, vecVar1);

                //Sorting the variables
                m_pEngine.Evaluate("n <- length(" + m_strFieldName + "); p <- ((1:n)-0.5)/(n)");
                m_pEngine.Evaluate("y <- quantile(" + m_strFieldName + ", c(0.25, 0.75))");
                Double[] dblSortedVariable = m_pEngine.Evaluate("sort(" + m_strFieldName + ")").AsNumeric().ToArray();
                //StringBuilder sbCommand = new StringBuilder();
                string strTitle = null;
                m_pEngine.Evaluate("library(car)");
                Double[] dblTransformedX = null;

                strTitle = "Normal Quantile";
                m_pEngine.Evaluate("q <- qnorm(p)");
                dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
                m_pEngine.Evaluate("x <- qnorm(c(0.25, 0.75))");

                //Calculate Slope and y intercept
                m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
                m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
                double dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
                double dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();
                //pSnippet.drawPlottoForm(strTitle, sbCommand.ToString());

                //Calculate Low and upper bound
                m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
                m_pEngine.Evaluate("SE<-(slope/dnorm(q))*sqrt(p*(1-p)/n)");
                m_pEngine.Evaluate("fit.value<-int+slope*q");

                double[] arrUpper = m_pEngine.Evaluate("fit.value+zz*SE").AsNumeric().ToArray();
                double[] arrLower = m_pEngine.Evaluate("fit.value-zz*SE").AsNumeric().ToArray();

                frmQQPlotResults pfrmQQPlotResults = new frmQQPlotResults();
                pfrmQQPlotResults.Text = "Quantile comparison plot of " + m_pFClass.AliasName;
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
                for (int j = 0; j < m_arrValue.Length; j++)
                {

                    seriesPts.Points.AddXY(dblTransformedX[j], dblSortedVariable[j]);
                }

                pfrmQQPlotResults.pChart.ChartAreas[0].AxisX.Title = strTitle;
                pfrmQQPlotResults.pChart.ChartAreas[0].AxisY.Title = m_strFieldName;

                //Add Lines
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

                for (int j = 0; j < m_arrValue.Length; j++)
                {
                    lowerLine.Points.AddXY(dblTransformedX[j], arrLower[j]);
                    upperLine.Points.AddXY(dblTransformedX[j], arrUpper[j]);

                }



                //pfrmQQPlotResults.strDistribution = cboDistribution.Text;
                //pfrmQQPlotResults.strDF1 = txtDf1.Text;
                //pfrmQQPlotResults.strDF2 = txtDf2.Text;
                pfrmQQPlotResults.adblVar1 = dblSortedVariable;
                pfrmQQPlotResults.adblVar2 = dblTransformedX;
                pfrmQQPlotResults.strVar1Name = m_strFieldName;
                pfrmQQPlotResults.m_pForm = m_pForm;
                pfrmQQPlotResults.m_pActiveView = m_pActiveView;
                IFeatureLayer pFLayer = new FeatureLayerClass();
                pFLayer.FeatureClass = m_pFClass;
                pfrmQQPlotResults.m_pFLayer = pFLayer;
                pfrmQQPlotResults.pMakerColor = pMarkerColor;
                //pfrmTemp.strFieldName = strFieldName;



                pfrmQQPlotResults.Show();
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

                if (intLIndex == -1)
                {
                    MessageBox.Show("Please select proper layer");
                    return;
                }

                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                IFields fields = m_pFClass.Fields;

                cboFieldName.Items.Clear();
                cboFieldName.Text = "";

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                }

                //Get FID for Brushing and linking
                IFeatureCursor pFCursor = m_pFLayer.Search(null, true);
                int intNFeatureCount = m_pFLayer.FeatureClass.FeatureCount(null);

                if (intNFeatureCount == 0)
                {
                    MessageBox.Show("There is no feature", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                m_strFIDNM = m_pFClass.OIDFieldName;
                IFeature pFeature = pFCursor.NextFeature();
                int intFIDIdx = m_pFClass.Fields.FindField(m_strFIDNM);
                int intRowIdx = 0;
                m_arrFID = new int[intNFeatureCount];
                while (pFeature != null)
                {
                    m_arrFID[intRowIdx] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
                    intRowIdx++;
                    pFeature = pFCursor.NextFeature();
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboFieldName.Text == "" || cboTargetLayer.Text == "")
                {
                    btnLog.Enabled = false;
                    btnTrans.Enabled = false;
                    chkGamma.Enabled = false;
                    return;
                }


                btnLog.Enabled = true;
                btnTrans.Enabled = true;
                chkGamma.Enabled = true;

                nudGamma.Value = 0;
                nudGamma.Maximum = 0;
                nudGamma.Minimum = 0;
                nudLambda.Value = 0;

                btnAddPlot.Enabled = false;

                grbPara.Enabled = false;
                grbSave.Enabled = false;

                if (chkGamma.Checked)
                {
                    trbGamma.Enabled = false;
                    nudGamma.Enabled = false;
                    nudGamma.ReadOnly = true;
                }

                m_strFieldName = cboFieldName.Text;
                string strLayerName = cboTargetLayer.Text;

                if (m_strFieldName.Length > 7)
                    txtSaveResult.Text = "tr_" + m_strFieldName.Substring(0, 7);
                else
                    txtSaveResult.Text = "tr_" + m_strFieldName;

                m_arrValue = GetArrayFromTable(strLayerName, m_strFieldName);
                DrawHist(m_arrValue, m_strFieldName, 0, 0);
                DrawQQPlot(m_arrValue, m_strFieldName, 0, 0);

                m_dblLogSum = CalLogSum(m_arrValue); // To calculate loglik

                if (m_arrValue.Min() <= 0)
                {
                    chkGamma.Checked = true;
                    chkGamma.Enabled = false;
                }
                else
                    chkGamma.Enabled = true;
                
                m_blnTransformed = false;


            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnTrans_Click(object sender, EventArgs e)
        {
            try{
            if (cboFieldName.Text == "" || cboTargetLayer.Text == "")
                return;
            if (m_arrValue == null)
                return;

            btnAddPlot.Enabled = true;

            grbPara.Enabled = true;
            grbSave.Enabled = true;

            if (chkGamma.Checked)
            {
                trbGamma.Enabled = true;
                nudGamma.Enabled = true;
                nudGamma.ReadOnly = false;
            }


            //string strFieldName = cboFieldName.Text;
            NumericVector vecValue = m_pEngine.CreateNumericVector(m_arrValue);
            m_pEngine.SetSymbol("bc.sample", vecValue);

            string strCommand = string.Empty;
            if (chkGamma.Checked)
                strCommand = "bc.par <- boxcoxfit(bc.sample, lambda2 = T)";
            else
                strCommand = "bc.par <- boxcoxfit(bc.sample)";

            m_pEngine.Evaluate(strCommand);

            double[] arrPara = m_pEngine.Evaluate("bc.par$lambda").AsNumeric().ToArray();

                //Assign manual value to avoid error.
                
                if (arrPara[0] < -2)
                {
                    arrPara[0] = -2;
                    MessageBox.Show("Fail to find an optimal lambda value. The value is below than -2.");
                }
                else if (arrPara[0] > 2)
                {
                    arrPara[0] = 2;
                    MessageBox.Show("Fail to find an optimal lambda value. The value is above than 2.");
                }

                if (chkGamma.Checked)
            {
                    //trbGamma.Enabled = true;


                    double dblMin = m_arrValue.Min();
                    double dblMax = m_arrValue.Max();
                    m_dblIinValue = (m_arrValue.Max() - m_arrValue.Min());

                    if (dblMin <= 0)
                    {
                        nudGamma.Minimum = Convert.ToDecimal(((-1) * dblMin) + 0.001);
                        nudGamma.Maximum = nudGamma.Minimum + Convert.ToDecimal(m_dblIinValue * 2);
                    }
                    else
                    {
                        if (dblMin - m_dblIinValue <= 0)
                        {
                            nudGamma.Minimum = Convert.ToDecimal(((-1) * dblMin) + 0.001);
                            nudGamma.Maximum = nudGamma.Minimum + Convert.ToDecimal(m_dblIinValue * 2);
                        }
                        else
                        {
                            nudGamma.Minimum = Convert.ToDecimal(-1*(m_dblIinValue));
                            nudGamma.Maximum = Convert.ToDecimal(m_dblIinValue);
                        }
                    }

                    //nudGamma.Maximum = Convert.ToDecimal(m_dblIinValue);
                    //nudGamma.Minimum = Convert.ToDecimal((-1) * m_dblIinValue);

                    //m_dblIinValue = Math.Abs(m_arrValue.Min()) + 0.00001 * (m_arrValue.Max() - m_arrValue.Min()); //Modified from this, 01/31/2019 HK

                    //trbGamma.Enabled = true;
                    //nudGamma.Maximum = Convert.ToDecimal(m_dblIinValue);
                    //nudGamma.Minimum = Convert.ToDecimal((-1) * m_dblIinValue)

                    //trbGamma.Value = Convert.ToInt32(arrPara[1] / m_dblIinValue * 200);
                    double dblnumGammaMin = Convert.ToDouble(nudGamma.Minimum);
                    trbGamma.Value = Convert.ToInt32((arrPara[1]- dblnumGammaMin) / (2*m_dblIinValue) * 200)-100;
                    nudGamma.Value = Convert.ToDecimal(arrPara[1]);
                nudGamma.Increment = Convert.ToDecimal(m_dblIinValue / Convert.ToDouble(100));

                nudLambda.Value = Convert.ToDecimal(arrPara[0]);
                //trbLambda.Value = Convert.ToInt32(arrPara[0] * 100); // Value are represented as percentage
                    trbLambda.Value = Convert.ToInt32(arrPara[0]/2 * 100); // Value are represented as percentage
                }
            else
            {
                trbGamma.Enabled = false;
                nudLambda.Value = Convert.ToDecimal(arrPara[0]);
                trbLambda.Value = Convert.ToInt32(arrPara[0]/2 * 100);
            }

            if (chkGamma.Checked)
            {
                m_arrTrValue = BoxCoxTransformation(m_arrValue, arrPara[0], arrPara[1]);
                DrawHist(m_arrTrValue, "transformed", Math.Round(arrPara[0], 2), arrPara[1]);
                DrawQQPlot(m_arrTrValue, "transformed", Math.Round(arrPara[0], 2), arrPara[1]);
            }
            else
            {
                m_arrTrValue = BoxCoxTransformation(m_arrValue, arrPara[0], 0);
                DrawHist(m_arrTrValue, "transformed", Math.Round(arrPara[0], 2), 0);
                DrawQQPlot(m_arrTrValue, "transformed", Math.Round(arrPara[0], 2), 0);
            }

                //Update SW
                NumericVector vecTrValue = m_pEngine.CreateNumericVector(m_arrTrValue);
                m_pEngine.SetSymbol("bc.result", vecTrValue);

                double dblProbSW = m_pEngine.Evaluate("shapiro.test(bc.result)$statistic").AsNumeric().First();
                txtSW.Text = Math.Round(dblProbSW, 5).ToString();
                m_blnTransformed = true;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void nudLambda_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                trbLambda.Value = Convert.ToInt32(nudLambda.Value/2 * 100);

                RedrawHist();
                RedrawQQPlot();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void nudGamma_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //trbGamma.Value = Convert.ToInt32((arrPara[1] - dblnumGammaMin) / m_dblIinValue * 200) - 100;
                //nudGamma.Value = nudGamma.Minimum + Convert.ToDecimal((Convert.ToDouble(trbGamma.Value + 100) * m_dblIinValue) / 200);

                trbGamma.Value = Convert.ToInt32(Convert.ToDouble(nudGamma.Value - nudGamma.Minimum) / (2*m_dblIinValue) * 200) - 100;
            //trbGamma.Value = Convert.ToInt32(Convert.ToDouble(nudGamma.Value * 200) / (m_dblIinValue));
                RedrawHist();
                RedrawQQPlot();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        #endregion

        #region Private Functions
        private double[] GetArrayFromTable(string strLayerName, string strFieldName)
        {
            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            if (intLIndex == -1)
                return null;

            int intNFeatureCount = 0;

            ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);
            IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

            IFeatureCursor pFCursor = null;
            //Consider all features 
            //IFeatureSelection pFeatureSelection = pFLayer as IFeatureSelection;
            //intNFeatureCount = pFeatureSelection.SelectionSet.Count;

            pFCursor = pFLayer.Search(null, true);
            intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);

            if (intNFeatureCount == 0)
                return null;

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
            return arrValue;
        }

        private void DrawQQPlot(double[] adblVar1, string strVar1Name, double dblLambda, double dblGamma)
        {
            //This function directly changes FID order with ordering of the selected variable
            NumericVector vecVar1 = m_pEngine.CreateNumericVector(adblVar1);
            m_pEngine.SetSymbol(strVar1Name, vecVar1);
            IntegerVector vecFID = m_pEngine.CreateIntegerVector(m_arrFID);
            m_pEngine.SetSymbol("FID", vecFID);

            //Sorting the variables
            m_pEngine.Evaluate("n <- length(" + strVar1Name + "); p <- ((1:n)-0.5)/(n)");
            m_pEngine.Evaluate("y <- quantile(" + strVar1Name + ", c(0.25, 0.75))");
            m_pEngine.Evaluate("sample.bind <- cbind(" + strVar1Name + ", FID)");
            Double[] dblSortedVariable = m_pEngine.Evaluate("sample.bind[order(sample.bind[,1]),1]").AsNumeric().ToArray();
            m_arrSortedFID = m_pEngine.Evaluate("sample.bind[order(sample.bind[,1]),2]").AsInteger().ToArray();

            //Double[] dblSortedVariable = m_pEngine.Evaluate("sort(" + strVar1Name + ")").AsNumeric().ToArray();
            //StringBuilder sbCommand = new StringBuilder();
            string strTitle = null;
            m_pEngine.Evaluate("library(car)");
            Double[] dblTransformedX = null;

            strTitle = "Normal Quantile";
            m_pEngine.Evaluate("q <- qnorm(p)");
            dblTransformedX = m_pEngine.Evaluate("q").AsNumeric().ToArray();
            m_pEngine.Evaluate("x <- qnorm(c(0.25, 0.75))");

            //Calculate Slope and y intercept
            m_pEngine.Evaluate("slope <- diff(y)/diff(x)");
            m_pEngine.Evaluate("int <- y[1L] - slope * x[1L]");
            double dblSlope = m_pEngine.Evaluate("slope").AsNumeric().ToArray().First();
            double dblyInt = m_pEngine.Evaluate("int").AsNumeric().ToArray().First();
            //pSnippet.drawPlottoForm(strTitle, sbCommand.ToString());

            //Calculate Low and upper bound
            m_pEngine.Evaluate("conf <- 0.95; zz<-qnorm(1-(1-conf)/2)");
            m_pEngine.Evaluate("SE<-(slope/dnorm(q))*sqrt(p*(1-p)/n)");
            m_pEngine.Evaluate("fit.value<-int+slope*q");

            double[] arrUpper = m_pEngine.Evaluate("fit.value+zz*SE").AsNumeric().ToArray();
            double[] arrLower = m_pEngine.Evaluate("fit.value-zz*SE").AsNumeric().ToArray();
            
            //Scatter Plot
            pQQPlot.Series.Clear();
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

            pQQPlot.Series.Add(seriesPts);

            for (int j = 0; j < adblVar1.Length; j++)
            {
                //adblVar1[i] = Convert.ToDouble(pFeature.get_Value(intVar1Idx));
                //adblVar2[i] = Convert.ToDouble(pFeature.get_Value(intVar2Idx));
                //Add Pts
                seriesPts.Points.AddXY(dblTransformedX[j], dblSortedVariable[j]);
            }

            //Add Trend Line
            var seriesLine = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Trend Line",
                Color = System.Drawing.Color.Black,
                //BorderColor = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
            };
            pQQPlot.Series.Add(seriesLine);
            seriesLine.Points.AddXY(dblTransformedX.Min(), dblTransformedX.Min() * dblSlope + dblyInt);
            seriesLine.Points.AddXY(dblTransformedX.Max(), dblTransformedX.Max() * dblSlope + dblyInt);

            //Add Line
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
            pQQPlot.Series.Add(upperLine);

            //Add Line
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
            pQQPlot.Series.Add(lowerLine);

            for (int j = 0; j < adblVar1.Length; j++)
            {
                lowerLine.Points.AddXY(dblTransformedX[j], arrLower[j]);
                upperLine.Points.AddXY(dblTransformedX[j], arrUpper[j]);

            }

            string strYTilte = string.Empty;
            if (dblGamma == 0 && dblLambda == 0)
                strYTilte = strVar1Name;
            else if (dblLambda != 0 && dblGamma == 0)
                strYTilte = strVar1Name + "(Lambda: " + dblLambda.ToString() + ")";
            else
                strYTilte = strVar1Name + "(Lambda: " + dblLambda.ToString() + ", Gamma: " + Math.Round(dblGamma, 5).ToString() + ")";

            pQQPlot.ChartAreas[0].AxisX.Title = strTitle;
            pQQPlot.ChartAreas[0].AxisY.Title = strYTilte;

            double dblSpacing = 0, dblMaxValue = dblSortedVariable.Max(), dblMinValue = dblSortedVariable.Min();
            dblSpacing = (dblMaxValue - dblMinValue) / 20;
            pQQPlot.ChartAreas[0].AxisY.Maximum = dblMaxValue + dblSpacing;
            pQQPlot.ChartAreas[0].AxisY.Minimum = dblMinValue - dblSpacing;
            pQQPlot.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            pQQPlot.ChartAreas[0].AxisX.CustomLabels.Clear();

            //For presentation
            pQQPlot.ChartAreas[0].AxisY.LabelStyle.Format = "#.###";

            for (int n = 0; n < 5; n++)
            {
                double dblValue = n - 2; 
                CustomLabel pcutsomLabel = new CustomLabel();
                pcutsomLabel.FromPosition = dblValue - 1;
                pcutsomLabel.ToPosition = dblValue + 1;
                pcutsomLabel.Text = dblValue.ToString();

                pQQPlot.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }
        }
        private void DrawHist(double[] arrValue, string strFieldName, double dblLambda, double dblGamma)
        {
            //This function determined value of VecMids, VecCounts, dblBraks
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            ////////////
            NumericVector vecValue = m_pEngine.CreateNumericVector(arrValue);
            m_pEngine.SetSymbol(strFieldName, vecValue);
            //m_pEngine.Evaluate("hist.sample <- hist(" + strFieldName + ", plot = FALSE, freq = FALSE)");
            m_pEngine.Evaluate("hist.sample <- hist(" + strFieldName + ", plot = FALSE)");
            this.pHistogram.Series.Clear();
            this.pHistogram.ChartAreas[0].AxisX.CustomLabels.Clear();


            m_vecMids = m_pEngine.Evaluate("hist.sample$mids").AsNumeric().ToArray();
            //Double[] vecCounts = m_pEngine.Evaluate("hist.sample$density*10").AsNumeric().ToArray();
            m_vecCounts = m_pEngine.Evaluate("hist.sample$counts").AsNumeric().ToArray();
            m_dblBreaks = m_pEngine.Evaluate("hist.sample$breaks").AsNumeric().ToArray();

            //watch.Stop();
            //double dblTime = watch.ElapsedMilliseconds;

            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.White,
                BorderColor = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Column,

            };

            this.pHistogram.Series.Add(series1);

            int intNBins = m_vecMids.Length;

            for (int j = 0; j < intNBins; j++)
            {
                series1.Points.AddXY(m_vecMids[j], m_vecCounts[j]);
            }

            string strXTilte = string.Empty;
            if (dblGamma == 0 && dblLambda == 0)
                strXTilte = strFieldName;
            else if (dblLambda != 0 && dblGamma == 0)
                strXTilte = strFieldName + "(Lambda: " + dblLambda.ToString() + ")";
            else
                strXTilte = strFieldName + "(Lambda: " + dblLambda.ToString() + ", Gamma: " + Math.Round(dblGamma, 5).ToString() + ")";

            this.pHistogram.Series[0]["PointWidth"] = "1";
            this.pHistogram.ChartAreas[0].AxisX.Title = strXTilte;
            this.pHistogram.ChartAreas[0].AxisY.Title = "Frequency";

            

            this.pHistogram.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            double dblInterval = m_dblBreaks[1] - m_dblBreaks[0];
            for (int n = 0; n < m_dblBreaks.Length; n++)
            {
                CustomLabel pcutsomLabel = new CustomLabel();
                pcutsomLabel.FromPosition = n;
                pcutsomLabel.ToPosition = n + 1;
                pcutsomLabel.Text = m_dblBreaks[n].ToString();

                this.pHistogram.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }

        }

        private double[] BoxCoxTransformation(double[] arrValue, double dblLambda, double dblGamma)
        {
            double[] arrResult = new double[arrValue.Length];

            if (dblGamma == null)
                dblGamma = 0;

            int i = 0;
            if (dblLambda == 0)
            {
                foreach (double dblValue in arrValue)
                {
                    arrResult[i] = Math.Log(dblValue + dblGamma);
                    i++;
                }

            }
            else
            {
                foreach (double dblValue in arrValue)
                {
                    arrResult[i] = (Math.Pow(dblValue + dblGamma, dblLambda) - 1) / dblLambda;
                    i++;
                }
            }

            return arrResult;

        }

        private void RedrawHist() // Update Transformed Array and LogLik in this function
        {
            m_dblLambda = 0;
            m_dblGamma = 0;
            m_dblLambda = Convert.ToDouble(nudLambda.Value);
            //txtLambda.Text = dblLambda.ToString();

            if (chkGamma.Checked)
            {
                m_dblGamma = Convert.ToDouble(nudGamma.Value);
                //dblGamma = (Convert.ToDouble(trbGamma.Value) / 200) * m_dblIinValue;
                //txtGamma.Text = dblGamma.ToString();
            }

            //if (dblGamma == 0 && dblLambda == 0)
            //    return;

            //Update Transformed Array
            if (chkGamma.Checked)
                m_arrTrValue = BoxCoxTransformation(m_arrValue, m_dblLambda, m_dblGamma);
            else
                m_arrTrValue = BoxCoxTransformation(m_arrValue, m_dblLambda, m_dblGamma);

            //Update SW
            NumericVector vecTrValue = m_pEngine.CreateNumericVector(m_arrTrValue);
            m_pEngine.SetSymbol("bc.result", vecTrValue);

            try
            {
                double dblProbSW = m_pEngine.Evaluate("shapiro.test(bc.result)$statistic").AsNumeric().First();
                txtSW.Text = Math.Round(dblProbSW, 5).ToString();
            }
            catch
            {
                MessageBox.Show("Fail to calculate due to rounding error. Please try with other values");
                return;
            }


            if (m_dblLambda == 0)
                DrawHist(m_arrTrValue, "log_transformed", Math.Round(m_dblLambda, 2), Math.Round(m_dblGamma, 2));
            else
                DrawHist(m_arrTrValue, "transformed", Math.Round(m_dblLambda, 2), Math.Round(m_dblGamma, 2));

        }
        private void RedrawQQPlot()
        {
            m_dblLambda = 0;
            m_dblGamma = 0;
            m_dblLambda = Convert.ToDouble(nudLambda.Value);

            if (chkGamma.Checked)
            {
                m_dblGamma = Convert.ToDouble(nudGamma.Value);
            }
            m_arrTrValue = BoxCoxTransformation(m_arrValue, m_dblLambda, m_dblGamma);

            if(m_dblLambda ==0)
                DrawQQPlot(m_arrTrValue, "log_transformed", Math.Round(m_dblLambda, 2), Math.Round(m_dblGamma, 2));
            else
                DrawQQPlot(m_arrTrValue, "transformed", Math.Round(m_dblLambda, 2), Math.Round(m_dblGamma, 2));

        }
        private double CalVarfromArray(double[] arrSubset)
        {
            double average = arrSubset.Average();
            double sumOfSquaresOfDifferences = arrSubset.Select(val => (val - average) * (val - average)).Sum();
            double dblVar = sumOfSquaresOfDifferences / arrSubset.Length;

            return dblVar;
        }
        private double CalLogSum(double[] arrVal)
        {
            double dblLogSum = arrVal.Select(val => Math.Log(val)).Sum();
            return dblLogSum;
        }
        private double CalLoglik(double[] arrTrValue, double dblLambda)
        {
            double dblMeanTrValue = arrTrValue.Average();
            double dblSigmasq = CalVarfromArray(arrTrValue);
            double dblLength = arrTrValue.Length;
            double dblLoglik = (-1 * (dblLength / 2) * (Math.Log(2 * Math.PI) + Math.Log(dblSigmasq) + 1)) + (dblLambda - 1) * m_dblLogSum;

            return dblLoglik;
        }
        #endregion

        #region Brushing and Linking Functions
        private void pQQPlot_MouseDown(object sender, MouseEventArgs e)
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

        private void pQQPlot_MouseMove(object sender, MouseEventArgs e)
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
                pGraphics = pQQPlot.CreateGraphics();
                pGraphics.DrawRectangle(pen, _rect);
                pGraphics.Dispose();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void pQQPlot_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //Export the chart to an image file
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    conMenu.Show(pQQPlot, e.X, e.Y);
                    return;
                }

                //Clear previous selection
                int intLastSeriesIdx = pQQPlot.Series.Count - 1;

                //Remove Previous Selection
                if (pQQPlot.Series[intLastSeriesIdx].Name == "SelPoints")
                    pQQPlot.Series.RemoveAt(intLastSeriesIdx);


                HitTestResult result = pQQPlot.HitTest(e.X, e.Y);

                int dblOriPtsSize = pQQPlot.Series[0].MarkerSize;
                _canDraw = false;

                Color pMarkerColor = System.Drawing.Color.Cyan;
                var seriesPts = new Series
                {
                    Name = "SelPoints",
                    Color = pMarkerColor,
                    BorderColor = pMarkerColor,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = dblOriPtsSize * 2

                };

                pQQPlot.Series.Add(seriesPts);

                StringBuilder plotCommmand = new StringBuilder();

                for (int i = 0; i < pQQPlot.Series[0].Points.Count; i++)
                {
                    int intX = (int)pQQPlot.ChartAreas[0].AxisX.ValueToPixelPosition(pQQPlot.Series[0].Points[i].XValue);
                    int intY = (int)pQQPlot.ChartAreas[0].AxisY.ValueToPixelPosition(pQQPlot.Series[0].Points[i].YValues[0]);

                    Point SelPts = new Point(intX, intY);

                    if (_rect.Contains(SelPts))
                    {
                        int index = result.PointIndex;
                        seriesPts.Points.AddXY(pQQPlot.Series[0].Points[i].XValue, pQQPlot.Series[0].Points[i].YValues[0]);
                        //plotCommmand.Append("(" + strVarNM + " = " + arrVar[i].ToString() + ") Or ");
                        plotCommmand.Append("(" + m_strFIDNM + " = " + m_arrSortedFID[i].ToString() + ") Or ");
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
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                }
                //Brushing to other graphs //The Function should be locatated after MapView Brushing

                m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
                BrushingToHistogram(m_pFLayer);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        public void BrushingToHistogram(IFeatureLayer pFLayer)
        {
            //try
            //{
            IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;
            int intVarIdx = pFLayer.FeatureClass.FindField(cboFieldName.Text);

            ICursor pCursor = null;
            featureSelection.SelectionSet.Search(null, false, out pCursor);
            IRow pRow = pCursor.NextRow();

            var series2 = new Series
            {
                Name = "SelSeries",
                Color = System.Drawing.Color.Red,
                BorderColor = System.Drawing.Color.Black,
                BackHatchStyle = ChartHatchStyle.DiagonalCross,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.StackedColumn,
            };

            var series1 = new Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.White,
                BorderColor = System.Drawing.Color.Black,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.StackedColumn,

            };

            var checkDup1 = pHistogram.Series.FindByName("Series1");
            if (checkDup1 != null)
            {
                if (pHistogram.Series.Count == 1)
                    pHistogram.Series.RemoveAt(0);
                else
                    pHistogram.Series.RemoveAt(1);
            }

            var checkDup2 = pHistogram.Series.FindByName("SelSeries");
            if (checkDup2 != null)
                pHistogram.Series.RemoveAt(0);

            pHistogram.Series.Add(series2);
            pHistogram.Series.Add(series1);

            Double[] vecMids = m_vecMids;
            Double[] dblBreaks = m_dblBreaks;
            Double[] vecCounts = m_vecCounts;

            int intNBins = m_vecMids.Length;
            int[] intFrequencies = new int[intNBins];

            while (pRow != null)
            {
                double dblValue = Convert.ToDouble(pRow.get_Value(intVarIdx));
                double dblTrValue = dblValue;
                if(m_blnTransformed)
                    dblTrValue = IndiBCTrans(dblValue, m_dblLambda, m_dblLambda);

                for (int j = 0; j < intNBins; j++)
                {
                    if (dblTrValue > dblBreaks[j] && dblTrValue <= dblBreaks[j + 1])
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

            pHistogram.Series[1]["PointWidth"] = "1";
            pHistogram.Series[0]["PointWidth"] = "1";

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Exception:" + ex.Message);
            //    return;
            //}
        }

        
        private void pHistogram_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //Export the chart to an image file
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    conMenu.Show(pHistogram, e.X, e.Y);
                    return;
                }

                HitTestResult result = pHistogram.HitTest(e.X, e.Y);

                //Remove Previous Selection
                if (pHistogram.Series.Count == 2)
                {
                    pHistogram.Series.RemoveAt(1);
                    pHistogram.Series.RemoveAt(0);

                    var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "Series1",
                        Color = System.Drawing.Color.White,
                        BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Column,

                    };

                    pHistogram.Series.Add(series1);

                    int intNBins = m_vecMids.Length;

                    for (int j = 0; j < intNBins; j++)
                    {
                        series1.Points.AddXY(m_vecMids[j], m_vecCounts[j]);
                    }
                    pHistogram.Series[0]["PointWidth"] = "1";
                }

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    //Brushing on histogram
                    int index = result.PointIndex;

                    for (int i = 0; i < pHistogram.Series[0].Points.Count; i++)
                    {
                        if (i == index)
                            pHistogram.Series[0].Points[i].Color = Color.Cyan;
                        else
                            pHistogram.Series[0].Points[i].Color = Color.White;
                    }


                    string whereClause = "";

                    if (m_blnTransformed)
                    {
                        double dblLowLimit = InvBCTrans(m_dblBreaks[index], m_dblLambda, m_dblGamma);
                        double dblUpperLimit = InvBCTrans(m_dblBreaks[index + 1], m_dblLambda, m_dblGamma);
                        whereClause = m_strFieldName + " > " + dblLowLimit.ToString() + " And " + m_strFieldName + " <= " + dblUpperLimit.ToString();
                    }
                    else
                        whereClause = m_strFieldName + " > " + m_dblBreaks[index].ToString() + " And " + m_strFieldName + " <= " + m_dblBreaks[index + 1].ToString();

                    //Brushing to ActiveView
                    m_pBL.FeatureSelectionOnActiveView(whereClause, m_pActiveView, m_pFLayer);

                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
                    BrushingToQQPlot(m_pFLayer);
                }
                else
                {
                    //Clear Selection Both Histogram and ActiveView
                    for (int i = 0; i < pHistogram.Series[0].Points.Count; i++)
                    {
                        pHistogram.Series[0].Points[i].Color = Color.White;
                    }
                    IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
                    BrushingToQQPlot(m_pFLayer);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
}
        public void BrushingToQQPlot(IFeatureLayer pFLayer)
        {
            IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;
            IFeatureClass pFeatureClass = pFLayer.FeatureClass;
            int intVar1Idx = pFeatureClass.Fields.FindField(m_strFIDNM);
            //string strVar1Name = pfrmPointsPlot.strVar1Name;

            ICursor pCursor = null;
            featureSelection.SelectionSet.Search(null, false, out pCursor);

            IRow pRow = pCursor.NextRow();

            int dblOriPtsSize = pQQPlot.Series[0].MarkerSize;

            Color pMarkerColor = Color.Red;
            var seriesPts = new Series
            {
                Name = "SelPoints",
                Color = pMarkerColor,
                BorderColor = pMarkerColor,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = dblOriPtsSize * 2

            };

            var checkDup = pQQPlot.Series.FindByName("SelPoints");
            if (checkDup != null)
                pQQPlot.Series.RemoveAt(pQQPlot.Series.Count-1);

            pQQPlot.Series.Add(seriesPts);
            int intNfeature = featureSelection.SelectionSet.Count;
            //int i = 0;
            //Double[] adblVar1 = new Double[intNfeature];

            while (pRow != null)
            {
                for (int j = 0; j < m_arrSortedFID.Length; j++)
                {
                    if (m_arrSortedFID[j] == Convert.ToInt32(pRow.get_Value(intVar1Idx)))
                        seriesPts.Points.AddXY(pQQPlot.Series[0].Points[j].XValue, pQQPlot.Series[0].Points[j].YValues[0]);
                }
                pRow = pCursor.NextRow();
            }

        }
        private double IndiBCTrans(double dblValue, double dblLambda, double dblGamma)
        {
            double dblResult = 0;
            
            if (dblLambda == 0)
                dblResult = Math.Log(dblValue + dblGamma);
            else
                dblResult = (Math.Pow(dblValue + dblGamma, dblLambda) - 1) / dblLambda;

            return dblResult;
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboFieldName.Text == "" || cboTargetLayer.Text == "")
                    return;
                if (m_arrValue == null)
                    return;

                btnAddPlot.Enabled = true;

                grbPara.Enabled = true;
                grbSave.Enabled = true;

                if (chkGamma.Checked)
                {
                    trbGamma.Enabled = true;
                    nudGamma.Enabled = true;
                    nudGamma.ReadOnly = false;
                }


                //string strFieldName = cboFieldName.Text;
                NumericVector vecValue = m_pEngine.CreateNumericVector(m_arrValue);
                m_pEngine.SetSymbol("bc.sample", vecValue);

                string strCommand = string.Empty;
                if (chkGamma.Checked)
                    strCommand = "bc.par <- boxcoxfit(bc.sample, lambda2 = T)";
                else
                    strCommand = "bc.par <- boxcoxfit(bc.sample)";

                m_pEngine.Evaluate(strCommand);

                double[] arrPara = m_pEngine.Evaluate("bc.par$lambda").AsNumeric().ToArray();


                if (chkGamma.Checked)
                {
                    //trbGamma.Enabled = true;


                    double dblMin = m_arrValue.Min();
                    double dblMax = m_arrValue.Max();
                    m_dblIinValue = (m_arrValue.Max() - m_arrValue.Min());

                    if (dblMin <= 0)
                    {
                        nudGamma.Minimum = Convert.ToDecimal(((-1) * dblMin) + 0.001);
                        nudGamma.Maximum = nudGamma.Minimum + Convert.ToDecimal(m_dblIinValue * 2);
                    }
                    else
                    {
                        if (dblMin - m_dblIinValue <= 0)
                        {
                            nudGamma.Minimum = Convert.ToDecimal(((-1) * dblMin) + 0.001);
                            nudGamma.Maximum = nudGamma.Minimum + Convert.ToDecimal(m_dblIinValue * 2);
                        }
                        else
                        {
                            nudGamma.Minimum = Convert.ToDecimal(-1 * (m_dblIinValue));
                            nudGamma.Maximum = Convert.ToDecimal(m_dblIinValue);
                        }
                    }

                    //nudGamma.Maximum = Convert.ToDecimal(m_dblIinValue);
                    //nudGamma.Minimum = Convert.ToDecimal((-1) * m_dblIinValue);

                    //m_dblIinValue = Math.Abs(m_arrValue.Min()) + 0.00001 * (m_arrValue.Max() - m_arrValue.Min()); //Modified from this, 01/31/2019 HK

                    //trbGamma.Enabled = true;
                    //nudGamma.Maximum = Convert.ToDecimal(m_dblIinValue);
                    //nudGamma.Minimum = Convert.ToDecimal((-1) * m_dblIinValue)

                    //trbGamma.Value = Convert.ToInt32(arrPara[1] / m_dblIinValue * 200);
                    double dblnumGammaMin = Convert.ToDouble(nudGamma.Minimum);
                    trbGamma.Value = Convert.ToInt32((arrPara[1] - dblnumGammaMin) / (2 * m_dblIinValue) * 200) - 100;
                    nudGamma.Value = Convert.ToDecimal(arrPara[1]);
                    nudGamma.Increment = Convert.ToDecimal(m_dblIinValue / Convert.ToDouble(100));

                    nudLambda.Value = 0;
                    //trbLambda.Value = Convert.ToInt32(arrPara[0] * 100); // Value are represented as percentage
                    trbLambda.Value = Convert.ToInt32(arrPara[0] / 2 * 100); // Value are represented as percentage
                }
                else
                {
                    trbGamma.Enabled = false;
                    nudLambda.Value = 0;
                    trbLambda.Value = Convert.ToInt32(arrPara[0] / 2 * 100);
                }

                if (chkGamma.Checked)
                {
                    m_arrTrValue = BoxCoxTransformation(m_arrValue, 0, arrPara[1]);
                    DrawHist(m_arrTrValue, "transformed", 0, arrPara[1]);
                    DrawQQPlot(m_arrTrValue, "transformed", 0, arrPara[1]);
                }
                else
                {
                    m_arrTrValue = BoxCoxTransformation(m_arrValue, 0, 0);
                    DrawHist(m_arrTrValue, "transformed", 0, 0);
                    DrawQQPlot(m_arrTrValue, "transformed", 0, 0);
                }

                //Update SW
                NumericVector vecTrValue = m_pEngine.CreateNumericVector(m_arrTrValue);
                m_pEngine.SetSymbol("bc.result", vecTrValue);

                double dblProbSW = m_pEngine.Evaluate("shapiro.test(bc.result)$statistic").AsNumeric().First();
                txtSW.Text = Math.Round(dblProbSW, 5).ToString();
                m_blnTransformed = true;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private double InvBCTrans(double dblValue, double dblLambda, double dblGamma)
        {
            double dblResult = 0;

            if (dblLambda == 0)
                dblResult = Math.Exp(dblValue) - dblGamma;
            else
                dblResult = (Math.Pow(dblValue* dblLambda + 1, 1/dblLambda)-dblGamma);

            return dblResult;
        }
        #endregion
    }
}
