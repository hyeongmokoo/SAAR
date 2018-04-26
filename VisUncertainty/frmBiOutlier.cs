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
    public partial class frmBiOutlier : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private BivariateOutlierSymbols[] m_pOutlierSymbols;

        public frmBiOutlier()
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
        private class BivariateOutlierSymbols
        {
            public int R; public int G; public int B;
            public string Label;
            public string Value;
            public double Angle;
        }
        private void MakingSymbols()
        {
            m_pOutlierSymbols = new BivariateOutlierSymbols[4];

            for (int i = 0; i < 4; i++)
                m_pOutlierSymbols[i] = new BivariateOutlierSymbols();

            m_pOutlierSymbols[0].R = 203; m_pOutlierSymbols[0].G = 24; m_pOutlierSymbols[0].B = 29;
            m_pOutlierSymbols[0].Label = "H*=H*"; m_pOutlierSymbols[0].Value = "High-High";
            m_pOutlierSymbols[0].Angle = 270;

            m_pOutlierSymbols[1].R = 106; m_pOutlierSymbols[1].G = 81; m_pOutlierSymbols[1].B = 163;
            m_pOutlierSymbols[1].Label = "H*=L*"; m_pOutlierSymbols[1].Value = "High-Low";
            m_pOutlierSymbols[1].Angle = 180;

            m_pOutlierSymbols[2].R = 35; m_pOutlierSymbols[2].G = 139; m_pOutlierSymbols[2].B = 69;
            m_pOutlierSymbols[2].Label = "L*=H*"; m_pOutlierSymbols[2].Value = "Low-High";
            m_pOutlierSymbols[2].Angle = 0;

            m_pOutlierSymbols[3].R = 33; m_pOutlierSymbols[3].G = 113; m_pOutlierSymbols[3].B = 181;
            m_pOutlierSymbols[3].Label = "L*=L*"; m_pOutlierSymbols[3].Value = "Low-Low";
            m_pOutlierSymbols[3].Angle = 90;
        }

        private void UpdateLables()
        {

            m_pOutlierSymbols[0].Label = "H0=H0"; 

           m_pOutlierSymbols[1].Label = "H0=L0";

            m_pOutlierSymbols[2].Label = "L0=H0"; 

            m_pOutlierSymbols[3].Label = "L0=L0"; 
        }
        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass)
        {
            try
            {
                pListView.Items.Clear();

                pListView.BeginUpdate();

                int intNFlds =1;
                string[] strFldNames = null;
                string[] strLvNames = null;

                string strOutputFldNM = "sp_out";
                string strOutput = "Spatial Outlier";

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
            string strNonZero = null;
            if (chkDiagZero.Checked)
                strNonZero = "FALSE";
            else
                strNonZero = "TRUE";

            pEngine.Evaluate("sample.result <- LARRY.bivariate.spatial.outlier(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, pearson.sig = " +
                strRSigLv + ", lee.sig = "+strLSigLv + ", method.pearson = '" + strRsig + "', method.lee = '" +
                strLSig + "', type.row.stand = '" + strRowStd + "', alternative = 'two', diag.zero = "
                + strNonZero + ")");

            string[] strSPOutliers = pEngine.Evaluate("as.character(sample.result$sp.outlier)").AsCharacter().ToArray();

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

            string strSpOutlierFldName = lvFields.Items[0].SubItems[1].Text;
            int intSpOutlierFldIdx = pFClass.FindField(strSpOutlierFldName);

            int featureIdx = 0;
            while (pFeature != null)
            {
                pFeature.set_Value(intSpOutlierFldIdx, strSPOutliers[featureIdx]);

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
                pUniqueValueRenderer.set_Field(0, strSpOutlierFldName);
                if (cboMaptype.Text == "choropleth map")
                {

                    ISimpleFillSymbol pSymbol;
                    IQueryFilter pQFilter = new QueryFilterClass();
                    int intTotalCount = 0;
                    string strLabel = null;

                    for (int j = 0; j < 4; j++)
                    {
                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(m_pOutlierSymbols[j].R, m_pOutlierSymbols[j].G, m_pOutlierSymbols[j].B);

                        pQFilter.WhereClause = strSpOutlierFldName + " = '" + m_pOutlierSymbols[j].Value + "'";

                        intTotalCount = pTable.RowCount(pQFilter);

                        strLabel = m_pOutlierSymbols[j].Label + " (" + intTotalCount.ToString() + ")";
                        pUniqueValueRenderer.AddValue(m_pOutlierSymbols[j].Value, null, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pOutlierSymbols[j].Value, strLabel);
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
                        pSymbol = new CharacterMarkerSymbolClass();
                        pSymbol.CharacterIndex = 248;
                        //pSymbol.Color = m_pSnippet.getRGB(m_pQuadrantSymbols[j].R, m_pQuadrantSymbols[j].G, m_pQuadrantSymbols[j].B);
                        pSymbol.Size = 10;
                        pSymbol.Font = stdFontCls;

                        pSymbol.Angle = m_pOutlierSymbols[j].Angle;

                        pQFilter.WhereClause = strSpOutlierFldName + " = '" + m_pOutlierSymbols[j].Value + "'";

                        intTotalCount = pTable.RowCount(pQFilter);

                        strLabel = m_pOutlierSymbols[j].Label + " (" + intTotalCount.ToString() + ")";
                        pUniqueValueRenderer.AddValue(m_pOutlierSymbols[j].Value, null, (ISymbol)pSymbol);
                        pUniqueValueRenderer.set_Label(m_pOutlierSymbols[j].Value, strLabel);
                    }
                    pUniqueValueRenderer.UseDefaultSymbol = false;
                }
                IFeatureLayer pNewFLayer = new FeatureLayerClass();
                pNewFLayer.FeatureClass = pFClass;
                    pNewFLayer.Name = "Bivariate Spatial Quadrants";
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
                UpdateLables();
        }
    }
}
