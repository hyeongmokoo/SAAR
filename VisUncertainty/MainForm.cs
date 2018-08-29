using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

using RDotNet;
using RDotNet.NativeLibrary;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

namespace VisUncertainty
{
    public sealed partial class MainForm: Form
    {
        #region class private members
        private IMapControl3 m_mapControl = null;
        private string m_mapDocumentName = string.Empty;
        private string strREngineName = "RDotNet";
        private clsSnippet m_pSnippet;
        private IToolbarControl2 m_ToolbarControl = null;
        private ITOCControl2 m_tocControl;
        private IToolbarMenu m_menuLayer;
        #endregion

        #region class public members
        public REngine pEngine;
        public System.Collections.Generic.List<string> multipageImage;
        public int intCurrentIdx;
        public string strPath;
        
        //For Attribute Uncertainty
        public double[,] arrSimuResults;
        public bool blnEnableHistUncern = false;
        public double[] arrOrivalue;
        public string strTargetLayerName;
        public string strValue;
        
        //For Histogram Linking
        public bool blnLinkingwithHistogram = false;
        public IFeatureLayer pHistFlayer;

        //For Uncertainty Classifications and their evaluations
        public List<clsRenderedLayers> lstRenderedLayers;

        //Feature counts for warning sign.
        public int intWarningCount = 200;

        //R Packages
        public bool[] blnsInstalledPcks;
        public string[] LibHome;
        #endregion

        #region class constructor
        public MainForm()
        {
            frmStartup pfrmStartup = new frmStartup();
            pfrmStartup.Show();
            InitializeComponent();
            pfrmStartup.Close();
        }
        #endregion

        #region MainForm Event Handler
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Get Start up path to set a sample data path and path of temporary folder
            strPath = System.Windows.Forms.Application.StartupPath;
            axMapControl1.ActiveView.FocusMap.Name = "Layers";
            //get the MapControl
            m_mapControl = (IMapControl3)axMapControl1.Object;
            
            //disable the Save menu (since there is no document yet)
            menuSaveDoc.Enabled = false;

            lstRenderedLayers = new List<clsRenderedLayers>();
            m_pSnippet = new clsSnippet();
            try
            {
                //Load sample mxd
                //string filePath = strPath + @"\Sample.mxd"; 
                //string filePath = strPath + @"\SampleData\Sample_plano.mxd";//For Plano
                //string filePath = strPath + @"\SampleData\Classification\Iowa_cnties.mxd";//For Iowa

                //if (axMapControl1.CheckMxFile(filePath))
                //    axMapControl1.LoadMxFile(filePath, Type.Missing, Type.Missing);
                //else
                //    MessageBox.Show("Wrong direction");

                ////Get Envelope of mxd
                //IActiveView pActiveView1 = axMapControl1.ActiveView.FocusMap as IActiveView;
                ////ILayer pLayer1 = pActiveView1.FocusMap.get_Layer(2);
                //ILayer pLayer1 = pActiveView1.FocusMap.get_Layer(0);

                ////adjust extent to fit a screen resolution
                //IFeatureLayer pFLayer1 = pLayer1 as IFeatureLayer;
                //IEnvelope envelope1 = new EnvelopeClass();
                //envelope1.PutCoords(pFLayer1.AreaOfInterest.Envelope.XMin - (pFLayer1.AreaOfInterest.Envelope.XMin * 0.0005), pFLayer1.AreaOfInterest.Envelope.YMin - (pFLayer1.AreaOfInterest.Envelope.YMin * 0.0005), pFLayer1.AreaOfInterest.Envelope.XMax * 1.0005, pFLayer1.AreaOfInterest.Envelope.YMax * 1.0005);
                //axMapControl1.ActiveView.Extent = envelope1;
                //axMapControl1.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 101: "+ex.Message);
            }

            try
            {
                //R environment setting

                #region Previous Methods
                //var envPath = Environment.GetEnvironmentVariable("PATH");
                //var rBinPath = strPath + @"\R-3.1.2\bin\i386"; // R is copited into startup path
                //Environment.SetEnvironmentVariable("PATH", envPath + System.IO.Path.PathSeparator + rBinPath);
                ////Environment.SetEnvironmentVariable("PATH", rBinPath); //Not working
                //Environment.SetEnvironmentVariable("R_HOME", strPath + @"\R-3.1.2");

                ////Loading REngine
                //pEngine = REngine.CreateInstance(strREngineName);
                //pEngine.Initialize();

                ////string[] strRHOME = pEngine.Evaluate("R.home(component = 'home')").AsCharacter().ToArray(); //For Deburgging
                //LibHome = pEngine.Evaluate(".libPaths()").AsCharacter().ToArray();

                //string strLibPath = strPath.Replace(@"\", @"/") + "/R-3.1.2/library";
                ////pEngine.Evaluate(".libPaths(" + strLibPath + ")");
                ////pEngine.Evaluate(".libPaths(c(" + strLibPath + ", .Library.site, .Library))");//Same results with the above
                ////pEngine.Evaluate(".libPaths(c(" + strLibPath + "))");//Same results with the above
                //pEngine.Evaluate(".Library.site <- file.path('"+strLibPath+"')"); //Same results with the above
                //pEngine.Evaluate("Sys.setenv(R_LIBS_USER='" + strLibPath + "')");
                ////string[] tempstring1 = pEngine.Evaluate("Sys.getenv('R_LIBS_USER')").AsCharacter().ToArray();
                ////string[] tempstring = pEngine.Evaluate(".Library.site").AsCharacter().ToArray();
                //pEngine.Evaluate(".libPaths(c('" + strLibPath + "', .Library.site, .Library))");

                //LibHome = pEngine.Evaluate(".libPaths()").AsCharacter().ToArray();
                //pEngine.Evaluate("ip <- installed.packages()").AsCharacter();
                //string[] installedPackages = pEngine.Evaluate("ip[,1]").AsCharacter().ToArray(); //To Check Installed Packages in R
                //clsRPackageNames pPckNames = new clsRPackageNames();
                //blnsInstalledPcks = pPckNames.CheckedRequiredPackages(installedPackages);

                #endregion

                //Current version of R is 3.4.4 (03/19/18 HK)
                var envPath = Environment.GetEnvironmentVariable("PATH");
                var rBinPath = strPath + @"\R-3.4.4\bin\i386"; // R is copited into startup path
                Environment.SetEnvironmentVariable("PATH", envPath + System.IO.Path.PathSeparator + rBinPath);
                Environment.SetEnvironmentVariable("R_HOME", strPath + @"\R-3.4.4");

                //Loading REngine
                pEngine = REngine.CreateInstance(strREngineName);
                pEngine.Initialize();

                //Set Library home and remove local home!
                LibHome = pEngine.Evaluate(".libPaths()").AsCharacter().ToArray();
                string strLibPath = strPath.Replace(@"\", @"/") + "/R-3.4.4/library"; //path for R packages
                pEngine.Evaluate(".Library.site <- file.path('" + strLibPath + "')");
                pEngine.Evaluate("Sys.setenv(R_LIBS_USER='" + strLibPath + "')");
                pEngine.Evaluate(".libPaths(c('" + strLibPath + "', .Library.site, .Library))");

                //Checked installed packages and R 
                LibHome = pEngine.Evaluate(".libPaths()").AsCharacter().ToArray();
                pEngine.Evaluate("ip <- installed.packages()").AsCharacter();
                string[] installedPackages = pEngine.Evaluate("ip[,1]").AsCharacter().ToArray(); //To Check Installed Packages in R
                clsRPackageNames pPckNames = new clsRPackageNames();
                blnsInstalledPcks = pPckNames.CheckedRequiredPackages(installedPackages);




                ////Installing Additional Package
                //Currently required pacakges:: MASS, geoR, car, spdep, maptools, deldir, rgeos, e1071
                //package required for Testing: fpc
                //pEngine.Evaluate("install.packages('fpc')");
                //pEngine.Evaluate("install.packages('e1071')");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 102:" + ex.Message);
            }

            try
            {

                //Toolbar Control, insert new tools here!!
                m_ToolbarControl = (IToolbarControl2)axToolbarControl1.Object;

                int intItemCounts = m_ToolbarControl.Count;
                m_ToolbarControl.AddItem(new toolDenPlot(), -1, intItemCounts, true, 0, esriCommandStyles.esriCommandStyleIconOnly); //Probability density plot tool
                //m_ToolbarControl.AddItem(new toolUncernFeature(), -1, intItemCounts+1, false, 0, esriCommandStyles.esriCommandStyleIconOnly); // Remove now 07/31/15
                m_ToolbarControl.AddItem(new ToolCumsum(), -1, intItemCounts + 1, false, 0, esriCommandStyles.esriCommandStyleIconOnly); // Empirical cumulative density function
                m_ToolbarControl.AddItem(new toolHistogram(), -1, intItemCounts + 2, false, 0, esriCommandStyles.esriCommandStyleIconOnly);// Histogram tool
                m_ToolbarControl.AddItem(new LinkingTool(), -1, intItemCounts + 3, true, 0, esriCommandStyles.esriCommandStyleIconOnly);// Histogram tool
                m_ToolbarControl.AddItem(new AddFeatureClass(), -1, 1, false, 0, esriCommandStyles.esriCommandStyleIconOnly);// Histogram tool

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 103: "+ex.Message);
            }

            try
            {
                //Loading Context menu at TOC
                m_tocControl = axTOCControl1.Object as ITOCControl2;
                m_tocControl.SetBuddyControl(m_mapControl);
                m_menuLayer = new ToolbarMenuClass();
                m_menuLayer.AddItem(new RemoveLayer(), -1, 0, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.AddItem(new OpenAttriTable(), -1, 1, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.AddItem(new ZoomToLayer(), -1, 2, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.AddItem(new ZoomToSelectedFeatures(), -1, 3, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.AddItem(new SaveLayerFile(), -1, 4, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.AddItem(new LayerSymbology(), -1, 5, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.AddItem(new LayerProperty(), -1, 6, false, esriCommandStyles.esriCommandStyleTextOnly);
                //m_menuLayer.AddItem(new Symbology(), -1, 3, false, esriCommandStyles.esriCommandStyleTextOnly);
                //m_menuLayer.AddItem(new SimpleSymbology(), -1, 3, false, esriCommandStyles.esriCommandStyleTextOnly);
                m_menuLayer.SetHook(m_mapControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 104:"+ex.Message);
            }
        
        }
        #endregion

        #region MapControl Event handler
        //listen to MapReplaced evant in order to update the statusbar and the Save menu
        private void axMapControl1_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {
            try
            {

                //get the current document name from the MapControl
                m_mapDocumentName = m_mapControl.DocumentFilename;

                //if there is no MapDocument, diable the Save menu and clear the statusbar
                if (m_mapDocumentName == string.Empty)
                {
                    menuSaveDoc.Enabled = false;
                    statusBarXY.Text = string.Empty;
                    this.Text = "Untitled";
                }
                else
                {
                    //enable the Save manu and write the doc name to the statusbar
                    menuSaveDoc.Enabled = true;
                    statusBarXY.Text = System.IO.Path.GetFileName(m_mapDocumentName);

                    this.Text = System.IO.Path.GetFileNameWithoutExtension(m_mapDocumentName);
                }

                //Close all opened forms
                FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
                while(pFormCollection.Count !=1)
                {
                    pFormCollection[1].Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            statusBarXY.Text = string.Format("{0}, {1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            try
            {
                //Can draw histrogram after uncertainty simulation function
                if (blnEnableHistUncern)
                {
                    int x = e.x;
                    int y = e.y;
                    IPoint pPoint = m_mapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                    int intFID = FindFeatureFID(pPoint);
                    int intColumnLength = arrSimuResults.GetUpperBound(1) + 1;
                    double[] arrHist = new double[intColumnLength];
                    for (int i = 0; i < intColumnLength; i++)
                    {
                        arrHist[i] = arrSimuResults[intFID, i];
                    }
                    NumericVector vecBLL = pEngine.CreateNumericVector(arrHist);
                    pEngine.SetSymbol(strValue, vecBLL);

                    string strCommand = "hist(" + strValue + ", freq=FALSE, main=paste('Histogram of FID ', " + intFID.ToString() + "));abline(v=" + Math.Round(arrOrivalue[intFID], 3).ToString() + ", col='red');";
                    string strTitle = "Histogram";

                    System.Text.StringBuilder CommandPlot = new System.Text.StringBuilder();

                    //Temporary path-needs to be changed
                    //Have to assing pathes differently at R and ArcObject
                    string pathr = @"D:\\temp\\";
                    string path = @"D:\temp\";

                    //Remove existing image file pathes
                    if (multipageImage == null)
                        multipageImage = new List<string>();
                    else
                        multipageImage.Clear();

                    //Create new list to delete and save the image pathes
                    //List<string> pmultipageImage = new List<string>();

                    //Delete existing image files
                    multipageImage.AddRange(Directory.GetFiles(path, "rnet*.wmf"));

                    for (int j = 0; j < multipageImage.Count; j++)
                    {
                        FileInfo pinfo = new FileInfo(multipageImage[j]);
                        if (pinfo.Exists)
                            pinfo.Delete();
                        pinfo.Refresh();
                    }

                    //Load Form and assign the settings
                    frmPlot pfrmPlot = new frmPlot();
                    pfrmPlot.Text = strTitle;
                    pfrmPlot.Show();
                    string strwidth = pfrmPlot.picPlot.Size.Width.ToString();
                    string strHeight = pfrmPlot.picPlot.Size.Height.ToString();

                    //Create Plots in R
                    //CommandPlot.Append("win.metafile('" + pathr + "rnet%02d.wmf', width=" + strwidth + ", height=" + strHeight + ");");
                    CommandPlot.Append("win.metafile('" + pathr + "rnet%01d.wmf');");
                    CommandPlot.Append(strCommand);
                    CommandPlot.Append("graphics.off()");
                    pEngine.Evaluate(CommandPlot.ToString());

                    //Add Plot pathes at List
                    multipageImage.Clear();
                    multipageImage.AddRange(Directory.GetFiles(path, "rnet*.wmf"));
                    intCurrentIdx = 0;

                    m_pSnippet.drawCurrentChart(multipageImage, intCurrentIdx, pfrmPlot);
                    m_pSnippet.enableButtons(multipageImage, intCurrentIdx, pfrmPlot);

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

        private void axMapControl1_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            try
            {
                if (blnLinkingwithHistogram)
                {
                    int x = e.x;
                    int y = e.y;
                    IPoint pPoint = m_mapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);

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
        #endregion

        #region TOCControl event handler

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            try
            {


                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null; ILayer layer = null;
                object other = null; object index = null;

                //Determine what kind of item is selected
                m_tocControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

                if (item == esriTOCControlItem.esriTOCControlItemNone)
                    return;

                // Load TOC context menu
                if (e.button == 2)
                {
                    if (item == esriTOCControlItem.esriTOCControlItemMap)
                        m_tocControl.SelectItem(map, null);
                    else
                        m_tocControl.SelectItem(layer, null);

                    //Set the layer into the CustomProperty (this is used by the custom layer commands)			
                    m_mapControl.CustomProperty = layer;

                    //Popup the correct context menu
                    if (item == esriTOCControlItem.esriTOCControlItemMap) return;
                    if (item == esriTOCControlItem.esriTOCControlItemLayer) m_menuLayer.PopupMenu(e.x, e.y, m_tocControl.hWnd);
                }
                //Ensure the item gets selected 
                
                //Load Simple Color pallette 071017 HK
                if (e.button == 1&&item == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    IFeatureLayer featureLayer = layer as IFeatureLayer;
                    IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)featureLayer;

                    DialogResult DR;
                    if (geoFeatureLayer.Renderer is ISimpleRenderer) // Only apply this function to simple Renderer
                        DR = cdColor.ShowDialog();
                    else
                        return;

                    IRgbColor pRGBcolor = new RgbColorClass();
                    if (DR == DialogResult.OK)
                        pRGBcolor = m_pSnippet.getRGB(cdColor.Color.R, cdColor.Color.G, cdColor.Color.B);

                    ISimpleRenderer simpleRenderer = (ISimpleRenderer)geoFeatureLayer.Renderer;
                    
                    ISymbol pSymbol = simpleRenderer.Symbol;
                    
                    //Update only color
                    switch (featureLayer.FeatureClass.ShapeType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            ISimpleMarkerSymbol pSimpleMarkerSymobl = (ISimpleMarkerSymbol)pSymbol;
                            pSimpleMarkerSymobl.Color = (IColor)pRGBcolor;
                            pSymbol = (ISymbol)pSimpleMarkerSymobl;
                            break;
                        case esriGeometryType.esriGeometryPolyline:
                            ISimpleLineSymbol pSimpleLineSymobl = (ISimpleLineSymbol)pSymbol;
                            pSimpleLineSymobl.Color = (IColor)pRGBcolor;
                            pSymbol = (ISymbol)pSimpleLineSymobl;
                            break;
                        case esriGeometryType.esriGeometryPolygon:
                            ISimpleFillSymbol pSimpleFillSymobl = (ISimpleFillSymbol)pSymbol;
                            pSimpleFillSymobl.Color = (IColor)pRGBcolor;
                            pSymbol = (ISymbol)pSimpleFillSymobl;
                            
                            break;
                    }
                    simpleRenderer.Symbol = pSymbol;
                    geoFeatureLayer.Renderer = (IFeatureRenderer)simpleRenderer;
                    m_tocControl.ActiveView.ContentsChanged();
                    m_mapControl.Refresh(esriViewDrawPhase.esriViewGeography, null, null);

                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        #endregion

        #region Private Functions
        private int FindFeatureFID(IPoint pPoint)
        {
            try
            {
                double Tol = 4;
                IEnvelope pEnvelop = pPoint.Envelope;
                pEnvelop.Expand(Tol, Tol, false);
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pEnvelop;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                int intTLayerIdx = m_pSnippet.GetIndexNumberFromLayerName(m_mapControl.ActiveView, strTargetLayerName);
                ILayer pLayer = m_mapControl.get_Layer(intTLayerIdx);
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;
                string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                pSpatialFilter.GeometryField = pFLayer.FeatureClass.ShapeFieldName;
                IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                IFeatureCursor pFCursor = pFeatureClass.Search(pSpatialFilter, false);
                IFeature pFeature = pFCursor.NextFeature();
                //int intFIDIdx = pFeatureClass.FindField("FID");
                int intFID = Convert.ToInt32(pFeature.get_Value(0)); //Get FID Value
                return intFID;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return -1;
            }
        }

        #endregion

        #region File Menu event handlers
        private void menuNewDoc_Click(object sender, EventArgs e)
        {
            try
            {
                //execute New Document command
                ICommand command = new CreateNewDocument();
                command.OnCreate(m_mapControl.Object);
                command.OnClick();
                //CloseAllOpenForms(); This function is moved to MapReplace Action 101917 HK
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void menuOpenDoc_Click(object sender, EventArgs e)
        {
            try
            {

                //execute Open Document command
                ICommand command = new ControlsOpenDocCommandClass();
                command.OnCreate(m_mapControl.Object);
                command.OnClick();
                CloseAllOpenForms();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void menuSaveDoc_Click(object sender, EventArgs e)
        {
            try
            {
                //execute Save Document command
                if (m_mapControl.CheckMxFile(m_mapDocumentName))
                {
                    //create a new instance of a MapDocument
                    IMapDocument mapDoc = new MapDocumentClass();
                    mapDoc.Open(m_mapDocumentName, string.Empty);

                    //Make sure that the MapDocument is not readonly
                    if (mapDoc.get_IsReadOnly(m_mapDocumentName))
                    {
                        MessageBox.Show("Map document is read only!");
                        mapDoc.Close();
                        return;
                    }

                    //Replace its contents with the current map
                    mapDoc.ReplaceContents((IMxdContents)m_mapControl.Map);

                    //save the MapDocument in order to persist it
                    mapDoc.Save(mapDoc.UsesRelativePaths, false);

                    //close the MapDocument
                    mapDoc.Close();
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            try
            {
                //execute SaveAs Document command
                ICommand command = new ControlsSaveAsDocCommandClass();
                command.OnCreate(m_mapControl.Object);
                command.OnClick();

                
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void menuExitApp_Click(object sender, EventArgs e)
        {
            //exit the application
            Application.Exit();
        }
        #endregion

        #region Data Menu event handlers
        private void fieldCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFieldCalculator pfrmFieldCalculator = new frmFieldCalculator();
            pfrmFieldCalculator.Show();
        }

        private void addFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddField pfrmAddField = new frmAddField();
            pfrmAddField.Show();
        }

        private void deleteFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeleteField pfrmDelField = new frmDeleteField();
            pfrmDelField.Show();
        }

        #endregion

        #region Graph Menu event handlers
        private void tsmHistogram_Click(object sender, EventArgs e)
        {
            frmHistogram pfrmHistogram = new frmHistogram();
            pfrmHistogram.Show();
        }

        private void tsmBoxplot_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Under reviewing");
            frmBoxplot pfrmBoxplot = new frmBoxplot();
            pfrmBoxplot.Show();
        }

        private void tsmScatterplot_Click(object sender, EventArgs e)
        {
            frmScatterplot pfrmScatterplot = new frmScatterplot();
            pfrmScatterplot.Show();
        }

        private void tsmQQplot_Click(object sender, EventArgs e)
        {
            frmQQplot pfrmQQplot = new frmQQplot();
            pfrmQQplot.Show();
        }
        private void moranScatterplotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMScatterplot pfrmMoranscatter = new frmMScatterplot();
            pfrmMoranscatter.Show();
        }
        #endregion

        #region Analysis Menu event handler
        private void oLSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegression pfrmRegression = new frmRegression();
            pfrmRegression.Show();
        }


        private void spatialRegressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSpatialRegression pfrmSRegression = new frmSpatialRegression();
            pfrmSRegression.Show();
        }
        private void localSpatialAutocorrelationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalSAM pfrmLocalSAM = new frmLocalSAM();
            pfrmLocalSAM.Show();
        }
        private void eigenVectorSpatialFilteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmESF pfrmESF = new frmESF();
            pfrmESF.Show();
        }

        private void spatialAutocorrelationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSAutoCorr pfrmSAutoCorr = new frmSAutoCorr();
            pfrmSAutoCorr.Show();
        }
        private void tsmDescriptiveStatistics_Click(object sender, EventArgs e)
        {
            frmDesStat pfrmErrors = new frmDesStat();
            pfrmErrors.Show();
        }
        #endregion

        #region Uncertainty Menu event handler

        private void createRandomPointWithErorrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmSample pfrmSample = new frmSample();
            //pfrmSample.Show();
        }

        

        private void graduatedSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAttGraduatedSymbols pfmAttGS = new frmAttGraduatedSymbols();
            pfmAttGS.Show();
        }



        private void proportionalSymbolWithSaturateAndTransparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPreVisuUncern pfrmPreVisuUncern = new frmPreVisuUncern();
            pfrmPreVisuUncern.Show();
        }

        private void choroplethMapWithGraduatedColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChoroplethwithOverlay pfrmChoroplethOverlay = new frmChoroplethwithOverlay();
            pfrmChoroplethOverlay.Show();
        }


        private void classSeparabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmClassSeparability pfrmCS = new frmClassSeparability();
            //pfrmCS.Show();
            frmNewClassSeparbility pfrmNewCS = new frmNewClassSeparbility();
            pfrmNewCS.Show();
        }


        private void theRoubustnessOfChoroplethMapClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmRobustness pfrmRoubstness = new frmRobustness();
            //pfrmRoubstness.Show();
            frmEvalRobustness pfrmEvalRobustness = new frmEvalRobustness();
            pfrmEvalRobustness.Show();
        }

        private void blinkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlinking pfrmBlinking = new frmBlinking();
            pfrmBlinking.Show();
        }

        private void gliderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSlider pfrmSlider = new frmSlider();
            pfrmSlider.Show();
        }

        #endregion

        #region Tools menu event handler

        private void createSpatialWeightMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateWeightMatrix pfrmCreateSWM = new frmCreateWeightMatrix();
            pfrmCreateSWM.Show();
        }
        #endregion

        #region Layout menu event handler
        private void openLayoutViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLayoutView pfrmLayoutView = new frmLayoutView();
            pfrmLayoutView.Show();
        }
        #endregion

        private void classificationOptimizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOptimizationSample pOptSample = new frmOptimizationSample();
            pOptSample.Show();
        }

        private void axMapControl1_SizeChanged(object sender, EventArgs e)
        {

        }
        private void CloseAllOpenForms()
        {
            FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
            for (int i = 0; i < pFormCollection.Count; i++)
            {
                if (this.Handle != pFormCollection[i].Handle)
                    pFormCollection[i].Close();
            }
        }

        private void dataTransformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBoxCox pfrmBoxCox = new frmBoxCox();
            pfrmBoxCox.Show();
        }

        private void reducedSecondMomentMeasureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmKFunction pfrmKfunction = new frmKFunction();
            pfrmKfunction.Show();
        }

        private void variogramCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVariogramCloud pVariCloud = new frmVariogramCloud();
            pVariCloud.Show();
        }

        private void parallelCoordinatePlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmParallPlotforRS pfrmParPlot = new frmParallPlotforRS();
            pfrmParPlot.Show();
        }

        private void spatialCorrelogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCorrelogram pfrmCorrelogram = new frmCorrelogram();
            pfrmCorrelogram.Show();
        }

        private void violinPlotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmViolinPlot pfrmViolinPlot = new frmViolinPlot();
            pfrmViolinPlot.Show();
        }

        private void programPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProgramProperties pfrmPrgPrt = new frmProgramProperties();
            pfrmPrgPrt.Show();
        }

        private void conditionedMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCCMaps pfrmCCMaps = new frmCCMaps();
            pfrmCCMaps.Show();
        }

        private void generalizedLinearModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGLM pfrmGLM = new frmGLM();
            pfrmGLM.Show();
        }

        private void createFlowLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateFlowLines pfrmCreateFlowLines = new frmCreateFlowLines();
            pfrmCreateFlowLines.Show();
        }

        private void spatialCorrelogramLocalVerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCorrelogram_local pfrmCorrelogram_Local = new frmCorrelogram_local();
            pfrmCorrelogram_Local.Show();
        }

        private void mLClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRSSample pfrmRSSample = new frmRSSample();
            pfrmRSSample.Show();
        }

        private void bivariateSpatialAssociationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBiLocalSAM pfrmBiLocalSAM = new frmBiLocalSAM();
            pfrmBiLocalSAM.Show();
        }

        private void globalBivariateSpatialAssociationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBiSAutoCorr pfrmBiSautoCorr = new frmBiSAutoCorr();
            pfrmBiSautoCorr.Show();
        }

        private void clustogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClustogram pfrmClustogram = new frmClustogram();
            pfrmClustogram.Show();
        }

        private void linearRegressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRegression pfrmRegression = new frmRegression();
            pfrmRegression.Show();

        }

        private void generalizedLinearModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGLM pfrmGLM = new frmGLM();
            pfrmGLM.Show();
        }

        private void spatialRegressionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSpatialRegression pfrmSRegression = new frmSpatialRegression();
            pfrmSRegression.Show();
        }

        private void eigenvectorSpatialFilteringToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmESF pfrmESF = new frmESF();
            pfrmESF.Show();
        }

        private void bivariateSpatialClusterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBiCluster pfrmBiCluster = new frmBiCluster();
            pfrmBiCluster.Show();
        }

        private void bivariateSpatialOutlierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBiOutlier pfrmBiOutlier = new frmBiOutlier();
            pfrmBiOutlier.Show();
        }

        private void bivariateSpatialRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBiQuadrants pBiQuad = new frmBiQuadrants();
            pBiQuad.Show();
        }

        private void bivariateLISAMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBivariateLISA pfrmBiLISA = new frmBivariateLISA();
            pfrmBiLISA.Show();
        }

        private void bivariateSpatialQuadrantMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBivariateSpQuadrant pfrmBiSpQuad = new frmBivariateSpQuadrant();
            pfrmBiSpQuad.Show();
        }

        private void bivariateSpatialProbabilityMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBivariateProbability pfrmBiProb = new frmBivariateProbability();
            pfrmBiProb.Show();
        }

        private void bivariateSpatialSignificanceMapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBivariateSignificance pfrmBivariateSig = new frmBivariateSignificance();
            pfrmBivariateSig.Show();
        }

        private void globalBivariateSpatialAssociationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmBivariateGlobal pfrmBivariateGlobal = new frmBivariateGlobal();
            pfrmBivariateGlobal.Show();
        }

        private void geocodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGeocoding pfrmGeoCoding = new frmGeocoding();
            pfrmGeoCoding.Show();
        }

        private void uncernSAMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUncernSAM pfrmUncernSAM = new frmUncernSAM();
            pfrmUncernSAM.Show();
        }

        private void spatialWeightsMatrixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSWMAnalysis pfrmSWMAnalysis = new frmSWMAnalysis();
            pfrmSWMAnalysis.Show();
        }

        private void hierarchicalClusteringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHierClustering pfrmHCluster = new frmHierClustering();
            pfrmHCluster.Show();
        }

        private void spatiallyVaryingCoefficientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVaryingCoefficients pfrmVC = new frmVaryingCoefficients();
            pfrmVC.Show();
        }
    }
}