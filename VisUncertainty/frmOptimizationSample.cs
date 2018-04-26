using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using Gurobi;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using System.Drawing.Drawing2D;


namespace VisUncertainty
{
    public partial class frmOptimizationSample : Form
    {
        private MainForm mForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private clsBrusingLinking m_pBL;

        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        public double[] m_cb;
        private int[] m_cbIdx;
        private int m_intNofFeatures;
        private ClassBreaksRenderer m_pRender;
        public IEnumColors m_pEnumColors;
        private int[,] m_arrColors;
        private string[] m_arrLabel;
        public int m_intGCBreakeCount;

        private int m_intRounding;
        //private int m_MaxFeaturesforPGB = 100;

        private double[] m_arrEst;
        private double[] m_arrVar;

        private GRBModel m_pModel; //Gurobi Optimization Model
        private GRBLinExpr m_pTotalStepsConst;
        private GRBVar[] m_pDecVar;
        private string m_strClassificationMethod = string.Empty;

        private int m_intAreaChartHeight = 10;
        private double m_dblChartMin = 0;
        private double m_dblChartMax;
        private int m_intDecimalPlace = 2;

        private int m_intPtsIdx = 0;
        private int m_intTotalNSeries = 0;
        private double[,] m_arrEvalStat;

        private bool m_blnAreaBrushing = false;
        #region Variables for Drawing Rectangle
        private int _startX, _startY;
        private bool _canDraw;
        private Rectangle _rect;
        private Graphics pGraphics;
        #endregion

        //Variables for DrawItem Event
        private string[] m_colorRampNames;
        private List<Color>[] m_colorLists;

        public frmOptimizationSample()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

                m_pActiveView = mForm.axMapControl1.ActiveView;
                m_pSnippet = new clsSnippet();
                m_pBL = new clsBrusingLinking();

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                m_intRounding = 2; //Default

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

        #region hidden
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboUncernFld.Text == "" || cboValueField.Text == "")
                {
                    MessageBox.Show("Please assign proper fields");
                    return;
                }
                m_pSnippet.AddRenderInfo(mForm, m_pFLayer, cboValueField.Text, cboUncernFld.Text, m_cb, m_arrColors, m_strClassificationMethod);

                IRgbColor pColorOutline = new RgbColor();
                //Can Change the color in here!
                pColorOutline = m_pSnippet.getRGB(picGCLineColor.BackColor.R, picGCLineColor.BackColor.G, picGCLineColor.BackColor.B);
                if (pColorOutline == null)
                    return;
                double dblGCOutlineSize = Convert.ToDouble(nudGCLinewidth.Value);

                ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
                pOutLines.Width = dblGCOutlineSize;
                pOutLines.Color = (IColor)pColorOutline;

                //' use this interface to set dialog properties
                IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)m_pRender;
                pUIProperties.ColorRamp = "Custom";
                ISimpleFillSymbol pSimpleFillSym;

                double dblAdding = Math.Pow(0.1, m_intRounding);
                //pEnumColors.Reset();
                //' be careful, indices are different for the diff lists
                for (int j = 0; j < m_intGCBreakeCount; j++)
                {
                    m_pRender.Break[j] = m_cb[j + 1];
                    if(j==0)
                        m_pRender.Label[j]  = Math.Round(m_cb[j], m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], 2).ToString("N" + m_intRounding.ToString());
                    else
                        m_pRender.Label[j] = Math.Round(m_cb[j] + dblAdding, m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], 2).ToString("N" + m_intRounding.ToString());
                    
                    pUIProperties.LowBreak[j] = m_cb[j];
                    pSimpleFillSym = new SimpleFillSymbolClass();
                    IRgbColor pRGBColor = m_pSnippet.getRGB(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]);
                    if (pRGBColor == null)
                        return;
                    pSimpleFillSym.Color = (IColor)pRGBColor;
                    pSimpleFillSym.Outline = pOutLines;
                    m_pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
                }

                IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)m_pFLayer;

                pGeofeatureLayer.Renderer = (IFeatureRenderer)m_pRender;

                mForm.axMapControl1.ActiveView.Refresh();
                mForm.axTOCControl1.Update();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        #region deprecated functions
        //public void DrawSymbolinListView(int intType)
        //{
        //    try
        //    {

        //        lvSymbol.Items.Clear();
        //        string strGCRenderField = cboValueField.Text;
        //        string strUncernfld = cboUncernFld.Text;
        //        m_intGCBreakeCount = Convert.ToInt32(nudGCNClasses.Value);

        //        int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
        //        int intUncernfldIdx = m_pFClass.FindField(strUncernfld);

        //        if (intUncernfldIdx == -1 || intValueFldIdx == -1 || cboMethod.Text == "")
        //            return;

        //        frmProgress pfrmProgress = new frmProgress();
        //        pfrmProgress.lblStatus.Text = "Processing:";
        //        pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
        //        pfrmProgress.Show();
        //        pfrmProgress.Refresh();

        //        ITable pTable = (ITable)m_pFClass;

        //        ITableSort pTableSort = new TableSort();
        //        pTableSort.Table = pTable;
        //        ICursor pCursor = (ICursor)m_pFClass.Search(null, false);

        //        pTableSort.Cursor = pCursor as ICursor;

        //        pTableSort.Fields = strGCRenderField;
        //        pTableSort.set_Ascending(strGCRenderField, true);

        //        // call the sort
        //        pTableSort.Sort(null);

        //        // retrieve the sorted rows
        //        IFeatureCursor pSortedCursor = pTableSort.Rows as IFeatureCursor;

        //        //Get Optimized result
        //        if (intType == 1)
        //            m_cb = OptUsingGurobi(m_intGCBreakeCount, m_intNofFeatures, pSortedCursor, intValueFldIdx, intUncernfldIdx);
        //        else if (intType == 2)
        //            m_cb = ChangeNBreaksGurobi(m_pModel, m_pTotalStepsConst, m_pDecVar, m_intGCBreakeCount, m_intNofFeatures);

        //        m_pRender = new ClassBreaksRenderer();

        //        m_pRender.Field = strGCRenderField;
        //        m_pRender.BreakCount = m_intGCBreakeCount;
        //        m_pRender.MinimumBreak = m_cb[0];

        //        string strColorRamp = cboColorRamp.Text;

        //        m_pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, m_intGCBreakeCount);
        //        m_pEnumColors.Reset();

        //        m_arrColors = new int[m_intGCBreakeCount, 3];

        //        for (int k = 0; k < m_intGCBreakeCount; k++)
        //        {
        //            IColor pColor = m_pEnumColors.Next();
        //            IRgbColor pRGBColor = new RgbColorClass();
        //            pRGBColor.RGB = pColor.RGB;

        //            m_arrColors[k, 0] = pRGBColor.Red;
        //            m_arrColors[k, 1] = pRGBColor.Green;
        //            m_arrColors[k, 2] = pRGBColor.Blue;
        //        }

        //        m_pEnumColors.Reset();

        //        pfrmProgress.Close();
        //        UpdateRange(lvSymbol, m_intGCBreakeCount);

        //        btnGraph.Enabled = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        //public void DrawSymbolinListViewwithCb(double[] Cs, int intGCBreakeCount)
        //{
        //    try
        //    {
        //        lvSymbol.Items.Clear();
        //        string strGCRenderField = cboValueField.Text;
        //        string strUncernfld = cboUncernFld.Text;

        //        int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
        //        int intUncernfldIdx = m_pFClass.FindField(strUncernfld);

        //        if (intUncernfldIdx == -1 || intValueFldIdx == -1 || cboMethod.Text == "")
        //            return;

        //        m_cb = Cs;

        //        m_pRender = new ClassBreaksRenderer();

        //        m_pRender.Field = strGCRenderField;
        //        m_pRender.BreakCount = intGCBreakeCount;
        //        m_pRender.MinimumBreak = m_cb[0];

        //        string strColorRamp = cboColorRamp.Text;

        //        m_pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, intGCBreakeCount);
        //        m_pEnumColors.Reset();

        //        m_arrColors = new int[intGCBreakeCount, 3];

        //        for (int k = 0; k < intGCBreakeCount; k++)
        //        {
        //            IColor pColor = m_pEnumColors.Next();
        //            IRgbColor pRGBColor = new RgbColorClass();
        //            pRGBColor.RGB = pColor.RGB;

        //            m_arrColors[k, 0] = pRGBColor.Red;
        //            m_arrColors[k, 1] = pRGBColor.Green;
        //            m_arrColors[k, 2] = pRGBColor.Blue;
        //        }

        //        m_pEnumColors.Reset();

        //        UpdateRange(lvSymbol, intGCBreakeCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        #endregion

        public void DrawSymbolinChartwithCb(double[] Cs, int intGCBreakeCount)
        {
            try
            {
                
                string strGCRenderField = cboValueField.Text;
                string strUncernfld = cboUncernFld.Text;

                int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
                int intUncernfldIdx = m_pFClass.FindField(strUncernfld);
                int intNFeatureCount = m_pFClass.FeatureCount(null);

                if (intUncernfldIdx == -1 || intValueFldIdx == -1 || cboMethod.Text == "")
                    return;
                
                m_cb = Cs;

                m_pRender = new ClassBreaksRenderer();

                m_pRender.Field = strGCRenderField;
                m_pRender.BreakCount = intGCBreakeCount;
                m_pRender.MinimumBreak = m_cb[0];

                string strColorRamp = cboColorRamp.Text;

                m_pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, intGCBreakeCount);
                m_pEnumColors.Reset();

                m_arrColors = new int[intGCBreakeCount, 3];

                for (int k = 0; k < intGCBreakeCount; k++)
                {
                    IColor pColor = m_pEnumColors.Next();
                    IRgbColor pRGBColor = new RgbColorClass();
                    pRGBColor.RGB = pColor.RGB;

                    m_arrColors[k, 0] = pRGBColor.Red;
                    m_arrColors[k, 1] = pRGBColor.Green;
                    m_arrColors[k, 2] = pRGBColor.Blue;
                }

                int intLastSeriesIdx = pChart.Series.Count - 1;
                int intRemovingSeriesCnt = (intGCBreakeCount * 2) -1;
                for (int j = 0; j < intRemovingSeriesCnt; j++)
                {
                    pChart.Series.RemoveAt(intLastSeriesIdx);
                    intLastSeriesIdx = pChart.Series.Count - 1;
                }

                m_pEnumColors.Reset();

                for (int j = 0; j < m_cb.Length - 1; j++)
                {
                    if (j == 0)
                        AddAreaChartSeries(this, "area_" + j.ToString(), Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), m_dblChartMin, m_cb[j + 1], m_intAreaChartHeight - 2);
                    else if (j == m_cb.Length - 2)
                        AddAreaChartSeries(this, "area_" + j.ToString(), Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), m_cb[j], m_dblChartMax, m_intAreaChartHeight - 2);
                    else
                        AddAreaChartSeries(this, "area_" + j.ToString(), Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), m_cb[j], m_cb[j + 1], m_intAreaChartHeight - 2);

                }

                for (int j = 1; j < m_cb.Length - 1; j++)
                {
                    AddVerticalLineSeries2(this, "ver_" + j.ToString(), Color.Red, m_cb[j], -3, 100 + m_intAreaChartHeight);
                }
                
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void UpdateRange(ListView lvSymbol, int intGCBreakeCount)
        {
            try
            {
                m_arrLabel = new string[intGCBreakeCount];

                lvSymbol.BeginUpdate();
                lvSymbol.Items.Clear();
                m_pEnumColors.Reset();

                for (int j = 0; j < intGCBreakeCount; j++)
                {

                    ListViewItem lvi = new ListViewItem("");
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        "", Color.White, Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), lvi.Font));

                    if (j == 0)
                    {
                        lvi.SubItems.Add(Math.Round(m_cb[j], m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], m_intRounding).ToString("N" + m_intRounding.ToString()));
                        lvi.SubItems.Add(Math.Round(m_cb[j], m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], 2).ToString("N" + m_intRounding.ToString()));
                        m_arrLabel[j] = Math.Round(m_cb[j], m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], 2).ToString("N" + m_intRounding.ToString());
                    }
                    else
                    {

                        double dblAdding = Math.Pow(0.1, m_intRounding);
                        lvi.SubItems.Add(Math.Round(m_cb[j] + dblAdding, m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], m_intRounding).ToString("N" + m_intRounding.ToString()));
                        lvi.SubItems.Add(Math.Round(m_cb[j] + dblAdding, m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], 2).ToString("N" + m_intRounding.ToString()));
                        m_arrLabel[j] = Math.Round(m_cb[j] + dblAdding, m_intRounding).ToString("N" + m_intRounding.ToString()) + " - " + Math.Round(m_cb[j + 1], 2).ToString("N" + m_intRounding.ToString());
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

        #endregion


        #region Classification Method
        #region deprecated functions that solved by MS solver express
        //Over 1000 decision variables cannot be solved by MS solver express (it requires Academic License)
        //private double[] OptUsingMSSolver(int intNClasses, int intNFeatures, IFeatureCursor pFCursor, int intEstIdx, int intVarIdx)
        //{
        //    frmProgress pfrmProgress = new frmProgress();
        //    if (intNFeatures > MaxFeaturesforPGB)
        //    {
        //        pfrmProgress.lblStatus.Text = "Calculate!!!";
        //        pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
        //        //pfrmProgress.pgbProgress.Value = 0;
        //        pfrmProgress.Show();
        //    }

        //    double[] Cs = new double[intNClasses + 1];

        //    //System.Array arrEst = new System.Array[intNFeatures];
        //    double[] arrEst = new double[intNFeatures];
        //    double[] arrVar = new double[intNFeatures];
        //    double[] arrResults = new double[intNFeatures - 1];
        //    double[] arrSortedResult = new double[intNFeatures - 1];

        //    //Create Solver
        //    SolverContext pSolver = SolverContext.GetContext();
        //    pSolver.ClearModel();
        //    Model pModel = pSolver.CreateModel();

        //    //Store Value to Array
        //    IFeature pFeature = pFCursor.NextFeature();
        //    int k = 0;
        //    while (pFeature != null)
        //    {
        //        //arrEst.SetValue(pFeature.get_Value(intEstIdx), k);
        //        arrEst[k] = Convert.ToDouble(pFeature.get_Value(intEstIdx));
        //        arrVar[k] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
        //        k++;
        //        pFeature = pFCursor.NextFeature();
        //    }
        //    int intNDecVar = (intNFeatures * (intNFeatures-1)) / 2;

        //    //Objective Function
        //    SumTermBuilder objective = new SumTermBuilder(intNDecVar);

        //    //Term for Total Steps constraints
        //    SumTermBuilder TotalSteps = new SumTermBuilder(intNDecVar);

        //    //Terms for Flow constraints
        //    var listInFlows = new List<SumTermBuilder>();
        //    for (int i = 1; i < intNFeatures; i++)
        //    {
        //        SumTermBuilder pInFlowComp = new SumTermBuilder(i);
        //        listInFlows.Add(pInFlowComp);
        //    }
        //    var listOutFlows = new List<SumTermBuilder>();

        //    for (int i = 0; i < intNFeatures - 1; i++)
        //    {
        //        //OutFlow Constraints
        //        SumTermBuilder pOutFlowComp = new SumTermBuilder(intNFeatures - (i + 1));

        //        for (int j = i + 1; j < intNFeatures; j++)
        //        {
        //            string strVarName = "L" + i.ToString() + "_" + j.ToString();

        //            //Extract Subet Array
        //            double[] arrSubEst = new double[j - i];
        //            double[] arrSubVar = new double[j - i];
        //            System.Array.Copy(arrEst, i, arrSubEst, 0, j - i);
        //            System.Array.Copy(arrVar, i, arrSubVar, 0, j - i);

        //            //Calculate cost!
        //            //1.Std
        //            double dblCost = CalSDfromArray((double[])arrSubEst);

        //            //Adding Decision variable
        //            var decision = new Decision(Domain.IntegerRange(0, 1), strVarName);
        //            pModel.AddDecision(decision);

        //            //Objective Function
        //            objective.Add(decision * dblCost);

        //            //Total Step Constraints
        //            TotalSteps.Add(decision);

        //            //OutFlow Constraints
        //            pOutFlowComp.Add(decision);

        //            //InFlow Constraints
        //            listInFlows[j-1].Add(decision);
        //        }

        //        listOutFlows.Add(pOutFlowComp);
        //    }

        //    //Add Total Steps Constraints
        //    var TStepsConst = TotalSteps.ToTerm() == intNClasses;
        //    pModel.AddConstraint("Nsteps", TStepsConst);

        //    //Add Flow Constraint to Model
        //    //Trick for Fist Steps and Last Steps
        //    listInFlows[0].Add(0);
        //    listOutFlows[intNFeatures - 2].Add(0);

        //    for (int i = 1; i < intNFeatures-1; i++)
        //    {
        //        string strConstName = "N" + i.ToString();
        //        var pConstraints = listOutFlows[i].ToTerm() == listInFlows[i-1].ToTerm();
        //        pModel.AddConstraint(strConstName, pConstraints);
        //    }

        //    //Add Start and End node constraints
        //    var pStartConst = listOutFlows[0].ToTerm() == 1;
        //    pModel.AddConstraint("StartNode", pStartConst);
        //    var pEndConst = listInFlows[intNFeatures - 2].ToTerm() == 1;
        //    pModel.AddConstraint("EndNode", pEndConst);

        //    //Solving
        //    var pSolution = pSolver.Solve();

        //    //Add Results to CS array
        //    Cs[0] = arrEst[0]; //Estimate Array was sorted
        //    int intIdxCs = 0;

        //    if (pSolution.Quality == SolverQuality.Optimal)
        //    {
        //        System.Collections.IEnumerator pDecisions = pModel.Decisions.GetEnumerator();

        //        while (pDecisions.MoveNext())
        //        {
        //            Decision pDecision = (Decision)pDecisions.Current;
        //            string strName = pDecision.Name;
        //            Double dblValue = pDecision.ToDouble();

        //            if (dblValue == 1)
        //            {
        //                intIdxCs++;
        //                int intIndexUBar = strName.IndexOf("_");
        //                //string strFrom = strName.Substring(1, strName.Length - intIndexUBar);
        //                string strTo = strName.Substring(intIndexUBar+1);
        //                int intToValue = Convert.ToInt16(strTo);
        //                Cs[intIdxCs] = arrEst[intToValue];
        //            }
        //        }

        //    }



        //    MessageBox.Show("Show");

        //    return Cs;

        //}
        #endregion

        //A Gurobi Academic License is used 
        private double[] OptUsingGurobi(int intNClasses, int intNFeatures, IFeatureCursor pFCursor, int intEstIdx, int intVarIdx)
        {

            bool blnMinMax = true;

            if (cboMethod.Text == "Maximum sum of within groups" || cboMethod.Text == "Maximum within groups")
                blnMinMax = true;
            else
                blnMinMax = false;
            //try
            //{
                //frmProgress pfrmProgress = new frmProgress();
                //if (intNFeatures > MaxFeaturesforPGB)
                //{
                //    pfrmProgress.lblStatus.Text = "Calculate!!!";
                //    pfrmProgress.pgbProgress.Style = ProgressBarStyle.Blocks;
                //    pfrmProgress.pgbProgress.Value = 0;
                //    pfrmProgress.Show();
                //}

                double[] Cs = new double[intNClasses + 1];
                m_cbIdx = new int[intNClasses + 1]; // For Graph

                m_arrEst = new double[intNFeatures];
                m_arrVar = new double[intNFeatures];

                //Create Solver
                GRBEnv env = new GRBEnv();
                m_pModel = new GRBModel(env);

                //Store Value to Array
                IFeature pFeature = pFCursor.NextFeature();
                int k = 0;
                while (pFeature != null)
                {
                    m_arrEst[k] = Convert.ToDouble(pFeature.get_Value(intEstIdx));
                    m_arrVar[k] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                    k++;
                    pFeature = pFCursor.NextFeature();
                }
                int intNDecVar = (intNFeatures * (intNFeatures + 1)) / 2; // Add L0_0

                double[] arrCosts = new double[intNDecVar];
                //Decision variables
                m_pDecVar = new GRBVar[intNDecVar];
                if (blnMinMax)
                {
                    m_pDecVar = new GRBVar[intNDecVar + 1];

                }


                ////Objective Function
                GRBLinExpr pObjFunc = new GRBLinExpr();

                ////Term for Total Steps constraints
                m_pTotalStepsConst = new GRBLinExpr();

                //Terms for Flow constraints
                var listInFlows = new List<GRBLinExpr>();
                for (int i = 0; i < intNFeatures; i++)
                {
                    GRBLinExpr pInFlowComp = new GRBLinExpr();
                    listInFlows.Add(pInFlowComp);
                }
                var listOutFlows = new List<GRBLinExpr>();

                int intID = 0;

            
                for (int i = 0; i < intNFeatures; i++)
                {
                    //OutFlow Constraints
                    GRBLinExpr pOutFlowComp = 0.0;

                    for (int j = i + 1; j < intNFeatures + 1; j++)
                    {
                        string strVarName = "L" + i.ToString() + "_" + j.ToString();

                        //Create Subset Arrays
                        int intArrayLen = j - i;
                        double[] arrSubEst = new double[intArrayLen];
                        double[] arrSubVar = new double[intArrayLen];
                        System.Array.Copy(m_arrEst, i, arrSubEst, 0, intArrayLen);
                        System.Array.Copy(m_arrVar, i, arrSubVar, 0, intArrayLen);

                        //Calculate cost for Array
                        //1.Std
                        double dblCost = 0;
                        StringBuilder sbClassificationMethod = new StringBuilder();
                        sbClassificationMethod.Append("Minimize ");
                        if (cboMethod.Text == "Sum of within groups" || cboMethod.Text == "Maximum sum of within groups")
                        {
                            if (cboMeasures.Text == "Class Separability")
                                dblCost = SumSeperablility(arrSubEst, arrSubVar);
                            else if (cboMeasures.Text == "Bhattacharyya distance ")
                                dblCost = SumBhatta(arrSubEst, arrSubVar);
                        }
                        else if (cboMethod.Text == "Sum of the maximum within groups" || cboMethod.Text == "Maximum within groups")
                        {
                            if (cboMeasures.Text == "Class Separability")
                                dblCost = MaxSeperablility(arrSubEst, arrSubVar);
                            else if (cboMeasures.Text == "Bhattacharyya distance ")
                                dblCost = MaxBhatta(arrSubEst, arrSubVar);
                        }
                        else if (cboMethod.Text == "MinMean")
                        {
                            if (cboMeasures.Text == "Class Separability")
                                dblCost = MeanSeperablility(arrSubEst, arrSubVar);
                            //else if (cboMeasures.Text == "Bhattacharyya distance ")
                            //dblCost = MaxBhatta(arrSubEst, arrSubVar);
                        }
                        else if (cboMethod.Text == "MinDistFromMean")
                        {
                            if (cboMeasures.Text == "Class Separability")
                                dblCost = SeperablilityFromMean(arrSubEst, arrSubVar);
                        }
                        else
                        {
                            MessageBox.Show("Minimizing variance within classes");
                            dblCost = CalSDfromArray(arrSubEst);
                        }

                        m_strClassificationMethod = "Minimize " + cboMethod.Text + " measures (" + cboMeasures.Text + ")";
                        //Adding Decision variable
                        m_pDecVar[intID] = m_pModel.AddVar(0, 1, 1, GRB.BINARY, strVarName);

                        //Objective Function
                        if (!blnMinMax)
                            pObjFunc.AddTerm(dblCost, m_pDecVar[intID]);
                        else
                            arrCosts[intID] = dblCost;
                        //else
                        //{
                        //    GRBLinExpr pMaxConst = new GRBLinExpr();
                        //    pMaxConst.AddTerm(dblCost, pDecVar[intID]);
                        //    GRBLinExpr pDesConst = new GRBLinExpr();
                        //    pDesConst.AddTerm(1, pDecVar[intID]);
                        //    string strMaxCon = "M" + i.ToString();
                        //    pModel.AddConstr(pDesConst, GRB.GREATER_EQUAL, pMaxConst, strMaxCon);
                        //}

                        //Total Step Constraints
                        m_pTotalStepsConst.AddTerm(1, m_pDecVar[intID]);

                        //OutFlow Constraints
                        pOutFlowComp.AddTerm(1, m_pDecVar[intID]);

                        //InFlow Constraints
                        listInFlows[j - 1].AddTerm(1, m_pDecVar[intID]);

                        intID++;
                    }

                    listOutFlows.Add(pOutFlowComp);
                }
                
                if (blnMinMax)
                {
                    m_pDecVar[intNDecVar] = m_pModel.AddVar(arrCosts.Min(), arrCosts.Max(), 1, GRB.CONTINUOUS, "z");
                    pObjFunc = new GRBLinExpr();
                    pObjFunc.AddTerm(1, m_pDecVar[intNDecVar]);
                }

                //Set Objective Function
                m_pModel.Update();
                m_pModel.SetObjective(pObjFunc, GRB.MINIMIZE);

                if (blnMinMax)
                {
                    GRBLinExpr pDesConst = new GRBLinExpr();
                    pDesConst.AddTerm(1, m_pDecVar[intNDecVar]);

                    for (int i = 0; i < intNDecVar; i++)
                    {
                        string strMaxCon = "M" + i.ToString();
                        
                        GRBLinExpr pMaxConst = new GRBLinExpr();
                        pMaxConst.AddTerm(arrCosts[i], m_pDecVar[i]);
                        
                        m_pModel.AddConstr(pDesConst, GRB.GREATER_EQUAL, pMaxConst, strMaxCon);
                    
                    }
                }

                for (int i = 1; i < intNFeatures - 1; i++)
                {
                    string strConstName = "N" + i.ToString();
                    m_pModel.AddConstr(listOutFlows[i], GRB.EQUAL, listInFlows[i - 1], strConstName);
                }

                //Add Start and End node constraints
                m_pModel.AddConstr(listOutFlows[0], GRB.EQUAL, 1, "StartNode");
                m_pModel.AddConstr(listInFlows[intNFeatures - 1], GRB.EQUAL, 1, "EndNode");

                //Add Total Steps Constraints
                m_pModel.AddConstr(m_pTotalStepsConst, GRB.EQUAL, intNClasses, "Nsteps");

                //Solving
                m_pModel.Optimize();

                //Add Results to CS array
                Cs[0] = m_arrEst[0]; //Estimate Array was sorted
                int intIdxCs = 0;

                if (m_pModel.Get(GRB.IntAttr.Status) == GRB.Status.OPTIMAL)
                {
                    for (int i = 0; i < intNDecVar; i++)
                    {
                        if (m_pDecVar[i].Get(GRB.DoubleAttr.X) == 1)
                        {
                            intIdxCs++;
                            string strName = m_pDecVar[i].Get(GRB.StringAttr.VarName);
                            int intIndexUBar = strName.IndexOf("_");
                            string strTo = strName.Substring(intIndexUBar + 1);
                            int intToValue = Convert.ToInt16(strTo);
                            Cs[intIdxCs] = m_arrEst[intToValue - 1]; //Closed
                            m_cbIdx[intIdxCs] = intToValue - 1;
                        }
                    }
                }

                //txtObjValue.Text = m_pModel.Get(GRB.DoubleAttr.ObjVal).ToString("N5");

                StringBuilder strObjectiveFunction = new StringBuilder();
                for (int i = 0; i < pObjFunc.Size; i++)
                {
                    strObjectiveFunction.Append(pObjFunc.GetCoeff(i).ToString("N2") + "*" + pObjFunc.GetVar(i).Get(GRB.StringAttr.VarName) + " + ");
                }

                string strObj = strObjectiveFunction.ToString();

                return Cs;
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return null;
            //}

        }

        private double[] ChangeNBreaksGurobi(GRBModel pModel, GRBLinExpr pTotalStepsConst, GRBVar[] pDecVar, int intNClasses, int intNFeatures)
        {
            try
            {
                double[] Cs = new double[intNClasses + 1];
                m_cbIdx = new int[intNClasses + 1]; // For Graph
                int intNDecVar = (intNFeatures * (intNFeatures + 1)) / 2; // Add L0_0

                pModel.Remove(pModel.GetConstrByName("Nsteps"));
                pModel.AddConstr(pTotalStepsConst, GRB.EQUAL, intNClasses, "Nsteps");

                //Solving
                pModel.Optimize();

                //Add Results to CS array
                Cs[0] = m_arrEst[0]; //Estimate Array was sorted
                int intIdxCs = 0;


                if (pModel.Get(GRB.IntAttr.Status) == GRB.Status.OPTIMAL)
                {
                    for (int i = 0; i < intNDecVar; i++)
                    {
                        if (pDecVar[i].Get(GRB.DoubleAttr.X) == 1)
                        {
                            intIdxCs++;
                            string strName = pDecVar[i].Get(GRB.StringAttr.VarName);
                            int intIndexUBar = strName.IndexOf("_");
                            string strTo = strName.Substring(intIndexUBar + 1);
                            int intToValue = Convert.ToInt16(strTo);
                            Cs[intIdxCs] = m_arrEst[intToValue - 1]; //Closed
                            m_cbIdx[intIdxCs] = intToValue - 1;
                        }
                    }
                }

                //txtObjValue.Text = pModel.Get(GRB.DoubleAttr.ObjVal).ToString("N5");

                return Cs;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
            }

        }

        #endregion

        #region Calculate Costs
        private double CalSDfromArray(double[] arrSubset)
        {
            double average = arrSubset.Average();
            double sumOfSquaresOfDifferences = arrSubset.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / arrSubset.Length);

            return sd;
        }
        private double MaxSeperablility(double[] arrSubEst, double[] arrSubVar)
        {
            double dblMaxSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblMaxSep;
            else
            {
                dblMaxSep = double.MinValue;
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblCL = Math.Abs(arrSubEst[n] - arrSubEst[m]) / (Math.Sqrt(Math.Pow(arrSubVar[n], 2) + Math.Pow(arrSubVar[m], 2)));
                        if (dblCL > dblMaxSep)
                            dblMaxSep = dblCL;
                    }
                }
                return dblMaxSep;
            }

        }
        private double SumSeperablility(double[] arrSubEst, double[] arrSubVar)
        {
            double dblSumSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblSumSep;
            else
            {
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblCL = Math.Abs(arrSubEst[n] - arrSubEst[m]) / (Math.Sqrt(Math.Pow(arrSubVar[n], 2) + Math.Pow(arrSubVar[m], 2)));
                        dblSumSep = dblSumSep + dblCL;
                    }
                }
                return dblSumSep;
            }

        }
        private double MeanSeperablility(double[] arrSubEst, double[] arrSubVar)
        {
            double dblMeanSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblMeanSep;
            else
            {
                int intCount = 0;
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblCL = Math.Abs(arrSubEst[n] - arrSubEst[m]) / (Math.Sqrt(Math.Pow(arrSubVar[n], 2) + Math.Pow(arrSubVar[m], 2)));
                        dblMeanSep = dblMeanSep + dblCL;
                        intCount++;
                    }
                }
                dblMeanSep = dblMeanSep / intCount;
                return dblMeanSep;
            }

        }
        private double MaxBhatta(double[] arrSubEst, double[] arrSubVar)
        {
            double dblMaxBhatt = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblMaxBhatt;
            else
            {
                dblMaxBhatt = double.MinValue;
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    double dblsquaredVar1 = Math.Pow(arrSubVar[m], 2);
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblsquaredVar2 = Math.Pow(arrSubVar[n], 2);
                        double dblVarComp = Math.Log(0.25 * ((dblsquaredVar1 / dblsquaredVar2) + (dblsquaredVar2 / dblsquaredVar1) + 2));
                        double dblMeanComp = Math.Pow(arrSubEst[m] - arrSubEst[n], 2) / (dblsquaredVar1 + dblsquaredVar2);
                        double dblBhatt = 0.25 * (dblVarComp + dblMeanComp);

                        if (dblBhatt > dblMaxBhatt)
                            dblMaxBhatt = dblBhatt;
                    }
                }
                return dblMaxBhatt;
            }

        }
        private double SumBhatta(double[] arrSubEst, double[] arrSubVar)
        {
            double dblSumBhatt = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblSumBhatt;
            else
            {
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    double dblsquaredVar1 = Math.Pow(arrSubVar[m], 2);
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblsquaredVar2 = Math.Pow(arrSubVar[n], 2);
                        double dblVarComp = Math.Log(0.25 * ((dblsquaredVar1 / dblsquaredVar2) + (dblsquaredVar2 / dblsquaredVar1) + 2));
                        double dblMeanComp = Math.Pow(arrSubEst[m] - arrSubEst[n], 2) / (dblsquaredVar1 + dblsquaredVar2);
                        double dblBhatt = 0.25 * (dblVarComp + dblMeanComp);

                        dblSumBhatt = dblSumBhatt + dblBhatt;
                    }
                }
                return dblSumBhatt;
            }

        }
        private double SeperablilityFromMean(double[] arrSubEst, double[] arrSubVar)
        {
            double average = arrSubEst.Average();
            double dblSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblSep;
            else
            {
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    //Sum of ABS
                    dblSep = dblSep + Math.Abs((average - arrSubEst[m]) / arrSubVar[m]);   
                }
                return dblSep;
            }

        }
        #endregion

        #region Actions
        //private void btnGraph_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmClassificationGraph pfrmClassfiGraph = new frmClassificationGraph();

        //        int intNfeature = m_arrEst.Length;
        //        double[,] adblValues = new double[intNfeature, 3];
        //        System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();
        //        pfrmClassfiGraph.nudConfidenceLevel.Value = 99;
        //        double dblConfidenceValue = Convert.ToDouble(pfrmClassfiGraph.nudConfidenceLevel.Value);
        //        double dblConInstance = pChart.DataManipulator.Statistics.InverseNormalDistribution(dblConfidenceValue / 100);

        //        for (int i = 0; i < intNfeature; i++)
        //        {
        //            double dblValue = m_arrEst[i];
        //            double dblUncern = dblConInstance * m_arrVar[i];
        //            if (dblValue < dblUncern)
        //            {
        //                adblValues[i, 0] = 0;
        //                adblValues[i, 1] = dblValue;
        //                adblValues[i, 2] = dblUncern;
        //            }
        //            else
        //            {
        //                adblValues[i, 0] = dblValue - dblUncern;
        //                adblValues[i, 1] = dblUncern;
        //                adblValues[i, 2] = dblUncern;
        //            }
        //        }

        //        AddStackedColumnSeries(pfrmClassfiGraph, "Low", Color.White, adblValues, 0, intNfeature);
        //        AddStackedColumnSeries(pfrmClassfiGraph, "Mean", Color.Gray, adblValues, 1, intNfeature);
        //        AddStackedColumnSeries(pfrmClassfiGraph, "High", Color.Gray, adblValues, 2, intNfeature);

        //        double dblMin = 0;
        //        double dblMax = m_arrEst.Max();

        //        for (int j = 1; j < m_cbIdx.Length - 1; j++)
        //        {
        //            AddVerticalLineSeries(pfrmClassfiGraph, "ver_" + j.ToString(), Color.Red, m_cbIdx[j] + 0.5, dblMin, dblMax);
        //        }

        //        pfrmClassfiGraph.pChart.ChartAreas[0].AxisX.IsStartedFromZero = true;
        //        pfrmClassfiGraph.pChart.ChartAreas[0].AxisY.Title = cboValueField.Text;

        //        pfrmClassfiGraph.arrEst = m_arrEst;
        //        pfrmClassfiGraph.arrVar = m_arrVar;
        //        pfrmClassfiGraph.cbIdx = m_cbIdx;
        //        pfrmClassfiGraph.Cs = m_cb;
        //        pfrmClassfiGraph.nudGCNClasses.Value = nudGCNClasses.Value;
        //        pfrmClassfiGraph.pModel = m_pModel;
        //        pfrmClassfiGraph.pDecVar = m_pDecVar;
        //        pfrmClassfiGraph.pTotalStepsConst = m_pTotalStepsConst;
        //        pfrmClassfiGraph.lblMethod.Text = m_strClassificationMethod;
        //        pfrmClassfiGraph.txtObjValue.Text = txtObjValue.Text;
        //        pfrmClassfiGraph.strValueFldName = cboValueField.Text;
        //        pfrmClassfiGraph.strUncernFldName = cboUncernFld.Text;
        //        pfrmClassfiGraph.m_pForm = mForm;
        //        pfrmClassfiGraph.m_pActiveView = m_pActiveView;
        //        pfrmClassfiGraph.m_pFLayer = m_pFLayer;
        //        pfrmClassfiGraph.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, cboSourceLayer.Text);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;
                m_intNofFeatures = m_pFClass.FeatureCount(null);

                IFields fields = m_pFClass.Fields;


                cboValueField.Items.Clear();
                cboUncernFld.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                    cboUncernFld.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void cboUncernFld_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView(1);
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
                return;
            else
            {
                DrawUncertaintySymbols();
                AddClassResultsSeries(1);
            }
        }
        private void nudGCNClasses_ValueChanged(object sender, EventArgs e)
        {
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
                return;
            else
            {
                AddClassResultsSeries(2);
            }
        }

        private void cboValueField_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView(1);
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
                return;
            else
                DrawUncertaintySymbols();
        }
        private void nudConfidenceLevel_ValueChanged(object sender, EventArgs e)
        {
            //if (cboValueField.Text == "" || cboUncernFld.Text == "")
            //    return;
            //else
            //    DrawUncertaintySymbols();
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
                return;
            else
            {
                DrawUncertaintySymbols();
                AddClassResultsSeries(1);
            }
        }
        private void cboMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView(1);
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
                return;
            else
            {
                AddClassResultsSeries(1);
            }
        }

        private void cboMeasures_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DrawSymbolinListView(1);
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
                return;
            else
            {
                AddClassResultsSeries(1);
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
                    pColorRamps.intLoadingPlaces = 3;
                    pColorRamps.Show();
                }
            }
            else
                DrawSymbolinChartwithCb(m_cb, m_intGCBreakeCount);
        }


        private void picGCLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGCLineColor.BackColor = cdColor.Color;
        }
        #endregion

        

        //private void AddStackedColumnSeries(frmClassificationGraph pClassGraph, string strSeriesName, System.Drawing.Color FillColor,  double[,] adblValues, int intStats, int intNfeatures)
        //{
        //    try
        //    {
        //        var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
        //        {
        //            Name = strSeriesName,
        //            Color = FillColor,
        //            BorderColor = Color.Black,
        //            IsVisibleInLegend = false,
        //            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn,

        //        };

        //        pClassGraph.pChart.Series.Add(pSeries);

        //        for (int j = 0; j < intNfeatures; j++)
        //        {
        //            pSeries.Points.AddXY(j, adblValues[j, intStats]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }
        //}
        //private void AddVerticalLineSeries(frmClassificationGraph pClassGraph, string strSeriesName, System.Drawing.Color FillColor, double dblX, double dblYMin, double dblYMax)
        //{
        //    try
        //    {
        //        var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
        //        {
        //            Name = strSeriesName,
        //            Color = FillColor,
        //            BorderColor = Color.Black,
        //            IsVisibleInLegend = false,
        //            ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

        //        };

        //        pClassGraph.pChart.Series.Add(pSeries);

        //        pSeries.Points.AddXY(dblX, dblYMin);
        //        pSeries.Points.AddXY(dblX, dblYMax);
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return;
        //    }

        //}
        private void AddUncertaintyRangeLineSeries(frmOptimizationSample pOptSample, string strSeriesName, System.Drawing.Color FillColor, double dblY, double dblXMin, double dblXMax)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    //For AAG
                    //Color = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                    //BorderWidth = 2, //For AAG

                };

                pOptSample.pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblXMin, dblY);
                pSeries.Points.AddXY(dblXMax, dblY);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private void AddVerEdgeLineSeries(frmOptimizationSample pOptSample, string strSeriesName, System.Drawing.Color FillColor, double dblY, double dblX, double dblScaleY)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    //For AAG
                    //Color = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                    //BorderWidth = 2, //For AAG

                };

                pOptSample.pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblX, dblY - (dblScaleY * 0.4));
                pSeries.Points.AddXY(dblX, dblY + (dblScaleY * 0.4));
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
       

        public void DrawUncertaintySymbols()
        {

            this.pChart.Series.Clear();

            string strGCRenderField = cboValueField.Text;
            string strUncernfld = cboUncernFld.Text;
           
            int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
            int intUncernfldIdx = m_pFClass.FindField(strUncernfld);

            if (intUncernfldIdx == -1 || intValueFldIdx == -1 || cboMethod.Text == "")
                return;

            ITable pTable = (ITable)m_pFClass;

            ITableSort pTableSort = new TableSort();
            pTableSort.Table = pTable;
            ICursor pCursor = (ICursor)m_pFClass.Search(null, false);

            pTableSort.Cursor = pCursor as ICursor;

            pTableSort.Fields = strGCRenderField;
            pTableSort.set_Ascending(strGCRenderField, true);

            // call the sort
            pTableSort.Sort(null);

            // retrieve the sorted rows
            IFeatureCursor pSortedCursor = pTableSort.Rows as IFeatureCursor;

            int intNfeature = pTable.RowCount(null);

            double[][] adblValues = new double[3][];
            adblValues[0] = new double[intNfeature];
            adblValues[1] = new double[intNfeature];
            adblValues[2] = new double[intNfeature];

            System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();
            double dblConfidenceValue = Convert.ToDouble(this.nudConfidenceLevel.Value);
            double dblConInstance = pChart.DataManipulator.Statistics.InverseNormalDistribution(dblConfidenceValue / 100);

            IRow pRow = pSortedCursor.NextFeature();

            int i = 0;
            double dblScaledY = 100 / Convert.ToDouble(intNfeature);
            while (pRow != null)
            {
                double dblValue = Convert.ToDouble(pRow.get_Value(intValueFldIdx));
                double dblUncern = dblConInstance * Convert.ToDouble(pRow.get_Value(intUncernfldIdx));
                adblValues[0][i] = dblValue - dblUncern;
                adblValues[1][i] = dblValue;
                adblValues[2][i] = dblValue + dblUncern;

                AddUncertaintyRangeLineSeries(this, "Range_" + i.ToString(), Color.Black, (i * dblScaledY) + m_intAreaChartHeight, adblValues[0][i], adblValues[2][i]);
                AddVerEdgeLineSeries(this, "LVert_" + i.ToString(), Color.Black, (i * dblScaledY) + m_intAreaChartHeight, adblValues[0][i], dblScaledY);
                AddVerEdgeLineSeries(this, "UVert_" + i.ToString(), Color.Black, (i * dblScaledY) + m_intAreaChartHeight, adblValues[2][i], dblScaledY);
                i++;
                pRow = pSortedCursor.NextFeature();
            }
            System.Drawing.Color pMarkerColor = System.Drawing.Color.Blue;
            //System.Drawing.Color pMarkerColor = System.Drawing.Color.FromArgb(217,217,217); 
            var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Points",
                Color = pMarkerColor,
                BorderColor = pMarkerColor,
                IsVisibleInLegend = false,
                IsXValueIndexed = false,
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                //MarkerSize = 2,
                MarkerSize = 4, 
                MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle

            };

            this.pChart.Series.Add(seriesPts);

            for (int j = 0; j < intNfeature; j++)
            {
                seriesPts.Points.AddXY(adblValues[1][j], (j * dblScaledY) + m_intAreaChartHeight);
            }

            m_dblChartMin = adblValues[0].Min();
            m_dblChartMax = adblValues[2].Max();
            double dblSpacing = (m_dblChartMax - m_dblChartMin) / 50;
            //this.pChart.ChartAreas[0].AxisY.Maximum = intNfeature + intAreaChartHeight;
            this.pChart.ChartAreas[0].AxisY.Maximum = 100 + m_intAreaChartHeight;
            this.pChart.ChartAreas[0].AxisY.Minimum = -3;
            this.pChart.ChartAreas[0].AxisY.Title = "Sorted Observations";

            this.pChart.ChartAreas[0].AxisX.Minimum = m_dblChartMin - dblSpacing;
            this.pChart.ChartAreas[0].AxisX.Maximum = m_dblChartMax + dblSpacing;

            this.pChart.ChartAreas[0].AxisX.Title = strGCRenderField;


            m_intPtsIdx = (intNfeature * 3);
            m_intTotalNSeries = this.pChart.Series.Count;
        }


        public void AddClassResultsSeries(int intType)
        {
            try
            {

            pChart.ChartAreas[0].AxisX.CustomLabels.Clear();
            int intNFeatureCount = m_pFClass.FeatureCount(null);

            //Clear previous selection
            string strPointSeriesName = pChart.Series[m_intPtsIdx].Name;
            int intLastSeriesIdx = pChart.Series.Count - 1;

            //Remove Previous Selection
            while (pChart.Series[intLastSeriesIdx].Name != strPointSeriesName)
            {
                pChart.Series.RemoveAt(intLastSeriesIdx);
                intLastSeriesIdx = pChart.Series.Count - 1;
            }

                string strGCRenderField = cboValueField.Text;
                string strUncernfld = cboUncernFld.Text;
                m_intGCBreakeCount = Convert.ToInt32(nudGCNClasses.Value);


                int intValueFldIdx = m_pFClass.FindField(strGCRenderField);
                int intUncernfldIdx = m_pFClass.FindField(strUncernfld);

                if (intUncernfldIdx == -1 || intValueFldIdx == -1 || cboMethod.Text == "")
                    return;

                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Processing:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();
                pfrmProgress.Refresh();

                ITable pTable = (ITable)m_pFClass;

                ITableSort pTableSort = new TableSort();
                pTableSort.Table = pTable;
                ICursor pCursor = (ICursor)m_pFClass.Search(null, false);

                pTableSort.Cursor = pCursor as ICursor;

                pTableSort.Fields = strGCRenderField;
                pTableSort.set_Ascending(strGCRenderField, true);

                // call the sort
                pTableSort.Sort(null);

                // retrieve the sorted rows
                IFeatureCursor pSortedCursor = pTableSort.Rows as IFeatureCursor;

                //Get Optimized result
                if (intType == 1)
                    m_cb = OptUsingGurobi(m_intGCBreakeCount, m_intNofFeatures, pSortedCursor, intValueFldIdx, intUncernfldIdx);
                else if (intType == 2)
                    m_cb = ChangeNBreaksGurobi(m_pModel, m_pTotalStepsConst, m_pDecVar, m_intGCBreakeCount, m_intNofFeatures);

                //m_cb = new double[7] { 22176, 33587, 40299, 47191, 56338, 69092, 86597 }; // NB 
                //m_cb = new double[7] { 22176, 22176,23350,25906,27627,29293, 86597 }; // CS 
                //m_cb = new double[7] { 22176, 36890, 41630, 46391, 51794, 62412, 86597 }; //SSS
                //m_cb = new double[7] { 22176, 49533, 53382, 53822, 63424, 74662, 86597 }; //SMS
                //m_cb = new double[7] { 22176, 36890, 41541, 45938, 51190, 61459, 86597 }; //MSS
                //m_cb = new double[7] { 22176, 38668, 44898, 52692, 65625, 73333, 86597 }; //MMS

                //m_cb = new double[7] { 22176, 36696, 42247, 47546, 54434, 66966, 86597 }; //SSB
                //m_cb = new double[7] { 22176, 34952, 47631, 52451, 66966, 81563, 86597 }; //SMB
                //m_cb = new double[7] { 22176, 36563, 42102, 46969, 53382, 65132, 86597 }; //MSB
                //m_cb = new double[7] { 22176, 35692, 45769, 51115, 61898, 74320, 86597 }; //MMB
                //m_cb = new double[5] { 11635.11,59145,87949,105767,193778.96 }; //Rockall

                m_pRender = new ClassBreaksRenderer();

                m_pRender.Field = strGCRenderField;
                m_pRender.BreakCount = m_intGCBreakeCount;
                m_pRender.MinimumBreak = m_cb[0];

                string strColorRamp = cboColorRamp.Text;

                m_pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, m_intGCBreakeCount);
                m_pEnumColors.Reset();

                m_arrColors = new int[m_intGCBreakeCount, 3];

                for (int k = 0; k < m_intGCBreakeCount; k++)
                {
                    IColor pColor = m_pEnumColors.Next();
                    IRgbColor pRGBColor = new RgbColorClass();
                    pRGBColor.RGB = pColor.RGB;

                    m_arrColors[k, 0] = pRGBColor.Red;
                    m_arrColors[k, 1] = pRGBColor.Green;
                    m_arrColors[k, 2] = pRGBColor.Blue;
                }
                
                ////For AAG Figures
                //m_arrColors[0, 0] = 247; m_arrColors[0, 1] = 247;  m_arrColors[0, 2] = 247;
                //m_arrColors[1, 0] = 217; m_arrColors[1, 1] = 217; m_arrColors[1, 2] = 217;
                //m_arrColors[2, 0] = 189; m_arrColors[2, 1] = 189; m_arrColors[2, 2] = 189;
                //m_arrColors[3, 0] = 150; m_arrColors[3, 1] = 150; m_arrColors[3, 2] = 150;
                //m_arrColors[4, 0] = 99; m_arrColors[4, 1] = 99; m_arrColors[4, 2] = 99;
                //m_arrColors[5, 0] = 38; m_arrColors[5, 1] = 38; m_arrColors[5, 2] = 38; 


                m_pEnumColors.Reset();

                for (int j = 0; j < m_cb.Length - 1; j++)
                {
                    if (j == 0)
                        AddAreaChartSeries(this, "area_" + j.ToString(), Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), m_dblChartMin, m_cb[j + 1], m_intAreaChartHeight - 2);
                    else if (j == m_cb.Length - 2)
                        AddAreaChartSeries(this, "area_" + j.ToString(), Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), m_cb[j], m_dblChartMax, m_intAreaChartHeight - 2);
                    else
                        AddAreaChartSeries(this, "area_" + j.ToString(), Color.FromArgb(m_arrColors[j, 0], m_arrColors[j, 1], m_arrColors[j, 2]), m_cb[j], m_cb[j + 1], m_intAreaChartHeight - 2);

                }

                for (int j = 1; j < m_cb.Length - 1; j++)
                {
                    AddVerticalLineSeries2(this, "ver_" + j.ToString(), Color.Red, m_cb[j], -3, m_intAreaChartHeight + 100);
                }

            // For testing 080516 HK
            //    double[] m_sss = new double[7] { 22176, 36890, 41630, 46391, 51794, 62412, 86597 };
            //    double[] m_sms = new double[7] { 22176, 49533, 53382, 53822, 63424, 74662, 86597 };
            //    double[] m_mss = new double[7] { 22176, 36890, 41541, 45938, 51190, 61459, 86597 };
            ////m_cb = new double[7] { 4.05931377411, 4.544580, 4.794135, 5.032068, 5.265394, 5.543087, 6.1233625412 }; // For Test 

                //for (int j = 1; j < m_sss.Length - 1; j++)
                //{
                //    AddVerticalLineSeries2(this, "sss_" + j.ToString(), Color.Blue, m_sss[j], -3, m_intAreaChartHeight + 100);
                //    AddVerticalLineSeries2(this, "sms_" + j.ToString(), Color.Green, m_sms[j], -3, m_intAreaChartHeight + 100);
                //    AddVerticalLineSeries2(this, "mss_" + j.ToString(), Color.Purple, m_mss[j], -3, m_intAreaChartHeight + 100);
                //}

                this.pChart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                for (int n = 0; n < m_cb.Length+2; n++)
                {
                    double dblIntervals = 0;

                    if (n == 0)
                        dblIntervals = (m_cb[0] - m_dblChartMin)/2;
                    else if (n == m_cb.Length + 1)
                        dblIntervals = (m_dblChartMax - m_cb[m_cb.Length - 1])/2;
                    else
                    {
                        int j = n - 1;
                        double dblSelVal = m_cb[j];
                        double dblUpVal = 0;
                        if (j == m_cb.Length - 1)
                            dblUpVal = m_dblChartMax;
                        else
                            dblUpVal = m_cb[j + 1];

                        double dblLoVal = 0;
                        if (j == 0)
                            dblLoVal = m_dblChartMin;
                        else
                            dblLoVal = m_cb[j - 1];

                        if (dblUpVal - dblSelVal > dblSelVal - dblLoVal)
                            dblIntervals = (dblSelVal - dblLoVal) / 2;
                        else
                            dblIntervals = (dblUpVal - dblSelVal) / 2;
                    }

                    System.Windows.Forms.DataVisualization.Charting.CustomLabel pcutsomLabel = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();

                    if (n == 0)
                    {
                        pcutsomLabel.FromPosition = m_dblChartMin - dblIntervals;
                        pcutsomLabel.ToPosition = m_dblChartMin + dblIntervals;
                        pcutsomLabel.Text = Math.Round(m_dblChartMin, m_intDecimalPlace).ToString();
                    }
                    else if (n == m_cb.Length + 1)
                    {
                        pcutsomLabel.FromPosition = m_dblChartMax - dblIntervals;
                        pcutsomLabel.ToPosition = m_dblChartMax + dblIntervals;
                        pcutsomLabel.Text = Math.Round(m_dblChartMax, m_intDecimalPlace).ToString();
                    }
                    else
                    {

                        int j = n - 1;
                        pcutsomLabel.FromPosition = m_cb[j] - dblIntervals;
                        pcutsomLabel.ToPosition = m_cb[j] + dblIntervals;
                        ////For AAG
                        //m_intDecimalPlace = 0;
                        //if (j == 0)
                        //{
                        //    pcutsomLabel.FromPosition = m_cb[j];
                        //    pcutsomLabel.ToPosition = m_cb[j] + 10000;
                        //}
                        //else if(j==m_cb.Length-1)
                        //{
                        //    pcutsomLabel.FromPosition = m_cb[j] - 12000;
                        //    pcutsomLabel.ToPosition = m_cb[j];

                        //}

                        pcutsomLabel.Text = Math.Round(m_cb[j], m_intDecimalPlace).ToString();
                    }

                    this.pChart.ChartAreas[0].AxisX.CustomLabels.Add(pcutsomLabel);

                }

                ////For AAG
                //this.pChart.ChartAreas[0].AxisX.LineWidth = 3;
                //this.pChart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12);
                //this.pChart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10);
                //this.pChart.ChartAreas[0].AxisY.LineWidth = 4;
                //this.pChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 12);
                //this.pChart.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial", 10);
                //this.pChart.ChartAreas[0].AxisX.CustomLabels.Clear();
               
                
                
                m_arrEvalStat = EvalStat(m_pFClass, m_cb);

                m_intPtsIdx = (intNFeatureCount * 3);
                m_intTotalNSeries = this.pChart.Series.Count;

            
                pfrmProgress.Close();

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void AddVerticalLineSeries2(frmOptimizationSample pfrmOptSamp, string strSeriesName, System.Drawing.Color FillColor, double dblX, double dblYMin, double dblYMax)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    //Color = Color.Black,
                    BorderColor = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,
                    //BorderWidth = 2 // For AAG
                    

                };

                pfrmOptSamp.pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblX, dblYMin);
                pSeries.Points.AddXY(dblX, dblYMax);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private void AddAreaChartSeries(frmOptimizationSample pfrmOptSamp, string strSeriesName, System.Drawing.Color FillColor, double dblMinX, double dblMaxX, double dblY)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = FillColor,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area,
                    IsXValueIndexed = false,
                    
                };

                //pSeries["PixelPointWidth"] = "6";
                pfrmOptSamp.pChart.Series.Add(pSeries);
                pSeries.Points.AddXY(dblMinX, dblY);
                pSeries.Points.AddXY(dblMaxX, dblY);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        #region Private Functions
        private double[,] EvalStat(IFeatureClass pFClass, double[] cb)
        {

            string strValueFld = cboValueField.Text;
            string strUncernfld = cboUncernFld.Text;

            if (strUncernfld == string.Empty || strValueFld == string.Empty)
                return null;

            int intValueIdx = pFClass.FindField(strValueFld);
            int intUncernIdx = pFClass.FindField(strUncernfld);

            int intClassCount = cb.Length - 1;

            double[,] arrEvalStat = new double[intClassCount + 2, 5];

            //string strTempfldName = txtFldName.Text;
            //strTempfldName = "MinSepfave";

            int intFCounts = pFClass.FeatureCount(null);
            Chart pChart = new Chart();



            IFeatureCursor pFCursor = null;
            pFCursor = pFClass.Search(null, false);

            IQueryFilter pQfilter = new QueryFilterClass();
            for (int j = 0; j < intClassCount; j++)
            {

                if (j == 0)
                    pQfilter.WhereClause = strValueFld + " >= " + cb[j].ToString() + " AND " + strValueFld + " <= " + cb[j + 1].ToString();
                else
                    pQfilter.WhereClause = strValueFld + " > " + cb[j].ToString() + " AND " + strValueFld + " <= " + cb[j + 1].ToString();

                pFCursor = pFClass.Search(pQfilter, true);
                IFeatureCursor pSortedCursor = SortData(pFCursor, pFClass, pQfilter, strValueFld);
                IFeature pFeat = null;
                pFeat = pSortedCursor.NextFeature();
                int intSelCnt = pFClass.FeatureCount(pQfilter);
                int i = 0;
                double[] arrSubEst = new double[intSelCnt];
                double[] arrSubVar = new double[intSelCnt];

                while (pFeat != null)
                {
                    arrSubEst[i] = Convert.ToDouble(pFeat.get_Value(intValueIdx));
                    arrSubVar[i] = Convert.ToDouble(pFeat.get_Value(intUncernIdx));
                    i++;
                    pFeat = pSortedCursor.NextFeature();
                }
                pSortedCursor.Flush();

                double dblSS = SumSeperablility(arrSubEst, arrSubVar);
                double dblSB = SumBhatta(arrSubEst, arrSubVar);
                double dblMS = MaxSeperablility(arrSubEst, arrSubVar);
                double dblMB = MaxBhatta(arrSubEst, arrSubVar);

                arrEvalStat[j, 0] = intSelCnt;
                arrEvalStat[j, 1] = dblSS;
                arrEvalStat[j, 2] = dblSB;
                arrEvalStat[j, 3] = dblMS;
                arrEvalStat[j, 4] = dblMB;

                arrEvalStat[intClassCount, 0] += intSelCnt;
                arrEvalStat[intClassCount, 1] += dblSS;
                arrEvalStat[intClassCount, 2] += dblSB;
                arrEvalStat[intClassCount, 3] += dblMS;
                arrEvalStat[intClassCount, 4] += dblMB;

                if (intSelCnt > arrEvalStat[intClassCount + 1, 0])
                    arrEvalStat[intClassCount + 1, 0] = intSelCnt;
                if (dblSS > arrEvalStat[intClassCount + 1, 1])
                    arrEvalStat[intClassCount + 1, 1] = dblSS;
                if (dblSB > arrEvalStat[intClassCount + 1, 2])
                    arrEvalStat[intClassCount + 1, 2] = dblSB;
                if (dblMS > arrEvalStat[intClassCount + 1, 3])
                    arrEvalStat[intClassCount + 1, 3] = dblMS;
                if (dblMB > arrEvalStat[intClassCount + 1, 4])
                    arrEvalStat[intClassCount + 1, 4] = dblMB;
            }

            return arrEvalStat;
            //UpdateRangeRobustness(lvSymbol, intClassCount, cb, dblClsMean, dblMeanRobustness, arrEvalStat);



        }
        private IFeatureCursor SortData(IFeatureCursor pCursor, IFeatureClass FClass, IQueryFilter pQfilter, string strValueFld)
        {
            //try
            //{
            // sort in descending by value
            ITable pTable = null;
            pTable = FClass as ITable;

            ITableSort pTableSort = null;
            pTableSort = new TableSort();
            pTableSort.Table = pTable;
            pTableSort.Cursor = pCursor as ICursor;

            //set up the query filter.
            IQueryFilter pQF = null;
            pQF = new QueryFilter();
            pQF.SubFields = "*";
            pQF.WhereClause = pQfilter.WhereClause;
            pTableSort.QueryFilter = pQF;

            pTableSort.Fields = strValueFld;
            pTableSort.set_Ascending(strValueFld, true);

            // call the sort
            pTableSort.Sort(null);

            // retrieve the sorted rows
            IFeatureCursor pSortedCursor = null;
            pSortedCursor = pTableSort.Rows as IFeatureCursor;

            return pSortedCursor;
            //}
            //catch (Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine("Sorting Error: " + e.Message);
            //    return null;
            //}
        }
        #endregion

        #region Mouse Actions on Chart
        private void pChart_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _canDraw = false;
                return;
            }

            System.Windows.Forms.DataVisualization.Charting.HitTestResult result = pChart.HitTest(e.X, e.Y);

            if (result.Series == null)
            {
                try
                {
                    for (int i = m_intPtsIdx + 1; i < m_intPtsIdx + m_cb.Length; i++)
                    {
                        pChart.Series[i].BorderColor = pChart.Series[i].Color;
                    }

                    m_blnAreaBrushing = false;
                    _canDraw = true;
                    _startX = e.X;
                    _startY = e.Y;
                    return;
                }
                catch
                {
                    return;
                }
            }


            if (result.Series.ChartType == System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area)
            {
                System.Windows.Forms.DataVisualization.Charting.Series pSelSeries = result.Series;

                for (int i = m_intPtsIdx + 1; i < m_intPtsIdx + m_cb.Length; i++)
                {
                    if (pSelSeries == pChart.Series[i])
                    {
                        m_blnAreaBrushing = true;
                        _canDraw = true;
                        _startX = e.X;
                        _startY = e.Y;

                        ////Not used 030216 HK
                        //pChart.Series[i].BorderColor = Color.Cyan;
                        //frmOptimizationSelResults pResult = new frmOptimizationSelResults();
                        //string[] strResult = new string[5];
                        //int intGroupIdx = i - (m_intPtsIdx + 1);
                        //strResult[0] = "Count: " + m_arrEvalStat[intGroupIdx, 0].ToString();
                        //strResult[1] = "Sum Separability: " + m_arrEvalStat[intGroupIdx, 1].ToString();
                        //strResult[2] = "Sum Bhatta: " + m_arrEvalStat[intGroupIdx, 2].ToString();
                        //strResult[3] = "Max Separability: " + m_arrEvalStat[intGroupIdx, 3].ToString();
                        //strResult[4] = "Max Bhatta: " + m_arrEvalStat[intGroupIdx, 4].ToString();
                        //pResult.txtResults.Lines = strResult;
                        //pResult.Show();
                    }
                    else
                        pChart.Series[i].BorderColor = pChart.Series[i].Color;
                }
            }
            else
            {
                for (int i = m_intPtsIdx + 1; i < m_intPtsIdx + m_cb.Length; i++)
                {
                    pChart.Series[i].BorderColor = pChart.Series[i].Color;
                }

                m_blnAreaBrushing = false;
                _canDraw = true;
                _startX = e.X;
                _startY = e.Y;
            }
        }

        private void pChart_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_canDraw) return;

                int x = Math.Min(_startX, e.X);
                int y = Math.Min(_startY, e.Y);

                int width = Math.Max(_startX, e.X) - Math.Min(_startX, e.X);

                int height = Math.Max(_startY, e.Y) - Math.Min(_startY, e.Y);
                _rect = new Rectangle(x, y, width, height);
                Refresh();

                Pen pen = new Pen(Color.Cyan, 1);
                pGraphics = pChart.CreateGraphics();
                pGraphics.DrawRectangle(pen, _rect);
                pGraphics.Dispose();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void pChart_MouseUp(object sender, MouseEventArgs e)
        {
            //Export the chart to an image file
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                conMenu.Show(pChart, e.X, e.Y);
                return;
            }

            _canDraw = false;

            //int intLastSeriesIdx = pChart.Series.Count - 1;
            if (pChart.Series.Count == 0)
                return;

            //Remove Previous Selection
            while (pChart.Series.Count != m_intTotalNSeries)
                pChart.Series.RemoveAt(pChart.Series.Count - 1);


            if (m_blnAreaBrushing)
            {
                //Create DataTable to store Result
                System.Data.DataTable tblResult = new DataTable("Evaluation");

                //Assign DataTable
                DataColumn dNoClass = new DataColumn();
                dNoClass.DataType = System.Type.GetType("System.String");
                dNoClass.ColumnName = "NoClass";
                tblResult.Columns.Add(dNoClass);
                
                DataColumn dColCount = new DataColumn();
                dColCount.DataType = System.Type.GetType("System.Int32");
                dColCount.ColumnName = "Count";
                tblResult.Columns.Add(dColCount);

                DataColumn dColSumSep = new DataColumn();
                dColSumSep.DataType = System.Type.GetType("System.Double");
                dColSumSep.ColumnName = "SumSep";
                tblResult.Columns.Add(dColSumSep);

                DataColumn dColSumBha = new DataColumn();
                dColSumBha.DataType = System.Type.GetType("System.Double");
                dColSumBha.ColumnName = "SumBha";
                tblResult.Columns.Add(dColSumBha);

                DataColumn dColMaxSep = new DataColumn();
                dColMaxSep.DataType = System.Type.GetType("System.Double");
                dColMaxSep.ColumnName = "MaxSep";
                tblResult.Columns.Add(dColMaxSep);

                DataColumn dColMaxBha = new DataColumn();
                dColMaxBha.DataType = System.Type.GetType("System.Double");
                dColMaxBha.ColumnName = "MaxBha";
                tblResult.Columns.Add(dColMaxBha);

                ////Assign Datagridview to Data Table
                //pfrmRegResult.dgvResults.DataSource = tblRegResult;

                int intSelCnt = 0;
                for (int i = m_intPtsIdx + 1; i < m_intPtsIdx + m_cb.Length; i++)
                {

                    //double dblX1 = pChart.Series[i].Points[0].XValue;
                    //double dblX2 = pChart.Series[i].Points[1].XValue;
                    int intX1 = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[i].Points[0].XValue);
                    int intX2 = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(pChart.Series[i].Points[1].XValue);
                    int intY1 = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(0);
                    int intY2 = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(m_intAreaChartHeight - 2);
                    int intSTX = 0, intSTY = 0, intWidth = 0, intHeight = 0;

                    if (intX1 > intX2)
                    {
                        intSTX = intX2;
                        intWidth = intX1 - intX2;
                    }
                    else
                    {
                        intSTX = intX1;
                        intWidth = intX2 - intX1;
                    }

                    if (intY1 > intY2)
                    {
                        intSTY = intY2;
                        intHeight = intY1 - intY2;
                    }
                    else
                    {
                        intSTY = intY1;
                        intHeight = intY2 - intY1;
                    }
                    System.Drawing.Rectangle pRec = new Rectangle(intSTX, intSTY, intWidth, intHeight);

                    if (_rect.IntersectsWith(pRec))
                    {
                        pChart.Series[i].BorderColor = Color.Cyan;

                        intSelCnt++;
                        int intGroupIdx = i - (m_intPtsIdx + 1);

                        DataRow pDataRow = tblResult.NewRow();
                        pDataRow["NoClass"] = (intGroupIdx + 1).ToString();
                        pDataRow["Count"] = Math.Round(m_arrEvalStat[intGroupIdx, 0], m_intDecimalPlace);
                        pDataRow["SumSep"] = Math.Round(m_arrEvalStat[intGroupIdx, 1], m_intDecimalPlace);
                        pDataRow["SumBha"] = Math.Round(m_arrEvalStat[intGroupIdx, 2], m_intDecimalPlace);
                        pDataRow["MaxSep"] = Math.Round(m_arrEvalStat[intGroupIdx, 3], m_intDecimalPlace);
                        pDataRow["MaxBha"] = Math.Round(m_arrEvalStat[intGroupIdx, 4], m_intDecimalPlace);
                        tblResult.Rows.Add(pDataRow);
                    }

                }

                for (int i = 0; i < 2; i++)
                {
                    int intGroupIdx = i + (m_cb.Length-1);

                    DataRow pDataRow = tblResult.NewRow();
                    if (i == 0)
                        pDataRow["NoClass"] = "SUM";
                    else
                        pDataRow["NoClass"] = "MAX";

                    pDataRow["Count"] = Math.Round(m_arrEvalStat[intGroupIdx, 0], m_intDecimalPlace);
                    pDataRow["SumSep"] = Math.Round(m_arrEvalStat[intGroupIdx, 1], m_intDecimalPlace);
                    pDataRow["SumBha"] = Math.Round(m_arrEvalStat[intGroupIdx, 2], m_intDecimalPlace);
                    pDataRow["MaxSep"] = Math.Round(m_arrEvalStat[intGroupIdx, 3], m_intDecimalPlace);
                    pDataRow["MaxBha"] = Math.Round(m_arrEvalStat[intGroupIdx, 4], m_intDecimalPlace);
                    tblResult.Rows.Add(pDataRow);
                }

                if (intSelCnt > 0)
                {
                    frmOptimizationSelResults pfrmOptSelResult = System.Windows.Forms.Application.OpenForms["frmOptimizationSelResults"] as frmOptimizationSelResults;
                    if(pfrmOptSelResult == null)
                        pfrmOptSelResult = new frmOptimizationSelResults();
                    
                    pfrmOptSelResult.dgvEvalResults.DataSource = tblResult;
                    pfrmOptSelResult.Show();
                }
            }
            else
            {
                int dblOriPtsSize = pChart.Series[m_intPtsIdx].MarkerSize;

                System.Drawing.Color pMarkerColor = System.Drawing.Color.Cyan;
                var seriesPts = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = "SelPoints",
                    Color = pMarkerColor,
                    BorderColor = pMarkerColor,
                    IsVisibleInLegend = false,
                    IsXValueIndexed = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle,
                    MarkerSize = dblOriPtsSize * 2

                };

                pChart.Series.Add(seriesPts);

                StringBuilder plotCommmand = new StringBuilder();

                for (int i = 0; i < pChart.Series[m_intPtsIdx].Points.Count; i++)
                {
                    double dblXChartValue = pChart.Series[m_intPtsIdx].Points[i].XValue;
                    double dblYChartValue = pChart.Series[m_intPtsIdx].Points[i].YValues[0];
                    int intX = (int)pChart.ChartAreas[0].AxisX.ValueToPixelPosition(dblXChartValue);
                    int intY = (int)pChart.ChartAreas[0].AxisY.ValueToPixelPosition(dblYChartValue);

                    System.Drawing.Point SelPts = new System.Drawing.Point(intX, intY);


                    if (_rect.Contains(SelPts))
                    {
                        seriesPts.Points.AddXY(dblXChartValue, dblYChartValue);
                        int k = (Convert.ToInt32(dblXChartValue) - 1) / 2;
                        plotCommmand.Append(cboValueField.Text + " = " + dblXChartValue.ToString() + " Or ");
                    }
                }

                //Brushing on ArcView
                if (plotCommmand.Length > 3)
                {
                    plotCommmand.Remove(plotCommmand.Length - 3, 3);
                    string whereClause = plotCommmand.ToString();
                    m_pBL.FeatureSelectionOnActiveView(whereClause, m_pActiveView, m_pFLayer);
                }
                else
                {
                    IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                    m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
                //Brushing to other graphs //The Function should be locatated after MapView Brushing
                m_pBL.BrushingToOthers(m_pFLayer, this.Handle);
            }

        }
        #endregion

        private void cboAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboValueField.Text == "" || cboUncernFld.Text == "")
            {
                MessageBox.Show("Please select value field");
                return;
            }
            else
                DrawSymbolinChartwithCb(m_cb, m_intGCBreakeCount);

        }

        private void exportToImageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExportChart pfrmExportChart = new frmExportChart();
            pfrmExportChart.thisChart = pChart;
            pfrmExportChart.nudHeight.Value = pChart.Height;
            pfrmExportChart.nudHeight.Maximum = pChart.Height * 5; //Restriction of maximum size
            pfrmExportChart.nudWidth.Value = pChart.Width;
            pfrmExportChart.nudWidth.Maximum = pChart.Width * 5;
            pfrmExportChart.Show();
        }

        private void frmOptimizationSample_Load(object sender, EventArgs e)
        {
            //Gurobi License Check : that need to be modified HK 041117
            try
            {
                GRBEnv env = new GRBEnv();
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233052)
                    MessageBox.Show("This function requires Gurobi Optimizer 7.0 (32 bit) and its license. Please install the program (http://www.gurobi.com/)"
                        , "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (ex.HResult == -2146233088)
                    MessageBox.Show("This function requires Gurobi License. If you would like to request a free academic license, you can do so from the Free Academic License page (http://www.gurobi.com/academia/for-universities)."
                        , "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("in "+this.Handle.ToString()+":" + " Error:" + ex.Message);

                this.Close();
            }
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
        
    }
}
