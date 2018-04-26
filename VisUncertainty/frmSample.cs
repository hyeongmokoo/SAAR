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

using RDotNet;
using RDotNet.NativeLibrary;

namespace VisUncertainty
{
    public partial class frmSample : Form
    {
        private MainForm mForm;
        private IActiveView pActiveView;

        public frmSample()
        {
            InitializeComponent();
            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            pActiveView = mForm.axMapControl1.ActiveView;

            for (int i = 0; i < mForm.axMapControl1.LayerCount; i++)
            {
                cboTargetLayer.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
                cboStudyArea.Items.Add(mForm.axMapControl1.get_Layer(i).Name);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboTargetLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsSnippet pSnippet = new clsSnippet();
            string strLayerName = cboTargetLayer.Text;

            int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strLayerName);
            ILayer pLayer = mForm.axMapControl1.get_Layer(intLIndex);

            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            ESRI.ArcGIS.Geodatabase.IFeatureClass pFClass = pFLayer.FeatureClass;

            IFields fields = pFClass.Fields;

            cboXField.Items.Clear();
            cboYField.Items.Clear();

            for (int i = 0; i < fields.FieldCount; i++)
            {
                cboXField.Items.Add(fields.get_Field(i).Name);
                cboYField.Items.Add(fields.get_Field(i).Name);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            REngine pEngine = mForm.pEngine;
            clsSnippet pSnippet = new clsSnippet();

            string strBLLayerName = cboTargetLayer.Text;
            string strXFieldName = cboXField.Text;
            string strYFieldName = cboYField.Text;
            string strSALayerName = cboStudyArea.Text;

            int intLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strSALayerName);
            int intBLIndex = pSnippet.GetIndexNumberFromLayerName(pActiveView, strBLLayerName);
            
            ILayer pSALayer = mForm.axMapControl1.get_Layer(intLIndex);
            IFeatureLayer pSAFLayer = (IFeatureLayer)pSALayer;
            IFeatureClass pSAFClass = pSAFLayer.FeatureClass;

            ILayer pBLLayer = mForm.axMapControl1.get_Layer(intBLIndex);
            IFeatureLayer pBLFLayer = (IFeatureLayer)pBLLayer;
            IGeoFeatureLayer pBLGeoFLayer = (IGeoFeatureLayer)pBLFLayer;
            IAttributeTable pBLAttTable = (IAttributeTable)pBLGeoFLayer;
            ITable pBLTable = (ITable)pBLAttTable;

            //Get File Name and Path
            IDataLayer pSADLayer = (IDataLayer)pSALayer;
            IDatasetName pSADSName = (IDatasetName)pSADLayer.DataSourceName;
            string strSAFCName = pSADSName.Name;
            string strSAFCPath = pSADSName.WorkspaceName.PathName;
            string strSAFullName = strSAFCPath + "\\" + strSAFCName+".shp";
            IDataLayer pBLDLayer = (IDataLayer)pBLLayer;
            IDatasetName pBLDSName = (IDatasetName)pBLDLayer.DataSourceName;
            string strBLFCName = pBLDSName.Name;
            string strBLFCPath = pBLDSName.WorkspaceName.PathName;
            string strBLFullName = strBLFCPath + "\\" + strBLFCName + "1.dbf"; ;

            mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            pActiveView = mForm.axMapControl1.ActiveView;
            IMap pMap = pActiveView.FocusMap;

            int intXFieldIdx = pBLFLayer.FeatureClass.Fields.FindField(strXFieldName)-1; //-1 to use for R idx
            int intYFieldIdx = pBLFLayer.FeatureClass.Fields.FindField(strYFieldName)-1;

            
            
            //StringBuilder plotCommmand = new StringBuilder();
            //plotCommmand.Append("library(circular);library(maptools);library(spdep);library(foreign);");
            //plotCommmand.Append("city.shp <- readShapePoly(" + strSAFullName + ");");
            //plotCommmand.Append("polycoords <- as.matrix(city.shp@polygons[[1]]@Polygons[[1]]@coords);");
            //plotCommmand.Append("bll.df <- as.data.frame(read.dbf(" + strBLFullName + ");");
            //plotCommmand.Append("del.row <- which(point.in.polygon(bll.df[,6],bll.df[,7],polycoords[,1],polycoords[,2])==0);");
            //plotCommmand.Append("hist(bll.df[,6])");
            //pEngine.Evaluate(plotCommmand.ToString());

            pEngine.Evaluate(@"library(circular);library(maptools);library(spdep);library(foreign)");
            pEngine.Evaluate(@"city.shp <- readShapePoly(" + strSAFullName + ")");
            //DataFrame pGV =pEngine.Evaluate(@"bll.df <- as.data.frame(read.dbf(" + strBLFullName + ")").AsDataFrame();
            //DataFrame pBLLdf = pEngine.
            pEngine.Evaluate("bll.df <- as.data.frame(read.dbf(\"" + strBLFullName + "\")");
            pEngine.Evaluate("hist(bll.df[,6])");



            // Get the table named XYSample.txt
            IStandaloneTableCollection pStTabCol;
            IStandaloneTable pStandaloneTable;
            ITable pTable = null;
            pStTabCol = (IStandaloneTableCollection)pMap;
            for (int intCount = 0; intCount < pStTabCol.StandaloneTableCount; intCount++)
            {
                pStandaloneTable = (IStandaloneTable)pStTabCol.get_StandaloneTable(intCount);
                if (pStandaloneTable.Name == "syracuse-bll.csv")
                {
                    pTable = pStandaloneTable.Table;
                    break;
                }
            }
            if (pTable == null)
            {
                MessageBox.Show("The table was not found");
                return;
            }

            // Get the table name object
            IDataset pDataSet;
            IName pTableName;
            pDataSet = (IDataset)pTable;
            pTableName = pDataSet.FullName;

            // Specify the X and Y fields
            IXYEvent2FieldsProperties pXYEvent2FieldsProperties = new XYEvent2FieldsPropertiesClass();
            pXYEvent2FieldsProperties.XFieldName = cboXField.Text;
            pXYEvent2FieldsProperties.YFieldName = cboYField.Text;
            pXYEvent2FieldsProperties.ZFieldName = "";

            // Create the XY name object and set it's properties 
            IXYEventSourceName pXYEventSourceName = new XYEventSourceNameClass();
            pXYEventSourceName.EventProperties = pXYEvent2FieldsProperties;
            IGeoDataset pGeoDataset = (IGeoDataset)pBLDLayer;
            pXYEventSourceName.SpatialReference = pGeoDataset.SpatialReference;
            //pXYEventSourceName.EventTableName = pTableName;
            
            IName pXYName = (IName)pXYEventSourceName;
            IXYEventSource pXYEventSource = (IXYEventSource)pXYName.Open();

            // Create a new Map Layer 
            IFeatureLayer pFLayer = new FeatureLayerClass();
            pFLayer.FeatureClass = (IFeatureClass) pXYEventSource;
            pFLayer.Name = "Sample XY Event layer";

            // Add the layer extension (this is done so that when you edit
            //   the layer's Source properties and click the Set Data Source
            //   button, the Add XY Events Dialog appears)
            //ILayerExtensions pLayerExt;
            //IFeatureLayerSourcePageExtension pRESPageExt = new XYDataSourcePageExtensionClass();
            //pLayerExt = (ILayerExtensions) pFLayer;
            //pLayerExt.AddExtension(pRESPageExt);

            pMap.AddLayer(pFLayer);



        }
    }
}
