using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;

//Remove 012016 HK
namespace VisUncertainty
{
    public partial class frmClassification : Form
    {
        public IFeatureLayer pFLayer;
        public double[] cb;
        public int intBreakCnt;
        public string strFieldName;


        public frmClassification()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            cb = fnClassification(pFLayer, nudCoNClasses.Value, cboWeight.Text, cboCoClassify.Text);
            intBreakCnt = Convert.ToInt32(nudCoNClasses.Value);
            strFieldName = cboWeight.Text;

            this.Hide();
        }

        private void frmClassification_Shown(object sender, EventArgs e)
        {
            if (pFLayer == null)
                return;

            IFields fields = pFLayer.FeatureClass.Fields;

            cboWeight.Items.Clear();

            for (int i = 0; i < fields.FieldCount; i++)
            {
                cboWeight.Items.Add(fields.get_Field(i).Name);
            }
            cboWeight.Items.Add("None");
        }
        private double[] fnClassification(IFeatureLayer pFLayer, decimal NClasses, string strClassifiedField, string ClassifiedMethod)
        {
            
            IFeatureClass pFClass = pFLayer.FeatureClass;

            //Create Rendering of Mean Value at Target Layer
            int intBreakeCount = Convert.ToInt32(NClasses);

            ITable pTable = (ITable)pFClass;
            IClassifyGEN pClassifyGEN;
            switch (ClassifiedMethod)
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

            //Need to be changed 1/29/15
            ITableHistogram pTableHistogram = new TableHistogramClass();
            pTableHistogram.Field = strClassifiedField;
            pTableHistogram.Table = pTable;
            IHistogram pHistogram = (IHistogram)pTableHistogram;

            object xVals, frqs;
            pHistogram.GetHistogram(out xVals, out frqs);
            pClassifyGEN.Classify(xVals, frqs, intBreakeCount);
            double[] cb = (double[])pClassifyGEN.ClassBreaks;

            return cb;
        }

        private void cboWeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboWeight.Text != "None")
            {
                cboCoClassify.Enabled = true;
                nudCoNClasses.Enabled = true;
            }
            else
            {
                cboCoClassify.Enabled = false;
                nudCoNClasses.Enabled = false;
            }
        }
    }
}
