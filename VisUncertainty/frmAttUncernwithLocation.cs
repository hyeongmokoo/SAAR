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
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.CartoUI;
using ESRI.ArcGIS.Display;

using RDotNet;
using RDotNet.NativeLibrary;
//Required library: maptools, foreign, sp, circular;

//Simulation part is required to be revised.

namespace VisUncertainty
{
    public partial class frmAttUncernwithLocation : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;
        private clsSnippet pSnippet;
        
        private double[,] arrSummary;

        public frmAttUncernwithLocation()
        {
            try
            {
                InitializeComponent();

                mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                pActiveView = mForm.axMapControl1.ActiveView;

                for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
                {
                    cboTargetLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                    cboSourceLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                }

                pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void cboSourceLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

                IFields fields = pFClass.Fields;

                cboFieldName.Items.Clear();
                cboX.Items.Clear();
                cboY.Items.Clear();

                for (int i = 0; i < fields.FieldCount; i++)
                {
                    cboFieldName.Items.Add(fields.get_Field(i).Name);
                    cboX.Items.Add(fields.get_Field(i).Name);
                    cboY.Items.Add(fields.get_Field(i).Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }
        }

        private void rbtRandom_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtRandom.Checked == false)
                rbtConstant.Checked = true;
        }

        private void rbtConstant_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtConstant.Checked == false)
                rbtRandom.Checked = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.lblStatus.Text = "Processing:";
                pfrmProgress.Show();

                int intProgress = 0;


                REngine pEngine = mForm.pEngine;

                //Declare constants
                double dblDistError = Convert.ToDouble(txtDistance.Text);
                double dblDirection = Convert.ToDouble(txtDirections.Text);
                int intNSimulation = Convert.ToInt32(txtNSimulation.Text);
                string strX = cboX.Text;
                string strY = cboY.Text;
                mForm.strValue = cboFieldName.Text;

                //Load Source Layer
                string strLayerName = cboSourceLayer.Text;

                int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
                ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

                IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                IFeatureClass pFClass = pFLayer.FeatureClass;
                int nFeature = pFClass.FeatureCount(null);

                IFeatureCursor pFCursor = pFLayer.Search(null, true);
                IFeature pFeature = pFCursor.NextFeature();

                //Load Target Layer
                mForm.strTargetLayerName = cboTargetLayer.Text;

                int intTargetIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, mForm.strTargetLayerName);
                ILayer pTargetLayer = mForm.axMapControl1.get_Layer(intTargetIndex);

                IFeatureLayer pFTargetLayer = pTargetLayer as IFeatureLayer;
                IFeatureClass pFTargetClass = pFTargetLayer.FeatureClass;
                int nTargetFeature = pFTargetClass.FeatureCount(null);

                //Store X, Y and Value to array
                double[] vecValue = new double[nFeature];
                double[,] arrXY = new double[nFeature, 2];

                int intXIdx = pFClass.FindField(strX);
                int intYIdx = pFClass.FindField(strY);
                int intVIdx = pFClass.FindField(mForm.strValue);

                int i = 0;
                while (pFeature != null)
                {

                    vecValue[i] = Convert.ToDouble(pFeature.get_Value(intVIdx));
                    arrXY[i, 0] = Convert.ToDouble(pFeature.get_Value(intXIdx));
                    arrXY[i, 1] = Convert.ToDouble(pFeature.get_Value(intYIdx));
                    i++;
                    pFeature = pFCursor.NextFeature();
                }
                pFCursor.Flush();

                ////Plot command for R
                //StringBuilder plotCommmand = new StringBuilder();

                //Get the file path and name to create spatial weight matrix
                string strNameR = pSnippet.FilePathinRfromLayer(pFTargetLayer);

                if (strNameR == null)
                    return;

               
                //Load Library and Data
                pEngine.Evaluate("library(maptools);library(foreign);library(sp);library(circular);");
                pEngine.Evaluate("ct.shp <- readShapePoly('" + strNameR + "')");

                //Create Shapepointdataframe in R
                NumericMatrix nmXY = pEngine.CreateNumericMatrix(arrXY);
                pEngine.SetSymbol("xy.coord.old", nmXY);
                NumericVector nvValue = pEngine.CreateNumericVector(vecValue);
                pEngine.SetSymbol("LEAD", nvValue);
                pEngine.Evaluate("v.bll <- as.data.frame(LEAD)");
                pEngine.Evaluate("old.points <- SpatialPointsDataFrame(xy.coord.old, v.bll)");

                string strOverMethod = "";

                if (cboStat.Text == "Mean")
                    strOverMethod = ", fn=mean";


                //Save Original value
                pEngine.Evaluate("n.obs <- nrow(v.bll)");
                pEngine.Evaluate("ori.value <- as.matrix(over(ct.shp, old.points" + strOverMethod + "))");
                NumericVector nvOriValue = pEngine.Evaluate("ori.value").AsNumeric();
                mForm.arrOrivalue = new double[nTargetFeature];
                nvOriValue.CopyTo(mForm.arrOrivalue, nTargetFeature);

                //Create Matrix to store simulation results
                pEngine.Evaluate("results.matrix <- matrix(NA, nrow=nrow(ct.shp), ncol=" + intNSimulation.ToString() + ")");



                //Simulation
                for (int j = 1; j <= intNSimulation; j++)
                {
                    //Progress bar
                    intProgress = j * 100 / intNSimulation;
                    pfrmProgress.pgbProgress.Value = intProgress;
                    //pfrmProgress.pgbProgress.Refresh();
                    pfrmProgress.lblStatus.Text = "Processing (" + intProgress.ToString() + "%)";
                    //pfrmProgress.lblStatus.Refresh();
                    pfrmProgress.Invalidate();

                    //Create Direction from vonmises distribution
                    pEngine.Evaluate("dir.new <- as.numeric(rvonmises(n.obs, mu=circular(" + txtDirections.Text + ", units='degrees'), kappa=" + txtConcetration.Text + "))");

                    //Add distance error
                    if (rbtRandom.Checked == true)
                        pEngine.Evaluate("dist.error <- runif(n.obs, 0, " + txtDistance.Text + ")");
                    else
                        pEngine.Evaluate("dist.error <- " + txtDistance.Text);

                    //Create New points and save to SpatialPointsDataFrame
                    pEngine.Evaluate("x.new <- xy.coord.old[,1] + cos(dir.new)*dist.error; y.new <- xy.coord.old[,2]+ sin(dir.new)*dist.error");
                    pEngine.Evaluate("xy.coord <- cbind(x.new, y.new)");
                    pEngine.Evaluate("new.points <- SpatialPointsDataFrame(xy.coord, v.bll)");

                    //Save results <- have to insert options for summary statistics, (only possilbe by Mean) 1/29/15
                    pEngine.Evaluate("results.matrix[," + j.ToString() + "] <- as.matrix(over(ct.shp, new.points" + strOverMethod + "))");

                }
                //Save Simulation results to Array
                NumericMatrix nmResults = pEngine.Evaluate("results.matrix").AsNumericMatrix();
                mForm.arrSimuResults = new double[nTargetFeature, intNSimulation];
                nmResults.CopyTo(mForm.arrSimuResults, nTargetFeature, intNSimulation);

                //Create matrix to save summary results(mean, var, mean/var)
                pfrmProgress.lblStatus.Text = "Creating Summary";
                pfrmProgress.Invalidate();
                pEngine.Evaluate("sum.mat <- matrix(NA,nrow=nrow(ct.shp), ncol=6)");

                //Calculate mean and variance of simulation
                for (int j = 1; j <= nTargetFeature; j++)
                {
                    pEngine.Evaluate("sum.mat[" + j.ToString() + ",1] <- mean(results.matrix[" + j.ToString() + ",])");
                    pEngine.Evaluate("sum.mat[" + j.ToString() + ",2] <- var(results.matrix[" + j.ToString() + ",])");
                    pEngine.Evaluate("sum.mat[" + j.ToString() + ",5] <- min(results.matrix[" + j.ToString() + ",])");
                    pEngine.Evaluate("sum.mat[" + j.ToString() + ",6] <- max(results.matrix[" + j.ToString() + ",])");

                    pEngine.Evaluate("diff.ori <- (results.matrix[" + j.ToString() + ",]- ori.value[" + j.ToString() + "])^2"); //Needs to be changed
                    pEngine.Evaluate("sum.mat[" + j.ToString() + ",4] <- sum(diff.ori)/" + intNSimulation.ToString());
                }
                //Calculate mean/variance
                pEngine.Evaluate("sum.mat[,3] <- sum.mat[,2]/sum.mat[,1]");

                //Save summary results to array
                NumericMatrix nmSummary = pEngine.Evaluate("sum.mat").AsNumericMatrix();
                arrSummary = new double[nTargetFeature, 6];
                nmSummary.CopyTo(arrSummary, nTargetFeature, 6);

                //Remove all memory 
                pEngine.Evaluate("rm(list=ls(all=TRUE))");

                //Enable to the histogram identify tool for simulation results
                if (mForm.arrSimuResults != null)
                    chkEnable.Enabled = true;



                //Add Summary data to Target feature Class

                //Create filed to store results
                if (pFTargetClass.FindField("Ori_Mean") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "Ori_Mean";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                if (pFTargetClass.FindField("Mean_val") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "Mean_val";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                if (pFTargetClass.FindField("Var_val") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "Var_val";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                if (pFTargetClass.FindField("Mean_Var") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "Mean_Var";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                if (pFTargetClass.FindField("Var_Ori") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "Var_Ori";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                if (pFTargetClass.FindField("MIN") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "MIN";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                if (pFTargetClass.FindField("MAX") == -1)
                {
                    IFieldEdit addField = new FieldClass();
                    addField.Name_2 = "MAX";
                    addField.Type_2 = esriFieldType.esriFieldTypeDouble;
                    pFTargetClass.AddField(addField);
                }
                //Save summary results to Target shapefile
                int intOriMeanIdx = pFTargetClass.FindField("Ori_Mean");
                int intMeanIdx = pFTargetClass.FindField("Mean_val");
                int intVarIdx = pFTargetClass.FindField("Var_val");
                int intMVIdx = pFTargetClass.FindField("Mean_Var");
                int intVOIdx = pFTargetClass.FindField("Var_Ori");
                int intMaxIdx = pFTargetClass.FindField("MAX");
                int intMinIdx = pFTargetClass.FindField("MIN");

                IFeatureCursor pTFUCursor = pFTargetClass.Update(null, false);
                IFeature pTFeature = pTFUCursor.NextFeature();

                i = 0;
                while (pTFeature != null)
                {
                    pTFeature.set_Value(intOriMeanIdx, mForm.arrOrivalue[i]);
                    pTFeature.set_Value(intMeanIdx, arrSummary[i, 0]);
                    pTFeature.set_Value(intVarIdx, arrSummary[i, 1]);
                    pTFeature.set_Value(intMVIdx, arrSummary[i, 2]);
                    pTFeature.set_Value(intVOIdx, arrSummary[i, 3]);
                    pTFeature.set_Value(intMinIdx, arrSummary[i, 4]);
                    pTFeature.set_Value(intMaxIdx, arrSummary[i, 5]);
                    pTFeature.Store();

                    i++;
                    pTFeature = pTFUCursor.NextFeature();
                }



                pfrmProgress.Close();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Handle.ToString() + " Error:" + ex.Message);
                return;
            }

        }

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnable.Checked == true && chkEnable.Enabled == true)
                mForm.blnEnableHistUncern = true;
            else
                mForm.blnEnableHistUncern = false;
        }

        private void cboStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStat.Text == "Counts")
                cboFieldName.Enabled = false;
        }
    }
}
