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


namespace VisUncertainty
{
    public partial class frmBlinking : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private ITrackCancel pTrackCancel;
        private clsSnippet pSnippet;
        private IFeatureLayer pFLayer;

        public frmBlinking()
        {
            try
            {
                InitializeComponent();


                pSnippet = new clsSnippet();
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboSourceLayer.Text;
                string strFieldName = cboWeight.Text;
                int intFlickerRate = Convert.ToInt32(Convert.ToDouble(nudFlikerRate.Value) * 1000);
                short intTransparency = Convert.ToInt16(nudTransparency.Value);
                btnStart.Text = "Hit Esc to Stop";

                if (strLayerName == "" || strFieldName == "")
                {
                    MessageBox.Show("Please select layer or field");
                    return;
                }
                if (strFieldName == "None")
                {

                    int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                    ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                    pFLayer = pLayer as IFeatureLayer;
                    string strOIDFieldNM = pFLayer.FeatureClass.OIDFieldName;

                    int intFeatureCounts = pFLayer.FeatureClass.FeatureCount(null);

                    pTrackCancel = new CancelTrackerClass();
                    pTrackCancel.CancelOnClick = false;
                    pTrackCancel.CancelOnKeyPress = true;

                    ILayerEffects pLayerEffect = (ILayerEffects)pFLayer;
                    pLayerEffect.Transparency = intTransparency;
                    //pActiveView.Refresh();

                    while (pTrackCancel.Continue())
                    {
                        IFeatureLayerDefinition2 pFDefinition = (IFeatureLayerDefinition2)pFLayer;
                        pFDefinition.DefinitionExpression = strOIDFieldNM + " < 0";
                        pActiveView.Refresh();
                        System.Threading.Thread.Sleep(intFlickerRate);
                        if (pTrackCancel.Continue() == false)
                        {
                            pFDefinition.DefinitionExpression = strOIDFieldNM + " >= 0";
                            pActiveView.Refresh();
                            btnStart.Text = "Start";
                            pLayerEffect.Transparency = 0;
                            break;
                        }
                        pFDefinition.DefinitionExpression = strOIDFieldNM + " >= 0";
                        pActiveView.Refresh();
                        System.Threading.Thread.Sleep(intFlickerRate);
                        if (pTrackCancel.Continue() == false)
                        {
                            pLayerEffect.Transparency = 0;
                            btnStart.Text = "Start";
                            break;
                        }
                    }
                }
                else
                {
                    if (strFieldName == "") return;
                    int intNClasses = Convert.ToInt32(nudCoNClasses.Value);
                    double[] cb = fnClassification(pFLayer, nudCoNClasses.Value, cboWeight.Text, cboCoClassify.Text);
                    fnBlinking(strLayerName, strFieldName, intFlickerRate, intNClasses, intTransparency, cb);

                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }


        private void fnBlinking(string strLayerName, string strFieldName, int intFlickerRate, int intNClasses, short srtTransparency, double[] cb)
        {
            try
            {
                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                pFLayer = pLayer as IFeatureLayer;

                pTrackCancel = new CancelTrackerClass();
                pTrackCancel.CancelOnClick = false;
                pTrackCancel.CancelOnKeyPress = true;

                ILayerEffects pLayerEffect = (ILayerEffects)pFLayer;
                pLayerEffect.Transparency = srtTransparency;

                while (pTrackCancel.Continue())
                {
                    IFeatureLayerDefinition2 pFDefinition = (IFeatureLayerDefinition2)pFLayer;

                    for (int i = 0; i < intNClasses; i++)
                    {
                        if(i==0)
                            pFDefinition.DefinitionExpression = strFieldName + " >= " + cb[i].ToString();
                        else
                            pFDefinition.DefinitionExpression = strFieldName + " > " + cb[i].ToString();

                        pActiveView.Refresh();
                        System.Threading.Thread.Sleep(intFlickerRate);
                        if (pTrackCancel.Continue() == false)
                        {
                            pFDefinition.DefinitionExpression = strFieldName + " >= " + cb[0].ToString();
                            pActiveView.Refresh();
                            btnStart.Text = "Start";
                            pLayerEffect.Transparency = 0;
                            break;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                pFLayer = pLayer as IFeatureLayer;
                IFields fields = pFLayer.FeatureClass.Fields;

                cboWeight.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboWeight.Items.Add(fields.get_Field(i).Name);
                }
                cboWeight.Items.Add("None");
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private double[] fnClassification(IFeatureLayer pFLayer, decimal NClasses, string strClassifiedField, string ClassifiedMethod)
        {
            try
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
                ITableHistogram pTableHistogram = new BasicTableHistogramClass();
                pTableHistogram.Field = strClassifiedField;
                pTableHistogram.Table = pTable;
                IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram;

                object xVals, frqs;
                pHistogram.GetHistogram(out xVals, out frqs);
                pClassifyGEN.Classify(xVals, frqs, intBreakeCount);
                double[] cb = (double[])pClassifyGEN.ClassBreaks;

                return cb;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
            }
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

        private void frmBlinking_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (pFLayer != null)
                {
                    ILayerEffects pLayerEffect = (ILayerEffects)pFLayer;
                    pLayerEffect.Transparency = 0;
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

    }
}
