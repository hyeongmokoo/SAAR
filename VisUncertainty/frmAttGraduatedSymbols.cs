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

using RDotNet;
using RDotNet.NativeLibrary;

using UncernVis.BivariateRenderer;


namespace VisUncertainty
{
    public partial class frmAttGraduatedSymbols : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;

        public frmAttGraduatedSymbols()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)pActiveView.FocusMap.get_Layer(i);

                    if(pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void picLineColor_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult DR = cdColor.ShowDialog();
                if (DR == DialogResult.OK)
                    picLineColor.BackColor = cdColor.Color;
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

        private void picSymbolColor_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult DR = cdColor.ShowDialog();
                if (DR == DialogResult.OK)
                    picSymbolColor.BackColor = cdColor.Color;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsSnippet pSnippet = new clsSnippet();
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboValueField.Items.Clear();
                CboUField.Items.Clear();
                //cboConField.Items.Clear();
                cboValueField.Text = "";
                CboUField.Text = "";

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                    CboUField.Items.Add(fields.get_Field(i).Name);
                    //cboConField.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void chkNewLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNewLayer.Checked == true)
                txtNewLayer.Enabled = true;
            else
                txtNewLayer.Enabled = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                //Declare variables
                clsSnippet pSnippet = new clsSnippet();
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                string strOriRenderField = cboValueField.Text;
                string strUncernRenderField = CboUField.Text;
                string strConLevelField = nudConfidenceLevel.Value.ToString();

                if (strOriRenderField == "" || strUncernRenderField == "")
                {
                    MessageBox.Show("Plese choose field names");
                    return;
                }

                //Find Fields
                ITable pTable = (ITable)pFClass;
                int intOriIdx = pTable.FindField(strOriRenderField);
                int intUncernIdx = pTable.FindField(strUncernRenderField);

                //Create Geofeature Layer
                IGeoFeatureLayer pGeofeatureLayer = null;
                if (chkNewLayer.Checked == true)
                {
                    IFeatureLayer pflOutput = new FeatureLayerClass();
                    pflOutput.FeatureClass = pFClass;
                    pflOutput.Name = txtNewLayer.Text;
                    pflOutput.Visible = true;
                    pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;
                }
                else
                {
                    pGeofeatureLayer = (IGeoFeatureLayer)pFLayer;
                }

                //Calculate confidence levels
                Chart pChart = new Chart();
                double dblConInstance = pChart.DataManipulator.Statistics.InverseNormalDistribution(Convert.ToDouble(nudConfidenceLevel.Value) / 100);


                if (tcUncer.SelectedIndex == 0) //Proportional symbol composite layers
                {
                    double dblMinPtSize = Convert.ToDouble(nudSymbolSize.Value);

                    //Find max value at attribute to set to initial value for finding Min value at composite symbols
                    IField pOriField = pTable.Fields.get_Field(intOriIdx);
                    ICursor pCursor = pTable.Search(null, false);
                    IDataStatistics pDataStat = new DataStatisticsClass();
                    pDataStat.Field = pOriField.Name;
                    pDataStat.Cursor = pCursor;
                    IStatisticsResults pStatResults = pDataStat.Statistics;

                    double dblMinValue = pStatResults.Maximum;
                    pCursor.Flush();

                    pCursor = pTable.Search(null, false);
                    IRow pRow = pCursor.NextRow();
                    double dblValue = 0;

                    //Cacluate Min and Max value based on the confidence intervals
                    //Min
                    while (pRow != null)
                    {
                        dblValue = Convert.ToDouble(pRow.get_Value(intOriIdx)) - (Convert.ToDouble(pRow.get_Value(intUncernIdx)) * dblConInstance);
                        if (dblValue < dblMinValue)
                            dblMinValue = dblValue;
                        pRow = pCursor.NextRow();
                    }

                    //Max
                    pCursor.Flush();

                    double dblMaxValue = 0;
                    pCursor = pTable.Search(null, false);
                    pRow = pCursor.NextRow();
                    dblValue = 0;

                    //Cacluate Min and Max value based on the confidence intervals
                    while (pRow != null)
                    {
                        dblValue = Convert.ToDouble(pRow.get_Value(intOriIdx)) + (Convert.ToDouble(pRow.get_Value(intUncernIdx)) * dblConInstance);
                        if (dblValue > dblMaxValue)
                            dblMaxValue = dblValue;
                        pRow = pCursor.NextRow();
                    }


                    //To adjust min value to 1, if the min value is zero
                    double dbladjuctMinvalue = 0;
                    if (dblMinValue <= 0)
                    {
                        dbladjuctMinvalue = (0 - dblMinValue) + 1;
                        dblMinValue = dblMinValue + dbladjuctMinvalue;
                    }


                    //Loading uncertainty proportional symbol renderer
                    IDisplay pDisplay = pActiveView.ScreenDisplay;

                    UncernVis.BivariateRenderer.IPropCompositeRenderer pUnProprotional = new UncernVis.BivariateRenderer.PropCompositeRenderer();

                    pUnProprotional.m_dblMinPtSize = dblMinPtSize;
                    pUnProprotional.m_dblMinValue = dblMinValue;
                    pUnProprotional.m_dblMaxValue = dblMaxValue;

                    pUnProprotional.m_dblOutlineSize = Convert.ToDouble(nudLinewidth.Value);
                    pUnProprotional.m_dblAdjustedMinValue = dbladjuctMinvalue;

                    IRgbColor pSymbolRgb = new RgbColorClass();
                    pSymbolRgb.Red = picSymbolColor.BackColor.R;
                    pSymbolRgb.Green = picSymbolColor.BackColor.G;
                    pSymbolRgb.Blue = picSymbolColor.BackColor.B;

                    IRgbColor pLineRgb = new RgbColorClass();
                    pLineRgb.Red = picLineColor.BackColor.R;
                    pLineRgb.Green = picLineColor.BackColor.G;
                    pLineRgb.Blue = picLineColor.BackColor.B;

                    pUnProprotional.m_pLineRgb = pLineRgb;
                    pUnProprotional.m_pSymbolRgb = pSymbolRgb;

                    pUnProprotional.m_strUncernRenderField = strUncernRenderField;
                    pUnProprotional.m_strOriRenderField = strOriRenderField;

                    pUnProprotional.m_dblConInstance = dblConInstance;
                    pUnProprotional.m_pGeometryTypes = pFClass.ShapeType;

                    //Create Legend
                    pUnProprotional.CreateLegend();

                    pGeofeatureLayer.Renderer = (IFeatureRenderer)pUnProprotional;


                    if (chkNewLayer.Checked == true)
                        mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGeofeatureLayer);
                    else
                    {
                        pFLayer = (IFeatureLayer)pGeofeatureLayer;
                    }

                }
                else if (tcUncer.SelectedIndex == 1) // Chart composite symbols
                {
                    double dblChartWidth = Convert.ToDouble(nudChartWidth.Value);
                    double dblChartSize = Convert.ToDouble(nudChartSize.Value);
                    double dblThickness = Convert.ToDouble(nudThickness.Value);

                    //Cacluate Max value based on the confidence intervals
                    ICursor pCursor = (ICursor)pFClass.Search(null, false);

                    IRow pRow = pCursor.NextRow();
                    double dblMaxValue = 0;
                    double dblTempValue = 0;
                    double dblMaxEstimate = 0;
                    double dblMaxUncern = 0;
                    double dblTempEstimate = 0;
                    double dblTempUncern = 0;

                    while (pRow != null)
                    {
                        dblTempEstimate = Convert.ToDouble(pRow.get_Value(intOriIdx));
                        dblTempUncern = Convert.ToDouble(pRow.get_Value(intUncernIdx)) * dblConInstance;
                        dblTempValue = dblTempEstimate + dblTempUncern;

                        if (dblTempValue > dblMaxValue)
                        {
                            dblMaxValue = dblTempValue;
                            dblMaxEstimate = dblTempEstimate;
                            dblMaxUncern = dblTempUncern;
                        }
                        pRow = pCursor.NextRow();
                    }
                    pCursor.Flush();


                    //IFeatureCursor pFCursor = pGeofeatureLayer.Search(null, true);

                    IChartCompositeRenderer pChartCompositeRenderer = new ChartCompositeRenderer();

                    pChartCompositeRenderer.m_dblConInstance = dblConInstance;

                    pChartCompositeRenderer.m_dblMaxValue = dblMaxValue;
                    pChartCompositeRenderer.m_bln3Dfeature = chk3D.Checked;
                    pChartCompositeRenderer.m_strOriRenderField = strOriRenderField;
                    pChartCompositeRenderer.m_strUncernRenderField = strUncernRenderField;

                    pChartCompositeRenderer.m_dblMaxEstimate = dblMaxEstimate;
                    pChartCompositeRenderer.m_dblMaxUncern = dblMaxUncern;

                    pChartCompositeRenderer.m_dblBarWidth = dblChartWidth;
                    pChartCompositeRenderer.m_dblBarSize = dblChartSize;
                    pChartCompositeRenderer.m_dblThickness = dblThickness;

                    pChartCompositeRenderer.CreateLegend();
                    pGeofeatureLayer.Renderer = pChartCompositeRenderer as IFeatureRenderer;
                    if (chkNewLayer.Checked == true)
                        mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGeofeatureLayer);
                    else
                    {
                        pFLayer = (IFeatureLayer)pGeofeatureLayer;
                    }
                }
                else if (tcUncer.SelectedIndex == 2) //For Line Symbol
                {
                    double dblMinPtSize = Convert.ToDouble(nudMinWidth.Value);

                    //Find max value at attribute to set to initial value for finding Min value at composite symbols
                    IField pOriField = pTable.Fields.get_Field(intOriIdx);
                    ICursor pCursor = pTable.Search(null, false);
                    IDataStatistics pDataStat = new DataStatisticsClass();
                    pDataStat.Field = pOriField.Name;
                    pDataStat.Cursor = pCursor;
                    IStatisticsResults pStatResults = pDataStat.Statistics;

                    double dblMinValue = pStatResults.Maximum;
                    pCursor.Flush();

                    pCursor = pTable.Search(null, false);
                    IRow pRow = pCursor.NextRow();
                    double dblValue = 0;

                    //Cacluate Min and Max value based on the confidence intervals
                    //Min
                    while (pRow != null)
                    {
                        dblValue = Convert.ToDouble(pRow.get_Value(intOriIdx)) - (Convert.ToDouble(pRow.get_Value(intUncernIdx)) * dblConInstance);
                        if (dblValue < dblMinValue)
                            dblMinValue = dblValue;
                        pRow = pCursor.NextRow();
                    }

                    //Max
                    pCursor.Flush();

                    double dblMaxValue = 0;
                    pCursor = pTable.Search(null, false);
                    pRow = pCursor.NextRow();
                    dblValue = 0;

                    //Cacluate Min and Max value based on the confidence intervals
                    while (pRow != null)
                    {
                        dblValue = Convert.ToDouble(pRow.get_Value(intOriIdx)) + (Convert.ToDouble(pRow.get_Value(intUncernIdx)) * dblConInstance);
                        if (dblValue > dblMaxValue)
                            dblMaxValue = dblValue;
                        pRow = pCursor.NextRow();
                    }


                    //To adjust min value to 1, if the min value is zero
                    double dbladjuctMinvalue = 0;
                    if (dblMinValue <= 0)
                    {
                        dbladjuctMinvalue = (0 - dblMinValue) + 1;
                        dblMinValue = dblMinValue + dbladjuctMinvalue;
                    }


                    //Loading uncertainty proportional symbol renderer
                    IDisplay pDisplay = pActiveView.ScreenDisplay;

                    UncernVis.BivariateRenderer.IPropCompositeRenderer pUnProprotional = new UncernVis.BivariateRenderer.PropCompositeRenderer();

                    pUnProprotional.m_dblMinPtSize = dblMinPtSize;
                    pUnProprotional.m_dblMinValue = dblMinValue;
                    pUnProprotional.m_dblMaxValue = dblMaxValue;

                    pUnProprotional.m_dblOutlineSize = 0;
                    pUnProprotional.m_dblAdjustedMinValue = dbladjuctMinvalue;

                    IRgbColor pSymbolRgb = pSnippet.getRGB(picLineConColor.BackColor.R, picLineConColor.BackColor.G, picLineConColor.BackColor.B); 
                    //pSymbolRgb.Red = picSymbolColor.BackColor.R;
                    //pSymbolRgb.Green = picSymbolColor.BackColor.G;
                    //pSymbolRgb.Blue = picSymbolColor.BackColor.B;

                    IRgbColor pLineRgb = pSnippet.getRGB(picLineCntColor.BackColor.R, picLineCntColor.BackColor.G, picLineCntColor.BackColor.B); 
                    //pLineRgb.Red = picLineColor.BackColor.R;
                    //pLineRgb.Green = picLineColor.BackColor.G;
                    //pLineRgb.Blue = picLineColor.BackColor.B;

                    pUnProprotional.m_pLineRgb = pLineRgb;
                    pUnProprotional.m_pSymbolRgb = pSymbolRgb;

                    pUnProprotional.m_strUncernRenderField = strUncernRenderField;
                    pUnProprotional.m_strOriRenderField = strOriRenderField;

                    pUnProprotional.m_dblConInstance = dblConInstance;
                    pUnProprotional.m_pGeometryTypes = pFClass.ShapeType;

                    //Create Legend
                    pUnProprotional.CreateLegend();

                    pGeofeatureLayer.Renderer = (IFeatureRenderer)pUnProprotional;


                    if (chkNewLayer.Checked == true)
                        mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGeofeatureLayer);
                    else
                    {
                        pFLayer = (IFeatureLayer)pGeofeatureLayer;
                    }

                }

                mForm.axMapControl1.ActiveView.Refresh();
                mForm.axTOCControl1.Update();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
            
        }

        private void chk3D_CheckedChanged(object sender, EventArgs e)
        {
            if (chk3D.Checked)
                nudThickness.Enabled = true;
            else
                nudThickness.Enabled = false;
        }

        private void tcUncer_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboSourceLayer.Items.Clear();
            cboValueField.Items.Clear();
            CboUField.Items.Clear();

            cboSourceLayer.Text = "";
            cboValueField.Text = "";
            CboUField.Text = "";

            for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)pActiveView.FocusMap.get_Layer(i);

                if (tcUncer.SelectedIndex == 2 && pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                else if(tcUncer.SelectedIndex == 0 || tcUncer.SelectedIndex == 1)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolyline)
                        cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

            }

        }

    }
}
