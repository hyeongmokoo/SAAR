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
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

using RDotNet;
using RDotNet.NativeLibrary;



namespace VisUncertainty
{
    public partial class frmDrawDenPlot : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        public IGeoFeatureLayer pGeofeatureLayer;
        private IFeatureLayer pFLayer;

        public double dblMaxValue;
        public double dblMaxEstimate;
        public double dblMaxUncern;
        public double dblError;

        public bool blnBoxplot = false;
        public bool bln3Dfeature = false;

        private string strOutputGFLName;
        

        public frmDrawDenPlot()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }
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

                pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboValueField.Items.Clear();
                cboUField.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                    cboUField.Items.Add(fields.get_Field(i).Name);
                }

                DrawLegend();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboUField_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawLegend();
        }

        private void frmDrawDenPlot_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pGeofeatureLayer != null && pFLayer != null)
            {
                //Remove Layer
                pActiveView.FocusMap.DeleteLayer(pGeofeatureLayer);

                //Clear Selection
                ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection;
                featureSelection.Clear();

                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
            else
                return;
        }
        private void DrawLegend()
        {
            try
            {
                if (blnBoxplot == true && cboSourceLayer.Text != "" && cboUField.Text != "" && cboValueField.Text != "")
                {

                    mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                    IActiveView pActiveView = mForm.axMapControl1.ActiveView;
                    string strTargetLayerName = cboSourceLayer.Text;
                    string strValueField = cboValueField.Text;
                    string strUncerField = cboUField.Text;

                    clsSnippet pSnippet = new clsSnippet();

                    int intTLayerIdx = pSnippet.GetIndexNumberFromLayerName(pActiveView, strTargetLayerName);

                    ILayer pLayer = mForm.axMapControl1.get_Layer(intTLayerIdx);
                    IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;

                    int intValueFldIdx = pFeatureClass.FindField(strValueField);
                    int intUncerFldIdx = pFeatureClass.FindField(strUncerField);

                    REngine pEngine = mForm.pEngine;

                    pEngine.Evaluate("confLevel <- " + nudConfiLevel.Value.ToString());
                    pEngine.Evaluate("bothlevel <- 1-((100-confLevel)/200)");
                    dblError = pEngine.Evaluate("error <- qnorm(bothlevel)").AsNumeric().First();

                    //Cacluate Max value based on the confidence intervals
                    ICursor pCursor = (ICursor)pFeatureClass.Search(null, false);

                    IRow pRow = pCursor.NextRow();
                    dblMaxValue = 0;
                    double dblTempValue = 0;
                    dblMaxEstimate = 0;
                    dblMaxUncern = 0;
                    double dblTempEstimate = 0;
                    double dblTempUncern = 0;

                    while (pRow != null)
                    {
                        dblTempEstimate = Convert.ToDouble(pRow.get_Value(intValueFldIdx));
                        dblTempUncern = Convert.ToDouble(pRow.get_Value(intUncerFldIdx)) * dblError;
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


                    if (pGeofeatureLayer == null)
                    {
                        pGeofeatureLayer = null;
                        IFeatureLayer pflOutput = new FeatureLayerClass();
                        pflOutput.FeatureClass = pFeatureClass;
                        strOutputGFLName = pFeatureClass.AliasName + "_Uncern";
                        pflOutput.Name = strOutputGFLName;
                        pflOutput.Visible = true;
                        pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;

                        StackedLegned pStackedLegend = new StackedLegned();
                        pStackedLegend.m_strOriRenderField = strValueField;
                        pStackedLegend.m_strUncernRenderField = strUncerField;
                        pStackedLegend.dblMaxValue = dblMaxValue;
                        pStackedLegend.dblMaxEstimate = dblMaxEstimate;
                        pStackedLegend.dblMaxUncern = dblMaxUncern;
                        pStackedLegend.bln3Dfeature = bln3Dfeature;
                        pStackedLegend.CreateLegend();
                        pStackedLegend.PrepareFilter(pFeatureClass, null);
                        pGeofeatureLayer.Renderer = pStackedLegend;

                        pActiveView.FocusMap.AddLayer(pGeofeatureLayer);
                    }
                    else
                    {
                        int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strOutputGFLName);
                        ILayer pNewLayer = mForm.axMapControl1.get_Layer(intLIndex);

                        IFeatureLayer pNewFLayer = pNewLayer as IFeatureLayer;
                        pGeofeatureLayer = pNewFLayer as IGeoFeatureLayer;

                        StackedLegned pStackedLegend = new StackedLegned();
                        pStackedLegend.m_strOriRenderField = strValueField;
                        pStackedLegend.m_strUncernRenderField = strUncerField;
                        pStackedLegend.dblMaxValue = dblMaxValue;
                        pStackedLegend.dblMaxEstimate = dblMaxEstimate;
                        pStackedLegend.dblMaxUncern = dblMaxUncern;
                        pStackedLegend.bln3Dfeature = bln3Dfeature;
                        pStackedLegend.CreateLegend();
                        //pStackedLegend.PrepareFilter(pFeatureClass, null);
                        pGeofeatureLayer.Renderer = pStackedLegend;

                        //int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strOutputGFLName);
                        //ILayer pNewLayer = mForm.axMapControl1.get_Layer(intLIndex);

                        //IFeatureLayer pNewFLayer = pNewLayer as IFeatureLayer;
                        pNewFLayer = (IFeatureLayer)pGeofeatureLayer;

                    }

                    pActiveView.Refresh();

                }
                else
                    return;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void chk3D_CheckedChanged(object sender, EventArgs e)
        {
            if (chk3D.Checked == true)
                bln3Dfeature = true;
            else
                bln3Dfeature = false;

            DrawLegend();
        }

        private void nudConfiLevel_Click(object sender, EventArgs e)
        {
            DrawLegend();
        }

        private void cboValueField_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawLegend();
        }

    }
}
