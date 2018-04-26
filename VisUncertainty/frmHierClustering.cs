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
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using RDotNet;


namespace VisUncertainty
{
    public partial class frmHierClustering : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;
        private string[] m_strFieldName;

        public frmHierClustering()
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
            DataGridViewComboBoxColumn pVarColumn = (DataGridViewComboBoxColumn)dgvVariables.Columns[0];
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

            for (int j = 0; j < intnVar; j++)
            {
                if (dgvVariables.Rows[j].Cells[0].Value.ToString() == "" || dgvVariables.Rows[j].Cells[1].Value.ToString() == "")
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
                //BhattaVariables tmp = new BhattaVariables();
                //tmp.variableNames = (string)dgvVariables.Rows[j].Cells[0].Value;
                //tmp.errorNames = (string)dgvVariables.Rows[j].Cells[1].Value;

                //tmp.idVar = m_pFClass.Fields.FindField(tmp.variableNames);
                //tmp.idError = m_pFClass.Fields.FindField(tmp.errorNames);

                //tmp.arrVar = new double[nFeature];
                //tmp.arrError = new double[nFeature];

                //pBVariable.Add(tmp);
                pBVariable[j] = new BhattaVariables();
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

            if(cboDistance.Text== "Bhattacharyya distance") //Bhatta dist
            {
                string strStartPath = m_pForm.strPath;
                string pathr = strStartPath.Replace(@"\", @"/");
                m_pEngine.Evaluate("source('" + pathr + "/clustering.R')");

                //Create Matrix for Distance calculation
                m_pEngine.Evaluate("Bhatta.diss <- matrix(0, " + nFeature.ToString() + ", " + nFeature.ToString() + ")");
                StringBuilder[] plotCommmand = new StringBuilder[4];

                //Need to optimize 12132017 HK
                for (int k = 0; k < nFeature; k++)
                {
                    for (int l = 0; l < k+1; l++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            plotCommmand[j] = new StringBuilder();
                        }

                        plotCommmand[0].Append("x1 <- cbind(");
                        plotCommmand[1].Append("x2 <- cbind(");
                        plotCommmand[2].Append("v1 <- cbind(");
                        plotCommmand[3].Append("v2 <- cbind(");

                        for (int j = 0; j < intnVar; j++)
                        {
                            plotCommmand[0].Append(pBVariable[j].arrVar[k].ToString() + ", ");
                            plotCommmand[1].Append(pBVariable[j].arrVar[l].ToString() + ", ");
                            plotCommmand[2].Append(pBVariable[j].arrError[k].ToString() + ", ");
                            plotCommmand[3].Append(pBVariable[j].arrError[l].ToString() + ", ");
                        }

                        for (int j = 0; j < 4; j++)
                        {
                            plotCommmand[j].Remove(plotCommmand[j].Length - 2, 2);
                            plotCommmand[j].Append(")");
                            m_pEngine.Evaluate(plotCommmand[j].ToString());
                        }


                        m_pEngine.Evaluate("Bhatta.diss[" + (k+1).ToString() + ", " + (l+1).ToString() + "] <- Bhatta.mdist(x1, x2, v1, v2)");
                        m_pEngine.Evaluate("Bhatta.diss[" + (l + 1).ToString() + ", " + (k + 1).ToString() + "] <- Bhatta.mdist(x2, x1, v2, v1)");
                        
                    }
                }

                pfrmProgress.lblStatus.Text = "Finding Clusters";

                m_pEngine.Evaluate("sample.hclu <- hclust(as.dist(Bhatta.diss))");
            }
            else if (cboDistance.Text== "Euclidean distance")
            {
                StringBuilder plotCommmand = new StringBuilder();
                plotCommmand.Append("Euc.dis <- dist(cbind(");

                for (int j = 0; j < intnVar; j++)
                {
                    
                    NumericVector vecVar = m_pEngine.CreateNumericVector(pBVariable[j].arrVar);
                    m_pEngine.SetSymbol(pBVariable[j].variableNames, vecVar);
                    plotCommmand.Append(pBVariable[j].variableNames + ", ");
                }

                plotCommmand.Remove(plotCommmand.Length - 2, 2);
                plotCommmand.Append("))");
                m_pEngine.Evaluate(plotCommmand.ToString());

                pfrmProgress.lblStatus.Text = "Finding Clusters";

                m_pEngine.Evaluate("sample.hclu <- hclust(Euc.dis)");

            }

            string strTitle = "Dendrogram";
            string strCommand = "plot(sample.hclu);";
            pSnippet.drawPlottoForm(strTitle, strCommand);

            int intNClasses = Convert.ToInt32(nudNClasses.Value);
            
            m_pEngine.Evaluate("sample.cut <- cutree(sample.hclu, " + intNClasses.ToString() + ")");

            //Save Outputs in SHP
            pfrmProgress.lblStatus.Text = "Save Results";
            //The field names are related with string[] DeterminedName in clsSnippet 
            string strOutputFldName = txtOutputFld.Text;

            //Get EVs and residuals
            NumericVector nvClasses = m_pEngine.Evaluate("sample.cut").AsNumeric();

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
            int intOutputFldIdx = m_pFClass.FindField(strOutputFldName);

            while (pFeature != null)
            {
                //Update Residuals
                pFeature.set_Value(intOutputFldIdx, (object)nvClasses[featureIdx]);

                pFCursor.UpdateFeature(pFeature);

                pFeature = pFCursor.NextFeature();
                featureIdx++;
            }

            //Not working properly
            ////Creating unique value map
            //IGeoFeatureLayer geoFeatureLayer = (IGeoFeatureLayer)m_pFLayer;
            //IUniqueValueRenderer uniqueValueRenderer = new UniqueValueRendererClass();
            //uniqueValueRenderer.FieldCount = 1;
            //uniqueValueRenderer.set_Field(0, strOutputFldName);


            //IDataStatistics pDataStat = new DataStatisticsClass();
            //pDataStat.Field = strOutputFldName;
            //ITable pTable = (ITable)m_pFClass;
            //ICursor pCursor = pTable.Search(null, true);
            //pDataStat.Cursor = pCursor;
            //int intUniqValueCnt = pDataStat.UniqueValueCount;

            //System.Collections.IEnumerator enumerator = pDataStat.UniqueValues;
            //enumerator.Reset();

            //ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            //simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            //IEnumColors enumColors = CreateAlgorithmicColorRamp(intUniqValueCnt).Colors;


            //for (int j = 0; j < intUniqValueCnt; j++)
            //{
            //    string value = enumerator.Current.ToString();
            //    enumerator.MoveNext();
            //    simpleFillSymbol = new SimpleFillSymbolClass();
            //    simpleFillSymbol.Color = enumColors.Next();
            //    uniqueValueRenderer.AddValue(value, strOutputFldName, simpleFillSymbol as ISymbol);
            //}

            ////IFeatureCursor featureCursor = geoFeatureLayer.FeatureClass.Search(null, false);
            ////IFeature feature;
            ////if (featureCursor != null)
            ////{
            ////    IEnumColors enumColors = CreateAlgorithmicColorRamp(8).Colors;
            ////    //int fieldIndex = geoFeatureLayer.FeatureClass.Fields.FindField("continent");
            ////    for (int j = 0; j < uniqueValueRenderer.ValueCount; j++)
            ////    {
            ////        string value = uniqueValueRenderer.get_Value(i);
            ////        uniqueValueRenderer.set_Symbol(value, simpleFillSymbol as ISymbol);

            ////        feature = featureCursor.NextFeature();
            ////        string nameValue = feature.get_Value(intOutputFldIdx).ToString();
            ////        simpleFillSymbol = new SimpleFillSymbolClass();
            ////        simpleFillSymbol.Color = enumColors.Next();
            ////        uniqueValueRenderer.AddValue(nameValue, strOutputFldName, simpleFillSymbol as ISymbol);
            ////    }
            ////}

            //geoFeatureLayer.Renderer = uniqueValueRenderer as IFeatureRenderer;

            //m_pActiveView.Refresh();
            //m_pForm.axTOCControl1.Update();

            MessageBox.Show("Clustering results are stored in the source shapefile");


            pfrmProgress.Close();
        }
        private IColorRamp CreateAlgorithmicColorRamp(int count)
        {
            IAlgorithmicColorRamp algColorRamp = new AlgorithmicColorRampClass();
            IRgbColor fromColor = new RgbColorClass();
            IRgbColor toColor = new RgbColorClass();
            fromColor.Red = 255;
            fromColor.Green = 0;
            fromColor.Blue = 0;

            toColor.Red = 0;
            toColor.Green = 0;
            toColor.Blue = 255;

            algColorRamp.ToColor = fromColor;
            algColorRamp.FromColor = toColor;

            algColorRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm;
            algColorRamp.Size = count;
            bool bture = true;
            algColorRamp.CreateRamp(out bture);
            return algColorRamp;
        }
    }
}
