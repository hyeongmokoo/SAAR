using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;


namespace VisUncertainty
{
    public partial class frmGeocoding : Form
    {
        public frmGeocoding()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            frmProgress pfrmProgress = new frmProgress();
            pfrmProgress.Show();
            pfrmProgress.lblStatus.Text = "Collecting Information";
            int intProgress = 0;
            pfrmProgress.bgWorker.ReportProgress(intProgress);

            //Get Sample Table setting
            string strTableName = txtSampleTable.Text;
            if (strTableName == "")
            {
                return;
            }
            string strSourceFolder = System.IO.Path.GetDirectoryName(strTableName);
            string strSourceFile = System.IO.Path.GetFileName(strTableName);
            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace workspace = workspaceFactory.OpenFromFile(strSourceFolder, 0);
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureClass pFClass = featureWorkspace.OpenFeatureClass(strSourceFile);
            //string strXNewFld = "NewRefX";
            string strXNewFld = "NewRefX1";
            int intXNewFldIdx = pFClass.FindField(strXNewFld);
            if (intXNewFldIdx == -1)
            {
                AddFld(pFClass, strXNewFld, esriFieldType.esriFieldTypeDouble);
                intXNewFldIdx = pFClass.FindField(strXNewFld);
            }

            //string strYNewFld = "NewRefY";
            string strYNewFld = "NewRefY1";
            int intYNewFldIdx = pFClass.FindField(strYNewFld);
            if (intYNewFldIdx == -1)
            {
                AddFld(pFClass, strYNewFld, esriFieldType.esriFieldTypeDouble);
                intYNewFldIdx = pFClass.FindField(strYNewFld);
            }

            string strFlagFld = "Flag";
            int intFlagFldIdx = pFClass.FindField(strFlagFld);
            if (intFlagFldIdx == -1)
            {
                AddFld(pFClass, strFlagFld, esriFieldType.esriFieldTypeSmallInteger);
                intFlagFldIdx = pFClass.FindField(strFlagFld);
            }

            //Get Reference Street file
            string strRefStreet = txtRefStreet.Text;
            if (strRefStreet == "")
                return;
            string strRefSource = System.IO.Path.GetDirectoryName(strRefStreet);
            string strRefName = System.IO.Path.GetFileName(strRefStreet);
            IWorkspace pwsRef = workspaceFactory.OpenFromFile(strRefSource, 0);
            IFeatureWorkspace pfwsRef = pwsRef as IFeatureWorkspace;
            IFeatureClass pRefFClass = pfwsRef.OpenFeatureClass(strRefName);

            //Get Reference Street File
            //string[] addFldNames = new string[] { "JOINID", "Length", "RLMean", "LLMean", "CompassA", "DirMean", "AveX", "AveY", "AveLen" };
            List<RefStreetVertices> lstRefStreetInfo = new List<RefStreetVertices>();

            //RefStreetInfo pRefStreetInfo = new RefStreetInfo();
            //int intInforCnt = addFldNames.Length;
            //int[] intInfoIdxs = new int[intInforCnt];

            //for (int i = 0; i < intInforCnt; i++)
            //    intInfoIdxs[i] = pRefFClass.FindField(addFldNames[i]);

            int intJoinIDIdx = pRefFClass.FindField("JOINID");
            //int intRefStreetCnt = pRefFClass.FeatureCount(null);
            IFeatureCursor pRefFCursor = pRefFClass.Search(null, false);
            IFeature pRefFeature = pRefFCursor.NextFeature();

            while (pRefFeature != null)
            {
                RefStreetVertices pRefStreetInfo = new RefStreetVertices();
                pRefStreetInfo.JoinID = pRefFeature.get_Value(intJoinIDIdx).ToString();
                IPolyline pPolyLine = (IPolyline)pRefFeature.ShapeCopy;
                IPointCollection pPointCollection = (IPointCollection)pPolyLine;
                pRefStreetInfo.pointCollection = pPointCollection;

                lstRefStreetInfo.Add(pRefStreetInfo);
                pRefFeature = pRefFCursor.NextFeature();
            }

            int intRefXIdx = pFClass.FindField(cboXRef.Text);
            int intRefYIdx = pFClass.FindField(cboYRef.Text);
            string strRefID = "Ref_ID";
            int intRefIDIdx = pFClass.FindField(strRefID);
            IQueryFilter pQFilter = new QueryFilterClass();
            //pQFilter.WhereClause = "Status = 'M'";
            pQFilter.WhereClause = "Score > 0";

            IFeatureCursor pFCursor = pFClass.Update(pQFilter, true);
            IFeature pFeature = pFCursor.NextFeature();

            int intFlag = 0;
            while (pFeature != null)
            {
                intFlag = 0;
                TargetPoint p = new TargetPoint();
                p.x = Convert.ToDouble(pFeature.get_Value(intRefXIdx));
                p.y = Convert.ToDouble(pFeature.get_Value(intRefYIdx));

                string strJoinID = pFeature.get_Value(intRefIDIdx).ToString();

                RefStreetVertices pRefStreetVertices = lstRefStreetInfo.Find(x => x.JoinID == strJoinID);

                int intVerticeCnt = pRefStreetVertices.pointCollection.PointCount;
                List<VerticesDist> lstVerDist = new List<VerticesDist>();

                IEnumVertex2 pEnumVertex = pRefStreetVertices.pointCollection.EnumVertices as IEnumVertex2;
                pEnumVertex.Reset();
                //Get the next vertex
                IPoint outVertex;
                int partIndex;
                int vertexIndex;
                pEnumVertex.Next(out outVertex, out partIndex, out vertexIndex);

                while (outVertex != null)
                {
                    VerticesDist pVerDist = new VerticesDist();
                    pVerDist.x = outVertex.X;
                    pVerDist.y = outVertex.Y;
                    pVerDist.ID = vertexIndex;
                    pVerDist.Distance = GetDistance(p.x, p.y, pVerDist.x, pVerDist.y);
                    lstVerDist.Add(pVerDist);
                    pEnumVertex.Next(out outVertex, out partIndex, out vertexIndex);
                }

                int intMinDistIdx = lstVerDist.FindIndex(x => x.Distance == lstVerDist.Min(t => t.Distance));
                TargetPoint l1 = new TargetPoint();
                l1.x = lstVerDist[intMinDistIdx].x;
                l1.y = lstVerDist[intMinDistIdx].y;

                TargetPoint l2 = new TargetPoint();
                TargetPoint pMovedRefPoint = new TargetPoint();

                int intSecondMinDistIdx = 0;
                if (intMinDistIdx == 0)
                {
                    intSecondMinDistIdx = intMinDistIdx + 1;
                    l2.x = lstVerDist[intSecondMinDistIdx].x;
                    l2.y = lstVerDist[intSecondMinDistIdx].y;
                    pMovedRefPoint = GetMovedReferencePoint(p, l1, l2);

                }
                else if (intMinDistIdx == lstVerDist.Count - 1)
                {
                    intSecondMinDistIdx = intMinDistIdx - 1;
                    l2.x = lstVerDist[intSecondMinDistIdx].x;
                    l2.y = lstVerDist[intSecondMinDistIdx].y;
                    pMovedRefPoint = GetMovedReferencePoint(p, l1, l2);
                }
                else
                {
                    TargetPoint pCandPt1 = new TargetPoint();
                    TargetPoint pCandPt2 = new TargetPoint();

                    intSecondMinDistIdx = intMinDistIdx - 1;
                    l2.x = lstVerDist[intSecondMinDistIdx].x;
                    l2.y = lstVerDist[intSecondMinDistIdx].y;
                    pCandPt1 = GetMovedReferencePoint(p, l1, l2);

                    intSecondMinDistIdx = intMinDistIdx + 1;
                    l2.x = lstVerDist[intSecondMinDistIdx].x;
                    l2.y = lstVerDist[intSecondMinDistIdx].y;

                    if (pCandPt1 == null)
                        pMovedRefPoint = GetMovedReferencePoint(p, l1, l2);
                    else
                    {
                        pCandPt2 = GetMovedReferencePoint(p, l1, l2);

                        if (pCandPt2 != null)
                        {
                            double distToCPt1 = GetDistance(p.x, p.y, pCandPt1.x, pCandPt1.y);
                            double distToCPt2 = GetDistance(p.x, p.y, pCandPt2.x, pCandPt2.y);

                            if (distToCPt1 > distToCPt2)
                                pMovedRefPoint = pCandPt2;
                            else
                                pMovedRefPoint = pCandPt1;
                        }
                        else
                            pMovedRefPoint = pCandPt1;
                    }
                }

                if (pMovedRefPoint == null)
                {
                    pMovedRefPoint = l1;
                    intFlag = 1;
                }


                pFeature.set_Value(intXNewFldIdx, pMovedRefPoint.x);
                pFeature.set_Value(intYNewFldIdx, pMovedRefPoint.y);
                pFeature.set_Value(intFlagFldIdx, intFlag);
                pFeature.Store();

                pFeature = pFCursor.NextFeature();

            }

            MessageBox.Show("Done");
            ////Spatial Reference from Reference dataseat
            //IGeoDataset geoDataset = pRefFClass as IGeoDataset;
            //ISpatialReference pSpatialRef = geoDataset.SpatialReference;

            ////Create ShapeFile First
            //string strOutput = txtOutput.Text;
            //if (strOutput == "")
            //    return;
            //string strOutputSource = System.IO.Path.GetDirectoryName(strOutput);
            //string strOutputFile = System.IO.Path.GetFileName(strOutput);
            ////IFeatureClass pFClass = CreateShapeFile(strOutputSource, strOutputFile, pSpatialRef, esriGeometryType.esriGeometryPoint);

        }
        private double GetDistance(double dblFX, double dblFY, double dblTX, double dblTY)
        {
            double dblXDiff = Math.Pow(dblFX - dblTX, 2);
            double dblYDiff = Math.Pow(dblFY - dblTY, 2);
            double dblDist = Math.Sqrt(dblXDiff + dblYDiff);

            return dblDist;
        }

        private IFeatureClass CreateShapeFile(string strOutFileSource, string strOutFileName, ISpatialReference spatialReference, esriGeometryType pGeometryType)
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
        private void AddFld(IFeatureClass pFClass, string strName, esriFieldType fieldType)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = strName;
            pFieldEdit.Type_2 = fieldType;

            pFClass.AddField(pField);
        }
        class RefStreetVertices
        {
            public string JoinID { get; set; }
            public IPointCollection pointCollection { get; set; }

        }
        class VerticesDist
        {
            public double x { get; set; }
            public double y { get; set; }
            public int ID { get; set; }
            public double Distance { get; set; }
        }

        class TargetPoint
        {
            public double x { get; set; }
            public double y { get; set; }
        }

        private TargetPoint GetMovedReferencePoint(TargetPoint p, TargetPoint l1, TargetPoint l2)
        {
            TargetPoint a = new TargetPoint();
            TargetPoint b = new TargetPoint();

            double m1 = 0, k1 = 0;  //Slope and intercept of Street Line

            double m2 = 0, k2 = 0;  // Slope and intercept of perpenticular line

            if (l1.x == l2.x)
            {
                a.x = l1.x;
                a.y = p.y;
            }
            else if (l1.y == l2.y)
            {
                a.x = p.x;
                a.y = l1.y;
            }
            else
            {
                m1 = (l1.y - l2.y) / (l1.x - l2.x);
                k1 = -m1 * l1.x + l1.y;

                m2 = -1.0 / m1;
                k2 = p.y - m2 * p.x;

                a.x = (k2 - k1) / (m1 - m2);
                a.y = m1 * a.x + k1;
            }

            if (a.x >= GetMin(l1.x, l2.x) && a.x <= GetMax(l1.x, l2.x) &&
                 a.y >= GetMin(l1.y, l2.y) && a.y <= GetMax(l1.y, l2.y)) //Check whether point a is on the street network
            {
                //Move 10 feet away from center lines
                double dblA = Math.Pow(m2, 2) + 1;
                double dblB = (2 * m2 * k2) - (2 * a.x) - (2 * m2 * a.y);
                double dblC = Math.Pow(a.x, 2) + Math.Pow(a.y, 2) - (2 * a.y * k2) + Math.Pow(k2, 2) - 100;

                b.x = ((-1 * dblB) + Math.Sqrt(Math.Pow(dblB, 2) - (4 * dblA * dblC))) / (2 * dblA);
                b.y = b.x * m2 + k2;

                double dblDist = Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2);

                if (b.x >= GetMin(a.x, p.x) && b.x <= GetMax(a.x, p.x) &&
     b.y >= GetMin(a.y, p.y) && b.y <= GetMax(a.y, p.y)) //Check whether point a is on the street network
                {
                    return b;
                }
                else
                {
                    b.x = ((-1 * dblB) - Math.Sqrt(Math.Pow(dblB, 2) - (4 * dblA * dblC))) / (2 * dblA);
                    b.y = b.x * m2 + k2;
                    dblDist = Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2);

                    if (b.x >= GetMin(a.x, p.x) && b.x <= GetMax(a.x, p.x) &&
b.y >= GetMin(a.y, p.y) && b.y <= GetMax(a.y, p.y)) //Check whether point a is on the street network
                    {
                        return b;
                    }
                    else
                        return null;

                }
            }
            else
            {
                return null;
            }
        }
        double GetMin(double d1, double d2)
        {
            if (d1 < d2)
                return d1;

            return d2;
        }

        double GetMax(double d1, double d2)
        {
            if (d1 > d2)
                return d1;

            return d2;
        }
    }
}
