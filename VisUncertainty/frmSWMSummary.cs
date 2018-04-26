using RDotNet;
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
using ESRI.ArcGIS.Carto;

namespace VisUncertainty
{
    public partial class frmSWMSummary : Form
    {
        public clsSnippet.Def_SpatialWeightsMatrix Def_SWM;
        private REngine m_pEngine;
        public IFeatureLayer m_pFLayer;
        private MainForm m_pForm;
        private IActiveView m_pActiveView;


        public frmSWMSummary()
        {
            InitializeComponent();
            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            m_pActiveView = m_pForm.axMapControl1.ActiveView;
            m_pEngine = m_pForm.pEngine;

        }

        private void frmSWMSummary_Load(object sender, EventArgs e)
        {

            if (Def_SWM == null)
                return;
            //Assign values at Textbox
            clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();

            string[] strResults = new string[7];
            if (Def_SWM.Definition == pSWMType.strPolyDefs[0] || Def_SWM.Definition == pSWMType.strPolyDefs[1])
            {
                strResults = new string[12];

            }
            strResults[0] = "Definition: " + Def_SWM.Definition;



            if (Def_SWM.Definition == pSWMType.strPointDef[0])
            {
                if (Def_SWM.Subset)
                    strResults[1] = "Clipped by polygon";
                else
                    strResults[1] = "Without clipping";
            }
            else if (Def_SWM.Definition == pSWMType.strPointDef[1])
                strResults[1] = "Threshold distance: " + Def_SWM.AdvancedValue.ToString("N2");
            else if (Def_SWM.Definition == pSWMType.strPointDef[2])
                strResults[1] = "Number of neighbors: " + Def_SWM.AdvancedValue.ToString("N0");
            else
                strResults[1] = " ";

            strResults[2] = "";
            strResults[3] = "Number of features: " + Def_SWM.FeatureCount.ToString();
            strResults[4] = "Number of non-zero neighbors: " + Def_SWM.NonZeroLinkCount.ToString();
            strResults[5] = "Percentage of non-zero neighbors: " + Def_SWM.PercentNonZeroWeight.ToString("N2");
            strResults[6] = "Average number of neighbors: " + Def_SWM.AverageNumberofLink.ToString("N3");

            //For higher lags
            if (Def_SWM.Definition == pSWMType.strPolyDefs[0] || Def_SWM.Definition == pSWMType.strPolyDefs[1])
            {
                if (Def_SWM.AdvancedValue > 1)
                {
                    if (Def_SWM.Cumulative)
                        strResults[1] = "Order: " + Def_SWM.AdvancedValue.ToString() + " (Cumulative)";
                    else
                        strResults[1] = "Order: " + Def_SWM.AdvancedValue.ToString();

                    StringBuilder pSBOrder = new StringBuilder();
                    pSBOrder.Append("Order\t");
                    StringBuilder pSBCount = new StringBuilder();
                    pSBCount.Append("Count\t");
                    StringBuilder pSBAverage = new StringBuilder();
                    pSBAverage.Append("Average\t");

                    for (int j = 0; j < Def_SWM.AverageforHigher.Length; j++)
                    {
                        pSBOrder.Append((j + 1).ToString() + "\t");
                        pSBCount.Append(Def_SWM.LinkCountforHigher[j].ToString() + "\t");
                        pSBAverage.Append(Def_SWM.AverageforHigher[j].ToString("N2") + "\t");
                    }

                    strResults[7] = "";
                    strResults[8] = "Higher order neighbors statistics";
                    strResults[9] = pSBOrder.ToString();
                    strResults[10] = pSBCount.ToString();
                    strResults[11] = pSBAverage.ToString();
                }

            }

            txtResult.Lines = strResults;
            this.ActiveControl = label1;
        }

        private void btnHistogram_Click(object sender, EventArgs e)
        {
            if (Def_SWM == null)
                return;

            int intBreakCount = Convert.ToInt32(Def_SWM.NeighborCounts.Max() - Def_SWM.NeighborCounts.Min() + 1);

            //NumericVector vecValue = m_pEngine.CreateNumericVector(Def_SWM.NeighborCounts);
            //m_pEngine.SetSymbol("sample.neighbors", vecValue);
            //m_pEngine.Evaluate("hist.sample <- hist(sample.neighbors, plot = FALSE, nclass=" + intBreakCount.ToString() + ")");

            //Double[] vecMids = m_pEngine.Evaluate("hist.sample$mids").AsNumeric().ToArray();
            Double[] vecMids = m_pEngine.Evaluate("c("+Def_SWM.NeighborCounts.Min().ToString("N0")+":"+ Def_SWM.NeighborCounts.Max().ToString("N0") + ")+0.5").AsNumeric().ToArray();
            //Double[] vecCounts = m_pEngine.Evaluate("hist.sample$counts").AsNumeric().ToArray();
            Double[] vecCounts = new double[intBreakCount];
            Double[] dblBreaks = m_pEngine.Evaluate("c(" + Def_SWM.NeighborCounts.Min().ToString("N0") + ":" + Def_SWM.NeighborCounts.Max().ToString("N0") + ")").AsNumeric().ToArray();

            
            StringBuilder whereClause = new StringBuilder();
            for (int j = 0; j < dblBreaks.Length; j++)
            {
                int intSelectedValue = Convert.ToInt32(dblBreaks[j]);
                int[] SelectedFIDs = Def_SWM.NeighborCounts.Select((b, i) => b == intSelectedValue ? i : -1).Where(i => i != -1).ToArray();
                vecCounts[j] = SelectedFIDs.Length;
            }


            frmConnectivityHistogram pfrmTemp = new frmConnectivityHistogram();
            pfrmTemp.Text = "Histogram of neighbors count";
            pfrmTemp.pChart.Series.Clear();

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

            pfrmTemp.pChart.Series[0]["PointWidth"] = "1";
            pfrmTemp.pChart.ChartAreas[0].AxisX.Title = "Number of neighbors";
            pfrmTemp.pChart.ChartAreas[0].AxisY.Title = "Frequency";

            pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;

            pfrmTemp.dblBreaks = dblBreaks;
            pfrmTemp.m_pForm = m_pForm;
            pfrmTemp.pActiveView = m_pActiveView;
            pfrmTemp.pFLayer = m_pFLayer;
            pfrmTemp.intNBins = intNBins;
            pfrmTemp.vecCounts = vecCounts;
            pfrmTemp.vecMids = vecMids;
            pfrmTemp.DefSWM = Def_SWM;

            //double dblInterval = dblBreaks[1] - dblBreaks[0];
            int intDecimalPlaces = 1;
            string strDecimalPlaces = "N" + intDecimalPlaces.ToString();
            for (int n = 0; n < dblBreaks.Length; n++)
            {
                CustomLabel pcutsomLabel = new CustomLabel();
                pcutsomLabel.FromPosition = n;
                pcutsomLabel.ToPosition = n + 1;
                pcutsomLabel.Text = dblBreaks[n].ToString(strDecimalPlaces);

                pfrmTemp.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);
            }

            pfrmTemp.Show();
        }

        private void btnConnectivity_Click(object sender, EventArgs e)
        {
            frmConnectivityMap pfrmConnectivityMap = new frmConnectivityMap();
            IActiveView pConnectActiveView = pfrmConnectivityMap.ConMapControl.ActiveView;
            IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
            featureSelection.Clear();

            pConnectActiveView.FocusMap.AddLayer(m_pFLayer);
            pfrmConnectivityMap.m_pFLayer = m_pFLayer;
            pfrmConnectivityMap.Def_SWM = Def_SWM;
            pfrmConnectivityMap.Show();
        }
    }
}
