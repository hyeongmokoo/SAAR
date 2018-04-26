

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;

using RDotNet;

//Removed : 09/03/15

namespace VisUncertainty
{
    public partial class frmCreatingEVMaps : Form
    {
        public IFeatureLayer fLayer;
        private clsSnippet pSnippet;
        private MainForm mForm;
        private IActiveView pActiveView;
        public CharacterVector vecNames;
        public int intNSelectedEVs;
        public int nIDepen;

        public frmCreatingEVMaps()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstSelEVs, lstEVMaps);
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstEVMaps, lstSelEVs);
        }

        private void lstSelEVs_DoubleClick(object sender, EventArgs e)
        {
            pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstSelEVs, lstEVMaps);
        }

        private void lstEVMaps_DoubleClick(object sender, EventArgs e)
        {
            pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstEVMaps, lstEVMaps);
        }

        private void frmCreatingEVMaps_Load(object sender, EventArgs e)
        {
            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            pActiveView = mForm.axMapControl1.ActiveView;

            for (int k = 1; k <= intNSelectedEVs; k++)
            {
                lstSelEVs.Items.Add((object)vecNames[k + nIDepen]);
            }

            pSnippet = new clsSnippet();
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            for(int i=0; i<lstEVMaps.Items.Count; i++)
            {
                string strRenderField = lstEVMaps.Items[i].ToString();
                DrawingChoroplethmap(fLayer, strRenderField, nudGCNClasses.Value);
            }

            
        }

        private void DrawingChoroplethmap(IFeatureLayer pFLayer, string strRenderField, decimal NClasses)
        {
            IFeatureClass pFClass = pFLayer.FeatureClass;

            //Create Rendering of Mean Value at Target Layer
            int intGCBreakeCount = Convert.ToInt32(NClasses);

            IGeoFeatureLayer pGeofeatureLayer;

            IFeatureLayer pflOutput = new FeatureLayerClass();
            pflOutput.FeatureClass = pFClass;
            pflOutput.Name = strRenderField;
            pflOutput.Visible = true;

            pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;

            ITable pTable = (ITable)pFClass;
            IClassifyGEN pClassifyGEN;
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

            ITableHistogram pTableHistogram = new TableHistogramClass();
            pTableHistogram.Field = strRenderField;
            pTableHistogram.Table = pTable;
            IHistogram pHistogram = (IHistogram)pTableHistogram;

            object xVals, frqs;
            pHistogram.GetHistogram(out xVals, out frqs);
            pClassifyGEN.Classify(xVals, frqs, intGCBreakeCount);

            ClassBreaksRenderer pRender = new ClassBreaksRenderer();
            double[] cb = (double[])pClassifyGEN.ClassBreaks;
            pRender.Field = strRenderField;
            pRender.BreakCount = intGCBreakeCount;
            pRender.MinimumBreak = cb[0];

            string strColorRamp = cboColorRamp.Text;

            IEnumColors pEnumColors = MultiPartColorRamp(strColorRamp, intGCBreakeCount);
            pEnumColors.Reset();

            IRgbColor pColorOutline = new RgbColor();
            //Can Change the color in here!
            pColorOutline.Red = picGCLineColor.BackColor.R;
            pColorOutline.Green = picGCLineColor.BackColor.G;
            pColorOutline.Blue = picGCLineColor.BackColor.B;
            double dblGCOutlineSize = Convert.ToDouble(nudGCLinewidth.Value);

            ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
            pOutLines.Width = dblGCOutlineSize;
            pOutLines.Color = (IColor)pColorOutline;

            //' use this interface to set dialog properties
            IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pRender;
            pUIProperties.ColorRamp = "Custom";

            ISimpleFillSymbol pSimpleFillSym;
            //' be careful, indices are different for the diff lists
            for (int j = 0; j < intGCBreakeCount; j++)
            {
                pRender.Break[j] = cb[j + 1];
                pRender.Label[j] = Math.Round(cb[j], 2).ToString() + " - " + Math.Round(cb[j + 1], 2).ToString();
                pUIProperties.LowBreak[j] = cb[j];
                pSimpleFillSym = new SimpleFillSymbolClass();
                pSimpleFillSym.Color = pEnumColors.Next();
                pSimpleFillSym.Outline = pOutLines;
                pRender.Symbol[j] = (ISymbol)pSimpleFillSym;
            }
            pGeofeatureLayer.Renderer = (IFeatureRenderer)pRender;
            mForm.axMapControl1.ActiveView.FocusMap.AddLayer(pGeofeatureLayer);
        }

        
        private void picGCLineColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picGCLineColor.BackColor = cdColor.Color;
        }

        private IEnumColors MultiPartColorRamp(string strColorRamp, int intGCBreakeCount)
        {

            IAlgorithmicColorRamp pColorRamp1 = new AlgorithmicColorRampClass();
            pColorRamp1.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            IAlgorithmicColorRamp pColorRamp2 = new AlgorithmicColorRampClass();
            pColorRamp2.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;

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


            IEnumColors pEnumColors = pMultiColorRamp.Colors;
            return pEnumColors;

        }
    }
}
