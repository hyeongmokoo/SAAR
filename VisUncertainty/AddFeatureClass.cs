using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;

namespace VisUncertainty
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("4ba58dd2-6fab-43fb-a491-5e4f33a68745")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.AddFeatureClass")]
    public sealed class AddFeatureClass : BaseCommand
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
        private MainForm mForm;

        public AddFeatureClass()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Add Data"; //localizable text
            base.m_caption = "Add Data";  //localizable text 
            base.m_message = "Add shapefiles into map";  //localizable text
            base.m_toolTip = "Add Shapefiles";  //localizable text
            base.m_name = "AddData";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
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
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            IActiveView pActiveView = mForm.axMapControl1.ActiveView;

            //string strFileName = "";

            if (mForm.ofdAddData.ShowDialog() == DialogResult.OK)
            {
                foreach (string strFileName in mForm.ofdAddData.FileNames)
                {
                    string strSourceFolder = null;
                    string strSourceFile = null;
                    strSourceFolder = System.IO.Path.GetDirectoryName(strFileName);
                    strSourceFile = System.IO.Path.GetFileName(strFileName);
                    IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                    IWorkspace ws = workspaceFactory.OpenFromFile(strSourceFolder, 0);
                    IFeatureWorkspace fws = ws as IFeatureWorkspace;
                    IFeatureClass pFClass = fws.OpenFeatureClass(strSourceFile);
                    IFeatureLayer pFLayer = new FeatureLayerClass();
                    pFLayer.FeatureClass = pFClass;
                    pFLayer.Name = pFClass.AliasName;
                    pActiveView.FocusMap.AddLayer(pFLayer);
                }
                pActiveView.Refresh();
                mForm.axTOCControl1.Update();
            }
            else
                return;




            #region IGxDialog -- not working
            //IGxObjectFilterCollection ipFilterCollection = new GxDialogClass();

            //IGxObjectFilter ipFilter1 = new GxFilterShapefilesClass();
            //ipFilterCollection.AddFilter(ipFilter1, true);
            //IGxDialog pGxDialog = (IGxDialog)(ipFilterCollection);
            ////IGxDialog pGxDialog = new GxDialog();
            ////IGxDialog pGxDialog = new GxDialog();
            //IGxObjectFilter pGxObFilter = new GxFilterShapefilesClass();
            //pGxDialog.AllowMultiSelect = true;
            //pGxDialog.ButtonCaption = "Add";
            //pGxDialog.ObjectFilter = pGxObFilter;
            //pGxDialog.Title = "Add Shapefiles";

            //IEnumGxObject pGxObjects;
            //pGxDialog.DoModalOpen(0, out pGxObjects);
            //IGxDataset pGxDataset = (IGxDataset)pGxObjects.Next();
            //while(pGxDataset != null)
            //{
            //    IFeatureLayer pFLayer = new FeatureLayerClass();
            //    pFLayer.FeatureClass = (IFeatureClass)pGxDataset.Dataset;
            //    pFLayer.Name = pGxDataset.Dataset.Name;
            //    pActiveView.FocusMap.AddLayer(pFLayer);
            //    pGxDataset = (IGxDataset)pGxObjects.Next();
            //}
            //pActiveView.Refresh();
            //mForm.axTOCControl1.Update();
            #endregion

            // TODO: Add AddFeatureClass.OnClick implementation
        }

        #endregion
    }
}
