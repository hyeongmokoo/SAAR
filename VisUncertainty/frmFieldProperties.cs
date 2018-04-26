using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;

namespace VisUncertainty
{
    public partial class frmFieldProperties : Form
    {
        public IField pField;

        public frmFieldProperties()
        {
            InitializeComponent();

        }

        private void frmFieldProperties_Load(object sender, EventArgs e)
        {
            try
            {
                txtName.Text = pField.Name;
                txtType.Text = pField.Type.ToString();

                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeSmallInteger:
                        txtType.Text = "Short Integer";
                        lblOption1.Text = "Precision:";
                        lblOption1.Visible = true;
                        lblOption2.Visible = false;

                        txtOption1.Visible = true;
                        txtOption1.Text = pField.Precision.ToString();
                        txtOption2.Visible = false;
                        break;
                    case esriFieldType.esriFieldTypeInteger:
                        txtType.Text = "Long Integer";
                        lblOption1.Text = "Precision:";
                        lblOption1.Visible = true;
                        lblOption2.Visible = false;

                        txtOption1.Visible = true;
                        txtOption1.Text = pField.Precision.ToString();
                        txtOption2.Visible = false;
                        break;
                    case esriFieldType.esriFieldTypeDouble:
                        txtType.Text = "Double";
                        lblOption1.Text = "Precision:";
                        lblOption2.Text = "Scale";
                        lblOption1.Visible = true;
                        lblOption2.Visible = true;

                        txtOption1.Visible = true;
                        txtOption1.Text = pField.Precision.ToString();
                        txtOption2.Visible = true;
                        txtOption2.Text = pField.Scale.ToString();
                        break;
                    case esriFieldType.esriFieldTypeSingle:
                        txtType.Text = "Float";
                        lblOption1.Text = "Precision:";
                        lblOption2.Text = "Scale";
                        lblOption1.Visible = true;
                        lblOption2.Visible = true;

                        txtOption1.Visible = true;
                        txtOption1.Text = pField.Precision.ToString();
                        txtOption2.Visible = true;
                        txtOption2.Text = pField.Scale.ToString();
                        break;
                    case esriFieldType.esriFieldTypeString:
                        txtType.Text = "Text";
                        lblOption1.Text = "Length:";
                        lblOption1.Visible = true;
                        lblOption2.Visible = false;

                        txtOption1.Visible = true;
                        txtOption1.Text = pField.Length.ToString();
                        txtOption2.Visible = false;
                        break;
                    case esriFieldType.esriFieldTypeDate:
                        txtType.Text = "Date";
                        lblOption1.Visible = false;
                        lblOption2.Visible = false;

                        txtOption1.Visible = false;
                        txtOption2.Visible = false;
                        break;
                    case esriFieldType.esriFieldTypeOID:
                        txtType.Text = "Object ID";
                        lblOption1.Visible = false;
                        lblOption2.Visible = false;

                        txtOption1.Visible = false;
                        txtOption2.Visible = false;
                        break;
                    case esriFieldType.esriFieldTypeGeometry:
                        txtType.Text = "Geometry";
                        lblOption1.Visible = false;
                        lblOption2.Visible = false;

                        txtOption1.Visible = false;
                        txtOption2.Visible = false;
                        break;
                    default:
                        txtType.Text = pField.Type.ToString();
                        lblOption1.Visible = false;
                        lblOption2.Visible = false;

                        txtOption1.Visible = false;
                        txtOption2.Visible = false;
                        break;
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
    }
}
