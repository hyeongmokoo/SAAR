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
    public partial class frmBiQuadrants : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private BivariateRangeSymbols[] m_pRangeSymbols;


        public frmBiQuadrants()
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
        #region private functions and classes 
        private class BivariateRangeSymbols
        {
            public int R; public int G; public int B;
            public string Label;
            public string Value;
            public string Heading;
        }

        private void MakingSymbols()
        {

            m_pRangeSymbols = new BivariateRangeSymbols[20];

            for (int i = 0; i < 20; i++)
                m_pRangeSymbols[i] = new BivariateRangeSymbols();

            m_pRangeSymbols[0].R = 165; m_pRangeSymbols[0].G = 15; m_pRangeSymbols[0].B = 21;
            m_pRangeSymbols[0].Value = "High-High0"; m_pRangeSymbols[0].Label = "Zero order";
            m_pRangeSymbols[0].Heading = "H*=H*";

            m_pRangeSymbols[1].R = 222; m_pRangeSymbols[1].G = 45; m_pRangeSymbols[1].B = 38;
            m_pRangeSymbols[1].Value = "High-High1"; m_pRangeSymbols[1].Label = "1st order";
            m_pRangeSymbols[1].Heading = "H*=H*";

            m_pRangeSymbols[2].R = 251; m_pRangeSymbols[2].G = 106; m_pRangeSymbols[2].B = 74;
            m_pRangeSymbols[2].Value = "High-High2"; m_pRangeSymbols[2].Label = "2nd order";
            m_pRangeSymbols[2].Heading = "H*=H*";

            m_pRangeSymbols[3].R = 252; m_pRangeSymbols[3].G = 174; m_pRangeSymbols[3].B = 145;
            m_pRangeSymbols[3].Value = "High-High3"; m_pRangeSymbols[3].Label = "3rd order";
            m_pRangeSymbols[3].Heading = "H*=H*";

            m_pRangeSymbols[4].R = 254; m_pRangeSymbols[4].G = 229; m_pRangeSymbols[4].B = 217;
            m_pRangeSymbols[4].Value = "High-High4"; m_pRangeSymbols[4].Label = "4th order";
            m_pRangeSymbols[4].Heading = "H*=H*";


            m_pRangeSymbols[5].R = 84; m_pRangeSymbols[5].G = 39; m_pRangeSymbols[5].B = 143;
            m_pRangeSymbols[5].Value = "High-Low0"; m_pRangeSymbols[5].Label = "Zero order";
            m_pRangeSymbols[5].Heading = "H*=L*";

            //m_pRangeSymbols[6].R = 117; m_pRangeSymbols[6].G = 107; m_pRangeSymbols[6].B = 177;
            m_pRangeSymbols[6].R = 119; m_pRangeSymbols[6].G = 42; m_pRangeSymbols[6].B = 130;
            m_pRangeSymbols[6].Value = "High-Low1"; m_pRangeSymbols[6].Label = "1st order";
            m_pRangeSymbols[6].Heading = "H*=L*";

            m_pRangeSymbols[7].R = 158; m_pRangeSymbols[7].G = 154; m_pRangeSymbols[7].B = 200;
            m_pRangeSymbols[7].Value = "High-Low2"; m_pRangeSymbols[7].Label = "2nd order";
            m_pRangeSymbols[7].Heading = "H*=L*";

            m_pRangeSymbols[8].R = 203; m_pRangeSymbols[8].G = 201; m_pRangeSymbols[8].B = 226;
            m_pRangeSymbols[8].Value = "High-Low3"; m_pRangeSymbols[8].Label = "3rd order";
            m_pRangeSymbols[8].Heading = "H*=L*";

            m_pRangeSymbols[9].R = 242; m_pRangeSymbols[9].G = 240; m_pRangeSymbols[9].B = 247;
            m_pRangeSymbols[9].Value = "High-Low4"; m_pRangeSymbols[9].Label = "4th order";
            m_pRangeSymbols[9].Heading = "H*=L*";


            m_pRangeSymbols[10].R = 0; m_pRangeSymbols[10].G = 109; m_pRangeSymbols[10].B = 44;
            m_pRangeSymbols[10].Value = "Low-High0"; m_pRangeSymbols[10].Label = "Zero order";
            m_pRangeSymbols[10].Heading = "L*=H*";

            m_pRangeSymbols[11].R = 49; m_pRangeSymbols[11].G = 163; m_pRangeSymbols[11].B = 84;
            m_pRangeSymbols[11].Value = "Low-High1"; m_pRangeSymbols[11].Label = "1st order";
            m_pRangeSymbols[11].Heading = "L*=H*";

            m_pRangeSymbols[12].R = 116; m_pRangeSymbols[12].G = 196; m_pRangeSymbols[12].B = 118;
            m_pRangeSymbols[12].Value = "Low-High2"; m_pRangeSymbols[12].Label = "2nd order";
            m_pRangeSymbols[12].Heading = "L*=H*";

            m_pRangeSymbols[13].R = 186; m_pRangeSymbols[13].G = 228; m_pRangeSymbols[13].B = 179;
            m_pRangeSymbols[13].Value = "Low-High3"; m_pRangeSymbols[13].Label = "3rd order";
            m_pRangeSymbols[13].Heading = "L*=H*";

            m_pRangeSymbols[14].R = 237; m_pRangeSymbols[14].G = 248; m_pRangeSymbols[14].B = 233;
            m_pRangeSymbols[14].Value = "Low-High4"; m_pRangeSymbols[14].Label = "4th order";
            m_pRangeSymbols[14].Heading = "L*=H*";


            m_pRangeSymbols[15].R = 8; m_pRangeSymbols[15].G = 81; m_pRangeSymbols[15].B = 156;
            m_pRangeSymbols[15].Value = "Low-Low0"; m_pRangeSymbols[15].Label = "Zero order";
            m_pRangeSymbols[15].Heading = "L*=L*";

            m_pRangeSymbols[16].R = 49; m_pRangeSymbols[16].G = 130; m_pRangeSymbols[16].B = 189;
            m_pRangeSymbols[16].Value = "Low-Low1"; m_pRangeSymbols[16].Label = "1st order";
            m_pRangeSymbols[16].Heading = "L*=L*";

            m_pRangeSymbols[17].R = 107; m_pRangeSymbols[17].G = 174; m_pRangeSymbols[17].B = 214;
            m_pRangeSymbols[17].Value = "Low-Low2"; m_pRangeSymbols[17].Label = "2nd order";
            m_pRangeSymbols[17].Heading = "L*=L*";

            m_pRangeSymbols[18].R = 189; m_pRangeSymbols[18].G = 215; m_pRangeSymbols[18].B = 231;
            m_pRangeSymbols[18].Value = "Low-Low3"; m_pRangeSymbols[18].Label = "3rd order";
            m_pRangeSymbols[18].Heading = "L*=L*";

            m_pRangeSymbols[19].R = 239; m_pRangeSymbols[19].G = 243; m_pRangeSymbols[19].B = 255;
            m_pRangeSymbols[19].Value = "Low-Low4"; m_pRangeSymbols[19].Label = "4th order";
            m_pRangeSymbols[19].Heading = "L*=L*";
        }
        private void UpdateLabels()
        {
            m_pRangeSymbols[0].Heading = "H0=H0";
            
            m_pRangeSymbols[1].Heading = "H0=H0";
            
            m_pRangeSymbols[2].Heading = "H0=H0";
            
            m_pRangeSymbols[3].Heading = "H0=H0";
            
            m_pRangeSymbols[4].Heading = "H0=H0";

            
            m_pRangeSymbols[5].Heading = "H0=L0";
            
            m_pRangeSymbols[6].Heading = "H0=L0";
            
            m_pRangeSymbols[7].Heading = "H0=L0";
            
            m_pRangeSymbols[8].Heading = "H0=L0";
            
            m_pRangeSymbols[9].Heading = "H0=L0";

            
            m_pRangeSymbols[10].Heading = "L0=H0";
            
            m_pRangeSymbols[11].Heading = "L0=H0";
            
            m_pRangeSymbols[12].Heading = "L0=H0";
            
            m_pRangeSymbols[13].Heading = "L0=H0";
            
            m_pRangeSymbols[14].Heading = "L0=H0";

            
            m_pRangeSymbols[15].Heading = "L0=L0";
            
            m_pRangeSymbols[16].Heading = "L0=L0";
            
            m_pRangeSymbols[17].Heading = "L0=L0";
            
            m_pRangeSymbols[18].Heading = "L0=L0";
            
            m_pRangeSymbols[19].Heading = "L0=L0";
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

                string strOutputFldNM = "sp_range";
                string strOutput = "Spatial range";

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
        #endregion
        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboTargetLayer.Text;

                int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

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

                UpdateListview(lvFields, pFClass);
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
            // Creates the input and output matrices from the shapefile//
            string strLayerName = cboTargetLayer.Text;

            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFClass = pFLayer.FeatureClass;
            int nFeature = pFClass.FeatureCount(null);

            IFeatureCursor pFCursor = pFLayer.Search(null, true);
            IFeature pFeature = pFCursor.NextFeature();

            //Get index for independent and dependent variables
            //Get variable index            
            string strVarNM1 = (string)cboFldnm1.SelectedItem;
            string strVarNM2 = (string)cboFldnm2.SelectedItem;
            int intVarIdx1 = pFClass.FindField(strVarNM1);
            int intVarIdx2 = pFClass.FindField(strVarNM2);

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
            string strNameR = m_pSnippet.FilePathinRfromLayer(pFLayer);

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

            string strRSigLv = nudRsigLv.Value.ToString();
            string strLSigLv = nudLsigLv.Value.ToString();
            string strLSig = cboLocalL.Text;
            string strRsig = cboLocalPearson.Text;
            string strRowStd = cboRowStandardization.Text;
            string strMaxRanges = nudMaxRange.Value.ToString();
            string strHigherOrder = cboHigherOrder.Text;
            string strNonZero = null;
            if (chkDiagZero.Checked)
                strNonZero = "FALSE";
            else
                strNonZero = "TRUE";

            pEngine.Evaluate("sample.result <- LARRY.bivariate.spatial.range(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, maxsr = " + strMaxRanges
                +", pearson.sig = " + strRSigLv + ", lee.sig = " + strLSigLv + ", method.pearson = '" + strRsig + "', method.lee = '" + 
                strLSig + "', type.higher.nb = '"+ strHigherOrder+"', type.row.stand = '" + strRowStd + "', alternative = 'two', diag.zero = "
                + strNonZero + ")");

            string[] strSPRanges = pEngine.Evaluate("as.character(sample.result$final.max.sr)").AsCharacter().ToArray();

            //Save Output on SHP
            //Add Target fields to store results in the shapefile // Keep loop 
            for (int j = 0; j < 1; j++)
            {
                string strfldName = lvFields.Items[j].SubItems[1].Text;
                if (pFClass.FindField(strfldName) == -1)
                {
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strfldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pFClass.AddField(newField);
                }
            }

            //Update Field
            pFCursor = pFClass.Update(null, false);
            pFeature = pFCursor.NextFeature();

            string strSpRangeFldName = lvFields.Items[0].SubItems[1].Text;
            int intSpQuadFldIdx = pFClass.FindField(strSpRangeFldName);

            int featureIdx = 0;
            while (pFeature != null)
            {
                pFeature.set_Value(intSpQuadFldIdx, strSPRanges[featureIdx]);

                pFCursor.UpdateFeature(pFeature);

                pFeature = pFCursor.NextFeature();
                featureIdx++;
            }
            pFCursor.Flush();

            if (chkMap.Checked)
            {
                ITable pTable = (ITable)pFClass;

                IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
                pUniqueValueRenderer.FieldCount = 1;
                pUniqueValueRenderer.set_Field(0, strSpRangeFldName);

                ISimpleFillSymbol pSymbol;
                IQueryFilter pQFilter = new QueryFilterClass();
                int intTotalCount = 0;
                string strHeading = null;

                for (int j = 0; j < m_pRangeSymbols.Length; j++)
                {
                    pSymbol = new SimpleFillSymbolClass();
                    pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    pSymbol.Color = m_pSnippet.getRGB(m_pRangeSymbols[j].R, m_pRangeSymbols[j].G, m_pRangeSymbols[j].B);


                    if (j % 5 == 0)
                    {
                        intTotalCount = 0;
                        for (int k = 0; k < 5; k++)
                        {
                            pQFilter.WhereClause = strSpRangeFldName + " = '" + m_pRangeSymbols[j + k].Value + "'";
                            int intCnt = pTable.RowCount(pQFilter);
                            intTotalCount += intCnt;
                        }
                        
                        strHeading = m_pRangeSymbols[j].Heading + " (" + intTotalCount.ToString() + ")";
                        pUniqueValueRenderer.AddValue(m_pRangeSymbols[j].Value, strHeading, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pRangeSymbols[j].Value, m_pRangeSymbols[j].Label);
                    }
                    else
                    {
                        pUniqueValueRenderer.AddValue(m_pRangeSymbols[j].Value, strHeading, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pRangeSymbols[j].Value, m_pRangeSymbols[j].Label);
                    }


                }
                //string strNotSig = "not sig.";
                //pSymbol = new SimpleMarkerSymbolClass();
                //pSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle
                //pSymbol.Color = m_pSnippet.getRGB(255, 255, 255);

                //pQFilter.WhereClause = strFinalQuadFldName + " = '" + strNotSig + "'";
                //intTotalCount = pTable.RowCount(pQFilter);

                //pUniqueValueRenderer.AddValue(strNotSig, null, (ISymbol)pSymbol);
                //pUniqueValueRenderer.set_Label(strNotSig, "Not significant (" + intTotalCount.ToString() + ")");

                pUniqueValueRenderer.UseDefaultSymbol = false;

                IFeatureLayer pNewFLayer = new FeatureLayerClass();
                pNewFLayer.FeatureClass = pFClass;
                pNewFLayer.Name = "Bivariate Spatial Range";
                IGeoFeatureLayer pGFLayer = (IGeoFeatureLayer)pNewFLayer;
                pGFLayer.Renderer = (IFeatureRenderer)pUniqueValueRenderer;
                m_pActiveView.FocusMap.AddLayer(pGFLayer);
                m_pActiveView.Refresh();
                m_pForm.axTOCControl1.Update();
                pfrmProgress.Close();
            }
            else
                MessageBox.Show("Complete. The results are stored in the shape file");
        }

        private void chkDiagZero_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDiagZero.Checked)
                MakingSymbols();
            else
                UpdateLabels();
        }
    }
}
