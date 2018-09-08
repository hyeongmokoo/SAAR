using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RDotNet;

namespace VisUncertainty
{
    public partial class frmCreateSWM_inR : Form
    {
        private IFeatureClass m_pFClass;
        private bool blnCorrelogram; //For removing higher order option for correlogram

        private clsSnippet m_pSnippet;
        private MainForm m_pForm;
        //private IActiveView m_pActiveView;
        private REngine m_pEngine;

        //Varaibles for SWM
        private IFeatureLayer m_pClippedPolygon;
        private bool m_blnSubset = false;

        //Output
       
        private IActiveView m_pActiveView;
        private IFeatureLayer m_pFLayer;

        public frmCreateSWM_inR()
        {
            InitializeComponent();
            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            m_pActiveView = m_pForm.axMapControl1.ActiveView;

            for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
            {
                cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
            }

            m_pSnippet = new clsSnippet();
            m_pEngine = m_pForm.pEngine;
            m_pEngine.Evaluate("library(spdep); library(maptools); library(MASS)");
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                IFields fields = m_pFClass.Fields;

                cboFieldName.Items.Clear();
                cboFieldName.Text = "";
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                    }
                }
                
                //New Spatial Weight matrix function 080317
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                {
                    //txtSWM.Text = pSWMType.strPolySWM;
                    txtSWM.DataSource = pSWMType.strPolyDefs;

                    lblClip.Visible = false; btnSubset.Visible = false; btnSubset.Enabled = false;
                    chkCumulate.Visible = true; chkCumulate.Text = "Cumulate neighbors:";
                    lblAdvanced.Text = "Contiguity Order:";
                    nudAdvanced.Value = 1; nudAdvanced.Increment = 1; nudAdvanced.DecimalPlaces = 0;
                }
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    //txtSWM.Text = pSWMType.strPointSWM;
                    txtSWM.DataSource = pSWMType.strPointDef;
                    lblClip.Visible = true; btnSubset.Visible = true; btnSubset.Enabled = true;
                    chkCumulate.Visible = false;
                    lblAdvanced.Text = "Threshold distance:";
                    nudAdvanced.Value = 100; nudAdvanced.Increment = 10; nudAdvanced.DecimalPlaces = 2;
                }
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    MessageBox.Show("Spatial weights matrix for polyline is not supported.");
                    btnSubset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (sfdSaveFile.ShowDialog() == DialogResult.OK)
                txtOutput.Text = string.Concat(sfdSaveFile.FileName);
        }

        private void txtSWM_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();

                if (txtSWM.Text == pSWMType.strPointDef[0])
                {

                    lblClip.Visible = true; lblClip.Enabled = true;
                    btnSubset.Visible = true; btnSubset.Enabled = true;
                    chkCumulate.Visible = false;
                    lblAdvanced.Text = "Threshold distance:";
                    lblAdvanced.Enabled = false;
                    nudAdvanced.Value = 100; nudAdvanced.Increment = 10; nudAdvanced.DecimalPlaces = 2;
                    nudAdvanced.Enabled = false;

                }
                else if (txtSWM.Text == pSWMType.strPointDef[1])
                {
                    lblClip.Visible = true; btnSubset.Visible = true; btnSubset.Enabled = true;
                    chkCumulate.Visible = false;
                    lblAdvanced.Text = "Threshold distance:";
                    lblAdvanced.Enabled = true;
                    nudAdvanced.Value = 100; nudAdvanced.Increment = 10; nudAdvanced.DecimalPlaces = 2;
                    nudAdvanced.Enabled = true;
                    btnSubset.Enabled = false; btnSubset.Visible = false;
                    lblClip.Enabled = false; lblClip.Visible = false;
                    //if (m_dblDefaultDistThres != null)
                    //    nudAdvanced.Value = Convert.ToDecimal(m_dblDefaultDistThres);
                }
                else if (txtSWM.Text == pSWMType.strPointDef[2])
                {
                    lblClip.Visible = true; btnSubset.Visible = true; btnSubset.Enabled = true;
                    chkCumulate.Visible = false;
                    lblAdvanced.Text = "Number of neighbors:";
                    lblAdvanced.Enabled = true;
                    nudAdvanced.Value = 4; nudAdvanced.Increment = 1; nudAdvanced.DecimalPlaces = 0;
                    nudAdvanced.Enabled = true;
                    btnSubset.Enabled = false; btnSubset.Visible = false;
                    lblClip.Enabled = false; lblClip.Visible = false;
                }
                else
                {
                    lblClip.Visible = false; btnSubset.Visible = false; btnSubset.Enabled = false;
                    chkCumulate.Enabled = false; chkCumulate.Visible = true;
                    lblAdvanced.Enabled = true; lblAdvanced.Text = "Contiguity Order:";
                    nudAdvanced.Enabled = true; nudAdvanced.Value = 1; nudAdvanced.Increment = 1; nudAdvanced.DecimalPlaces = 0;

                    int intT = pSWMType.strPolyDefs.Length;
                    for (int i = 0; i < intT; i++)
                    {
                        if (txtSWM.Text == pSWMType.strPolyDefs[i])
                            return;
                    }
                    btnSubset.Enabled = false;
                    chkCumulate.Enabled = false;
                    lblAdvanced.Enabled = false;
                    nudAdvanced.Enabled = false;
                    lblClip.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void nudAdvanced_ValueChanged(object sender, EventArgs e)
        {
            if (m_pFClass == null)
                return;

            if (m_pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                if (nudAdvanced.Value > 1)
                {
                    chkCumulate.Enabled = true;
                }
                else if (nudAdvanced.Value == 1)
                    chkCumulate.Enabled = false;
            }
        }

        private void btnSubset_Click(object sender, EventArgs e)
        {
            try
            {
                frmSubsetPoly pfrmSubsetPoly = new frmSubsetPoly();
                pfrmSubsetPoly.ShowDialog();
                m_blnSubset = pfrmSubsetPoly.m_blnSubset;
                if (m_blnSubset)
                {
                    m_pClippedPolygon = pfrmSubsetPoly.m_pClipPolygon; ;
                    chkCumulate.Visible = true;
                    chkCumulate.Text = "Clipped by '" + m_pClippedPolygon.Name + "'";
                    chkCumulate.Checked = true;
                    chkCumulate.Enabled = true;
                }
                else
                {
                    chkCumulate.Visible = false;
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {

                if (cboFieldName.Text == "")
                {
                    MessageBox.Show("Please select an ID field to be used as region ID.",
        "Please choose an ID field");
                    return;
                }


                if (txtOutput.Text == "")
                {
                    MessageBox.Show("Please specify path and file name.",
        "Please specify a path");
                    return;
                }


                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Processing:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();

                //Create Row ID vectors
                int nFeature = m_pFClass.FeatureCount(null);

                //Get index for independent and dependent variables
                string strIDfield = cboFieldName.Text;
                int intIDIdx = m_pFClass.Fields.FindField(strIDfield);

                double[] arrRowID = new double[nFeature];

                int i = 0;
                IFeatureCursor pFCursor = m_pFClass.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();
                while (pFeature != null)
                {
                    arrRowID[i] = Convert.ToDouble(pFeature.get_Value(intIDIdx));

                    i++;
                    pFeature = pFCursor.NextFeature();
                }
                NumericVector vecRowID = m_pEngine.CreateNumericVector(arrRowID);
                m_pEngine.SetSymbol("sample.ids", vecRowID);

                //Get the file path and name to create spatial weight matrix
                //Input
                string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);
                if (strNameR == null)
                    return;

                //Shp name
                IDataset dataset = (IDataset)(m_pFLayer);
                string strShpname = dataset.BrowseName;
                if (dataset.Category == "Shapefile Feature Class")
                    strShpname = strShpname + ".shp";

                //Output
                string strOutput = m_pSnippet.FilePathinRfromText(txtOutput.Text);

                int intSuccess = 0;

                //Create spatial weight matrix in R
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    m_pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
                    intSuccess = m_pSnippet.CreateSpatialWeightMatrixPolywithID(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, Convert.ToDouble(nudAdvanced.Value), chkCumulate.Checked);

                }
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    m_pEngine.Evaluate("sample.shp <- readShapePoints('" + strNameR + "')");
                    //intSuccess = m_pSnippet.ExploreSpatialWeightMatrix1(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, Convert.ToDouble(nudAdvanced.Value), chkCumulate.Checked);
                    intSuccess = m_pSnippet.CreateSpatialWeightMatrixPtswithID(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, Convert.ToDouble(nudAdvanced.Value), chkCumulate.Checked, m_pClippedPolygon);

                    //chkCumulate.Visible = false;
                }
                else
                {
                    MessageBox.Show("This geometry type is not supported");
                    pfrmProgress.Close();
                    this.Close();
                }

                if (intSuccess == 0)
                {
                    MessageBox.Show("Fail to create spatial weights.", "Fail");
                    return;
                }
                else
                {
                    m_pEngine.Evaluate("write.nb.gal(sample.nb, '" + strOutput + "', oldstyle = F, shpfile = '" + strShpname + "', ind = '" + strIDfield + "')");
                    MessageBox.Show("A spatial weights file is successfuly created in " + strOutput, "Success");
                }


                pfrmProgress.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
