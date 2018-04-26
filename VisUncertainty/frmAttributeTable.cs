using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace VisUncertainty
{
    public partial class frmAttributeTable : Form
    {
        public ILayer m_pLayer;

        private clsSnippet m_pSnippet;
        private clsBrusingLinking m_pBL;
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private string m_strFldName;

        private DataGridViewColumn pdgvColumn;
        
        public frmAttributeTable()
        {
            try
            {
                InitializeComponent();
                m_pSnippet = new clsSnippet();
                m_pBL = new clsBrusingLinking();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void frmAttributeTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                IFeatureLayer pFLayer = (IFeatureLayer)m_pLayer;
                int intFCnt = m_pBL.RemoveBrushing(m_pForm, pFLayer);
                if (intFCnt == -1)
                    return;
                else if (intFCnt == 0)
                {
                    IFeatureSelection featureSelection = (IFeatureSelection)pFLayer;
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                    featureSelection.Clear();
                    m_pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        

        private void addFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmAddField pfrmAddField = new frmAddField();
                pfrmAddField.cboLayer.Text = m_pLayer.Name;
                pfrmAddField.intHandle = this.Handle;
                pfrmAddField.Show();
                
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void deleteFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmDeleteField pfrmDelField = new frmDeleteField();
                pfrmDelField.cboLayer.Text = m_pLayer.Name;
                pfrmDelField.intHandle = this.Handle;
                pfrmDelField.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void fieldCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmFieldCalculator pfrmFieldCalc = new frmFieldCalculator();
                pfrmFieldCalc.cboLayer.Text = m_pLayer.Name;
                pfrmFieldCalc.intHandle = this.Handle;
                pfrmFieldCalc.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void deleteFieldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmDeleteField pfrmDelField = new frmDeleteField();
                pfrmDelField.cboLayer.Text = m_pLayer.Name;
                for (int i = 0; i < pfrmDelField.clistFields.Items.Count; i++)
                {
                    if (pfrmDelField.clistFields.Items[i].ToString() == m_strFldName)
                        pfrmDelField.clistFields.SetItemChecked(i, true);
                }
                pfrmDelField.intHandle = this.Handle;
                pfrmDelField.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void dgvAttTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Right) return;
                else
                {
                    pdgvColumn = dgvAttTable.Columns[e.ColumnIndex];
                    m_strFldName = pdgvColumn.Name;
                    columnHeadContextMenu.Show(MousePosition);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void fieldCalculatorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                frmFieldCalculator pfrmFieldCalc = new frmFieldCalculator();
                pfrmFieldCalc.cboLayer.Text = m_pLayer.Name;
                pfrmFieldCalc.txtTargetField.Text = m_strFldName;
                pfrmFieldCalc.intHandle = this.Handle;
                pfrmFieldCalc.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void sortAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dgvAttTable.Sort(pdgvColumn, ListSortDirection.Ascending);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void sortDescendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dgvAttTable.Sort(pdgvColumn, ListSortDirection.Descending);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_pSnippet.drawHistogram(m_pLayer.Name, m_strFldName);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmFieldProperties pFldProperties = new frmFieldProperties();
                IFeatureLayer pFlayer = (IFeatureLayer)m_pLayer;
                IFeatureClass pFClass = pFlayer.FeatureClass;
                int intFldIdx = pFClass.FindField(m_strFldName);
                IField pField = pFClass.Fields.get_Field(intFldIdx);

                pFldProperties.pField = pField;
                pFldProperties.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void dgvAttTable_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                int intSelCounts = dgvAttTable.SelectedRows.Count;
                IFeatureLayer pFLayer = m_pLayer as IFeatureLayer;
                if (intSelCounts == 0)
                {

                    m_pSnippet.ClearSelectedMapFeatures(m_pActiveView, pFLayer);
                }
                else//Brushing on Map
                {
                    for (int i = 0; i < intSelCounts; i++)
                    {

                        string value1 = dgvAttTable.SelectedRows[i].Cells[0].Value.ToString();


                        string whereClause = "FID =" + value1;

                        if (m_pActiveView == null || pFLayer == null || whereClause == null || value1 == "")
                        {
                            return;
                        }
                        ESRI.ArcGIS.Carto.IFeatureSelection featureSelection = pFLayer as ESRI.ArcGIS.Carto.IFeatureSelection; // Dynamic Cast

                        // Set up the query
                        ESRI.ArcGIS.Geodatabase.IQueryFilter queryFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
                        queryFilter.WhereClause = whereClause;

                        // Invalidate only the selection cache. Flag the original selection
                        m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                        // Perform the selection
                        if (i == 0)
                            featureSelection.SelectFeatures(queryFilter, ESRI.ArcGIS.Carto.esriSelectionResultEnum.esriSelectionResultNew, false);
                        else
                            featureSelection.SelectFeatures(queryFilter, ESRI.ArcGIS.Carto.esriSelectionResultEnum.esriSelectionResultAdd, false);

                    }
                    // Flag the new selection
                    m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);

                    //Brushing to other graphs
                    m_pBL.BrushingToOthers(pFLayer, this.Handle);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
                
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int intDeciPlaces = 3;
                m_pSnippet.CalculateDesStat((IFeatureLayer)m_pLayer, m_strFldName, intDeciPlaces);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        
    }
}
