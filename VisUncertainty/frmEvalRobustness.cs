using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.DataVisualization.Charting;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;

//Under developing 012016 HK

namespace VisUncertainty
{
    public partial class frmEvalRobustness : Form
    {
        private MainForm mForm;
        private IActiveView m_pActiveView;
        private clsSnippet m_pSnippet;
        private clsRenderedLayers m_pRenderedLayer;

        private int intRounding = 0;

        //Visualizing
        private IFeatureLayer m_pFLayer;
        private double[] m_arrValue;
        double[,] dblClsMean;
        double dblMeanRobustness;

        public frmEvalRobustness()
        {
            try
            {
                InitializeComponent();
                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;

                m_pActiveView = mForm.axMapControl1.ActiveView;
                m_pSnippet = new clsSnippet();

                for (int i = 0; i < mForm.lstRenderedLayers.Count; i++)
                {

                    cboSourceLayer.Items.Add(mForm.lstRenderedLayers[i].strLayerName);
                }

                intRounding = 2; //Default
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

            
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < mForm.lstRenderedLayers.Count; i++)
                {
                    if (cboSourceLayer.Text == mForm.lstRenderedLayers[i].strLayerName)
                        m_pRenderedLayer = mForm.lstRenderedLayers[i];
                }
                lblUncernNM.Text = "Uncertainty Field: " + m_pRenderedLayer.strUncernFldName;
                lblValueName.Text = "Estimates Field: " + m_pRenderedLayer.strValueFldName;
                lblMethod.Text = "Classfication Method: " + m_pRenderedLayer.strClassificationType;
                UpdateRange(lvSymbol, m_pRenderedLayer.ClassBreaks.Length - 1, m_pRenderedLayer.ClassBreaks);

                if (m_pRenderedLayer.strUncernFldName == string.Empty)
                {
                    cboUncernFld.Visible = true;
                    int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, cboSourceLayer.Text);
                    ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                    m_pFLayer = pLayer as IFeatureLayer;

                    IFields fields = m_pFLayer.FeatureClass.Fields;

                    cboUncernFld.Items.Clear();

                    for (int i = 0; i < fields.FieldCount; i++)
                    {
                        cboUncernFld.Items.Add(fields.get_Field(i).Name);
                    }
                    if(cboMeasure.Text == "Entropy") 
                        txtFldName.Text = UpdateFldName("Ent", m_pFLayer.FeatureClass);
                    else
                        txtFldName.Text = UpdateFldName("Rob", m_pFLayer.FeatureClass);

                }
                else
                    cboUncernFld.Visible = false;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }


        private void cboMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMeasure.Text == "Entropy")
                txtFldName.Text = UpdateFldName("Ent", m_pFLayer.FeatureClass);
            else
                txtFldName.Text = UpdateFldName("Rob", m_pFLayer.FeatureClass);
        }

        #region Private Functions
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
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return null;
            }
        }

        #endregion

        private void btnEval_Click(object sender, EventArgs e)
        {
            EvalRobustness(m_pRenderedLayer.ClassBreaks);
            //EvalStat(pRenderedLayer.ClassBreaks);
            
        }
        private class Robustness
        {
            public int TargetClass;
            public double[] Robustnesses;
            public double Entropy;

        }
        private void EvalRobustness(double[] cb)
        {
            int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, cboSourceLayer.Text);
            ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

            m_pFLayer = pLayer as IFeatureLayer;
            IFeatureClass pFClass = m_pFLayer.FeatureClass;
            string strValueFld = m_pRenderedLayer.strValueFldName;
            string strUncernfld = string.Empty;
            if (m_pRenderedLayer.strUncernFldName == string.Empty)
                strUncernfld = cboUncernFld.Text;
            else
                strUncernfld = m_pRenderedLayer.strUncernFldName;
            if (strUncernfld == string.Empty)
                return;

            int intValueIdx = pFClass.FindField(strValueFld);
            int intUncernIdx = pFClass.FindField(strUncernfld);
            int intClassCount = cb.Length - 1;

            string strSavefldnm = txtFldName.Text;
            //strTempfldName = "MinSepfave";


            if (pFClass.FindField(strSavefldnm) == -1)
                    m_pSnippet.AddField(pFClass, strSavefldnm, esriFieldType.esriFieldTypeDouble);

            int intSavefldIdx = pFClass.FindField(strSavefldnm);

            int intFCounts = pFClass.FeatureCount(null);
            Chart pChart = new Chart();

            IFeature pFeat = null;
            
            IFeatureCursor pFCursor = null;
            pFCursor = pFClass.Update(null, false);

            pFeat = pFCursor.NextFeature();
            Robustness[] pRobustness = new Robustness[intFCounts];
            double[] arrValue = new double[intFCounts];

            int i = 0;
            while (pFeat != null)
            {
                pRobustness[i] = new Robustness();
                pRobustness[i].Robustnesses = new double[intClassCount];

                double dblValue = Convert.ToDouble(pFeat.get_Value(intValueIdx));
                double dblStd = Convert.ToDouble(pFeat.get_Value(intUncernIdx));

                double dblEntropy = 0;
                for (int j = 0; j < intClassCount; j++)
                {
                    double dblUpperZvalue = (cb[j + 1] - dblValue) / dblStd;
                    double dblUpperConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblUpperZvalue);
                    double dblLowerZvalue = (cb[j] - dblValue) / dblStd;
                    double dblLowerConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblLowerZvalue);
                    double dblProb = dblUpperConfLev - dblLowerConfLev;
                    pRobustness[i].Robustnesses[j] = dblProb; //Probability of an observation to each class

                    if(dblProb != 0)
                        dblEntropy += dblProb * Math.Log(dblProb, 2);

                    if (j == 0)
                    {
                        if (dblValue >= cb[j] && dblValue <= cb[j + 1])
                            pRobustness[i].TargetClass = j;
                    }
                    else
                    {
                        if (dblValue > cb[j] && dblValue <= cb[j + 1])
                            pRobustness[i].TargetClass = j;
                    }
                }



                pRobustness[i].Entropy = ((double)(-1) * dblEntropy) / (Math.Log(intClassCount, 2));

                if (cboMeasure.Text == "Entropy")
                {
                    arrValue[i] = pRobustness[i].Entropy;
                    pFeat.set_Value(intSavefldIdx, pRobustness[i].Entropy);
                }
                else
                {

                    pFeat.set_Value(intSavefldIdx, pRobustness[i].Robustnesses[pRobustness[i].TargetClass]);
                    arrValue[i] = pRobustness[i].Robustnesses[pRobustness[i].TargetClass];
                }

                
                pFCursor.UpdateFeature(pFeat);
                pFeat = pFCursor.NextFeature();
                i++;

            }

            //Visualization
            //dblClsMean = new double[intClassCount, 3];
            //double dblSumRobustness = 0;
            //for (int j = 0; j < intFCounts; j++)
            //{
            //    dblSumRobustness = dblSumRobustness + arrRobustness[j, 0];
            //    for (int k = 0; k < intClassCount; k++)
            //    {
            //        if (arrRobustness[j, 1] == k)
            //        {
            //            dblClsMean[k, 0] = arrRobustness[j, 0] + dblClsMean[k, 0];
            //            dblClsMean[k, 1] = dblClsMean[k, 1] + 1;
            //        }
            //    }
            //}
            //for (int k = 0; k < intClassCount; k++)
            //{
            //    dblClsMean[k, 2] = dblClsMean[k, 0] / dblClsMean[k, 1];
            //}
            //dblMeanRobustness = dblSumRobustness / intFCounts;


            //switch (cboGCClassify.Text)
            //{
            //    case "Equal Interval":
            //        pClassifyGEN = new EqualIntervalClass();
            //        break;
            //    case "Geometrical Interval":
            //        pClassifyGEN = new GeometricalInterval();
            //        break;
            //    case "Natural Breaks":
            //        pClassifyGEN = new NaturalBreaksClass();
            //        break;
            //    case "Quantile":
            //        pClassifyGEN = new QuantileClass();
            //        break;
            //    case "StandardDeviation":
            //        pClassifyGEN = new StandardDeviationClass();
            //        break;
            //    default:
            //        pClassifyGEN = new NaturalBreaksClass();
            //        break;
            //}
            if (chkAddMap.Checked)
            {


                IClassifyGEN pClassifyGEN = new NaturalBreaksClass();

                IFeatureLayer pflUncern = new FeatureLayerClass();
                pflUncern.FeatureClass = pFClass;
                pflUncern.Name = cboSourceLayer.Text + " " + cboMeasure.Text;
                pflUncern.Visible = true;

                IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)pflUncern;

                ITableHistogram pTableHistogram2 = new BasicTableHistogramClass();
                ITable pTable = (ITable)pFClass;
                pTableHistogram2.Field = strSavefldnm;
                pTableHistogram2.Table = pTable;
                //IHistogram pHistogram = (IHistogram)pTableHistogram2;
                IBasicHistogram pHistogram = (IBasicHistogram)pTableHistogram2;

                object xVals, frqs;
                pHistogram.GetHistogram(out xVals, out frqs);
                pClassifyGEN.Classify(xVals, frqs, 4); //Fixed Class count
                double[] cb_uncern = (double[])pClassifyGEN.ClassBreaks;

                if (cboMeasure.Text == "Entropy")
                {
                    cb_uncern[1] = 0.4; cb_uncern[2] = 0.6; cb_uncern[3] = 0.75;
                }
                else
                {

                    cb_uncern[1] = 0.4; cb_uncern[2] = 0.6; cb_uncern[3] = 0.75;
                }


                ISpacingBreaksRenderer pSpacingBrksRenderers = new SpacingBreaksRendererClass();
                pSpacingBrksRenderers.arrClassBrks = cb_uncern;
                pSpacingBrksRenderers.arrValue = arrValue;

                if (cboMeasure.Text == "Entropy")
                {
                    pSpacingBrksRenderers.dblFromSep = Convert.ToDouble(1);
                    pSpacingBrksRenderers.dblToSep = Convert.ToDouble(20);
                }
                else
                {

                    pSpacingBrksRenderers.dblFromSep = Convert.ToDouble(20);
                    pSpacingBrksRenderers.dblToSep = Convert.ToDouble(1);
                }

                pSpacingBrksRenderers.dblLineAngle = Convert.ToDouble(45);
                pSpacingBrksRenderers.dblLineWidth = Convert.ToDouble(0.1);
                pSpacingBrksRenderers.m_pLineRgb = m_pSnippet.getRGB(0, 0, 0);
                if (pSpacingBrksRenderers.m_pLineRgb == null)
                    return;

                pSpacingBrksRenderers.strHeading = cboMeasure.Text;
                pSpacingBrksRenderers.intRoundingDigits = 2;
                pSpacingBrksRenderers.CreateLegend();

                pGeofeatureLayer.Renderer = (IFeatureRenderer)pSpacingBrksRenderers;

                m_pActiveView.FocusMap.AddLayer(pGeofeatureLayer);
                m_pActiveView.Refresh();
                mForm.axTOCControl1.Update();
            }

        }
        private IFeatureCursor SortData(IFeatureCursor pCursor, IFeatureClass FClass, IQueryFilter pQfilter, string strValueFld)
        {
            //try
            //{
                // sort in descending by value
                ITable pTable = null;
                pTable = FClass as ITable;

                ITableSort pTableSort = null;
                pTableSort = new TableSort();
                pTableSort.Table = pTable;
                pTableSort.Cursor = pCursor as ICursor;

                //set up the query filter.
                IQueryFilter pQF = null;
                pQF = new QueryFilter();
                pQF.SubFields = "*";
                pQF.WhereClause = pQfilter.WhereClause;
                pTableSort.QueryFilter = pQF;

                pTableSort.Fields = strValueFld;
                pTableSort.set_Ascending(strValueFld, true);

                // call the sort
                pTableSort.Sort(null);

                // retrieve the sorted rows
                IFeatureCursor pSortedCursor = null;
                pSortedCursor = pTableSort.Rows as IFeatureCursor;

                return pSortedCursor;
            //}
            //catch (Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine("Sorting Error: " + e.Message);
            //    return null;
            //}
        }
        
        //private void btnAddLayer_Click(object sender, EventArgs e)
        //{
        //    //Create Rendering of Mean Value at Target Layer
        //    int intBreakCount = Convert.ToInt32(nudTeNClasses.Value);
        //    //string strGCRenderField = cboValueField.Text;
        //    string strNewLayerName = "Evalutation";
        //    string strNewHeading = "Robustness";

        //    IFeatureLayer pflOutput = new FeatureLayerClass();
        //    pflOutput.FeatureClass = pFLayer.FeatureClass;
        //    pflOutput.Name = strNewLayerName;
        //    pflOutput.Visible = true;
        //    IGeoFeatureLayer pGeofeatureLayer = (IGeoFeatureLayer)pflOutput;

        //    IClassifyGEN pClassifyGEN;
        //    switch (cboGCClassify.Text)
        //    {
        //        case "Equal Interval":
        //            pClassifyGEN = new EqualIntervalClass();
        //            break;
        //        case "Geometrical Interval":
        //            pClassifyGEN = new GeometricalInterval();
        //            break;
        //        case "Natural Breaks":
        //            pClassifyGEN = new NaturalBreaksClass();
        //            break;
        //        case "Quantile":
        //            pClassifyGEN = new QuantileClass();
        //            break;
        //        case "StandardDeviation":
        //            pClassifyGEN = new StandardDeviationClass();
        //            break;
        //        default:
        //            pClassifyGEN = new NaturalBreaksClass();
        //            break;
        //    }
        //    IDataHistogram pDataHistogram = new DataHistogramClass();

        //    pDataHistogram.SetData(arrValue);

        //    IHistogram pHistogram = (IHistogram)pDataHistogram;

        //    object xVals, frqs;
        //    pHistogram.GetHistogram(out xVals, out frqs);
        //    pClassifyGEN.Classify(xVals, frqs, intBreakCount);
        //    double[] cb = (double[])pClassifyGEN.ClassBreaks;

        //    ISpacingBreaksRenderer pSpacingBrksRenderers = new SpacingBreaksRendererClass();
        //    pSpacingBrksRenderers.arrClassBrks = cb;
        //    pSpacingBrksRenderers.arrValue = arrValue;
        //    pSpacingBrksRenderers.dblFromSep = Convert.ToDouble(nudSeperationFrom.Value);
        //    pSpacingBrksRenderers.dblToSep = Convert.ToDouble(nudSeperationTo.Value);
        //    pSpacingBrksRenderers.dblLineAngle = Convert.ToDouble(nudAngleFrom.Value);
        //    pSpacingBrksRenderers.dblLineWidth = Convert.ToDouble(nudTeLinewidth.Value);
        //    pSpacingBrksRenderers.m_pLineRgb = pSnippet.getRGB(picTeLineColor.BackColor.R, picTeLineColor.BackColor.G, picTeLineColor.BackColor.B);
        //    if (pSpacingBrksRenderers.m_pLineRgb == null)
        //        return;
            
        //    pSpacingBrksRenderers.strHeading = strNewHeading;
        //    pSpacingBrksRenderers.intRoundingDigits = 2;
        //    pSpacingBrksRenderers.CreateLegend();

        //    pGeofeatureLayer.Renderer = (IFeatureRenderer)pSpacingBrksRenderers;
            
        //    pActiveView.FocusMap.AddLayer(pGeofeatureLayer);
        //    pActiveView.Refresh();
        //    mForm.axTOCControl1.Update();
        //}

        #region Deprecated functions
        private void UpdateRange(ListView lvSymbol, int intGCBreakeCount, double[] cb)
        {
            lvSymbol.BeginUpdate();
            lvSymbol.Items.Clear();


            for (int j = 0; j < intGCBreakeCount; j++)
            {

                ListViewItem lvi = new ListViewItem("");
                lvi.UseItemStyleForSubItems = false;
                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //    "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

                if (j == 0)
                {
                    lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                }
                else
                {

                    double dblAdding = Math.Pow(0.1, intRounding);
                    lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));

                }
                lvSymbol.Items.Add(lvi);
            }
            lvSymbol.EndUpdate();
        }
        private void UpdateRangeRobustness(ListView lvSymbol, int intGCBreakeCount, double[] cb, double[,] dblClsMean, double dblMeanRobustness, double[,] arrEvalStat)
        {
            lvSymbol.BeginUpdate();
            lvSymbol.Items.Clear();

            double[] dblTotalEval = new double[5];
            for (int j = 0; j < intGCBreakeCount; j++)
            {

                ListViewItem lvi = new ListViewItem("");
                lvi.UseItemStyleForSubItems = false;
                //lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                //    "", Color.White, Color.FromArgb(arrColors[j, 0], arrColors[j, 1], arrColors[j, 2]), lvi.Font));

                if (j == 0)
                {
                    lvi.SubItems.Add(Math.Round(cb[j], intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(dblClsMean[j, 2], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 0], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 1], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 2], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 3], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 4], intRounding).ToString("N" + intRounding.ToString()));
                }
                else
                {
                    double dblAdding = Math.Pow(0.1, intRounding);
                    lvi.SubItems.Add(Math.Round(cb[j] + dblAdding, intRounding).ToString("N" + intRounding.ToString()) + " - " + Math.Round(cb[j + 1], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(dblClsMean[j, 2], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 0], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 1], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 2], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 3], intRounding).ToString("N" + intRounding.ToString()));
                    lvi.SubItems.Add(Math.Round(arrEvalStat[j, 4], intRounding).ToString("N" + intRounding.ToString()));
                }
                lvSymbol.Items.Add(lvi);

                //Calculate Total
                dblTotalEval[0] += arrEvalStat[j, 0];
                dblTotalEval[1] += arrEvalStat[j, 1];
                dblTotalEval[2] += arrEvalStat[j, 2];
                dblTotalEval[3] += arrEvalStat[j, 3];
                dblTotalEval[4] += arrEvalStat[j, 4];
            }
            ListViewItem lviforTotal = new ListViewItem("");
            lviforTotal.UseItemStyleForSubItems = false;
            lviforTotal.SubItems.Add("Total");
            lviforTotal.SubItems.Add(Math.Round(dblMeanRobustness, intRounding).ToString("N" + intRounding.ToString()));
            //Add Total
            lviforTotal.SubItems.Add(Math.Round(dblTotalEval[0], intRounding).ToString("N" + intRounding.ToString()));
            lviforTotal.SubItems.Add(Math.Round(dblTotalEval[1], intRounding).ToString("N" + intRounding.ToString()));
            lviforTotal.SubItems.Add(Math.Round(dblTotalEval[2], intRounding).ToString("N" + intRounding.ToString()));
            lviforTotal.SubItems.Add(Math.Round(dblTotalEval[3], intRounding).ToString("N" + intRounding.ToString()));
            lviforTotal.SubItems.Add(Math.Round(dblTotalEval[4], intRounding).ToString("N" + intRounding.ToString()));
            lvSymbol.Items.Add(lviforTotal);

            lvSymbol.EndUpdate();
        }

        private void EvalStat(double[] cb)
        {

            IFeatureClass pFClass = m_pFLayer.FeatureClass;
            string strValueFld = m_pRenderedLayer.strValueFldName;
            string strUncernfld = string.Empty;
            if (m_pRenderedLayer.strUncernFldName == string.Empty)
                strUncernfld = cboUncernFld.Text;
            else
                strUncernfld = m_pRenderedLayer.strUncernFldName;
            if (strUncernfld == string.Empty)
                return;

            int intValueIdx = pFClass.FindField(strValueFld);
            int intUncernIdx = pFClass.FindField(strUncernfld);

            int intClassCount = cb.Length - 1;

            double[,] arrEvalStat = new double[intClassCount, 5];

            string strTempfldName = txtFldName.Text;
            strTempfldName = "MinSepfave";

            int intFCounts = pFClass.FeatureCount(null);
            Chart pChart = new Chart();



            IFeatureCursor pFCursor = null;
            pFCursor = pFClass.Search(null, false);

            IQueryFilter pQfilter = new QueryFilterClass();
            for (int j = 0; j < intClassCount; j++)
            {

                if (j == 0)
                    pQfilter.WhereClause = strValueFld + " >= " + cb[j].ToString() + " AND " + strValueFld + " <= " + cb[j + 1].ToString();
                else
                    pQfilter.WhereClause = strValueFld + " > " + cb[j].ToString() + " AND " + strValueFld + " <= " + cb[j + 1].ToString();

                pFCursor = pFClass.Search(pQfilter, true);
                IFeatureCursor pSortedCursor = SortData(pFCursor, pFClass, pQfilter, strValueFld);
                IFeature pFeat = null;
                pFeat = pSortedCursor.NextFeature();
                int intSelCnt = pFClass.FeatureCount(pQfilter);
                int i = 0;
                double[] arrSubEst = new double[intSelCnt];
                double[] arrSubVar = new double[intSelCnt];

                while (pFeat != null)
                {
                    arrSubEst[i] = Convert.ToDouble(pFeat.get_Value(intValueIdx));
                    arrSubVar[i] = Convert.ToDouble(pFeat.get_Value(intUncernIdx));
                    i++;
                    pFeat = pSortedCursor.NextFeature();
                }
                pSortedCursor.Flush();

                arrEvalStat[j, 0] = intSelCnt;
                arrEvalStat[j, 1] = SumSeperablility(arrSubEst, arrSubVar);
                arrEvalStat[j, 2] = SumBhatta(arrSubEst, arrSubVar);
                arrEvalStat[j, 3] = MaxSeperablility(arrSubEst, arrSubVar);
                arrEvalStat[j, 4] = MaxBhatta(arrSubEst, arrSubVar);
            }

            //int k = 0;
            //IFeatureCursor pSortedCursor = SortData(pFCursor, pFClass

            UpdateRangeRobustness(lvSymbol, intClassCount, cb, dblClsMean, dblMeanRobustness, arrEvalStat);

            //double[,] arrRobustness = new double[intFCounts, 2];
            //arrValue = new double[intFCounts];//For visualizing

            //int i = 0;
            //while (pFeat != null)
            //{
            //    for (int j = 0; j < intClassCount; j++)
            //    {

            //        double dblValue = Convert.ToDouble(pFeat.get_Value(intValueIdx));
            //        double dblStd = Convert.ToDouble(pFeat.get_Value(intUncernIdx));
            //        if (j == 0)
            //        {
            //            if (dblValue >= cb[j] && dblValue <= cb[j + 1])
            //            {
            //                double dblUpperZvalue = (cb[j + 1] - dblValue) / dblStd;
            //                double dblUpperConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblUpperZvalue);
            //                double dblLowerZvalue = (cb[j] - dblValue) / dblStd;
            //                double dblLowerConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblLowerZvalue);
            //                arrRobustness[i, 0] = dblUpperConfLev - dblLowerConfLev;
            //                arrRobustness[i, 1] = j;
            //                arrValue[i] = arrRobustness[i, 0];
            //                if (chkRobustness.Checked)
            //                    pFeat.set_Value(intTempfldIdx, arrValue[i]);
            //            }

            //        }
            //        else
            //        {
            //            if (dblValue > cb[j] && dblValue <= cb[j + 1])
            //            {
            //                double dblUpperZvalue = (cb[j + 1] - dblValue) / dblStd;
            //                double dblUpperConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblUpperZvalue);
            //                double dblLowerZvalue = (cb[j] - dblValue) / dblStd;
            //                double dblLowerConfLev = pChart.DataManipulator.Statistics.NormalDistribution(dblLowerZvalue);
            //                arrRobustness[i, 0] = dblUpperConfLev - dblLowerConfLev;
            //                arrRobustness[i, 1] = j;
            //                arrValue[i] = arrRobustness[i, 0];
            //                if (chkRobustness.Checked)
            //                    pFeat.set_Value(intTempfldIdx, arrValue[i]);
            //            }
            //        }
            //    }
            //    i++;
            //    if (chkRobustness.Checked)
            //        pFCursor.UpdateFeature(pFeat);
            //    pFeat = pFCursor.NextFeature();
            //}
            //double[,] dblClsMean = new double[intClassCount, 3];
            //double dblSumRobustness = 0;
            //for (int j = 0; j < intFCounts; j++)
            //{
            //    dblSumRobustness = dblSumRobustness + arrRobustness[j, 0];
            //    for (int k = 0; k < intClassCount; k++)
            //    {
            //        if (arrRobustness[j, 1] == k)
            //        {
            //            dblClsMean[k, 0] = arrRobustness[j, 0] + dblClsMean[k, 0];
            //            dblClsMean[k, 1] = dblClsMean[k, 1] + 1;
            //        }
            //    }
            //}
            //for (int k = 0; k < intClassCount; k++)
            //{
            //    dblClsMean[k, 2] = dblClsMean[k, 0] / dblClsMean[k, 1];
            //}
            //double dblMeanRobustness = dblSumRobustness / intFCounts;
            //UpdateRangeRobustness(lvSymbol, intClassCount, cb, dblClsMean, dblMeanRobustness);


        }
        #endregion
        #region Calculate Costs
        private double CalSDfromArray(double[] arrSubset)
        {
            double average = arrSubset.Average();
            double sumOfSquaresOfDifferences = arrSubset.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / arrSubset.Length);

            return sd;
        }
        private double MaxSeperablility(double[] arrSubEst, double[] arrSubVar)
        {
            double dblMaxSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblMaxSep;
            else
            {
                dblMaxSep = double.MinValue;
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblCL = Math.Abs(arrSubEst[n] - arrSubEst[m]) / (Math.Sqrt(Math.Pow(arrSubVar[n], 2) + Math.Pow(arrSubVar[m], 2)));
                        if (dblCL > dblMaxSep)
                            dblMaxSep = dblCL;
                    }
                }
                return dblMaxSep;
            }

        }
        private double SumSeperablility(double[] arrSubEst, double[] arrSubVar)
        {
            double dblSumSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblSumSep;
            else
            {
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblCL = Math.Abs(arrSubEst[n] - arrSubEst[m]) / (Math.Sqrt(Math.Pow(arrSubVar[n], 2) + Math.Pow(arrSubVar[m], 2)));
                        dblSumSep = dblSumSep + dblCL;
                    }
                }
                return dblSumSep;
            }

        }
        private double MeanSeperablility(double[] arrSubEst, double[] arrSubVar)
        {
            double dblMeanSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblMeanSep;
            else
            {
                int intCount = 0;
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblCL = Math.Abs(arrSubEst[n] - arrSubEst[m]) / (Math.Sqrt(Math.Pow(arrSubVar[n], 2) + Math.Pow(arrSubVar[m], 2)));
                        dblMeanSep = dblMeanSep + dblCL;
                        intCount++;
                    }
                }
                dblMeanSep = dblMeanSep / intCount;
                return dblMeanSep;
            }

        }
        private double MaxBhatta(double[] arrSubEst, double[] arrSubVar)
        {
            double dblMaxBhatt = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblMaxBhatt;
            else
            {
                dblMaxBhatt = double.MinValue;
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    double dblsquaredVar1 = Math.Pow(arrSubVar[m], 2);
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblsquaredVar2 = Math.Pow(arrSubVar[n], 2);
                        double dblVarComp = Math.Log(0.25 * ((dblsquaredVar1 / dblsquaredVar2) + (dblsquaredVar2 / dblsquaredVar1) + 2));
                        double dblMeanComp = Math.Pow(arrSubEst[m] - arrSubEst[n], 2) / (dblsquaredVar1 + dblsquaredVar2);
                        double dblBhatt = 0.25 * (dblVarComp + dblMeanComp);

                        if (dblBhatt > dblMaxBhatt)
                            dblMaxBhatt = dblBhatt;
                    }
                }
                return dblMaxBhatt;
            }

        }
        private double SumBhatta(double[] arrSubEst, double[] arrSubVar)
        {
            double dblSumBhatt = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblSumBhatt;
            else
            {
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    double dblsquaredVar1 = Math.Pow(arrSubVar[m], 2);
                    for (int n = (m + 1); n < intArrayLen; n++)
                    {
                        double dblsquaredVar2 = Math.Pow(arrSubVar[n], 2);
                        double dblVarComp = Math.Log(0.25 * ((dblsquaredVar1 / dblsquaredVar2) + (dblsquaredVar2 / dblsquaredVar1) + 2));
                        double dblMeanComp = Math.Pow(arrSubEst[m] - arrSubEst[n], 2) / (dblsquaredVar1 + dblsquaredVar2);
                        double dblBhatt = 0.25 * (dblVarComp + dblMeanComp);

                        dblSumBhatt = dblSumBhatt + dblBhatt;
                    }
                }
                return dblSumBhatt;
            }

        }
        private double SeperablilityFromMean(double[] arrSubEst, double[] arrSubVar)
        {
            double average = arrSubEst.Average();
            double dblSep = 0;
            int intArrayLen = arrSubEst.Length;

            if (intArrayLen == 1)
                return dblSep;
            else
            {
                for (int m = 0; m < intArrayLen - 1; m++)
                {
                    //Sum of ABS
                    dblSep = dblSep + Math.Abs((average - arrSubEst[m]) / arrSubVar[m]);
                }
                return dblSep;
            }

        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
