using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Collections;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Geometry;

namespace VisUncertainty
{
    public partial class frmLayoutView : Form
    {

        #region Class internal Members
        internal PrintPreviewDialog printPreviewDialog1;
        internal PrintDialog printDialog1;
        internal PageSetupDialog pageSetupDialog1;
        #endregion

        #region Class Private Members
        private MainForm mForm;
        private IActiveView pActiveView;
        private short m_CurrentPrintPage;
        private double dblMinXMF;
        private double dblMinYMF;
        private double dblMaxXMF;
        private double dblMaxYMF;
        private double dblMinMaxDivision;
        #endregion

        #region Class Public Members
        public PrintDocument pPrintDoc = new PrintDocument();
        public IPaper paper;
        public IPrinter printer;

        #endregion

        public frmLayoutView()
        {
            try
            {
                InitializeComponent();
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

                CopyAndOverwriteMap(mForm);
                axPageLayoutControl1.ZoomToWholePage();
                this.Text = "Layout View of " + mForm.Text;
                pActiveView = axPageLayoutControl1.ActiveView;

                //Preview and Document Settings
                InitializePrintPreviewDialog(); //initialize the print preview dialog
                printDialog1 = new PrintDialog(); //create a print dialog object
                InitializePageSetupDialog(); //initialize the page setup dialog
                InitialLocationofMapsurroundings();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void InitialLocationofMapsurroundings()
        {
            try
            {
                //Find Map frame to set up the initial location of other map surrounds
                IGraphicsContainer graphicsContainer = axPageLayoutControl1.GraphicsContainer;
                graphicsContainer.Reset();
                IElement pElement = graphicsContainer.Next();

                IEnvelope pEnvelope = new EnvelopeClass();
                while (pElement != null)
                {
                    if (pElement is IMapFrame)
                    {
                        pEnvelope = pElement.Geometry.Envelope;
                        dblMinXMF = pEnvelope.XMin;
                        dblMinYMF = pEnvelope.YMin;
                        dblMaxXMF = pEnvelope.XMax;
                        dblMaxYMF = pEnvelope.YMax;
                        if (dblMaxYMF > dblMaxXMF)
                            dblMinMaxDivision = (dblMaxYMF - dblMinYMF) / 10;
                        else
                            dblMinMaxDivision = (dblMaxXMF - dblMinXMF) / 10;
                    }

                    pElement = graphicsContainer.Next();
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void CopyAndOverwriteMap(MainForm mForm)
        {
            try
            {
                //Get IObjectCopy interface
                IObjectCopy objectCopy = new ObjectCopyClass();

                //Get IUnknown interface (map to copy)
                object toCopyMap = mForm.axMapControl1.ActiveView.FocusMap;

                //Get IUnknown interface (copied map)
                object copiedMap = objectCopy.Copy(toCopyMap);

                //Get IUnknown interface (map to overwrite)
                object toOverwriteMap = axPageLayoutControl1.ActiveView.FocusMap;

                //Overwrite the MapControl's map
                objectCopy.Overwrite(copiedMap, ref toOverwriteMap);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }


        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExport pfrmExport = new frmExport();
            pfrmExport.pActiveView = pActiveView;
            pfrmExport.Show();
        }

        private void InitializePrintPreviewDialog()
        {
            try
            {
                // create a new PrintPreviewDialog using constructor
                printPreviewDialog1 = new PrintPreviewDialog();
                //set the size, location, name and the minimum size the dialog can be resized to
                printPreviewDialog1.ClientSize = new System.Drawing.Size(800, 600);
                printPreviewDialog1.Location = new System.Drawing.Point(29, 29);
                printPreviewDialog1.Name = "PrintPreviewDialog1";
                printPreviewDialog1.MinimumSize = new System.Drawing.Size(375, 250);
                //set UseAntiAlias to true to allow the operating system to smooth fonts
                printPreviewDialog1.UseAntiAlias = true;

                //associate the event-handling method with the document's PrintPage event
                this.pPrintDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(document_PrintPage);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void InitializePageSetupDialog()
        {
            try
            {
                //create a new PageSetupDialog using constructor
                pageSetupDialog1 = new PageSetupDialog();
                //initialize the dialog's PrinterSettings property to hold user defined printer settings
                pageSetupDialog1.PageSettings = new System.Drawing.Printing.PageSettings();
                //initialize dialog's PrinterSettings property to hold user set printer settings
                pageSetupDialog1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                //do not show the network in the printer dialog
                pageSetupDialog1.ShowNetwork = false;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                //this code will be called when the PrintPreviewDialog.Show method is called
                //set the PageToPrinterMapping property of the Page. This specifies how the page 
                //is mapped onto the printer page. By default the page will be tiled 


                //get the selected mapping option
                string sPageToPrinterMapping = null;//(string)this.comboBox1.SelectedItem;
                if (sPageToPrinterMapping == null)
                {
                    //if no selection has been made the default is scaling
                    axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingScale;

                }
                //Default Settings
                else if (sPageToPrinterMapping.Equals("esriPageMappingTile"))
                    axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;
                else if (sPageToPrinterMapping.Equals("esriPageMappingCrop"))
                    axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;
                else if (sPageToPrinterMapping.Equals("esriPageMappingScale"))
                    axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingScale;
                else
                    axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;

                //get the resolution of the graphics device used by the print preview (including the graphics device)
                short dpi = (short)e.Graphics.DpiX;
                //envelope for the device boundaries
                IEnvelope devBounds = new EnvelopeClass();
                //get page
                IPage page = axPageLayoutControl1.Page;

                //the number of printer pages the page will be printed on
                short printPageCount;
                printPageCount = axPageLayoutControl1.get_PrinterPageCount(0);
                m_CurrentPrintPage++;

                //the currently selected printer
                IPrinter printer = axPageLayoutControl1.Printer;

                //get the device bounds of the currently selected printer
                page.GetDeviceBounds(printer, m_CurrentPrintPage, 0, dpi, devBounds);

                //structure for the device boundaries
                tagRECT deviceRect;
                //Returns the coordinates of lower, left and upper, right corners
                double xmin, ymin, xmax, ymax;
                devBounds.QueryCoords(out xmin, out ymin, out xmax, out ymax);
                //initialize the structure for the device boundaries
                deviceRect.bottom = (int)ymax;
                deviceRect.left = (int)xmin;
                deviceRect.top = (int)ymin;
                deviceRect.right = (int)xmax;

                //determine the visible bounds of the currently printed page
                IEnvelope visBounds = new EnvelopeClass();
                page.GetPageBounds(printer, m_CurrentPrintPage, 0, visBounds);

                //get a handle to the graphics device that the print preview will be drawn to
                IntPtr hdc = e.Graphics.GetHdc();

                //print the page to the graphics device using the specified boundaries 
                axPageLayoutControl1.ActiveView.Output(hdc.ToInt32(), dpi, ref deviceRect, visBounds, null);

                //release the graphics device handle
                e.Graphics.ReleaseHdc(hdc);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void pageAndPrintSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Show the page setup dialog storing the result.
                DialogResult result = pageSetupDialog1.ShowDialog();

                //set the printer settings of the preview document to the selected printer settings
                pPrintDoc.PrinterSettings = pageSetupDialog1.PrinterSettings;

                //set the page settings of the preview document to the selected page settings
                pPrintDoc.DefaultPageSettings = pageSetupDialog1.PageSettings;

                //due to a bug in PageSetupDialog the PaperSize has to be set explicitly by iterating through the
                //available PaperSizes in the PageSetupDialog finding the selected PaperSize
                int i;
                IEnumerator paperSizes = pageSetupDialog1.PrinterSettings.PaperSizes.GetEnumerator();
                paperSizes.Reset();

                for (i = 0; i < pageSetupDialog1.PrinterSettings.PaperSizes.Count; ++i)
                {
                    paperSizes.MoveNext();
                    if (((PaperSize)paperSizes.Current).Kind == pPrintDoc.DefaultPageSettings.PaperSize.Kind)
                    {
                        pPrintDoc.DefaultPageSettings.PaperSize = ((PaperSize)paperSizes.Current);
                    }
                }

                /////////////////////////////////////////////////////////////
                ///initialize the current printer from the printer settings selected
                ///in the page setup dialog
                /////////////////////////////////////////////////////////////
                IPaper paper;
                paper = new PaperClass(); //create a paper object

                IPrinter printer;
                printer = new EmfPrinterClass(); //create a printer object
                //in this case an EMF printer, alternatively a PS printer could be used

                //initialize the paper with the DEVMODE and DEVNAMES structures from the windows GDI
                //these structures specify information about the initialization and environment of a printer as well as
                //driver, device, and output port names for a printer
                paper.Attach(pageSetupDialog1.PrinterSettings.GetHdevmode(pageSetupDialog1.PageSettings).ToInt32(), pageSetupDialog1.PrinterSettings.GetHdevnames().ToInt32());

                //pass the paper to the emf printer
                printer.Paper = paper;

                //set the page layout control's printer to the currently selected printer
                axPageLayoutControl1.Printer = printer;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //allow the user to choose the page range to be printed
                printDialog1.AllowSomePages = true;
                //show the help button.
                printDialog1.ShowHelp = true;

                //set the Document property to the PrintDocument for which the PrintPage Event 
                //has been handled. To display the dialog, either this property or the 
                //PrinterSettings property must be set 
                printDialog1.Document = pPrintDoc;

                //show the print dialog and wait for user input
                DialogResult result = printDialog1.ShowDialog();

                // If the result is OK then print the document.
                if (result == DialogResult.OK) pPrintDoc.Print();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //initialize the currently printed page number
                m_CurrentPrintPage = 0;

                //set the PrintPreviewDialog.Document property to the PrintDocument object selected by the user
                printPreviewDialog1.Document = pPrintDoc;

                //show the dialog - this triggers the document's PrintPage event
                printPreviewDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void legendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                InitialLocationofMapsurroundings();
                double dblLegendSize = 5;
                AddMapLegend(axPageLayoutControl1.PageLayout, pActiveView.FocusMap, dblMinXMF + dblMinMaxDivision, dblMinYMF + dblMinMaxDivision, dblLegendSize);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }


        public void AddMapLegend(IPageLayout pageLayout, IMap map, System.Double posX, System.Double posY, System.Double legW)
        {
            try
            {
                if (pageLayout == null || map == null)
                {
                    return;
                }
                IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer; // Dynamic Cast
                IMapFrame mapFrame = graphicsContainer.FindFrame(map) as IMapFrame; // Dynamic Cast
                ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
                uid.Value = "esriCarto.Legend";
                IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame((ESRI.ArcGIS.esriSystem.UID)uid, null); // Explicit Cast

                //Get aspect ratio
                IQuerySize querySize = mapSurroundFrame.MapSurround as IQuerySize; // Dynamic Cast
                System.Double w = 0;
                System.Double h = 0;
                querySize.QuerySize(ref w, ref h);
                System.Double aspectRatio = w / h;

                IEnvelope envelope = new EnvelopeClass();

                envelope.PutCoords(posX, posY, (posX * legW), (posY * legW / aspectRatio));
                IElement element = mapSurroundFrame as IElement; // Dynamic Cast
                element.Geometry = envelope;
                graphicsContainer.AddElement(element, 0);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }



        public void AddNorthArrow(IPageLayout pageLayout, IMap map, double posX, double posY, double division)
        {
            try
            {
                if (pageLayout == null || map == null)
                {
                    return;
                }

                IEnvelope envelope = new EnvelopeClass();
                envelope.PutCoords(posX - division, posY - division, posX, posY); //  Specify the location and size of the north arrow

                ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
                uid.Value = "esriCarto.MarkerNorthArrow";

                // Create a Surround. Set the geometry of the MapSurroundFrame to give it a location
                // Activate it and add it to the PageLayout's graphics container
                IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer; // Dynamic Cast
                IActiveView activeView = pageLayout as IActiveView; // Dynamic Cast
                IFrameElement frameElement = graphicsContainer.FindFrame(map);
                IMapFrame mapFrame = frameElement as IMapFrame; // Dynamic Cast
                IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null); // Dynamic Cast
                IElement element = mapSurroundFrame as IElement; // Dynamic Cast
                element.Geometry = envelope;
                element.Activate(activeView.ScreenDisplay);
                graphicsContainer.AddElement(element, 0);
                IMapSurround mapSurround = mapSurroundFrame.MapSurround;

                // Change out the default north arrow
                IMarkerNorthArrow markerNorthArrow = mapSurround as IMarkerNorthArrow; // Dynamic Cast
                IMarkerSymbol markerSymbol = markerNorthArrow.MarkerSymbol;
                ICharacterMarkerSymbol characterMarkerSymbol = markerSymbol as ICharacterMarkerSymbol; // Dynamic Cast
                characterMarkerSymbol.CharacterIndex = 174; // change the symbol for the North Arrow
                markerNorthArrow.MarkerSymbol = characterMarkerSymbol;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void northArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialLocationofMapsurroundings();
            AddNorthArrow(axPageLayoutControl1.PageLayout, pActiveView.FocusMap, dblMaxXMF - dblMinMaxDivision, dblMaxYMF - dblMinMaxDivision, dblMinMaxDivision);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }




        private void scaleBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialLocationofMapsurroundings();
            frmScaleBarProperties pfrmScaleBarProp = new frmScaleBarProperties();
            pfrmScaleBarProp.pPageLayout = axPageLayoutControl1.PageLayout;
            pfrmScaleBarProp.pActiveView = pActiveView;
            pfrmScaleBarProp.dblMaxX = dblMaxXMF;
            pfrmScaleBarProp.dblMinY = dblMinYMF;
            pfrmScaleBarProp.dblProportion = dblMinMaxDivision;

            pfrmScaleBarProp.Show();

        }
        
    }
}
