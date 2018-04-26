using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace VisUncertainty
{
    public partial class frmScaleBarProperties : Form
    {
        public IPageLayout pPageLayout;
        public IActiveView pActiveView;
        public double dblMaxX;
        public double dblMinY;
        public double dblProportion;

        public frmScaleBarProperties()
        {
            InitializeComponent();
        }
        public void AddScalebar(IPageLayout pageLayout, IMap map, double posX, double posY, double division, string strMapUnits, short srtDivisions)
        {
            try
            {

                if (pageLayout == null || map == null)
                {
                    return;
                }

                IEnvelope envelope = new EnvelopeClass();
                envelope.PutCoords(posX - (division * 2), posY, posX, posY + (division * 0.5)); // Specify the location and size of the scalebar
                ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
                uid.Value = "esriCarto.AlternatingScaleBar";

                // Create a Surround. Set the geometry of the MapSurroundFrame to give it a location
                // Activate it and add it to the PageLayout's graphics container
                IGraphicsContainer graphicsContainer = pageLayout as IGraphicsContainer; // Dynamic Cast
                IActiveView activeView = pageLayout as IActiveView; // Dynamic Cast
                IFrameElement frameElement = graphicsContainer.FindFrame(map);
                IMapFrame mapFrame = frameElement as IMapFrame; // Dynamic Cast
                IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null); // Dynamic Cast
                IElement element = mapSurroundFrame as IElement; // Dynamic Cast
                element.Geometry = envelope;
                element.Activate(activeView.ScreenDisplay);
                graphicsContainer.AddElement(element, 0);
                IMapSurround mapSurround = mapSurroundFrame.MapSurround;


                IScaleBar markerScaleBar = ((IScaleBar)(mapSurround));
                markerScaleBar.LabelPosition = esriVertPosEnum.esriBelow;
                markerScaleBar.UseMapSettings();
                markerScaleBar.Divisions = srtDivisions;
                markerScaleBar.DivisionsBeforeZero = 0;
                markerScaleBar.Subdivisions = 0;
                markerScaleBar.LabelFrequency = esriScaleBarFrequency.esriScaleBarMajorDivisions;

                if (strMapUnits == "Feet")
                    markerScaleBar.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet;
                //Default Settings
                else if (strMapUnits == "Miles")
                    markerScaleBar.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriMiles;
                else if (strMapUnits == "Meters")
                    markerScaleBar.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters;
                else if (strMapUnits == "Kilometers")
                    markerScaleBar.Units = ESRI.ArcGIS.esriSystem.esriUnits.esriKilometers;
                else
                    return;
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

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                string strMapUnits = cboDivisionUnits.Text;
                if (strMapUnits == "") return;
                short srtDivisions = Convert.ToInt16(nudNDivisions.Value);

                AddScalebar(pPageLayout, pActiveView.FocusMap, dblMaxX - (dblProportion / 2), dblMinY + (dblProportion / 2), dblProportion, strMapUnits, srtDivisions);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
