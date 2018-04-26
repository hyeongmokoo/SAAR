using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using System.Web.UI.DataVisualization.Charting;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisUncertainty
{
    public partial class frmCSDesign : Form
    {
        private int intLstCounts = 10;
        private int intLock = 0;
        private clsSnippet pSnippet;

        private double[] arrProV;
        private double[] arrSortedProV;
        private int intNClasses;

        //Pulbic Members
        public double[] arrEst;
        public double[] arrVar;
        public int[] intResultIdx;
        public double[] cb;
        public string strValueFldName;
        public string strUncernFldName;
        public MainForm mForm;
        public IActiveView pActiveView;
        public IFeatureLayer pFLayer;
        public double[] arrResults;
        public double[] dblpValue;

        public frmCSDesign()
        {

            InitializeComponent();
            pSnippet = new clsSnippet();

        }

        private void AddMultiRamps(ListView lvSymbol, int intFormColumnCnt, int[,] arrFormViewColors)
        {
            try
            {
                lvSymbol.BeginUpdate();
                lvSymbol.Items.Clear();
                int intItemCnts = lvSymbol.Items.Count;
                ListViewItem lvi = new ListViewItem(intItemCnts.ToString());

                for (int j = 0; j < intFormColumnCnt; j++)
                {
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        "", Color.White, Color.FromArgb(arrFormViewColors[j, 0], arrFormViewColors[j, 1], arrFormViewColors[j, 2]), lvi.Font));
                }

                lvSymbol.Items.Add(lvi);

                lvSymbol.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }
        private int[,] RedToBlueColorRamps()
        {
            IEnumColors pEnumColors = null;
            int[,] arrSepLineColor = new int[10, 3];
            IAlgorithmicColorRamp pColorRamp1 = new AlgorithmicColorRampClass();
            IAlgorithmicColorRamp pColorRamp2 = new AlgorithmicColorRampClass();

            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;

            IRgbColor pColor1 = new RgbColor();
            IRgbColor pColor2 = new RgbColor();
            IRgbColor pColor3 = new RgbColor();



            pColor1 = pSnippet.getRGB(165, 0, 38);
            pColor2 = pSnippet.getRGB(255, 255, 191);
            pColor3 = pSnippet.getRGB(49, 54, 149);

            pColorRamp1.FromColor = pColor1;
            pColorRamp1.ToColor = pColor2;
            pColorRamp2.FromColor = pColor2;
            pColorRamp2.ToColor = pColor3;

            Boolean blnOK = true;

            IMultiPartColorRamp pMultiColorRamp = new MultiPartColorRampClass();
            pMultiColorRamp.Ramp[0] = pColorRamp1;
            pMultiColorRamp.Ramp[1] = pColorRamp2;
            pMultiColorRamp.Size = 10;
            pMultiColorRamp.CreateRamp(out blnOK);


            pEnumColors = pMultiColorRamp.Colors;
            pEnumColors.Reset();
            for (int k = 0; k < 10; k++)
            {
                IColor pColor = pEnumColors.Next();
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.RGB = pColor.RGB;

                arrSepLineColor[k, 0] = pRGBColor.Red;
                arrSepLineColor[k, 1] = pRGBColor.Green;
                arrSepLineColor[k, 2] = pRGBColor.Blue;
            }

            return arrSepLineColor;
        }
        private void InitializingArrays()
        {
            int intLength = arrResults.Length;
            arrProV = new double[intLength];
            arrSortedProV = new double[intLength];

            System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();
            for (int i = 0; i < intLength; i++)
            {
                //intResultIdx[i] = System.Array.IndexOf(arrResults, arrSortedResult[i]);
                arrProV[i] = pChart.DataManipulator.Statistics.NormalDistribution(arrResults[i]);
                arrSortedProV[i] = pChart.DataManipulator.Statistics.NormalDistribution(arrResults[i]);
            }
            //arrSortedProV = arrProV; 
            System.Array.Sort<double>(arrSortedProV, new Comparison<double>(
                                (i1, i2) => i2.CompareTo(i1)
                        ));
                
        }
        private void ReDrawVertical()
        {
            while (this.pChart.Series.Count > 3)
            {
                this.pChart.Series.RemoveAt(3);
            }

            double dblMin = 0;
            double dblMax = arrEst.Max() + (3 * arrVar.Max());

            int[,] arrSepVerColors = RedToBlueColorRamps();
            int intSepLineIdx = 0;
            System.Drawing.Color pColor = new Color();
            for (int j = 0; j < intResultIdx.Length; j++)
            {
                intSepLineIdx = Convert.ToInt32(Math.Floor(dblpValue[j] * 10));
                pColor = new Color();
                pColor = Color.FromArgb(arrSepVerColors[intSepLineIdx, 0], arrSepVerColors[intSepLineIdx, 1], arrSepVerColors[intSepLineIdx, 2]);
                AddVerticalLineSeries(this, "ver_" + j.ToString(), pColor, intResultIdx[j] + 0.5, dblMin, dblMax);
            }
        }
        private void ReDrawChart()
        {
            if (dblpValue == null)
                return;
            
            if (this.pChart.Series.Count != 0)
                this.pChart.Series.Clear();
            

            int intNfeature = arrEst.Length;
            double[,] adblValues = new double[intNfeature, 3];
            System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();
            //nudConfidenceLevel.Value = 99;
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
            double dblMax = arrEst.Max() + (3 * arrVar.Max());

            int[,] arrSepVerColors = RedToBlueColorRamps();
            int intSepLineIdx = 0;
            System.Drawing.Color pColor = new Color();
            for (int j = 0; j < intResultIdx.Length; j++)
            {
                intSepLineIdx = Convert.ToInt32(Math.Floor(dblpValue[j] * 10));
                pColor = new Color();
                pColor = Color.FromArgb(arrSepVerColors[intSepLineIdx, 0], arrSepVerColors[intSepLineIdx, 1], arrSepVerColors[intSepLineIdx, 2]);
                AddVerticalLineSeries(this, "ver_" + j.ToString(), pColor, intResultIdx[j] + 0.5, dblMin, dblMax);
            }
        }
        private void AddStackedColumnSeries(frmCSDesign pfrmCSDesign, string strSeriesName, System.Drawing.Color FillColor, double[,] adblValues, int intStats, int intNfeatures)
        {
            //try
            //{
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn,

                };

                pfrmCSDesign.pChart.Series.Add(pSeries);

                for (int j = 0; j < intNfeatures; j++)
                {
                    pSeries.Points.AddXY(j, adblValues[j, intStats]);
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
            //    return;
            //}
        }
        private void AddVerticalLineSeries(frmCSDesign pfrmCSDesign, string strSeriesName, System.Drawing.Color FillColor, double dblX, double dblYMin, double dblYMax)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = Color.Black,
                   BorderWidth = 2,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };

                pfrmCSDesign.pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblX, dblYMin);
                pSeries.Points.AddXY(dblX, dblYMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }

        }
        private void UpdateCbbyClassNumber(int intNClasses)
        {
            if (arrProV == null || arrSortedProV == null)
                return;

            System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();

            dblpValue = new double[intNClasses - 1];
            intResultIdx = new int[intNClasses - 1];
            for (int i = 0; i < intNClasses - 1; i++)
            {
                intResultIdx[i] = System.Array.IndexOf(arrProV, arrSortedProV[i]);
            }
            System.Array.Sort(intResultIdx);

            cb = new double[intNClasses + 1];
            cb[0] = arrEst.Min();

            cb[intNClasses] = arrEst.Max();
            for (int i = 0; i < intNClasses - 1; i++)
            {
                cb[i + 1] = arrEst[intResultIdx[i]];
                dblpValue[i] = pChart.DataManipulator.Statistics.NormalDistribution(arrResults[intResultIdx[i]]);
            }

        }



        private void nudGCNClasses_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                    if (dblpValue == null)
                        return;

                    int intNewClassNumber = Convert.ToInt32(nudGCNClasses.Value);
                    if (intNClasses != intNewClassNumber)
                    {
                        UpdateCbbyClassNumber(intNewClassNumber);
                        //ReDrawChart();
                        ReDrawVertical();
                        txtMinSep.Text = dblpValue.Min().ToString("N3");
                        trbpValue.Value = Convert.ToInt32(dblpValue.Min() * 100);
                        intNClasses = intNewClassNumber;
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void frmCSDesign_Load(object sender, EventArgs e)
        {
            AddMultiRamps(listFormcolor, intLstCounts, RedToBlueColorRamps());
            InitializingArrays();
            intNClasses = Convert.ToInt32(nudGCNClasses.Value);
            trbpValue.Value = Convert.ToInt32(dblpValue.Min() * 100);

            txtMinSep.Text = dblpValue.Min().ToString("N3");

        }

        private void nudConfidenceLevel_ValueChanged(object sender, EventArgs e)
        {
            ReDrawChart();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ////frmClassSeparability pfrmClassSep = System.Windows.Forms.Application.OpenForms["frmClassSeparability"] as frmClassSeparability;
            ////if (pfrmClassSep == null)
            //{
            //    MessageBox.Show("The form for classification result is closed");
            //    this.Close();
            //}
            
            //pfrmClassSep.dblpValue = dblpValue;
            //pfrmClassSep.cb = cb;
            //pfrmClassSep.intGCBreakeCount = intNClasses;
            //pfrmClassSep.nudGCNClasses.Value = intNClasses;
            //pfrmClassSep.DrawSymbolinListViewwithCb(intNClasses, cb);
            //this.Close();
        }

        private int UpdateNClassesbyMinValue(double dblMinValue)
        {
            if (arrProV == null || arrSortedProV == null)
                return -1;

            System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();
            int intNewClassNumber = 0;
            while (arrSortedProV[intNewClassNumber] >= dblMinValue)
            {
                intNewClassNumber++;
            }
            intNewClassNumber += 1;

            return intNewClassNumber;

        }

        private void trbpValue_Scroll(object sender, EventArgs e)
        {
            if (dblpValue == null)
                return;
            double dblMinSepValue = Convert.ToDouble(trbpValue.Value);

            int intNewClassNumber = UpdateNClassesbyMinValue(dblMinSepValue / 100);
            nudGCNClasses.Value = intNewClassNumber;
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.HitTestResult result = pChart.HitTest(e.X, e.Y);

            int intLastSeriesIdx = pChart.Series.Count - 1;

            //Remove Previous Selection
            if (pChart.Series[intLastSeriesIdx].Name == "SelSeries")
            {
                pChart.Series.RemoveAt(intLastSeriesIdx);
            }

            if (result.ChartElementType == System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint)
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
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column,

                };
                pChart.Series.Add(Selseries1);
                Selseries1.Points.AddXY(dblXvalue, dblSelYValue);

                string whereClause = strValueFldName + " = " + dblYValue.ToString();

                //Brushing to ActiveView
                pSnippet.FeatureSelectionOnActiveView(whereClause, pActiveView, pFLayer);

                //Brushing to other graphs
                pSnippet.BrushingToOthers(pFLayer, this.Handle);
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
                IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;
                pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                featureSelection.Clear();
                pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                //Brushing to other graphs
                pSnippet.BrushingToOthers(pFLayer, this.Handle);
            }
        }

        private void frmCSDesign_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                int intFCnt = pSnippet.RemoveBrushing(mForm, pFLayer);
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
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }


    }
}
