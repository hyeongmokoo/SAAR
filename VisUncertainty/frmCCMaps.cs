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
    public partial class frmCCMaps : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private IFeatureLayer m_pFLayer;

        public frmCCMaps()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
                m_pFLayer = null;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
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

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = m_pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboVariable.Items.Clear();
                cboHorCon.Items.Clear();
                cboVerCon.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboVariable.Items.Add(fields.get_Field(i).Name);
                        cboHorCon.Items.Add(fields.get_Field(i).Name);
                        cboVerCon.Items.Add(fields.get_Field(i).Name);
                    }
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_pFLayer == null)
                    return;
                if (cboVariable.Text == "" || cboHorCon.Text == "" || cboVerCon.Text == "")
                    return;

                frmCCMapsResults pfrmCCMapResults = new frmCCMapsResults();
                pfrmCCMapResults.strHorConFldName = cboHorCon.Text;
                pfrmCCMapResults.strVerConFldName = cboVerCon.Text;
                pfrmCCMapResults.strVarFldName = cboVariable.Text;
                pfrmCCMapResults.Text = "C-C maps of " + m_pFLayer.Name;
                pfrmCCMapResults.intHorCnt = Convert.ToInt32(nudHor.Value);
                pfrmCCMapResults.intVerCnt = Convert.ToInt32(nudVer.Value);


                pfrmCCMapResults.pFLayer = m_pFLayer;

                pfrmCCMapResults.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
