using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace VisUncertainty
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("e2df9def-9008-44e5-923a-8f93a8542f0d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.LayerSymbology")]
    public sealed class LayerSymbology : BaseCommand
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
        private IMapControl3 m_mapControl;

        public LayerSymbology()
        {
            base.m_caption = "Symbology";
            ////
            //// TODO: Define values for the public properties
            ////
            //base.m_category = ""; //localizable text
            //base.m_caption = "";  //localizable text 
            //base.m_message = "This should work in ArcMap/MapControl/PageLayoutControl";  //localizable text
            //base.m_toolTip = "";  //localizable text
            //base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            //try
            //{
            //    //
            //    // TODO: change bitmap name if necessary
            //    //
            //    string bitmapResourceName = GetType().Name + ".bmp";
            //    base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            //}
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
            m_mapControl = (IMapControl3)hook;
            //try
            //{
            //    m_hookHelper = new HookHelperClass();
            //    m_hookHelper.Hook = hook;
            //    if (m_hookHelper.ActiveView == null)
            //        m_hookHelper = null;
            //}
            //catch
            //{
            //    m_hookHelper = null;
            //}

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
            // TODO: Add LayerSymbology.OnClick implementation
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            frmSymbology pfrmPropeties = new frmSymbology();
            pfrmPropeties.mlayer = pLayer;
            pfrmPropeties.ShowDialog();
        }

        #endregion
    }
}
