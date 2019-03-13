using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.DataVisualization.Charting;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;
using System.Drawing.Drawing2D;

namespace VisUncertainty
{
    public partial class frmSymbology : Form
    {
        //private IMapControl3 m_mapControl;

        public ILayer mlayer;

        //Private Members
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_fLayer;
        private clsSnippet m_pSnippet;
        private ClassBreaksRenderer m_pRender;
        private double[] cb;
        public IEnumColors pEnumColors;
        private int intGCBreakeCount;
        private MainForm mForm;
        private int[,] arrColors;
        private string[] arrLabel;
        private int intRounding;
        private int intNofFeatures;
        private string m_strClassificationMethod;

        private string[] m_colorRampNames;
        private List<Color>[] m_colorLists;

        private Image[] m_previewImages;
        private ICartographicLineSymbol m_pOutline;
        private IRgbColor m_pOutColor;
        private bool blnClassRedraw = false;

        //Proportional symbol map members
        private IProportionalSymbolRenderer m_pPropRenderer;
        private double m_dblMinSymbolSize;
        private double m_dblMaxSymbolSize;
        private double m_dblMinDataValue;
        private double m_dblMaxDataValue;
        private ISimpleMarkerSymbol m_pMinMarkerSymbol;
        private ISimpleLineSymbol m_pMinLineSymbol;
        public ISimpleFillSymbol m_BackSymbol;

        //Simple Symbol members
        public IStyleGalleryItem m_styleGalleryItem;
        private bool m_blnInitializing;

        //Graduate Symbol map renderer
        private IRgbColor m_pSymColor;
        private double[] m_arrSizes;

        #region Form initializing and general functions
        public frmSymbology()
        {
            InitializeComponent();
            

            //Create Color ramps for drawing event in combobox;
            cboColorRamp.DrawMode = DrawMode.OwnerDrawVariable;
            clsColorRamps pColorRamps = new clsColorRamps();
            m_colorRampNames = pColorRamps.colorRampNames;
            m_colorLists = pColorRamps.CreateColorList();
        }

        private void frmProperties_Load(object sender, EventArgs e)
        {
            try
            {
                m_blnInitializing = true;
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                //m_mapControl = (IMapControl3)mForm.axMapControl1.Object;
                //mlayer = (ILayer)m_mapControl.CustomProperty;
                m_pSnippet = new clsSnippet();
                m_fLayer = (IFeatureLayer)mlayer;
                m_pFClass = (IFeatureClass)m_fLayer.FeatureClass;

                //IGeoDataset GeoDataset = (IGeoDataset)m_pFClass;
                //ISpatialReference pSpatialReference = GeoDataset.SpatialReference;
                //ESRI.ArcGIS.Geodatabase.IDataset dataset = (ESRI.ArcGIS.Geodatabase.IDataset)(m_fLayer);


                this.Text = mlayer.Name + " Symbology";

                //Load Symbols sytle
                //Get the ArcGIS install location
                string sInstall = ESRI.ArcGIS.RuntimeManager.ActiveRuntime.Path;

                //Load the ESRI.ServerStyle file into the SymbologyControl
                axSymbologyControl1.LoadStyleFile(sInstall + "\\Styles\\ESRI.ServerStyle");


                //if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                //    tbcProperties.SelectedIndex = 0;
                //else
                //    tbcProperties.SelectedIndex = 1;



                //Load Simple symbol setting based on a feature type
                //Select SymbologyStyleClass based upon feature type
                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        //Simple 
                        cboSimSymStyle.Items.Clear();
                        cboSimSymStyle.Items.Add("Circle"); cboSimSymStyle.Items.Add("Square"); cboSimSymStyle.Items.Add("Cross"); cboSimSymStyle.Items.Add("X"); cboSimSymStyle.Items.Add("Diamond");
                        cboSimSymStyle.Text = "Circle";
                        //Proportional
                        btnProBackSymbol.Visible = false;
                        //GraduatedSymbol
                        btnGSBackSymbol.Visible = false;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        //SimpleSymbol
                        cboSimSymStyle.Items.Clear();
                        cboSimSymStyle.Items.Add("Solid"); cboSimSymStyle.Items.Add("Dashed"); cboSimSymStyle.Items.Add("Dotted");
                        cboSimSymStyle.Text = "Solid";
                        lblSimSymSize.Visible = false; nudSimSymSize.Visible = false;
                        //Proportional symbol
                        lblProSize.Visible = false; nudProSize.Visible = false;
                        lblSimOutColor.Visible = false; picSimOutColor.Visible = false;
                        lblProOutColor.Visible = false; picProOutColor.Visible = false;
                        chkSimOutlines.Visible = false;
                        chkProOutline.Visible = false; chkFlannery.Visible = false;
                        lblSimOutWidth.Text = "Width:";
                        lblProOutWidth.Text = "Width:";
                        btnProBackSymbol.Visible = false;
                        //Choropleth
                        nudChoSymbolSize.Visible = false; lblChoSymbolSize.Visible = false;
                        lblGCLineWidth.Text = "Line width:";
                        lblGCLineColor.Visible = false; picGCLineColor.Visible = false;
                        //GraduateSymobl
                        lblGSOutlineColor.Visible = false; picGSOutColor.Visible = false;
                        lblGSOutlineWidth.Visible = false; nudGSOutWidth.Visible = false;
                        btnGSBackSymbol.Visible = false;
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        //Simple
                        lblSimSymSize.Visible = false; nudSimSymSize.Visible = false;
                        chkSimOutlines.Visible = false;
                        lblSimSymStyle.Visible = false; cboSimSymStyle.Visible = false;
                        picSimPreview.BorderStyle = BorderStyle.None;
                        //Choropleth
                        nudChoSymbolSize.Visible = false; lblChoSymbolSize.Visible = false;
                        break;
                }


                //Add White images to remove X mark in grid view 081117 HK
                IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();
                IRgbColor pRGBColor = m_pSnippet.getRGB(255, 255, 255);
                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                        pSimpleMarkerSym.Color = (IColor)pRGBColor;
                        pSimpleMarkerSym.Outline = false;
                        styleGalleryItem.Item = (ISymbol)pSimpleMarkerSym;
                        dgvSymbols.Rows.Clear();
                        dgvSymbols.Rows[0].Cells[0].Value = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
                        dgvGSSymbol.Rows.Clear();
                        dgvGSSymbol.Rows[0].Cells[0].Value = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pSimpleLinerSym = new SimpleLineSymbolClass();
                        pSimpleLinerSym.Color = (IColor)pRGBColor;
                        pSimpleLinerSym.Width = 0;

                        styleGalleryItem.Item = (ISymbol)pSimpleLinerSym;
                        dgvSymbols.Rows.Clear();
                        dgvSymbols.Rows[0].Cells[0].Value = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);
                        dgvGSSymbol.Rows.Clear();
                        dgvGSSymbol.Rows[0].Cells[0].Value = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);

                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();

                        m_pOutline = new CartographicLineSymbolClass();
                        m_pOutline.Color = (IColor)pRGBColor;
                        m_pOutline.Width = 0;

                        pFillSymbol.Color = (IColor)pRGBColor;
                        pFillSymbol.Outline = m_pOutline;
                        styleGalleryItem.Item = (ISymbol)pFillSymbol;

                        dgvSymbols.Rows.Clear();
                        dgvSymbols.Rows[0].Cells[0].Value = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);
                        dgvGSSymbol.Rows.Clear();
                        dgvGSSymbol.Rows[0].Cells[0].Value = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);
                        break;
                }


                IFields fields = m_pFClass.Fields;
                cboProValue.Items.Clear();
                cboValueField.Items.Clear();
                cboUncernFld.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                    cboUncernFld.Items.Add(fields.get_Field(i).Name);
                    cboProValue.Items.Add(fields.get_Field(i).Name);
                    cboGSvaluefld.Items.Add(fields.get_Field(i).Name);
                }

                IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)m_fLayer;
                //Change tab focus according to a renderer type
                if (geoFeatureLayer.Renderer is IClassBreaksRenderer) 
                    tbcProperties.SelectedIndex = 1;
                else if (geoFeatureLayer.Renderer is IProportionalSymbolRenderer)
                    tbcProperties.SelectedIndex = 2;
                else
                {
                    tbcProperties.SelectedIndex = 0;
                    DisplaySimpleSymbols();
                }
                m_blnInitializing = false;
                //lblLayerName.Text = mlayer.Name;
                //lblLayerProjection.Text = pSpatialReference.Name;
                //txtFilePath.Text = dataset.Workspace.PathName;
                intRounding = 2;
                intNofFeatures = m_pFClass.FeatureCount(null);

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void tbcProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)m_fLayer;

                if (tbcProperties.SelectedIndex == 0)
                {
                    DisplaySimpleSymbols();
                    //Changes others.
                }


                if (tbcProperties.SelectedIndex == 1) //Select Choropleth tabl
                {

                    if (geoFeatureLayer.Renderer is IClassBreaksRenderer) //When it has simple renderer, collect information from layer
                    {
                        IClassBreaksRenderer pCBRenderer = (IClassBreaksRenderer)geoFeatureLayer.Renderer;


                        IFields fields = m_pFClass.Fields;

                        cboValueField.Items.Clear();
                        cboUncernFld.Items.Clear();

                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            cboValueField.Items.Add(fields.get_Field(i).Name);
                            if (pCBRenderer.Field == fields.get_Field(i).Name)
                                cboValueField.Text = fields.get_Field(i).Name;
                        }

                        nudGCNClasses.Value = Convert.ToDecimal(pCBRenderer.BreakCount);
                    }
                }

                if (tbcProperties.SelectedIndex == 2) //Select Prop tabl
                {
                    if (geoFeatureLayer.Renderer is IProportionalSymbolRenderer) //When it has simple renderer, collect information from layer
                    {
                        IProportionalSymbolRenderer pProRenderer = (IProportionalSymbolRenderer)geoFeatureLayer.Renderer;


                        IFields fields = m_pFClass.Fields;

                        cboProValue.Items.Clear();

                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            cboProValue.Items.Add(fields.get_Field(i).Name);
                            if (pProRenderer.Field == fields.get_Field(i).Name)
                                cboProValue.Text = fields.get_Field(i).Name;
                        }

                        ISimpleMarkerSymbol pSimMarkerSymbol = (ISimpleMarkerSymbol)pProRenderer.MinSymbol;
                        IRgbColor pRgbColor = (IRgbColor)pSimMarkerSymbol.Color;
                        picProColor.BackColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                        chkProOutline.Checked = pSimMarkerSymbol.Outline;
                        
                        if (chkSimOutlines.Checked)
                        {
                            IRgbColor pOutColor = (IRgbColor)pSimMarkerSymbol.OutlineColor;
                            picProOutColor.BackColor = Color.FromArgb(pOutColor.Red, pOutColor.Green, pOutColor.Blue);
                            nudProOutWidth.Value = Convert.ToDecimal(pSimMarkerSymbol.OutlineSize);
                        }

                        nudProSize.Value = Convert.ToDecimal(pSimMarkerSymbol.Size);
                    }
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        #endregion

        #region Simple mapping functions
        private void DisplaySimpleSymbols()
        {

            //Add Current Symbol
            IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)m_fLayer;

            //Get the IStyleGalleryItem
            IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();

            ISimpleRenderer simpleRenderer;
            //styleGalleryItem.Item = symbol;
            if (geoFeatureLayer.Renderer is ISimpleRenderer) //When it has simple renderer, collect information from layer
            {
                simpleRenderer = (ISimpleRenderer)geoFeatureLayer.Renderer;

                ISymbol pSymbol = simpleRenderer.Symbol;
                IRgbColor pRgbColor = new RgbColor();
                IRgbColor pOutColor = new RgbColor();

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol pSimMarkerSymbol = (ISimpleMarkerSymbol)pSymbol;
                        pRgbColor = (IRgbColor)pSimMarkerSymbol.Color;
                        picSimSymColor.BackColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                        nudSimSymSize.Value = Convert.ToDecimal(pSimMarkerSymbol.Size);
                        chkSimOutlines.Checked = pSimMarkerSymbol.Outline;
                        switch (pSimMarkerSymbol.Style)
                        {
                            case esriSimpleMarkerStyle.esriSMSCircle:
                                cboSimSymStyle.Text = "Circle";
                                break;
                            case esriSimpleMarkerStyle.esriSMSCross:
                                break;
                            case esriSimpleMarkerStyle.esriSMSDiamond:
                                cboSimSymStyle.Text = "Diamond";
                                break;
                            case esriSimpleMarkerStyle.esriSMSSquare:
                                cboSimSymStyle.Text = "Square";
                                break;
                            case esriSimpleMarkerStyle.esriSMSX:
                                cboSimSymStyle.Text = "X";
                                break;
                        }

                        if (chkSimOutlines.Checked)
                        {
                            lblSimOutColor.Enabled = true; picSimOutColor.Enabled = true;
                            lblSimOutWidth.Enabled = true; nudSimOutWidth.Enabled = true;
                            pOutColor = (IRgbColor)pSimMarkerSymbol.OutlineColor;
                            picSimOutColor.BackColor = Color.FromArgb(pOutColor.Red, pOutColor.Green, pOutColor.Blue);
                            nudSimOutWidth.Value = Convert.ToDecimal(pSimMarkerSymbol.OutlineSize);
                        }
                        else
                        {
                            lblSimOutColor.Enabled = false; picSimOutColor.Enabled = false;
                            lblSimOutWidth.Enabled = false; nudSimOutWidth.Enabled = false;
                        }
                        styleGalleryItem.Item = (ISymbol)pSimMarkerSymbol;
                        PreviewImage1(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pSimLineSymbol = (ISimpleLineSymbol)pSymbol;
                        pRgbColor = (IRgbColor)pSimLineSymbol.Color;
                        picSimSymColor.BackColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                        nudSimOutWidth.Value = Convert.ToDecimal(pSimLineSymbol.Width);

                        switch (cboSimSymStyle.Text)
                        {
                            case "Solid":
                                pSimLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                                break;
                            case "Dashed":
                                pSimLineSymbol.Style = esriSimpleLineStyle.esriSLSDash;
                                break;
                            case "Dotted":
                                pSimLineSymbol.Style = esriSimpleLineStyle.esriSLSDot;
                                break;
                        }
                        styleGalleryItem.Item = (ISymbol)pSimLineSymbol;
                        PreviewImage1(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleFillSymbol pSimFillSymbol = (ISimpleFillSymbol)pSymbol;
                        pRgbColor = (IRgbColor)pSimFillSymbol.Color;
                        picSimSymColor.BackColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                        pOutColor = (IRgbColor)pSimFillSymbol.Outline.Color;
                        picSimOutColor.BackColor = Color.FromArgb(pOutColor.Red, pOutColor.Green, pOutColor.Blue);
                        nudSimOutWidth.Value = Convert.ToDecimal(pSimFillSymbol.Outline.Width);
                        styleGalleryItem.Item = (ISymbol)pSimFillSymbol;
                        PreviewImage1(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);
                        break;
                }
            }
            else //Others default symbols
            {
                simpleRenderer = new SimpleRendererClass();
                IRgbColor pRgbColor = new RgbColor();
                pRgbColor = m_pSnippet.getRGB(picSimSymColor.BackColor.R, picSimSymColor.BackColor.G, picSimSymColor.BackColor.B);

                IRgbColor pOutColor = new RgbColor();
                pOutColor = m_pSnippet.getRGB(picSimOutColor.BackColor.R, picSimOutColor.BackColor.G, picSimOutColor.BackColor.B);

                double dblOutlineWidth = Convert.ToDouble(nudSimOutWidth.Value);

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol pSimMarkerSymbol = new SimpleMarkerSymbolClass();
                        pRgbColor = m_pSnippet.getRGB(picSimSymColor.BackColor.R, picSimSymColor.BackColor.G, picSimSymColor.BackColor.B);
                        pSimMarkerSymbol.Color = (IColor)pRgbColor;
                        pSimMarkerSymbol.Size = Convert.ToDouble(nudSimSymSize.Value);
                        pSimMarkerSymbol.Outline = chkSimOutlines.Checked;

                        switch (cboSimSymStyle.Text)
                        {
                            case "Circle":
                                pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                                break;
                            case "Cross":
                                pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCross;
                                break;
                            case "Diamond":
                                pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
                                break;
                            case "Square":
                                pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
                                break;
                            case "X":
                                pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSX;
                                break;
                        }

                        if (chkSimOutlines.Checked)
                        {
                            pOutColor = m_pSnippet.getRGB(picSimOutColor.BackColor.R, picSimOutColor.BackColor.G, picSimOutColor.BackColor.B);
                            pSimMarkerSymbol.OutlineColor = (IColor)pOutColor;
                            pSimMarkerSymbol.OutlineSize = Convert.ToDouble(nudSimOutWidth.Value);
                        }
                        simpleRenderer.Symbol = (ISymbol)pSimMarkerSymbol;
                        styleGalleryItem.Item = (ISymbol)pSimMarkerSymbol;
                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, simpleRenderer.Symbol);
                        PreviewImage1(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
                        pLineSymbol.Color = (IColor)pRgbColor;
                        pLineSymbol.Width = dblOutlineWidth;
                        simpleRenderer.Symbol = (ISymbol)pLineSymbol;
                        styleGalleryItem.Item = (ISymbol)pLineSymbol;
                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassLineSymbols, simpleRenderer.Symbol);
                        PreviewImage1(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                        pFillSymbol.Color = (IColor)pRgbColor;
                        ICartographicLineSymbol pOutline = new CartographicLineSymbolClass();
                        pOutline.Color = (IColor)pOutColor;
                        pOutline.Width = dblOutlineWidth;
                        pFillSymbol.Outline = pOutline;
                        simpleRenderer.Symbol = (ISymbol)pFillSymbol;
                        styleGalleryItem.Item = (ISymbol)pFillSymbol;
                        PreviewImage1(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);
                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassFillSymbols, simpleRenderer.Symbol);
                        break;
                }

            }
        }

        private void PreviewImage1(esriSymbologyStyleClass styleClass, IStyleGalleryItem styleGalleryItem)
        {
            m_styleGalleryItem = styleGalleryItem;
            axSymbologyControl1.StyleClass = styleClass;
            //Get and set the style class 
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
            //Preview an image of the symbol
            stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(styleGalleryItem, picSimPreview.Width, picSimPreview.Height);
            System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            picSimPreview.Image = image;
        }
        private void UpdateSimpleLineSymbol()
        {
            if (m_styleGalleryItem.Item == null) return; //m_styleGallery is not null, after initializng
            ISymbol pSymbol = (ISymbol)m_styleGalleryItem.Item;
            ISimpleLineSymbol pSimLineSymbol = (ISimpleLineSymbol)pSymbol;
            IRgbColor pRgbColor = m_pSnippet.getRGB(picSimSymColor.BackColor.R, picSimSymColor.BackColor.G, picSimSymColor.BackColor.B);
            pSimLineSymbol.Color = (IColor)pRgbColor;

            double dblSimOutWidth = Convert.ToDouble(nudSimOutWidth.Value);
            pSimLineSymbol.Width = dblSimOutWidth;
            switch (cboSimSymStyle.Text)
            {
                case "Solid":
                    pSimLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    break;
                case "Dashed":
                    pSimLineSymbol.Style = esriSimpleLineStyle.esriSLSDash;
                    break;
                case "Dotted":
                    pSimLineSymbol.Style = esriSimpleLineStyle.esriSLSDot;
                    break;
            }

            IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();
            styleGalleryItem.Item = (ISymbol)pSimLineSymbol;
            PreviewImage1(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);
        }

        private void UpdateSimpleFillSymbol()
        {
            if (m_styleGalleryItem.Item == null) return;//m_styleGallery is not null, after initializng
            ISymbol pSymbol = (ISymbol)m_styleGalleryItem.Item;
            ISimpleFillSymbol pSimFillSymbol = (ISimpleFillSymbol)pSymbol;
            IRgbColor pRgbColor = m_pSnippet.getRGB(picSimSymColor.BackColor.R, picSimSymColor.BackColor.G, picSimSymColor.BackColor.B);
            pSimFillSymbol.Color = (IColor)pRgbColor;

            IRgbColor pColorOutline = new RgbColor();
            //Can Change the color in here!
            pColorOutline = m_pSnippet.getRGB(picSimOutColor.BackColor.R, picSimOutColor.BackColor.G, picSimOutColor.BackColor.B);
            double dblSimOutWidth = Convert.ToDouble(nudSimOutWidth.Value);

            ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
            pOutLines.Width = dblSimOutWidth;
            pOutLines.Color = (IColor)pColorOutline;

            pSimFillSymbol.Outline = pOutLines;

            m_styleGalleryItem.Item = (ISymbol)pSimFillSymbol;

            IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();
            styleGalleryItem.Item = (ISymbol)pSimFillSymbol;
            PreviewImage1(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);
        }

        private void UpdateSimpleMarkerSymbol()
        {
            if (m_styleGalleryItem.Item == null) return;//m_styleGallery is not null, after initializng
            ISymbol pSymbol = (ISymbol)m_styleGalleryItem.Item;
            ISimpleMarkerSymbol pSimMarkerSymbol = (ISimpleMarkerSymbol)pSymbol;
            IRgbColor pRgbColor = m_pSnippet.getRGB(picSimSymColor.BackColor.R, picSimSymColor.BackColor.G, picSimSymColor.BackColor.B);
            pSimMarkerSymbol.Color = (IColor)pRgbColor;
            pSimMarkerSymbol.Size = Convert.ToDouble(nudSimSymSize.Value);
            IRgbColor pColorOutline = new RgbColor();
            //Can Change the color in here!
            pColorOutline = m_pSnippet.getRGB(picSimOutColor.BackColor.R, picSimOutColor.BackColor.G, picSimOutColor.BackColor.B);
            double dblSimOutWidth = Convert.ToDouble(nudSimOutWidth.Value);

            switch (cboSimSymStyle.Text)
            {
                case "Circle":
                    pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    break;
                case "Cross":
                    pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCross;
                    break;
                case "Diamond":
                    pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
                    break;
                case "Square":
                    pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
                    break;
                case "X":
                    pSimMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSX;
                    break;
            }

            if (chkSimOutlines.Checked)
            {
                pSimMarkerSymbol.Outline = true;
                pSimMarkerSymbol.OutlineColor = (IColor)pColorOutline;
                pSimMarkerSymbol.OutlineSize = dblSimOutWidth;
            }
            else
                pSimMarkerSymbol.Outline = false;

            IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();
            styleGalleryItem.Item = (ISymbol)pSimMarkerSymbol;
            PreviewImage1(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
        }

        private void nudSimOutWidth_ValueChanged(object sender, EventArgs e)
        {
            if (m_blnInitializing == true) return; //When initializing components, do not update symbols
            switch (m_fLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    UpdateSimpleMarkerSymbol();
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    UpdateSimpleLineSymbol();
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    UpdateSimpleFillSymbol();
                    break;
            }

        }

        private void picSimSymColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
            {
                picSimSymColor.BackColor = cdColor.Color;
                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        UpdateSimpleMarkerSymbol();
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        UpdateSimpleLineSymbol();
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        UpdateSimpleFillSymbol();
                        break;
                }
            }

        }

        private void axSymbologyControl1_OnItemSelected(object sender, ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            //Preview the selected item
            m_styleGalleryItem = (IStyleGalleryItem)e.styleGalleryItem;
        }

        private void nudSimSymSize_ValueChanged(object sender, EventArgs e)
        {
            if (m_blnInitializing == true) return; //When initializing components, do not update symbols

            switch (m_fLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    UpdateSimpleMarkerSymbol();
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    UpdateSimpleLineSymbol();
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    UpdateSimpleFillSymbol();
                    break;
            }
        }

        private void btnSimCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSimApply_Click(object sender, EventArgs e)
        {
            IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)m_fLayer;

            //Create a new renderer
            ISimpleRenderer simpleRenderer = new SimpleRendererClass();
            //Set its symbol from the styleGalleryItem
            simpleRenderer.Symbol = (ISymbol)m_styleGalleryItem.Item;
            //Set the renderer into the geoFeatureLayer
            geoFeatureLayer.Renderer = (IFeatureRenderer)simpleRenderer;

            //Fire contents changed event that the TOCControl listens to
            mForm.axMapControl1.ActiveView.ContentsChanged();
            //Refresh the display
            mForm.axMapControl1.Refresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        private void picSimOutColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
            {
                picSimOutColor.BackColor = cdColor.Color;
                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        UpdateSimpleMarkerSymbol();
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        UpdateSimpleLineSymbol();
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        UpdateSimpleFillSymbol();
                        break;
                }
            }
        }

        private void chkSimOutlines_CheckedChanged(object sender, EventArgs e)
        {
            if (m_blnInitializing == true) return; //When initializing components, do not update symbols

            if (chkSimOutlines.Checked)
            {
                lblSimOutColor.Enabled = true; picSimOutColor.Enabled = true;
                lblSimOutWidth.Enabled = true; nudSimOutWidth.Enabled = true;
            }
            else
            {
                lblSimOutColor.Enabled = false; picSimOutColor.Enabled = false;
                lblSimOutWidth.Enabled = false; nudSimOutWidth.Enabled = false;
            }

            switch (m_fLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    UpdateSimpleMarkerSymbol();
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    UpdateSimpleLineSymbol();
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    UpdateSimpleFillSymbol();
                    break;
            }
        }

        private void cboSimSymStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_blnInitializing == true) return; //When initializing components, do not update symbols
            switch (m_fLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    UpdateSimpleMarkerSymbol();
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    UpdateSimpleLineSymbol();
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    UpdateSimpleFillSymbol();
                    break;
            }
        }

        #endregion

        #region Choropleth Mapping functions

        private void dgvSymbols_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                {
                    string value = intRounding.ToString();
                    if (InputBox("Decimal Places", "Number of decimal places:", ref value) == DialogResult.OK)
                    {
                        intRounding = Convert.ToInt32(value);
                        UpdateData(intGCBreakeCount);
                        //UpdateRange(lvSymbol, intGCBreakeCount);
                    }
                }

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void nudGCLinewidth_ValueChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }

        private void nudChoSymbolSize_ValueChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }
        private void cboValueField_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (cb == null)
                    return;
                if (cboValueField.Text == "")
                    return;

                m_pSnippet.AddRenderInfo(mForm, m_fLayer, cboValueField.Text, string.Empty, cb, arrColors, m_strClassificationMethod);

                IRgbColor pColorOutline = new RgbColor();
                //Can Change the color in here!
                pColorOutline = m_pSnippet.getRGB(picGCLineColor.BackColor.R, picGCLineColor.BackColor.G, picGCLineColor.BackColor.B);
                double dblGCOutlineSize = Convert.ToDouble(nudGCLinewidth.Value);



                //' use this interface to set dialog properties
                IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)m_pRender;
                pUIProperties.ColorRamp = "Custom";

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol pSimpleMarkerSym;
                        double dblSymbolSize = Convert.ToDouble(nudChoSymbolSize.Value);
                        pEnumColors.Reset();
                        //' be careful, indices are different for the diff lists
                        for (int j = 0; j < intGCBreakeCount; j++)
                        {
                            m_pRender.Break[j] = cb[j + 1];
                            m_pRender.Label[j] = arrLabel[j];
                            pUIProperties.LowBreak[j] = cb[j];
                            pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                            IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                            pSimpleMarkerSym.Color = (IColor)pRGBColor;
                            pSimpleMarkerSym.Outline = true;
                            pSimpleMarkerSym.OutlineSize = dblGCOutlineSize;
                            pSimpleMarkerSym.Size = dblSymbolSize;
                            pSimpleMarkerSym.OutlineColor = (IColor)pColorOutline;
                            m_pRender.Symbol[j] = (ISymbol)pSimpleMarkerSym;
                        }

                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pSimpleLinerSym;

                        pEnumColors.Reset();
                        //' be careful, indices are different for the diff lists
                        for (int j = 0; j < intGCBreakeCount; j++)
                        {
                            m_pRender.Break[j] = cb[j + 1];
                            m_pRender.Label[j] = arrLabel[j];
                            pUIProperties.LowBreak[j] = cb[j];
                            pSimpleLinerSym = new SimpleLineSymbolClass();
                            IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                            pSimpleLinerSym.Color = (IColor)pRGBColor;
                            pSimpleLinerSym.Width = dblGCOutlineSize;
                            m_pRender.Symbol[j] = (ISymbol)pSimpleLinerSym;
                        }
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
                        pOutLines.Width = dblGCOutlineSize;
                        pOutLines.Color = (IColor)pColorOutline;

                        ISimpleFillSymbol pSimpleFillSym;

                        pEnumColors.Reset();
                        //' be careful, indices are different for the diff lists
                        for (int j = 0; j < intGCBreakeCount; j++)
                        {
                            m_pRender.Break[j] = cb[j + 1];
                            m_pRender.Label[j] = arrLabel[j];
                            pUIProperties.LowBreak[j] = cb[j];
                            pSimpleFillSym = new SimpleFillSymbolClass();
                            IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                            pSimpleFillSym.Color = (IColor)pRGBColor;
                            pSimpleFillSym.Outline = pOutLines;
                            
                            m_pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
                        }
                        break;
                }


                IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)m_fLayer;

                pGeofeatureLayer.Renderer = (IFeatureRenderer)m_pRender;

                mForm.axMapControl1.ActiveView.Refresh();
                mForm.axTOCControl1.Update();
                //mForm.axTOCControl1.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        public void DrawSymboliinDataGridView()
        {
            try
            {

                string strGCRenderField = cboValueField.Text;
                string strUncernfld = cboUncernFld.Text;

                //Error control to deal with binary data (1.0.6 Update)
                int intUniqueCnt = GetUniqueCnt(strGCRenderField);
                if (intUniqueCnt < Convert.ToInt32(nudGCNClasses.Value))
                {
                    nudGCNClasses.Value = intUniqueCnt;
                    //return;
                }

                intGCBreakeCount = Convert.ToInt32(nudGCNClasses.Value);

                double dblOutlineWidth = Convert.ToDouble(nudGCLinewidth.Value);
                double dblGCSymbolSize = Convert.ToDouble(nudChoSymbolSize.Value);
                m_pOutColor = m_pSnippet.getRGB(picGCLineColor.BackColor.R, picGCLineColor.BackColor.G, picGCLineColor.BackColor.B);

                int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
                int intUncernfldIdx = m_pFClass.FindField(strUncernfld);

                if (intUncernfldIdx == -1 && cboGCClassify.Text == "Class Separability")
                    return;

                ITable pTable = (ITable)m_pFClass;
                IClassifyGEN pClassifyGEN = null;

                switch (cboGCClassify.Text)
                {
                    case "Equal Interval":
                        pClassifyGEN = new EqualIntervalClass();
                        break;
                    case "Geometrical Interval":
                        pClassifyGEN = new GeometricalInterval();
                        break;
                    case "Natural Breaks":
                        pClassifyGEN = new NaturalBreaksClass();
                        break;
                    case "Quantile":
                        pClassifyGEN = new QuantileClass();
                        break;
                    case "StandardDeviation":
                        pClassifyGEN = new StandardDeviationClass();
                        break;
                    default:
                        pClassifyGEN = new NaturalBreaksClass();
                        break;
                }

                m_strClassificationMethod = cboGCClassify.Text;


                //ITableHistogram pTableHistogram = new TableHistogramClass();
                ITableHistogram pTableHistogram2 = new BasicTableHistogramClass();

                pTableHistogram2.Field = strGCRenderField;
                pTableHistogram2.Table = pTable;
                //IHistogram pHistogram = (IHistogram)pTableHistogram2;
                IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram2;
                object xVals, frqs;
                pHistogram.GetHistogram(out xVals, out frqs);
                pClassifyGEN.Classify(xVals, frqs, intGCBreakeCount);
                cb = (double[])pClassifyGEN.ClassBreaks;

                m_pRender = new ClassBreaksRenderer();

                m_pRender.Field = strGCRenderField;
                m_pRender.BreakCount = intGCBreakeCount;
                m_pRender.MinimumBreak = cb[0];

                string strColorRamp = cboColorRamp.Text;

                pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, intGCBreakeCount);
                pEnumColors.Reset();

                arrColors = new int[intGCBreakeCount, 3];

                IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol pSimpleMarkerSym = new SimpleMarkerSymbolClass();

                        m_previewImages = new Image[intGCBreakeCount];

                        for (int k = 0; k < intGCBreakeCount; k++)
                        {
                            IColor pColor = pEnumColors.Next();
                            IRgbColor pRGBColor = new RgbColorClass();
                            pRGBColor.RGB = pColor.RGB;

                            pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                            styleGalleryItem = new ServerStyleGalleryItem();

                            pSimpleMarkerSym.Color = (IColor)pRGBColor;
                            pSimpleMarkerSym.OutlineColor = (IColor)m_pOutColor;
                            pSimpleMarkerSym.OutlineSize = dblOutlineWidth;
                            pSimpleMarkerSym.Size = dblGCSymbolSize;
                            styleGalleryItem.Item = (ISymbol)pSimpleMarkerSym;
                            m_previewImages[k] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);


                            arrColors[k, 0] = pRGBColor.Red;
                            arrColors[k, 1] = pRGBColor.Green;
                            arrColors[k, 2] = pRGBColor.Blue;
                        }
                        pEnumColors.Reset();
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pSimpleLinerSym = new SimpleLineSymbolClass();

                        m_previewImages = new Image[intGCBreakeCount];

                        for (int k = 0; k < intGCBreakeCount; k++)
                        {
                            IColor pColor = pEnumColors.Next();
                            IRgbColor pRGBColor = new RgbColorClass();
                            pRGBColor.RGB = pColor.RGB;

                            pSimpleLinerSym = new SimpleLineSymbolClass();
                            styleGalleryItem = new ServerStyleGalleryItem();

                            pSimpleLinerSym.Color = (IColor)pRGBColor;
                            pSimpleLinerSym.Width = dblOutlineWidth;

                            styleGalleryItem.Item = (ISymbol)pSimpleLinerSym;
                            m_previewImages[k] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);

                            arrColors[k, 0] = pRGBColor.Red;
                            arrColors[k, 1] = pRGBColor.Green;
                            arrColors[k, 2] = pRGBColor.Blue;
                        }
                        pEnumColors.Reset();
                        break;
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();

                        m_pOutline = new CartographicLineSymbolClass();
                        m_pOutline.Color = (IColor)m_pOutColor;
                        m_pOutline.Width = dblOutlineWidth;

                        m_previewImages = new Image[intGCBreakeCount];

                        for (int k = 0; k < intGCBreakeCount; k++)
                        {
                            IColor pColor = pEnumColors.Next();
                            IRgbColor pRGBColor = new RgbColorClass();
                            pRGBColor.RGB = pColor.RGB;

                            pFillSymbol = new SimpleFillSymbolClass();
                            styleGalleryItem = new ServerStyleGalleryItem();

                            pFillSymbol.Color = (IColor)pRGBColor;
                            pFillSymbol.Outline = m_pOutline;
                            styleGalleryItem.Item = (ISymbol)pFillSymbol;
                            m_previewImages[k] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);


                            arrColors[k, 0] = pRGBColor.Red;
                            arrColors[k, 1] = pRGBColor.Green;
                            arrColors[k, 2] = pRGBColor.Blue;
                        }
                        pEnumColors.Reset();
                        break;
                }
                UpdateData(intGCBreakeCount);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void UpdateData(int intGCBreakeCount)
        {
            dgvSymbols.Rows.Clear();
            double dblAdding = Math.Pow(0.1, intRounding);
            arrLabel = new string[intGCBreakeCount];

            for (int j = 0; j < intGCBreakeCount; j++)
            {
                if (j < intGCBreakeCount - 1)
                    dgvSymbols.Rows.Add();

                dgvSymbols.Rows[j].Cells[0].Value = m_previewImages[j];

                string strLabel = "";
                if (j == 0)
                    strLabel = Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString());
                else
                    strLabel = Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString());

                dgvSymbols.Rows[j].Cells[1].Value = strLabel;
                dgvSymbols.Rows[j].Cells[2].Value = strLabel;
                arrLabel[j] = strLabel;

            }


        }
        private Image GetPreviewImage(esriSymbologyStyleClass styleClass, IStyleGalleryItem styleGalleryItem)
        {
            axSymbologyControl1.StyleClass = styleClass;
            //Get and set the style class 
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(axSymbologyControl1.StyleClass);
            //Preview an image of the symbol
            int intImagewidth = Convert.ToInt32(dgvSymbols.Columns[0].Width * 0.9);
            int intImageHeight = Convert.ToInt32(dgvSymbols.Rows[0].Height * 0.9);
            stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(styleGalleryItem, intImagewidth, intImageHeight);
            System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            return image;
        }


        private void cboGCClassify_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGCClassify.Text != "Class Separability")
            {
                if (cboUncernFld.Enabled)
                {
                    cboUncernFld.Text = "";
                    cboUncernFld.Enabled = false;
                }
                if (lblUncerfld.Enabled)
                    lblUncerfld.Enabled = false;
                //DrawSymbolinListView();
                DrawSymboliinDataGridView();
            }
            else
            {
                if (cboUncernFld.Enabled == false)
                    cboUncernFld.Enabled = true;
                if (lblUncerfld.Enabled == false)
                    lblUncerfld.Enabled = true;
                //DrawSymbolinListView();
                DrawSymboliinDataGridView();
            }
        }

        private void nudGCNClasses_ValueChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }

        private void cboAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }

        private void picSymolfrom_BackColorChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }

        private void picSymbolTo_BackColorChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }


        private void picGCLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGCLineColor.BackColor = cdColor.Color;

            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }


        private DialogResult PopupInput(ListViewItem.ListViewSubItem pSelectedSubItems, int border, int length, ref string output)
        {
                System.Drawing.Point ctrlpt = pSelectedSubItems.Bounds.Location;
                ctrlpt = this.PointToScreen(pSelectedSubItems.Bounds.Location);
                ctrlpt.Y += 153;
                ctrlpt.X += 70;
                TextBox input = new TextBox { Height = 20, Width = length, Top = border / 2, Left = border / 2 };
                input.BorderStyle = BorderStyle.FixedSingle;
                input.Text = output;
                //######## SetColor to your preference
                input.BackColor = Color.Azure;

                Button btnok = new Button { DialogResult = System.Windows.Forms.DialogResult.OK, Top = 25 };
                Button btncn = new Button { DialogResult = System.Windows.Forms.DialogResult.Cancel, Top = 25 };

                Form frm = new Form { ControlBox = false, AcceptButton = btnok, CancelButton = btncn, StartPosition = FormStartPosition.Manual, Location = ctrlpt };
                frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //######## SetColor to your preference
                frm.BackColor = Color.Black;

                RectangleF rec = new RectangleF(0, 0, (length + border), (20 + border));
                System.Drawing.Drawing2D.GraphicsPath GP = new System.Drawing.Drawing2D.GraphicsPath(); //GetRoundedRect(rec, 4.0F);
                float diameter = 8.0F;
                SizeF sizef = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rec.Location, sizef);
                GP.AddArc(arc, 180, 90);
                arc.X = rec.Right - diameter;
                GP.AddArc(arc, 270, 90);
                arc.Y = rec.Bottom - diameter;
                GP.AddArc(arc, 0, 90);
                arc.X = rec.Left;
                GP.AddArc(arc, 90, 90);
                GP.CloseFigure();

                frm.Region = new Region(GP);
                frm.Controls.AddRange(new Control[] { input, btncn, btnok });
                DialogResult rst = frm.ShowDialog();
                output = input.Text;
                return rst;
        }

        private void UpdateOnlyLabel(int intGCBreakeCount)
        {
            dgvSymbols.Rows.Clear();
            double dblAdding = Math.Pow(0.1, intRounding);
            //arrLabel = new string[intGCBreakeCount];

            for (int j = 0; j < intGCBreakeCount; j++)
            {
                if (j < intGCBreakeCount - 1)
                    dgvSymbols.Rows.Add();

                dgvSymbols.Rows[j].Cells[0].Value = m_previewImages[j];

                string strLabel = "";
                if (j == 0)
                    strLabel = Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString());
                else
                    strLabel = Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString());

                dgvSymbols.Rows[j].Cells[1].Value = strLabel;
                dgvSymbols.Rows[j].Cells[2].Value = arrLabel[j];

            }
            #region For listview
            //lvSymbol.BeginUpdate();
            //lvSymbol.Items.Clear();

            //for (int j = 0; j < intGCBreakeCount; j++)
            //{

            //    ListViewItem lvi = new ListViewItem("");
            //    lvi.UseItemStyleForSubItems = false;
            //    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
            //        "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

            //    if (j == 0)
            //    {
            //        lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
            //        lvi.SubItems.Add(arrLabel[j]);
            //    }
            //    else
            //    {
            //        double dblAdding = Math.Pow(0.1, intRounding);
            //        lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
            //        lvi.SubItems.Add(arrLabel[j]);
            //    }
            //    lvSymbol.Items.Add(lvi);
            //}
            //lvSymbol.EndUpdate();
            #endregion
        }
        private void dgvSymbols_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int intColIdx = e.ColumnIndex;
            int intRowIdx = e.RowIndex;

            if (intColIdx == 0) //Change individual color of symbols
            {
                DialogResult DR = cdColor.ShowDialog();
                if (DR == DialogResult.OK)
                {
                    arrColors[intRowIdx, 0] = cdColor.Color.R;
                    arrColors[intRowIdx, 1] = cdColor.Color.G;
                    arrColors[intRowIdx, 2] = cdColor.Color.B;
                    IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[intRowIdx, 0], arrColors[intRowIdx, 1], arrColors[intRowIdx, 2]);


                    IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();

                    switch (m_fLayer.FeatureClass.ShapeType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            ISimpleMarkerSymbol pSimpleMarkerSym = new SimpleMarkerSymbolClass();

                            pSimpleMarkerSym.Color = (IColor)pRGBColor;
                            pSimpleMarkerSym.OutlineColor = m_pOutColor;
                            pSimpleMarkerSym.OutlineSize = Convert.ToDouble(nudGCLinewidth.Value);
                            pSimpleMarkerSym.Size = Convert.ToDouble(nudChoSymbolSize.Value);
                            styleGalleryItem.Item = (ISymbol)pSimpleMarkerSym;
                            m_previewImages[intRowIdx] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
                             break;
                        case esriGeometryType.esriGeometryPolyline:
                            ISimpleLineSymbol pSimpleLinerSym = new SimpleLineSymbolClass();

                            pSimpleLinerSym.Color = (IColor)pRGBColor;
                            pSimpleLinerSym.Width = Convert.ToDouble(nudGCLinewidth.Value);
                            styleGalleryItem.Item = (ISymbol)pSimpleLinerSym;
                            m_previewImages[intRowIdx] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);
                            break;
                        case esriGeometryType.esriGeometryPolygon:
                            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                            pFillSymbol.Color = (IColor)pRGBColor;
                            pFillSymbol.Outline = m_pOutline;
                            styleGalleryItem.Item = (ISymbol)pFillSymbol;
                            m_previewImages[intRowIdx] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassFillSymbols, styleGalleryItem);
                            break;
                    }
                    UpdateOnlyLabel(intGCBreakeCount);

                }
            }
            else if (intColIdx == 1)
            {
                string var = cb[intRowIdx + 1].ToString();

                int intLength = var.Length * 6 + 30;

                if (PopupInputGridView(dgvSymbols, intRowIdx, intColIdx, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
                {
                    if (cb[intRowIdx + 1] != Convert.ToDouble(var))
                    {
                        cboGCClassify.Text = "Manual";
                        cb[intRowIdx + 1] = Convert.ToDouble(var);
                    }

                    UpdateData(intGCBreakeCount);
                }
            }
            else if (intColIdx == 2)
            {
                string var = arrLabel[intRowIdx];
                int intLength = var.Length * 6 + 30;
                if (PopupInputGridView(dgvSymbols, intRowIdx, intColIdx, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
                {
                    arrLabel[intRowIdx] = var;
                    UpdateOnlyLabel(intGCBreakeCount);
                }
            }
        }

        private DialogResult PopupInputGridView(DataGridView dgvSymbols, int intRowIdx, int intColIdx, int border, int length, ref string output)
        {
            Rectangle pRectangle = dgvSymbols.GetCellDisplayRectangle(intColIdx, intRowIdx, false);
            pRectangle.X += dgvSymbols.Left + dgvSymbols.Columns[intColIdx].Width - length;
            pRectangle.Y += dgvSymbols.Top + Convert.ToInt32(1.5 * dgvSymbols.Rows[intRowIdx].Height);
            //System.Drawing.Point ctrlpt = new System.Drawing.Point();
            //ctrlpt.X = dgvSymbols.Rows[intRowIdx].Cells[intColIdx]..ContentBounds.X;
            //ctrlpt.Y = dgvSymbols.Rows[intRowIdx].Cells[intColIdx].ContentBounds.Y;
            System.Drawing.Point ctrlpt = this.PointToScreen(new System.Drawing.Point(pRectangle.X, pRectangle.Y));
            //ctrlpt = this.PointToScreen(ctrlpt);
            //ctrlpt.Y += 153;
            //ctrlpt.X += 70;
            TextBox input = new TextBox { Height = 20, Width = length, Top = border / 2, Left = border / 2 };
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Text = output;
            //######## SetColor to your preference
            input.BackColor = Color.Azure;

            Button btnok = new Button { DialogResult = System.Windows.Forms.DialogResult.OK, Top = 25 };
            Button btncn = new Button { DialogResult = System.Windows.Forms.DialogResult.Cancel, Top = 25 };

            Form frm = new Form { ControlBox = false, AcceptButton = btnok, CancelButton = btncn, StartPosition = FormStartPosition.Manual, Location = ctrlpt };
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //######## SetColor to your preference
            frm.BackColor = Color.Black;

            RectangleF rec = new RectangleF(0, 0, (length + border), (20 + border));
            System.Drawing.Drawing2D.GraphicsPath GP = new System.Drawing.Drawing2D.GraphicsPath(); //GetRoundedRect(rec, 4.0F);
            float diameter = 8.0F;
            SizeF sizef = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(rec.Location, sizef);
            GP.AddArc(arc, 180, 90);
            arc.X = rec.Right - diameter;
            GP.AddArc(arc, 270, 90);
            arc.Y = rec.Bottom - diameter;
            GP.AddArc(arc, 0, 90);
            arc.X = rec.Left;
            GP.AddArc(arc, 90, 90);
            GP.CloseFigure();

            frm.Region = new Region(GP);
            frm.Controls.AddRange(new Control[] { input, btncn, btnok });
            DialogResult rst = frm.ShowDialog();
            output = input.Text;
            return rst;
        }


        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
                Form form = new Form();
                Label label = new Label();
                TextBox textBox = new TextBox();
                Button buttonOk = new Button();
                Button buttonCancel = new Button();

                form.Text = title;
                label.Text = promptText;
                textBox.Text = value;

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;

                label.SetBounds(9, 20, 372, 13);
                textBox.SetBounds(12, 36, 372, 20);
                buttonOk.SetBounds(228, 72, 75, 23);
                buttonCancel.SetBounds(309, 72, 75, 23);

                label.AutoSize = true;
                textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                form.ClientSize = new Size(396, 107);
                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                DialogResult dialogResult = form.ShowDialog();
                value = textBox.Text;
                return dialogResult;

        }
        private IEnumColors MultiPartColorRamp(string strColorRamp, string strAlgorithm, int intGCBreakeCount)
        {
            try
            {
                IEnumColors pEnumColors = null;

                if (strColorRamp == "Blue Light to Dark" || strColorRamp == "Green Light to Dark" || strColorRamp == "Orange Light to Dark" || strColorRamp == "Red Light to Dark")
                {
                    IAlgorithmicColorRamp pColorRamp = new AlgorithmicColorRampClass();
                    switch (strAlgorithm)
                    {
                        case "HSV":
                            pColorRamp.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
                            break;
                        case "CIE Lab":
                            pColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            break;
                        case "Lab LCh":
                            pColorRamp.Algorithm = esriColorRampAlgorithm.esriLabLChAlgorithm;
                            break;
                        default:
                            pColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            break;
                    }

                    IRgbColor pColor1 = new RgbColor();
                    IRgbColor pColor2 = new RgbColor();

                    switch (strColorRamp)
                    {
                        case "Blue Light to Dark":
                            pColor1 = m_pSnippet.getRGB(239, 243, 255);
                            pColor2 = m_pSnippet.getRGB(8, 81, 156);
                            break;
                        case "Green Light to Dark":
                            pColor1 = m_pSnippet.getRGB(237, 248, 233);
                            pColor2 = m_pSnippet.getRGB(0, 109, 44);
                            break;
                        case "Orange Light to Dark":
                            pColor1 = m_pSnippet.getRGB(254, 237, 222);
                            pColor2 = m_pSnippet.getRGB(166, 54, 3);
                            break;
                        case "Red Light to Dark":
                            pColor1 = m_pSnippet.getRGB(254, 229, 217);
                            pColor2 = m_pSnippet.getRGB(165, 15, 21);
                            break;
                        default:
                            pColor1 = m_pSnippet.getRGB(254, 229, 217);
                            pColor2 = m_pSnippet.getRGB(165, 15, 21);
                            break;
                    }

                    Boolean blnOK = true;

                    pColorRamp.FromColor = pColor1;
                    pColorRamp.ToColor = pColor2;
                    pColorRamp.Size = intGCBreakeCount;
                    pColorRamp.CreateRamp(out blnOK);


                    //arrColors = new int [intGCBreakeCount,3];
                    pEnumColors = pColorRamp.Colors;

                }
                else if (strColorRamp == "Custom")
                {
                    Boolean blnOK = true;
                    frmColorRamps m_pColorRamps = System.Windows.Forms.Application.OpenForms["frmColorRamps"] as frmColorRamps;
                    IMultiPartColorRamp pMultiColorRamp = m_pColorRamps.pMulitColorRampsResults;

                    pMultiColorRamp.Size = intGCBreakeCount;
                    pMultiColorRamp.CreateRamp(out blnOK);


                    pEnumColors = pMultiColorRamp.Colors;
                }
                else
                {
                    IAlgorithmicColorRamp pColorRamp1 = new AlgorithmicColorRampClass();
                    IAlgorithmicColorRamp pColorRamp2 = new AlgorithmicColorRampClass();


                    switch (strAlgorithm)
                    {
                        case "HSV":
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
                            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
                            break;
                        case "CIE Lab":
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            break;
                        case "Lab LCh":
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriLabLChAlgorithm;
                            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriLabLChAlgorithm;
                            break;
                        default:
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            break;
                    }

                    IRgbColor pColor1 = new RgbColor();
                    IRgbColor pColor2 = new RgbColor();
                    IRgbColor pColor3 = new RgbColor();

                    switch (strColorRamp)
                    {
                        case "Blue to Red":
                            pColor1 = m_pSnippet.getRGB(49, 54, 149);
                            pColor2 = m_pSnippet.getRGB(255, 255, 191);
                            pColor3 = m_pSnippet.getRGB(165, 0, 38);
                            break;
                        case "Green to Purple":
                            pColor1 = m_pSnippet.getRGB(0, 68, 27);
                            pColor2 = m_pSnippet.getRGB(247, 247, 247);
                            pColor3 = m_pSnippet.getRGB(64, 0, 75);
                            break;
                        case "Green to Red":
                            pColor1 = m_pSnippet.getRGB(0, 104, 55);
                            pColor2 = m_pSnippet.getRGB(255, 255, 191);
                            pColor3 = m_pSnippet.getRGB(165, 0, 38);
                            break;
                        case "Purple to Brown":
                            pColor1 = m_pSnippet.getRGB(45, 0, 75);
                            pColor2 = m_pSnippet.getRGB(247, 247, 247);
                            pColor3 = m_pSnippet.getRGB(127, 59, 8);
                            break;
                        default:
                            pColor1 = m_pSnippet.getRGB(49, 54, 149);
                            pColor2 = m_pSnippet.getRGB(255, 255, 191);
                            pColor3 = m_pSnippet.getRGB(165, 0, 38);
                            break;
                    }


                    pColorRamp1.FromColor = pColor1;
                    pColorRamp1.ToColor = pColor2;
                    pColorRamp2.FromColor = pColor2;
                    pColorRamp2.ToColor = pColor3;

                    Boolean blnOK = true;

                    IMultiPartColorRamp pMultiColorRamp = new MultiPartColorRampClass();
                    pMultiColorRamp.Ramp[0] = pColorRamp1;
                    pMultiColorRamp.Ramp[1] = pColorRamp2;
                    pMultiColorRamp.Size = intGCBreakeCount;
                    pMultiColorRamp.CreateRamp(out blnOK);


                    pEnumColors = pMultiColorRamp.Colors;
                }
                pEnumColors.Reset();
                return pEnumColors;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
            }
        }

        private void cboColorRamp_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cboColorRamp.Text == "Custom")
            {

                if (cboValueField.Text == "")
                {
                    MessageBox.Show("Please select value field");
                    cboColorRamp.Text = "Red Light to Dark";
                    return;
                }
                else
                {
                    frmColorRamps pColorRamps = new frmColorRamps();
                    pColorRamps.intLoadingPlaces = 1;
                    pColorRamps.Show();
                }
            }
            else
            {
                //DrawSymbolinListView();
                DrawSymboliinDataGridView();
            }
        }

        private double[] ClassSeparabilityMeasure(int intNClasses, int intNFeatures, IFeatureCursor pFCursor, int intEstIdx, int intVarIdx)
        {
            double[] Cs = new double[intNClasses+1];

            double[] arrEst = new double[intNFeatures];
            double[] arrVar = new double[intNFeatures];
            double[] arrResults = new double[intNFeatures-1];
            double[] arrSortedResult = new double[intNFeatures-1];

            IFeature pFeature = pFCursor.NextFeature();
            int k = 0;
            while (pFeature != null)
            {
                arrEst[k] = Convert.ToDouble(pFeature.get_Value(intEstIdx));
                arrVar[k] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                k++;
                pFeature = pFCursor.NextFeature();
            }

            for (int i = 0; i < intNFeatures - 2; i++)
            {
                double dblCLMin = double.MaxValue; // needs to be changed to p-value 09.25.15 HK
                double dblCL = 0;
                for (int m = i; m >= 0; m--)
                {
                    for (int n = (i + 1); n < intNFeatures; n++)
                    {
                        dblCL = Math.Abs(arrEst[n] - arrEst[m]) / (Math.Sqrt(Math.Pow(arrVar[n], 2) + Math.Pow(arrVar[m], 2)));
                        if (dblCL < dblCLMin)
                            dblCLMin = dblCL;
                    }
                }
                arrResults[i] = dblCLMin;
                arrSortedResult[i] = dblCLMin;
            }

            System.Array.Sort<double>(arrSortedResult, new Comparison<double>(
                            (i1, i2) => i2.CompareTo(i1)
                    ));

            Chart pChart = new Chart();
            
            double[] dblpValue = new double[intNClasses - 1];
            int[] intResultIdx = new int[intNClasses-1];
            for (int i = 0; i < intNClasses-1; i++)
            {
                intResultIdx[i] = System.Array.IndexOf(arrResults, arrSortedResult[i]);
                dblpValue[i] = pChart.DataManipulator.Statistics.NormalDistribution(arrSortedResult[i]);
            }
            System.Array.Sort(intResultIdx);

            Cs[0] = arrEst.Min();
            Cs[intNClasses] = arrEst.Max();
            for (int i = 0; i < intNClasses-1; i++)
            {
                Cs[i + 1] = arrEst[intResultIdx[i]];
            }

            return Cs;

        }

        private void cboUncernFld_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView();
            DrawSymboliinDataGridView();
        }

        private void cboColorRamp_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Override this function to draw items in the Color comboBox

            // Get the Graphics Object (aka. CDC or Device Context Object )
            // passed via the DrawItemEventArgs parameter
            Graphics g = e.Graphics;

            // Get the bounding rectangle of the item currently being painted
            Rectangle r = e.Bounds;

            if (e.Index >= 0)
            {
                // Set the string format options
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;

                //if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                //{
                //    // if the item is not selected draw it with a different color
                //    e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                //    e.Graphics.DrawString(m_colorRampNames[e.Index], new Font("Ariel", 8, FontStyle.Regular), new SolidBrush(Color.Black), r, sf);
                //    e.DrawFocusRectangle();
                //}
                //else
                //{
                    List<Color> pColors = m_colorLists[e.Index];
                    int intColorCnt = pColors.Count;

                    int intwidth = r.Width / 2;
                    int intheight = r.Height;
                    int intX = r.X;
                    int inty = r.Y;

                    if (intColorCnt == 2)
                    {
                        LinearGradientBrush b = new LinearGradientBrush(r, pColors[0], pColors[1], 0F);
                        Rectangle Rec = new Rectangle(intX, inty, intwidth * 2, intheight); //Keep the width of bound same.
                        e.Graphics.FillRectangle(b, Rec);
                    }
                    else if (intColorCnt == 3)
                    {
                        Rectangle leftRec = new Rectangle(intX, inty, intwidth, intheight);
                        Rectangle rightRec = new Rectangle(intwidth + intX, inty, intwidth, intheight);

                        LinearGradientBrush b1 = new LinearGradientBrush(leftRec, pColors[0], pColors[1], 0F);
                        LinearGradientBrush b2 = new LinearGradientBrush(rightRec, pColors[1], pColors[2], 0F);
                        e.Graphics.FillRectangle(b1, leftRec);
                        e.Graphics.FillRectangle(b2, rightRec);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                        e.Graphics.DrawString(m_colorRampNames[e.Index], new Font("Ariel", 8, FontStyle.Regular), new SolidBrush(Color.Black), r, sf);
                        e.DrawFocusRectangle();
                    }
                    // if the item is selected draw it with a different color
                    
                    //e.Graphics.DrawString(m_colorRampNames[e.Index], new Font("Veranda", 12, FontStyle.Bold), new SolidBrush(Color.Red), r, sf);
                    e.DrawFocusRectangle();
                //}
            }
        }

        #endregion

        #region Proportional mapping functions
        private void nudProSize_ValueChanged(object sender, EventArgs e)
        {
            CalculateMaxSymbolSizeforProportionalSymbol(cboProValue.Text);
            DisplayProportionalSymbols();
        }

        private void btnProCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cboProValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateMaxSymbolSizeforProportionalSymbol(cboProValue.Text);
            DisplayProportionalSymbols();
        }
        private void nudProOutWidth_ValueChanged(object sender, EventArgs e)
        {
            CalculateMaxSymbolSizeforProportionalSymbol(cboProValue.Text);
            DisplayProportionalSymbols();
        }

        private void chkProOutline_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProOutline.Checked)
            {
                lblProOutColor.Enabled = true; picProOutColor.Enabled = true;
                lblProOutWidth.Enabled = true; nudProOutWidth.Enabled = true;
            }
            else
            {
                lblProOutColor.Enabled = false; picProOutColor.Enabled = false;
                lblProOutWidth.Enabled = false; nudProOutWidth.Enabled = false;
            }

            DisplayProportionalSymbols();
        }

        private void chkFlannery_CheckedChanged(object sender, EventArgs e)
        {
            CalculateMaxSymbolSizeforProportionalSymbol(cboProValue.Text);
            DisplayProportionalSymbols();
        }

        private void picProColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picProColor.BackColor = cdColor.Color;

            DisplayProportionalSymbols();
        }

        private void picProOutColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picProOutColor.BackColor = cdColor.Color;

            DisplayProportionalSymbols();
        }
        private void PreviewProportionalImage(esriSymbologyStyleClass styleClass, IStyleGalleryItem styleGalleryItem, bool blnMin)
        {
            //Get and set the style class 
            axSymbologyControl1.StyleClass = styleClass;
            ISymbologyStyleClass symbologyStyleClass = axSymbologyControl1.GetStyleClass(styleClass);
            if (blnMin)
            {
                //Preview an image of the symbol
                stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(styleGalleryItem, picProMinSymbol.Width, picProMinSymbol.Height);
                System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                picProMinSymbol.Image = image;
            }
            else
            {
                //Preview an image of the symbol
                stdole.IPictureDisp picture = symbologyStyleClass.PreviewItem(styleGalleryItem, picProMaxSymbol.Width, picProMaxSymbol.Height);
                System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                picProMaxSymbol.Image = image;
            }
        }

        private void CalculateMaxSymbolSizeforProportionalSymbol(string strPropRenderFld)
        {
            if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon || m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                m_dblMinSymbolSize = Convert.ToDouble(nudProSize.Value);
            else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                m_dblMinSymbolSize = Convert.ToDouble(nudProOutWidth.Value);


            if (m_dblMinSymbolSize == 0)
            {
                MessageBox.Show("Please select a proper value for the minimum symbol size");
                return;
            }
            int intValueFldIdx = m_pFClass.FindField(strPropRenderFld);

            if (intValueFldIdx == -1)
                return;
            ITable pTable = (ITable)m_pFClass;

            ICursor pCursor = pTable.Search(null, true);
            IDataStatistics pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pCursor;
            pDataStatistics.Field = strPropRenderFld;
            IStatisticsResults pStatisticsResult = pDataStatistics.Statistics;
            m_dblMinDataValue = pStatisticsResult.Minimum;
            m_dblMaxDataValue = pStatisticsResult.Maximum;

            if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon || m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                if (chkFlannery.Checked)
                    m_dblMaxSymbolSize = 1.0083 * Math.Pow(m_dblMaxDataValue / m_dblMinDataValue, 0.5716) * m_dblMinSymbolSize;
                else
                    m_dblMaxSymbolSize = Math.Sqrt(m_dblMaxDataValue / m_dblMinDataValue) * m_dblMinSymbolSize;

            }
            else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                m_dblMaxSymbolSize = (m_dblMaxDataValue / m_dblMinDataValue) * m_dblMinSymbolSize;

        }
        private void DisplayProportionalSymbols()
        {
            try
            {

                string strFld = cboProValue.Text;
                if (strFld == "") return;

                IRgbColor pRgbColor = new RgbColor();
                pRgbColor = m_pSnippet.getRGB(picProColor.BackColor.R, picProColor.BackColor.G, picProColor.BackColor.B);

                IRgbColor pOutColor = new RgbColor();
                pOutColor = m_pSnippet.getRGB(picProOutColor.BackColor.R, picProOutColor.BackColor.G, picProOutColor.BackColor.B);

                double dblOutlineWidth = Convert.ToDouble(nudProOutWidth.Value);

                IStyleGalleryItem pMinstyleGalleryItem = new ServerStyleGalleryItem();
                IStyleGalleryItem pMaxstyleGalleryItem = new ServerStyleGalleryItem();

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                    case esriGeometryType.esriGeometryPolygon:
                        m_pMinMarkerSymbol = new SimpleMarkerSymbolClass();
                        m_pMinMarkerSymbol.Color = (IColor)pRgbColor;
                        m_pMinMarkerSymbol.Size = m_dblMinSymbolSize;
                        m_pMinMarkerSymbol.Outline = chkProOutline.Checked;

                        if (chkProOutline.Checked)
                        {
                            m_pMinMarkerSymbol.OutlineColor = (IColor)pOutColor;
                            m_pMinMarkerSymbol.OutlineSize = dblOutlineWidth;
                        }
                        else
                            m_pMinMarkerSymbol.Outline = false;
                        ISimpleMarkerSymbol pMaxMarkerSymbol = new SimpleMarkerSymbolClass();
                        pMaxMarkerSymbol.Color = (IColor)pRgbColor;
                        pMaxMarkerSymbol.Size = m_dblMaxSymbolSize;
                        pMaxMarkerSymbol.Outline = chkProOutline.Checked;

                        if (chkProOutline.Checked)
                        {
                            pMaxMarkerSymbol.OutlineColor = (IColor)pOutColor;
                            pMaxMarkerSymbol.OutlineSize = dblOutlineWidth;
                        }
                        else
                            pMaxMarkerSymbol.Outline = false;

                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, (ISymbol)pMinMarkerSymbol);
                        pMinstyleGalleryItem.Item = (ISymbol)m_pMinMarkerSymbol;
                        PreviewProportionalImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, pMinstyleGalleryItem, true);

                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, (ISymbol)pMaxMarkerSymbol);
                        pMaxstyleGalleryItem.Item = (ISymbol)pMaxMarkerSymbol;
                        PreviewProportionalImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, pMaxstyleGalleryItem, false);

                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        m_pMinLineSymbol = new SimpleLineSymbolClass();
                        m_pMinLineSymbol.Color = (IColor)pRgbColor;
                        m_pMinLineSymbol.Width = m_dblMinSymbolSize;

                        ISimpleLineSymbol pMaxLineSymbol = new SimpleLineSymbolClass();
                        pMaxLineSymbol.Color = (IColor)pRgbColor;
                        pMaxLineSymbol.Width = m_dblMaxSymbolSize;

                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, (ISymbol)pMinMarkerSymbol);
                        pMinstyleGalleryItem.Item = (ISymbol)m_pMinLineSymbol;
                        PreviewProportionalImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, pMinstyleGalleryItem, true);

                        //styleGalleryItem = GetGalleryItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, (ISymbol)pMaxMarkerSymbol);
                        pMaxstyleGalleryItem.Item = (ISymbol)pMaxLineSymbol;
                        PreviewProportionalImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, pMaxstyleGalleryItem, false);
                        break;
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void btnProApply_Click(object sender, EventArgs e)
        {
            try
            {

                if (cboProValue.Text == "")
                    return;


                IProportionalSymbolRenderer pProSymbRenerer = new ProportionalSymbolRendererClass();
                pProSymbRenerer.ValueUnit = esriUnits.esriUnknownUnits;
                pProSymbRenerer.Field = cboProValue.Text;
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint || m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    pProSymbRenerer.MinSymbol = (ISymbol)m_pMinMarkerSymbol;
                    pProSymbRenerer.FlanneryCompensation = chkFlannery.Checked;
                    if (m_BackSymbol != null)
                        pProSymbRenerer.BackgroundSymbol = (IFillSymbol)m_BackSymbol;
                }
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    pProSymbRenerer.MinSymbol = (ISymbol)m_pMinLineSymbol;
                else
                {
                    MessageBox.Show("Shape type is invalid for this process.");
                    return;
                }


                pProSymbRenerer.MinDataValue = m_dblMinDataValue;
                pProSymbRenerer.MaxDataValue = m_dblMaxDataValue;

                pProSymbRenerer.LegendSymbolCount = Convert.ToInt32(nudLegendCount.Value);
                pProSymbRenerer.CreateLegendSymbols();
                IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)m_fLayer;
                pGeofeatureLayer.Renderer = (IFeatureRenderer)pProSymbRenerer;

                mForm.axTOCControl1.Update();
                mForm.axMapControl1.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void btnProBackSymbol_Click(object sender, EventArgs e)
        {
            frmSimpleSymbol pfrmSimpleSymbol = new frmSimpleSymbol();
            pfrmSimpleSymbol.Text = "Background";
            pfrmSimpleSymbol.ShowDialog();
        }


        #endregion

        #region GC with Listview
        //public void DrawSymbolinListView()
        //{
        //    //try
        //    //{
        //        lvSymbol.Items.Clear();
        //        string strGCRenderField = cboValueField.Text;
        //        string strUncernfld = cboUncernFld.Text;
        //        intGCBreakeCount = Convert.ToInt32(nudGCNClasses.Value);

        //        int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
        //        int intUncernfldIdx = m_pFClass.FindField(strUncernfld);

        //        if (intUncernfldIdx == -1 && cboGCClassify.Text == "Class Separability")
        //            return;

        //        ITable pTable = (ITable)m_pFClass;
        //        IClassifyGEN pClassifyGEN = null;
        //        if (cboGCClassify.Text != "Class Separability")
        //        {
        //            switch (cboGCClassify.Text)
        //            {
        //                case "Equal Interval":
        //                    pClassifyGEN = new EqualIntervalClass();
        //                    break;
        //                case "Geometrical Interval":
        //                    pClassifyGEN = new GeometricalInterval();
        //                    break;
        //                case "Natural Breaks":
        //                    pClassifyGEN = new NaturalBreaksClass();
        //                    break;
        //                case "Quantile":
        //                    pClassifyGEN = new QuantileClass();
        //                    break;
        //                case "StandardDeviation":
        //                    pClassifyGEN = new StandardDeviationClass();
        //                    break;
        //                default:
        //                    pClassifyGEN = new NaturalBreaksClass();
        //                    break;
        //            }

        //            strClassificationMethod = cboGCClassify.Text;


        //            //ITableHistogram pTableHistogram = new TableHistogramClass();
        //        ITableHistogram pTableHistogram2 = new BasicTableHistogramClass();

        //        pTableHistogram2.Field = strGCRenderField;
        //        pTableHistogram2.Table = pTable;
        //            //IHistogram pHistogram = (IHistogram)pTableHistogram2;
        //        IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram2;
        //        object xVals, frqs;
        //            pHistogram.GetHistogram(out xVals, out frqs);
        //            pClassifyGEN.Classify(xVals, frqs, intGCBreakeCount);
        //            cb = (double[])pClassifyGEN.ClassBreaks;
        //        }
        //        else
        //        {
        //            ITableSort pTableSort = new TableSort();
        //            pTableSort.Table = pTable;
        //            ICursor pCursor = (ICursor)m_pFClass.Search(null, false);

        //            pTableSort.Cursor = pCursor as ICursor;

        //            ////set up the query filter.
        //            //IQueryFilter pQF = null;
        //            //pQF = new QueryFilter();
        //            //pQF.SubFields = "*";
        //            //pQF.WhereClause = m_pQueryFilter.WhereClause;
        //            //pTableSort.QueryFilter = pQF;

        //            pTableSort.Fields = strGCRenderField;
        //            pTableSort.set_Ascending(strGCRenderField, true);

        //            // call the sort
        //            pTableSort.Sort(null);

        //            // retrieve the sorted rows
        //            IFeatureCursor pSortedCursor = pTableSort.Rows as IFeatureCursor;

        //            cb = ClassSeparabilityMeasure(intGCBreakeCount, intNofFeatures, pSortedCursor, intValueFldIdx, intUncernfldIdx);
        //        }

        //        m_pRender = new ClassBreaksRenderer();

        //        m_pRender.Field = strGCRenderField;
        //        m_pRender.BreakCount = intGCBreakeCount;
        //        m_pRender.MinimumBreak = cb[0];

        //        string strColorRamp = cboColorRamp.Text;

        //        pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, intGCBreakeCount);
        //        pEnumColors.Reset();

        //        arrColors = new int[intGCBreakeCount, 3];

        //    for (int k = 0; k < intGCBreakeCount; k++)
        //        {
        //            IColor pColor = pEnumColors.Next();
        //            IRgbColor pRGBColor = new RgbColorClass();
        //            pRGBColor.RGB = pColor.RGB;



        //        arrColors[k, 0] = pRGBColor.Red;
        //            arrColors[k, 1] = pRGBColor.Green;
        //            arrColors[k, 2] = pRGBColor.Blue;
        //    }


        //    pEnumColors.Reset();

        //        UpdateRange(lvSymbol, intGCBreakeCount);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //    //    return;
        //    //}
        //}
        //private void UpdateRange(ListView lvSymbol, int intGCBreakeCount)
        //{
        //    try
        //    {
        //        arrLabel = new string[intGCBreakeCount];

        //        lvSymbol.BeginUpdate();
        //        lvSymbol.Items.Clear();
        //        pEnumColors.Reset();


        //        for (int j = 0; j < intGCBreakeCount; j++)
        //        {

        //            ListViewItem lvi = new ListViewItem("");
        //            lvi.UseItemStyleForSubItems = false;
        //            lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
        //                "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

        //            lvi.ImageIndex = j;

        //            if (j == 0)
        //            {
        //                lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
        //                lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString()));
        //                arrLabel[j] = Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString());
        //            }
        //            else
        //            {

        //                double dblAdding = Math.Pow(0.1, intRounding);
        //                lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
        //                lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString()));
        //                arrLabel[j] = Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString());
        //            }
        //            lvSymbol.Items.Add(lvi);
        //        }
        //        lvSymbol.EndUpdate();
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        //private void lvSymbol_MouseUp(object sender, MouseEventArgs e)
        //{
        //    try
        //    {
        //        ListViewItem.ListViewSubItem pSelectedSubItems = null;
        //        //selection = lvSymbol.GetItemAt(e.X, e.Y);
        //        for (int i = 0; i < intGCBreakeCount; i++)
        //        {
        //            for (int j = 0; j < 4; j++)
        //            {
        //                if (lvSymbol.Items[i].SubItems[j].Bounds.Contains(e.Location))
        //                {
        //                    pSelectedSubItems = lvSymbol.Items[i].SubItems[j];
        //                    if (j == 1)
        //                    {
        //                        DialogResult DR = cdColor.ShowDialog();
        //                        if (DR == DialogResult.OK)
        //                        {
        //                            pSelectedSubItems.BackColor = cdColor.Color;
        //                            arrColors[i, 0] = pSelectedSubItems.BackColor.R;
        //                            arrColors[i, 1] = pSelectedSubItems.BackColor.G;
        //                            arrColors[i, 2] = pSelectedSubItems.BackColor.B;
        //                        }

        //                    }
        //                    else if (j == 2)
        //                    {
        //                        string var = cb[i + 1].ToString();

        //                        int intLength = var.Length * 6 + 30;

        //                        if (PopupInput(pSelectedSubItems, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
        //                        {
        //                            if (cb[i + 1] != Convert.ToDouble(var))
        //                            {
        //                                cboGCClassify.Text = "Manual";
        //                                cb[i + 1] = Convert.ToDouble(var);
        //                            }

        //                            UpdateRange(lvSymbol, intGCBreakeCount);
        //                        }
        //                    }
        //                    else if (j == 3)
        //                    {
        //                        string var = arrLabel[i];
        //                        int intLength = var.Length * 6 + 30;
        //                        if (PopupInput(pSelectedSubItems, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
        //                        {
        //                            arrLabel[i] = var;
        //                            UpdateOnlyLabel(lvSymbol, intGCBreakeCount);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        lvSymbol.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        //private void lvSymbol_ColumnClick(object sender, ColumnClickEventArgs e)
        //{
        //    try
        //    {
        //        string value = intRounding.ToString();
        //        if (InputBox("Decimal Places", "Number of decimal places:", ref value) == DialogResult.OK)
        //        {
        //            intRounding = Convert.ToInt32(value);
        //            UpdateRange(lvSymbol, intGCBreakeCount);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        #endregion

        #region Graduated Symbols mapping functions
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGSSymColor.BackColor = cdColor.Color;

            DrawGSSymboliinDataGridView();
        }


        private void cboGSvaluefld_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawGSSymboliinDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboGSClassificationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawGSSymboliinDataGridView();
        }

        private void nudGSNClasses_ValueChanged(object sender, EventArgs e)
        {
            DrawGSSymboliinDataGridView();
        }

        private void nudGSMinSymSz_ValueChanged(object sender, EventArgs e)
        {
            DrawGSSymboliinDataGridView();
        }

        private void nudGSMaxSymSz_ValueChanged(object sender, EventArgs e)
        {
            DrawGSSymboliinDataGridView();
        }

        private void nudGSOutWidth_ValueChanged(object sender, EventArgs e)
        {
            DrawGSSymboliinDataGridView();
        }

        private void picGSOutColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGSOutColor.BackColor = cdColor.Color;

            DrawGSSymboliinDataGridView();
        }

        private void btnGSApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (cb == null)
                    return;
                if (cboGSvaluefld.Text == "")
                    return;

                m_pSnippet.AddRenderInfo(mForm, m_fLayer, cboGSvaluefld.Text, string.Empty, cb, null, m_strClassificationMethod);
                double dblGCOutlineSize = Convert.ToDouble(nudGSOutWidth.Value);

                //' use this interface to set dialog properties
                IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)m_pRender;
                pUIProperties.ColorRamp = "Custom";
                if (m_BackSymbol != null)
                    m_pRender.BackgroundSymbol = (IFillSymbol)m_BackSymbol;

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleMarkerSymbol pSimpleMarkerSym;

                        //' be careful, indices are different for the diff lists
                        for (int j = 0; j < intGCBreakeCount; j++)
                        {
                            m_pRender.Break[j] = cb[j + 1];
                            m_pRender.Label[j] = arrLabel[j];

                            pUIProperties.LowBreak[j] = cb[j];
                            pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                            pSimpleMarkerSym.Color = (IColor)m_pSymColor;
                            pSimpleMarkerSym.Outline = true;
                            pSimpleMarkerSym.OutlineSize = dblGCOutlineSize;
                            pSimpleMarkerSym.Size = m_arrSizes[j];
                            pSimpleMarkerSym.OutlineColor = (IColor)m_pOutColor;
                            m_pRender.Symbol[j] = (ISymbol)pSimpleMarkerSym;
                        }

                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pSimpleLinerSym;


                        for (int j = 0; j < intGCBreakeCount; j++)
                        {
                            m_pRender.Break[j] = cb[j + 1];
                            m_pRender.Label[j] = arrLabel[j];
                            pUIProperties.LowBreak[j] = cb[j];
                            pSimpleLinerSym = new SimpleLineSymbolClass();
                            pSimpleLinerSym.Color = (IColor)m_pSymColor;
                            pSimpleLinerSym.Width = m_arrSizes[j];
                            m_pRender.Symbol[j] = (ISymbol)pSimpleLinerSym;
                        }
                        break;
                }


                IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)m_fLayer;

                pGeofeatureLayer.Renderer = (IFeatureRenderer)m_pRender;

                mForm.axMapControl1.ActiveView.Refresh();
                mForm.axTOCControl1.Update();
                //mForm.axTOCControl1.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnGSBackSymbol_Click(object sender, EventArgs e)
        {
            frmSimpleSymbol pfrmSimpleSymbol = new frmSimpleSymbol();
            pfrmSimpleSymbol.Text = "Background";
            pfrmSimpleSymbol.ShowDialog();
        }

        private void dgvGSSymbol_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int intColIdx = e.ColumnIndex;
            int intRowIdx = e.RowIndex;

            
            if (intColIdx == 1)
            {
                string var = cb[intRowIdx + 1].ToString();

                int intLength = var.Length * 6 + 30;

                if (PopupInputGridView(dgvGSSymbol, intRowIdx, intColIdx, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
                {
                    if (cb[intRowIdx + 1] != Convert.ToDouble(var))
                    {
                        cboGSClassificationMethod.Text = "Manual";
                        cb[intRowIdx + 1] = Convert.ToDouble(var);
                    }

                    UpdateGSData(intGCBreakeCount);
                }
            }
            else if (intColIdx == 2)
            {
                string var = arrLabel[intRowIdx];
                int intLength = var.Length * 6 + 30;
                if (PopupInputGridView(dgvGSSymbol, intRowIdx, intColIdx, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
                {
                    arrLabel[intRowIdx] = var;
                    UpdateGSData(intGCBreakeCount);
                }
            }
        }

        private void dgvGSSymbol_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                {
                    string value = intRounding.ToString();
                    if (InputBox("Decimal Places", "Number of decimal places:", ref value) == DialogResult.OK)
                    {
                        intRounding = Convert.ToInt32(value);
                        UpdateGSData(intGCBreakeCount);
                        //UpdateRange(lvSymbol, intGCBreakeCount);
                    }
                }

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        //Error control to deal with binary data (1.0.6 Update)
        public int GetUniqueCnt(string strFldName)
        {
            ITable pTable = (ITable)m_pFClass;

            ICursor pCursor = pTable.Search(null, true);
            int InitialNumber = 0;


            int intFldIdx = m_pFClass.Fields.FindField(strFldName);

            List<object> uvList = new List<object>();
            IRow ipRow = pCursor.NextRow();
            while (ipRow != null)
            {
                object curValue = ipRow.get_Value(intFldIdx);

                if (!uvList.Contains(curValue))
                {
                    uvList.Add(curValue);
                }

                ipRow = pCursor.NextRow();
            }
            int intUniqueCnt = uvList.Count;

            //Not working
            //DataStatistics ipDataStat = new DataStatisticsClass();
            //ipDataStat.Field = strFldName;
            //ipDataStat.Cursor = pCursor;
            //System.Collections.IEnumerator ipEnum = ipDataStat.UniqueValues;
            
            //InitialNumber = ipDataStat.UniqueValueCount;
            
            if (intUniqueCnt <= 1)
            {
                MessageBox.Show("Fail to create classes. This field contains only a unique value.");
                InitialNumber = 1;
                
            }

            return intUniqueCnt;
            
        }

        public void DrawGSSymboliinDataGridView()
        {
            try
            {
                string strGSRenderField = cboGSvaluefld.Text;


                //Error control to deal with binary data (1.0.6 Update)

                int intUniqueCnt = GetUniqueCnt(strGSRenderField);
                if (intUniqueCnt < Convert.ToInt32(nudGSNClasses.Value))
                {
                    nudGSNClasses.Value = intUniqueCnt;
                    //return;
                }

                intGCBreakeCount = Convert.ToInt32(nudGSNClasses.Value);

                double dblOutlineWidth = Convert.ToDouble(nudGSOutWidth.Value);
                double dblGSSymbolMinSize = Convert.ToDouble(nudGSMinSymSz.Value);
                double dblGSSymbolMaxSize = Convert.ToDouble(nudGSMaxSymSz.Value);
                double dblIncrement = 0;
                if (dblGSSymbolMaxSize <= dblGSSymbolMinSize)
                {
                    MessageBox.Show("Please input a proper range of symbol sizes.");
                    return;
                }
                else
                    dblIncrement = (dblGSSymbolMaxSize - dblGSSymbolMinSize) / Convert.ToDouble(intGCBreakeCount - 1);

                m_pOutColor = m_pSnippet.getRGB(picGSOutColor.BackColor.R, picGSOutColor.BackColor.G, picGSOutColor.BackColor.B);
                m_pSymColor = m_pSnippet.getRGB(picGSSymColor.BackColor.R, picGSSymColor.BackColor.G, picGSSymColor.BackColor.B);

                int intValueFldIdx = m_pFClass.FindField(strGSRenderField);

                ITable pTable = (ITable)m_pFClass;
                IClassifyGEN pClassifyGEN = null;

                m_strClassificationMethod = cboGSClassificationMethod.Text;

                switch (m_strClassificationMethod)
                {
                    case "Equal Interval":
                        pClassifyGEN = new EqualIntervalClass();
                        break;
                    case "Geometrical Interval":
                        pClassifyGEN = new GeometricalInterval();
                        break;
                    case "Natural Breaks":
                        pClassifyGEN = new NaturalBreaksClass();
                        break;
                    case "Quantile":
                        pClassifyGEN = new QuantileClass();
                        break;
                    case "StandardDeviation":
                        pClassifyGEN = new StandardDeviationClass();
                        break;
                    default:
                        pClassifyGEN = new NaturalBreaksClass();
                        break;
                }

                //ITableHistogram pTableHistogram = new TableHistogramClass();
                ITableHistogram pTableHistogram2 = new BasicTableHistogramClass();

                pTableHistogram2.Field = strGSRenderField;
                pTableHistogram2.Table = pTable;
                //IHistogram pHistogram = (IHistogram)pTableHistogram2;
                IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram2;
                object xVals, frqs;
                pHistogram.GetHistogram(out xVals, out frqs);
                pClassifyGEN.Classify(xVals, frqs, intGCBreakeCount);
                cb = (double[])pClassifyGEN.ClassBreaks;

                m_pRender = new ClassBreaksRenderer();

                m_pRender.Field = strGSRenderField;
                m_pRender.BreakCount = intGCBreakeCount;
                m_pRender.MinimumBreak = cb[0];

                m_arrSizes = new double[intGCBreakeCount];

                IStyleGalleryItem styleGalleryItem = new ServerStyleGalleryItem();

                switch (m_fLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPoint:
                    case esriGeometryType.esriGeometryPolygon:
                        ISimpleMarkerSymbol pSimpleMarkerSym = new SimpleMarkerSymbolClass();

                        m_previewImages = new Image[intGCBreakeCount];

                        for (int k = 0; k < intGCBreakeCount; k++)
                        {
                            pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                            styleGalleryItem = new ServerStyleGalleryItem();

                            pSimpleMarkerSym.Color = (IColor)m_pSymColor;
                            pSimpleMarkerSym.OutlineColor = (IColor)m_pOutColor;
                            pSimpleMarkerSym.OutlineSize = dblOutlineWidth;
                            m_arrSizes[k] = dblGSSymbolMinSize + dblIncrement * Convert.ToDouble(k);
                            pSimpleMarkerSym.Size = m_arrSizes[k];
                            pSimpleMarkerSym.Outline = true;
                            styleGalleryItem.Item = (ISymbol)pSimpleMarkerSym;
                            m_previewImages[k] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassMarkerSymbols, styleGalleryItem);
                        }
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pSimpleLinerSym = new SimpleLineSymbolClass();

                        m_previewImages = new Image[intGCBreakeCount];

                        for (int k = 0; k < intGCBreakeCount; k++)
                        {
                            pSimpleLinerSym = new SimpleLineSymbolClass();
                            styleGalleryItem = new ServerStyleGalleryItem();

                            pSimpleLinerSym.Color = (IColor)m_pSymColor;
                            m_arrSizes[k] = dblGSSymbolMinSize + dblIncrement * Convert.ToDouble(k);
                            pSimpleLinerSym.Width = m_arrSizes[k];

                            styleGalleryItem.Item = (ISymbol)pSimpleLinerSym;
                            m_previewImages[k] = GetPreviewImage(esriSymbologyStyleClass.esriStyleClassLineSymbols, styleGalleryItem);

                        }
                        break;
                }
                UpdateGSData(intGCBreakeCount);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void UpdateGSData(int intGCBreakeCount)
        {
            dgvGSSymbol.Rows.Clear();
            double dblAdding = Math.Pow(0.1, intRounding);
            arrLabel = new string[intGCBreakeCount];

            for (int j = 0; j < intGCBreakeCount; j++)
            {
                if (j < intGCBreakeCount - 1)
                    dgvGSSymbol.Rows.Add();

                dgvGSSymbol.Rows[j].Cells[0].Value = m_previewImages[j];

                string strLabel = "";
                if (j == 0)
                    strLabel = Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString());
                else
                    strLabel = Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString());

                dgvGSSymbol.Rows[j].Cells[1].Value = strLabel;
                dgvGSSymbol.Rows[j].Cells[2].Value = strLabel;
                arrLabel[j] = strLabel;

            }
        }

        #endregion
    }
}
