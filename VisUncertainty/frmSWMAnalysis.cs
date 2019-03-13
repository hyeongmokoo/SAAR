using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using RDotNet;
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
    public partial class frmSWMAnalysis : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private double[,] m_arrXYCoord; //For Brushing and Linking

        private bool m_blnSubset = false;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;
        private REngine m_pEngine;

        //Varaibles for SWM
        private IFeatureLayer m_pClippedPolygon;

        public frmSWMAnalysis()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);

                    //IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    //if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    //    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
                m_pEngine = m_pForm.pEngine;
                try
                {
                    m_pEngine.Evaluate("library(spdep); library(maptools)");
                }
                catch
                {
                    MessageBox.Show("Please checked R packages installed in your local computer.");
                }

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
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

                int intFeatureCnt = m_pFClass.FeatureCount(null);
                
                //New Spatial Weight matrix function 083117 HK
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                {
                    //txtSWM.Text = pSWMType.strPolySWM;
                    txtSWM.DataSource = pSWMType.strPolyDefs;

                    lblClip.Visible = false; btnSubset.Visible = false; btnSubset.Enabled = false;
                    chkCumulate.Visible = true;chkCumulate.Text = "Cumulate neighbors:";
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
                    MessageBox.Show("Spatial weight matrix for polyline is not supported.");
                    btnSubset.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        

        private void btnSubset_Click(object sender, EventArgs e)
        {
            try
            {

                //string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

                //m_pEngine.Evaluate("sample.shp <- readShapePoints('" + strNameR + "')");

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

        private void btnOpenSWM_Click(object sender, EventArgs e)
        {
            if (ofdOpenSWM.ShowDialog() == DialogResult.OK)
                txtSWM.Text = string.Concat(ofdOpenSWM.FileName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            try
            {
                
            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Processing:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            int nFeature = m_pFClass.FeatureCount(null);
            double dblAdvancedValue = Convert.ToDouble(nudAdvanced.Value);
            //Plot command for R
            StringBuilder plotCommmand = new StringBuilder();

            //Get the file path and name to create spatial weight matrix
            string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

            if (strNameR == null)
                return;
            int intSuccess = 0;
            //Create spatial weight matrix in R
            if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
            {
                m_pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
                intSuccess = m_pSnippet.ExploreSpatialWeightMatrix1(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, dblAdvancedValue, chkCumulate.Checked);

            }
            else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
            {
                m_pEngine.Evaluate("sample.shp <- readShapePoints('" + strNameR + "')");
                intSuccess = m_pSnippet.ExploreSpatialWeightMatrixPts(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress, dblAdvancedValue, chkCumulate.Checked, m_pClippedPolygon);
            }
            else
            {
                MessageBox.Show("This geometry type is not supported");
                pfrmProgress.Close();
                this.Close();
            }

            if (intSuccess == 0)
                return;


            IFeatureCursor pFCursor = m_pFClass.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get variable index            
            int intFIDIdx = m_pFClass.FindField(m_pFClass.OIDFieldName); // Collect FIDs to apply Brushing and Linking
            int[] arrFID = new int[nFeature];

            int i = 0;

            m_arrXYCoord = new double[nFeature, 2];
            List<int>[] NBIDs = new List<int>[nFeature];

            IArea pArea;
            IPoint pPoint;

            while (pFeature != null)
            {
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    pArea = (IArea)pFeature.Shape;
                    m_arrXYCoord[i, 0] = pArea.Centroid.X;
                    m_arrXYCoord[i, 1] = pArea.Centroid.Y;
                }
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    pPoint = (IPoint)pFeature.Shape;
                    m_arrXYCoord[i, 0] = pPoint.X;
                    m_arrXYCoord[i, 1] = pPoint.Y;
                }
                NBIDs[i] = m_pEngine.Evaluate("sample.nb[[" + (i + 1).ToString() + "]]").AsInteger().ToList();

                arrFID[i] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
                i++;
                pFeature = pFCursor.NextFeature();
            }
            pFCursor.Flush();

            //Save Def_SWM
            clsSnippet.Def_SpatialWeightsMatrix pDefSWM = new clsSnippet.Def_SpatialWeightsMatrix();
            pDefSWM.Geometry = m_pFClass.ShapeType;
            pDefSWM.Definition = txtSWM.Text;
            pDefSWM.AdvancedValue = dblAdvancedValue;
            pDefSWM.Cumulative = chkCumulate.Checked;
            pDefSWM.Subset = m_blnSubset;

            pDefSWM.FIDs = arrFID;
            pDefSWM.NBIDs = NBIDs;
            pDefSWM.XYCoord = m_arrXYCoord;

            m_pEngine.Evaluate("c.nb <- card(sample.nb)");
            pDefSWM.FeatureCount = nFeature;
            pDefSWM.NeighborCounts = m_pEngine.Evaluate("c.nb").AsNumeric().ToArray();
            pDefSWM.NonZeroLinkCount = Convert.ToInt32(pDefSWM.NeighborCounts.Sum());
            pDefSWM.PercentNonZeroWeight = Convert.ToDouble(pDefSWM.NonZeroLinkCount) / Math.Pow(nFeature, 2) * 100;
            pDefSWM.AverageNumberofLink = pDefSWM.NeighborCounts.Average();



            //For higher order;
            if (m_pFClass.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                if (dblAdvancedValue > 1)
                {
                    int intMaxLag = Convert.ToInt32(dblAdvancedValue);
                    pDefSWM.LinkCountforHigher = new double[intMaxLag];
                    pDefSWM.AverageforHigher = new double[intMaxLag];

                    for (int j = 0; j < intMaxLag; j++)
                    {
                        m_pEngine.Evaluate("h.nb <- card(sample.nblags[[" + (j+1).ToString() + "]])");
                        pDefSWM.LinkCountforHigher[j] = m_pEngine.Evaluate("sum(h.nb)").AsNumeric().First();
                        pDefSWM.AverageforHigher[j] = m_pEngine.Evaluate("mean(h.nb)").AsNumeric().First();
                    }
                }
            }

            frmSWMSummary pSMWSummary = new frmSWMSummary();
            pSMWSummary.Def_SWM = pDefSWM;
            pSMWSummary.txtLayer.Text = m_pFLayer.Name;
            pSMWSummary.m_pFLayer = m_pFLayer;
            pSMWSummary.Show();

            pfrmProgress.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void txtSWM_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();

                if (txtSWM.Text == pSWMType.strPointDef[0])
                {

                    lblClip.Visible = true;lblClip.Enabled = true;
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
    }
}
