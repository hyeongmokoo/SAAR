using System;
using System.Windows.Forms;
using System.Linq;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

//For Chart
using System.Windows.Forms.DataVisualization.Charting;
using ESRI.ArcGIS.CartoUI;
using RDotNet;

namespace VisUncertainty
{
    public partial class frmHistogram : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;

        public frmHistogram()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
                m_pEngine = m_pForm.pEngine;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
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

                if (intLIndex == -1)
                {
                    MessageBox.Show("Please select proper layer");
                    return;
                }

                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboFieldName.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                }

                int intNfeatureCount = pFClass.FeatureCount(null);
                double dblBinCnt = Math.Ceiling(Math.Pow(intNfeatureCount, 1.0f / 3.0f) * 2.0);
                nudBinsCnt.Value = Convert.ToDecimal(dblBinCnt);

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void btnHistogram_Click(object sender, EventArgs e)
        {
            //try
            //{
                string strLayerName = cboTargetLayer.Text;
                string strFieldName = cboFieldName.Text;

                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                if (intLIndex == -1)
                    return;

                int intNFeatureCount = 0;

                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

                IFeatureCursor pFCursor = pFLayer.Search(null, true);
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

                //This procedure is not working in ArcGIS 10.5 022417HK
                //IDataHistogram pDataHistogram = new DataHistogramClass();
                ////DataHistogram pDataHistogram = new DataHistogramClass();

                ////IDataHistogram pDataHistogram = new DataHistogram();
                //pDataHistogram.SetData(arrValue);
                //    IBasicHistogram pHistogram = (IBasicHistogram)pDataHistogram;

                //    object xVals, frqs;
                //    pHistogram.GetHistogram(out xVals, out frqs);

                //    IClassifyGEN pClassifyGEN = new EqualIntervalClass();

                //    int intBreakCount = Convert.ToInt32(nudBinsCnt.Value);
                //    pClassifyGEN.Classify(xVals, frqs, intBreakCount);

                //    double[] cb = (double[])pClassifyGEN.ClassBreaks;
                //    Double[] vecMids = new Double[intBreakCount];
                //    for (int j = 0; j < cb.Length - 1; j++)
                //    {
                //        vecMids[j] = (cb[j + 1] + cb[j]) / 2.0F;

                //    }

                //    Double[] vecCounts = new Double[intBreakCount];


                //    for (int j = 0; j < arrValue.Length; j++)
                //    {
                //        for (int k = 0; k < intBreakCount; k++)
                //        {
                //            if (arrValue[j] >= cb[k] && arrValue[j] <= cb[k + 1])
                //                vecCounts[k]++;
                //        }
                //    }
                int intBreakCount = Convert.ToInt32(nudBinsCnt.Value)-1;

                NumericVector vecValue = m_pEngine.CreateNumericVector(arrValue);
                m_pEngine.SetSymbol(strFieldName, vecValue);

            m_pEngine.Evaluate("hist.sample <- hist(" + strFieldName + ", plot = FALSE, nclass="+intBreakCount.ToString()+")");

                Double[] vecMids = m_pEngine.Evaluate("hist.sample$mids").AsNumeric().ToArray();
                Double[] vecCounts = m_pEngine.Evaluate("hist.sample$counts").AsNumeric().ToArray();
                Double[] dblBreaks = m_pEngine.Evaluate("hist.sample$breaks").AsNumeric().ToArray();

                frmHistResults pfrmTemp = new frmHistResults();
                pfrmTemp.Text = "Histogram of " + pFLayer.Name;
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
                pfrmTemp.pChart.ChartAreas[0].AxisX.Title = strFieldName;
                pfrmTemp.pChart.ChartAreas[0].AxisY.Title = "Frequency";

                pfrmTemp.pChart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;

                pfrmTemp.dblBreaks = dblBreaks;
                pfrmTemp.m_pForm = m_pForm;
                pfrmTemp.pActiveView = m_pActiveView;
                pfrmTemp.pFLayer = pFLayer;
                pfrmTemp.strFieldName = strFieldName;
                pfrmTemp.intNBins = intNBins;
                pfrmTemp.vecCounts = vecCounts;
                pfrmTemp.vecMids = vecMids;

                double dblInterval = dblBreaks[1] - dblBreaks[0];
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
                this.Close();
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return;
            //}
        }

        //The method is move to Snippet class 012016 HK
        //private void drawHistogram(string strLayerName, string strFieldName)
        //{
        //    MainForm mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
        //    REngine pEngine = mForm.pEngine;
        //    IActiveView pActiveView = mForm.axMapControl1.ActiveView;

        //    int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
        //    int intNFeatureCount = 0;

        //    ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);
        //    IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

        //    IFeatureCursor pFCursor = null;
        //    IFeatureSelection pFeatureSelection = pFLayer as IFeatureSelection;
        //    intNFeatureCount = pFeatureSelection.SelectionSet.Count;

        //        pFCursor = pFLayer.Search(null, true);
        //        intNFeatureCount = pFLayer.FeatureClass.FeatureCount(null);


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
        //    pEngine.Evaluate("hist.sample <- hist(" + strFieldName + ", plot = FALSE)");

        //    frmHistResults pfrmTemp = new frmHistResults();
        //    pfrmTemp.Text = "Histogram of " + pFLayer.Name;
        //    pfrmTemp.pChart.Series.Clear();

        //    Double[] vecMids = pEngine.Evaluate("hist.sample$mids").AsNumeric().ToArray();
        //    Double[] vecCounts = pEngine.Evaluate("hist.sample$counts").AsNumeric().ToArray();
        //    Double[] dblBreaks = pEngine.Evaluate("hist.sample$breaks").AsNumeric().ToArray();

        //    var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
        //    {
        //        Name = "Series1",
        //        Color = System.Drawing.Color.White,
        //        BorderColor = System.Drawing.Color.Black,
        //        IsVisibleInLegend = false,
        //        IsXValueIndexed = true,
        //        ChartType = SeriesChartType.Column,

        //    };

        //    pfrmTemp.pChart.Series.Add(series1);

        //    int intNBins = vecMids.Length;

        //    for (int j=0; j < intNBins; j++)
        //    {
        //        series1.Points.AddXY(vecMids[j], vecCounts[j]);
        //    }

        //    //pfrmTemp.pChart.Invalidate();
        //    pfrmTemp.pChart.Series[0]["PointWidth"] = "1";
        //    pfrmTemp.pChart.ChartAreas[0].AxisX.Title = strFieldName;
        //    pfrmTemp.pChart.ChartAreas[0].AxisY.Title = "Frequency";

        //    pfrmTemp.dblBreaks = dblBreaks;
        //    pfrmTemp.mForm = mForm;
        //    pfrmTemp.pActiveView = pActiveView;
        //    pfrmTemp.pFLayer = pFLayer;
        //    pfrmTemp.strFieldName = strFieldName;
        //    pfrmTemp.intNBins = intNBins;
        //    pfrmTemp.vecCounts = vecCounts;
        //    pfrmTemp.vecMids = vecMids;

        //    pfrmTemp.Show();
        //}
        public class HistSample
        {
            public static void CalculateOptimalBinWidth(double[] x)
            {
                double xMax = x.Max(), xMin = x.Min();
                int minBins = 4, maxBins = 50;
                double[] N = Enumerable.Range(minBins, maxBins - minBins)
                    .Select(v => (double)v).ToArray();
                double[] D = N.Select(v => (xMax - xMin) / v).ToArray();
                double[] C = new double[D.Length];

                for (int i = 0; i < N.Length; i++)
                {
                    double[] binIntervals = LinearSpace(xMin, xMax, (int)N[i] + 1);
                    double[] ki = Histogram(x, binIntervals);
                    ki = ki.Skip(1).Take(ki.Length - 2).ToArray();

                    double mean = ki.Average();
                    double variance = ki.Select(v => Math.Pow(v - mean, 2)).Sum() / N[i];

                    C[i] = (2 * mean - variance) / (Math.Pow(D[i], 2));
                }

                double minC = C.Min();
                int index = C.Select((c, ix) => new { Value = c, Index = ix })
                    .Where(c => c.Value == minC).First().Index;
                double optimalBinWidth = D[index];
            }

            public static double[] Histogram(double[] data, double[] binEdges)
            {
                double[] counts = new double[binEdges.Length - 1];

                for (int i = 0; i < binEdges.Length - 1; i++)
                {
                    double lower = binEdges[i], upper = binEdges[i + 1];

                    for (int j = 0; j < data.Length; j++)
                    {
                        if (data[j] >= lower && data[j] <= upper)
                        {
                            counts[i]++;
                        }
                    }
                }

                return counts;
            }

            public static double[] LinearSpace(double a, double b, int count)
            {
                double[] output = new double[count];

                for (int i = 0; i < count; i++)
                {
                    output[i] = a + ((i * (b - a)) / (count - 1));
                }

                return output;
            }
        }
    }
}
