using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
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
    public partial class frmHCluster : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;
        private string[] m_strFieldName;

        public frmHCluster()
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
            m_pEngine.Evaluate("library(maptools); library(fpc)");
            //m_pEngine.Evaluate("library(maptools)");

        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                
                IFields fields = m_pFClass.Fields;


                List<string> lstFieldname = new List<string>();

                
                while (dgvVariables.Rows.Count > 1)
                {
                    dgvVariables.Rows.RemoveAt(0);
                }
                

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        lstFieldname.Add(fields.get_Field(i).Name);
                    }
                }
                m_strFieldName = lstFieldname.ToArray();
                DataGridViewComboBoxColumn pVarColumn = (DataGridViewComboBoxColumn) dgvVariables.Columns[0];
                pVarColumn.Items.AddRange(m_strFieldName);

                DataGridViewComboBoxColumn pErrorColumn = (DataGridViewComboBoxColumn)dgvVariables.Columns[1];
                pErrorColumn.Items.AddRange(m_strFieldName);
                
            //}
            //catch (Exception ex)
            //{
            //    frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
            //    return;
            //}
        }

        public class BhattaVariables
        {
            public string variableNames { get; set; }
            public string errorNames { get; set; }
            public int idVar { get; set; }
            public int idError { get; set; }
            public double[] arrVar { get; set; }
            public double[] arrError { get; set; }
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Collecting Data:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            //Get number of variables            
            int intnVar = dgvVariables.Rows.Count - 1;

            for(int j =0; j< intnVar; j++)
            {
                if(dgvVariables.Rows[j].Cells[0].Value.ToString()==""|| dgvVariables.Rows[j].Cells[1].Value.ToString() == "")
                {
                    MessageBox.Show("Please select both variable and standard errors.", "Errors");
                    return;
                }
            }


            clsSnippet pSnippet = new clsSnippet();

            
            // Gets the variable names and indices
            BhattaVariables[] pBVariable = new BhattaVariables[intnVar];
            int nFeature = m_pFClass.FeatureCount(null);

            for (int j = 0; j < intnVar; j++)
            {
                pBVariable[j].variableNames = (string)dgvVariables.Rows[j].Cells[0].Value;
                pBVariable[j].errorNames = (string)dgvVariables.Rows[j].Cells[1].Value;

                pBVariable[j].idVar = m_pFClass.Fields.FindField(pBVariable[j].variableNames);
                pBVariable[j].idError = m_pFClass.Fields.FindField(pBVariable[j].errorNames);

                pBVariable[j].arrVar = new double[nFeature];
                pBVariable[j].arrError = new double[nFeature];
            }

            //Get values of var and error from shp file
            IFeatureCursor pFCursor = m_pFClass.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();
            
            //Store independent values at Array
            
            int i = 0;
            while (pFeature != null)
            {
                for (int j = 0; j < intnVar; j++)
                {
                    //arrInDepen[j] = new double[nFeature];
                    pBVariable[j].arrVar[i] = Convert.ToDouble(pFeature.get_Value(pBVariable[j].idVar));
                    pBVariable[j].arrError[i] = Convert.ToDouble(pFeature.get_Value(pBVariable[j].idError));
                }

                i++;
                pFeature = pFCursor.NextFeature();
            }

            pfrmProgress.lblStatus.Text = "Creating Distance Matrix";

            //Create Matrix for Distance calculation
            m_pEngine.Evaluate("Bhatta.diss < -matrix(0, " + nFeature.ToString() + ", " + nFeature.ToString()+")");
            StringBuilder[] plotCommmand = new StringBuilder[4];

            //Need to optimize 12132017 HK
            for(int k = 0; k < nFeature; k++)
            {
                for (int l = 0; l < nFeature; l++)
                {
                    plotCommmand[0].Append("x1 < -cbind(");
                    plotCommmand[1].Append("x2 < -cbind(");
                    plotCommmand[2].Append("v1 < -cbind(");
                    plotCommmand[3].Append("v2 < -cbind(");

                    for (int j = 0; j < intnVar; j++)
                    {
                        plotCommmand[0].Append(pBVariable[j].arrVar[k].ToString()+", ");
                        plotCommmand[1].Append(pBVariable[j].arrVar[l].ToString() + ", ");
                        plotCommmand[2].Append(pBVariable[j].arrError[k].ToString() + ", ");
                        plotCommmand[3].Append(pBVariable[j].arrError[k].ToString() + ", ");
                    }

                    for (int j=0; j < 4; j++)
                    {
                        plotCommmand[j].Remove(plotCommmand[j].Length - 2, 2);
                        plotCommmand[j].Append(")");
                        m_pEngine.Evaluate(plotCommmand[j].ToString());
                    }

                    m_pEngine.Evaluate("Bhatta.diss[" + k.ToString() + ", " + l.ToString() + "] < -Bhatta.mdist(x1, x2, v1, v2)");
                }
            }

            pfrmProgress.lblStatus.Text = "Finding Clusters";

            m_pEngine.Evaluate("sample.hclu < -hclust(as.dist(Bhatta.diss))");

            string strTitle = "Dendrogram";
            string strCommand = "plot(plot(sample.hclu));";
            pSnippet.drawPlottoForm(strTitle, strCommand);

            int intNClasses = Convert.ToInt32(nudNClasses.Value);
            m_pEngine.Evaluate("sample.cut <- cutree(sample.hclu, "+intNClasses.ToString()+")");

            //Save Outputs in SHP

                //The field names are related with string[] DeterminedName in clsSnippet 
                string strOutputFldName = lstSave.Items[0].SubItems[1].Text;

                //Get EVs and residuals
                NumericVector nvResiduals = m_pEngine.Evaluate("as.numeric(sum.lm$residuals)").AsNumeric();

                // Create field, if there isn't
                if (m_pFClass.FindField(strOutputFldName) == -1)
                {
                    //Add fields
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strOutputFldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    m_pFClass.AddField(newField);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to overwrite " + strOutputFldName + " field?", "Overwrite", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }


                //Update Field
                pFCursor.Flush();
                pFCursor = m_pFClass.Update(null, false);
                pFeature = pFCursor.NextFeature();

                int featureIdx = 0;
                int intResiFldIdx = m_pFClass.FindField(strOutputFldName);

                while (pFeature != null)
                {
                    //Update Residuals
                    pFeature.set_Value(intResiFldIdx, (object)nvResiduals[featureIdx]);

                    pFCursor.UpdateFeature(pFeature);

                    pFeature = pFCursor.NextFeature();
                    featureIdx++;
                }

                MessageBox.Show("Residuals are stored in the shape file");
            }

            pfrmProgress.Close();



        }
    }
}
