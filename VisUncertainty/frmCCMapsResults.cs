using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Geometry;
using System.Drawing.Drawing2D;


namespace VisUncertainty
{
    public partial class frmCCMapsResults : Form
    {
        //Public Variables
        public string strVarFldName;
        public string strVerConFldName;
        public string strHorConFldName;

        public int intHorCnt = 3; //Default value for avoiding zero-dividing exception
        public int intVerCnt = 3; //Default value for avoiding zero-dividing exception

        public IFeatureLayer pFLayer;

        public bool blnBrushingFromMapControl = false;
        //Private variables
        public AxMapControl[] m_axMapControls;
        private TextBox[] HoriTxtBoxes;
        private TextBox[] VertTxtBoxes;
        private Label[] FeatureCountLabels;
        private int m_intMapCnt;
        private clsSnippet m_pSnippet;
        private clsBrusingLinking m_pBL;
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private int[] m_intsFeatureCnts;

        private int m_intClassNumber;

        private int m_intRounding = 2;

        private double[] m_VerticalBreaks;
        private double[] m_HorizontalBreaks;
        private bool m_blnAddBoxes = false;
        private bool m_blnInitialMaps = false;
        private bool m_blnPanelSizeChanged = false;

        private IEnvelope m_pFEvelop;

        private double m_dblPreHeight; //For deburg
        //private bool m_blnAddLabels = false;
        
        //Variables for DrawItem Event
        private string[] m_colorRampNames;
        private List<Color>[] m_colorLists;

        public frmCCMapsResults()
        {
            try
            {
                InitializeComponent();

                m_pSnippet = new clsSnippet();
                m_pBL = new clsBrusingLinking();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                //Create Color ramps for drawing event in combobox;
                cboColorRamp.DrawMode = DrawMode.OwnerDrawVariable;
                clsColorRamps pColorRamps = new clsColorRamps();
                m_colorRampNames = pColorRamps.colorRampNames;
                m_colorLists = pColorRamps.CreateColorList();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void InitialMapControls(int intRCount, int intCCount)
        {
            try
            {
                this.pnMapCntrls.SuspendLayout();
                this.SuspendLayout();

                int intTotal = intRCount * intCCount;
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewTemp));

                //Caculate MapControl Size and Initial Location

                int intXSize = pnMapCntrls.Size.Width / intCCount;
                int intYSize = pnMapCntrls.Size.Height / intRCount;
                //int intXIni = pnMapCntrls.Location.X; 
                //int intYIni = pnMapCntrls.Location.Y + pnMapCntrls.Size.Height - intYSize;

                int intXIni = 0;
                int intYIni = 0 + pnMapCntrls.Size.Height - intYSize;


                //Add Labels for feature counts
                this.FeatureCountLabels = new Label[intTotal];

                int intLabelWidth = 50;
                int intLabelHeight = 15;

                int intLabelXIni = intXSize - intLabelWidth - (intLabelWidth / 2);
                int intLabelYIni = (pnMapCntrls.Size.Height - intYSize) + (intLabelHeight / 2);

                int k = 0;

                for (int i = 0; i < intRCount; i++)
                {
                    for (int j = 0; j < intCCount; j++)
                    {
                        //Add Labels first,
                        string strLabelName = "lblCnt" + k.ToString();
                        int intLabelX = intLabelXIni + (intXSize * j);
                        int intLabelY = intLabelYIni - (intYSize * i);

                        this.FeatureCountLabels[k] = new Label();

                        this.FeatureCountLabels[k].AutoSize = true;
                        this.FeatureCountLabels[k].BackColor = System.Drawing.SystemColors.Window;
                        this.FeatureCountLabels[k].Location = new System.Drawing.Point(intLabelX, intLabelY);
                        this.FeatureCountLabels[k].Name = strLabelName;
                        this.FeatureCountLabels[k].Size = new System.Drawing.Size(intLabelWidth, intLabelHeight);
                        this.FeatureCountLabels[k].Text = "";
                        this.FeatureCountLabels[k].TextAlign = System.Drawing.ContentAlignment.MiddleRight;

                        this.pnMapCntrls.Controls.Add(FeatureCountLabels[k]);


                        //Then, add map controls
                        string strName = "axMapControl" + k.ToString();
                        int intX = intXIni + (intXSize * j);
                        int intY = intYIni - (intYSize * i);

                        this.m_axMapControls[k] = new ESRI.ArcGIS.Controls.AxMapControl();
                        ((System.ComponentModel.ISupportInitialize)(this.m_axMapControls[k])).BeginInit();

                        //this.SuspendLayout();

                        this.m_axMapControls[k].Location = new System.Drawing.Point(intX, intY);
                        this.m_axMapControls[k].Name = strName;
                        this.m_axMapControls[k].OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject(strName + ".OcxState")));
                        this.m_axMapControls[k].Size = new System.Drawing.Size(intXSize, intYSize);

                        //The size of map controls is not properly set. 052516 HK, so these codes are inserted below
                        this.m_axMapControls[k].MaximumSize = new System.Drawing.Size(intXSize, intYSize);
                        this.m_axMapControls[k].MinimumSize = new System.Drawing.Size(intXSize, intYSize);
                        //this.m_axMapControls[k].BorderStyle = esriControlsBorderStyle.esriNoBorder;

                        this.m_axMapControls[k].TabIndex = k;

                        this.m_axMapControls[k].Enter += new EventHandler(MapContrls_Enter);
                        //this.m_axMapControls[k].Leave += new EventHandler(MapContrls_Leave);
                        this.m_axMapControls[k].OnExtentUpdated += new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(MapCntrls_OnExtentUpdated); //For zoom in out

                        this.m_axMapControls[k].OnSelectionChanged += new EventHandler(MapCntrls_OnSelectionChanged);
                        //this.m_axMapControls[k].GotFocus += new EventHandler(MapContrls_GotFocus);
                        //this.m_axMapControls[k].Validated += new EventHandler(MapContrls_GotFocus);

                        this.pnMapCntrls.Controls.Add(this.m_axMapControls[k]);

                        ((System.ComponentModel.ISupportInitialize)(this.m_axMapControls[k])).EndInit();
                        k++;
                    }
                }
                //Checking Dock
                this.pnMapCntrls.Dock = System.Windows.Forms.DockStyle.Fill;


                this.pnMapCntrls.ResumeLayout(false);
                this.pnMapCntrls.PerformLayout();
                this.ResumeLayout(false);
                //this.PerformLayout();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        void MapCntrls_OnSelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (blnBrushingFromMapControl)
                    return;

                AxMapControl FocusedMapCntrl = (AxMapControl)sender;
                //MessageBox.Show(FocusedMapCntrl.Name);
                IMapControl3 pFocusedMapContrl = (IMapControl3)FocusedMapCntrl.Object;
                IFeatureLayer pSelFlayer = (IFeatureLayer)pFocusedMapContrl.ActiveView.FocusMap.get_Layer(0);
                IFeatureSelection pSelSet = (IFeatureSelection)pSelFlayer;

                if (pSelSet.SelectionSet.Count > 0)
                {
                    int intMapCntrlCnt = this.m_axMapControls.Length;
                    for (int i = 0; i < intMapCntrlCnt; i++)
                    {
                        //AxMapControl MapCntrl = this.m_axMapControls[i].Object;
                        IMapControl3 pMapCntrl = (IMapControl3)this.m_axMapControls[i].Object;

                        if (pMapCntrl != pFocusedMapContrl)
                        {
                            pMapCntrl.ActiveView.FocusMap.ClearSelection();
                            pMapCntrl.ActiveView.Refresh();

                        }
                    }
                }

                FeatureSelectionOnActiveView(m_pActiveView, pFLayer, pSelSet.SelectionSet);
                m_pBL.BrushingToOthers(pFLayer, this.Handle);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void FeatureSelectionOnActiveView(IActiveView pActiveView, IFeatureLayer pFLayer, ISelectionSet pSelSet)
        {
            ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast
            featureSelection.SelectionSet = pSelSet;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
        }
        void MapCntrls_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            try
            {
                if (m_blnPanelSizeChanged)
                    return;

                //try
                //{
                if (m_blnInitialMaps)
                {
                    IEnvelope pNewEnv = (IEnvelope)e.newEnvelope; //Get New envelope from e. 061516 HK

                    //Check the sender
                    AxMapControl FocusedMapCntrl = (AxMapControl)sender;
                    IMapControl3 pFocusedMapContrl = (IMapControl3)FocusedMapCntrl.Object;

                    //this event will be evoked only from the first map control that is same to the buddy of the toolbar control
                    object objBuddy = axTools.Buddy;
                    IToolbarControl2 ptbControl = (IToolbarControl2)axTools.Object;

                    IMapControl3 BuddyMapCntrl = (IMapControl3)objBuddy;
                    if (BuddyMapCntrl == pFocusedMapContrl)
                    {

                        int intMapCntrlCnt = this.m_axMapControls.Length;

                        for (int i = 0; i < intMapCntrlCnt; i++)
                        {
                            string strName = "axMapControl" + i.ToString();

                            AxMapControl pMapCntrl = (AxMapControl)this.pnMapCntrls.Controls[strName];

                            if (pMapCntrl != FocusedMapCntrl)
                                pMapCntrl.Extent = pNewEnv;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void MapContrls_Enter(object sender, EventArgs e)
        {
            try
            {
                AxMapControl FocusedMapCntrl = (AxMapControl)sender;
                IMapControl3 pMapContrl = (IMapControl3)FocusedMapCntrl.Object;

                object objBuddy = axTools.Buddy;
                IToolbarControl2 ptbControl = (IToolbarControl2)axTools.Object;


                ESRI.ArcGIS.SystemUI.ITool pCurrentTool = ptbControl.CurrentTool;

                //int intToolCursor = 0;
                //if (pCurrentTool != null)
                //    intToolCursor = ptbControl.CurrentTool.Cursor;

                IMapControl3 BuddyMapCntrl = (IMapControl3)objBuddy;

                if (pMapContrl != BuddyMapCntrl)
                {
                    axTools.SetBuddyControl(sender);
                    ptbControl.CurrentTool = pCurrentTool;
                    //FocusedMapCntrl.BackColor = Color.LightBlue;
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }


        private void frmCCMapsResults_Load(object sender, EventArgs e)
        {
            m_intMapCnt = intHorCnt * intVerCnt;
            m_axMapControls = new AxMapControl[m_intMapCnt];
            InitialMapControls(intVerCnt, intHorCnt);

            DrawCCMaps();
            m_blnInitialMaps = true;
        }

        private void DrawCCMaps()
        {
            m_intClassNumber = Convert.ToInt32(nudGCNClasses.Value);
            IFeatureClass pFClass = pFLayer.FeatureClass;

            //Determine Class Breaks for variable
            int intValueFldIdx = pFClass.FindField(strVarFldName);

            ITable pTable = (ITable)pFClass;
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

            
            ITableHistogram pTableHistogram = new BasicTableHistogramClass();
            
            pTableHistogram.Field = strVarFldName;
            pTableHistogram.Table = pTable;
            //IHistogram pHistogram = (IHistogram)pTableHistogram2;
            IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram;

            ////Not working in ArcGIS 10.5 022417 HK
            //ITableHistogram pTableHistogram = new TableHistogramClass();
            //pTableHistogram.Field = strVarFldName;
            //pTableHistogram.Table = pTable;
            //IHistogram pHistogram = (IHistogram)pTableHistogram;

            object xVals, frqs;
            pHistogram.GetHistogram(out xVals, out frqs);
            pClassifyGEN.Classify(xVals, frqs, m_intClassNumber);
            double[] cb = (double[])pClassifyGEN.ClassBreaks;

            //Class Determinations for vertical and horizontal axis

            if (m_VerticalBreaks == null)
            {

                pClassifyGEN = new QuantileClass(); //Using Quatile
                pTableHistogram = new BasicTableHistogramClass();
                pTableHistogram.Field = strVerConFldName;
                pTableHistogram.Table = pTable;
                pHistogram = (IBasicHistogram)pTableHistogram;

                pHistogram.GetHistogram(out xVals, out frqs);
                pClassifyGEN.Classify(xVals, frqs, intVerCnt);
                m_VerticalBreaks = (double[])pClassifyGEN.ClassBreaks;
            }

            if (m_HorizontalBreaks == null)
            {
                pClassifyGEN = new QuantileClass(); //Using Quatile
                pTableHistogram = new BasicTableHistogramClass();
                pTableHistogram.Field = strHorConFldName;
                pTableHistogram.Table = pTable;
                pHistogram = (IBasicHistogram)pTableHistogram;

                pHistogram.GetHistogram(out xVals, out frqs);
                pClassifyGEN.Classify(xVals, frqs, intHorCnt);
                m_HorizontalBreaks = (double[])pClassifyGEN.ClassBreaks;
            }

            //Create Renderer
            IClassBreaksRenderer pRender = new ClassBreaksRenderer();

            pRender.Field = strVarFldName;
            pRender.BreakCount = m_intClassNumber;
            pRender.MinimumBreak = cb[0];

            string strColorRamp = cboColorRamp.Text;

            IEnumColors pEnumColors = MultiPartColorRamp(strColorRamp, "CIE Lab", m_intClassNumber);
            pEnumColors.Reset();

            int[,] arrColors = new int[m_intClassNumber, 3];

            for (int k = 0; k < m_intClassNumber; k++)
            {
                IColor pColor = pEnumColors.Next();
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.RGB = pColor.RGB;

                arrColors[k, 0] = pRGBColor.Red;
                arrColors[k, 1] = pRGBColor.Green;
                arrColors[k, 2] = pRGBColor.Blue;
            }

            pEnumColors.Reset();
            IRgbColor pColorOutline = new RgbColor();
            //Can Change the color in here!
            pColorOutline = m_pSnippet.getRGB(picOutlineColor.BackColor.R, picOutlineColor.BackColor.G, picOutlineColor.BackColor.B);

            double dblGCOutlineSize = Convert.ToDouble(nudOutlinewidth.Value);

            ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
            pOutLines.Width = dblGCOutlineSize;
            pOutLines.Color = (IColor)pColorOutline;

            //' use this interface to set dialog properties
            IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pRender;
            pUIProperties.ColorRamp = "Custom";
            ISimpleFillSymbol pSimpleFillSym;

            pEnumColors.Reset();

            double dblAdding = Math.Pow(0.1, m_intRounding);
            for (int j = 0; j < m_intClassNumber; j++)
            {
                pRender.Break[j] = cb[j + 1];

                if (j == 0)
                {
                    pRender.Label[j] = Math.Round(cb[j], m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + m_intRounding.ToString());
                }
                else
                {
                    pRender.Label[j] = Math.Round(cb[j] + dblAdding, m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + m_intRounding.ToString());
                }

                pUIProperties.LowBreak[j] = cb[j];

                pSimpleFillSym = new SimpleFillSymbolClass();
                IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                pSimpleFillSym.Color = (IColor)pRGBColor;
                pSimpleFillSym.Outline = pOutLines;
                pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
            }

            //Background Layer
            ISimpleRenderer pSimpleRender = new SimpleRendererClass();
            ISimpleFillSymbol pBGSymbol = new SimpleFillSymbolClass();
            pBGSymbol.Color = (IColor)m_pSnippet.getRGB(picBGColor.BackColor.R, picBGColor.BackColor.G, picBGColor.BackColor.B);

            ICartographicLineSymbol pBGOutLines = new CartographicLineSymbol();
            pBGOutLines.Width = 0;
            pBGOutLines.Color = m_pSnippet.getRGB(255,255,255);
            pBGSymbol.Outline = pBGOutLines;

            pSimpleRender.Symbol = (ISymbol)pBGSymbol;

            IFeatureLayer pflBG = new FeatureLayerClass();
            pflBG.FeatureClass = pFClass;

            IGeoFeatureLayer pGeoBG = (IGeoFeatureLayer)pflBG;
            pGeoBG.Renderer = (IFeatureRenderer)pSimpleRender;
            pGeoBG.Selectable = false;

            //Feature Count for each map
            m_intsFeatureCnts = new int[intVerCnt * intHorCnt];

            int l = 0;

            for (int i = 0; i < intVerCnt; i++)
            {
                for (int j = 0; j < intHorCnt; j++)
                {
                    IFeatureLayer pflOutput = new FeatureLayerClass();
                    pflOutput.FeatureClass = pFClass;

                    IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;

                    pGeofeatureLayer.Renderer = (IFeatureRenderer)pRender;

                    IFeatureLayerDefinition2 pFDefinition = (IFeatureLayerDefinition2)pGeofeatureLayer;
                    string strWhereClause = null;

                    if (i == 0 && j == 0)
                        strWhereClause = strVerConFldName + " >= " + m_VerticalBreaks[i].ToString() + " AND " +
                            strVerConFldName + " <= " + m_VerticalBreaks[i + 1].ToString() + " AND " +
                            strHorConFldName + " >= " + m_HorizontalBreaks[j].ToString() + " AND " +
                            strHorConFldName + " <= " + m_HorizontalBreaks[j + 1].ToString();
                    else if (i != 0 && j == 0)
                        strWhereClause = strVerConFldName + " > " + m_VerticalBreaks[i].ToString() + " AND " +
                            strVerConFldName + " <= " + m_VerticalBreaks[i + 1].ToString() + " AND " +
                            strHorConFldName + " >= " + m_HorizontalBreaks[j].ToString() + " AND " +
                            strHorConFldName + " <= " + m_HorizontalBreaks[j + 1].ToString();
                    else if (i == 0 && j != 0)
                        strWhereClause = strVerConFldName + " >= " + m_VerticalBreaks[i].ToString() + " AND " +
                            strVerConFldName + " <= " + m_VerticalBreaks[i + 1].ToString() + " AND " +
                            strHorConFldName + " > " + m_HorizontalBreaks[j].ToString() + " AND " +
                            strHorConFldName + " <= " + m_HorizontalBreaks[j + 1].ToString();
                    else
                        strWhereClause = strVerConFldName + " > " + m_VerticalBreaks[i].ToString() + " AND " +
                            strVerConFldName + " <= " + m_VerticalBreaks[i + 1].ToString() + " AND " +
                            strHorConFldName + " > " + m_HorizontalBreaks[j].ToString() + " AND " +
                            strHorConFldName + " <= " + m_HorizontalBreaks[j + 1].ToString();
                   
                    pFDefinition.DefinitionExpression = strWhereClause;

                    IQueryFilter pQfilter = new QueryFilterClass();
                    pQfilter.WhereClause = strWhereClause;

                    m_intsFeatureCnts[l] = pGeofeatureLayer.FeatureClass.FeatureCount(pQfilter);
                    m_axMapControls[l].ActiveView.FocusMap.ClearLayers();
                    m_axMapControls[l].ActiveView.FocusMap.AddLayer(pGeoBG);
                    m_axMapControls[l].ActiveView.FocusMap.AddLayer(pGeofeatureLayer);
                    m_axMapControls[l].ActiveView.Extent = m_axMapControls[l].ActiveView.FullExtent;
                    m_axMapControls[l].ActiveView.Refresh();
                    l++;
                }
            }

            lblVariable.Text = "Selected Variable: " + strVarFldName;
            UpdateRange(lvSymbol, m_intClassNumber, cb, arrColors, m_intRounding);

            if (m_blnAddBoxes)
                UpdateTextBoxes();
            else
                AddTextBoxes();

            UpdateLabels();
            UpdateHorVerLabels();

        }
        private void UpdateLabels()
        {
            int intTotalCells = intHorCnt * intVerCnt;
            for (int i = 0; i < intTotalCells; i++)
            {
                ((Label)pnMapCntrls.Controls["lblCnt" + i.ToString()]).Text = m_intsFeatureCnts[i].ToString();
            }
        }

        private void UpdateTextBoxes()
        {
            for (int i = 0; i < intHorCnt - 1; i++)
            {
                ((TextBox)pnHori.Controls["txtHori" + i.ToString()]).Text = m_HorizontalBreaks[i + 1].ToString();
                ((TextBox)pnHori.Controls["txtHori" + i.ToString()]).ForeColor = Color.Black;
            }
            for (int i = 0; i < intVerCnt - 1; i++)
            {
                ((TextBox)pnVert.Controls["txtVert" + i.ToString()]).Text = m_VerticalBreaks[i + 1].ToString();
                ((TextBox)pnVert.Controls["txtVert" + i.ToString()]).ForeColor = Color.Black;
            }
        }
        private void UpdateHorVerLabels()
        {
            lblHori.Text = "X-axis: " + strHorConFldName + " (Min: " + m_HorizontalBreaks[0].ToString() + ")" ;
            lblVert.Text = "Y-axis: " + strVerConFldName;
            lblVerMin.Text = "(Min: " + m_VerticalBreaks[0].ToString() + ")";
            lblHoriMax.Text = "(Max: " + m_HorizontalBreaks[intHorCnt].ToString() + ")";
            
            lblVertMax.Text=  "(Max: " + m_VerticalBreaks[intVerCnt].ToString() + ")";
        
        }

        private void AddTextBoxes()
        {
            int intTxtBoxWidth = 100;
            int intTxtBoxHeight = 20;

            int intXSize = pnMapCntrls.Size.Width / intHorCnt;
            int intYSize = pnMapCntrls.Size.Height / intVerCnt;

            this.pnHori.SuspendLayout();
            this.pnVert.SuspendLayout();
            this.SuspendLayout();

            HoriTxtBoxes = new TextBox[intHorCnt - 1];

            for (int i = 0; i < intHorCnt - 1; i++)
            {
                int intX = intXSize * (i + 1) - (intTxtBoxWidth / 2);
                int intY = (pnHori.Height / 2) - (intTxtBoxHeight / 2);

                HoriTxtBoxes[i] = new TextBox();
                this.HoriTxtBoxes[i].Location = new System.Drawing.Point(intX, intY);
                this.HoriTxtBoxes[i].Name = "txtHori" + i.ToString();
                this.HoriTxtBoxes[i].Size = new System.Drawing.Size(intTxtBoxWidth, intTxtBoxHeight);
                //this.textBox1.TabIndex = 2;
                this.HoriTxtBoxes[i].Text = m_HorizontalBreaks[i + 1].ToString();
                this.HoriTxtBoxes[i].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                this.HoriTxtBoxes[i].TextChanged += new EventHandler(txtBoxes_TextChanged);
                this.pnHori.Controls.Add(this.HoriTxtBoxes[i]);
            }

            VertTxtBoxes = new TextBox[intVerCnt - 1];
            
            for (int i = 0; i < intVerCnt - 1; i++)
            {
                //Add txtBox from Bottom
                int intX = (pnVert.Width / 2) - (intTxtBoxWidth / 2);
                int intY = intYSize * (intVerCnt - (i + 1)) - (intTxtBoxHeight / 2);

                VertTxtBoxes[i] = new TextBox();
                this.VertTxtBoxes[i].Location = new System.Drawing.Point(intX, intY);
                this.VertTxtBoxes[i].Name = "txtVert" + i.ToString();
                this.VertTxtBoxes[i].Size = new System.Drawing.Size(intTxtBoxWidth, intTxtBoxHeight);
                //this.textBox1.TabIndex = 2;
                this.VertTxtBoxes[i].Text = m_VerticalBreaks[i + 1].ToString();
                this.VertTxtBoxes[i].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                this.VertTxtBoxes[i].TextChanged += new EventHandler(txtBoxes_TextChanged);
                this.pnVert.Controls.Add(this.VertTxtBoxes[i]);
            }

            this.pnHori.ResumeLayout(false);
            this.pnHori.PerformLayout();
            this.pnVert.ResumeLayout(false);
            this.pnVert.PerformLayout();
            this.ResumeLayout(false);

            m_blnAddBoxes = true;


        }
        private void UpdateRange(ListView lvSymbol, int intGCBreakeCount, double[] cb, int[,] arrColors, int intRounding)
        {
            try
            {
                lvSymbol.BeginUpdate();
                lvSymbol.Items.Clear();

                for (int j = 0; j < intGCBreakeCount; j++)
                {

                    ListViewItem lvi = new ListViewItem("");
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

                    if (j == 0)
                    {
                        lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                        }
                    else
                    {

                        double dblAdding = Math.Pow(0.1, intRounding);
                        lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                        }
                    lvSymbol.Items.Add(lvi);
                }
                lvSymbol.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
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

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            DrawCCMaps();
        }

        private void txtBoxes_TextChanged(object sender, EventArgs e) 
        {
            if (m_VerticalBreaks != null)//When txtBox.text is changed, update Breaks array.
            {
                if (this.VertTxtBoxes.Length != (intVerCnt - 1)) //Check the TextBoxes
                    return;

                for (int i = 0; i < intVerCnt - 1; i++)
                {
                    double dblValue = Convert.ToDouble(((TextBox)pnVert.Controls["txtVert" + i.ToString()]).Text); 
                    //double dblValue = Convert.ToDouble(VertTxtBoxes[i].Text);

                    if (dblValue != m_VerticalBreaks[i + 1])
                    {
                        ((TextBox)pnVert.Controls["txtVert" + i.ToString()]).ForeColor = Color.Red;
                        m_VerticalBreaks[i + 1] = dblValue;
                    }
                }
            }

            if (m_HorizontalBreaks != null)
            {
                if (this.HoriTxtBoxes.Length != (intHorCnt - 1)) //Check the TextBoxes
                    return;

                for (int i = 0; i < intHorCnt - 1; i++)
                {
                    double dblValue = Convert.ToDouble(((TextBox)pnHori.Controls["txtHori" + i.ToString()]).Text); 
                    //Convert.ToDouble(HoriTxtBoxes[i].Text);

                    if (dblValue != m_HorizontalBreaks[i + 1])
                    {
                        ((TextBox)pnHori.Controls["txtHori" + i.ToString()]).ForeColor = Color.Red;
                        m_HorizontalBreaks[i + 1] = dblValue;
                    }
                }
            }
        }

        private void pnMapCntrls_Resize(object sender, EventArgs e)
        {
            m_blnPanelSizeChanged = true;
            
            int intXSize = pnMapCntrls.Size.Width / intHorCnt;
            int intYSize = pnMapCntrls.Size.Height / intVerCnt;

            int intXIni = 0;
            int intYIni = 0 + pnMapCntrls.Size.Height - intYSize;

            int intLabelWidth = 50;
            int intLabelHeight = 15;

            int intLabelXIni = intXSize - intLabelWidth - (intLabelWidth / 2);
            int intLabelYIni = (pnMapCntrls.Size.Height - intYSize) + (intLabelHeight / 2);

            int k = 0;

            for (int i = 0; i < intVerCnt; i++)
            {
                for (int j = 0; j < intHorCnt; j++)
                {
                    string strLabelName = "lblCnt" + k.ToString();
                    int intLabelX = intLabelXIni + (intXSize * j);
                    int intLabelY = intLabelYIni - (intYSize * i);

                    //Label Location Setting
                   ((Label)pnMapCntrls.Controls[strLabelName]).Location = new System.Drawing.Point(intLabelX, intLabelY);


                    //Then, add map controls
                    string strName = "axMapControl" + k.ToString();
                    int intX = intXIni + (intXSize * j);
                    int intY = intYIni - (intYSize * i);

                    ((AxMapControl)this.pnMapCntrls.Controls[strName]).Location = new System.Drawing.Point(intX, intY);
                    ((AxMapControl)this.pnMapCntrls.Controls[strName]).Size = new System.Drawing.Size(intXSize, intYSize);
                    ((AxMapControl)this.pnMapCntrls.Controls[strName]).MaximumSize = new System.Drawing.Size(intXSize, intYSize);
                    ((AxMapControl)this.pnMapCntrls.Controls[strName]).MinimumSize = new System.Drawing.Size(intXSize, intYSize);

                   ((AxMapControl)this.pnMapCntrls.Controls[strName]).ActiveView.Refresh();
                    k++;
                }
            }

            //Text Box Location setting


            int intTxtBoxWidth = 100;
            int intTxtBoxHeight = 20;

            for (int i = 0; i < intHorCnt - 1; i++)
            {
                int intX = intXSize * (i + 1) - (intTxtBoxWidth / 2);
                int intY = (pnHori.Height / 2) - (intTxtBoxHeight / 2);

                ((TextBox)pnHori.Controls["txtHori" + i.ToString()]).Location = new System.Drawing.Point(intX, intY);

            }

            for (int i = 0; i < intVerCnt - 1; i++)
            {
                //Add txtBox from Bottom
                int intX = (pnVert.Width / 2) - (intTxtBoxWidth / 2);
                int intY = intYSize * (intVerCnt - (i + 1)) - (intTxtBoxHeight / 2);

                ((TextBox)pnVert.Controls["txtVert" + i.ToString()]).Location = new System.Drawing.Point(intX, intY);
            }

            m_blnPanelSizeChanged = false;

        }

        private void picBGColor_Click(object sender, EventArgs e)
        {
            try
            {
                
                DialogResult DR = cdColor.ShowDialog();
                if (DR == DialogResult.OK)
                    picBGColor.BackColor = cdColor.Color;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void picOutlineColor_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult DR = cdColor.ShowDialog();
                if (DR == DialogResult.OK)
                    picOutlineColor.BackColor = cdColor.Color;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            //Not working properly; 022817 HK
            frmExportChart pfrmExportChart = new frmExportChart();
            //pfrmExportChart.thisChart = pChart;
            pfrmExportChart.thisForm = this;
            pfrmExportChart.nudHeight.Value = this.Height;
            pfrmExportChart.nudHeight.Maximum = this.Height * 5; //Restriction of maximum size
            pfrmExportChart.nudWidth.Value = this.Width;
            pfrmExportChart.nudWidth.Maximum = this.Width * 5;
            pfrmExportChart.Show();
        }

        private void cboColorRamp_DrawItem(object sender, DrawItemEventArgs e)
        {

            Graphics g = e.Graphics;

            // Get the bounding rectangle of the item currently being painted
            Rectangle r = e.Bounds;

            if (e.Index >= 0)
            {
                // Set the string format options
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;

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

                e.DrawFocusRectangle();
            }
        }

        private void cboColorRamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboColorRamp.Text == "Custom")
            {
                frmColorRamps pColorRamps = new frmColorRamps();
                pColorRamps.intLoadingPlaces = 4;
                pColorRamps.Show();
            }
        }


    }
}
