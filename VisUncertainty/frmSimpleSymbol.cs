using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisUncertainty
{
    public partial class frmSimpleSymbol : Form
    {
        public frmSimpleSymbol()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            clsSnippet pSnippet = new clsSnippet();
            FormCollection pFormCollection = System.Windows.Forms.Application.OpenForms;
            frmSymbology pfrmSymbology = null;
            for (int j = 0; j < pFormCollection.Count; j++)
                if (pFormCollection[j].Name == "frmSymbology")//Brushing to Histogram
                    pfrmSymbology = pFormCollection[j] as frmSymbology;

            ISimpleFillSymbol pBackFillSymbol = null;
            if (pfrmSymbology != null)
            {
                pBackFillSymbol = new SimpleFillSymbolClass();
                IRgbColor pBackRGB = new RgbColorClass();
                pBackRGB = pSnippet.getRGB(picSymColor.BackColor.R, picSymColor.BackColor.G, picSymColor.BackColor.B);
                pBackFillSymbol.Color = (IColor)pBackRGB;
                ICartographicLineSymbol pBackOut = new CartographicLineSymbolClass();
                IRgbColor pBackOutRGB = new RgbColorClass();
                pBackOutRGB = pSnippet.getRGB(picOutColor.BackColor.R, picOutColor.BackColor.G, picOutColor.BackColor.B);
                pBackOut.Color = (IColor)pBackOutRGB;
                pBackOut.Width = Convert.ToDouble(nudOutWidth.Value);
                pBackFillSymbol.Outline = pBackOut;
                pfrmSymbology.m_BackSymbol = pBackFillSymbol;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picSymColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picSymColor.BackColor = cdColor.Color;

        }

        private void picOutColor_Click(object sender, EventArgs e)
        {
            DialogResult DR = cdColor.ShowDialog();
            if (DR == DialogResult.OK)
                picOutColor.BackColor = cdColor.Color;

        }
    }
}
