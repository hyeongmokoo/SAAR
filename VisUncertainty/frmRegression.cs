using System;
using System.IO;
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

using RDotNet;
using RDotNet.NativeLibrary;

//using Accord.Controls;
//using Accord.IO;
//using Accord.Math;
//using Accord.Statistics;
//using Accord.Statistics.Analysis;


namespace VisUncertainty
{
    public partial class frmRegression : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private REngine m_pEngine;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        //Varaibles for SWM
        private bool m_blnCreateSWM = false;

        public frmRegression()
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
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                m_pFLayer = pLayer as IFeatureLayer;
                m_pFClass = m_pFLayer.FeatureClass;

                IFields fields = m_pFClass.Fields;

                cboFieldName.Items.Clear();
                lstFields.Items.Clear();
                lstIndeVar.Items.Clear();
                cboFieldName.Text = "";
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                        lstFields.Items.Add(fields.get_Field(i).Name);
                    }
                }
                
                //New Spatial Weight matrix function 080317
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                    txtSWM.Text = pSWMType.strPolySWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    txtSWM.Text = pSWMType.strPointSWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    MessageBox.Show("Spatial weight matrix for polyline is not supported.");

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
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
            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Collecting Data:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            try
            {
                //REngine pEngine = m_pForm.pEngine;
                if (cboFieldName.Text == "")
                {
                    MessageBox.Show("Please select the dependent input variables to be used in the regression model.",
                        "Please choose at least one input variable");
                }
                if (lstIndeVar.Items.Count == 0 && chkIntercept.Checked == false)
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
                clsSnippet pSnippet = new clsSnippet();
                //string strLayerName = cboTargetLayer.Text;

                //int intLIndex = pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                //ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                //IFeatureClass pFClass = pFLayer.FeatureClass;

                int nFeature = m_pFClass.FeatureCount(null);


                IFeatureCursor pFCursor = m_pFClass.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Get index for independent and dependent variables
                int intDepenIdx = m_pFClass.Fields.FindField(dependentName);
                int[] idxes = new int[nIDepen];

                for (int j = 0; j < nIDepen; j++)
                {
                    idxes[j] = m_pFClass.Fields.FindField(independentNames[j]);
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
                pfrmProgress.lblStatus.Text = "Calculate Regression Coefficients";
                //Plot command for R
                StringBuilder plotCommmand = new StringBuilder();

                //Dependent variable to R vector
                NumericVector vecDepen = m_pEngine.CreateNumericVector(arrDepen);
                m_pEngine.SetSymbol(dependentName, vecDepen);
                plotCommmand.Append("lm(" + dependentName + "~");

                if (chkIntercept.Checked == false)
                {
                    for (int j = 0; j < nIDepen; j++)
                    {
                        //double[] arrVector = arrInDepen.GetColumn<double>(j);
                        NumericVector vecIndepen = m_pEngine.CreateNumericVector(arrInDepen[j]);
                        m_pEngine.SetSymbol(independentNames[j], vecIndepen);
                        plotCommmand.Append(independentNames[j] + "+");
                    }
                    plotCommmand.Remove(plotCommmand.Length - 1, 1);
                }
                else
                    plotCommmand.Append("1");

                plotCommmand.Append(")");
                m_pEngine.Evaluate("sum.lm <- summary(" + plotCommmand + ")");

                NumericMatrix matCoe = m_pEngine.Evaluate("as.matrix(sum.lm$coefficient)").AsNumericMatrix();
                NumericVector vecF = m_pEngine.Evaluate("as.numeric(sum.lm$fstatistic)").AsNumeric();
                m_pEngine.Evaluate("fvalue <- as.numeric(sum.lm$fstatistic)");
                double dblPValueF = m_pEngine.Evaluate("pf(fvalue[1],fvalue[2],fvalue[3],lower.tail=F)").AsNumeric().First();
                double dblRsqaure = m_pEngine.Evaluate("sum.lm$r.squared").AsNumeric().First();
                double dblAdjRsqaure = m_pEngine.Evaluate("sum.lm$adj.r.squared").AsNumeric().First();
                double dblResiSE = m_pEngine.Evaluate("sum.lm$sigma").AsNumeric().First();
                NumericVector vecResiDF = m_pEngine.Evaluate("sum.lm$df").AsNumeric();

                double dblResiMC = 0;
                double dblResiMCpVal = 0;

                if (chkResiAuto.Checked)
                {
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
                    

                    m_pEngine.Evaluate("sample.n <- length(sample.nb)");

                    //Calculate MC
                    m_pEngine.Evaluate("zmc <- lm.morantest(" + plotCommmand.ToString() + ", listw=sample.listw, zero.policy=TRUE)");
                    dblResiMC = m_pEngine.Evaluate("zmc$estimate[1]").AsNumeric().First();
                    dblResiMCpVal = m_pEngine.Evaluate("zmc$p.value").AsNumeric().First();
                }

                pfrmProgress.lblStatus.Text = "Printing Output:";
                //Open Ouput form
                frmRegResult pfrmRegResult = new frmRegResult();
                pfrmRegResult.Text = "Linear Regression Summary";

                //Create DataTable to store Result
                System.Data.DataTable tblRegResult = new DataTable("OLSResult");

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
                dColTValue.ColumnName = "t value";
                tblRegResult.Columns.Add(dColTValue);

                DataColumn dColPvT = new DataColumn();
                dColPvT.DataType = System.Type.GetType("System.Double");
                dColPvT.ColumnName = "Pr(>|t|)";
                tblRegResult.Columns.Add(dColPvT);

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
                        pDataRow["Name"] = independentNames[j - 1];
                    }
                    pDataRow["Estimate"] = Math.Round(matCoe[j, 0], intDeciPlaces);
                    pDataRow["Std. Error"] = Math.Round(matCoe[j, 1], intDeciPlaces);
                    pDataRow["t value"] = Math.Round(matCoe[j, 2], intDeciPlaces);
                    pDataRow["Pr(>|t|)"] = Math.Round(matCoe[j, 3], intDeciPlaces);
                    tblRegResult.Rows.Add(pDataRow);
                }

                //Assign Datagridview to Data Table
                pfrmRegResult.dgvResults.DataSource = tblRegResult;

                //Assign values at Textbox
                string[] strResults = null;
                if (chkResiAuto.Checked)
                {
                    strResults = new string[4];
                    strResults[3] = "MC of residuals: " + dblResiMC.ToString("N3") + ", p-value: " + dblResiMCpVal.ToString("N3");
                }
                else
                    strResults = new string[3];

                strResults[0] = "Residual standard error: " + dblResiSE.ToString("N" + intDeciPlaces.ToString()) +
                        " on " + vecResiDF[1].ToString() + " degrees of freedom";
                strResults[1] = "Multiple R-squared: " + dblRsqaure.ToString("N" + intDeciPlaces.ToString()) +
                    ", Adjusted R-squared: " + dblAdjRsqaure.ToString("N" + intDeciPlaces.ToString());

                if (chkIntercept.Checked == false)
                {
                    strResults[2] = "F-Statistic: " + vecF[0].ToString("N" + intDeciPlaces.ToString()) +
                    " on " + vecF[1].ToString() + " and " + vecF[2].ToString() + " DF, p-value: " + dblPValueF.ToString("N" + intDeciPlaces.ToString());

                }
                else
                    strResults[2] = "";

                pfrmRegResult.txtOutput.Lines = strResults;
                pfrmRegResult.Show();

                //Create Plots for Regression
                if (chkPlots.Checked)
                {
                    string strTitle = "Linear Regression Results";
                    string strCommand = "plot(" + plotCommmand + ");";

                    pSnippet.drawPlottoForm(strTitle, strCommand);

                }

                //Save Outputs in SHP
                if (chkSave.Checked)
                {
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

                    MessageBox.Show("Residuals are stored in the shape file");
                }

                pfrmProgress.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();
                pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                pfrmProgress.Close();
                return;
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
                string strResiFldNm = "lin_resi";

                //Update Name Using the UpdateFldName Function

                strResiFldNm = UpdateFldName(strResiFldNm, pFeatureClass);
                if (strResiFldNm == null)
                    return;

                pListView.Items[0].SubItems[1].Text = strResiFldNm;
                pListView.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
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
            ctrlpt.Y += 351;
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

        #endregion

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
            if(chkResiAuto.Checked)
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

        private void chkIntercept_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIntercept.Checked == false)
            {
                lstFields.Enabled = true;
                lstIndeVar.Enabled = true;
                btnMoveLeft.Enabled = true;
                btnMoveRight.Enabled = true;
            }
            else
            {
                lstFields.Enabled = false;
                lstIndeVar.Enabled = false;
                btnMoveLeft.Enabled = false;
                btnMoveRight.Enabled = false;
            }
        }
    }
}
