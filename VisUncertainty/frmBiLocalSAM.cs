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
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using RDotNet;
using ESRI.ArcGIS.esriSystem;
//spdep, and maptools packages in R are required

namespace VisUncertainty
{
    public partial class frmBiLocalSAM : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;

        public frmBiLocalSAM()
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
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

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
                string strPAdjust = cboAdjustment.Text;
                string strMethod = cboSAM.Text;

                UpdateListview(lvFields, pFClass, strMethod);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void UpdateListview(ListView pListView, IFeatureClass pFeatureClass, string strMethod)
        {
            try
            {

                pListView.Items.Clear();

                pListView.BeginUpdate();

                int intNFlds = 0;
                string[] strFldNames = null;
                string[] strLvNames = null;

                if (strMethod == "Lee's L")
                {
                    intNFlds = 3;
                    string strStatisticFldNM = "Li";
                    string strStatistic = "Statistic";
                    string strPrFldNM = "Pr";
                    string strPr = "p-Value";
                    string strFlgFldNM = "flg";
                    string strFlg = "FLAG";

                    if (strStatisticFldNM == null || strPrFldNM == null || strFlgFldNM==null)
                        return;

                    strFldNames = new string[intNFlds];
                    strFldNames[0] = strStatisticFldNM;
                    strFldNames[1] = strPrFldNM;
                    strFldNames[2] = strFlgFldNM;

                    strLvNames = new string[intNFlds];
                    strLvNames[0] = strStatistic;
                    strLvNames[1] = strPr;
                    strLvNames[2] = strFlg;
                }


                //Update Name Using the UpdateFldNames Function to Update Name with the same number


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
        //private string UpdateFldName(string strFldNM, IFeatureClass pFeatureClass)
        //{
        //    try
        //    {
        //        string returnNM = strFldNM;
        //        int i = 1;
        //        while (pFeatureClass.FindField(returnNM) != -1)
        //        {
        //            returnNM = strFldNM + "_" + i.ToString();
        //            i++;
        //        }
        //        return returnNM;
        //    }
        //    catch (Exception ex)
        //    {
        //        frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
        //        return null;
        //    }
        //}
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
            try
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

                //Plot command for R
                StringBuilder plotCommmand = new StringBuilder();

                string strStartPath = m_pForm.strPath;
                string pathr = strStartPath.Replace(@"\", @"/");
                pEngine.Evaluate("source('" + pathr + "/AllFunctions_LeeL.R')");

                //Get the file path and name to create spatial weight matrix
                string strNameR = m_pSnippet.FilePathinRfromLayer(pFLayer);

                if (strNameR == null)
                    return;

                //Create spatial weight matrix in R
                pEngine.Evaluate("library(spdep); library(maptools)");
                pEngine.Evaluate("sample.shp <- readShapePoly('" + strNameR + "')");

                if (txtSWM.Text == "Default")
                {
                    pEngine.Evaluate("sample.nb <- poly2nb(sample.shp)");
                }
                else
                {
                    int intResult = m_pSnippet.SWMusingGAL(pEngine, pFClass, txtSWM.Text);
                    if (intResult == -1)
                        return;
                }

                NumericVector vecVar1 = pEngine.CreateNumericVector(arrVar1);
                pEngine.SetSymbol("sample.v1", vecVar1);
                NumericVector vecVar2 = pEngine.CreateNumericVector(arrVar2);
                pEngine.SetSymbol("sample.v2", vecVar2);

                string strSigLv = nudSigLv.Value.ToString();
                string strSigMethod = cboSigMethod.Text;

                if (cboSAM.Text == "Lee's L")
                {
                    #region Li
                    pEngine.Evaluate("sample.l <- L.local.test(sample.v1, sample.v2, 1:length(sample.nb), sample.nb, style='W', sig.levels=c(" + strSigLv + "), method='" + strSigMethod + "', alternative='two.sided', diag.zero=FALSE)");

                    double[] dblLValues = pEngine.Evaluate("sample.l$local.L").AsNumeric().ToArray();
                    double[] dblPvalues = pEngine.Evaluate("sample.l$pvalue").AsNumeric().ToArray();
                    string[] strFlgs = pEngine.Evaluate("as.character(sample.l$sig)").AsCharacter().ToArray();

                    //Save Output on SHP
                    //Add Target fields to store results in the shapefile
                    for (int j = 0; j < 3; j++)
                    {
                        IField newField = new FieldClass();
                        IFieldEdit fieldEdit = (IFieldEdit)newField;
                        fieldEdit.Name_2 = lvFields.Items[j].SubItems[1].Text;
                        if (j == 2)
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        else
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        pFClass.AddField(newField);
                    }

                    //Update Field
                    pFCursor = pFClass.Update(null, false);
                    pFeature = pFCursor.NextFeature();

                    string strFlgFldNam = lvFields.Items[2].SubItems[1].Text;
                    int intStatFldIdx = pFClass.FindField(lvFields.Items[0].SubItems[1].Text);
                    int intPrFldIdx = pFClass.FindField(lvFields.Items[1].SubItems[1].Text);
                    int intFlgFldIdx = pFClass.FindField(strFlgFldNam);

                    int featureIdx = 0;
                    while (pFeature != null)
                    {
                        pFeature.set_Value(intStatFldIdx, dblLValues[featureIdx]);
                        pFeature.set_Value(intPrFldIdx, dblPvalues[featureIdx]);
                        pFeature.set_Value(intFlgFldIdx, strFlgs[featureIdx]);

                        pFCursor.UpdateFeature(pFeature);

                        pFeature = pFCursor.NextFeature();
                        featureIdx++;
                    }
                    pFCursor.Flush();

                    pfrmProgress.Close();

                    if (chkMap.Checked)
                    {
                        double[,] adblMinMaxForLabel = new double[2, 4];
                        ITable pTable = (ITable)pFClass;

                        IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
                        pUniqueValueRenderer.FieldCount = 1;
                        pUniqueValueRenderer.set_Field(0, strFlgFldNam);
                        IDataStatistics pDataStat;
                        IStatisticsResults pStatResults;
                        ISimpleFillSymbol pSymbol;
                        ICursor pCursor;

                        IQueryFilter pQFilter = new QueryFilterClass();
                        pQFilter.WhereClause = strFlgFldNam + " = 'HH'";

                        int intCnt = pTable.RowCount(pQFilter);

                        pCursor = pTable.Search(pQFilter, true);
                        pDataStat = new DataStatisticsClass();
                        pDataStat.Field = lvFields.Items[1].SubItems[1].Text;
                        pDataStat.Cursor = pCursor;
                        pStatResults = pDataStat.Statistics;
                        adblMinMaxForLabel[0, 0] = pStatResults.Minimum;
                        adblMinMaxForLabel[1, 0] = pStatResults.Maximum;
                        pCursor.Flush();

                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(255, 80, 80);
                        pUniqueValueRenderer.AddValue("HH", null, (ISymbol)pSymbol);
                        if (intCnt == 1)
                            pUniqueValueRenderer.set_Label("HH", "HH (" + adblMinMaxForLabel[0, 0].ToString("N1") + ")");
                        else if (intCnt == 0)
                            pUniqueValueRenderer.set_Label("HH", "HH (no obs)");
                        else
                            pUniqueValueRenderer.set_Label("HH", "HH (" + adblMinMaxForLabel[0, 0].ToString("N1") + "-" + adblMinMaxForLabel[1, 0].ToString("N1") + ")");


                        pQFilter.WhereClause = strFlgFldNam + " = 'LL'";
                        intCnt = pTable.RowCount(pQFilter);


                        pCursor = pTable.Search(pQFilter, true);
                        pDataStat = new DataStatisticsClass();
                        pDataStat.Field = lvFields.Items[1].SubItems[1].Text;
                        pDataStat.Cursor = pCursor;
                        pStatResults = pDataStat.Statistics;
                        adblMinMaxForLabel[0, 1] = pStatResults.Minimum;
                        adblMinMaxForLabel[1, 1] = pStatResults.Maximum;
                        pCursor.Flush();

                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(50, 157, 194);
                        pUniqueValueRenderer.AddValue("LL", null, (ISymbol)pSymbol);

                        if (intCnt == 1)
                            pUniqueValueRenderer.set_Label("LL", "LL (" + adblMinMaxForLabel[0, 1].ToString("N1") + ")");
                        else if (intCnt == 0)
                            pUniqueValueRenderer.set_Label("LL", "LL (no obs)");
                        else
                            pUniqueValueRenderer.set_Label("LL", "LL (" + adblMinMaxForLabel[0, 1].ToString("N1") + "-" + adblMinMaxForLabel[1, 1].ToString("N1") + ")");


                        pQFilter.WhereClause = strFlgFldNam + " = 'HL'";
                        intCnt = pTable.RowCount(pQFilter);


                        pCursor = pTable.Search(pQFilter, true);
                        pDataStat = new DataStatisticsClass();
                        pDataStat.Field = lvFields.Items[1].SubItems[1].Text;
                        pDataStat.Cursor = pCursor;
                        pStatResults = pDataStat.Statistics;
                        adblMinMaxForLabel[0, 2] = pStatResults.Minimum;
                        adblMinMaxForLabel[1, 2] = pStatResults.Maximum;
                        pCursor.Flush();

                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(244, 199, 0);
                        pUniqueValueRenderer.AddValue("HL", null, (ISymbol)pSymbol);

                        if (intCnt == 1)
                            pUniqueValueRenderer.set_Label("HL", "HL (" + adblMinMaxForLabel[0, 2].ToString("N1") + ")");
                        else if (intCnt == 0)
                            pUniqueValueRenderer.set_Label("HL", "HL (no obs)");
                        else
                            pUniqueValueRenderer.set_Label("HL", "HL (" + adblMinMaxForLabel[0, 2].ToString("N1") + "-" + adblMinMaxForLabel[1, 2].ToString("N1") + ")");


                        pQFilter.WhereClause = strFlgFldNam + " = 'LH'";
                        intCnt = pTable.RowCount(pQFilter);

                        pCursor = pTable.Search(pQFilter, true);
                        pDataStat = new DataStatisticsClass();
                        pDataStat.Field = lvFields.Items[1].SubItems[1].Text;
                        pDataStat.Cursor = pCursor;
                        pStatResults = pDataStat.Statistics;
                        adblMinMaxForLabel[0, 3] = pStatResults.Minimum;
                        adblMinMaxForLabel[1, 3] = pStatResults.Maximum;
                        pCursor.Flush();

                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(173, 255, 179);
                        pUniqueValueRenderer.AddValue("LH", null, (ISymbol)pSymbol);

                        if (intCnt == 1)
                            pUniqueValueRenderer.set_Label("LH", "LH (" + adblMinMaxForLabel[0, 3].ToString("N1") + ")");
                        else if (intCnt == 0)
                            pUniqueValueRenderer.set_Label("LH", "LH (no obs)");
                        else
                            pUniqueValueRenderer.set_Label("LH", "LH (" + adblMinMaxForLabel[0, 3].ToString("N1") + "-" + adblMinMaxForLabel[1, 3].ToString("N1") + ")");




                        pSymbol = new SimpleFillSymbolClass();
                        pSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                        pSymbol.Color = m_pSnippet.getRGB(200, 200, 200);
                        //pUniqueValueRenderer.AddValue("", strFlgFldNam, (ISymbol)pSymbol);
                        //pUniqueValueRenderer.set_Label("", "Not significant");
                        pUniqueValueRenderer.DefaultSymbol = (ISymbol)pSymbol;
                        pUniqueValueRenderer.DefaultLabel = "Not significant";

                        pUniqueValueRenderer.UseDefaultSymbol = true;

                        IFeatureLayer pNewFLayer = new FeatureLayerClass();
                        pNewFLayer.FeatureClass = pFClass;
                        pNewFLayer.Name = cboSAM.Text + " of " + pFLayer.Name;
                        IGeoFeatureLayer pGFLayer = (IGeoFeatureLayer)pNewFLayer;
                        pGFLayer.Renderer = (IFeatureRenderer)pUniqueValueRenderer;
                        m_pActiveView.FocusMap.AddLayer(pGFLayer);
                        m_pActiveView.Refresh();
                        m_pForm.axTOCControl1.Update();
                    }
                    else
                        MessageBox.Show("Complete. The results are stored in the shape file");
                    #endregion

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
