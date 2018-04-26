using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using Gurobi;
using System.Windows.Forms.DataVisualization.Charting;



namespace VisUncertainty
{
    public partial class frmClassificationGraph : Form
    {
        public MainForm m_pForm;
        public IActiveView m_pActiveView;
        public IFeatureLayer m_pFLayer;
        public int[] cbIdx;
        public double[] Cs;
        public double[] arrEst;
        public double[] arrVar;
        
        //Gurobi Optimization Model
        public GRBModel pModel; 
        public GRBLinExpr pTotalStepsConst;
        public GRBVar[] pDecVar;
        public string strValueFldName;
        public string strUncernFldName;

        //private clsSnippet m_pSnippet;
        private clsBrusingLinking m_pBL;

        public frmClassificationGraph()
        {
            InitializeComponent();
            //m_pSnippet = new clsSnippet();
            m_pBL = new clsBrusingLinking();
        }

        private void nudConfidenceLevel_ValueChanged(object sender, EventArgs e)
        {
            ReDrawChart();
        }
        private void ReDrawChart()
        {
            try
            {
                if (this.pChart.Series.Count != 0)
                    this.pChart.Series.Clear();

                int intNfeature = arrEst.Length;
                double[,] adblValues = new double[intNfeature, 3];
                System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();

                double dblConfidenceValue = Convert.ToDouble(nudConfidenceLevel.Value);
                double dblConInstance = pChart.DataManipulator.Statistics.InverseNormalDistribution(dblConfidenceValue / 100);

                for (int i = 0; i < intNfeature; i++)
                {
                    double dblValue = arrEst[i];
                    double dblUncern = dblConInstance * arrVar[i];
                    if (dblValue < dblUncern)
                    {
                        adblValues[i, 0] = 0;
                        adblValues[i, 1] = dblValue;
                        adblValues[i, 2] = dblUncern;
                    }
                    else
                    {
                        adblValues[i, 0] = dblValue - dblUncern;
                        adblValues[i, 1] = dblUncern;
                        adblValues[i, 2] = dblUncern;
                    }
                }

                AddStackedColumnSeries(this, "Low", Color.White, adblValues, 0, intNfeature);
                AddStackedColumnSeries(this, "Mean", Color.Gray, adblValues, 1, intNfeature);
                AddStackedColumnSeries(this, "High", Color.Gray, adblValues, 2, intNfeature);

                double dblMin = 0;
                double dblMax = arrEst.Max();

                for (int j = 1; j < cbIdx.Length - 1; j++)
                {
                    AddVerticalLineSeries(this, "ver_" + j.ToString(), Color.Red, cbIdx[j] + 0.5, dblMin, dblMax);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }
        private void AddStackedColumnSeries(frmClassificationGraph pClassGraph, string strSeriesName, System.Drawing.Color FillColor, double[,] adblValues, int intStats, int intNfeatures)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn,

                };

                pClassGraph.pChart.Series.Add(pSeries);

                for (int j = 0; j < intNfeatures; j++)
                {
                    pSeries.Points.AddXY(j, adblValues[j, intStats]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }
        private void AddVerticalLineSeries(frmClassificationGraph pClassGraph, string strSeriesName, System.Drawing.Color FillColor, double dblX, double dblYMin, double dblYMax)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                };

                pClassGraph.pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblX, dblYMin);
                pSeries.Points.AddXY(dblX, dblYMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void ChangeNBreaksGurobi(GRBModel pModel, GRBLinExpr pTotalStepsConst, GRBVar[] pDecVar, int intNClasses, int intNFeatures)
        {
            try
            {
                Cs = new double[intNClasses + 1];
                cbIdx = new int[intNClasses + 1]; // For Graph
                int intNDecVar = (intNFeatures * (intNFeatures + 1)) / 2; // Add L0_0

                pModel.Remove(pModel.GetConstrByName("Nsteps"));
                pModel.AddConstr(pTotalStepsConst, GRB.EQUAL, intNClasses, "Nsteps");

                //Solving
                pModel.Optimize();

                //Add Results to CS array
                Cs[0] = arrEst[0]; //Estimate Array was sorted
                int intIdxCs = 0;


                if (pModel.Get(GRB.IntAttr.Status) == GRB.Status.OPTIMAL)
                {
                    for (int i = 0; i < intNDecVar; i++)
                    {
                        if (pDecVar[i].Get(GRB.DoubleAttr.X) == 1)
                        {
                            intIdxCs++;
                            string strName = pDecVar[i].Get(GRB.StringAttr.VarName);
                            int intIndexUBar = strName.IndexOf("_");
                            string strTo = strName.Substring(intIndexUBar + 1);
                            int intToValue = Convert.ToInt16(strTo);
                            Cs[intIdxCs] = arrEst[intToValue - 1]; //Closed
                            cbIdx[intIdxCs] = intToValue - 1;
                        }
                    }
                }

                txtObjValue.Text = pModel.Get(GRB.DoubleAttr.ObjVal).ToString("N5");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void nudGCNClasses_ValueChanged(object sender, EventArgs e)
        {
            if (pModel != null)
            {
                ChangeNBreaksGurobi(pModel, pTotalStepsConst, pDecVar, Convert.ToInt32(nudGCNClasses.Value), arrEst.Length);
                ReDrawChart();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                frmOptimizationSample pOptSampe = System.Windows.Forms.Application.OpenForms["frmOptimizationSample"] as frmOptimizationSample;
                if (pOptSampe == null)
                {
                    MessageBox.Show("The form for classification result is closed");
                    this.Close();
                }
                pOptSampe.DrawSymbolinListViewwithCb(Cs, Convert.ToInt32(nudGCNClasses.Value));
                pOptSampe.nudGCNClasses.Value = this.nudGCNClasses.Value;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult result = pChart.HitTest(e.X, e.Y);

                int intLastSeriesIdx = pChart.Series.Count - 1;

                //Remove Previous Selection
                if (pChart.Series[intLastSeriesIdx].Name == "SelSeries")
                {
                    pChart.Series.RemoveAt(intLastSeriesIdx);
                }

                if (result.ChartElementType == ChartElementType.DataPoint)
                {
                    //Brushing on Graph
                    int index = result.PointIndex;

                    double dblYValue = pChart.Series[0].Points[index].YValues[0] + pChart.Series[1].Points[index].YValues[0];
                    double dblSelYValue = pChart.Series[0].Points[index].YValues[0] + pChart.Series[1].Points[index].YValues[0] + pChart.Series[2].Points[index].YValues[0];

                    double dblXvalue = pChart.Series[1].Points[index].XValue;

                    var Selseries1 = new System.Windows.Forms.DataVisualization.Charting.Series
                    {
                        Name = "SelSeries",
                        Color = System.Drawing.Color.Cyan,
                        BorderColor = System.Drawing.Color.Black,
                        IsVisibleInLegend = false,
                        //BackHatchStyle = ChartHatchStyle.DiagonalCross,
                        ChartType = SeriesChartType.Column,

                    };
                    pChart.Series.Add(Selseries1);
                    Selseries1.Points.AddXY(dblXvalue, dblSelYValue);

                    string whereClause = strValueFldName + " = " + dblYValue.ToString();

                    //Brushing to ActiveView
                    m_pBL.FeatureSelectionOnActiveView(whereClause, m_pActiveView, m_pFLayer);

                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
                }
                else
                {
                    //Clear Selection Both Histogram and ActiveView
                    //Remove Previous Selection
                    intLastSeriesIdx = pChart.Series.Count - 1;
                    if (pChart.Series[intLastSeriesIdx].Name == "SelSeries")
                    {
                        pChart.Series.RemoveAt(intLastSeriesIdx);
                    }
                    IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                    m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void frmClassificationGraph_FormClosed(object sender, FormClosedEventArgs e)
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
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }


    }
}
