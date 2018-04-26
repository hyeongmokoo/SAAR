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
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using RDotNet;
using ESRI.ArcGIS.esriSystem;
//spdep, and maptools packages in R are required


namespace VisUncertainty
{
    public partial class frmBivariateLISA : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private BivariateLISASymbols[] m_pBiLISASym;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        public frmBivariateLISA()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Only polygon to make spatial weight matrix 10/9/15 HK
                        cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                }

                m_pSnippet = new clsSnippet();
                MakingSymbols();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private class BivariateLISASymbols
        {
            public int R; public int G; public int B;
            public string Label;
            public double LValue;
            public double UValue;
        }

        private void MakingSymbols()
        {
            m_pBiLISASym = new BivariateLISASymbols[6];

            for (int i = 0; i < 6; i++)
                m_pBiLISASym[i] = new BivariateLISASymbols();

            m_pBiLISASym[0].R = 49; m_pBiLISASym[0].G = 130; m_pBiLISASym[0].B = 189;
            m_pBiLISASym[0].Label = "< -2"; m_pBiLISASym[0].UValue = -2;

            m_pBiLISASym[1].R = 158; m_pBiLISASym[1].G = 202; m_pBiLISASym[1].B = 225;
            m_pBiLISASym[1].Label = "-2 - -1"; m_pBiLISASym[1].LValue = -2; m_pBiLISASym[1].UValue = -1;

            m_pBiLISASym[2].R = 222; m_pBiLISASym[2].G = 235; m_pBiLISASym[2].B = 247;
            m_pBiLISASym[2].Label = "-1 - 0"; m_pBiLISASym[2].LValue = -1; m_pBiLISASym[2].UValue = 0;

            m_pBiLISASym[3].R = 254; m_pBiLISASym[3].G = 224; m_pBiLISASym[3].B = 210;
            m_pBiLISASym[3].Label = "0 - 1"; m_pBiLISASym[3].LValue = 0; m_pBiLISASym[3].UValue = 1;

            m_pBiLISASym[4].R = 252; m_pBiLISASym[4].G = 146; m_pBiLISASym[4].B = 114;
            m_pBiLISASym[4].Label = "1 - 2"; m_pBiLISASym[4].LValue = 1; m_pBiLISASym[4].UValue = 2;

            m_pBiLISASym[5].R = 222; m_pBiLISASym[5].G = 45; m_pBiLISASym[5].B = 38;
            m_pBiLISASym[5].Label = "> 2"; m_pBiLISASym[5].LValue = 2;
        }
        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass)
        {
            try
            {
                pListView.Items.Clear();

                pListView.BeginUpdate();

                int intNFlds = 1;
                string[] strFldNames = null;
                string[] strLvNames = null;

                string strOutputFldNM = string.Empty;
                string strOutput = string.Empty;

                if(cboMeasure.Text == "Lee's L")
                {
                    if (chkDiagZero.Checked)
                    {
                        strOutput = "Lee's L*";
                    }
                    else
                        strOutput = "Lee's L0";

                    strOutputFldNM = "LocalL";
                }
                else if(cboMeasure.Text == "Local Pearson")
                {


                    if (cboMapOption.Text == "Local Pearson")
                        strOutputFldNM = "LocalR";
                    else if (cboMapOption.Text == "z-score of variable 1")
                        strOutputFldNM = "z_v1";
                    else if (cboMapOption.Text == "z-score of variable 2")
                        strOutputFldNM = "z_v2";

                    strOutput = "Local Pearson";
                }

                if (strOutputFldNM == null)
                    return;

                strFldNames = new string[intNFlds];
                strFldNames[0] = strOutputFldNM;

                strLvNames = new string[intNFlds];
                strLvNames[0] = strOutput;

                string[] strNewNames = UpdateFldNames(strFldNames, pFeatureClass);

                for (int i = 0; i < intNFlds; i++)
                {
                    ListViewItem lvi = new ListViewItem(strLvNames[i]);
                    lvi.UseItemStyleForSubItems = false;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, strNewNames[i]));

                    pListView.Items.Add(lvi);
                }

                pListView.EndUpdate();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }
        private string[] UpdateFldNames(string[] strFldNMs, IFeatureClass pFeatureClass)
        {
            try
            {
                int intMax = 0;
                for (int j = 0; j < strFldNMs.Length; j++)
                {
                    string strNM = strFldNMs[j];
                    int i = 1;
                    while (pFeatureClass.FindField(strNM) != -1)
                    {
                        strNM = strFldNMs[j] + "_" + i.ToString();
                        i++;
                    }
                    if (i > intMax)
                        intMax = i;
                }
                string[] strReturnNMs = new string[strFldNMs.Length];
                for (int j = 0; j < strFldNMs.Length; j++)
                {
                    if (intMax == 1)
                        strReturnNMs[j] = strFldNMs[j];
                    else
                        strReturnNMs[j] = strFldNMs[j] + "_" + (intMax - 1).ToString();
                }
                return strReturnNMs;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
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

                IFields fields = m_pFClass.Fields;

                cboFldnm1.Text = "";
                cboFldnm2.Text = "";

                cboFldnm1.Items.Clear();
                cboFldnm2.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                    {
                        cboFldnm1.Items.Add(fields.get_Field(i).Name);
                        cboFldnm2.Items.Add(fields.get_Field(i).Name);
                    }
                }

                UpdateListview(lvFields, m_pFClass);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void cboMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListview(lvFields, m_pFClass);

            if (cboMeasure.Text == "Local Pearson")
            {
                cboMapOption.Enabled = true;
                chkDiagZero.Enabled = false;
                cboMapOption.Text = "Local Pearson";
            }
            else
            {
                chkDiagZero.Enabled = true;
                cboMapOption.Enabled = false;
                cboMapOption.Text = string.Empty;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cboFldnm1.Text == "" || cboFldnm2.Text == "")
            {
                MessageBox.Show("Please select target field");
                return;
            }

            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.lblStatus.Text = "Processing:";
            pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
            pfrmProgress.Show();

            REngine pEngine = m_pForm.pEngine;

            int nFeature = m_pFClass.FeatureCount(null);

            IFeatureCursor pFCursor = m_pFClass.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get index for independent and dependent variables
            //Get variable index            
            string strVarNM1 = (string)cboFldnm1.SelectedItem;
            string strVarNM2 = (string)cboFldnm2.SelectedItem;
            int intVarIdx1 = m_pFClass.FindField(strVarNM1);
            int intVarIdx2 = m_pFClass.FindField(strVarNM2);

            //Store Variable at Array
            double[] arrVar1 = new double[nFeature];
            double[] arrVar2 = new double[nFeature];

            int i = 0;

            while (pFeature != null)
            {
                arrVar1[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx1));
                arrVar2[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx2));
                i++;
                pFeature = pFCursor.NextFeature();
            }

            pFCursor.Flush();

            //Plot command for R
            StringBuilder plotCommmand = new StringBuilder();

            string strStartPath = m_pForm.strPath;
            string pathr = strStartPath.Replace(@"\", @"/");
            pEngine.Evaluate("source('" + pathr + "/ESDA_LEE/AllFunctions_LARRY.R')");
            pEngine.Evaluate("source('" + pathr + "/ESDA_LEE/AllFunctions_neighbor.R')");
            pEngine.Evaluate("source('" + pathr + "/ESDA_LEE/AllFunctions_SASbi.R')");

            //Get the file path and name to create spatial weight matrix
            string strNameR = m_pSnippet.FilePathinRfromLayer(m_pFLayer);

            if (strNameR == null)
                return;

            //Create spatial weight matrix in R
            pEngine.Evaluate("library(spdep); library(maptools)");
            pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");
            //pEngine.Evaluate("sample.nb <- poly2nb(sample.shp, queen=FALSE)");
            pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)");

            NumericVector vecVar1 = pEngine.CreateNumericVector(arrVar1);
            pEngine.SetSymbol("sample.v1", vecVar1);
            NumericVector vecVar2 = pEngine.CreateNumericVector(arrVar2);
            pEngine.SetSymbol("sample.v2", vecVar2);

            //string strRSigLv = nudRsigLv.Value.ToString();
            //string strLSigLv = nudLsigLv.Value.ToString();
            //string strLSig = cboLocalL.Text;
            //string strRsig = cboLocalPearson.Text;
            //string strRowStd = cboRowStandardization.Text;
            //string strMaxRanges = nudMaxRange.Value.ToString();
            //string strHigherOrder = cboHigherOrder.Text;
            string strNonZero = null;
            if (chkDiagZero.Checked)
                strNonZero = "FALSE";
            else
                strNonZero = "TRUE";

            double[] dblLoclLisa = null;

            if (cboMeasure.Text=="Lee's L")
            {
                pEngine.Evaluate("sample.result <- LARRY.bivariate.LISA.lee(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, style = 'W', diag.zero = " + strNonZero + ")");
                dblLoclLisa = pEngine.Evaluate("as.numeric(sample.result$local.L)").AsNumeric().ToArray();
            }
            else if(cboMeasure.Text== "Local Pearson")
            {
                pEngine.Evaluate("sample.result <- LARRY.bivariate.LISA.pearson(sample.v1, sample.v2, 1:length(sample.nb))");

                if (cboMapOption.Text == "Local Pearson")
                    dblLoclLisa = pEngine.Evaluate("as.numeric(sample.result$local.pearson)").AsNumeric().ToArray();
                else if(cboMapOption.Text == "z-score of variable 1")
                    dblLoclLisa = pEngine.Evaluate("as.numeric(sample.result$z.x)").AsNumeric().ToArray();
                else if (cboMapOption.Text == "z-score of variable 2")
                    dblLoclLisa = pEngine.Evaluate("as.numeric(sample.result$z.y)").AsNumeric().ToArray();

            }

            //Save Output on SHP
            //Add Target fields to store results in the shapefile // Keep loop 
            for (int j = 0; j < 1; j++)
            {
                string strfldName = lvFields.Items[j].SubItems[1].Text;
                if (m_pFClass.FindField(strfldName) == -1)
                {
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strfldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    m_pFClass.AddField(newField);
                }
            }

            //Update Field
            pFCursor = m_pFClass.Update(null, false);
            pFeature = pFCursor.NextFeature();

            string strLocalLISAFldName = lvFields.Items[0].SubItems[1].Text;
            int intSpQuadFldIdx = m_pFClass.FindField(strLocalLISAFldName);

            int featureIdx = 0;
            while (pFeature != null)
            {
                pFeature.set_Value(intSpQuadFldIdx, dblLoclLisa[featureIdx]);

                pFCursor.UpdateFeature(pFeature);

                pFeature = pFCursor.NextFeature();
                featureIdx++;
            }
            pFCursor.Flush();

            if (chkMap.Checked)
            {
                ITable pTable = (ITable)m_pFClass;

                pFCursor = m_pFClass.Search(null, false);
                IDataStatistics pDataStat = new DataStatisticsClass();
                pDataStat.Field = strLocalLISAFldName;
                pDataStat.Cursor = (ICursor)pFCursor;

                IStatisticsResults pStatResults = pDataStat.Statistics;
                double dblMax = pStatResults.Maximum;
                double dblMin = pStatResults.Minimum;

                int intBreaksCount = m_pBiLISASym.Length + 1;
                double[] cb = new double[intBreaksCount];

                //Assign Min and Max values for class breaks
                cb[0] = dblMin;
                cb[intBreaksCount - 1] = dblMax;
                
                for (int k = 0; k < intBreaksCount - 2; k++)
                    cb[k + 1] = m_pBiLISASym[k].UValue;

                IClassBreaksRenderer pCBRenderer = new ClassBreaksRenderer();
                pCBRenderer.Field = strLocalLISAFldName;
                pCBRenderer.BreakCount = intBreaksCount - 1;
                pCBRenderer.MinimumBreak = cb[0];

                //' use this interface to set dialog properties
                IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pCBRenderer;
                pUIProperties.ColorRamp = "Custom";
                ISimpleFillSymbol pSimpleFillSym;

                //int[,] arrColors = CreateColorRamp();

                ////Add Probability Value Manually
                //string[] strsProbLabels = new string[] { "(0.01)", "(0.05)", "(0.1)", "(0.1)", "(0.05)", "(0.01)" };
                
                //' be careful, indices are different for the diff lists
                for (int j = 0; j < intBreaksCount - 1; j++)
                {
                    pCBRenderer.Break[j] = cb[j + 1];
                    pCBRenderer.Label[j] = m_pBiLISASym[j].Label;

                    pUIProperties.LowBreak[j] = cb[j];
                    pSimpleFillSym = new SimpleFillSymbolClass();
                    IRgbColor pRGBColor = m_pSnippet.getRGB(m_pBiLISASym[j].R, m_pBiLISASym[j].G, m_pBiLISASym[j].B);
                    pSimpleFillSym.Color = (IColor)pRGBColor;
                    pCBRenderer.Symbol[j] = (ISymbol)pSimpleFillSym;
                }

                IFeatureLayer pNewFLayer = new FeatureLayerClass();
                pNewFLayer.FeatureClass = m_pFClass;
                pNewFLayer.Name = lvFields.Items[0].SubItems[0].Text + " of " + m_pFLayer.Name;
                IGeoFeatureLayer pGFLayer = (IGeoFeatureLayer)pNewFLayer;
                pGFLayer.Renderer = (IFeatureRenderer)pCBRenderer;
                m_pActiveView.FocusMap.AddLayer(pGFLayer);
                m_pActiveView.Refresh();
                m_pForm.axTOCControl1.Update();

                pfrmProgress.Close();
            }
            else
                MessageBox.Show("Complete. The results are stored in the shape file");
        }

        private void cboMapOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListview(lvFields, m_pFClass);
        }

        private void chkDiagZero_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListview(lvFields, m_pFClass);
        }
    }
}
