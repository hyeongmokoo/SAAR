using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.DataVisualization.Charting;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;

namespace VisUncertainty
{
    public partial class frmRobustness : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public frmRobustness()
        {
            InitializeComponent();
            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            pActiveView = mForm.axMapControl1.ActiveView;
            pSnippet = new clsSnippet();

            for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pActiveView.FocusMap.get_Layer(i);

                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
            }
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            pSnippet.AddFieldsForTwoCbo(cboSourceLayer.Text, pActiveView, mForm, cboValueField, cboUField);
        }

        private void picSymolfrom_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picSymolfrom.BackColor = cdColor.Color;
        }

        private void picSymbolTo_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picSymbolTo.BackColor = cdColor.Color;
        }

        private void picGCLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGCLineColor.BackColor = cdColor.Color;
        }
        private void picTeLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picTeLineColor.BackColor = cdColor.Color;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //Choropleth mapping : Exactly same with the function in frmChoroplethwithOverlay HK102915
            string strLayerName = cboSourceLayer.Text;
            if (cboSourceLayer.Text == "" || cboValueField.Text == "" || cboUField.Text == "")
            {
                MessageBox.Show("Assign proper layer and field");
                return;
            }

            int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
            ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFClass = pFLayer.FeatureClass;

            //Create Rendering of Mean Value at Target Layer
            int intGCBreakeCount = Convert.ToInt32(nudGCNClasses.Value);
            string strGCRenderField = cboValueField.Text;

            IGeoFeatureLayer pGeofeatureLayer;

            pGeofeatureLayer = (IGeoFeatureLayer)pFLayer;

            ITable pTable = (ITable)pFClass;
            IClassifyGEN pClassifyGEN;
            switch (cboGCClassify.Text)
            {
                case "Equal Interval":
                    pClassifyGEN = new EqualIntervalClass();
                    break;
                case "Geometrical Interval":
                    pClassifyGEN = new GeometricalInterval();
                    break;
                case "Natural Breaks":
                    pClassifyGEN = new NaturalBreaksClass();
                    break;
                case "Quantile":
                    pClassifyGEN = new QuantileClass();
                    break;
                case "StandardDeviation":
                    pClassifyGEN = new StandardDeviationClass();
                    break;
                default:
                    pClassifyGEN = new NaturalBreaksClass();
                    break;
            }

            //Need to be changed 1/29/15
            ITableHistogram pTableHistogram = new TableHistogramClass();
            pTableHistogram.Field = strGCRenderField;
            pTableHistogram.Table = pTable;
            IHistogram pHistogram = (IHistogram)pTableHistogram;

            object xVals, frqs;
            pHistogram.GetHistogram(out xVals, out frqs);
            pClassifyGEN.Classify(xVals, frqs, intGCBreakeCount);

            ClassBreaksRenderer pRender = new ClassBreaksRenderer();
            double[] cb = (double[])pClassifyGEN.ClassBreaks;
            pRender.Field = strGCRenderField;
            pRender.BreakCount = intGCBreakeCount;
            pRender.MinimumBreak = cb[0];

            //' create our color ramp
            IAlgorithmicColorRamp pColorRamp = new AlgorithmicColorRampClass();
            pColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            IRgbColor pColor1 = new RgbColor();
            IRgbColor pColor2 = new RgbColor();

            //Can Change the color in here!
            pColor1.Red = picSymolfrom.BackColor.R;
            pColor1.Green = picSymolfrom.BackColor.G;
            pColor1.Blue = picSymolfrom.BackColor.B;

            Boolean blnOK = true;
            pColor2.Red = picSymbolTo.BackColor.R;
            pColor2.Green = picSymbolTo.BackColor.G;
            pColor2.Blue = picSymbolTo.BackColor.B;
            pColorRamp.FromColor = pColor1;
            pColorRamp.ToColor = pColor2;
            pColorRamp.Size = intGCBreakeCount;
            pColorRamp.CreateRamp(out blnOK);

            IEnumColors pEnumColors = pColorRamp.Colors;
            pEnumColors.Reset();

            IRgbColor pColorOutline = new RgbColor();
            //Can Change the color in here!
            pColorOutline.Red = picGCLineColor.BackColor.R;
            pColorOutline.Green = picGCLineColor.BackColor.G;
            pColorOutline.Blue = picGCLineColor.BackColor.B;
            double dblGCOutlineSize = Convert.ToDouble(nudGCLinewidth.Value);

            ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
            pOutLines.Width = dblGCOutlineSize;
            pOutLines.Color = (IColor)pColorOutline;

            //' use this interface to set dialog properties
            IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pRender;
            pUIProperties.ColorRamp = "Custom";

            ISimpleFillSymbol pSimpleFillSym;
            //' be careful, indices are different for the diff lists
            for (int j = 0; j < intGCBreakeCount; j++)
            {
                pRender.Break[j] = cb[j + 1];
                pRender.Label[j] = Math.Round(cb[j], 2).ToString() + " - " + Math.Round(cb[j + 1], 2).ToString();
                pUIProperties.LowBreak[j] = cb[j];
                pSimpleFillSym = new SimpleFillSymbolClass();
                pSimpleFillSym.Color = pEnumColors.Next();
                pSimpleFillSym.Outline = pOutLines;
                pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
            }
            pGeofeatureLayer.Renderer = (IFeatureRenderer)pRender;


            //////////////////The Robustness
            int intRoundingDigits = 2;
            int intUncernBreakCount = Convert.ToInt32(nudTeNClasses.Value);
            string strUncerFieldName = cboUField.Text;

            int intUncernIdx = pFClass.FindField(strUncerFieldName);
            int intValueIdx = pFClass.FindField(strGCRenderField);

            //Calculate Robustness
            //Add fld
            int intTempfldIdx = 0;
            string strTempfldName = txtFldName.Text;
            if (chkRobustness.Checked)
            {
                if (pFClass.FindField(strTempfldName) == -1)
                    AddField(pFClass, strTempfldName);
                intTempfldIdx = pFClass.FindField(strTempfldName);
            }

            Chart pChart = new Chart();
            
            IFeature pFeat = null;
            //IFeatureCursor pFCursor = pFClass.Search(null, false);
            IFeatureCursor pFCursor = null;
            if (chkRobustness.Checked)
                pFCursor = pFClass.Update(null, false);
            else
                pFCursor = pFClass.Search(null, false);

            pFeat = pFCursor.NextFeature();
            double[] arrRobustness = new double[pFClass.FeatureCount(null)];

            int i = 0;
            while (pFeat != null)
            {
                for (int j = 0; j < (cb.Length-1); j++)
                {
                    
                    double dblValue = Convert.ToDouble(pFeat.get_Value(intValueIdx));
                    double dblStd = Convert.ToDouble(pFeat.get_Value(intUncernIdx));
                    if(j==0)
                    {
                        if(dblValue >= cb[j] && dblValue <= cb[j+1])
                        {
                            double dblUpperZvalue = (cb[j+1] - dblValue)/dblStd;
                            double dblUpperConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblUpperZvalue);
                            double dblLowerZvalue = (cb[j] - dblValue)/dblStd;
                            double dblLowerConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblLowerZvalue);
                            arrRobustness[i] = dblUpperConfLev - dblLowerConfLev;
                            if(chkRobustness.Checked)
                                pFeat.set_Value(intTempfldIdx, arrRobustness[i]);
                        }

                    }
                    else
                    {
                        if (dblValue > cb[j] && dblValue <= cb[j + 1])
                        {
                            double dblUpperZvalue = (cb[j + 1] - dblValue) / dblStd;
                            double dblUpperConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblUpperZvalue);
                            double dblLowerZvalue = (cb[j] - dblValue) / dblStd;
                            double dblLowerConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblLowerZvalue);
                            arrRobustness[i] = dblUpperConfLev - dblLowerConfLev;
                            if (chkRobustness.Checked)
                                pFeat.set_Value(intTempfldIdx, arrRobustness[i]);
                        }
                    }
                }
                if (chkRobustness.Checked)
                    pFCursor.UpdateFeature(pFeat);

                i++;
                pFeat = pFCursor.NextFeature();

            }

            //Define the intervals (the last class is fixed to 1)
            if (intUncernBreakCount == 1)
                return;

            double[] arrRobustBrks = new double[intUncernBreakCount+1];
            double dblRBrksIntervals = Math.Round(1 / Convert.ToDouble(intUncernBreakCount-1), intRoundingDigits);
            arrRobustBrks[0] = 0;
            for(int j = 1; j < intUncernBreakCount; j++)
                arrRobustBrks[j]= dblRBrksIntervals * j;
            arrRobustBrks[intUncernBreakCount] = 1;



            IFeatureLayer pflUncern = new FeatureLayerClass();
            pflUncern.FeatureClass = pFClass;
            pflUncern.Name = "Robustness";
            pflUncern.Visible = true;

            IGeoFeatureLayer pGFLUncern = (IGeoFeatureLayer)pflUncern;
            pFCursor = pGFLUncern.Search(null, true);
            RobustnessRenderer pRobustnessRenderer = new RobustnessRenderer();
            pRobustnessRenderer.arrRobustBrks = arrRobustBrks;
            pRobustnessRenderer.arrRobustness = arrRobustness;
            pRobustnessRenderer.dblAngle = Convert.ToDouble(nudAngleFrom.Value);
            pRobustnessRenderer.dblFromSep = Convert.ToDouble(nudSeperationFrom.Value);
            pRobustnessRenderer.dblLinewidth = Convert.ToDouble(nudTeLinewidth.Value);
            pRobustnessRenderer.dblToSep = Convert.ToDouble(nudSeperationTo.Value);
            pRobustnessRenderer.intUncernBreakCount = intUncernBreakCount;
            pRobustnessRenderer.pLineColor = pSnippet.getRGB(picTeLineColor.BackColor.R, picTeLineColor.BackColor.G, picTeLineColor.BackColor.B);
            IQueryFilter pQFilter = new QueryFilterClass();


            pRobustnessRenderer.PrepareFilter(pFClass, pQFilter);
            pRobustnessRenderer.Draw(pFCursor, esriDrawPhase.esriDPSelection, pActiveView.ScreenDisplay, null);
            pRobustnessRenderer.CreateLegend();
            pGFLUncern.Renderer = pRobustnessRenderer;

            pActiveView.FocusMap.AddLayer(pGFLUncern as ILayer);
            
            mForm.axMapControl1.ActiveView.Refresh();
            mForm.axTOCControl1.Update();
        }

        //Temp
        private void AddField(IFeatureClass fClass, string name)
        {
            IField newField = new FieldClass();
            IFieldEdit fieldEdit = (IFieldEdit)newField;
            fieldEdit.Name_2 = name;
            fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;

            fClass.AddField(newField);
        }

        private void chkRobustness_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRobustness.Checked)
            {
                lblFldName.Enabled = true;
                txtFldName.Enabled = true;
            }
            else
            {
                lblFldName.Enabled = false;
                txtFldName.Enabled = false;
            }
        }
    }
}
