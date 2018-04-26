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
    public partial class frmBivariateProbability : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private BivariateProbSymbols[] m_pBiLISASym;
        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        public frmBivariateProbability()
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
        private class BivariateProbSymbols
        {
            public int R; public int G; public int B;
            public string Label;
            public double LValue;
            public double UValue;
        }

        private void MakingSymbols()
        {
            //Class 9

            m_pBiLISASym = new BivariateProbSymbols[9];

            for (int i = 0; i < 9; i++)
            {
                m_pBiLISASym[i] = new BivariateProbSymbols();
            }

            m_pBiLISASym[0].LValue = 0; m_pBiLISASym[0].UValue = 0.001; m_pBiLISASym[0].Label = "< 0.001";
            m_pBiLISASym[0].R = 8; m_pBiLISASym[0].G = 290; m_pBiLISASym[0].B = 88;

            m_pBiLISASym[1].LValue = 0.001; m_pBiLISASym[1].UValue = 0.005; m_pBiLISASym[1].Label = "0.001 - 0.005";
            m_pBiLISASym[1].R = 37; m_pBiLISASym[1].G = 52; m_pBiLISASym[1].B = 148;

            m_pBiLISASym[2].LValue = 0.005; m_pBiLISASym[2].UValue = 0.01; m_pBiLISASym[2].Label = "0.005 - 0.01";
            m_pBiLISASym[2].R = 34; m_pBiLISASym[2].G = 94; m_pBiLISASym[2].B = 168;

            m_pBiLISASym[3].LValue = 0.01; m_pBiLISASym[3].UValue = 0.05; m_pBiLISASym[3].Label = "0.01 - 0.05";
            m_pBiLISASym[3].R = 29; m_pBiLISASym[3].G = 145; m_pBiLISASym[3].B = 192;

            m_pBiLISASym[4].LValue = 0.05; m_pBiLISASym[4].UValue = 0.10; m_pBiLISASym[4].Label = "0.05 - 0.10";
            m_pBiLISASym[4].R = 65; m_pBiLISASym[4].G = 182; m_pBiLISASym[4].B = 196;

            m_pBiLISASym[5].LValue = 0.10; m_pBiLISASym[5].UValue = 0.25; m_pBiLISASym[5].Label = "0.10 - 0.25";
            m_pBiLISASym[5].R = 127; m_pBiLISASym[5].G = 205; m_pBiLISASym[5].B = 187;

            m_pBiLISASym[6].LValue = 0.25; m_pBiLISASym[6].UValue = 0.50; m_pBiLISASym[6].Label = "0.25 - 0.50";
            m_pBiLISASym[6].R = 199; m_pBiLISASym[6].G = 233; m_pBiLISASym[6].B = 180;

            m_pBiLISASym[7].LValue = 0.50; m_pBiLISASym[7].UValue = 0.75; m_pBiLISASym[7].Label = "0.50 - 0. 75";
            m_pBiLISASym[7].R = 237; m_pBiLISASym[7].G = 248; m_pBiLISASym[7].B = 177;

            m_pBiLISASym[8].LValue = 0.75; m_pBiLISASym[8].UValue = 0.99; m_pBiLISASym[8].Label = "> 0.75";
            m_pBiLISASym[8].R = 255; m_pBiLISASym[8].G = 255; m_pBiLISASym[8].B = 217;

            //Class 10
            //m_pBiLISASym = new BivariateProbSymbols[10];

            //for (int i = 0; i < 10; i++)
            //{
            //    m_pBiLISASym[i] = new BivariateProbSymbols();
            //    m_pBiLISASym[i].LValue = 0.1 * i;
            //    m_pBiLISASym[i].UValue = (0.1 * i) + 0.1;
            //    m_pBiLISASym[i].Label = m_pBiLISASym[i].LValue.ToString() + " - " + m_pBiLISASym[i].UValue.ToString();
            //}


            //m_pBiLISASym[0].R = 8; m_pBiLISASym[0].G = 290; m_pBiLISASym[0].B = 88;

            //m_pBiLISASym[1].R = 37; m_pBiLISASym[1].G = 52; m_pBiLISASym[1].B = 148;

            //m_pBiLISASym[2].R = 34; m_pBiLISASym[2].G = 94; m_pBiLISASym[2].B = 168;

            //m_pBiLISASym[3].R = 29; m_pBiLISASym[3].G = 145; m_pBiLISASym[3].B = 192;

            //m_pBiLISASym[4].R = 65; m_pBiLISASym[4].G = 182; m_pBiLISASym[4].B = 196;

            //m_pBiLISASym[5].R = 127; m_pBiLISASym[5].G = 205; m_pBiLISASym[5].B = 187;

            //m_pBiLISASym[6].R = 199; m_pBiLISASym[6].G = 233; m_pBiLISASym[6].B = 180;
            //m_pBiLISASym[7].R = 237; m_pBiLISASym[7].G = 248; m_pBiLISASym[7].B = 177;
            //m_pBiLISASym[8].R = 255; m_pBiLISASym[8].G = 255; m_pBiLISASym[8].B = 217;
            //m_pBiLISASym[9].R = 255; m_pBiLISASym[9].G = 255; m_pBiLISASym[9].B = 255;



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

                if (cboMeasure.Text == "Lee's L")
                {
                    if (chkDiagZero.Checked)
                    {
                        strOutput = "Lee's L*";
                    }
                    else
                        strOutput = "Lee's L0";

                    strOutputFldNM = "L_p";
                }
                else if (cboMeasure.Text == "Local Pearson")
                {
                    strOutputFldNM = "R_p";

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

        private void cboMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListview(lvFields, m_pFClass);

            if (cboMeasure.Text == "Local Pearson")
            {
                chkDiagZero.Enabled = false;

                cboMethod.Text = "randomization";
                cboMethod.Items.Clear();
                cboMethod.Items.Add("randomization");
                cboMethod.Items.Add("normality");
            }
            else
            {
                chkDiagZero.Enabled = true;

                cboMethod.Text = "total";
                cboMethod.Items.Clear();
                cboMethod.Items.Add("total");
                cboMethod.Items.Add("conditional");
            }
        }

        private void chkDiagZero_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListview(lvFields, m_pFClass);
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

            string strNonZero = null;
            if (chkDiagZero.Checked)
                strNonZero = "FALSE";
            else
                strNonZero = "TRUE";

            double[] dblLoclLisa = null;

            if (cboMeasure.Text == "Lee's L")
            {
                //pEngine.Evaluate("sample.result <- LARRY.bivariate.probability.lee(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, style = 'W', sig.level=c(0.05), method='total', alternative='two.sided', diag.zero = " + strNonZero + ")");

                pEngine.Evaluate("sample.result <- LARRY.bivariate.probability.lee(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, style = 'W', sig.level=c(0.05), method='"+cboMethod.Text+"', alternative='"+txtAlternative.Text+"', diag.zero = " + strNonZero + ")");
                dblLoclLisa = pEngine.Evaluate("as.numeric(sample.result$p.value)").AsNumeric().ToArray();
            }
            else if (cboMeasure.Text == "Local Pearson")
            {
                //pEngine.Evaluate("sample.result <- LARRY.bivariate.probability.pearson(sample.v1, sample.v2, 1:length(sample.nb), sig.levels=c(0.05), method='randomization', alternative='two.sided')");
                pEngine.Evaluate("sample.result <- LARRY.bivariate.probability.pearson(sample.v1, sample.v2, 1:length(sample.nb), sig.levels=c(0.05), method='" + cboMethod.Text + "', alternative='" + txtAlternative.Text + "')");
                dblLoclLisa = pEngine.Evaluate("as.numeric(sample.result$p.value)").AsNumeric().ToArray();

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

                for (int k = 0; k < intBreaksCount; k++)
                    cb[k] = k * 0.1;

                IClassBreaksRenderer pCBRenderer = new ClassBreaksRenderer();
                pCBRenderer.Field = strLocalLISAFldName;
                pCBRenderer.BreakCount = intBreaksCount - 1;
                pCBRenderer.MinimumBreak = cb[0];

                //' use this interface to set dialog properties
                IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pCBRenderer;
                pUIProperties.ColorRamp = "Custom";
                ISimpleFillSymbol pSimpleFillSym;
                
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
    }
}
