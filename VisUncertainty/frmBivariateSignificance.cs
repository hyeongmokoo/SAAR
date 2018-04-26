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
    public partial class frmBivariateSignificance : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private BivariateQuandrantSymbols[] m_pQuadrantSymbols;

        private IFeatureClass m_pFClass;
        private IFeatureLayer m_pFLayer;

        public frmBivariateSignificance()
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
        private class BivariateQuandrantSymbols
        {
            public int R; public int G; public int B;
            public string Label;
            public string Value;
            public double Angle;
        }
        private void MakingSymbols()
        {
            m_pQuadrantSymbols = new BivariateQuandrantSymbols[5];

            for (int i = 0; i < 5; i++)
                m_pQuadrantSymbols[i] = new BivariateQuandrantSymbols();

            m_pQuadrantSymbols[0].R = 228; m_pQuadrantSymbols[0].G = 26; m_pQuadrantSymbols[0].B = 28;
            m_pQuadrantSymbols[0].Label = "H*=H*"; m_pQuadrantSymbols[0].Value = "High-High"; m_pQuadrantSymbols[0].Angle = 270;

            m_pQuadrantSymbols[1].R = 152; m_pQuadrantSymbols[1].G = 78; m_pQuadrantSymbols[1].B = 163;
            m_pQuadrantSymbols[1].Label = "H*=L*"; m_pQuadrantSymbols[1].Value = "High-Low"; m_pQuadrantSymbols[1].Angle = 180;

            m_pQuadrantSymbols[2].R = 77; m_pQuadrantSymbols[2].G = 175; m_pQuadrantSymbols[2].B = 74;
            m_pQuadrantSymbols[2].Label = "L*=H*"; m_pQuadrantSymbols[2].Value = "Low-High"; m_pQuadrantSymbols[2].Angle = 0;

            m_pQuadrantSymbols[3].R = 55; m_pQuadrantSymbols[3].G = 126; m_pQuadrantSymbols[3].B = 184;
            m_pQuadrantSymbols[3].Label = "L*=L*"; m_pQuadrantSymbols[3].Value = "Low-Low"; m_pQuadrantSymbols[3].Angle = 90;

            m_pQuadrantSymbols[4].R = 255; m_pQuadrantSymbols[4].G = 255; m_pQuadrantSymbols[4].B = 255;
            m_pQuadrantSymbols[4].Label = "Not sig.*"; m_pQuadrantSymbols[4].Value = "not sig."; m_pQuadrantSymbols[4].Angle = 0;

        }
        private void UpdateMapLables()
        {
            if (cboMeasure.Text == "Lee's L")
            {
                if (chkDiagZero.Checked)
                {
                    m_pQuadrantSymbols[0].Label = "H*=H*";
                    m_pQuadrantSymbols[1].Label = "H*=L*";
                    m_pQuadrantSymbols[2].Label = "L*=H*";
                    m_pQuadrantSymbols[3].Label = "L*=L*";
                }
                else
                {
                    m_pQuadrantSymbols[0].Label = "H0=H0";
                    m_pQuadrantSymbols[1].Label = "H0=L0";
                    m_pQuadrantSymbols[2].Label = "L0=H0";
                    m_pQuadrantSymbols[3].Label = "L0=L0";
                }
            }
            else if (cboMeasure.Text == "Local Pearson")
            {
                m_pQuadrantSymbols[0].Label = "H=H";
                m_pQuadrantSymbols[1].Label = "H=L";
                m_pQuadrantSymbols[2].Label = "L=H";
                m_pQuadrantSymbols[3].Label = "L=L";
            }

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
                        strOutput = "Lee's L* Sig";
                    }
                    else
                        strOutput = "Lee's L0 Sig";

                    strOutputFldNM = "L_sig";
                }
                else if (cboMeasure.Text == "Local Pearson")
                {
                    strOutput = "Local Pearson";
                    strOutputFldNM = "R_sig";
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
            UpdateMapLables();

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
            UpdateMapLables();
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
            int intFIDIdx = m_pFClass.FindField(m_pFClass.OIDFieldName);

            //Store Variable at Array
            double[] arrVar1 = new double[nFeature];
            double[] arrVar2 = new double[nFeature];

            int[] arrFID = new int[nFeature];

            int i = 0;

            while (pFeature != null)
            {
                arrVar1[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx1));
                arrVar2[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx2));
                arrFID[i] = Convert.ToInt32(pFeature.get_Value(intFIDIdx));
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
            string strSigLv = nudSigLv.Value.ToString();
            string[] strSPQuadrants = null;

            //double[] adblVar1 = null;
            //double[] adblVar2 = null;
            //NumericVector vecCoeff = null;
            if (cboMeasure.Text == "Lee's L")
            {
                pEngine.Evaluate("sample.result <- LARRY.bivariate.sig.quad.lee(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, style = 'W', sig.level=c(" + strSigLv + "), method='" + cboMethod.Text + "', alternative='" + txtAlternative.Text + "', diag.zero = " + strNonZero + ")");

                //pEngine.Evaluate("sample.result <- LARRY.bivariate.sig.quad.lee(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, style = 'W', sig.level=c(" + strSigLv + "), method='total', alternative='two.sided', diag.zero = " + strNonZero + ")");
                strSPQuadrants = pEngine.Evaluate("as.character(sample.result[[6]])").AsCharacter().ToArray();
            }
            else if (cboMeasure.Text == "Local Pearson")
            {
                pEngine.Evaluate("sample.result <- LARRY.bivariate.sig.quad.pearson(sample.v1, sample.v2, 1:length(sample.nb), sig.level=c(" + strSigLv + ", 0.01), method='" + cboMethod.Text + "', alternative='" + txtAlternative.Text + "')");

                //pEngine.Evaluate("sample.result <- LARRY.bivariate.sig.quad.pearson(sample.v1, sample.v2, 1:length(sample.nb), sig.level=c(" + strSigLv + "), method='total', alternative='two.sided')");
                strSPQuadrants = pEngine.Evaluate("as.character(sample.result[[6]])").AsCharacter().ToArray();
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
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    m_pFClass.AddField(newField);
                }
            }

            //Update Field
            pFCursor = m_pFClass.Update(null, false);
            pFeature = pFCursor.NextFeature();

            string strSpQuadFldName = lvFields.Items[0].SubItems[1].Text;
            int intSpQuadFldIdx = m_pFClass.FindField(strSpQuadFldName);

            int featureIdx = 0;
            while (pFeature != null)
            {
                pFeature.set_Value(intSpQuadFldIdx, strSPQuadrants[featureIdx]);

                pFCursor.UpdateFeature(pFeature);

                pFeature = pFCursor.NextFeature();
                featureIdx++;
            }
            pFCursor.Flush();

            if (chkMap.Checked)
            {
                ITable pTable = (ITable)m_pFClass;

                IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
                pUniqueValueRenderer.FieldCount = 1;
                pUniqueValueRenderer.set_Field(0, strSpQuadFldName);

                if (cboMaptype.Text == "choropleth map")
                {

                    ISimpleFillSymbol pSymbol;
                    IQueryFilter pQFilter = new QueryFilterClass();
                    int intTotalCount = 0;
                    string strLabel = null;

                    for (int j = 0; j < 5; j++)
                    {
                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(m_pQuadrantSymbols[j].R, m_pQuadrantSymbols[j].G, m_pQuadrantSymbols[j].B);

                        pQFilter.WhereClause = strSpQuadFldName + " = '" + m_pQuadrantSymbols[j].Value + "'";

                        intTotalCount = pTable.RowCount(pQFilter);

                        strLabel = m_pQuadrantSymbols[j].Label + " (" + intTotalCount.ToString() + ")";
                        pUniqueValueRenderer.AddValue(m_pQuadrantSymbols[j].Value, null, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pQuadrantSymbols[j].Value, strLabel);
                    }
                    pUniqueValueRenderer.UseDefaultSymbol = false;
                }
                else if (cboMaptype.Text == "point map")
                {
                    ICharacterMarkerSymbol pSymbol;
                    stdole.IFontDisp stdFontCls = ((stdole.IFontDisp)(new stdole.StdFont()));
                    stdFontCls.Name = "ESRI NIMA VMAP1&2 PT";
                    stdFontCls.Size = 10;

                    IQueryFilter pQFilter = new QueryFilterClass();
                    int intTotalCount = 0;
                    string strLabel = null;

                    for (int j = 0; j < 4; j++)
                    {
                        ////Without hallo
                        //pSymbol = new CharacterMarkerSymbolClass();
                        //pSymbol.CharacterIndex = 248;
                        ////pSymbol.Color = m_pSnippet.getRGB(m_pQuadrantSymbols[j].R, m_pQuadrantSymbols[j].G, m_pQuadrantSymbols[j].B);
                        //pSymbol.Size = 10;
                        //pSymbol.Font = stdFontCls;

                        //pSymbol.Angle = m_pQuadrantSymbols[j].Angle;

                        //With hallo
                        pSymbol = new CharacterMarkerSymbolClass();
                        pSymbol.CharacterIndex = 248;
                        //pSymbol.Color = m_pSnippet.getRGB(m_pQuadrantSymbols[j].R, m_pQuadrantSymbols[j].G, m_pQuadrantSymbols[j].B);
                        pSymbol.Size = 10;
                        pSymbol.Font = stdFontCls;

                        pSymbol.Angle = m_pQuadrantSymbols[j].Angle;

                        //Create a Fill Symbol for the Mask 
                        ISimpleFillSymbol smpFill = new SimpleFillSymbol();
                        smpFill.Color = m_pSnippet.getRGB(0, 0, 0);
                        smpFill.Style = esriSimpleFillStyle.esriSFSSolid;
                        //Create a MultiLayerMarkerSymbol
                        IMultiLayerMarkerSymbol multiLyrMrk = new MultiLayerMarkerSymbol();
                        //Add the simple marker to the MultiLayer 
                        multiLyrMrk.AddLayer(pSymbol);
                        //Create a Mask for the MultiLayerMarkerSymbol 
                        IMask mrkMask = (IMask)multiLyrMrk;
                        mrkMask.MaskSymbol = smpFill;
                        mrkMask.MaskStyle = esriMaskStyle.esriMSHalo;
                        mrkMask.MaskSize = 0.5;

///to here
                        pQFilter.WhereClause = strSpQuadFldName + " = '" + m_pQuadrantSymbols[j].Value + "'";

                        intTotalCount = pTable.RowCount(pQFilter);

                        strLabel = m_pQuadrantSymbols[j].Label + " (" + intTotalCount.ToString() + ")";
                        pUniqueValueRenderer.AddValue(m_pQuadrantSymbols[j].Value, null, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pQuadrantSymbols[j].Value, strLabel);
                    }
                    pUniqueValueRenderer.UseDefaultSymbol = false;
                }
                IFeatureLayer pNewFLayer = new FeatureLayerClass();
                pNewFLayer.FeatureClass = m_pFClass;
                pNewFLayer.Name = "Bivariate Spatial Significant Quadrant";
                IGeoFeatureLayer pGFLayer = (IGeoFeatureLayer)pNewFLayer;
                pGFLayer.Renderer = (IFeatureRenderer)pUniqueValueRenderer;
                m_pActiveView.FocusMap.AddLayer(pGFLayer);
                m_pActiveView.Refresh();
                m_pForm.axTOCControl1.Update();
            }
            pfrmProgress.Close();
        }
    }
}
