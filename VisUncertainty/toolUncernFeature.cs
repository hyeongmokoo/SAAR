using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.Text;
using System.Linq;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

using RDotNet;
using RDotNet.NativeLibrary;

namespace VisUncertainty
{
    //Removed 012016 HK


    /// <summary>
    /// Summary description for toolUncernFeature.
    /// </summary>
    [Guid("d3b356a3-31c8-4f1e-bba6-4575b1380396")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.toolUncernFeature")]
    public sealed class toolUncernFeature : BaseTool
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper;
        private MainForm mForm;
        private frmDrawDenPlot pDrawDenplot;

        public toolUncernFeature()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Boxplot Symbol"; //localizable text 
            base.m_caption = "Boxplot Symbol";  //localizable text 
            base.m_message = "";  //localizable text
            base.m_toolTip = "Draws Interactive boxplot symbol";  //localizable text
            base.m_name = "Uncern_DrawSymbol";   //unique id, non-localizable (e.g. "MyCategory_MyTool")

            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            try
            {
                //
                // TODO: change resource name if necessary
                //

                string bitmapResourceName = mForm.strPath + @"\icons\picBoxplot.bmp";
                base.m_bitmap = new Bitmap(bitmapResourceName);
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), GetType().Name + ".cur");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods
        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add toolDenPlot.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {

            pDrawDenplot = new frmDrawDenPlot();
            pDrawDenplot.Show();
            // TODO: Add toolDenPlot.OnClick implementation
            //For This symolization
            pDrawDenplot.blnBoxplot = true;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (pDrawDenplot.cboSourceLayer.Text != "" && pDrawDenplot.cboUField.Text != "" && pDrawDenplot.cboValueField.Text != "")
            {


                IGeoFeatureLayer pGeoFeatureLayer = pDrawDenplot.pGeofeatureLayer;

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                IActiveView pActiveView = mForm.axMapControl1.ActiveView;
                string strTargetLayerName = pDrawDenplot.cboSourceLayer.Text;
                string strValueField = pDrawDenplot.cboValueField.Text;
                string strUncerField = pDrawDenplot.cboUField.Text;

                clsSnippet pSnippet = new clsSnippet();

                //Using IRubberband
                IEnvelope pEnvelop = pSnippet.DrawRectangle(pActiveView);

                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pEnvelop;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                int intTLayerIdx = pSnippet.GetIndexNumberFromLayerName(pActiveView, strTargetLayerName);

                ILayer pLayer = mForm.axMapControl1.get_Layer(intTLayerIdx);
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;
                
                string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                pSpatialFilter.GeometryField = pFLayer.FeatureClass.ShapeFieldName;
                IFeatureClass pFeatureClass = pFLayer.FeatureClass;

                
                int intValueFldIdx = pFeatureClass.FindField(strValueField);
                int intUncerFldIdx = pFeatureClass.FindField(strUncerField);

                IFeatureCursor pFCursor = pGeoFeatureLayer.Search(pSpatialFilter, true);

                StackedChartRenderer pStackedChartRenderer = new StackedChartRenderer();

                pStackedChartRenderer.dblError = pDrawDenplot.dblError;
                pStackedChartRenderer.intValueFldIdx = intValueFldIdx;
                pStackedChartRenderer.intUncerFldIdx = intUncerFldIdx;
                pStackedChartRenderer.dblMaxValue = pDrawDenplot.dblMaxValue;
                pStackedChartRenderer.bln3Dfeature = pDrawDenplot.bln3Dfeature;
                pStackedChartRenderer.m_strOriRenderField = strValueField;
                pStackedChartRenderer.m_strUncernRenderField = strUncerField;

                pStackedChartRenderer.dblMaxEstimate = pDrawDenplot.dblMaxEstimate;
                pStackedChartRenderer.dblMaxUncern = pDrawDenplot.dblMaxUncern;

                pStackedChartRenderer.m_pQueryFilter = pSpatialFilter as IQueryFilter;
                
                //pStackedChartRenderer.m_stackedChart = pGeoFeatureLayer.Renderer;

                //pStackedChartRenderer.PrepareFilter(pFeatureClass, pSpatialFilter);
                pStackedChartRenderer.m_pDisplay = pActiveView.ScreenDisplay;
                //pStackedChartRenderer.Draw(pFCursor, esriDrawPhase.esriDPSelection, pDisplay, null);
                //pStackedChartRenderer.CreateLegend();
                pGeoFeatureLayer.Renderer = pStackedChartRenderer;


                //mForm.axMapControl1.ActiveView.Refresh();
                pActiveView.Refresh();



            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add toolDenPlot.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add toolDenPlot.OnMouseUp implementation
        }
        #endregion
    }
}