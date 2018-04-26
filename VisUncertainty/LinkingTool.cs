using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;


namespace VisUncertainty
{
    /// <summary>
    /// Summary description for LinkingTool.
    /// </summary>
    [Guid("51919f07-0d8e-4be7-a724-829d49d100e2")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.LinkingTool")]
    public sealed class LinkingTool : BaseTool
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
        private MainForm m_pForm;
        //private clsSnippet m_pSnippet = new clsSnippet();
        private clsBrusingLinking m_pBL = new clsBrusingLinking();

        public LinkingTool()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "ESDATool"; //localizable text 
            base.m_caption = "Brushing and Linking";  //localizable text 
            base.m_message = "Brushed features in the Map control causes the brush effect to be applied on those items in the other plots that represent the same data items";  //localizable text
            base.m_toolTip = "Brushing and Linking";  //localizable text
            base.m_name = "Brushing_Linking";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
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
            // TODO: Add LinkingTool.OnClick implementation
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
            IActiveView pActiveView = m_pForm.axMapControl1.ActiveView;

            clsSnippet pSnippet = new clsSnippet();

            //Using IRubberband
            IEnvelope pEnvelop = pSnippet.DrawRectangle(pActiveView);
            if (pEnvelop.IsEmpty)
            {
                int x = X;
                int y = Y;
                IPoint pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);


                double Tol_x = x / int.MaxValue;
                double Tol_y = y / int.MaxValue;
                pEnvelop = pPoint.Envelope;
                pEnvelop.Expand(Tol_x, Tol_y, false);
            }

            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pEnvelop;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            for (int i = 0; i < pActiveView.FocusMap.LayerCount; i++)
            {
                
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(i);
                
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;

                if (pFLayer.Visible)
                {
                    //Brushing to Mapcontrol
                    string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                    pSpatialFilter.GeometryField = pFLayer.FeatureClass.ShapeFieldName;
                    BrushingOnMapControl(pSpatialFilter, pActiveView, pFLayer);
                    m_pBL.BrushingToOthers(pFLayer, m_pForm.Handle);
                }
            }
            pActiveView.GraphicsContainer.DeleteAllElements();
            pActiveView.Refresh();
            
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LinkingTool.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add LinkingTool.OnMouseUp implementation
        }
        #endregion

        #region private functions
        private void BrushingOnMapControl(ISpatialFilter pSpatialFilter, IActiveView pActiveView, IFeatureLayer pFLayer)
        {
            ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast

            // Invalidate only the selection cache. Flag the original selection
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            // Perform the selection
            featureSelection.SelectFeatures(pSpatialFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

            // Flag the new selection
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
        }

        #endregion
    }
}
