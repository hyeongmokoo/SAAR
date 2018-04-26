using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace VisUncertainty
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("15592ba5-2cff-4892-b24e-2f5c32be1546")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.ZoomToSelectedFeatures")]
    public sealed class ZoomToSelectedFeatures : BaseCommand
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

        //private IHookHelper m_hookHelper = null;
        private IMapControl3 m_mapControl;

        public ZoomToSelectedFeatures()
        {
            //
            // TODO: Define values for the public properties
            //
            //base.m_category = ""; //localizable text
            base.m_caption = "Zoom To Selected Features";  //localizable text 
            //base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            //base.m_toolTip = "";  //localizable text
            //base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                //m_hookHelper = new HookHelperClass();
                //m_hookHelper.Hook = hook;
                //if (m_hookHelper.ActiveView == null)
                //    m_hookHelper = null;
                m_mapControl = (IMapControl3)hook;
            }
            catch
            {
                m_mapControl = null;
                //m_hookHelper = null;
            }

            //if (m_hookHelper == null)
            //    base.m_enabled = false;
            //else
            //    base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ZoomToSelectedFeatures.OnClick implementation
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFLayer = (IFeatureLayer)pLayer;
            IFeatureSelection pFSel = (IFeatureSelection)pFLayer;
            ISelectionSet pSelSet = pFSel.SelectionSet;

            IEnumGeometry pEnumGeom = new EnumFeatureGeometry();
            IEnumGeometryBind pEnumGeomBind = (IEnumGeometryBind)pEnumGeom;
            pEnumGeomBind.BindGeometrySource(null, pSelSet);

            IGeometryFactory pGeomFactory = new GeometryEnvironmentClass();
            IGeometry pGeom = pGeomFactory.CreateGeometryFromEnumerator(pEnumGeom);
            double dblXPer = (pGeom.Envelope.XMax - pGeom.Envelope.XMin) / 10;
            double dblYPer = (pGeom.Envelope.YMax - pGeom.Envelope.YMin) / 10;
            IEnvelope pEnvel = new EnvelopeClass();
            pEnvel.PutCoords(pGeom.Envelope.XMin - dblXPer, pGeom.Envelope.YMin - dblYPer, pGeom.Envelope.XMax + dblXPer, pGeom.Envelope.YMax + dblYPer);
            //envelope1.PutCoords(pFLayer1.AreaOfInterest.Envelope.XMin - (pFLayer1.AreaOfInterest.Envelope.XMin * 0.0005), pFLayer1.AreaOfInterest.Envelope.YMin - (pFLayer1.AreaOfInterest.Envelope.YMin * 0.0005), pFLayer1.AreaOfInterest.Envelope.XMax * 1.0005, pFLayer1.AreaOfInterest.Envelope.YMax * 1.0005);

            m_mapControl.ActiveView.Extent = pEnvel;
            m_mapControl.ActiveView.Refresh();

        }

        #endregion


    }
}
