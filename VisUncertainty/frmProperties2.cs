using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;
using System.Drawing.Drawing2D;

namespace VisUncertainty
{
    public partial class frmProperties2 : Form
    {
        public ILayer mlayer;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_fLayer;

        public frmProperties2()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmProperties2_Load(object sender, EventArgs e)
        {
            try
            {
                //mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                //m_mapControl = (IMapControl3)mForm.axMapControl1.Object;
                //mlayer = (ILayer)m_mapControl.CustomProperty;
                //m_pSnippet = new clsSnippet();
                m_fLayer = (IFeatureLayer)mlayer;
                m_pFClass = (IFeatureClass)m_fLayer.FeatureClass;

                IGeoDataset GeoDataset = (IGeoDataset)m_pFClass;
                ISpatialReference pSpatialReference = GeoDataset.SpatialReference;
                ESRI.ArcGIS.Geodatabase.IDataset dataset = (ESRI.ArcGIS.Geodatabase.IDataset)(m_fLayer);


                this.Text = mlayer.Name + " Properties";
                lblLayerName.Text = mlayer.Name;
                lblLayerProjection.Text = pSpatialReference.Name;
                txtFilePath.Text = dataset.Workspace.PathName;

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
