using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.DataSourcesRaster;

namespace VisUncertainty
{
    public partial class frmChoroplethwithOverlay : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public frmChoroplethwithOverlay()
        {
            try
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
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void picUnColorFrom_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picCoColorFrom.BackColor = cdColor.Color;
        }

        private void picUnColorTo_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picCoColorTo.BackColor = cdColor.Color;
        }

        private void picUnLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picCoLineColor.BackColor = cdColor.Color;
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

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            pSnippet.AddFieldsForTwoCbo(cboSourceLayer.Text, pActiveView, mForm, cboValueField, cboUField);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkNewLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewLayer.Checked == true)
                txtNewLayer.Enabled = true;
            else
            {
               txtNewLayer.Text = "";
               txtNewLayer.Enabled = false;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
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
                if (chkNewLayer.Checked == true)
                {
                    IFeatureLayer pflOutput = new FeatureLayerClass();
                    pflOutput.FeatureClass = pFClass;
                    pflOutput.Name = txtNewLayer.Text;
                    pflOutput.Visible = true;

                    pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;
                }
                else
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
                ITableHistogram pTableHistogram = new BasicTableHistogramClass();
                pTableHistogram.Field = strGCRenderField;
                pTableHistogram.Table = pTable;
                IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram;

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
                if (chkNewLayer.Checked == true)
                    mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGeofeatureLayer);



                ////* Uncertainty Part *////
                //Declare variables in if parts

                if (tcUncern.SelectedIndex == 0) //Graduated Color
                {
                    int intUncernBreakCount = Convert.ToInt32(nudCoNClasses.Value);
                    string strUncerFieldName = cboUField.Text;

                    IFeatureLayer pflUncern = new FeatureLayerClass();
                    pflUncern.FeatureClass = pFClass;
                    pflUncern.Name = cboSourceLayer.Text + " Uncertainty";
                    pflUncern.Visible = true;

                    IGeoFeatureLayer pGFLUncern = (IGeoFeatureLayer)pflUncern;
                    switch (cboTeClassify.Text)
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
                    pTableHistogram = new BasicTableHistogramClass();
                    pTableHistogram.Field = strUncerFieldName;
                    pTableHistogram.Table = pTable;
                    pHistogram = (IBasicHistogram)pTableHistogram;

                    pHistogram.GetHistogram(out xVals, out frqs);
                    pClassifyGEN.Classify(xVals, frqs, intUncernBreakCount);

                    pRender = new ClassBreaksRenderer();
                    cb = (double[])pClassifyGEN.ClassBreaks;
                    pRender.Field = strUncerFieldName;
                    pRender.BreakCount = intUncernBreakCount;
                    pRender.MinimumBreak = cb[0];

                    IClassBreaksUIProperties pUIColProperties = (IClassBreaksUIProperties)pRender;
                    pUIColProperties.ColorRamp = "Custom";

                    pColorRamp = new AlgorithmicColorRampClass();
                    pColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                    pColor1 = new RgbColor();
                    pColor2 = new RgbColor();

                    //Can Change the color in here!
                    pColor1 = pSnippet.getRGB(picCoColorFrom.BackColor.R, picCoColorFrom.BackColor.G, picCoColorFrom.BackColor.B);
                    pColor2 = pSnippet.getRGB(picCoColorTo.BackColor.R, picCoColorTo.BackColor.G, picCoColorTo.BackColor.B);
                    if (pColor1 == null || pColor2 == null)
                        return;

                    blnOK = true;
                    pColorRamp.FromColor = pColor1;
                    pColorRamp.ToColor = pColor2;
                    pColorRamp.Size = intUncernBreakCount;
                    pColorRamp.CreateRamp(out blnOK);

                    pEnumColors = pColorRamp.Colors;
                    pEnumColors.Reset();

                    pColorOutline = pSnippet.getRGB(picCoLineColor.BackColor.R, picCoLineColor.BackColor.G, picCoLineColor.BackColor.B);
                    if (pColorOutline == null)
                        return;

                    double dblCoOutlineSize = Convert.ToDouble(nudCoLinewidth.Value);

                    pOutLines = new CartographicLineSymbol();
                    pOutLines.Width = dblCoOutlineSize;
                    pOutLines.Color = (IColor)pColorOutline;

                    //' use this interface to set dialog properties
                    pUIColProperties = (IClassBreaksUIProperties)pRender;
                    pUIColProperties.ColorRamp = "Custom";

                    ISimpleMarkerSymbol pSimpleMarkerSym;
                    double dblCoSymSize = Convert.ToDouble(nudCoSymbolSize.Value);
                    //' be careful, indices are different for the diff lists
                    for (int j = 0; j < intUncernBreakCount; j++)
                    {
                        pRender.Break[j] = cb[j + 1];
                        pRender.Label[j] = Math.Round(cb[j], 2).ToString() + " - " + Math.Round(cb[j + 1], 2).ToString();
                        pUIColProperties.LowBreak[j] = cb[j];
                        pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                        pSimpleMarkerSym.Size = dblCoSymSize;
                        pSimpleMarkerSym.Color = pEnumColors.Next();
                        pSimpleMarkerSym.Outline = true;
                        pSimpleMarkerSym.OutlineColor = pColorOutline;
                        pSimpleMarkerSym.OutlineSize = dblCoOutlineSize;
                        pRender.Symbol[j] = (ISymbol)pSimpleMarkerSym;
                    }

                    pGFLUncern.Renderer = (IFeatureRenderer)pRender;
                    mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGFLUncern);
                }
                else if (tcUncern.SelectedIndex == 1) //Texture
                {
                    //Create Rendering of Uncertainty at Target Layer
                    int intUncernBreakCount = Convert.ToInt32(nudTeNClasses.Value);
                    string strUncerFieldName = cboUField.Text;

                    IFeatureLayer pflUncern = new FeatureLayerClass();
                    pflUncern.FeatureClass = pFClass;
                    pflUncern.Name = cboSourceLayer.Text + " Uncertainty";
                    pflUncern.Visible = true;

                    IGeoFeatureLayer pGFLUncern = (IGeoFeatureLayer)pflUncern;
                    switch (cboTeClassify.Text)
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
                    pTableHistogram = new BasicTableHistogramClass();
                    pTableHistogram.Field = strUncerFieldName;
                    pTableHistogram.Table = pTable;
                    pHistogram = (IBasicHistogram)pTableHistogram;

                    pHistogram.GetHistogram(out xVals, out frqs);
                    pClassifyGEN.Classify(xVals, frqs, intUncernBreakCount);

                    pRender = new ClassBreaksRenderer();
                    cb = (double[])pClassifyGEN.ClassBreaks;
                    pRender.Field = strUncerFieldName;
                    pRender.BreakCount = intUncernBreakCount;
                    pRender.MinimumBreak = cb[0];

                    IClassBreaksUIProperties pUITexProperties = (IClassBreaksUIProperties)pRender;
                    pUITexProperties.ColorRamp = "Custom";

                    ILineFillSymbol pLineFillSym = new LineFillSymbolClass();
                    double dblFromSep = Convert.ToDouble(nudSeperationFrom.Value);
                    double dblToSep = Convert.ToDouble(nudSeperationTo.Value);
                    double dblInstantSep = (dblFromSep - dblToSep) / Convert.ToDouble(intUncernBreakCount - 1);
                    double dblFromAngle = Convert.ToDouble(nudAngleFrom.Value);
                    double dblToAngle = Convert.ToDouble(nudAngleFrom.Value); // Remove the angle part (04/16)
                    double dblInstantAngle = (dblToAngle - dblFromAngle) / Convert.ToDouble(intUncernBreakCount - 1);
                    double dblLinewidth = Convert.ToDouble(nudTeLinewidth.Value);
                    IRgbColor pLineColor = new RgbColor();
                    pLineColor.Red = picTeLineColor.BackColor.R;
                    pLineColor.Green = picTeLineColor.BackColor.G;
                    pLineColor.Blue = picTeLineColor.BackColor.B;

                    //' be careful, indices are different for the diff lists
                    for (int j = 0; j < intUncernBreakCount; j++)
                    {

                        pRender.Break[j] = cb[j + 1];
                        pRender.Label[j] = Math.Round(cb[j], 5).ToString() + " - " + Math.Round(cb[j + 1], 5).ToString();
                        pUITexProperties.LowBreak[j] = cb[j];
                        pLineFillSym = new LineFillSymbolClass();
                        pLineFillSym.Angle = dblFromAngle + (dblInstantAngle * Convert.ToDouble(j));
                        pLineFillSym.Color = pLineColor;
                        pLineFillSym.Separation = dblFromSep - (dblInstantSep * Convert.ToDouble(j));
                        pLineFillSym.LineSymbol.Width = dblLinewidth;
                        pRender.Symbol[j] = (ISymbol)pLineFillSym;
                    }
                    pGFLUncern.Renderer = (IFeatureRenderer)pRender;
                    mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGFLUncern);
                }
                else if (tcUncern.SelectedIndex == 2) //For Proportional Symbols
                {
                    string strUncerFieldName = cboUField.Text;
                    double dblMinPtSize = Convert.ToDouble(nudProSymbolSize.Value);
                    double dblLineWidth = Convert.ToDouble(nudProLinewidth.Value);

                    IFeatureLayer pflUncern = new FeatureLayerClass();
                    pflUncern.FeatureClass = pFClass;
                    pflUncern.Name = cboSourceLayer.Text + " Uncertainty";
                    pflUncern.Visible = true;

                    //Find Fields
                    int intUncernIdx = pTable.FindField(strUncerFieldName);

                    //Find Min value 
                    //Set to initial value for min
                    IField pUncernField = pTable.Fields.get_Field(intUncernIdx);
                    ICursor pCursor = pTable.Search(null, false);
                    IDataStatistics pDataStat = new DataStatisticsClass();
                    pDataStat.Field = pUncernField.Name;
                    pDataStat.Cursor = pCursor;
                    IStatisticsResults pStatResults = pDataStat.Statistics;
                    double dblMinValue = pStatResults.Minimum;
                    double dblMaxValue = pStatResults.Maximum;
                    pCursor.Flush();


                    IRgbColor pSymbolRgb = pSnippet.getRGB(picProSymbolColor.BackColor.R, picProSymbolColor.BackColor.G, picProSymbolColor.BackColor.B);
                    if (pSymbolRgb == null)
                        return;

                    IRgbColor pLineRgb = pSnippet.getRGB(picProiLineColor.BackColor.R, picProiLineColor.BackColor.G, picProiLineColor.BackColor.B);
                    if (pLineRgb == null)
                        return;

                    ISimpleMarkerSymbol pSMarkerSym = new SimpleMarkerSymbolClass();
                    pSMarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSMarkerSym.Size = dblMinPtSize;
                    pSMarkerSym.OutlineSize = dblLineWidth;
                    pSMarkerSym.Outline = true;
                    pSMarkerSym.OutlineColor = (IColor)pLineRgb;
                    pSMarkerSym.Color = (IColor)pSymbolRgb;

                    IGeoFeatureLayer pGFLUncern = (IGeoFeatureLayer)pflUncern;
                    IProportionalSymbolRenderer pUncernRender = new ProportionalSymbolRendererClass();
                    pUncernRender.LegendSymbolCount = 2; //Need to be changed 0219
                    pUncernRender.Field = strUncerFieldName;
                    pUncernRender.MaxDataValue = dblMaxValue;
                    pUncernRender.MinDataValue = dblMinValue;
                    pUncernRender.MinSymbol = (ISymbol)pSMarkerSym;
                    pUncernRender.ValueUnit = esriUnits.esriUnknownUnits;
                    pUncernRender.BackgroundSymbol = null;
                    pUncernRender.CreateLegendSymbols();

                    pGFLUncern.Renderer = (IFeatureRenderer)pUncernRender;
                    mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGFLUncern);
                }
                else if (tcUncern.SelectedIndex == 3) // Bar
                {
                    string strUncerFieldName = cboUField.Text;
                    double dblMaxLength = Convert.ToDouble(nudMaxBarHeight.Value);
                    double dblBarWidth = Convert.ToDouble(nudBarWidth.Value);

                    IFeatureLayer pflUncern = new FeatureLayerClass();
                    pflUncern.FeatureClass = pFClass;
                    pflUncern.Name = cboSourceLayer.Text + " Uncertainty";
                    pflUncern.Visible = true;

                    int intUncernIdx = pTable.FindField(strUncerFieldName);
                    IField pUncernField = pTable.Fields.get_Field(intUncernIdx);
                    ICursor pCursor = pTable.Search(null, false);
                    IDataStatistics pDataStat = new DataStatisticsClass();
                    pDataStat.Field = pUncernField.Name;
                    pDataStat.Cursor = pCursor;
                    IStatisticsResults pStatResults = pDataStat.Statistics;
                    double dblMaxValue = pStatResults.Maximum;
                    pCursor.Flush();


                    IChartRenderer chartRenderer = new ChartRendererClass();
                    IRendererFields rendererFields = chartRenderer as IRendererFields;
                    rendererFields.AddField(strUncerFieldName);

                    IBarChartSymbol barChartSymbol = new BarChartSymbolClass();
                    barChartSymbol.Width = dblBarWidth;
                    IMarkerSymbol markerSymbol = barChartSymbol as IMarkerSymbol;
                    markerSymbol.Size = dblMaxLength;
                    IChartSymbol chartSymbol = barChartSymbol as IChartSymbol;
                    chartSymbol.MaxValue = dblMaxValue;
                    ISymbolArray symbolArray = barChartSymbol as ISymbolArray;
                    IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                    fillSymbol.Color = pSnippet.getRGB(picBarSymCol.BackColor.R, picBarSymCol.BackColor.G, picBarSymCol.BackColor.B);
                    if (fillSymbol.Color == null)
                        return;
                    symbolArray.AddSymbol(fillSymbol as ISymbol);

                    if (chk3D.Checked)
                    {
                        I3DChartSymbol p3DChartSymbol = barChartSymbol as I3DChartSymbol;
                        p3DChartSymbol.Display3D = true;
                        p3DChartSymbol.Thickness = 3;
                    }
                    chartRenderer.ChartSymbol = barChartSymbol as IChartSymbol;
                    SimpleFillSymbol pBaseFillSym = new SimpleFillSymbolClass();
                    //pBaseFillSym.Color = pSnippet.getRGB(picBarSymCol.BackColor.R, picBarSymCol.BackColor.G, picBarSymCol.BackColor.B);
                    //chartRenderer.BaseSymbol = pBaseFillSym as ISymbol;
                    chartRenderer.UseOverposter = false;
                    chartRenderer.CreateLegend();
                    IGeoFeatureLayer pGFLUncern = (IGeoFeatureLayer)pflUncern;
                    pGFLUncern.Renderer = (IFeatureRenderer)chartRenderer;
                    mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGFLUncern);

                }
                #region illumination
                //This function is not applied in this version. 032317 HK
                //}
                //    else if (tcUncern.SelectedIndex == 4) //illumination
                //    {
                //        frmProgress pfrmProgress = new frmProgress();
                //        pfrmProgress.lblStatus.Text = "Processing:";
                //        pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                //        pfrmProgress.Show();

                //        string strUncerFieldName = cboUField.Text;

                //        IGeoDataset geoDataset_output = createRasterfromPolygon(pFClass, strUncerFieldName, pFLayer, 100);

                //        double altitude = Convert.ToDouble(nudAltitude.Value);
                //        double azimuth = Convert.ToDouble(nudAzimuth.Value);
                //        object zFactor = Convert.ToDouble(nudZFactor.Value);


                //        ISurfaceOp2 pSurfOP = new RasterSurfaceOpClass();
                //        IRaster pOutputDS = (IRaster)pSurfOP.HillShade(geoDataset_output, azimuth, altitude, true, ref zFactor);

                //        ((IDataset)geoDataset_output).Delete();
                //        // Create a raster for viewing
                //        ESRI.ArcGIS.Carto.IRasterLayer rasterLayer = new ESRI.ArcGIS.Carto.RasterLayerClass();
                //        rasterLayer.CreateFromRaster(pOutputDS);

                //        //Calculate hillshade value at slope 0 to set as background value
                //        double dblRadian = (Math.PI / 180) * (90 - altitude);
                //        double dblBackValue = Math.Truncate(255 * Math.Cos(dblRadian));

                //        IRasterStretch pRasterStretch = new RasterStretchColorRampRendererClass();
                //        IColor pColor = new RgbColorClass();
                //        pColor.NullColor = true;
                //        pColor.Transparency = 0;
                //        pRasterStretch.Background = true;
                //        pRasterStretch.BackgroundColor = pColor;
                //        pRasterStretch.set_BackgroundValues(ref dblBackValue);

                //        rasterLayer.Name = "Uncertainty of " + strGCRenderField;
                //        rasterLayer.Renderer = pRasterStretch as IRasterRenderer;
                //        rasterLayer.Renderer.Update();

                //        //Apply Transparency
                //        ILayerEffects pLayerEffect = (ILayerEffects)rasterLayer;
                //        pLayerEffect.Transparency = Convert.ToInt16(nudTransparent.Value);

                //        pfrmProgress.Close();
                //        // Add the raster to the map
                //        pActiveView.FocusMap.AddLayer(rasterLayer);

                //    }

                //    mForm.axMapControl1.ActiveView.Refresh();
                //    mForm.axTOCControl1.Update();
                #endregion
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void picTeLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picTeLineColor.BackColor = cdColor.Color;
        }

        private void picBarSymCol_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picBarSymCol.BackColor = cdColor.Color;
        }

        private void picProSymbolColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picProSymbolColor.BackColor = cdColor.Color;
        }

        private void picProiLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picProiLineColor.BackColor = cdColor.Color;
        }

        #region function not used
        //The function below is used for illumination that is not applied in this version 032317 HK
        //private IGeoDataset createRasterfromPolygon(IFeatureClass pFClass, string strUncerFieldName, IFeatureLayer pFLayer, double dblCellSize)
        //{
        //    try
        //    {
        //        ESRI.ArcGIS.GeoAnalyst.IFeatureClassDescriptor featureClassDescriptor = new ESRI.ArcGIS.GeoAnalyst.FeatureClassDescriptorClass();
        //        featureClassDescriptor.Create(pFClass, null, strUncerFieldName);

        //        ESRI.ArcGIS.Geodatabase.IGeoDataset geoDataset = (ESRI.ArcGIS.Geodatabase.IGeoDataset)featureClassDescriptor; // Explicit Cast

        //        ESRI.ArcGIS.Geometry.ISpatialReference spatialReference = geoDataset.SpatialReference;

        //        Create a RasterMaker operator
        //        ESRI.ArcGIS.GeoAnalyst.IConversionOp conversionOp = new ESRI.ArcGIS.GeoAnalyst.RasterConversionOpClass();

        //        ESRI.ArcGIS.Geodatabase.IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesRaster.RasterWorkspaceFactoryClass();

        //        set output workspace
        //        ESRI.ArcGIS.Geodatabase.IDataset dataset = (ESRI.ArcGIS.Geodatabase.IDataset)(pFLayer);

        //        ESRI.ArcGIS.Geodatabase.IWorkspace workspace = workspaceFactory.OpenFromFile(dataset.Workspace.PathName, 0);

        //        Create analysis environment
        //        ESRI.ArcGIS.GeoAnalyst.IRasterAnalysisEnvironment rasterAnalysisEnvironment = (ESRI.ArcGIS.GeoAnalyst.IRasterAnalysisEnvironment)conversionOp; // Explicit Cast
        //        rasterAnalysisEnvironment.OutWorkspace = workspace;

        //        ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
        //        envelope = geoDataset.Extent;


        //        object object_cellSize = (System.Object)dblCellSize;
        //        rasterAnalysisEnvironment.SetCellSize(ESRI.ArcGIS.GeoAnalyst.esriRasterEnvSettingEnum.esriRasterEnvValue, ref object_cellSize);

        //        Set output extent
        //        object object_Envelope = (System.Object)envelope; // Explict Cast
        //        object object_Missing = System.Type.Missing;
        //        rasterAnalysisEnvironment.SetExtent(ESRI.ArcGIS.GeoAnalyst.esriRasterEnvSettingEnum.esriRasterEnvValue, ref object_Envelope, ref object_Missing);

        //        Set output spatial reference
        //        rasterAnalysisEnvironment.OutSpatialReference = spatialReference;

        //        Perform spatial operation
        //        ESRI.ArcGIS.Geodatabase.IRasterDataset rasterDataset = new ESRI.ArcGIS.DataSourcesRaster.RasterDatasetClass();

        //        Create the new raster name that meets the coverage naming convention
        //        System.String string_RasterName = pFClass.AliasName;
        //        System.String string_RasterName = "traster1";
        //        if (string_RasterName.Length > 13)
        //        {
        //            string_RasterName = string_RasterName.Substring(0, 13);
        //        }
        //        string_RasterName = string_RasterName.Replace(" ", "_");

        //        rasterDataset = conversionOp.ToRasterDataset(geoDataset, "GRID", workspace, string_RasterName);
        //        ESRI.ArcGIS.Geodatabase.IGeoDataset geoDataset_output = (ESRI.ArcGIS.Geodatabase.IGeoDataset)rasterDataset;

        //        return geoDataset_output;
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return null;
        //    }

        //}
        #endregion
    }
}
