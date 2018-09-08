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
using RDotNet;
using ESRI.ArcGIS.Geometry;

namespace VisUncertainty
{
    public partial class frmGLM : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private IFeatureLayer m_pFLayer;
        private IFeatureClass m_pFClass;
        private REngine m_pEngine;

        //Varaibles for SWM
        private bool m_blnCreateSWM = false;

        public frmGLM()
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
                if (cboTargetLayer.Text != "" && cboFamily.Text != "")
                {

                    string strLayerName = cboTargetLayer.Text;

                    int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                    ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                    m_pFLayer = pLayer as IFeatureLayer;
                    m_pFClass = m_pFLayer.FeatureClass;

                    //New Spatial Weight matrix function 080317
                    clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                    if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                        txtSWM.Text = pSWMType.strPolySWM;
                    else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        txtSWM.Text = pSWMType.strPointSWM;
                    else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                        MessageBox.Show("Spatial weight matrix for polyline is not supported.");


                    IFields fields = m_pFClass.Fields;

                    cboFieldName.Items.Clear();
                    cboFieldName.Text = "";
                    lstFields.Items.Clear();
                    lstIndeVar.Items.Clear();
                    cboNormalization.Text = "";
                    cboNormalization.Items.Clear();

                    if (cboFamily.Text == "Poisson")
                    {
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                            {
                                lstFields.Items.Add(fields.get_Field(i).Name);
                                cboNormalization.Items.Add(fields.get_Field(i).Name);
                                if (fields.get_Field(i).Type == esriFieldType.esriFieldTypeInteger || fields.get_Field(i).Type == esriFieldType.esriFieldTypeSmallInteger)
                                    cboFieldName.Items.Add(fields.get_Field(i).Name);
                            }

                        }
                    }
                    else
                    {
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                            {
                                cboFieldName.Items.Add(fields.get_Field(i).Name);
                                lstFields.Items.Add(fields.get_Field(i).Name);
                                cboNormalization.Items.Add(fields.get_Field(i).Name);
                            }
                        }
                    }
                    //Add intercept in the listview for independent variables
                    lstIndeVar.Items.Add("Intercept");
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            m_pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstFields, lstIndeVar);
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            m_pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstIndeVar, lstFields);
        }

        private void cboFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboTargetLayer.Text != "" && cboFamily.Text != "")
                {
                    if (cboFamily.Text == "Binomial")
                    {
                        lblNorm.Text = "Normalization:";
                        lblNorm.Enabled = true;
                        cboNormalization.Enabled = true;
                    }

                    else if (cboFamily.Text == "Poisson")
                    {
                        lblNorm.Text = "Offset (Optional):";
                        lblNorm.Enabled = true;
                        cboNormalization.Enabled = true;
                    }
                    else if (cboFamily.Text == "Logistic")
                    {
                        cboNormalization.Enabled = false;
                        lblNorm.Enabled = false;
                    }

                    string strLayerName = cboTargetLayer.Text;

                    int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                    ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                    m_pFLayer = pLayer as IFeatureLayer;
                    m_pFClass = m_pFLayer.FeatureClass;

                    IFields fields = m_pFClass.Fields;

                    cboFieldName.Items.Clear();
                    cboFieldName.Text = "";
                    lstFields.Items.Clear();
                    lstIndeVar.Items.Clear();
                    cboNormalization.Text = "";

                    if (cboFamily.Text == "Poisson"||cboFamily.Text == "Logistic")
                    {
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                            {
                                lstFields.Items.Add(fields.get_Field(i).Name);
                                cboNormalization.Items.Add(fields.get_Field(i).Name);
                                if (fields.get_Field(i).Type == esriFieldType.esriFieldTypeInteger || fields.get_Field(i).Type == esriFieldType.esriFieldTypeSmallInteger)
                                    cboFieldName.Items.Add(fields.get_Field(i).Name);
                            }

                        }
                    }
                    else
                    {
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                            {
                                cboFieldName.Items.Add(fields.get_Field(i).Name);
                                lstFields.Items.Add(fields.get_Field(i).Name);
                                cboNormalization.Items.Add(fields.get_Field(i).Name);
                            }
                        }
                    }
                    //Add intercept in the listview for independent variables
                    lstIndeVar.Items.Add("Intercept");

                }
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

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboFieldName.Text == "")
                {
                    MessageBox.Show("Please select the dependent input variables to be used in the regression model.",
                        "Please choose at least one input variable");
                    return;
                }
                if (lstIndeVar.Items.Count == 0)
                {
                    MessageBox.Show("Please select independents input variables to be used in the regression model.",
                        "Please choose at least one input variable");
                    return;
                }
                //if (cboFamily.Text == "Binomial" && cboNormalization.Text == "")
                //{
                //    MessageBox.Show("Please select a variable for normailization");
                //    return;
                //}

                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Pre-Processing:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();


                //Decimal places
                int intDeciPlaces = 5;

                //Get number of Independent variables 
                int nIndevarlistCnt = lstIndeVar.Items.Count;
                //Indicate an intercept only model (2) or a non-intercept model (1) or not (0)
                int intInterceptModel = 1;
                for (int j = 0; j < nIndevarlistCnt; j++)
                {
                    if ((string)lstIndeVar.Items[j] == "Intercept")
                        intInterceptModel = 0;
                }
                if (nIndevarlistCnt == 1 && intInterceptModel == 0)
                    intInterceptModel = 2;

                int nIDepen = 0;
                if (intInterceptModel == 0)
                    nIDepen = nIndevarlistCnt - 1;
                else if (intInterceptModel == 1)
                    nIDepen = nIndevarlistCnt;

                // Gets the column of the dependent variable
                String dependentName = (string)cboFieldName.SelectedItem;
                string strNoramlName = cboNormalization.Text;
                //sourceTable.AcceptChanges();
                //DataTable dependent = sourceTable.DefaultView.ToTable(false, dependentName);

                // Gets the columns of the independent variables
                String[] independentNames = new string[nIDepen];
                int intIdices = 0;
                string strIndependentName = "";
                for (int j = 0; j < nIndevarlistCnt; j++)
                {
                    strIndependentName = (string)lstIndeVar.Items[j];
                    if (strIndependentName != "Intercept")
                    {
                        independentNames[intIdices] = strIndependentName;
                        intIdices++;
                    }
                }

                int nFeature = m_pFClass.FeatureCount(null);

                IFeatureCursor pFCursor = m_pFLayer.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Get index for independent and dependent variables
                int intDepenIdx = m_pFClass.Fields.FindField(dependentName);
                int intNoramIdx = -1;
                if (strNoramlName != "")
                    intNoramIdx = m_pFClass.Fields.FindField(strNoramlName);

                int[] idxes = new int[nIDepen];

                for (int j = 0; j < nIDepen; j++)
                {
                    idxes[j] = m_pFLayer.FeatureClass.Fields.FindField(independentNames[j]);
                }


                //Store independent values at Array
                double[] arrDepen = new double[nFeature];
                double[][] arrInDepen = new double[nIDepen][];
                double[] arrNormal = new double[nFeature];

                for (int j = 0; j < nIDepen; j++)
                {
                    arrInDepen[j] = new double[nFeature];
                }

                int i = 0;
                while (pFeature != null)
                {

                    arrDepen[i] = Convert.ToDouble(pFeature.get_Value(intDepenIdx));

                    if (intNoramIdx != -1)
                        arrNormal[i] = Convert.ToDouble(pFeature.get_Value(intNoramIdx));

                    for (int j = 0; j < nIDepen; j++)
                    {
                        arrInDepen[j][i] = Convert.ToDouble(pFeature.get_Value(idxes[j]));
                    }

                    i++;
                    pFeature = pFCursor.NextFeature();
                }

                if (cboFamily.Text == "Binomial" && intNoramIdx == -1)
                {
                    double dblMaxDepen = arrDepen.Max();
                    double dblMinDepen = arrDepen.Min();
                    if (dblMinDepen < 0 || dblMaxDepen > 1)
                    {
                        MessageBox.Show("The value range of a dependent variable must be between 0 and 1");

                        pfrmProgress.Close();
                        return;
                    }
                }

                //Plot command for R
                StringBuilder plotCommmand = new StringBuilder();

                pfrmProgress.lblStatus.Text = "Collecting Data:";

                if (!m_blnCreateSWM)
                {
                    //Get the file path and name to create spatial weight matrix
                    string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

                    if (strNameR == null)
                        return;

                    //Create spatial weight matrix in R
                    if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        m_pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
                    else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        m_pEngine.Evaluate("sample.shp <- readShapePoints('" + strNameR + "')");
                    else
                    {
                        MessageBox.Show("This geometry type is not supported");
                        pfrmProgress.Close();
                        this.Close();
                    }


                    int intSuccess = m_pSnippet.CreateSpatialWeightMatrix(m_pEngine, m_pFClass, txtSWM.Text, pfrmProgress);
                    if (intSuccess == 0)
                        return;
                }

                //Dependent variable to R vector
                NumericVector vecDepen = m_pEngine.CreateNumericVector(arrDepen);
                m_pEngine.SetSymbol(dependentName, vecDepen);
                NumericVector vecNormal = null;
                if (cboFamily.Text == "Binomial" && intNoramIdx != -1)
                {
                    vecNormal = m_pEngine.CreateNumericVector(arrNormal);
                    m_pEngine.SetSymbol(strNoramlName, vecNormal);
                    plotCommmand.Append("cbind(" + dependentName + ", " + strNoramlName + "-" + dependentName + ")~");
                }
                else if (cboFamily.Text == "Poisson" && intNoramIdx != -1)
                {
                    vecNormal = m_pEngine.CreateNumericVector(arrNormal);
                    m_pEngine.SetSymbol(strNoramlName, vecNormal);
                    plotCommmand.Append(dependentName + "~");
                }
                else
                    plotCommmand.Append(dependentName + "~");

                if(intInterceptModel == 2)
                {
                    plotCommmand.Append("1");
                }
                else
                {
                    for (int j = 0; j < nIDepen; j++)
                    {
                        NumericVector vecIndepen = m_pEngine.CreateNumericVector(arrInDepen[j]);
                        m_pEngine.SetSymbol(independentNames[j], vecIndepen);
                        plotCommmand.Append(independentNames[j] + "+");
                    }
                    plotCommmand.Remove(plotCommmand.Length - 1, 1);

                    if (intInterceptModel == 1)
                        plotCommmand.Append("-1");
                }


                if (cboFamily.Text == "Poisson")
                {
                    PoissonRegression(pfrmProgress, m_pFLayer, plotCommmand.ToString(), nIDepen, independentNames, strNoramlName, intDeciPlaces, intInterceptModel);
                }
                else if (cboFamily.Text == "Binomial" || cboFamily.Text == "Logistic")
                    BinomRegression(pfrmProgress, m_pFLayer, plotCommmand.ToString(), nIDepen, independentNames, intDeciPlaces, intInterceptModel);

                pfrmProgress.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        
        private void PoissonRegression(frmProgress pfrmProgress, IFeatureLayer pFLayer, string strLM, int nIDepen, String[] independentNames, string strNoramlName, int intDeciPlaces, int intInterceptModel)
        {
            pfrmProgress.lblStatus.Text = "Calculate Regression Coefficients";

            if (strNoramlName == "")
                m_pEngine.Evaluate("sample.glm <- glm(" + strLM + ", family='poisson')");
            else
                m_pEngine.Evaluate("sample.glm <- glm(" + strLM + ", offset=" + strNoramlName + ", family='poisson')");

            pfrmProgress.lblStatus.Text = "Printing Output:";
            m_pEngine.Evaluate("sum.glm <- summary(sample.glm)");
            m_pEngine.Evaluate("sample.n <- length(sample.nb)");

            NumericMatrix matCoe = m_pEngine.Evaluate("as.matrix(sum.glm$coefficient)").AsNumericMatrix();
            CharacterVector vecNames = m_pEngine.Evaluate("attributes(sum.glm$coefficients)$dimnames[[1]]").AsCharacter();
            double dblNullDevi = m_pEngine.Evaluate("sum.glm$null.deviance").AsNumeric().First();
            double dblNullDF = m_pEngine.Evaluate("sum.glm$df.null").AsNumeric().First();
            double dblResiDevi = m_pEngine.Evaluate("sum.glm$deviance").AsNumeric().First();
            double dblResiDF = m_pEngine.Evaluate("sum.glm$df.residual").AsNumeric().First();

            //Nagelkerke r squared
            double dblPseudoRsquared = m_pEngine.Evaluate("(1 - exp((sample.glm$dev - sample.glm$null)/sample.n))/(1 - exp(-sample.glm$null/sample.n))").AsNumeric().First();

            double dblResiLMMC = 0;
            double dblResiLMpVal = 0;
            if (chkResiAuto.Checked)
            {
                m_pEngine.Evaluate("orgresi.mc <-moran.mc(residuals(sample.glm, type='" + cboResiType.Text + "'), listw =sample.listb, nsim=999, zero.policy=TRUE)");
                dblResiLMMC = m_pEngine.Evaluate("orgresi.mc$statistic").AsNumeric().First();
                dblResiLMpVal = m_pEngine.Evaluate("orgresi.mc$p.value").AsNumeric().First();
            }

            NumericVector nvecNonAIC = m_pEngine.Evaluate("sample.glm$aic").AsNumeric();
            //Open Ouput form
            frmRegResult pfrmRegResult = new frmRegResult();

            if (strNoramlName == "")
                pfrmRegResult.Text = "Poisson Regression Summary";
            else
                pfrmRegResult.Text = "Poisson Regression with Offset (" + strNoramlName + ") Summary";

            //Create DataTable to store Result
            System.Data.DataTable tblRegResult = new DataTable("PoiResult");

            //Assign DataTable
            DataColumn dColName = new DataColumn();
            dColName.DataType = System.Type.GetType("System.String");
            dColName.ColumnName = "Name";
            tblRegResult.Columns.Add(dColName);

            DataColumn dColValue = new DataColumn();
            dColValue.DataType = System.Type.GetType("System.Double");
            dColValue.ColumnName = "Estimate";
            tblRegResult.Columns.Add(dColValue);

            DataColumn dColSE = new DataColumn();
            dColSE.DataType = System.Type.GetType("System.Double");
            dColSE.ColumnName = "Std. Error";
            tblRegResult.Columns.Add(dColSE);

            DataColumn dColTValue = new DataColumn();
            dColTValue.DataType = System.Type.GetType("System.Double");
            dColTValue.ColumnName = "z value";
            tblRegResult.Columns.Add(dColTValue);

            DataColumn dColPvT = new DataColumn();
            dColPvT.DataType = System.Type.GetType("System.Double");
            dColPvT.ColumnName = "Pr(>|z|)";
            tblRegResult.Columns.Add(dColPvT);

            int intNCoeff = matCoe.RowCount;

            //Store Data Table by R result
            for (int j = 0; j < intNCoeff; j++)
            {
                DataRow pDataRow = tblRegResult.NewRow();
                if (j == 0 && intInterceptModel != 1)
                {
                    pDataRow["Name"] = "(Intercept)";
                }
                else if (intInterceptModel == 1)
                    pDataRow["Name"] = independentNames[j];
                else
                {
                    pDataRow["Name"] = independentNames[j - 1];
                }
                pDataRow["Estimate"] = Math.Round(matCoe[j, 0], intDeciPlaces);
                pDataRow["Std. Error"] = Math.Round(matCoe[j, 1], intDeciPlaces);
                pDataRow["z value"] = Math.Round(matCoe[j, 2], intDeciPlaces);
                pDataRow["Pr(>|z|)"] = Math.Round(matCoe[j, 3], intDeciPlaces);
                tblRegResult.Rows.Add(pDataRow);
            }

            //Assign Datagridview to Data Table
            pfrmRegResult.dgvResults.DataSource = tblRegResult;

            int nFeature = pFLayer.FeatureClass.FeatureCount(null);
            //Assign values at Textbox
            string strDecimalPlaces = "N" + intDeciPlaces.ToString();
            string[] strResults = new string[6];
            strResults[0] = "Number of rows: " + nFeature.ToString();
            strResults[1] = "AIC: " + nvecNonAIC.Last().ToString(strDecimalPlaces);
            strResults[2] = "Null deviance: " + dblNullDevi.ToString(strDecimalPlaces) + " on " + dblNullDF.ToString("N0") + " degrees of freedom";
            strResults[3] = "Residual deviance: " + dblResiDevi.ToString(strDecimalPlaces) + " on " + dblResiDF.ToString("N0") + " degrees of freedom";
            if (intInterceptModel != 1)
                strResults[4] = "Nagelkerke pseudo R squared: " + dblPseudoRsquared.ToString(strDecimalPlaces);
            else
                strResults[4] = "";
            if (chkResiAuto.Checked)
                strResults[5] = "MC of residuals: " + dblResiLMMC.ToString("N3") + ", p-value: " + dblResiLMpVal.ToString("N3");
            else
                strResults[5] = "";

            pfrmRegResult.txtOutput.Lines = strResults;

            if (chkSave.Checked)
            {
                pfrmProgress.lblStatus.Text = "Saving residuals:";
                //The field names are related with string[] DeterminedName in clsSnippet 
                string strResiFldName = lstSave.Items[0].SubItems[1].Text;

                //Get EVs and residuals
                NumericVector nvResiduals = m_pEngine.Evaluate("as.numeric(residuals(sample.glm, type='" + cboResiType.Text + "'))").AsNumeric();

                // Create field, if there isn't
                if (m_pFClass.FindField(strResiFldName) == -1)
                {
                    //Add fields
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strResiFldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    m_pFClass.AddField(newField);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to overwrite " + strResiFldName + " field?", "Overwrite", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }


                //Update Field
                IFeatureCursor pFCursor = m_pFClass.Update(null, false);
                IFeature pFeature = pFCursor.NextFeature();

                int featureIdx = 0;
                int intResiFldIdx = m_pFClass.FindField(strResiFldName);

                while (pFeature != null)
                {
                    //Update Residuals
                    pFeature.set_Value(intResiFldIdx, (object)nvResiduals[featureIdx]);

                    pFCursor.UpdateFeature(pFeature);

                    pFeature = pFCursor.NextFeature();
                    featureIdx++;
                }

            }
            pfrmRegResult.Show();
        }
        private void BinomRegression(frmProgress pfrmProgress, IFeatureLayer pFLayer, string strLM, int nIDepen, String[] independentNames, int intDeciPlaces, int intInterceptModel)
        {
            pfrmProgress.lblStatus.Text = "Calculate Regression Coefficients";
            m_pEngine.Evaluate("sample.glm <- glm(" + strLM + ", family='binomial')");

            pfrmProgress.lblStatus.Text = "Printing Output:";
            m_pEngine.Evaluate("sum.glm <- summary(sample.glm)");
            m_pEngine.Evaluate("sample.n <- length(sample.nb)");

            NumericMatrix matCoe = m_pEngine.Evaluate("as.matrix(sum.glm$coefficient)").AsNumericMatrix();
            CharacterVector vecNames = m_pEngine.Evaluate("attributes(sum.glm$coefficients)$dimnames[[1]]").AsCharacter();
            double dblNullDevi = m_pEngine.Evaluate("sum.glm$null.deviance").AsNumeric().First();
            double dblNullDF = m_pEngine.Evaluate("sum.glm$df.null").AsNumeric().First();
            double dblResiDevi = m_pEngine.Evaluate("sum.glm$deviance").AsNumeric().First();
            double dblResiDF = m_pEngine.Evaluate("sum.glm$df.residual").AsNumeric().First();

            //Nagelkerke r squared
            double dblPseudoRsquared = m_pEngine.Evaluate("(1 - exp((sample.glm$dev - sample.glm$null)/sample.n))/(1 - exp(-sample.glm$null/sample.n))").AsNumeric().First();

            double dblResiLMMC = 0;
            double dblResiLMpVal = 0;
            if (chkResiAuto.Checked)
            {
                m_pEngine.Evaluate("orgresi.mc <-moran.mc(residuals(sample.glm, type='" + cboResiType.Text + "'), listw =sample.listb, nsim=999, zero.policy=TRUE)");
                dblResiLMMC = m_pEngine.Evaluate("orgresi.mc$statistic").AsNumeric().First();
                dblResiLMpVal = m_pEngine.Evaluate("orgresi.mc$p.value").AsNumeric().First();
            }
            
            NumericVector nvecNonAIC = m_pEngine.Evaluate("sample.glm$aic").AsNumeric();

            //Open Ouput form
            frmRegResult pfrmRegResult = new frmRegResult();
            pfrmRegResult.Text = "Binomial Regression Summary";

            //Create DataTable to store Result
            System.Data.DataTable tblRegResult = new DataTable("BinoResult");

            //Assign DataTable
            DataColumn dColName = new DataColumn();
            dColName.DataType = System.Type.GetType("System.String");
            dColName.ColumnName = "Name";
            tblRegResult.Columns.Add(dColName);

            DataColumn dColValue = new DataColumn();
            dColValue.DataType = System.Type.GetType("System.Double");
            dColValue.ColumnName = "Estimate";
            tblRegResult.Columns.Add(dColValue);

            DataColumn dColSE = new DataColumn();
            dColSE.DataType = System.Type.GetType("System.Double");
            dColSE.ColumnName = "Std. Error";
            tblRegResult.Columns.Add(dColSE);

            DataColumn dColTValue = new DataColumn();
            dColTValue.DataType = System.Type.GetType("System.Double");
            dColTValue.ColumnName = "z value";
            tblRegResult.Columns.Add(dColTValue);

            DataColumn dColPvT = new DataColumn();
            dColPvT.DataType = System.Type.GetType("System.Double");
            dColPvT.ColumnName = "Pr(>|z|)";
            tblRegResult.Columns.Add(dColPvT);

            int intNCoeff = matCoe.RowCount;

            //Store Data Table by R result
            for (int j = 0; j < intNCoeff; j++)
            {
                DataRow pDataRow = tblRegResult.NewRow();
                if (j == 0 && intInterceptModel != 1)
                {
                    pDataRow["Name"] = "(Intercept)";
                }
                else if (intInterceptModel == 1)
                    pDataRow["Name"] = independentNames[j];
                else
                {
                    pDataRow["Name"] = independentNames[j - 1];
                }
                pDataRow["Estimate"] = Math.Round(matCoe[j, 0], intDeciPlaces);
                pDataRow["Std. Error"] = Math.Round(matCoe[j, 1], intDeciPlaces);
                pDataRow["z value"] = Math.Round(matCoe[j, 2], intDeciPlaces);
                pDataRow["Pr(>|z|)"] = Math.Round(matCoe[j, 3], intDeciPlaces);
                tblRegResult.Rows.Add(pDataRow);
            }

            //Assign Datagridview to Data Table
            pfrmRegResult.dgvResults.DataSource = tblRegResult;

            int nFeature = pFLayer.FeatureClass.FeatureCount(null);
            //Assign values at Textbox
            string strDecimalPlaces = "N" + intDeciPlaces.ToString();
            string[] strResults = new string[6];
            strResults[0] = "Number of rows: " + nFeature.ToString();
            strResults[1] = "AIC: " + nvecNonAIC.Last().ToString(strDecimalPlaces);
            strResults[2] = "Null deviance: " + dblNullDevi.ToString(strDecimalPlaces) + " on " + dblNullDF.ToString("N0") + " degrees of freedom";
            strResults[3] = "Residual deviance: " + dblResiDevi.ToString(strDecimalPlaces) + " on " + dblResiDF.ToString("N0") + " degrees of freedom";
            if (intInterceptModel != 1)
                strResults[4] = "Nagelkerke pseudo R squared: " + dblPseudoRsquared.ToString(strDecimalPlaces);
            else
                strResults[4] = "";
            if (chkResiAuto.Checked)
                strResults[5] = "MC of residuals: " + dblResiLMMC.ToString("N3") + ", p-value: " + dblResiLMpVal.ToString("N3");
            else
                strResults[5] = "";

            pfrmRegResult.txtOutput.Lines = strResults;

            //Save Outputs in SHP
            if (chkSave.Checked)
            {
                pfrmProgress.lblStatus.Text = "Saving residuals:";
                //The field names are related with string[] DeterminedName in clsSnippet 
                string strResiFldName = lstSave.Items[0].SubItems[1].Text;

                //Get EVs and residuals
                NumericVector nvResiduals = m_pEngine.Evaluate("as.numeric(residuals(sample.glm, type='" + cboResiType.Text + "'))").AsNumeric();

                // Create field, if there isn't
                if (m_pFClass.FindField(strResiFldName) == -1)
                {
                    //Add fields
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strResiFldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    m_pFClass.AddField(newField);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to overwrite " + strResiFldName + " field?", "Overwrite", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }


                //Update Field
                IFeatureCursor pFCursor = m_pFClass.Update(null, false);
                IFeature pFeature = pFCursor.NextFeature();

                int featureIdx = 0;
                int intResiFldIdx = m_pFClass.FindField(strResiFldName);

                while (pFeature != null)
                {
                    //Update Residuals
                    pFeature.set_Value(intResiFldIdx, (object)nvResiduals[featureIdx]);

                    pFCursor.UpdateFeature(pFeature);

                    pFeature = pFCursor.NextFeature();
                    featureIdx++;
                }

            }

            pfrmRegResult.Show();
        }
        #region update listview for residuals

        private void chkSave_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSave.Checked)
            {
                if (m_pFClass != null)
                {
                    UpdateListview(lstSave, m_pFClass);
                    lstSave.Enabled = true;
                    lblType.Enabled = true;
                    cboResiType.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Select a target layer first");

                    chkSave.Checked = false;
                    return;
                }
            }
            else
            {
                lstSave.Enabled = false;
                lblType.Enabled = false;
                cboResiType.Enabled = false;
            }
                
        }
        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass)
        {
            try
            {
                pListView.BeginUpdate();
                string strResiFldNm = "glm_resi";

                //Update Name Using the UpdateFldName Function

                strResiFldNm = UpdateFldName(strResiFldNm, pFeatureClass);
                if (strResiFldNm == null)
                    return;

                pListView.Items[0].SubItems[1].Text = strResiFldNm;
                pListView.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private string UpdateFldName(string strFldNM, IFeatureClass pFeatureClass)
        {
            try
            {
                string returnNM = strFldNM;
                int i = 1;
                while (pFeatureClass.FindField(returnNM) != -1)
                {
                    returnNM = strFldNM + "_" + i.ToString();
                    i++;
                }
                return returnNM;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
            }
        }
        private DialogResult PopupInput(ListViewItem.ListViewSubItem pSelectedSubItems, int border, int length, ref string output)
        {

            System.Drawing.Point ctrlpt = pSelectedSubItems.Bounds.Location;
            ctrlpt = this.PointToScreen(pSelectedSubItems.Bounds.Location);
            //ctrlpt.Y += grbSave.Location.Y + 10;
            //ctrlpt.X += grbSave.Location.X + (length / 2);
            ctrlpt.Y += 391;
            ctrlpt.X += 28 + (length / 2);
            TextBox input = new TextBox { Height = 20, Width = length, Top = border / 2, Left = border / 2 };
            input.BorderStyle = BorderStyle.FixedSingle;
            input.Text = output;

            //######## SetColor to your preference
            input.BackColor = Color.Azure;

            Button btnok = new Button { DialogResult = System.Windows.Forms.DialogResult.OK, Top = 25 };
            Button btncn = new Button { DialogResult = System.Windows.Forms.DialogResult.Cancel, Top = 25 };

            Form frm = new Form { ControlBox = false, AcceptButton = btnok, CancelButton = btncn, StartPosition = FormStartPosition.Manual, Location = ctrlpt };
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //######## SetColor to your preference
            frm.BackColor = Color.Black;

            RectangleF rec = new RectangleF(0, 0, (length + border), (20 + border));
            System.Drawing.Drawing2D.GraphicsPath GP = new System.Drawing.Drawing2D.GraphicsPath(); //GetRoundedRect(rec, 4.0F);
            float diameter = 8.0F;
            SizeF sizef = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(rec.Location, sizef);
            GP.AddArc(arc, 180, 90);
            arc.X = rec.Right - diameter;
            GP.AddArc(arc, 270, 90);
            arc.Y = rec.Bottom - diameter;
            GP.AddArc(arc, 0, 90);
            arc.X = rec.Left;
            GP.AddArc(arc, 90, 90);
            GP.CloseFigure();

            frm.Region = new Region(GP);
            frm.Controls.AddRange(new Control[] { input, btncn, btnok });
            DialogResult rst = frm.ShowDialog();
            output = input.Text;
            return rst;
        }
        private void lstSave_MouseUp(object sender, MouseEventArgs e)
        {
            ListViewItem.ListViewSubItem pSelectedSubItems = null;
            //selection = lvSymbol.GetItemAt(e.X, e.Y);
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (lstSave.Items[i].SubItems[j].Bounds.Contains(e.Location))
                    {
                        pSelectedSubItems = lstSave.Items[i].SubItems[j];
                        if (j == 1)
                        {
                            string var = pSelectedSubItems.Text;

                            int intLength = var.Length * 6 + 30;

                            if (PopupInput(pSelectedSubItems, 2, intLength, ref var) == System.Windows.Forms.DialogResult.OK)
                            {
                                lstSave.Items[i].SubItems[j].Text = var;
                            }
                        }

                    }
                }
            }

            lstSave.Update();
        }

        #endregion

        private void btnOpenSWM_Click(object sender, EventArgs e)
        {
            if (m_pFClass == null)
            {
                MessageBox.Show("Please select a target layer");
                return;
            }
            frmAdvSWM pfrmAdvSWM = new frmAdvSWM();
            pfrmAdvSWM.m_pFClass = m_pFClass;
            pfrmAdvSWM.blnCorrelogram = false;
            pfrmAdvSWM.ShowDialog();
            m_blnCreateSWM = pfrmAdvSWM.blnSWMCreation;
            txtSWM.Text = pfrmAdvSWM.txtSWM.Text;
        }

        private void chkResiAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkResiAuto.Checked)
            {
                lblSWM.Enabled = true;
                txtSWM.Enabled = true;
                btnOpenSWM.Enabled = true;
            }
            else
            {
                lblSWM.Enabled = false;
                txtSWM.Enabled = false;
                btnOpenSWM.Enabled = false;
            }
        }

        private void lstFields_DoubleClick(object sender, EventArgs e)
        {
            m_pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstFields, lstIndeVar);
        }

        private void lstIndeVar_DoubleClick(object sender, EventArgs e)
        {
            m_pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstIndeVar, lstFields);
        }

        //private void chkIntercept_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkIntercept.Checked == false)
        //    {
        //        lstFields.Enabled = true;
        //        lstIndeVar.Enabled = true;
        //        btnMoveLeft.Enabled = true;
        //        btnMoveRight.Enabled = true;
        //    }
        //    else
        //    {
        //        lstFields.Enabled = false;
        //        lstIndeVar.Enabled = false;
        //        btnMoveLeft.Enabled = false;
        //        btnMoveRight.Enabled = false;
        //    }
        //}
    }
}
