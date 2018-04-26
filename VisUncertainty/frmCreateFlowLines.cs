using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Carto;
using RDotNet;
using ESRI.ArcGIS.Geometry;
using System.Diagnostics;

namespace VisUncertainty
{
    public partial class frmCreateFlowLines : Form
    {
        private clsSnippet m_pSnippet;
        private ITable m_tblFlow;
        private MainForm m_pForm;
        private IActiveView m_pActiveView;
        private IFeatureLayer m_pFLayer;
        private IFeatureClass m_pFClass;
        //private REngine m_pEngine;

        public frmCreateFlowLines()
        {
            try
            {
                InitializeComponent();
                m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
                m_pActiveView = m_pForm.axMapControl1.ActiveView;
                m_pSnippet = new clsSnippet();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
        private void ChangeLabels()
        {
            try
            {
                string strSPInfo = cboSpatialInfo.Text;

                if (strSPInfo == "From ShapeFile")
                {
                    lbl1.Text = "Reference Shape File:";
                    lbl2.Text = "Join Field:";
                    lbl3.Text = "Origin ID:";
                    lbl4.Text = "Destination ID:";
                }
                else if (strSPInfo == "From Table")
                {
                    lbl1.Text = "Origin X Coord:";
                    lbl2.Text = "Origin Y Coord:";
                    lbl3.Text = "Desination X Coord:";
                    lbl4.Text = "Destination Y Coord:";
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void UpdateSpatialInfoElements(string strSPInfo)
        {
            string strTableName = txtFlowFile.Text;
            string strSourceFolder = null;
            string strSourceFile = null;

            if (strTableName == "")
            {
                return;
            }
            else
            {
                strSourceFolder = System.IO.Path.GetDirectoryName(strTableName);
                strSourceFile = System.IO.Path.GetFileName(strTableName);
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IWorkspace ws = workspaceFactory.OpenFromFile(strSourceFolder, 0);
                IFeatureWorkspace fws = ws as IFeatureWorkspace;

                m_tblFlow = fws.OpenTable(strSourceFile);

                IFields fields = m_tblFlow.Fields;

                if (strSPInfo == "From ShapeFile")
                {
                    cbo1.Items.Clear();
                    cbo2.Items.Clear();
                    cbo3.Items.Clear();
                    cbo4.Items.Clear();

                    for (int i = 0; i < m_pForm.axMapControl1.LayerCount; i++)
                    {
                        cbo1.Items.Add(m_pForm.axMapControl1.get_Layer(i).Name);
                    }

                    for (int i = 0; i < fields.FieldCount; i++)
                    {
                        cbo3.Items.Add(fields.get_Field(i).Name);
                        cbo4.Items.Add(fields.get_Field(i).Name);
                    }
                }
                else if (strSPInfo == "From Table")
                {
                    cbo1.Items.Clear();
                    cbo2.Items.Clear();
                    cbo3.Items.Clear();
                    cbo4.Items.Clear();

                    for (int i = 0; i < fields.FieldCount; i++)
                    {
                        if (m_pSnippet.FindNumberFieldType(fields.get_Field(i)))
                        {
                            cbo1.Items.Add(fields.get_Field(i).Name);
                            cbo2.Items.Add(fields.get_Field(i).Name);
                            cbo3.Items.Add(fields.get_Field(i).Name);
                            cbo4.Items.Add(fields.get_Field(i).Name);
                        }
                    }
                }
            }
        }

        private void AddFldList()
        {
            if (txtFlowFile.Text == "")
                return;
            else if (m_tblFlow == null)
                return;

            IFields fields = m_tblFlow.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                clistFields.Items.Add(fields.get_Field(i).Name);
            }

        }
        private void cboSpatialInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeLabels();
            UpdateSpatialInfoElements(cboSpatialInfo.Text);
        }

        private void btnOpenFlow_Click(object sender, EventArgs e)
        {
            if (ofdOpenDBF.ShowDialog() == DialogResult.OK)
                txtFlowFile.Text = string.Concat(ofdOpenDBF.FileName);
        }

        private void txtFlowFile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeLabels();
                UpdateSpatialInfoElements(cboSpatialInfo.Text);
                AddFldList();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sfdSaveShp.ShowDialog() == DialogResult.OK)
                txtOutput.Text = string.Concat(sfdSaveShp.FileName);
        }

        private void cbo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboSpatialInfo.Text == "From ShapeFile")
                {
                    cbo2.Items.Clear();

                    if (cbo1.Text != "")
                    {
                        string strLayerName = cbo1.Text;

                        int intLIndex = m_pSnippet.GetIndexNumberFromLayerName(m_pActiveView, strLayerName);
                        ILayer pLayer = m_pForm.axMapControl1.get_Layer(intLIndex);

                        m_pFLayer = pLayer as IFeatureLayer;
                        m_pFClass = m_pFLayer.FeatureClass;

                        IFields fields = m_pFClass.Fields;
                        for (int i = 0; i < fields.FieldCount; i++)
                        {
                            cbo2.Items.Add(fields.get_Field(i).Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                frmProgress pfrmProgress = new frmProgress();
                pfrmProgress.bgWorker.ReportProgress(0);
                pfrmProgress.lblStatus.Text = "Create Flow Lines";
                pfrmProgress.Show();

                string strOutput = txtOutput.Text;
                string strOutputPath = System.IO.Path.GetDirectoryName(strOutput);
                string strOutputName = System.IO.Path.GetFileName(strOutput);
                ISpatialReference pSpatialRef = null;
                if (cboSpatialInfo.Text == "From ShapeFile")
                {
                    IGeoDataset geoDataset = m_pFClass as IGeoDataset;
                    pSpatialRef = geoDataset.SpatialReference;
                }

                //Create New FeatureClass
                IFeatureClass pFCOutput = CreateShapeFile(strOutputPath, strOutputName, pSpatialRef, esriGeometryType.esriGeometryPolyline);

                //Add Fld Ids to added in new FC
                int[] intAddFldIdsInTable = null;
                int intCheckedFldCnt = clistFields.CheckedItems.Count;
                if (intCheckedFldCnt > 0)
                {
                    intAddFldIdsInTable = new int[intCheckedFldCnt];
                    for (int i = 0; i < intCheckedFldCnt; i++)
                    {
                        intAddFldIdsInTable[i] = m_tblFlow.FindField((string)clistFields.CheckedItems[i]);
                    }
                }
                AddFldsFromTable(pFCOutput, m_tblFlow, intAddFldIdsInTable);
                //Get Ids from FeatureClass
                int[] intAddFldIdsInFC = null;

                if (intCheckedFldCnt > 0)
                {
                    intAddFldIdsInFC = new int[intCheckedFldCnt];
                    for (int i = 0; i < intCheckedFldCnt; i++)
                    {
                        intAddFldIdsInFC[i] = pFCOutput.FindField((string)clistFields.CheckedItems[i]);
                    }
                }
                var watch = Stopwatch.StartNew();


                if (cboSpatialInfo.Text == "From Table")
                {
                    //First Method, it takes some times, require better algorithm. 0714 HK
                    string strFromXFld = cbo1.Text;
                    string strFromYFld = cbo2.Text;
                    string strToXFld = cbo3.Text;
                    string strToYFld = cbo4.Text;

                    int intFromXIdx = m_tblFlow.FindField(strFromXFld);
                    int intFromYIdx = m_tblFlow.FindField(strFromYFld);
                    int intToXIdx = m_tblFlow.FindField(strToXFld);
                    int intToYIdx = m_tblFlow.FindField(strToYFld);

                    int intTotalFlowCnt = m_tblFlow.RowCount(null);
                    ICursor pCursor = m_tblFlow.Search(null, false);
                    IRow pRow = pCursor.NextRow();
                    //IPoint pFromPt = new ESRI.ArcGIS.Geometry.Point();
                    //IPoint pToPts = new ESRI.ArcGIS.Geometry.Point();
                    IPoint pFromPt = null;
                    IPoint pToPt = null;
                    double dblFromX = 0, dblFromY = 0, dblToX = 0, dblToY = 0;

                    IPolyline pPolyline = new PolylineClass();

                    int cnt = 0;
                    if (intCheckedFldCnt > 0)
                    {
                        while (pRow != null)
                        {
                            int intProgress = cnt * 100 / intTotalFlowCnt;
                            pfrmProgress.bgWorker.ReportProgress(intProgress);

                            dblFromX = Convert.ToDouble(pRow.get_Value(intFromXIdx));
                            dblFromY = Convert.ToDouble(pRow.get_Value(intFromYIdx));
                            dblToX = Convert.ToDouble(pRow.get_Value(intToXIdx));
                            dblToY = Convert.ToDouble(pRow.get_Value(intToYIdx));

                            pFromPt = new PointClass();
                            pToPt = new PointClass();
                            pFromPt.PutCoords(dblFromX, dblFromY);
                            pToPt.PutCoords(dblToX, dblToY);

                            pPolyline = new PolylineClass();
                            pPolyline.FromPoint = pFromPt;
                            pPolyline.ToPoint = pToPt;
                            IFeature pFeature = pFCOutput.CreateFeature();
                            pFeature.Shape = pPolyline;

                            for (int i = 0; i < intCheckedFldCnt; i++)
                                pFeature.set_Value(intAddFldIdsInFC[i], pRow.get_Value(intAddFldIdsInTable[i]));

                            pFeature.Store();
                            pRow = pCursor.NextRow();
                            cnt++;
                        }
                    }
                    else
                    {
                        int intProgress = cnt * 100 / intTotalFlowCnt;
                        pfrmProgress.bgWorker.ReportProgress(intProgress);

                        dblFromX = Convert.ToDouble(pRow.get_Value(intFromXIdx));
                        dblFromY = Convert.ToDouble(pRow.get_Value(intFromYIdx));
                        dblToX = Convert.ToDouble(pRow.get_Value(intToXIdx));
                        dblToY = Convert.ToDouble(pRow.get_Value(intToYIdx));

                        pFromPt = new PointClass();
                        pToPt = new PointClass();
                        pFromPt.PutCoords(dblFromX, dblFromY);
                        pToPt.PutCoords(dblToX, dblToY);

                        pPolyline = new PolylineClass();
                        pPolyline.FromPoint = pFromPt;
                        pPolyline.ToPoint = pToPt;
                        IFeature pFeature = pFCOutput.CreateFeature();
                        pFeature.Shape = pPolyline;

                        pFeature.Store();
                        pRow = pCursor.NextRow();
                        cnt++;
                    }
                    #region deprecated 101917 HK
                    //    //Second Method, it takes similar calculation time. 0714 HK
                    //    string strFromXFld = cbo1.Text;
                    //    string strFromYFld = cbo2.Text;
                    //    string strToXFld = cbo3.Text;
                    //    string strToYFld = cbo4.Text;

                    //    int intFromXIdx = m_tblFlow.FindField(strFromXFld);
                    //    int intFromYIdx = m_tblFlow.FindField(strFromYFld);
                    //    int intToXIdx = m_tblFlow.FindField(strToXFld);
                    //    int intToYIdx = m_tblFlow.FindField(strToYFld);

                    //    int intTotalFlowCnt = m_tblFlow.RowCount(null);
                    //    ICursor pCursor = m_tblFlow.Search(null, false);
                    //    IRow pRow = pCursor.NextRow();
                    //    //IPoint pFromPt = new ESRI.ArcGIS.Geometry.Point();
                    //    //IPoint pToPts = new ESRI.ArcGIS.Geometry.Point();
                    //    IPoint pFromPt = null;
                    //    IPoint pToPt = null;

                    //    IPolyline pPolyline = new PolylineClass();

                    //    double[][] dblValues = new double[intTotalFlowCnt][];
                    //    int cnt = 0;
                    //    while (pRow != null)
                    //    {
                    //        pfrmProgress.lblStatus.Text = "Collect data";
                    //        int intProgress = cnt * 100 / intTotalFlowCnt;
                    //        pfrmProgress.bgWorker.ReportProgress(intProgress);


                    //        dblValues[cnt] = new double[4 + intCheckedFldCnt];

                    //        dblValues[cnt][0] = Convert.ToDouble(pRow.get_Value(intFromXIdx));
                    //        dblValues[cnt][1] = Convert.ToDouble(pRow.get_Value(intFromYIdx));
                    //        dblValues[cnt][2] = Convert.ToDouble(pRow.get_Value(intToXIdx));
                    //        dblValues[cnt][3] = Convert.ToDouble(pRow.get_Value(intToYIdx));

                    //        if (intCheckedFldCnt > 0)
                    //            for (int i = 0; i < intCheckedFldCnt; i++)
                    //                dblValues[cnt][3+i] = Convert.ToDouble(pRow.get_Value(intAddFldIdsInTable[i]));

                    //        pRow = pCursor.NextRow();
                    //        cnt++;
                    //    }
                    //    for(int j = 0; j<intTotalFlowCnt; j++)
                    //    {
                    //        pfrmProgress.lblStatus.Text = "Create Line";
                    //        int intProgress = j * 100 / intTotalFlowCnt;
                    //        pfrmProgress.bgWorker.ReportProgress(intProgress);

                    //        pFromPt = new PointClass();
                    //        pToPt = new PointClass();
                    //        pFromPt.PutCoords(dblValues[j][0], dblValues[j][1]);
                    //        pToPt.PutCoords(dblValues[j][2], dblValues[j][3]);

                    //        pPolyline = new PolylineClass();
                    //        pPolyline.FromPoint = pFromPt;
                    //        pPolyline.ToPoint = pToPt;
                    //        IFeature pFeature = pFCOutput.CreateFeature();
                    //        pFeature.Shape = pPolyline;
                    //        if (intCheckedFldCnt > 0)
                    //            for (int i = 0; i < intCheckedFldCnt; i++)
                    //                pFeature.set_Value(intAddFldIdsInFC[i], dblValues[j][3+i]);

                    //        pFeature.Store();
                    //    }
                    #endregion

                }
                else if (cboSpatialInfo.Text == "From ShapeFile")
                {
                    string strRefSHPName = cbo1.Text;
                    string strJoinFldName = cbo2.Text;
                    string strOIDFldName = cbo3.Text;
                    string strDIDFldName = cbo4.Text;

                    int intJoinFldIdx = m_pFClass.FindField(strJoinFldName);
                    int intOIDIdx = m_tblFlow.FindField(strOIDFldName);
                    int intDIDIdx = m_tblFlow.FindField(strDIDFldName);

                    int intFeatureCount = m_pFClass.FeatureCount(null);

                    IFeatureCursor pFCursor = m_pFClass.Search(null, true);
                    IFeature pFeature = pFCursor.NextFeature();

                    List<RefSpatialInfo> lstRefInfo = new List<RefSpatialInfo>();

                    IArea pArea;
                    IPoint pPoint;

                    int cnt = 0;
                    while (pFeature != null)
                    {
                        pfrmProgress.lblStatus.Text = "Getting Spatial Information from Shape file";
                        int intProgress = cnt * 100 / intFeatureCount;
                        pfrmProgress.bgWorker.ReportProgress(intProgress);

                        RefSpatialInfo RefInfo = new RefSpatialInfo();
                        RefInfo.JoinID = pFeature.get_Value(intJoinFldIdx).ToString();

                        if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            pArea = (IArea)pFeature.Shape;
                            RefInfo.XCoor = pArea.Centroid.X;
                            RefInfo.YCoor = pArea.Centroid.Y;
                        }
                        else if (m_pFClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        {
                            pPoint = (IPoint)pFeature.Shape;
                            RefInfo.XCoor = pPoint.X;
                            RefInfo.YCoor = pPoint.Y;
                        }

                        lstRefInfo.Add(RefInfo);

                        cnt++;
                        pFeature = pFCursor.NextFeature();
                    }
                    pFCursor.Flush();

                    int intTotalFlowCnt = m_tblFlow.RowCount(null);
                    ICursor pCursor = m_tblFlow.Search(null, false);
                    IRow pRow = pCursor.NextRow();

                    IPoint pFromPt = null;
                    IPoint pToPt = null;
                    double dblFromX = 0, dblFromY = 0, dblToX = 0, dblToY = 0;

                    IPolyline pPolyline = new PolylineClass();

                    cnt = 0;
                    int intNonMatchedFlow = 0;
                    while (pRow != null)
                    {
                        pfrmProgress.lblStatus.Text = "Create Flow Lines";
                        int intProgress = cnt * 100 / intTotalFlowCnt;
                        pfrmProgress.bgWorker.ReportProgress(intProgress);

                        string strOID = pRow.get_Value(intOIDIdx).ToString();
                        string strDID = pRow.get_Value(intDIDIdx).ToString();

                        RefSpatialInfo RefOInfo = lstRefInfo.Find(x => x.JoinID == strOID);
                        RefSpatialInfo RefDInfo = lstRefInfo.Find(x => x.JoinID == strDID);

                        if (RefOInfo == null || RefDInfo == null)
                        {
                            intNonMatchedFlow++;
                            pRow = pCursor.NextRow();
                            cnt++;
                        }
                        else
                        {

                            dblFromX = RefOInfo.XCoor;
                            dblFromY = RefOInfo.YCoor;
                            dblToX = RefDInfo.XCoor;
                            dblToY = RefDInfo.YCoor;

                            pFromPt = new PointClass();
                            pToPt = new PointClass();
                            pFromPt.PutCoords(dblFromX, dblFromY);
                            pToPt.PutCoords(dblToX, dblToY);

                            pPolyline = new PolylineClass();
                            pPolyline.FromPoint = pFromPt;
                            pPolyline.ToPoint = pToPt;
                            IFeature pNewFeature = pFCOutput.CreateFeature();
                            pNewFeature.Shape = pPolyline;

                            if (intCheckedFldCnt > 0)
                            {
                                for (int i = 0; i < intCheckedFldCnt; i++)
                                    pNewFeature.set_Value(intAddFldIdsInFC[i], pRow.get_Value(intAddFldIdsInTable[i]));
                            }

                            pNewFeature.Store();
                            pRow = pCursor.NextRow();
                            cnt++;
                        }
                    }
                    //MessageBox.Show(intNonMatchedFlow.ToString()+" flow lines are not matched with the reference file"); //Check the message. 080516 HK
                }
                pfrmProgress.Close();

                watch.Stop();
                double dblTime = watch.ElapsedMilliseconds;
                MessageBox.Show("Run-time: " + dblTime.ToString());

                DialogResult dialogResult = MessageBox.Show("Do you want to add the output data to the map?", "Add Data", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    IFeatureLayer pFeaturelayer = new FeatureLayer();
                    pFeaturelayer.Name = pFCOutput.AliasName;
                    pFeaturelayer.FeatureClass = pFCOutput;
                    m_pActiveView.FocusMap.AddLayer((ILayer)pFeaturelayer);
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void AddFldsFromTable(IFeatureClass pFClass, ITable pTable, int[] FldIds)
        {
            try
            {
                int intNewFldCnt = FldIds.Length;

                for (int i = 0; i < intNewFldCnt; i++)
                {
                    IField pField = new FieldClass();
                    IFieldEdit pFieldEdit = (IFieldEdit)pField;
                    pFieldEdit.Name_2 = pTable.Fields.Field[FldIds[i]].Name;
                    pFieldEdit.Type_2 = pTable.Fields.Field[FldIds[i]].Type;
                    pFClass.AddField(pField);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to add a new field.");
                return;
            }
        }

        private IFeatureClass CreateShapeFile(string strOutFileSource, string strOutFileName, ISpatialReference spatialReference, esriGeometryType pGeometryType)
        {
            try
            {
                //Setting Output Data
                IWorkspaceFactory wsfOutput = new ShapefileWorkspaceFactoryClass();
                IWorkspace wsOutput = wsfOutput.OpenFromFile(strOutFileSource, 0);
                IFeatureWorkspace fwOutput = wsOutput as IFeatureWorkspace;

                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;

                IField oidField = new FieldClass();
                IFieldEdit oidFieldEdit = (IFieldEdit)oidField;
                oidFieldEdit.Name_2 = "OID";
                oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                pFieldsEdit.AddField(oidField);

                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = pGeometryType;

                if (spatialReference != null)
                    geometryDefEdit.SpatialReference_2 = spatialReference;

                IField geometryField = new FieldClass();
                IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
                geometryFieldEdit.Name_2 = "Shape";
                geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                geometryFieldEdit.GeometryDef_2 = geometryDef;
                pFieldsEdit.AddField(geometryField);

                IFieldChecker fieldChecker = new FieldCheckerClass();
                IEnumFieldError enumFieldError = null;
                IFields validatedFields = null;
                fieldChecker.ValidateWorkspace = (IWorkspace)fwOutput;
                fieldChecker.Validate(pFields, out enumFieldError, out validatedFields);

                IFeatureClass fcOutput = fwOutput.CreateFeatureClass(strOutFileName, validatedFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                return fcOutput;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to create a new shapefile.");
                return null;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clistFields.Items.Count; i++)
                clistFields.SetItemChecked(i, true);
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clistFields.Items.Count; i++)
                clistFields.SetItemChecked(i, false);
        }
        class RefSpatialInfo
        {
            public string JoinID { get; set; }
            public double XCoor { get; set; }
            public double YCoor { get; set; }
        }
    }
}
