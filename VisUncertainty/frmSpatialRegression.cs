using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

using RDotNet;
using System.Drawing;
using ESRI.ArcGIS.Geometry;

//car, spdep, and maptools packages in R are required

namespace VisUncertainty
{
    public partial class frmSpatialRegression : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        //Varaibles for SWM
        private bool m_blnCreateSWM = false;

        public frmSpatialRegression()
        {
            try
            {
                InitializeComponent();

                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
                m_pEngine = m_pForm.pEngine;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }
                m_pSnippet = new clsSnippet();

                m_pEngine.Evaluate("library(car); library(spdep);library(maptools)");
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
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                    txtSWM.Text = pSWMType.strPolySWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    txtSWM.Text = pSWMType.strPointSWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    MessageBox.Show("Spatial weight matrix for polyline is not supported.");


                IFields fields = m_pFClass.Fields;

                cboFieldName.Items.Clear();
                lstFields.Items.Clear();
                lstIndeVar.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                        lstFields.Items.Add(fields.get_Field(i).Name);
                    }
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

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            m_pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstFields, lstIndeVar);

        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            m_pSnippet.MoveSelectedItemsinListBoxtoOtherListBox(lstIndeVar, lstFields);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Processing:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();

                if (cboFieldName.Text == "")
                {
                    MessageBox.Show("Please select the dependent input variables to be used in the regression model.",
                        "Please choose at least one input variable");
                }
                if (lstIndeVar.Items.Count == 0)
                {
                    MessageBox.Show("Please select independents input variables to be used in the regression model.",
                        "Please choose at least one input variable");
                }
                //Decimal places
                int intDeciPlaces = 5;

                //Get number of Independent variables            
                int nIDepen = lstIndeVar.Items.Count;
                // Gets the column of the dependent variable
                String dependentName = (string)cboFieldName.SelectedItem;
                //sourceTable.AcceptChanges();
                //DataTable dependent = sourceTable.DefaultView.ToTable(false, dependentName);

                // Gets the columns of the independent variables
                String[] independentNames = new string[nIDepen];
                for (int j = 0; j < nIDepen; j++)
                {
                    independentNames[j] = (string)lstIndeVar.Items[j];
                }

                // Creates the input and output matrices from the shapefile//
                //string strLayerName = cboTargetLayer.Text;

                //int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                //ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                //ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;
                int nFeature = m_pFClass.FeatureCount(null);

                //Warning for method
                if (nFeature > m_pForm.intWarningCount)
                {
                    DialogResult dialogResult = MessageBox.Show("It might take a lot of time. Do you want to continue?", "Warning", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.No)
                    {
                        pfrmProgress.Close();
                        return;
                    }
                }


                IFeatureCursor pFCursor = m_pFLayer.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Get index for independent and dependent variables
                int intDepenIdx = m_pFLayer.FeatureClass.Fields.FindField(dependentName);
                int[] idxes = new int[nIDepen];

                for (int j = 0; j < nIDepen; j++)
                {
                    idxes[j] = m_pFLayer.FeatureClass.Fields.FindField(independentNames[j]);
                }


                //Store independent values at Array
                double[] arrDepen = new double[nFeature];
                double[][] arrInDepen = new double[nIDepen][]; //Zigzaged Array needs to be define 

                for (int j = 0; j < nIDepen; j++)
                {
                    arrInDepen[j] = new double[nFeature];
                }

                int i = 0;
                while (pFeature != null)
                {

                    arrDepen[i] = Convert.ToDouble(pFeature.get_Value(intDepenIdx));

                    for (int j = 0; j < nIDepen; j++)
                    {
                        //arrInDepen[j] = new double[nFeature];
                        arrInDepen[j][i] = Convert.ToDouble(pFeature.get_Value(idxes[j]));
                        //arrInDepen[i, j] = Convert.ToDouble(pFeature.get_Value(idxes[j]));
                    }

                    i++;
                    pFeature = pFCursor.NextFeature();
                }
                //Plot command for R
                StringBuilder plotCommmand = new StringBuilder();

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
                if (rbtError.Checked)
                    plotCommmand.Append("errorsarlm(" + dependentName + "~");
                else if(rbtLag.Checked||rbtDurbin.Checked)
                    plotCommmand.Append("lagsarlm(" + dependentName + "~");
                else
                    plotCommmand.Append("spautolm(" + dependentName + "~");


                for (int j = 0; j < nIDepen; j++)
                {
                    //double[] arrVector = arrInDepen.GetColumn<double>(j);
                    NumericVector vecIndepen = m_pEngine.CreateNumericVector(arrInDepen[j]);
                    m_pEngine.SetSymbol(independentNames[j], vecIndepen);
                    plotCommmand.Append(independentNames[j] + "+");
                }
                plotCommmand.Remove(plotCommmand.Length - 1, 1);

                //Select Method
                if (rbtEigen.Checked)
                    plotCommmand.Append(", method='eigen'");
                else if (rbtMatrix.Checked)
                    plotCommmand.Append(", method='Matrix'");
                else if (rbtMatrixJ.Checked)
                    plotCommmand.Append(", method='Matrix_J'");
                else if (rbtLU.Checked)
                    plotCommmand.Append(", method='LU'");
                else if (rbtChebyshev.Checked)
                    plotCommmand.Append(", method='Chebyshev'");
                else if (rbtMC.Checked)
                    plotCommmand.Append(", method='MC'");
                else
                    plotCommmand.Append(", method='eigen'");

                if (rbtError.Checked)
                    plotCommmand.Append(", listw=sample.listw,  tol.solve=1.0e-20, zero.policy=TRUE)");
                else if (rbtLag.Checked)
                    plotCommmand.Append(", listw=sample.listw,  tol.solve=1.0e-20, zero.policy=TRUE)");
                else if (rbtCAR.Checked)
                    plotCommmand.Append(", listw=sample.listw, family='CAR', verbose=TRUE, zero.policy=TRUE)");
                else if (rbtSMA.Checked)
                    plotCommmand.Append(", listw=sample.listw, family='SMA', verbose=TRUE, zero.policy=TRUE)");
                else if (rbtDurbin.Checked)
                    plotCommmand.Append(", type='mixed', listw=sample.listw,  tol.solve=1.0e-20, zero.policy=TRUE)");
                else
                    return;

                try
                {
                    m_pEngine.Evaluate("sum.lm <- summary(" + plotCommmand.ToString() + ", Nagelkerke=T)");
                }
                catch
                {
                    MessageBox.Show("Cannot solve the regression. Try again with different variables.");
                    pfrmProgress.Close();
                    return;
                }

                //Collect results from R
                NumericMatrix matCoe = m_pEngine.Evaluate("as.matrix(sum.lm$Coef)").AsNumericMatrix();

                double dblLRLambda = m_pEngine.Evaluate("as.numeric(sum.lm$LR1$statistic)").AsNumeric().First();
                double dblpLambda = m_pEngine.Evaluate("as.numeric(sum.lm$LR1$p.value)").AsNumeric().First();

                double dblLRErrorModel = m_pEngine.Evaluate("as.numeric(sum.lm$LR1$estimate)").AsNumeric().First();

                double dblSigmasquared = 0;
                double dblAIC = 0;
                double dblWald = 0;
                double dblpWald = 0;

                if (rbtLag.Checked || rbtError.Checked||rbtDurbin.Checked)
                {
                    dblSigmasquared = m_pEngine.Evaluate("as.numeric(sum.lm$s2)").AsNumeric().First();
                    //dblAIC = pEngine.Evaluate("as.numeric(sum.lm$AIC_lm.model)").AsNumeric().First();
                    dblWald = m_pEngine.Evaluate("as.numeric(sum.lm$Wald1$statistic)").AsNumeric().First();
                    dblpWald = m_pEngine.Evaluate("as.numeric(sum.lm$Wald1$p.value)").AsNumeric().First();
                    
                    double dblParaCnt = m_pEngine.Evaluate("as.numeric(sum.lm$parameters)").AsNumeric().First();
                    dblAIC = (2 * dblParaCnt) - (2 * dblLRErrorModel);

                }
                else
                {
                    dblSigmasquared = m_pEngine.Evaluate("as.numeric(sum.lm$fit$s2)").AsNumeric().First();
                    double dblParaCnt = m_pEngine.Evaluate("as.numeric(sum.lm$parameters)").AsNumeric().First();
                    dblAIC = (2 * dblParaCnt) - (2 * dblLRErrorModel);
                }
                
                //int intNObser = pEngine.Evaluate("as.numeric(nrow(sum.lm$X))").AsInteger().First();
                //int intNParmeter = pEngine.Evaluate(" as.numeric(sum.lm$parameters)").AsInteger().First();
                
                double dblLambda = 0;
                double dblSELambda = 0;
                double dblResiAuto = 0;
                double dblResiAutoP = 0;


                if(rbtLag.Checked||rbtDurbin.Checked)
                {
                    dblLambda = m_pEngine.Evaluate("as.numeric(sum.lm$rho)").AsNumeric().First();
                    dblSELambda = m_pEngine.Evaluate("as.numeric(sum.lm$rho.se)").AsNumeric().First();
                    dblResiAuto = m_pEngine.Evaluate("as.numeric(sum.lm$LMtest)").AsNumeric().First();
                    dblResiAutoP = m_pEngine.Evaluate("as.numeric(sum.lm$rho.se)").AsNumeric().First();
                }
                else
                {
                    dblLambda = m_pEngine.Evaluate("as.numeric(sum.lm$lambda)").AsNumeric().First();
                    dblSELambda = m_pEngine.Evaluate("as.numeric(sum.lm$lambda.se)").AsNumeric().First();
                }
                double dblRsquared = m_pEngine.Evaluate("as.numeric(sum.lm$NK)").AsNumeric().First();
                //Draw result form

                //Open Ouput form
                frmRegResult pfrmRegResult = new frmRegResult();
                if (rbtError.Checked)
                    pfrmRegResult.Text = "Spatial Autoregressive Model Summary (Error Model)";
                else if (rbtLag.Checked)
                    pfrmRegResult.Text = "Spatial Autoregressive Model Summary (Lag Model)";
                else if (rbtCAR.Checked)
                    pfrmRegResult.Text = "Spatial Autoregressive Model Summary (CAR Model)";
                else if (rbtDurbin.Checked)
                    pfrmRegResult.Text = "Spatial Autoregressive Model Summary (Spatial Durbin Model)";
                else
                    pfrmRegResult.Text = "Spatial Autoregressive Model Summary (SMA Model)";

                //pfrmRegResult.panel2.Visible = true;
                //Create DataTable to store Result
                System.Data.DataTable tblRegResult = new DataTable("SRResult");

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
                String.Format("{0:0.##}", tblRegResult.Columns["Std. Error"]);

                DataColumn dColTValue = new DataColumn();
                dColTValue.DataType = System.Type.GetType("System.Double");
                dColTValue.ColumnName = "z value";
                tblRegResult.Columns.Add(dColTValue);

                DataColumn dColPvT = new DataColumn();
                dColPvT.DataType = System.Type.GetType("System.Double");
                dColPvT.ColumnName = "Pr(>|z|)";
                tblRegResult.Columns.Add(dColPvT);

                if (rbtDurbin.Checked)
                    nIDepen = nIDepen * 2;

                //Store Data Table by R result
                for (int j = 0; j < nIDepen + 1; j++)
                {
                    DataRow pDataRow = tblRegResult.NewRow();
                    if (j == 0)
                    {
                        pDataRow["Name"] = "(Intercept)";
                    }
                    else
                    {
                        if (rbtDurbin.Checked)
                        {
                            if(j <= nIDepen /2)
                                pDataRow["Name"] = independentNames[j - 1];
                            else
                                pDataRow["Name"] = "lag." + independentNames[j - (nIDepen / 2) - 1];

                        }
                        else
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

                //Assign values at Textbox
                string strDecimalPlaces = "N" + intDeciPlaces.ToString();
                string[] strResults = new string[5];
                if (rbtLag.Checked||rbtDurbin.Checked)
                {
                    strResults[0] = "rho: " + dblLambda.ToString(strDecimalPlaces) +
                        ", LR Test Value: " + dblLRLambda.ToString(strDecimalPlaces) + ", p-value: " + dblpLambda.ToString(strDecimalPlaces);
                    strResults[1] = "Asymptotic S.E: " + dblSELambda.ToString(strDecimalPlaces) +
                        ", Wald: " + dblWald.ToString(strDecimalPlaces) + ", p-value: " + dblpWald.ToString(strDecimalPlaces);
                    strResults[2] = "Log likelihood: " + dblLRErrorModel.ToString(strDecimalPlaces) +
                        ", Sigma-squared: " + dblSigmasquared.ToString(strDecimalPlaces);
                    strResults[3] = "AIC: " + dblAIC.ToString(strDecimalPlaces) + ", LM test for residuals autocorrelation: " + dblResiAuto.ToString(strDecimalPlaces);
                }
                else if(rbtError.Checked)
                {
                    strResults[0] = "Lambda: " + dblLambda.ToString(strDecimalPlaces) +
                        ", LR Test Value: " + dblLRLambda.ToString(strDecimalPlaces) + ", p-value: " + dblpLambda.ToString(strDecimalPlaces);
                    strResults[1] = "Asymptotic S.E: " + dblSELambda.ToString(strDecimalPlaces) +
                        ", Wald: " + dblWald.ToString(strDecimalPlaces) + ", p-value: " + dblpWald.ToString(strDecimalPlaces);
                    strResults[2] = "Log likelihood: " + dblLRErrorModel.ToString(strDecimalPlaces) +
                        ", Sigma-squared: " + dblSigmasquared.ToString(strDecimalPlaces);
                    strResults[3] = "AIC: " + dblAIC.ToString(strDecimalPlaces);
                }
                else
                {
                    strResults[0] = "Lambda: " + dblLambda.ToString(strDecimalPlaces) +
                        ", LR Test Value: " + dblLRLambda.ToString(strDecimalPlaces) + ", p-value: " + dblpLambda.ToString(strDecimalPlaces);
                    strResults[1] = "Numerical Hessian S.E of lambda: " + dblSELambda.ToString(strDecimalPlaces);
                    strResults[2] = "Log likelihood: " + dblLRErrorModel.ToString(strDecimalPlaces) +
                        ", Sigma-squared: " + dblSigmasquared.ToString(strDecimalPlaces);
                    strResults[3] = "AIC: " + dblAIC.ToString(strDecimalPlaces);
                }
                strResults[4] = "Nagelkerke pseudo-R-squared: " + dblRsquared.ToString(strDecimalPlaces);

                pfrmRegResult.txtOutput.Lines = strResults;

                //Save Outputs in SHP
                if (chkSave.Checked)
                {
                    pfrmProgress.lblStatus.Text = "Saving residuals:";
                    //The field names are related with string[] DeterminedName in clsSnippet 
                    string strResiFldName = lstSave.Items[0].SubItems[1].Text;

                    //Get EVs and residuals
                    NumericVector nvResiduals = m_pEngine.Evaluate("as.numeric(sum.lm$residuals)").AsNumeric();

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
                    pFCursor.Flush();
                    pFCursor = m_pFClass.Update(null, false);
                    pFeature = pFCursor.NextFeature();

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

                pfrmProgress.Close();
                pfrmRegResult.Show();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void rbtError_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtError.Checked)
            {
                rbtLag.Checked = false;
                rbtCAR.Checked = false;
                rbtSMA.Checked = false;
                rbtDurbin.Checked = false;


                rbtMatrix.Enabled = true;
                rbtMatrixJ.Enabled = true;
                rbtLU.Enabled = true;
                rbtChebyshev.Enabled = true;
                rbtMC.Enabled = true;
            }
        }

        private void rbtLag_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtLag.Checked)
            {
                rbtError.Checked = false;
                //rbtLag.Checked = false;
                rbtCAR.Checked = false;
                rbtSMA.Checked = false;
                rbtDurbin.Checked = false;


                rbtMatrix.Enabled = true;
                rbtMatrixJ.Enabled = true;
                rbtLU.Enabled = true;
                rbtChebyshev.Enabled = true;
                rbtMC.Enabled = true;
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

        private void rbtCAR_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtCAR.Checked)
            {
                rbtError.Checked = false;
                rbtLag.Checked = false;
                //rbtCAR.Checked = false;
                rbtSMA.Checked = false;
                rbtDurbin.Checked = false;

                rbtMatrix.Enabled = true;
                rbtMatrixJ.Enabled = true;
                rbtLU.Enabled = true;
                rbtChebyshev.Enabled = true;
                rbtMC.Enabled = true;
            }
        }

        private void rbtSMA_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtSMA.Checked)
            {
                rbtError.Checked = false;
                rbtLag.Checked = false;
                rbtCAR.Checked = false;
                //rbtSMA.Checked = false;
                rbtDurbin.Checked = false;

                rbtEigen.Checked = true;
                rbtMatrix.Checked = false;
                rbtMatrixJ.Checked = false;
                rbtLU.Checked = false;
                rbtChebyshev.Checked = false;
                rbtMC.Checked = false;

                rbtMatrix.Enabled = false;
                rbtMatrixJ.Enabled = false;
                rbtLU.Enabled = false;
                rbtChebyshev.Enabled = false;
                rbtMC.Enabled = false;
            }
        }

        private void rbtDurbin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtDurbin.Checked)
            {
                rbtError.Checked = false;
                rbtLag.Checked = false;
                rbtCAR.Checked = false;
                rbtSMA.Checked = false;
                //rbtDurbin.Checked = false;


                rbtMatrix.Enabled = true;
                rbtMatrixJ.Enabled = true;
                rbtLU.Enabled = true;
                rbtChebyshev.Enabled = true;
                rbtMC.Enabled = true;
            }
        }

        private void rbtEigen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtEigen.Checked)
            {
                rbtMatrix.Checked = false;
                rbtMatrixJ.Checked = false;
                rbtLU.Checked = false;
                rbtChebyshev.Checked = false;
                rbtMC.Checked = false;
            }
        }

        private void rbtMatrix_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMatrix.Checked)
            {
                //rbtMatrix.Checked = false;
                rbtMatrixJ.Checked = false;
                rbtLU.Checked = false;
                rbtChebyshev.Checked = false;
                rbtMC.Checked = false;
                rbtEigen.Checked = false;
            }
        }

        private void rbtMatrixJ_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMatrixJ.Checked)
            {
                rbtMatrix.Checked = false;
                //rbtMatrixJ.Checked = false;
                rbtLU.Checked = false;
                rbtChebyshev.Checked = false;
                rbtMC.Checked = false;
                rbtEigen.Checked = false;
            }
        }

        private void rbtLU_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtLU.Checked)
            {
                rbtMatrix.Checked = false;
                rbtMatrixJ.Checked = false;
                //rbtLU.Checked = false;
                rbtChebyshev.Checked = false;
                rbtMC.Checked = false;
                rbtEigen.Checked = false;
            }
        }

        private void rbtChebyshev_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtChebyshev.Checked)
            {
                rbtMatrix.Checked = false;
                rbtMatrixJ.Checked = false;
                rbtLU.Checked = false;
                //rbtChebyshev.Checked = false;
                rbtMC.Checked = false;
                rbtEigen.Checked = false;
            }
        }

        private void rbtMC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMC.Checked)
            {
                rbtMatrix.Checked = false;
                rbtMatrixJ.Checked = false;
                rbtLU.Checked = false;
                rbtChebyshev.Checked = false;
                //rbtMC.Checked = false;
                rbtEigen.Checked = false;
            }
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
                }
                else
                {
                    MessageBox.Show("Select a target layer first");

                    chkSave.Checked = false;
                    return;
                }
            }
            else
                lstSave.Enabled = false;
        }
        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass)
        {
            try
            {
                pListView.BeginUpdate();
                string strResiFldNm = "spr_resi";

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
            ctrlpt.Y += 251;
            ctrlpt.X += 328 + (length / 2);
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

    }
}
