using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

using RDotNet;
using ESRI.ArcGIS.esriSystem;

//using Accord.Math;
//spdep, and maptools packages in R are required

namespace VisUncertainty
{
    public partial class frmLocalSAM : Form
    {
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private IFeatureLayer m_pFLayer;
        private IFeatureClass m_pFClass;
        private REngine m_pEngine;
        //Varaibles for SWM
        private bool m_blnCreateSWM = false;


        public frmLocalSAM()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
                m_pEngine = m_pForm.pEngine;

                for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                {
                    //IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pActiveView.FocusMap.get_Layer(i);

                    //if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Only polygon to make spatial weight matrix 10/9/15 HK
                    //    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                    cboTargetLayer.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name); //It supports all types, but visualizaiton for point data is under developing. 080317 HK
                }

                m_pSnippet = new clsSnippet();
                m_pEngine.Evaluate("rm(list=ls(all=TRUE))");
                m_pEngine.Evaluate("library(spdep); library(maptools)");
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

                //New Spatial Weight matrix function 080317
                clsSnippet.SpatialWeightMatrixType pSWMType = new clsSnippet.SpatialWeightMatrixType();
                if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon) //Apply Different Spatial weight matrix according to geometry type 07052017 HK
                    txtSWM.Text = pSWMType.strPolySWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    txtSWM.Text = pSWMType.strPointSWM;
                else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    MessageBox.Show("Spatial weight matrix for polyline is not supported.");
                //

                IFields fields = m_pFClass.Fields;

                cboFieldName.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        cboFieldName.Items.Add(fields.get_Field(i).Name);
                }
                string strPAdjust = cboAdjustment.Text;
                string strMethod = cboSAM.Text;
                
                UpdateListview(lvFields, m_pFClass, strMethod);
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

                if (strMethod == "Local Moran")
                {
                    intNFlds = 4;
                    string strStatisticFldNM = "Ii";
                    string strStatistic = "Statistic";
                    string strSDFldNM = "Zi";
                    string strSD = "Standard deviate";
                    string strPrFldNM = "Pr";
                    string strPr = "p-Value";
                    string strFlgFldNM = "flg";
                    string strFlg = "FLAG";

                    if (strStatisticFldNM == null || strSDFldNM == null || strPrFldNM == null || strFlgFldNM == null)
                        return;

                    strFldNames = new string[intNFlds];
                    strFldNames[0] = strStatisticFldNM; strFldNames[1] = strSDFldNM;
                    strFldNames[2] = strPrFldNM; strFldNames[3] = strFlgFldNM;

                    strLvNames = new string[intNFlds];
                    strLvNames[0] = strStatistic; strLvNames[1] = strSD;
                    strLvNames[2] = strPr; strLvNames[3] = strFlg;

                }
                else if (strMethod == "Gi*")
                {
                    intNFlds = 2;
                    string strStatisticFldNM = "Gi";
                    string strStatistic = "Statistic";
                    string strPrFldNM = "Pr";
                    string strPr = "p-Value";

                    if (strStatisticFldNM == null || strPrFldNM == null )
                        return;

                    strFldNames = new string[intNFlds];
                    strFldNames[0] = strStatisticFldNM; 
                    strFldNames[1] = strPrFldNM;

                    strLvNames = new string[intNFlds];
                    strLvNames[0] = strStatistic; 
                    strLvNames[1] = strPr; 

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
                        strNM = strFldNMs[j] +"_" + i.ToString();
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
                        strReturnNMs[j] = strFldNMs[j] + "_" + (intMax-1).ToString();
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
                //Checking
                if (cboFieldName.Text == "")
                {
                    MessageBox.Show("Please select target field");
                    return;
                }

                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Processing:";
                pfrmProgress.pgbProgress.Style = ProgressBarStyle.Marquee;
                pfrmProgress.Show();

                int nFeature = m_pFClass.FeatureCount(null);

                IFeatureCursor pFCursor = m_pFLayer.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Get index for independent and dependent variables
                //Get variable index            
                string strVarNM = (string)cboFieldName.SelectedItem;
                int intVarIdx = m_pFClass.FindField(strVarNM);

                //Store Variable at Array
                double[] arrVar = new double[nFeature];

                int i = 0;

                while (pFeature != null)
                {
                    arrVar[i] = Convert.ToDouble(pFeature.get_Value(intVarIdx));
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
                
                NumericVector vecVar = m_pEngine.CreateNumericVector(arrVar);
                m_pEngine.SetSymbol(strVarNM, vecVar);

                if (cboSAM.Text == "Local Moran")
                {
                    #region Local Moran
                    plotCommmand.Append("localmoran(" + strVarNM + ", sample.listw, alternative = 'two.sided', ");

                    //select multiple correction method (only Bonferroni.. 100915 HK)
                    if (cboAdjustment.Text == "None")
                        plotCommmand.Append(", zero.policy=TRUE)");
                    else if (cboAdjustment.Text == "Bonferroni correction")
                        plotCommmand.Append("p.adjust.method='bonferroni', zero.policy=TRUE)");

                    NumericMatrix nmResults = m_pEngine.Evaluate(plotCommmand.ToString()).AsNumericMatrix();

                    string strFlgFldNam = lvFields.Items[3].SubItems[1].Text;
                    //Save Output on SHP
                    //Add Target fields to store results in the shapefile
                    for (int j = 0; j < 4; j++)
                    {
                        IField newField = new FieldClass();
                        IFieldEdit fieldEdit = (IFieldEdit)newField;
                        fieldEdit.Name_2 = lvFields.Items[j].SubItems[1].Text;
                        if (j == 3)
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                        else
                            fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        m_pFClass.AddField(newField);
                    }


                    //Update Field
                    pFCursor = m_pFClass.Update(null, false);
                    pFeature = pFCursor.NextFeature();

                    int intStatFldIdx = m_pFClass.FindField(lvFields.Items[0].SubItems[1].Text);
                    int intZFldIdx = m_pFClass.FindField(lvFields.Items[1].SubItems[1].Text);
                    int intPrFldIdx = m_pFClass.FindField(lvFields.Items[2].SubItems[1].Text);
                    int intFlgFldIdx = m_pFClass.FindField(strFlgFldNam);

                    double dblValue = 0, dblPvalue = 0, dblZvalue = 0;
                    double dblValueMean = arrVar.Average();
                    double dblPrCri = Convert.ToDouble(nudConfLevel.Value);

                    int featureIdx = 0;
                    while (pFeature != null)
                    {
                        dblValue = arrVar[featureIdx] - dblValueMean;
                        dblZvalue = nmResults[featureIdx, 3];
                        dblPvalue = nmResults[featureIdx, 4];
                        pFeature.set_Value(intStatFldIdx, (object)nmResults[featureIdx, 0]);
                        pFeature.set_Value(intZFldIdx, dblZvalue);
                        pFeature.set_Value(intPrFldIdx, dblPvalue);

                        if (dblPvalue < dblPrCri)
                        {
                            if (dblZvalue > 0)
                            {
                                if (dblValue > 0)
                                    pFeature.set_Value(intFlgFldIdx, "HH");
                                else
                                    pFeature.set_Value(intFlgFldIdx, "LL");
                            }
                            else
                            {
                                if (dblValue > 0)
                                    pFeature.set_Value(intFlgFldIdx, "HL");
                                else
                                    pFeature.set_Value(intFlgFldIdx, "LH");
                            }
                        }
                        //else
                        //    pFeature.set_Value(intFlgFldIdx, "");
                        pFCursor.UpdateFeature(pFeature);

                        pFeature = pFCursor.NextFeature();
                        featureIdx++;
                    }

                    pfrmProgress.Close();
                    if (chkMap.Checked)
                    {
                        double[,] adblMinMaxForLabel = new double[2, 4];
                        ITable pTable = (ITable)m_pFClass;

                        IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
                        pUniqueValueRenderer.FieldCount = 1;
                        pUniqueValueRenderer.set_Field(0, strFlgFldNam);
                        IDataStatistics pDataStat;
                        IStatisticsResults pStatResults;

                        ICursor pCursor;

                        if(m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            ISimpleFillSymbol pSymbol;
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
                        }
                        else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        {
                            ISimpleMarkerSymbol pSymbol;
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

                            pSymbol = new SimpleMarkerSymbolClass();
                            pSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
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

                            pSymbol = new SimpleMarkerSymbolClass();
                            pSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
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

                            pSymbol = new SimpleMarkerSymbolClass();
                            pSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
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

                            pSymbol = new SimpleMarkerSymbolClass();
                            pSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                            pSymbol.Color = m_pSnippet.getRGB(173, 255, 179);
                            pUniqueValueRenderer.AddValue("LH", null, (ISymbol)pSymbol);

                            if (intCnt == 1)
                                pUniqueValueRenderer.set_Label("LH", "LH (" + adblMinMaxForLabel[0, 3].ToString("N1") + ")");
                            else if (intCnt == 0)
                                pUniqueValueRenderer.set_Label("LH", "LH (no obs)");
                            else
                                pUniqueValueRenderer.set_Label("LH", "LH (" + adblMinMaxForLabel[0, 3].ToString("N1") + "-" + adblMinMaxForLabel[1, 3].ToString("N1") + ")");




                            pSymbol = new SimpleMarkerSymbolClass();
                            pSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle ;
                            pSymbol.Color = m_pSnippet.getRGB(200, 200, 200);
                            //pUniqueValueRenderer.AddValue("", strFlgFldNam, (ISymbol)pSymbol);
                            //pUniqueValueRenderer.set_Label("", "Not significant");
                            pUniqueValueRenderer.DefaultSymbol = (ISymbol)pSymbol;
                            pUniqueValueRenderer.DefaultLabel = "Not significant";

                            pUniqueValueRenderer.UseDefaultSymbol = true;
                        }



                        IFeatureLayer pNewFLayer = new FeatureLayerClass();
                        pNewFLayer.FeatureClass = m_pFClass;
                        pNewFLayer.Name = cboSAM.Text + " of " + m_pFLayer.Name;
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
                else if (cboSAM.Text == "Gi*")
                {
                    #region Gi*
                    m_pEngine.Evaluate("sample.lg <- localG(" + strVarNM + ", sample.listw, zero.policy=TRUE)");
                    m_pEngine.Evaluate("sample.p <- 2*pnorm(-abs(sample.lg))");

                    if (cboAdjustment.Text == "Bonferroni correction")
                        m_pEngine.Evaluate("sample.p <- p.adjust(sample.p, method = 'bonferroni', n = length(sample.p))");

                    double[] dblGValues = m_pEngine.Evaluate("sample.lg").AsNumeric().ToArray();
                    double[] dblPvalues = m_pEngine.Evaluate("sample.p").AsNumeric().ToArray();

                    //Save Output on SHP
                    //Add Target fields to store results in the shapefile
                    for (int j = 0; j < 2; j++)
                    {
                        IField newField = new FieldClass();
                        IFieldEdit fieldEdit = (IFieldEdit)newField;
                        fieldEdit.Name_2 = lvFields.Items[j].SubItems[1].Text;
                        fieldEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                        m_pFClass.AddField(newField);
                    }

                    //Update Field
                    pFCursor = m_pFClass.Update(null, false);
                    pFeature = pFCursor.NextFeature();

                    int intStatFldIdx = m_pFClass.FindField(lvFields.Items[0].SubItems[1].Text);
                    int intPrFldIdx = m_pFClass.FindField(lvFields.Items[1].SubItems[1].Text);

                    int featureIdx = 0;
                    while (pFeature != null)
                    {
                        pFeature.set_Value(intStatFldIdx, dblGValues[featureIdx]);
                        pFeature.set_Value(intPrFldIdx, dblPvalues[featureIdx]);

                        pFCursor.UpdateFeature(pFeature);

                        pFeature = pFCursor.NextFeature();
                        featureIdx++;
                    }
                    pFCursor.Flush();

                    pfrmProgress.Close();

                    if (chkMap.Checked)
                    {
                        string strStaticFldName = lvFields.Items[0].SubItems[1].Text;

                        m_pEngine.Evaluate("p.vals <- c(0.1, 0.05, 0.01)");
                    if (cboAdjustment.Text == "Bonferroni correction")
                    {
                        m_pEngine.Evaluate("sample.n <- length(sample.p)");
                        m_pEngine.Evaluate("p.vals <- p.vals/sample.n");
                    }
                        m_pEngine.Evaluate("zc <- qnorm(1 - (p.vals/2))");
                        double[] dblZBrks = m_pEngine.Evaluate("sort(cbind(zc, -zc))").AsNumeric().ToArray();
                        
                        pFCursor = m_pFClass.Search(null, false);
                        IDataStatistics pDataStat = new DataStatisticsClass();
                        pDataStat.Field = strStaticFldName;
                        pDataStat.Cursor = (ICursor)pFCursor;
                        IStatisticsResults pStatResults = pDataStat.Statistics;
                        double dblMax = pStatResults.Maximum;
                        double dblMin = pStatResults.Minimum;
                        int intBreaksCount = dblZBrks.Length + 2;
                        double[] cb = new double[intBreaksCount];

                        //Assign Min and Max values for class breaks
                        if(dblMin < dblZBrks[0])
                            cb[0] = dblMin;
                        else
                            cb[0] = dblZBrks[0] - 1; //Manually Assigned minimum value

                        if (dblMax > dblZBrks[dblZBrks.Length - 1])
                            cb[intBreaksCount - 1] = dblMax;
                        else
                            cb[intBreaksCount -1] = dblZBrks[dblZBrks.Length - 1] + 1;//Manually Assigned minimum value

                        for (int k = 0; k < intBreaksCount-2; k++)
                            cb[k + 1] = dblZBrks[k];

                        IClassBreaksRenderer pCBRenderer = new ClassBreaksRenderer();
                        pCBRenderer.Field = strStaticFldName;
                        pCBRenderer.BreakCount = intBreaksCount -1;
                        pCBRenderer.MinimumBreak = cb[0];

                        //' use this interface to set dialog properties
                        IClassBreaksUIProperties pUIProperties = (IClassBreaksUIProperties)pCBRenderer;
                        pUIProperties.ColorRamp = "Custom";
                        if(m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            ISimpleFillSymbol pSimpleFillSym;

                            int[,] arrColors = CreateColorRamp();

                            //Add Probability Value Manually
                            string[] strsProbLabels = new string[] { "(0.01)", "(0.05)", "(0.1)", "(0.1)", "(0.05)", "(0.01)" };
                            //' be careful, indices are different for the diff lists
                            for (int j = 0; j < intBreaksCount - 1; j++)
                            {
                                pCBRenderer.Break[j] = cb[j + 1];
                                if (j == 0)
                                    pCBRenderer.Label[j] = " <= " + Math.Round(cb[j + 1], 2).ToString() + strsProbLabels[j];
                                else if (j == intBreaksCount - 2)
                                    pCBRenderer.Label[j] = " > " + Math.Round(cb[j], 2).ToString() + strsProbLabels[j - 1];
                                else
                                    pCBRenderer.Label[j] = Math.Round(cb[j], 2).ToString() + strsProbLabels[j - 1] + " ~ " + Math.Round(cb[j + 1], 2).ToString() + strsProbLabels[j];
                                pUIProperties.LowBreak[j] = cb[j];
                                pSimpleFillSym = new SimpleFillSymbolClass();
                                IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                                pSimpleFillSym.Color = (IColor)pRGBColor;
                                pCBRenderer.Symbol[j] = (ISymbol)pSimpleFillSym;
                            }
                        }
                        else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        {
                            ISimpleMarkerSymbol pSimpleMarkerSym;

                            int[,] arrColors = CreateColorRamp();

                            //Add Probability Value Manually
                            string[] strsProbLabels = new string[] { "(0.01)", "(0.05)", "(0.1)", "(0.1)", "(0.05)", "(0.01)" };
                            //' be careful, indices are different for the diff lists
                            for (int j = 0; j < intBreaksCount - 1; j++)
                            {
                                pCBRenderer.Break[j] = cb[j + 1];
                                if (j == 0)
                                    pCBRenderer.Label[j] = " <= " + Math.Round(cb[j + 1], 2).ToString() + strsProbLabels[j];
                                else if (j == intBreaksCount - 2)
                                    pCBRenderer.Label[j] = " > " + Math.Round(cb[j], 2).ToString() + strsProbLabels[j - 1];
                                else
                                    pCBRenderer.Label[j] = Math.Round(cb[j], 2).ToString() + strsProbLabels[j - 1] + " ~ " + Math.Round(cb[j + 1], 2).ToString() + strsProbLabels[j];
                                pUIProperties.LowBreak[j] = cb[j];
                                pSimpleMarkerSym = new SimpleMarkerSymbolClass();
                                IRgbColor pRGBColor = m_pSnippet.getRGB(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]);
                                pSimpleMarkerSym.Color = (IColor)pRGBColor;
                                pCBRenderer.Symbol[j] = (ISymbol)pSimpleMarkerSym;
                            }
                        }
                        

                        IFeatureLayer pNewFLayer = new FeatureLayerClass();
                        pNewFLayer.FeatureClass = m_pFClass;
                        pNewFLayer.Name = cboSAM.Text + " of " + m_pFLayer.Name;
                        IGeoFeatureLayer pGFLayer = (IGeoFeatureLayer)pNewFLayer;
                        pGFLayer.Renderer = (IFeatureRenderer)pCBRenderer;
                        m_pActiveView.FocusMap.AddLayer(pGFLayer);
                        m_pActiveView.Refresh();
                        m_pForm.axTOCControl1.Update();
                    }
                    else
                        MessageBox.Show("Complete. The results are stored in the shape file");
                    #endregion
                }

                this.Close();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnOpenSWM_Click(object sender, EventArgs e)
        {
            //Open SWM
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

        private void cboSAM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_pFClass == null)
                return;
            //string strLayerName = cboTargetLayer.Text;

            //int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
            //ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

            //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            //ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

            string strMethod = cboSAM.Text;

            UpdateListview(lvFields, m_pFClass, strMethod);
        }
        private int[,] CreateColorRamp()
        {
            int[,] arrColors = new int[7, 3];
            arrColors[0, 0] = 33; arrColors[0, 1] = 102; arrColors[0, 2] = 172;
            arrColors[1, 0] = 103; arrColors[1, 1] = 169; arrColors[1, 2] = 207;
            arrColors[2, 0] = 209; arrColors[2, 1] = 229; arrColors[2, 2] = 240;
            arrColors[3, 0] = 247; arrColors[3, 1] = 247; arrColors[3, 2] = 247;
            arrColors[4, 0] = 253; arrColors[4, 1] = 219; arrColors[4, 2] = 199;
            arrColors[5, 0] = 239; arrColors[5, 1] = 138; arrColors[5, 2] = 98;
            arrColors[6, 0] = 178; arrColors[6, 1] = 24; arrColors[6, 2] = 43;

            return arrColors;
        }



    }
}
