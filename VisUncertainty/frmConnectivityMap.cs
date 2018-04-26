using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisUncertainty
{
    public partial class frmConnectivityMap : Form
    {
        public IFeatureLayer m_pFLayer;
        public clsSnippet.Def_SpatialWeightsMatrix Def_SWM;

        private clsBrusingLinking m_pBL;
        private string m_strFieldName;
        private clsSnippet m_pSnippet;
        private IActiveView m_pActiveView;
        private double m_dblTolerance;

        public frmConnectivityMap()
        {
            InitializeComponent();
            m_pBL = new clsBrusingLinking();

            m_pSnippet = new clsSnippet();
            m_pActiveView = ConMapControl.ActiveView;
        }

        private void ConMapControl_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            try
            {
                m_pActiveView.GraphicsContainer.DeleteAllElements();

                IFeatureSelection featureSelection = (IFeatureSelection)m_pFLayer;
                m_pActiveView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeoSelection, null, null);
                featureSelection.Clear();
                m_pActiveView.Refresh();

                int x = e.x;
                int y = e.y;

                IPoint pPoint = m_pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(x, y);
                int intFID = FindFeatureFID(pPoint, m_pFLayer, m_dblTolerance);
                if (intFID == -1)
                    return;

                string whereClause = m_strFieldName + " = " + intFID.ToString();
                m_pBL.FeatureSelectionOnActiveView(whereClause, m_pActiveView, m_pFLayer);

                int intNCount = Def_SWM.NBIDs[intFID].Count;
                StringBuilder sbStatusLabel = new StringBuilder();
                sbStatusLabel.Append("The feature (ID:" + intFID.ToString() + ") has " + intNCount.ToString() + " neighbors: ");


                if (intNCount == 1 && Def_SWM.NBIDs[intFID][0] == 0)
                    return;

                DrawLineOnActiveView(intFID, Def_SWM.NBIDs[intFID], Def_SWM.XYCoord, m_pActiveView);

                for (int i = 0; i < intNCount; i++)
                {
                    sbStatusLabel.Append(Def_SWM.NBIDs[intFID][i].ToString() + ", ");
                }
                if (sbStatusLabel.Length > 3)
                    sbStatusLabel.Remove(sbStatusLabel.Length - 2, 2);
                toolStripStatusLabel1.Text = sbStatusLabel.ToString();
                statusStrip1.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private int FindFeatureFID(IPoint pPoint, IFeatureLayer pFLayer, double PtTolerance)
        {
            try
            {
               
                double Tol = 4;
                if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    Tol = PtTolerance;

                IEnvelope pEnvelop = pPoint.Envelope;
                pEnvelop.Expand(Tol, Tol, false);
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pEnvelop;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                string ShapeFieldName = pFLayer.FeatureClass.ShapeFieldName;
                pSpatialFilter.GeometryField = pFLayer.FeatureClass.ShapeFieldName;
                IFeatureClass pFeatureClass = pFLayer.FeatureClass;
                IFeatureCursor pFCursor = pFeatureClass.Search(pSpatialFilter, false);
                IFeature pFeature = pFCursor.NextFeature();
                //int intFIDIdx = pFeatureClass.FindField("FID");
                if (pFeature == null)
                    return -1;

                int intFID = Convert.ToInt32(pFeature.get_Value(0)); //Get FID Value
                return intFID;
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return -1;
            }
        }

        private void DrawLineOnActiveView(int intFromLinkID, List<int> arrToLinks, double[,] arrXYCoord, IActiveView pActiveView)
        {
            try
            {
                IGraphicsContainer pGraphicContainer = pActiveView.GraphicsContainer;
                //pGraphicContainer.DeleteAllElements();

                IRgbColor pRgbColor = m_pSnippet.getRGB(0, 255, 255);

                ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
                pSimpleLineSymbol.Width = 2;
                pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pSimpleLineSymbol.Color = pRgbColor;
                int intFromIdx = intFromLinkID;
                ESRI.ArcGIS.Geometry.IPoint FromP = new PointClass();
                FromP.X = arrXYCoord[intFromIdx, 0]; FromP.Y = arrXYCoord[intFromIdx, 1];

                int intArrLengthCnt = arrToLinks.Count;
                for (int i = 0; i < intArrLengthCnt; i++)
                {
                    int intToIdx = arrToLinks[i] - 1;

                    ESRI.ArcGIS.Geometry.IPoint ToP = new PointClass();
                    ToP.X = arrXYCoord[intToIdx, 0]; ToP.Y = arrXYCoord[intToIdx, 1];

                    IPolyline polyline = new PolylineClass();
                    IPointCollection pointColl = polyline as IPointCollection;
                    pointColl.AddPoint(FromP);
                    pointColl.AddPoint(ToP);

                    IElement pElement = new LineElementClass();
                    ILineElement pLineElement = (ILineElement)pElement;
                    pLineElement.Symbol = pSimpleLineSymbol;
                    pElement.Geometry = polyline;

                    pGraphicContainer.AddElement(pElement, 0);
                }

                pActiveView.Refresh();
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog(); pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }

        }

        private void frmConnectivityMap_Load(object sender, EventArgs e)
        {
            m_strFieldName = m_pFLayer.FeatureClass.OIDFieldName;
            double dblXBnd = (m_pActiveView.Extent.XMax - m_pActiveView.Extent.XMin) / (Def_SWM.FeatureCount);
            double dblYBnd = (m_pActiveView.Extent.YMax - m_pActiveView.Extent.YMin) / (Def_SWM.FeatureCount);
            m_dblTolerance = (dblXBnd + dblYBnd) / 2.0;
        }
    }
}
