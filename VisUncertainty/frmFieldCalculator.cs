using System;
using System.Linq;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseUI;

using RDotNet;

namespace VisUncertainty
{
    public partial class frmFieldCalculator : Form
    {
        public IntPtr intHandle; // This is handle of Attribute table 
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;

        public frmFieldCalculator()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                lstFields.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        lstFields.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnAddTarget_Click(object sender, EventArgs e)
        {
            try
            {

                if (lstFields.SelectedItems.Count > 0)
                {
                    txtTargetField.Text = lstFields.Items[lstFields.SelectedIndex].ToString();
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

        private void btnAddExpression_Click(object sender, EventArgs e)
        {
            AddFieldNametoExpression();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboLayer.Text;
                string strExpression = txtNExpression.Text;
                string strTargetFld = txtTargetField.Text;

                //Load Feature Class
                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;


                if (strTargetFld == "")
                {
                    MessageBox.Show("Please select the target field");
                    return;
                }
                else if (strExpression == "")
                {
                    MessageBox.Show("Please insert expression to calculate field");
                    return;
                }

                if (radR.Checked)
                {
                    try
                    {
                        REngine pEngine = mForm.pEngine;
                        // Split the text into an array of words
                        string[] words = strExpression.Split(new char[] { ',', '$', '(', ')', '/', '*', '+', '-', '^', '=', ' ' }, StringSplitOptions.RemoveEmptyEntries); //Any other splitters?

                        //Get number and names of Expressed fields
                        int intNField = lstFields.Items.Count;
                        int intNExpressedFld = 0;

                        string[] fieldNames = new string[intNField];

                        for (int j = 0; j < intNField; j++)
                        {
                            string strTemp = (string)lstFields.Items[j];
                            if (words.Contains(strTemp))
                            {
                                fieldNames[intNExpressedFld] = (string)lstFields.Items[j];
                                intNExpressedFld++;
                            }
                        }

                        //Get Features
                        int nFeature = pFClass.FeatureCount(null);

                        IFeatureCursor pFCursor = pFLayer.Search(null, true);
                        IFeature pFeature = pFCursor.NextFeature();

                        //Get index for target and source fields
                        int intTargetIdx = pFLayer.FeatureClass.Fields.FindField(strTargetFld);
                        IField pField = pFLayer.FeatureClass.Fields.Field[intTargetIdx];
                        if (pField.Type != esriFieldType.esriFieldTypeDouble)
                        {
                            MessageBox.Show("R sytanx is only applicable to Double field type");
                            return;
                        }

                        NumericVector vecResult = null;
                        bool blnINF = false;
                        bool blnNAN = false;
                        LogicalVector vecINF = null;
                        LogicalVector vecNAN = null;

                        //Get values from selected fields
                        if (intNExpressedFld > 0)
                        {
                            int[] idxes = new int[intNExpressedFld];
                            for (int j = 0; j < intNExpressedFld; j++)
                            {
                                idxes[j] = pFLayer.FeatureClass.Fields.FindField(fieldNames[j]);
                            }

                            //Store values in source fields into Array
                           //double[,] arrSource = new double[nFeature, intNExpressedFld];
                            double[][] arrSource = new double[intNExpressedFld][];

                            for (int j = 0; j < intNExpressedFld; j++)
                            {
                                arrSource[j] = new double[nFeature];
                            }

                            int i = 0;
                            while (pFeature != null)
                            {
                                for (int j = 0; j < intNExpressedFld; j++)
                                {
                                    arrSource[j][i] = Convert.ToDouble(pFeature.get_Value(idxes[j]));
                                }

                                i++;
                                pFeature = pFCursor.NextFeature();
                            }

                            //Source value to R vector
                            for (int j = 0; j < intNExpressedFld; j++)
                            {
                                //double[] arrVector = arrSource.GetColumn<double>(j);
                                NumericVector vecSource = pEngine.CreateNumericVector(arrSource[j]);
                                pEngine.SetSymbol(fieldNames[j], vecSource);
                            }

                            //Verifying before evaluation
                            vecINF = pEngine.Evaluate("is.infinite(" + strExpression + ")").AsLogical();
                            vecNAN = pEngine.Evaluate("is.nan(" + strExpression + ")").AsLogical();

                            for (int k = 0; k < vecINF.Length; k++)
                            {
                                blnINF = vecINF[k];
                                blnNAN = vecNAN[k];
                                if (blnINF)
                                {
                                    MessageBox.Show("INF");
                                    return;
                                }
                                else if (blnNAN)
                                {
                                    MessageBox.Show("NAN");
                                    return;
                                }
                            }

                            //Calculate
                            vecResult = pEngine.Evaluate(strExpression).AsNumeric();
                        }
                        else if (intNExpressedFld == 0) //Constant
                        {
                            //Verifying before evaluation
                            blnINF = pEngine.Evaluate("is.infinite(" + strExpression + ")").AsLogical().First();
                            blnNAN = pEngine.Evaluate("is.nan(" + strExpression + ")").AsLogical().First();
                            if (blnINF)
                            {
                                MessageBox.Show("INF");
                                return;
                            }
                            else if (blnNAN)
                            {
                                MessageBox.Show("NAN");
                                return;
                            }

                            vecResult = pEngine.Evaluate("rep(" + strExpression + ", " + nFeature.ToString() + ")").AsNumeric();
                        }

                        //Update Field
                        pFCursor = pFLayer.FeatureClass.Update(null, false);
                        pFeature = pFCursor.NextFeature();

                        int featureIdx = 0;

                        while (pFeature != null)
                        {
                            pFeature.set_Value(intTargetIdx, (object)vecResult[featureIdx]);
                            pFCursor.UpdateFeature(pFeature);

                            pFeature = pFCursor.NextFeature();
                            featureIdx++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("R Syntax Error: " + ex.ToString());
                        return;
                    }
                }
                else
                {
                    try
                    {
                        ICalculator pCal = new Calculator();
                        IFeatureCursor pFCursor = pFClass.Update(null, false);
                        pCal.Cursor = (ICursor)pFCursor;
                        pCal.Expression = strExpression;
                        pCal.PreExpression = "";
                        pCal.Field = strTargetFld;
                        pCal.Calculate();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ArcGIS Syntax Error: " + ex.ToString());
                        return;
                    }
                }

                MessageBox.Show("Done");
                if (intHandle != IntPtr.Zero)
                {
                    frmAttributeTable pfrmAttributeTable = pSnippet.returnAttTable(intHandle);
                    if (pfrmAttributeTable == null)
                        return;
                    pSnippet.LoadingAttributeTable(pLayer, pfrmAttributeTable);
                }

            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void lstFields_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lstFields.SelectedItems.Count > 0)
                {
                    if (txtTargetField.Text == "")
                        txtTargetField.Text = (string)lstFields.Items[lstFields.SelectedIndex];
                    else
                        AddFieldNametoExpression();
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

        private void radR_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radR.Checked)
                    rdArcGIS.Checked = false;
                else
                    rdArcGIS.Checked = true;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void AddFieldNametoExpression()
        {
            try{
                if (radR.Checked)
                {
                    if (lstFields.SelectedItems.Count > 0)
                    {
                        txtNExpression.Text = txtNExpression.Text + (string)lstFields.Items[lstFields.SelectedIndex];
                    }
                    else
                        return;
                }
                else
                {
                    if (lstFields.SelectedItems.Count > 0)
                    {
                        txtNExpression.Text = txtNExpression.Text + "[" + (string)lstFields.Items[lstFields.SelectedIndex] + "]";
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            } 
        }
    }
}
