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
//R library car is required


namespace VisUncertainty
{
    public partial class frmScatterplot : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        public frmScatterplot()
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
            }
            catch
            {
                MessageBox.Show("Error 231");
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
                cboVariable2.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboVariable1.Items.Add(fields.get_Field(i).Name);
                        cboVariable2.Items.Add(fields.get_Field(i).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 232: "+ex.Message);
                return;
            }
        }

        private void btnScatterplot_Click(object sender, EventArgs e)
        {
            //try
            //{
                //REngine pEngine = m_pForm.pEngine;

                string strTargetLayerName = cboTargetLayer.Text;
                string strVar1Name = cboVariable1.Text;
                string strVar2Name = cboVariable2.Text;

                int intTargetIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strTargetLayerName);
                int intNFeatureCount = 0;

                ILayer pTargetLayer = m_pForm.axMapControl1.get_Layer(intTargetIndex);

                IFeatureLayer pTargetFLayer = (IFeatureLayer)pTargetLayer;

                ////Extract geometry from selected feature
                IFeatureCursor pFCursor = null;

            //Draw scatter plot for only entire features 030117 HK
            //IFeatureSelection pFeatureSelection = pTargetFLayer as IFeatureSelection;
            //intNFeatureCount = pFeatureSelection.SelectionSet.Count;
            //if (intNFeatureCount > 0 && chkUseSelected.Checked == true)
            //{
            //    ICursor pCursor = null;

            //    pFeatureSelection.SelectionSet.Search(null, true, out pCursor);
            //    pFCursor = (IFeatureCursor)pCursor;

            //}
            //else if (intNFeatureCount == 0 && chkUseSelected.Checked == true)
            //{
            //    MessageBox.Show("Select at least one feature");
            //    return;
            //}
            //else
            //{
            //    pFCursor = pTargetFLayer.Search(null, true);
            //    intNFeatureCount = pTargetFLayer.FeatureClass.FeatureCount(null);
            //}

            pFCursor = pTargetFLayer.Search(null, true);
            intNFeatureCount = pTargetFLayer.FeatureClass.FeatureCount(null);
            IFeature pFeature = pFCursor.NextFeature();

                //Source and Group Field Index
                int intVar1Idx = pTargetFLayer.FeatureClass.Fields.FindField(strVar1Name);
                int intVar2Idx = pTargetFLayer.FeatureClass.Fields.FindField(strVar2Name);

                double[] adblVar1 = new double[intNFeatureCount];
                double[] adblVar2 = new double[intNFeatureCount];


                //Scatter Plot
                frmScatterPlotResults pfrmScatterPlotResult = new frmScatterPlotResults();
            
                pfrmScatterPlotResult.Text = "Scatter Plot of " + pTargetFLayer.Name;
                pfrmScatterPlotResult.pChart.Series.Clear();
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

                pfrmScatterPlotResult.pChart.Series.Add(seriesPts);

                //Manually Calculate Regression Coefficients 

                double sumOfX = 0;
                double sumOfY = 0;
                double sumOfXSq = 0;
                double sumOfYSq = 0;
                double ssX = 0;
                double ssY = 0;
                double sumCodeviates = 0;
                double sCo = 0;

                //Feature Value to Array
                int i = 0;
                while (pFeature != null)
                {
                    double x = Convert.ToDouble(pFeature.get_Value(intVar1Idx));
                    double y = Convert.ToDouble(pFeature.get_Value(intVar2Idx));

                    adblVar1[i] = x;
                    adblVar2[i] = y;
                    //Add Pts
                    seriesPts.Points.AddXY(x, y);

                    sumCodeviates += x * y;
                    sumOfX += x;
                    sumOfY += y;
                    sumOfXSq += x * x;
                    sumOfYSq += y * y;

                    i++;
                    pFeature = pFCursor.NextFeature();
                }

                //Manually Calculate Regression Coefficients 
                double count = Convert.ToDouble(intNFeatureCount);
                ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
                ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
                double RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
                double RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
                 * (count * sumOfYSq - (sumOfY * sumOfY));
                sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

                double meanX = sumOfX / count;
                double meanY = sumOfY / count;
                //double dblR = RNumerator / Math.Sqrt(RDenom);
                //double rsquared = dblR * dblR;
                double yintercept = meanY - ((sCo / ssX) * meanX);
                double slope = sCo / ssX;

                ////Array to R vector.
                //NumericVector vecVar1 = pEngine.CreateNumericVector(adblVar1);
                //NumericVector vecVar2 = pEngine.CreateNumericVector(adblVar2);
                //pEngine.SetSymbol(strVar1Name, vecVar1);
                //pEngine.SetSymbol(strVar2Name, vecVar2);
                //////pEngine.Evaluate("library(car)");
                ////string strCommand = "scatterplot(" + strVar2Name + "~" + strVar1Name + ", smoother=F);";
                ////string strCommand = "plot(" + strVar1Name + ", " + strVar2Name + ");";
                ////string strTitle = "Scatterplot";

                ////pSnippet.drawPlottoForm(strTitle, strCommand);
                //NumericVector vecCoeff = pEngine.Evaluate("lm(" + strVar2Name + "~" + strVar1Name + ")$coefficients").AsNumeric();

                var seriesLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "Line",
                    Color = System.Drawing.Color.Red,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };

                pfrmScatterPlotResult.pChart.Series.Add(seriesLine);

                seriesLine.Points.AddXY(adblVar1.Min(), adblVar1.Min() * slope + yintercept);
                seriesLine.Points.AddXY(adblVar1.Max(), adblVar1.Max() * slope + yintercept);

                pfrmScatterPlotResult.pChart.ChartAreas[0].AxisX.Title = strVar1Name;
                pfrmScatterPlotResult.pChart.ChartAreas[0].AxisY.Title = strVar2Name;

                //Define min and max for y axis
                double dblYmin = adblVar2.Min();
                double dblYMax = adblVar2.Max();
                double dblOffset = (dblYMax - dblYmin) / 20;
                if (dblYmin > 0 && dblYmin < dblOffset)
                    pfrmScatterPlotResult.pChart.ChartAreas[0].AxisY.Minimum = 0;
                else
                    pfrmScatterPlotResult.pChart.ChartAreas[0].AxisY.Minimum = dblYmin-dblOffset;

                pfrmScatterPlotResult.pChart.ChartAreas[0].AxisY.Maximum = dblYMax + dblOffset;
            //For Presentation 030118 HK
            pfrmScatterPlotResult.pChart.ChartAreas[0].AxisY.LabelStyle.Format = "#.####";
            pfrmScatterPlotResult.pChart.ChartAreas[0].AxisX.LabelStyle.Format = "#.####";

            ////pfrmTemp.dblBreaks = dblBreaks;
            //pfrmScatterPlotResult.intGraphType = 1;
            pfrmScatterPlotResult.adblVar1 = adblVar1;
                pfrmScatterPlotResult.adblVar2 = adblVar2;
                pfrmScatterPlotResult.strVar1Name = strVar1Name;
                pfrmScatterPlotResult.strVar2Name = strVar2Name;
                pfrmScatterPlotResult.m_pForm = m_pForm;
                pfrmScatterPlotResult.m_pActiveView = m_pActiveView;
                pfrmScatterPlotResult.m_pFLayer = pTargetFLayer;
                //pfrmPtsPlot.Text = strTitle;
                pfrmScatterPlotResult.pMakerColor = pMarkerColor;
                pfrmScatterPlotResult.lblRegression.Text = strVar2Name + " = " + slope.ToString("N3") + " * " + strVar1Name + " + " + yintercept.ToString("N3");
                //pfrmTemp.strFieldName = strFieldName;
                pfrmScatterPlotResult.Show();
                //this.Close();
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return;
            //}
        }
    }
}
