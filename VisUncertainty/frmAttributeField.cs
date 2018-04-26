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

namespace VisUncertainty
{
    public partial class frmAttributeField : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public double[] arrValue;

        public frmAttributeField()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboSourceLayer.Text;
                cboValueField.Text = "";

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboValueField.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboValueField.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboValueField_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strTargetLayerName = cboSourceLayer.Text;
                string strValueField = cboValueField.Text;

                int intTLayerIdx = pSnippet.GetIndexNumberFromLayerName(pActiveView, strTargetLayerName);

                ILayer pLayer = mForm.axMapControl1.get_Layer(intTLayerIdx);
                IFeatureLayer pFLayer = (IFeatureLayer)pLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;
                ICursor pCursor = (ICursor)pFLayer.Search(null, false);
                IRow pRow = pCursor.NextRow();
                arrValue = new double[pFClass.FeatureCount(null)];
                int intValueIdx = pFClass.FindField(strValueField);
                int i = 0;
                while (pRow != null)
                {
                    arrValue[i] = Convert.ToDouble(pRow.get_Value(intValueIdx));
                    i++;
                    pRow = pCursor.NextRow();
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
