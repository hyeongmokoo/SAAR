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
    public partial class frmBiCluster : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        private BivariateClusterSymbols[] m_pClusterSymbols;

        public frmBiCluster()
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
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass)
        {
            try
            {
                pListView.Items.Clear();

                pListView.BeginUpdate();

                int intNFlds = 5;
                string[] strFldNames = null;
                string[] strLvNames = null;

                string strGlobalFldNM = "GL";
                string strGlobal = "Global";

                string strFDRFldNM = "FD";
                string strFDR = "FDR";

                string strSpatialBonfFldNM = "SB";
                string strSptialBonf = "Spatial Bonf.";

                string strGenBonfFldNM = "GB";
                string strGenBonf = "Gen. Bof.";

                string strFinalQuadFldNM = "clu_quad";
                string strFinalQuad = "Final Quad.";

                if (strGlobalFldNM == null || strFDRFldNM == null || strSpatialBonfFldNM == null|| strGenBonfFldNM==null|| strFinalQuadFldNM==null)
                    return;

                strFldNames = new string[intNFlds];
                strFldNames[0] = strGlobalFldNM;
                strFldNames[1] = strFDRFldNM;
                strFldNames[2] = strSpatialBonfFldNM;
                strFldNames[3] = strGenBonfFldNM;
                strFldNames[4] = strFinalQuadFldNM;

                strLvNames = new string[intNFlds];
                strLvNames[0] = strGlobal;
                strLvNames[1] = strFDR;
                strLvNames[2] = strSptialBonf;
                strLvNames[3] = strGenBonf;
                strLvNames[4] = strFinalQuad;

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
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
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
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
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
            pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)");

            NumericVector vecVar1 = pEngine.CreateNumericVector(arrVar1);
            pEngine.SetSymbol("sample.v1", vecVar1);
            NumericVector vecVar2 = pEngine.CreateNumericVector(arrVar2);
            pEngine.SetSymbol("sample.v2", vecVar2);

            string strSigLv = nudSigLv.Value.ToString();
            string strLSig = cboLocalL.Text;
            string strRsig = cboLocalPearson.Text;
            string strRowStd = cboRowStandardization.Text;
            string strNonZero = null;
            if (chkDiagZero.Checked)
                strNonZero = "FALSE";
            else
                strNonZero = "TRUE";

            pEngine.Evaluate("sample.result <- LARRY.bivariate.spatial.cluster(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, global.sig = " + 
                strSigLv + ", method = '" + strLSig + "', type.row.stand = '" + strRowStd + "', alternative = 'two', diag.zero = " + strNonZero + ")");

            string[] strGlobal = pEngine.Evaluate("as.character(sample.result[[1]]$sig.global)").AsCharacter().ToArray();
            string[] strFDR = pEngine.Evaluate("as.character(sample.result[[1]]$sig.FDR)").AsCharacter().ToArray();
            string[] strSpBonf = pEngine.Evaluate("as.character(sample.result[[1]]$sig.spBonf)").AsCharacter().ToArray();
            string[] strBonf = pEngine.Evaluate("as.character(sample.result[[1]]$sig.genBonf)").AsCharacter().ToArray();
            string[] strQuad = pEngine.Evaluate("as.character(sample.result[[1]]$final.quad)").AsCharacter().ToArray();

            string[] strSigLevels = pEngine.Evaluate("as.character(round(sample.result[[2]],6))").AsCharacter().ToArray();
            //Save Output on SHP
            //Add Target fields to store results in the shapefile
            for (int j = 0; j < 5; j++)
            {
                string strfldName = lvFields.Items[j].SubItems[1].Text;
                if(pFClass.FindField(strfldName)==-1)
                {
                    IField newField = new FieldClass();
                    IFieldEdit fieldEdit = (IFieldEdit)newField;
                    fieldEdit.Name_2 = strfldName;
                    fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    pFClass.AddField(newField);

                    //MessageBox.Show("Same field name exists.", "Same field name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //pfrmProgress.Close();
                    //return;
                }
            }

            //Update Field
            pFCursor = pFClass.Update(null, false);
            pFeature = pFCursor.NextFeature();

            string strFinalQuadFldName = lvFields.Items[4].SubItems[1].Text;
            int intGlobalFldIdx = pFClass.FindField(lvFields.Items[0].SubItems[1].Text);
            int intFDRFldIdx = pFClass.FindField(lvFields.Items[1].SubItems[1].Text);
            int intSpatialBonfFldIdx = pFClass.FindField(lvFields.Items[2].SubItems[1].Text);
            int intGenBonfFldIdx = pFClass.FindField(lvFields.Items[3].SubItems[1].Text);
            int intFinalQuadFldIdx = pFClass.FindField(strFinalQuadFldName);
            
            int featureIdx = 0;
            while (pFeature != null)
            {
                pFeature.set_Value(intGlobalFldIdx, strGlobal[featureIdx]);
                pFeature.set_Value(intFDRFldIdx, strFDR[featureIdx]);
                pFeature.set_Value(intSpatialBonfFldIdx, strSpBonf[featureIdx]);
                pFeature.set_Value(intGenBonfFldIdx, strBonf[featureIdx]);
                pFeature.set_Value(intFinalQuadFldIdx, strQuad[featureIdx]);

                pFCursor.UpdateFeature(pFeature);

                pFeature = pFCursor.NextFeature();
                featureIdx++;
            }
            pFCursor.Flush();

            if (chkMap.Checked)
            {
                double[,] adblMinMaxForLabel = new double[2, 4];
                ITable pTable = (ITable)pFClass;

                IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
                pUniqueValueRenderer.FieldCount = 1;
                pUniqueValueRenderer.set_Field(0, strFinalQuadFldName);
                ISimpleFillSymbol pSymbol;
                IQueryFilter pQFilter = new QueryFilterClass();
                int intTotalCount = 0;
                string strHeading = null;
                int intSigIdx = 0;

                for (int j = 0; j < 16; j++)
                {
                    pSymbol = new SimpleFillSymbolClass();
                    pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    pSymbol.Color = m_pSnippet.getRGB(m_pClusterSymbols[j].R, m_pClusterSymbols[j].G, m_pClusterSymbols[j].B);


                    if (j%4==0)
                    {
                        intTotalCount = 0;
                        for (int k = 0; k < 4; k++)
                        {
                            pQFilter.WhereClause = strFinalQuadFldName + " = '"+m_pClusterSymbols[j+k].Value+"'";
                            int intCnt = pTable.RowCount(pQFilter);
                            intTotalCount += intCnt;
                        }

                        intSigIdx = 3;

                        strHeading = m_pClusterSymbols[j].Heading + " (" + intTotalCount.ToString() + ")";
                        pUniqueValueRenderer.AddValue(m_pClusterSymbols[j].Value, strHeading, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pClusterSymbols[j].Value, m_pClusterSymbols[j].Label+"("+strSigLevels[intSigIdx]+")");
                    }
                    else
                    {
                        intSigIdx--;
                        pUniqueValueRenderer.AddValue(m_pClusterSymbols[j].Value, strHeading, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pClusterSymbols[j].Value, m_pClusterSymbols[j].Label + "(" + strSigLevels[intSigIdx] + ")");
                    }
                    

                }

                string strNotSig = "not sig.";
                pSymbol = new SimpleFillSymbolClass();
                pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                pSymbol.Color = m_pSnippet.getRGB(255, 255, 255);

                pQFilter.WhereClause = strFinalQuadFldName + " = '"+ strNotSig+"'";
                intTotalCount = pTable.RowCount(pQFilter);

                pUniqueValueRenderer.AddValue(strNotSig, null, (ISymbol)pSymbol);
                pUniqueValueRenderer.set_Label(strNotSig, "Not significant (" + intTotalCount.ToString() + ")");

                pUniqueValueRenderer.UseDefaultSymbol = false;

                IFeatureLayer pNewFLayer = new FeatureLayerClass();
                pNewFLayer.FeatureClass = pFClass;
                pNewFLayer.Name = "Bivariate Spatial Clusters";
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

        private class BivariateClusterSymbols
        {
            public int R; public int G; public int B;
            public string Value;
            public string Label;
            public string Heading;
        }

        private void MakingSymbols()
        {
            m_pClusterSymbols = new BivariateClusterSymbols[16];

            for(int i=0; i<16; i++)
                m_pClusterSymbols[i] = new BivariateClusterSymbols();

            m_pClusterSymbols[0].R = 203; m_pClusterSymbols[0].G = 24; m_pClusterSymbols[0].B = 29;
            m_pClusterSymbols[0].Value = "High-High1"; m_pClusterSymbols[0].Label = "Gen. Bonf.";
            m_pClusterSymbols[0].Heading = "H*=H*";

            m_pClusterSymbols[1].R = 251; m_pClusterSymbols[1].G = 106; m_pClusterSymbols[1].B = 74;
            m_pClusterSymbols[1].Value = "High-High2"; m_pClusterSymbols[1].Label = "Spatial Bonf.";
            m_pClusterSymbols[1].Heading = "H*=H*";

            m_pClusterSymbols[2].R = 252; m_pClusterSymbols[2].G = 174; m_pClusterSymbols[2].B = 145;
            m_pClusterSymbols[2].Value = "High-High3"; m_pClusterSymbols[2].Label = "FDR";
            m_pClusterSymbols[2].Heading = "H*=H*";

            m_pClusterSymbols[3].R = 254; m_pClusterSymbols[3].G = 229; m_pClusterSymbols[3].B = 217;
            m_pClusterSymbols[3].Value = "High-High4"; m_pClusterSymbols[3].Label = "Global";
            m_pClusterSymbols[3].Heading = "H*=H*";


            m_pClusterSymbols[4].R = 106; m_pClusterSymbols[4].G = 81; m_pClusterSymbols[4].B = 163;
            m_pClusterSymbols[4].Value = "High-Low1"; m_pClusterSymbols[4].Label = "Gen. Bonf.";
            m_pClusterSymbols[4].Heading = "H*=L*";

            m_pClusterSymbols[5].R = 158; m_pClusterSymbols[5].G = 154; m_pClusterSymbols[5].B = 200;
            m_pClusterSymbols[5].Value = "High-Low2"; m_pClusterSymbols[5].Label = "Spatial Bonf.";
            m_pClusterSymbols[5].Heading = "H*=L*";

            m_pClusterSymbols[6].R = 203; m_pClusterSymbols[6].G = 201; m_pClusterSymbols[6].B = 226;
            m_pClusterSymbols[6].Value = "High-Low3"; m_pClusterSymbols[6].Label = "FDR";
            m_pClusterSymbols[6].Heading = "H*=L*";

            m_pClusterSymbols[7].R = 242; m_pClusterSymbols[7].G = 240; m_pClusterSymbols[7].B = 247;
            m_pClusterSymbols[7].Value = "High-Low4"; m_pClusterSymbols[7].Label = "Global";
            m_pClusterSymbols[7].Heading = "H*=L*";

            m_pClusterSymbols[8].R = 35; m_pClusterSymbols[8].G = 139; m_pClusterSymbols[8].B = 69;
            m_pClusterSymbols[8].Value = "Low-High1"; m_pClusterSymbols[8].Label = "Gen. Bonf.";
            m_pClusterSymbols[8].Heading = "L*=H*";

            m_pClusterSymbols[9].R = 116; m_pClusterSymbols[9].G = 196; m_pClusterSymbols[9].B = 118;
            m_pClusterSymbols[9].Value = "Low-High2"; m_pClusterSymbols[9].Label = "Spatial Bonf.";
            m_pClusterSymbols[9].Heading = "L*=H*";

            m_pClusterSymbols[10].R = 186; m_pClusterSymbols[10].G = 228; m_pClusterSymbols[10].B = 179;
            m_pClusterSymbols[10].Value = "Low-High3"; m_pClusterSymbols[10].Label = "FDR";
            m_pClusterSymbols[10].Heading = "L*=H*";

            m_pClusterSymbols[11].R = 237; m_pClusterSymbols[11].G = 248; m_pClusterSymbols[11].B = 233;
            m_pClusterSymbols[11].Value = "Low-High4"; m_pClusterSymbols[11].Label = "Global";
            m_pClusterSymbols[11].Heading = "L*=H*";

            m_pClusterSymbols[12].R = 33; m_pClusterSymbols[12].G = 113; m_pClusterSymbols[12].B = 181;
            m_pClusterSymbols[12].Value = "Low-Low1"; m_pClusterSymbols[12].Label = "Gen. Bonf.";
            m_pClusterSymbols[12].Heading = "L*=L*";

            m_pClusterSymbols[13].R = 107; m_pClusterSymbols[13].G = 174; m_pClusterSymbols[13].B = 214;
            m_pClusterSymbols[13].Value = "Low-Low2"; m_pClusterSymbols[13].Label = "Spatial Bonf.";
            m_pClusterSymbols[13].Heading = "L*=L*";

            m_pClusterSymbols[14].R = 189; m_pClusterSymbols[14].G = 215; m_pClusterSymbols[14].B = 231;
            m_pClusterSymbols[14].Value = "Low-Low3"; m_pClusterSymbols[14].Label = "FDR";
            m_pClusterSymbols[14].Heading = "L*=L*";

            m_pClusterSymbols[15].R = 239; m_pClusterSymbols[15].G = 243; m_pClusterSymbols[15].B = 255;
            m_pClusterSymbols[15].Value = "Low-Low4"; m_pClusterSymbols[15].Label = "Global";
            m_pClusterSymbols[15].Heading = "L*=L*";
        }

        private void UpdateLables()
        {

            m_pClusterSymbols[0].Heading = "H0=H0";

            m_pClusterSymbols[1].Heading = "H0=H0";
            
            m_pClusterSymbols[2].Heading = "H0=H0";
            
            m_pClusterSymbols[3].Heading = "H0=H0";


            m_pClusterSymbols[4].Heading = "H0=L0";

            m_pClusterSymbols[5].Heading = "H0=L0";

            m_pClusterSymbols[6].Heading = "H0=L0";

            m_pClusterSymbols[7].Heading = "H0=L0";

            m_pClusterSymbols[8].Heading = "L0=H0";

            m_pClusterSymbols[9].Heading = "L0=H0";

            m_pClusterSymbols[10].Heading = "L0=H0";

            m_pClusterSymbols[11].Heading = "L0=H0";

           m_pClusterSymbols[12].Heading = "L0=L0";

            m_pClusterSymbols[13].Heading = "L0=L0";

            m_pClusterSymbols[14].Heading = "L0=L0";

           m_pClusterSymbols[15].Heading = "L0=L0";
        }

        private void chkDiagZero_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDiagZero.Checked)
                MakingSymbols();
            else
                UpdateLables();
         
        }
    }
}
