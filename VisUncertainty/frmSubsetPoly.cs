using ESRI.ArcGIS.Carto;
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


    public partial class frmSubsetPoly : Form
    {
        public MainForm m_pForm;
        public bool m_blnSubset;
        private clsSnippet m_pSnippet;
        private IActiveView m_pActiveView;
        public IFeatureLayer m_pFLayer;

        public IFeatureLayer m_pClipPolygon;
        //public string strPolygonNM;

        public frmSubsetPoly()
        {
            InitializeComponent();

            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            m_pActiveView = m_pForm.axMapControl1.ActiveView;

            for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
            }
            m_pSnippet = new clsSnippet();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            m_blnSubset = false;

            m_blnSubset = true;
            m_pClipPolygon = m_pFLayer;
            //pfrmProgress.Close();
            this.Close();

        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLayerName = cboTargetLayer.Text;

            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

            m_pFLayer = pLayer as IFeatureLayer;
            
        }
    }
}
