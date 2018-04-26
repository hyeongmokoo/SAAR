using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using RDotNet;

namespace VisUncertainty
{
    public partial class frmUncernSAM: Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        public frmUncernSAM()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Only polygon to make spatial weight matrix 10/9/15 HK
                        cboSourceLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass)
        {
            try
            {
                pListView.Items.Clear();

                pListView.BeginUpdate();

                int intNFlds = 3;
                string[] strFldNames = null;
                string[] strLvNames = null;

                string strBDOutputFldNM = "BD";
                string strBDOutput = "B-distance";

                string strBCOutputFldNM = "BC";
                string strBCOutput = "B-coefficient";

                string strHDOutputFldNM = "HD";
                string strHDOutput = "Hellinger distance";

                strFldNames = new string[intNFlds];
                strFldNames[0] = strBDOutputFldNM;
                strFldNames[1] = strBCOutputFldNM;
                strFldNames[2] = strHDOutputFldNM;

                strLvNames = new string[intNFlds];
                strLvNames[0] = strBDOutput;
                strLvNames[1] = strBCOutput;
                strLvNames[2] = strHDOutput;

                string[] strNewNames = UpdateFldNames(strFldNames, pFeatureClass);

                for (int i = 0; i < intNFlds; i++)
                {
                    ListViewItem lvi = new ListViewItem(strLvNames[i]);
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, strNewNames[i]));

                    pListView.Items.Add(lvi);
                }

                pListView.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private string[] UpdateFldNames(string[] strFldNMs, IFeatureClass pFeatureClass)
        {
            try
            {
                int intMax = 0;
                for (int j = 0; j < strFldNMs.Length; j++)
                {
                    string strNM = strFldNMs[j];
                    int i = 1;
                    while (pFeatureClass.FindField(strNM) != -1)
                    {
                        strNM = strFldNMs[j] + "_" + i.ToString();
                        i++;
                    }
                    if (i > intMax)
                        intMax = i;
                }
                string[] strReturnNMs = new string[strFldNMs.Length];
                for (int j = 0; j < strFldNMs.Length; j++)
                {
                    if (intMax == 1)
                        strReturnNMs[j] = strFldNMs[j];
                    else
                        strReturnNMs[j] = strFldNMs[j] + "_" + (intMax - 1).ToString();
                }
                return strReturnNMs;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
            }
        }
        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                IFields fields = m_pFClass.Fields;

                cboValueField.Text = "";
                cboUField.Text = "";

                cboValueField.Items.Clear();
                cboUField.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboValueField.Items.Add(fields.get_Field(i).Name);
                        cboUField.Items.Add(fields.get_Field(i).Name);
                    }
                }

                UpdateListview(lvFields, m_pFClass);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cboValueField.Text == "" || cboUField.Text == "")
            {
                MessageBox.Show("Please select target field");
                return;
            }

            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Processing:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            REngine pEngine = m_pForm.pEngine;

            int nFeature = m_pFClass.FeatureCount(null);

            IFeatureCursor pFCursor = m_pFClass.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get index for independent and dependent variables
            //Get variable index            
            string strVarNM = (string)cboValueField.SelectedItem;
            string strUncNM = (string)cboUField.SelectedItem;
            int intVarIdx = m_pFClass.FindField(strVarNM);
            int intUncIdx = m_pFClass.FindField(strUncNM);
            int intFIDIdx = m_pFClass.FindField(m_pFClass.OIDFieldName);

            //Store Variable at Array
            double[] arrVar = new double[nFeature];
            double[] arrUnc = new double[nFeature];

            int[] arrFID = new int[nFeature];

            int i = 0;

            while (pFeature != null)
            {
                arrVar[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                arrUnc[i] = Convert.ToDouble(pFeature.get_Value(intUncIdx));
                arrFID[i] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
                i++;
                pFeature = pFCursor.NextFeature();
            }

            pFCursor.Flush();

            //Plot command for R
            StringBuilder plotCommmand = new StringBuilder();

            string strStartPath = m_pForm.strPath;
            string pathr = strStartPath.Replace(@"\", @"/");
            pEngine.Evaluate("source('" + pathr + "/UncernSAM/functions.R')");

            //Get the file path and name to create spatial weight matrix
            string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

            if (strNameR == null)
                return;

            //Create spatial weight matrix in R
            pEngine.Evaluate("library(spdep); library(maptools)");
            pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
            //pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=FALSE)");
            pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)");

            NumericVector vecVar1 = pEngine.CreateNumericVector(arrVar);
            pEngine.SetSymbol("sample.est", vecVar1);
            NumericVector vecVar2 = pEngine.CreateNumericVector(arrUnc);
            pEngine.SetSymbol("sample.var", vecVar2);

            pEngine.Evaluate("sample.result <- UncernSAM(sample.est, sample.var, sample.nb)");

            //Local Geary for comparison
            pEngine.Evaluate("source('" + pathr + "/AllFunctions.R')");
            pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W', zero.policy=TRUE)");
            pEngine.Evaluate("localsam.result <- localgeary(sample.est, sample.listw);");

            double[] adblBD = pEngine.Evaluate("sample.result[[2]]").AsNumeric().ToArray();
            double[] adblBC = pEngine.Evaluate("sample.result[[3]]").AsNumeric().ToArray();
            //double[] adblHD = pEngine.Evaluate("sample.result[[4]]").AsNumeric().ToArray();
            double[] adblHD = pEngine.Evaluate("localsam.result[,1]").AsNumeric().ToArray();
            //Save Output on SHP
            //Add Target fields to store results in the shapefile // Keep loop 
            for (int j = 0; j < 3; j++)
            {
                string strfldName = lvFields.Items[j].SubItems[1].Text;
                if (m_pFClass.FindField(strfldName) == -1)
                {
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strfldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    m_pFClass.AddField(newField);
                }
            }

            //Update Field
            pFCursor = m_pFClass.Update(null, false);
            pFeature = pFCursor.NextFeature();

            string strBDFldName = lvFields.Items[0].SubItems[1].Text;
            int intBDFldIdx = m_pFClass.FindField(strBDFldName);

            string strBCFldName = lvFields.Items[1].SubItems[1].Text;
            int intBCFldIdx = m_pFClass.FindField(strBCFldName);

            string strHDFldName = lvFields.Items[2].SubItems[1].Text;
            int intHDFldIdx = m_pFClass.FindField(strHDFldName);


            int featureIdx = 0;
            while (pFeature != null)
            {
                pFeature.set_Value(intBDFldIdx, adblBD[featureIdx]);
                pFeature.set_Value(intBCFldIdx, adblBC[featureIdx]);
                pFeature.set_Value(intHDFldIdx, adblHD[featureIdx]);

                pFCursor.UpdateFeature(pFeature);

                pFeature = pFCursor.NextFeature();
                featureIdx++;
            }
            pFCursor.Flush();


            if(chkScatterplot.Checked)
            {
                pEngine.Evaluate("sample.listw <- nb2listw(sample.nb, style='W')");

                NumericVector vecWeightVar = null;
                vecWeightVar = pEngine.Evaluate("wx.sample <- lag.listw(sample.listw, sample.est, zero.policy=TRUE)").AsNumeric();
                
                pEngine.SetSymbol("WVar.sample", vecWeightVar);

                NumericVector vecCoeff = pEngine.Evaluate("lm(WVar.sample~sample.est)$coefficients").AsNumeric();

                frmMCscatterwithColor pfrmMScatterResult = new frmMCscatterwithColor();
                pfrmMScatterResult.pChart.ChartAreas[0].AxisX.IsStartedFromZero = false;
                pfrmMScatterResult.pChart.ChartAreas[0].AxisX.IsMarginVisible = true;

                pfrmMScatterResult.pChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

                pfrmMScatterResult.Text = "Moran Scatter Plot of " + m_pFLayer.Name;
                pfrmMScatterResult.pChart.Series.Clear();
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

                pfrmMScatterResult.pChart.Series.Add(seriesPts);

                for (int j = 0; j < arrVar.Length; j++)
                    seriesPts.Points.AddXY(arrVar[j], vecWeightVar[j]);



                var VLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "VLine",
                    Color = System.Drawing.Color.Black,
                    BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };
                pfrmMScatterResult.pChart.Series.Add(VLine);

                VLine.Points.AddXY(arrVar.Average(), vecWeightVar.Min());
                VLine.Points.AddXY(arrVar.Average(), vecWeightVar.Max());

                var HLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "HLine",
                    Color = System.Drawing.Color.Black,
                    BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };
                pfrmMScatterResult.pChart.Series.Add(HLine);

                HLine.Points.AddXY(arrVar.Min(), vecWeightVar.Average());
                HLine.Points.AddXY(arrVar.Max(), vecWeightVar.Average());

                var seriesLine = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "RegLine",
                    Color = System.Drawing.Color.Red,
                    //BorderColor = System.Drawing.Color.Black,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
                };

                pfrmMScatterResult.pChart.Series.Add(seriesLine);

                seriesLine.Points.AddXY(arrVar.Min(), arrVar.Min() * vecCoeff[1] + vecCoeff[0]);
                seriesLine.Points.AddXY(arrVar.Max(), arrVar.Max() * vecCoeff[1] + vecCoeff[0]);


            }

            pfrmProgress.Close();
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



            pColor1 = m_pSnippet.getRGB(165, 0, 38);
            pColor2 = m_pSnippet.getRGB(255, 255, 191);
            pColor3 = m_pSnippet.getRGB(49, 54, 149);

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
    }
}
