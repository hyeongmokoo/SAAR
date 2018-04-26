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
    public partial class frmCreateWeightMatrix : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;

        public frmCreateWeightMatrix()
        {
            try
            {
                InitializeComponent();
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)pActiveView.FocusMap.get_Layer(i);
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        cboTargetLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clsSnippet pSnippet = new clsSnippet();
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboFieldName.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboFieldName.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if(sfdSaveFile.ShowDialog()==DialogResult.OK)
                txtOutput.Text=string.Concat(sfdSaveFile.FileName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;
                string strIDFldName = cboFieldName.Text;
                string strOutputName = txtOutput.Text;
                bool blnRook = true;

                if (strLayerName == "" || strIDFldName == "" || strOutputName == "")
                {
                    MessageBox.Show("Please select layer or fields");
                    return;
                }
                clsSnippet pSnippet = new clsSnippet();
                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;
                IFeatureCursor pFCursor = pFClass.Search(null, false);

                IFeature pFeature = pFCursor.NextFeature();
                int intIDIdx = pFCursor.FindField(strIDFldName);

                ISpatialFilter pSF = new SpatialFilterClass();


                IFeatureCursor pNBCursor = null;
                IFeature pNBFeature = null;
                int NBNumber = 0;
                string strNBIDs = null;

                ITopologicalOperator pTopoOp = (ITopologicalOperator)pFeature.Shape;
                pSF.Geometry = pFeature.ShapeCopy;
                pSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                IPointCollection pPointCol;

                System.IO.StreamWriter pSW = new System.IO.StreamWriter(strOutputName);
                pSW.WriteLine("0 " + pFClass.FeatureCount(null).ToString() + " " + pFClass.AliasName + " " + strIDFldName);


                while (pFeature != null)
                {
                    NBNumber = 0;
                    strNBIDs = null;

                    pSF.Geometry = pFeature.ShapeCopy;
                    pSF.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelIntersects;

                    pNBCursor = pFClass.Search(pSF, true);
                    pNBFeature = pNBCursor.NextFeature();

                    while (pNBFeature != null)
                    {
                        if (pFeature.get_Value(intIDIdx).Equals(pNBFeature.get_Value(intIDIdx)))
                            //if(pFeature.get_Value(intIDIdx) == pNBFeature.get_Value(intIDIdx))
                            pNBFeature = pNBCursor.NextFeature();
                        else
                        {
                            NBNumber = NBNumber + 1;
                            pPointCol = (IPointCollection)pTopoOp.Intersect(pNBFeature.Shape, esriGeometryDimension.esriGeometry0Dimension);
                            if (blnRook)
                            {
                                if (pPointCol.PointCount != 1)
                                    strNBIDs = strNBIDs + " " + pNBFeature.get_Value(intIDIdx);
                                else
                                    NBNumber -= 1;
                            }
                            else
                                strNBIDs = strNBIDs + " " + pNBFeature.get_Value(intIDIdx);

                            pNBFeature = pNBCursor.NextFeature();
                        }
                    }

                    pNBCursor.Flush();

                    if (NBNumber > 0)
                    {
                        pSW.WriteLine(pFeature.get_Value(intIDIdx).ToString() + " " + NBNumber.ToString());
                        pSW.WriteLine(strNBIDs.Substring(1));
                    }
                    else
                    {
                        pSW.WriteLine(pFeature.get_Value(intIDIdx).ToString() + " " + NBNumber.ToString());
                        pSW.WriteLine("");
                    }

                    pFeature = pFCursor.NextFeature();

                }

                pSW.Close();
                pSW.Dispose();

                MessageBox.Show(strOutputName + " is generated.");
                //this.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
