using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Text;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RDotNet;

namespace VisUncertainty
{
    /// <summary>
    /// Summary description for toolDenPlot.
    /// </summary>
    [Guid("0b064f87-eac5-4941-92bd-19505c91cc5b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.toolDenPlot")]
    public sealed class toolDenPlot : BaseTool
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

        public toolDenPlot()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Uncertatiny Density Plot"; //localizable text 
            base.m_caption = "Probability Density Plot";  //localizable text 
            base.m_message = "Draw the plot for normal probability density function based on Mean and Standard errors of a selected feature";  //localizable text
            base.m_toolTip = "Probability Density Plot";  //localizable text
            base.m_name = "Uncern_DenPlot";   //unique id, non-localizable (e.g. "MyCategory_MyTool")

            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            
            try
            {
                //
                // TODO: change resource name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);

                //string bitmapResourceName = mForm.strPath + @"\icons\picDenPlot.bmp";
                //base.m_bitmap = new Bitmap(bitmapResourceName);
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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
            pDrawDenplot.chk3D.Visible = false;
            pDrawDenplot.Show();
            // TODO: Add toolDenPlot.OnClick implementation

        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (pDrawDenplot.cboSourceLayer.Text != "" && pDrawDenplot.cboUField.Text != "" && pDrawDenplot.cboValueField.Text != "")
            {
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

                IActiveView pActiveView = mForm.axMapControl1.ActiveView;
                string strTargetLayerName = pDrawDenplot.cboSourceLayer.Text;
                string strValueField = pDrawDenplot.cboValueField.Text;
                string strUncerField = pDrawDenplot.cboUField.Text;


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
                int intUncerFldIdx = pFeatureClass.FindField(strUncerField);


                double[] dblValue = new double[1];
                dblValue[0] = Convert.ToDouble(pFeature.get_Value(intValueFldIdx));
                double[] dblUncern = new double[1];
                dblUncern[0] = Convert.ToDouble(pFeature.get_Value(intUncerFldIdx));
                REngine pEngine = mForm.pEngine;
                NumericVector vecValue = pEngine.CreateNumericVector(dblValue);
                NumericVector vecSD = pEngine.CreateNumericVector(dblUncern);
                pEngine.SetSymbol("samp.mean", vecValue);
                pEngine.SetSymbol("samp.sd", vecSD);

                pEngine.Evaluate("samp.xlim <- c(samp.mean-(samp.sd*3), samp.mean+(samp.sd*3))");
                pEngine.Evaluate("confLevel <- "+pDrawDenplot.nudConfiLevel.Value.ToString());
                pEngine.Evaluate("bothlevel <- 1-((100-confLevel)/200)");
                pEngine.Evaluate("error <- qnorm(bothlevel)*samp.sd");
                pEngine.Evaluate("confint <- c(samp.mean-error, samp.mean+error)");

                StringBuilder sbCommand = new StringBuilder();
                sbCommand.Append("curve(dnorm(x, mean=samp.mean, sd=samp.sd), lwd=2, xlim=samp.xlim, ylab='Probability', xlab='"+strValueField+"');");
                sbCommand.Append("abline(v=samp.mean, col='red');");
                sbCommand.Append("abline(v=confint, col='blue');");
                string strTitle = "Probability Denstiy Plot";

                pSnippet.drawPlottoForm(strTitle, sbCommand.ToString());

            }

            //int intFID = Convert.ToInt32(pFeature.get_Value(0)); //Get FID Value
            //return intFID;



            //int intFID = FindFeatureFID(pPoint);
            //int intColumnLength = arrSimuResults.GetUpperBound(1) + 1;
            //double[] arrHist = new double[intColumnLength];
            //for (int i = 0; i < intColumnLength; i++)
            //{
            //    arrHist[i] = arrSimuResults[intFID, i];
            //}
            //NumericVector vecBLL = pEngine.CreateNumericVector(arrHist);
            //pEngine.SetSymbol(strValue, vecBLL);

            //string strCommand = "hist(" + strValue + ", freq=FALSE, main=paste('Histogram of FID ', " + intFID.ToString() + "));abline(v=" + Math.Round(arrOrivalue[intFID], 3).ToString() + ", col='red');";
            //string strTitle = "Histogram";

            //System.Text.StringBuilder CommandPlot = new System.Text.StringBuilder();


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
