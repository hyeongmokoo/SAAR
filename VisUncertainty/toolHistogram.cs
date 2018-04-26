using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using System.Text;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RDotNet;
using RDotNet.NativeLibrary;

namespace VisUncertainty
{
    /// <summary>
    /// Summary description for toolHistogram.
    /// </summary>
    [Guid("3f8dea48-2c31-494e-9d6d-7bd704d96fc0")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.toolHistogram")]
    public sealed class toolHistogram : BaseTool
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
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        private frmAttributeField pfrmAttributeField;
        private MainForm mForm;

        public toolHistogram()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Histogram"; //localizable text 
            base.m_caption = "Histogram";  //localizable text 
            base.m_message = "Draw histogram by all vlaues and mark the value of selected feature";  //localizable text
            base.m_toolTip = "Histogram";  //localizable text
            base.m_name = "Histogram";   //unique id, non-localizable (e.g. "MyCategory_MyTool")

            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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
            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                {
                    m_hookHelper = null;
                }
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            pfrmAttributeField = new frmAttributeField();
            pfrmAttributeField.Text = "Histogram";
            pfrmAttributeField.Show();
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            try
            {
                if (pfrmAttributeField.cboSourceLayer.Text != "" && pfrmAttributeField.cboValueField.Text != "")
                {
                    mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

                    IActiveView pActiveView = mForm.axMapControl1.ActiveView;
                    string strTargetLayerName = pfrmAttributeField.cboSourceLayer.Text;
                    string strValueField = pfrmAttributeField.cboValueField.Text;

                    int x = X;
                    int y = Y;
                    IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);


                    double Tol = 4; //Needs to be changed
                    IEnvelope pEnvelop = pPoint.Envelope;
                    pEnvelop.Expand(Tol, Tol, false);
                    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    pSpatialFilter.Geometry = pEnvelop;
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    clsSnippet pSnippet = new clsSnippet();

                    int intTLayerIdx = pSnippet.GetIndexNumberFromLayerName(pActiveView, strTargetLayerName);

                    ILayer pLayer = mForm.axMapControl1.get_Layer(intTLayerIdx);
                    IFeatureLayer pFLayer = (IFeatureLayer)pLayer;
                    string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                    pSpatialFilter.GeometryField = pFLayer.FeatureClass.ShapeFieldName;
                    IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                    IFeatureCursor pFCursor = pFeatureClass.Search(pSpatialFilter, false);
                    IFeature pFeature = pFCursor.NextFeature();
                    pFCursor.Flush();
                    if (pFeature == null)
                        return;

                    int intValueFldIdx = pFeatureClass.FindField(strValueField);

                    double[] dblAllValue = pfrmAttributeField.arrValue;
                    double[] dblValue = new double[1];
                    dblValue[0] = Convert.ToDouble(pFeature.get_Value(intValueFldIdx));
                    int intCount = pFeatureClass.FeatureCount(null);
                    REngine pEngine = mForm.pEngine;
                    NumericVector vecValue = pEngine.CreateNumericVector(dblValue);
                    NumericVector vecAllValue = pEngine.CreateNumericVector(dblAllValue);
                    pEngine.SetSymbol("value", vecValue);
                    pEngine.SetSymbol("all.value", vecAllValue);

                    StringBuilder sbCommand = new StringBuilder();
                    sbCommand.Append("hist(all.value);");
                    sbCommand.Append("abline(v=value, col='red');");
                    string strTitle = "Histogram";

                    pSnippet.drawPlottoForm(strTitle, sbCommand.ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HistTool Error:" + ex.Message);
                return;
            }
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add toolHistogram.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add toolHistogram.OnMouseUp implementation
        }
        #endregion
    }
}
