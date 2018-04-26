//Many functions are shared with the functions in frmProperties (09/30/15 HK)

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

using RDotNet;
using RDotNet.NativeLibrary;

using UncernVis.BivariateRenderer;

// Bhatt dist is not applied 012016 HK

namespace VisUncertainty
{
    public partial class frmClassSeparability : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;
        private REngine pEngine;

        private IFeatureClass pFClass;
        private IFeatureLayer pFLayer;

        public double[] cb;
        private double[] arrEst;
        private double[] arrVar;
        public double[] dblpValue;
        private int[] intResultIdx; 
        private int intNofFeatures;
        private ClassBreaksRenderer pRender;
        public IEnumColors pEnumColors;
        private int[,] arrColors;
        private string[] arrLabel;
        public int intGCBreakeCount;
        private double[] arrResults;

        private int intRounding;
        private int MaxFeaturesforPGB = 200;

        string strClassificationMethod = string.Empty;
        public frmClassSeparability()
        {
            try
            {
                InitializeComponent();
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pEngine = mForm.pEngine;
                pActiveView = mForm.axMapControl1.ActiveView;
                pSnippet = new clsSnippet();

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                intRounding = 2; //Default
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pSnippet.AddFieldsForTwoCbo(cboSourceLayer.Text, pActiveView, mForm, cboValueField, cboUncernFld);

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, cboSourceLayer.Text);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                pFLayer = pLayer as IFeatureLayer;
                pFClass = pFLayer.FeatureClass;
                intNofFeatures = pFClass.FeatureCount(null);

                IFields fields = pFClass.Fields;


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
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        public void DrawSymbolinListViewwithCb(int intGCBreakeCount, double[] cb)
        {
            try
            {
                
                if (cb == null)
                    return;

                strClassificationMethod = "Breaks maximum " + cboMethod.Text + " between Observations";
                pRender = new ClassBreaksRenderer();

                pRender.Field = cboValueField.Text;
                pRender.BreakCount = intGCBreakeCount;
                pRender.MinimumBreak = cb[0];

                string strColorRamp = cboColorRamp.Text;

                pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, intGCBreakeCount);
                pEnumColors.Reset();

                arrColors = new int[intGCBreakeCount, 3];

                for (int k = 0; k < intGCBreakeCount; k++)
                {
                    IColor pColor = pEnumColors.Next();
                    IRgbColor pRGBColor = new RgbColorClass();
                    pRGBColor.RGB = pColor.RGB;

                    arrColors[k, 0] = pRGBColor.Red;
                    arrColors[k, 1] = pRGBColor.Green;
                    arrColors[k, 2] = pRGBColor.Blue;
                }

                pEnumColors.Reset();

                UpdateRange(lvSymbol, intGCBreakeCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        public void DrawSymbolinListView()
        {
            try
            {
                lvSymbol.Items.Clear();
                string strGCRenderField = cboValueField.Text;
                string strUncernfld = cboUncernFld.Text;
                intGCBreakeCount = Convert.ToInt32(nudGCNClasses.Value);

                int intValueFldIdx = pFClass.FindField(strGCRenderField);
                int intUncernfldIdx = pFClass.FindField(strUncernfld);

                if (intUncernfldIdx == -1 || intValueFldIdx == -1 || cboMethod.Text == "")
                    return;

                ITable pTable = (ITable)pFClass;

                ITableSort pTableSort = new TableSort();
                pTableSort.Table = pTable;
                ICursor pCursor = (ICursor)pFClass.Search(null, false);

                pTableSort.Cursor = pCursor as ICursor;

                pTableSort.Fields = strGCRenderField;
                pTableSort.set_Ascending(strGCRenderField, true);

                // call the sort
                pTableSort.Sort(null);

                // retrieve the sorted rows
                IFeatureCursor pSortedCursor = pTableSort.Rows as IFeatureCursor;

                if (cboMethod.Text == "Class Seperability")
                    cb = ClassSeparabilityMeasure(intGCBreakeCount, intNofFeatures, pSortedCursor, intValueFldIdx, intUncernfldIdx);
                else if (cboMethod.Text == "Bhattacharyya distance")
                {
                    //cb = BhattaDist(intGCBreakeCount, intNofFeatures, pSortedCursor, intValueFldIdx, intUncernfldIdx);
                    MessageBox.Show("Under Reviewing");
                    return;
                }
                if (cb == null)
                    return;
                
                strClassificationMethod = "Breaks maximum " + cboMethod.Text + " between Observations";
                pRender = new ClassBreaksRenderer();

                pRender.Field = strGCRenderField;
                pRender.BreakCount = intGCBreakeCount;
                pRender.MinimumBreak = cb[0];

                string strColorRamp = cboColorRamp.Text;

                pEnumColors = MultiPartColorRamp(strColorRamp, cboAlgorithm.Text, intGCBreakeCount);
                pEnumColors.Reset();

                arrColors = new int[intGCBreakeCount, 3];

                for (int k = 0; k < intGCBreakeCount; k++)
                {
                    IColor pColor = pEnumColors.Next();
                    IRgbColor pRGBColor = new RgbColorClass();
                    pRGBColor.RGB = pColor.RGB;

                    arrColors[k, 0] = pRGBColor.Red;
                    arrColors[k, 1] = pRGBColor.Green;
                    arrColors[k, 2] = pRGBColor.Blue;
                }

                pEnumColors.Reset();

                UpdateRange(lvSymbol, intGCBreakeCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }
        
        private void UpdateRange(ListView lvSymbol, int intGCBreakeCount)
        {
            //try
            //{
                //int intBrksCount = (intGCBreakeCount * 2) - 1;

                arrLabel = new string[intGCBreakeCount];

            //Symbol Listview
                lvSymbol.BeginUpdate();
                lvSymbol.Items.Clear();
            //SepListView
                lvSep.BeginUpdate();
                lvSep.Items.Clear();

                pEnumColors.Reset();
                int[,] arrSepLineColors = RedToBlueColorRamps();

                for (int j = 0; j < intGCBreakeCount; j++)
                {

                    ListViewItem lvi = new ListViewItem("");
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

                    if (j == 0)
                    {
                        lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                        lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString()));
                        arrLabel[j] = Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString());
                    }
                    else
                    {

                        double dblAdding = Math.Pow(0.1, intRounding);
                        lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                        lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString()));
                        arrLabel[j] = Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString());
                    }
                    lvSymbol.Items.Add(lvi);


                    if(j!=0)
                    {
                        int intSepLineIdx = Convert.ToInt32(Math.Floor(dblpValue[j - 1] * 10));
                        lvi = new ListViewItem("");

                        lvi.UseItemStyleForSubItems = false;
                        lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        dblpValue[j - 1].ToString("N" + intRounding.ToString()), Color.White, Color.FromArgb(arrSepLineColors[intSepLineIdx, 0], arrSepLineColors[intSepLineIdx, 1], arrSepLineColors[intSepLineIdx, 2]), lvi.Font));
                        lvSep.Items.Add(lvi);
                    }


                }
                //        int intValueIdx = (j - 1) / 2;
                //        int intSepLineIdx = Convert.ToInt32(Math.Floor(dblpValue[intValueIdx] * 10));

                //        ListViewItem lvi = new ListViewItem("");
                //        lvi.UseItemStyleForSubItems = false;
                //        lvi.SubItems.Add("");

                //        lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //            "", Color.White, Color.FromArgb(arrSepLineColors[intSepLineIdx, 0], arrSepLineColors[intSepLineIdx, 1], arrSepLineColors[intSepLineIdx, 2]), lvi.Font));
                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //    "", Color.White, Color.FromArgb(arrSepLineColors[intSepLineIdx, 0], arrSepLineColors[intSepLineIdx, 1], arrSepLineColors[intSepLineIdx, 2]), lvi.Font));
                //lvi.SubItems.Add(dblpValue[intValueIdx].ToString("N" + intRounding.ToString()));
                //        lvSymbol.Items.Add(lvi);

#region Previous Method 030716 HK
                //for (int j = 0; j < intGCBreakeCount; j++)
                //{
                //    if (j % 2 == 0)
                //    {
                //        int k = j / 2;
                //        ListViewItem lvi = new ListViewItem("");
                //        lvi.UseItemStyleForSubItems = false;
                //        lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //            "", Color.White, Color.FromArgb(arrColors[k, 0], arrColors[k, 1], arrColors[k, 2]), lvi.Font));

                //        if (k == 0)
                //        {
                //            lvi.SubItems.Add(Math.Round(cb[k], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[k + 1], intRounding).ToString("N" + intRounding.ToString()));
                //            lvi.SubItems.Add(Math.Round(cb[k], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[k + 1], 2).ToString("N" + intRounding.ToString()));
                //            lvi.SubItems.Add("");
                //            arrLabel[k] = Math.Round(cb[k], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[k + 1], 2).ToString("N" + intRounding.ToString());
                //        }
                //        else
                //        {
                //            double dblAdding = Math.Pow(0.1, intRounding);
                //            lvi.SubItems.Add(Math.Round(cb[k] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[k + 1], intRounding).ToString("N" + intRounding.ToString()));
                //            lvi.SubItems.Add(Math.Round(cb[k] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[k + 1], 2).ToString("N" + intRounding.ToString()));
                //            lvi.SubItems.Add("");
                //            arrLabel[k] = Math.Round(cb[k] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[k + 1], 2).ToString("N" + intRounding.ToString());
                //        }
                //        lvSymbol.Items.Add(lvi);
                //    }
                //    else
                //    {
                //        int intValueIdx = (j - 1) / 2;
                //        int intSepLineIdx = Convert.ToInt32(Math.Floor(dblpValue[intValueIdx] * 10));
                        
                //        ListViewItem lvi = new ListViewItem("");
                //        lvi.UseItemStyleForSubItems = false;
                //        lvi.SubItems.Add("");

                //        lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //            "", Color.White, Color.FromArgb(arrSepLineColors[intSepLineIdx, 0], arrSepLineColors[intSepLineIdx, 1], arrSepLineColors[intSepLineIdx, 2]), lvi.Font));
                //        lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //            "", Color.White, Color.FromArgb(arrSepLineColors[intSepLineIdx, 0], arrSepLineColors[intSepLineIdx, 1], arrSepLineColors[intSepLineIdx, 2]), lvi.Font));
                //        lvi.SubItems.Add(dblpValue[intValueIdx].ToString("N" + intRounding.ToString()));
                //        lvSymbol.Items.Add(lvi);
                //    }
                //}
#endregion
#region previous method 021116 HK
                //for (int j = 0; j < intGCBreakeCount; j++)
                //{

                //    ListViewItem lvi = new ListViewItem("");
                //    lvi.UseItemStyleForSubItems = false;
                //    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //        "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

                //    if (j == 0)
                //    {
                //        lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                //        lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString()));
                //        arrLabel[j] = Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString());
                //    }
                //    else
                //    {

                //        double dblAdding = Math.Pow(0.1, intRounding);
                //        lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                //        lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString()));
                //        arrLabel[j] = Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], 2).ToString("N" + intRounding.ToString());
                //    }
                //    lvSymbol.Items.Add(lvi);

#endregion
                lvSymbol.EndUpdate();
                lvSep.EndUpdate();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
            //    return;
            //}
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
                            pColor1 = pSnippet.getRGB(239, 243, 255);
                            pColor2 = pSnippet.getRGB(8, 81, 156);
                            break;
                        case "Green Light to Dark":
                            pColor1 = pSnippet.getRGB(237, 248, 233);
                            pColor2 = pSnippet.getRGB(0, 109, 44);
                            break;
                        case "Orange Light to Dark":
                            pColor1 = pSnippet.getRGB(254, 237, 222);
                            pColor2 = pSnippet.getRGB(166, 54, 3);
                            break;
                        case "Red Light to Dark":
                            pColor1 = pSnippet.getRGB(254, 229, 217);
                            pColor2 = pSnippet.getRGB(165, 15, 21);
                            break;
                        default:
                            pColor1 = pSnippet.getRGB(254, 229, 217);
                            pColor2 = pSnippet.getRGB(165, 15, 21);
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
                            pColor1 = pSnippet.getRGB(49, 54, 149);
                            pColor2 = pSnippet.getRGB(255, 255, 191);
                            pColor3 = pSnippet.getRGB(165, 0, 38);
                            break;
                        case "Green to Purple":
                            pColor1 = pSnippet.getRGB(0, 68, 27);
                            pColor2 = pSnippet.getRGB(247, 247, 247);
                            pColor3 = pSnippet.getRGB(64, 0, 75);
                            break;
                        case "Green to Red":
                            pColor1 = pSnippet.getRGB(0, 104, 55);
                            pColor2 = pSnippet.getRGB(255, 255, 191);
                            pColor3 = pSnippet.getRGB(165, 0, 38);
                            break;
                        case "Purple to Brown":
                            pColor1 = pSnippet.getRGB(45, 0, 75);
                            pColor2 = pSnippet.getRGB(247, 247, 247);
                            pColor3 = pSnippet.getRGB(127, 59, 8);
                            break;
                        default:
                            pColor1 = pSnippet.getRGB(49, 54, 149);
                            pColor2 = pSnippet.getRGB(255, 255, 191);
                            pColor3 = pSnippet.getRGB(165, 0, 38);
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
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return null;
            }

        }



        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboUncernFld.Text == "" || cboValueField.Text == "")
                {
                    MessageBox.Show("Please assign proper fields");
                    return;
                }
                pSnippet.AddRenderInfo(mForm, pFLayer, cboValueField.Text, cboUncernFld.Text, cb, arrColors, strClassificationMethod);

                IRgbColor pColorOutline = new RgbColor();
                //Can Change the color in here!
                pColorOutline = pSnippet.getRGB(picGCLineColor.BackColor.R, picGCLineColor.BackColor.G, picGCLineColor.BackColor.B);
                if (pColorOutline == null)
                    return;
                double dblGCOutlineSize = Convert.ToDouble(nudGCLinewidth.Value);

                //Previous Methods 020416
                //ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
                //pOutLines.Width = dblGCOutlineSize;
                //pOutLines.Color = (IColor)pColorOutline;

                ////' use this interface to set dialog properties
                //IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pRender;
                //pUIProperties.ColorRamp = "Custom";
                //ISimpleFillSymbol pSimpleFillSym;

                //pEnumColors.Reset();
                ////' be careful, indices are different for the diff lists
                //for (int j = 0; j < intGCBreakeCount; j++)
                //{
                //    pRender.Break[j] = cb[j + 1];
                //    pRender.Label[j] = arrLabel[j];
                //    pUIProperties.LowBreak[j] = cb[j];
                //    pSimpleFillSym = new SimpleFillSymbolClass();
                //    IRgbColor pRGBColor = pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                //    if (pRGBColor == null)
                //        return;
                //    pSimpleFillSym.Color = (IColor)pRGBColor;
                //    pSimpleFillSym.Outline = pOutLines;
                //    pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
                //}

                //IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)pFLayer;

                //pGeofeatureLayer.Renderer = (IFeatureRenderer)pRender;

                //New Method
                IClassSeparabilityRenderer pClassSeparabilityRenderer = new ClassSeparabilityRenderer();
                pClassSeparabilityRenderer.strValueFldName = cboValueField.Text;
                pClassSeparabilityRenderer.arrClassBrks = cb;
                pClassSeparabilityRenderer.arrColors = arrColors;
                pClassSeparabilityRenderer.arrSeparability = dblpValue;
                pClassSeparabilityRenderer.arrSepLineColors = RedToBlueColorRamps();
                //pClassSeparabilityRenderer.arrValue = arrEst;
                pClassSeparabilityRenderer.dblLineWidth = dblGCOutlineSize;
                pClassSeparabilityRenderer.intRoundingDigits = intRounding;
                pClassSeparabilityRenderer.m_pLineRgb = pColorOutline;
                pClassSeparabilityRenderer.strHeading = "Estimate and Separability";
                pClassSeparabilityRenderer.CreateLegend();

                IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)pFLayer;

                pGeofeatureLayer.Renderer = (IFeatureRenderer)pClassSeparabilityRenderer;
                mForm.axMapControl1.ActiveView.Refresh();
                mForm.axTOCControl1.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void nudGCNClasses_ValueChanged(object sender, EventArgs e)
        {
            DrawSymbolinListView();
        }
        private void cboValueField_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawSymbolinListView();
        }

        private void cboUncernFld_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawSymbolinListView();
        }

        private void cboAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawSymbolinListView();
        }
        private void cboMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawSymbolinListView();
        }
        private void cboColorRamp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboColorRamp.Text == "Custom")
                {
                    if (cboValueField.Text == "" || cboUncernFld.Text == "")
                    {
                        MessageBox.Show("Please select fields");
                        cboColorRamp.Text = "Red Light to Dark";
                        return;
                    }
                    else
                    {
                        frmColorRamps pColorRamps = new frmColorRamps();
                        pColorRamps.intLoadingPlaces = 2;
                        pColorRamps.Show();
                    }
                }
                else
                    DrawSymbolinListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void picGCLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGCLineColor.BackColor = cdColor.Color;
        }
        #region classification method region

        private double[] ClassSeparabilityMeasure(int intNClasses, int intNFeatures, IFeatureCursor pFCursor, int intEstIdx, int intVarIdx)
        {
            try
            {
                frmProgress pfrmProgress = new frmProgress();
                if (intNFeatures > MaxFeaturesforPGB)
                {
                    pfrmProgress.lblStatus.Text = "Calculate Class Separabilities";
                    pfrmProgress.pgbProgress.Style = ProgressBarStyle.Blocks;
                    pfrmProgress.pgbProgress.Value = 0;
                    pfrmProgress.Show();
                }

                double[] Cs = new double[intNClasses + 1];

                arrEst = new double[intNFeatures];
                arrVar = new double[intNFeatures];
                arrResults = new double[intNFeatures - 1];
                double[] arrSortedResult = new double[intNFeatures - 1];

                IFeature pFeature = pFCursor.NextFeature();
                int k = 0;
                while (pFeature != null)
                {
                    arrEst[k] = Convert.ToDouble(pFeature.get_Value(intEstIdx));
                    arrVar[k] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                    k++;
                    pFeature = pFCursor.NextFeature();
                }

                for (int i = 0; i < intNFeatures - 1; i++)
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

                    if (intNFeatures > MaxFeaturesforPGB)
                        pfrmProgress.pgbProgress.Value = (i * 100) / intNFeatures;
                }

                if (intNFeatures > MaxFeaturesforPGB)
                {
                    pfrmProgress.pgbProgress.Value = 100;
                    pfrmProgress.lblStatus.Text = "Show Results";
                }

                System.Array.Sort<double>(arrSortedResult, new Comparison<double>(
                                (i1, i2) => i2.CompareTo(i1)
                        ));

                Chart pChart = new Chart();

                dblpValue = new double[intNClasses - 1];
                intResultIdx = new int[intNClasses - 1];
                for (int i = 0; i < intNClasses - 1; i++)
                {
                    intResultIdx[i] = System.Array.IndexOf(arrResults, arrSortedResult[i]);
                    //dblpValue[i] = pChart.DataManipulator.Statistics.NormalDistribution(arrSortedResult[i]);
                }
                System.Array.Sort(intResultIdx);

                Cs[0] = arrEst.Min();

                Cs[intNClasses] = arrEst.Max();
                for (int i = 0; i < intNClasses - 1; i++)
                {
                    Cs[i + 1] = arrEst[intResultIdx[i]];
                    dblpValue[i] = pChart.DataManipulator.Statistics.NormalDistribution(arrResults[intResultIdx[i]]);
                }
                
                pfrmProgress.Close();
                return Cs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return null;
            }

        }

        private double[] BhattaDist(int intNClasses, int intNFeatures, IFeatureCursor pFCursor, int intEstIdx, int intVarIdx)
        {
            try
            {
                frmProgress pfrmProgress = new frmProgress();
                if (intNFeatures > MaxFeaturesforPGB)
                {
                    pfrmProgress.lblStatus.Text = "Calculate Bhattacharyya distance";
                    pfrmProgress.pgbProgress.Style = ProgressBarStyle.Blocks;
                    pfrmProgress.pgbProgress.Value = 0;
                    pfrmProgress.Show();
                }

                double[] Cs = new double[intNClasses + 1];

                arrEst = new double[intNFeatures];
                double[] arrVar = new double[intNFeatures];
                double[,] arrResults = new double[intNFeatures, intNFeatures];

                IFeature pFeature = pFCursor.NextFeature();
                int k = 0;
                while (pFeature != null)
                {
                    arrEst[k] = Convert.ToDouble(pFeature.get_Value(intEstIdx));
                    arrVar[k] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
                    k++;
                    pFeature = pFCursor.NextFeature();
                }

                for (int i = 0; i < intNFeatures; i++)
                {
                    double dblsquaredVar1 = Math.Pow(arrVar[i], 2);

                    for (int j = 0; j < intNFeatures; j++)
                    {

                        double dblsquaredVar2 = Math.Pow(arrVar[j], 2);
                        double dblVarComp = Math.Log(0.25 * ((dblsquaredVar1 / dblsquaredVar2) + (dblsquaredVar2 / dblsquaredVar1) + 2));
                        double dblMeanComp = Math.Pow(arrEst[i] - arrEst[j], 2) / (dblsquaredVar1 + dblsquaredVar2);
                        arrResults[i, j] = 0.25 * (dblVarComp + dblMeanComp);
                    }

                    if (intNFeatures > MaxFeaturesforPGB)
                        pfrmProgress.pgbProgress.Value = (i * 100) / intNFeatures;
                }

                if (intNFeatures > MaxFeaturesforPGB)
                {
                    pfrmProgress.pgbProgress.Value = 100;
                    pfrmProgress.lblStatus.Text = "Show Results";
                }

                NumericMatrix pBhattaDist = pEngine.CreateNumericMatrix(arrResults);
                pEngine.SetSymbol("Bhatta.diss", pBhattaDist);
                pEngine.Evaluate("library(cluster)");
                pEngine.Evaluate("kmed.result <- pam(as.dist(Bhatta.diss), diss=TRUE, " + intNClasses.ToString() + ")");
                NumericVector pClustering = pEngine.Evaluate("kmed.result$clustering").AsNumeric();

                Cs[0] = arrEst.Min();
                Cs[intNClasses] = arrEst.Max();

                //double[] tempCs = new double[intNClasses + 1];
                //tempCs[0] = Cs[0];
                //tempCs[intNClasses] = Cs[intNClasses];

                for (int i = 0; i < intNFeatures; i++)
                {
                    for (int j = 1; j < intNClasses; j++)
                    {
                        if (pClustering[i] == j)
                        {
                            double tempClassMax = arrEst[i];
                            if (tempClassMax > Cs[j])
                                Cs[j] = tempClassMax;
                        }
                    }
                }

                pfrmProgress.Close();
                return Cs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return null;
            }

        }
        #endregion

        private void btnGraph_Click(object sender, EventArgs e)
        {
            frmCSDesign pfrmCSDesign = new frmCSDesign();

            int intNfeature = arrEst.Length;
            double[,] adblValues = new double[intNfeature, 3];
            System.Web.UI.DataVisualization.Charting.Chart pChart = new System.Web.UI.DataVisualization.Charting.Chart();
            pfrmCSDesign.nudConfidenceLevel.Value = 99;
            double dblConfidenceValue = Convert.ToDouble(pfrmCSDesign.nudConfidenceLevel.Value);
            double dblConInstance = pChart.DataManipulator.Statistics.InverseNormalDistribution(dblConfidenceValue / 100);

            for (int i = 0; i < intNfeature; i++)
            {
                double dblValue = arrEst[i];
                double dblUncern = dblConInstance * arrVar[i];
                if (dblValue < dblUncern)
                {
                    adblValues[i, 0] = 0;
                    adblValues[i, 1] = dblValue;
                    adblValues[i, 2] = dblUncern;
                }
                else
                {
                    adblValues[i, 0] = dblValue - dblUncern;
                    adblValues[i, 1] = dblUncern;
                    adblValues[i, 2] = dblUncern;
                }
            }

            AddStackedColumnSeries(pfrmCSDesign, "Low", Color.White, adblValues, 0, intNfeature);
            AddStackedColumnSeries(pfrmCSDesign, "Mean", Color.Gray, adblValues, 1, intNfeature);
            AddStackedColumnSeries(pfrmCSDesign, "High", Color.Gray, adblValues, 2, intNfeature);

            double dblMin = 0;
            double dblMax = arrEst.Max() + (3 * arrVar.Max());

            int[,] arrSepVerColors = RedToBlueColorRamps();
            int intSepLineIdx = 0;
            System.Drawing.Color pColor = new Color();
            for (int j = 0; j < intResultIdx.Length; j++)
            {
                intSepLineIdx = Convert.ToInt32(Math.Floor(dblpValue[j] * 10));
                pColor = new Color();
                pColor = Color.FromArgb(arrSepVerColors[intSepLineIdx, 0], arrSepVerColors[intSepLineIdx, 1], arrSepVerColors[intSepLineIdx, 2]);
                AddVerticalLineSeries(pfrmCSDesign, "ver_" + j.ToString(), pColor, intResultIdx[j] + 0.5, dblMin, dblMax);
            }


            pfrmCSDesign.pChart.ChartAreas[0].AxisX.IsStartedFromZero = true;
            pfrmCSDesign.pChart.ChartAreas[0].AxisY.Title = cboValueField.Text;
            pfrmCSDesign.nudConfidenceLevel.Value = 99;
            pfrmCSDesign.arrEst = arrEst;
            pfrmCSDesign.arrVar = arrVar;
            pfrmCSDesign.intResultIdx = intResultIdx;
            pfrmCSDesign.cb = cb;
            pfrmCSDesign.nudGCNClasses.Value = nudGCNClasses.Value;
            pfrmCSDesign.arrResults = arrResults;
            pfrmCSDesign.dblpValue = dblpValue;
            //pfrmClassfiGraph.pModel = pModel;
            //pfrmClassfiGraph.pDecVar = pDecVar;
            //pfrmClassfiGraph.pTotalStepsConst = pTotalStepsConst;
            pfrmCSDesign.lblMethod.Text = strClassificationMethod;
            //pfrmClassfiGraph.txtObjValue.Text = txtObjValue.Text;
            pfrmCSDesign.strValueFldName = cboValueField.Text;
            pfrmCSDesign.strUncernFldName = cboUncernFld.Text;
            pfrmCSDesign.mForm = mForm;
            pfrmCSDesign.pActiveView = pActiveView;
            pfrmCSDesign.pFLayer = pFLayer;
            pfrmCSDesign.Show();

        }
        private void AddStackedColumnSeries(frmCSDesign pfrmCSDesign, string strSeriesName, System.Drawing.Color FillColor, double[,] adblValues, int intStats, int intNfeatures)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = Color.Black,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn,

                };

                pfrmCSDesign.pChart.Series.Add(pSeries);

                for (int j = 0; j < intNfeatures; j++)
                {
                    pSeries.Points.AddXY(j, adblValues[j, intStats]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }
        private void AddVerticalLineSeries(frmCSDesign pfrmCSDesign, string strSeriesName, System.Drawing.Color FillColor, double dblX, double dblYMin, double dblYMax)
        {
            try
            {
                var pSeries = new System.Windows.Forms.DataVisualization.Charting.Series
                {
                    Name = strSeriesName,
                    Color = FillColor,
                    BorderColor = Color.Black,
                    BorderWidth = 2,
                    IsVisibleInLegend = false,
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line,

                };

                pfrmCSDesign.pChart.Series.Add(pSeries);

                pSeries.Points.AddXY(dblX, dblYMin);
                pSeries.Points.AddXY(dblX, dblYMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }

        }
        private int[,] RedToBlueColorRamps()
        {
            IEnumColors pEnumColors = null;
            int[,] arrSepLineColor = new int[10, 3];
            IAlgorithmicColorRamp pColorRamp1 = new AlgorithmicColorRampClass();
            IAlgorithmicColorRamp pColorRamp2 = new AlgorithmicColorRampClass();

            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;

            IRgbColor pColor1 = new RgbColor();
            IRgbColor pColor2 = new RgbColor();
            IRgbColor pColor3 = new RgbColor();



            pColor1 = pSnippet.getRGB(165, 0, 38);
            pColor2 = pSnippet.getRGB(255, 255, 191);
            pColor3 = pSnippet.getRGB(49, 54, 149);

            pColorRamp1.FromColor = pColor1;
            pColorRamp1.ToColor = pColor2;
            pColorRamp2.FromColor = pColor2;
            pColorRamp2.ToColor = pColor3;

            Boolean blnOK = true;

            IMultiPartColorRamp pMultiColorRamp = new MultiPartColorRampClass();
            pMultiColorRamp.Ramp[0] = pColorRamp1;
            pMultiColorRamp.Ramp[1] = pColorRamp2;
            pMultiColorRamp.Size = 10;
            pMultiColorRamp.CreateRamp(out blnOK);


            pEnumColors = pMultiColorRamp.Colors;
            pEnumColors.Reset();
            for (int k = 0; k < 10; k++)
            {
                IColor pColor = pEnumColors.Next();
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.RGB = pColor.RGB;

                arrSepLineColor[k, 0] = pRGBColor.Red;
                arrSepLineColor[k, 1] = pRGBColor.Green;
                arrSepLineColor[k, 2] = pRGBColor.Blue;
            }

            return arrSepLineColor;
        }


    }
}
