using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;

namespace VisUncertainty
{
    public partial class frmColorRamps : Form
    {
        private int[,] arrListviewColors;
        private int[,] arrFormViewColors;
        private IEnumColors pEnumColors;
        private IEnumColors pTotalEnumColors;
        private int intMaximumRamps = 15;
        private int intRampsCounts;
        private int[,] arrTotalColors;
        private clsSnippet pSnippet;
        private IMultiPartColorRamp pMultiColorRamp;

        public IMultiPartColorRamp pMulitColorRampsResults;
        public int intLoadingPlaces; //(1 from Properties, 2 from ClassSeparability, 3 from Sample Optimization, 4 from CCMaps, 08/03/16 HK)

        public frmColorRamps()
        {
            //Close previous color Ramp form


            InitializeComponent();
            intRampsCounts = 0;
            arrTotalColors = new int[intMaximumRamps, 6];
            pSnippet = new clsSnippet();

            IntPtr ThisHandle = this.Handle;
            int intOpenedFormsCnt = System.Windows.Forms.Application.OpenForms.Count;
            for (int i = 0; i < intOpenedFormsCnt; i++)
            {
                if (System.Windows.Forms.Application.OpenForms[i].Name == this.Name && System.Windows.Forms.Application.OpenForms[i].Handle != ThisHandle)
                    System.Windows.Forms.Application.OpenForms[i].Close();
            }
        }

        private void CreateAlgorimcColorRamps(string strAlgorithm, int intDefault)
        {
            try
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

                IRgbColor pColor1 = new RgbColorClass();
                IRgbColor pColor2 = new RgbColorClass();

                //Can Change the color in here!
                pColor1 = pSnippet.getRGB(arrTotalColors[intRampsCounts, 0], arrTotalColors[intRampsCounts, 1], arrTotalColors[intRampsCounts, 2]);
                pColor2 = pSnippet.getRGB(arrTotalColors[intRampsCounts, 3], arrTotalColors[intRampsCounts, 4], arrTotalColors[intRampsCounts, 5]);

                if (pColor1 == null || pColor2 == null)
                    return;

                Boolean blnOK = true;

                pColorRamp.FromColor = pColor1;
                pColorRamp.ToColor = pColor2;
                pColorRamp.Size = intDefault;
                pColorRamp.CreateRamp(out blnOK);

                arrListviewColors = new int[intDefault, 3];
                pEnumColors = pColorRamp.Colors;
                pEnumColors.Reset();

                for (int k = 0; k < intDefault; k++)
                {
                    IColor pColor = pEnumColors.Next();
                    IRgbColor pRGBColor = new RgbColorClass();
                    pRGBColor.RGB = pColor.RGB;

                    arrListviewColors[k, 0] = pRGBColor.Red;
                    arrListviewColors[k, 1] = pRGBColor.Green;
                    arrListviewColors[k, 2] = pRGBColor.Blue;
                }

                pEnumColors.Reset();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private void CreateMultipartRamps(string strAlgorithm, int intFormColumnCnt)
        {
            try
            {
                pMultiColorRamp = new MultiPartColorRampClass();

                for (int i = 0; i < intRampsCounts + 1; i++)
                {
                    IAlgorithmicColorRamp pColorRamp1 = new AlgorithmicColorRampClass();
                    switch (strAlgorithm)
                    {
                        case "HSV":
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriHSVAlgorithm;
                            break;
                        case "CIE Lab":
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            break;
                        case "Lab LCh":
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriLabLChAlgorithm;
                            break;
                        default:
                            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
                            break;
                    }
                    IRgbColor pColor1 = new RgbColor();
                    IRgbColor pColor2 = new RgbColor();
                    pColor1 = pSnippet.getRGB(arrTotalColors[i, 0], arrTotalColors[i, 1], arrTotalColors[i, 2]);
                    pColor2 = pSnippet.getRGB(arrTotalColors[i, 3], arrTotalColors[i, 4], arrTotalColors[i, 5]);
                    if (pColor1 == null || pColor2 == null)
                        return;
                    
                    pColorRamp1.FromColor = pColor1;
                    pColorRamp1.ToColor = pColor2;

                    pMultiColorRamp.Ramp[i] = pColorRamp1;
                }

                pMultiColorRamp.Size = intFormColumnCnt;

                Boolean blnOK = true;


                pMultiColorRamp.CreateRamp(out blnOK);


                pTotalEnumColors = pMultiColorRamp.Colors;

                arrFormViewColors = new int[intFormColumnCnt, 3];

                pTotalEnumColors.Reset();

                for (int k = 0; k < intFormColumnCnt; k++)
                {
                    IColor pColor = pTotalEnumColors.Next();
                    IRgbColor pRGBColor = new RgbColorClass();
                    pRGBColor.RGB = pColor.RGB;

                    arrFormViewColors[k, 0] = pRGBColor.Red;
                    arrFormViewColors[k, 1] = pRGBColor.Green;
                    arrFormViewColors[k, 2] = pRGBColor.Blue;
                }
                pTotalEnumColors.Reset();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                int intDefault = 15; // Same with Column Count in the listview
                int intFormColumnCnt = 30;

                arrTotalColors[intRampsCounts, 0] = picSymolfrom.BackColor.R;
                arrTotalColors[intRampsCounts, 1] = picSymolfrom.BackColor.G;
                arrTotalColors[intRampsCounts, 2] = picSymolfrom.BackColor.B;

                arrTotalColors[intRampsCounts, 3] = picSymbolTo.BackColor.R;
                arrTotalColors[intRampsCounts, 4] = picSymbolTo.BackColor.G;
                arrTotalColors[intRampsCounts, 5] = picSymbolTo.BackColor.B;

                CreateAlgorimcColorRamps(cboAlgorithm.Text, intDefault);
                AddRamps(listRamps, intDefault, arrListviewColors);

                CreateMultipartRamps(cboAlgorithm.Text, intFormColumnCnt);
                AddMultiRamps(listFormcolor, intFormColumnCnt, arrFormViewColors);
                intRampsCounts++;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private void AddMultiRamps(ListView lvSymbol, int intFormColumnCnt, int[,] arrFormViewColors)
        {
            try
            {
                lvSymbol.BeginUpdate();
                lvSymbol.Items.Clear();
                int intItemCnts = lvSymbol.Items.Count;
                ListViewItem lvi = new ListViewItem(intItemCnts.ToString());

                for (int j = 0; j < intFormColumnCnt; j++)
                {
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        "", Color.White, Color.FromArgb(arrFormViewColors[j, 0], arrFormViewColors[j, 1], arrFormViewColors[j, 2]), lvi.Font));
                }

                lvSymbol.Items.Add(lvi);

                lvSymbol.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void AddRamps(ListView lvSymbol, int intDefault, int[,] arrListviewColors)
        {
            try
            {
                lvSymbol.BeginUpdate();
                int intItemCnts = lvSymbol.Items.Count;
                ListViewItem lvi = new ListViewItem(intItemCnts.ToString());

                for (int j = 0; j < intDefault; j++)
                {
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        "", Color.White, Color.FromArgb(arrListviewColors[j, 0], arrListviewColors[j, 1], arrListviewColors[j, 2]), lvi.Font));
                }

                lvSymbol.Items.Add(lvi);

                lvSymbol.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                pMulitColorRampsResults = new MultiPartColorRampClass();
                pMulitColorRampsResults = pMultiColorRamp;

                if (intLoadingPlaces == 1)
                {
                    frmSymbology pForm = System.Windows.Forms.Application.OpenForms["frmProperties"] as frmSymbology;
                    //pForm.DrawSymbolinListView();
                    pForm.DrawSymboliinDataGridView();
                }
                else if (intLoadingPlaces == 2)
                {
                    frmNewClassSeparbility pForm = System.Windows.Forms.Application.OpenForms["frmNewClassSeparability"] as frmNewClassSeparbility;
                    pForm.DrawSymbolinChartwithCb(pForm.m_cb, pForm.m_intGCBreakeCount);
                    //frmClassSeparability pForm = System.Windows.Forms.Application.OpenForms["frmClassSeparability"] as frmClassSeparability;
                    //pForm.DrawSymbolinListView();
                }
                else if (intLoadingPlaces == 3)
                {
                    frmOptimizationSample pForm = System.Windows.Forms.Application.OpenForms["frmOptimizationSample"] as frmOptimizationSample;
                    pForm.DrawSymbolinChartwithCb(pForm.m_cb, pForm.m_intGCBreakeCount);
                }
                else if (intLoadingPlaces == 4)
                {
                    MessageBox.Show("The cmstom color ramp will be applied after udapting maps"); 
                }

                this.Hide();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void picSymolfrom_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picSymolfrom.BackColor = cdColor.Color;
        }

        private void picSymbolTo_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picSymbolTo.BackColor = cdColor.Color;
        }

    }
}
