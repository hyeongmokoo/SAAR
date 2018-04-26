using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace VisUncertainty
{
    /// <summary>
    /// Summary description for SaveLayerFile.
    /// </summary>
    [Guid("164f2fd7-cbfb-4a77-abe1-79e9995ad37d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.SaveLayerFile")]
    public sealed class SaveLayerFile : BaseCommand
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

        //private IHookHelper m_hookHelper;
        private IMapControl3 m_mapControl;

        public SaveLayerFile()
        {
            base.m_caption = "Save Layer File";


            ////
            //// TODO: Define values for the public properties
            ////
            //base.m_category = ""; //localizable text
            //base.m_caption = "Save Layer File";
            //base.m_message = "";  //localizable text 
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
            m_mapControl = (IMapControl3)hook;

            //if (hook == null)
            //    return;

            //if (m_hookHelper == null)
            //    m_hookHelper = new HookHelperClass();

            //m_hookHelper.Hook = hook;

            //// TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add SaveLayerFile.OnClick implementation
            ILayer layer = (ILayer)m_mapControl.CustomProperty;


            //ask the user to set a name for the new layer file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Layer File|*.lyr|All Files|*.*";
            saveFileDialog.Title = "Create Layer File";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = System.IO.Path.Combine(saveFileDialog.InitialDirectory, layer.Name + ".lyr");

            //get the layer name from the user
            DialogResult dr = saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "" && dr == DialogResult.OK)
            {
                if (System.IO.File.Exists(saveFileDialog.FileName))
                {
                    //try to delete the existing file
                    System.IO.File.Delete(saveFileDialog.FileName);
                }

                //create a new LayerFile instance
                ILayerFile layerFile = new LayerFileClass();
                //create a new layer file
                layerFile.New(saveFileDialog.FileName);
                //attach the layer file with the actual layer
                layerFile.ReplaceContents(layer);
                //save the layer file
                layerFile.Save();

                //ask the user whether he'd like to add the layer to the map
                if (DialogResult.Yes == MessageBox.Show("Would you like to add the layer to the map?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    m_mapControl.AddLayerFromFile(saveFileDialog.FileName, 0);
                }
            }
        }

        #endregion
    }
}
