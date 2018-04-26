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
    public partial class frmAddField : Form
    {
        public IntPtr intHandle; // This is handle of Attribute table 

        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public frmAddField()
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (cboType.Text)
                {
                    case "Short Integer":
                    case "Long Integer":
                        lblOption1.Text = "Precision:";
                        lblOption1.Visible = true;
                        lblOption2.Visible = false;

                        nudOption1.Visible = true;
                        nudOption2.Visible = false;
                        break;
                    case "Float":
                    case "Double":
                        lblOption1.Text = "Precision:";
                        lblOption2.Text = "Scale";
                        lblOption1.Visible = true;
                        lblOption2.Visible = true;

                        nudOption1.Visible = true;
                        nudOption2.Visible = true;
                        break;
                    case "Text":
                        lblOption1.Text = "Length:";
                        lblOption1.Visible = true;
                        lblOption2.Visible = false;

                        nudOption1.Visible = true;
                        nudOption2.Visible = false;
                        break;
                    case "Date":
                        lblOption1.Visible = false;
                        lblOption2.Visible = false;

                        nudOption1.Visible = false;
                        nudOption2.Visible = false;
                        break;
                }
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboLayer.Text;
                string strFieldType = cboType.Text;
                string strFieldName = txtName.Text;

                if (strLayerName == "" || strFieldType == "" || strFieldName == "")
                    MessageBox.Show("Please select variables");

                clsBlockNames pBlockNames = new clsBlockNames();
                if (pBlockNames.BlockPreDeterminedName(strFieldName))
                    return;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;
                int intFldIdx = pFClass.FindField(strFieldName);
                if (intFldIdx != -1)
                {
                    MessageBox.Show("The field name is already assigned.");
                    return;
                }

                AddField(pFClass, strFieldName, strFieldType);
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
        private void AddField(IFeatureClass fClass, string name, string strType)
        {
            try
            {
                IField newField = new FieldClass();
                IFieldEdit fieldEdit = (IFieldEdit)newField;
                fieldEdit.Name_2 = name;
                switch (strType)
                {
                    case "Short Integer":
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
                        if ((int)nudOption1.Value != 0)
                            fieldEdit.Precision_2 = (int)nudOption1.Value;
                        break;
                    case "Long Integer":
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                        if ((int)nudOption1.Value != 0)
                            fieldEdit.Precision_2 = (int)nudOption1.Value;
                        break;
                    case "Float":
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeSingle;
                        if ((int)nudOption1.Value != 0)
                            fieldEdit.Precision_2 = (int)nudOption1.Value;
                        if ((int)nudOption2.Value != 0)
                            fieldEdit.Scale_2 = (int)nudOption2.Value;
                        break;
                    case "Double":
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        if ((int)nudOption1.Value != 0)
                            fieldEdit.Precision_2 = (int)nudOption1.Value;
                        if ((int)nudOption2.Value != 0)
                            fieldEdit.Scale_2 = (int)nudOption2.Value;
                        break;
                    case "Text":
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        if ((int)nudOption1.Value != 0)
                            fieldEdit.Length_2 = (int)nudOption1.Value;
                        break;
                    case "Date":
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeDate;
                        break;
                }

                fClass.AddField(newField);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

    }
}
