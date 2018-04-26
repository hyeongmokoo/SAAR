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

namespace VisUncertainty
{
    public partial class frmDeleteField : Form
    {
        public IntPtr intHandle; // This is handle of Attribute table 
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public frmDeleteField()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;
                clistFields.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    clistFields.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clistFields.Items.Count; i++)
                clistFields.SetItemChecked(i, true);
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clistFields.Items.Count; i++)
                clistFields.SetItemChecked(i, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;
                if (clistFields.CheckedItems.Count > 0)
                {
                    for (int i = 0; i < clistFields.CheckedItems.Count; i++)
                    {
                        pSnippet.DeleteField(pFClass, (string)clistFields.CheckedItems[i]);
                    }
                }
                else
                    MessageBox.Show("Select Fields to delete");

                MessageBox.Show("Done");
                if (intHandle != IntPtr.Zero)
                {
                    frmAttributeTable pfrmAttributeTable = pSnippet.returnAttTable(intHandle);
                    if (pfrmAttributeTable == null)
                        return;
                    pSnippet.LoadingAttributeTable(pLayer, pfrmAttributeTable);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

    }
}
