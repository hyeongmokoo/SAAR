using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace VisUncertainty
{
    [Guid("0ee36605-5708-4ba9-a605-a42168f519a5")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.ArcGISClass1")]
    public class SpacingBreaksRendererClass : ISpacingBreaksRenderer, IFeatureRenderer, ILegendInfo
    {
        //ISpacingBrksRenderer variables
        double[] arrValue = null;
        double[] arrClassBrks = null;
        IRgbColor pLineRgb = null;
        double dblLineWidth = 0;
        double dblLineAngle = 0;
        double dblFromSep = 0;
        double dblToSep = 0;
        int intRoundingDigits = 2;
        string strHeading = string.Empty;

        //IfeatureRenderer variables
        private IFeatureClass m_pFeatureClass;
        private IQueryFilter m_pQueryFilter;
        private IFeatureIDSet m_pExclusionSet = null;

        //ILegendGroup Variables
        //private ILegendGroups m_pTwoLegendGroup;
        private ILegendGroup m_pLegendGroup;

        //Internal variables;
        private int intBrksCount = 0;
        private double dblInstantSep = 0;

        #region ISpacingBrksRenderer Members

        double[] ISpacingBreaksRenderer.arrValue
        {
            get { return this.arrValue; }
            set { this.arrValue = value; }
        }

        double[] ISpacingBreaksRenderer.arrClassBrks
        {
            get { return this.arrClassBrks; }
            set { this.arrClassBrks = value; }
        }

        ESRI.ArcGIS.Display.IRgbColor ISpacingBreaksRenderer.m_pLineRgb
        {
            get { return this.pLineRgb; }
            set { this.pLineRgb = value; }
        }

        double ISpacingBreaksRenderer.dblLineWidth
        {
            get { return this.dblLineWidth; }
            set { this.dblLineWidth = value; }
        }

        double ISpacingBreaksRenderer.dblLineAngle
        {
            get { return this.dblLineAngle; }
            set { this.dblLineAngle = value; }
        }

        double ISpacingBreaksRenderer.dblFromSep
        {
            get { return this.dblFromSep; }
            set { this.dblFromSep = value; }
        }

        double ISpacingBreaksRenderer.dblToSep
        {
            get { return this.dblToSep; }
            set { this.dblToSep = value; }
        }

        int ISpacingBreaksRenderer.intRoundingDigits
        {
            get { return this.intRoundingDigits; }
            set { this.intRoundingDigits = value; }
        }

        string ISpacingBreaksRenderer.strHeading
        {
            get { return this.strHeading; }
            set { this.strHeading = value; }
        }

        void ISpacingBreaksRenderer.CreateLegend()
        {
            //Create Legend Group
            m_pLegendGroup = new LegendGroup();

            m_pLegendGroup.Heading = strHeading;
            m_pLegendGroup.Editable = true;
            m_pLegendGroup.Visible = true;

            intBrksCount = arrClassBrks.Length - 1;
            dblInstantSep = (dblFromSep - dblToSep) / Convert.ToDouble(intBrksCount - 1);



            //For Values
            ILegendClass legendClass = new LegendClass();

            ILineFillSymbol pLineFillSym = new LineFillSymbolClass();

            for (int i = 0; i < intBrksCount; i++)
            {

                legendClass = new LegendClass();
                //if (i == intUncernBreakCount - 1)
                //    legendClass.Label = "1";
                //else if (i == intUncernBreakCount - 2)
                //    legendClass.Label = arrClassBrks[i].ToString() + " - " + (1 - (1 * Math.Pow(0.1, intRoundingDigits))).ToString();
                //else
                legendClass.Label = Math.Round(arrClassBrks[i], intRoundingDigits).ToString() + " - " + (Math.Round(arrClassBrks[i+1], intRoundingDigits) - (1 * Math.Pow(0.1, intRoundingDigits))).ToString();

                pLineFillSym = new LineFillSymbolClass();

                pLineFillSym.Angle = dblLineAngle;
                pLineFillSym.Color = pLineRgb;
                pLineFillSym.LineSymbol.Width = dblLineWidth;
                pLineFillSym.Separation = dblToSep + (dblInstantSep * Convert.ToDouble(i));

                legendClass.Symbol = pLineFillSym as ISymbol;
                m_pLegendGroup.AddClass(legendClass);
            }
        }
        #endregion

        #region IFeatureRenderer Members
        bool IFeatureRenderer.CanRender(ESRI.ArcGIS.Geodatabase.IFeatureClass featClass, IDisplay Display)
        {
            return (featClass.ShapeType == esriGeometryType.esriGeometryPolygon);
        
        }

        void IFeatureRenderer.Draw(ESRI.ArcGIS.Geodatabase.IFeatureCursor Cursor, ESRI.ArcGIS.esriSystem.esriDrawPhase DrawPhase, IDisplay Display, ESRI.ArcGIS.esriSystem.ITrackCancel TrackCancel)
        {
            // do not draw features if no display
            if (Display == null)
                return;
            else
            {
                IFeature pFeat = null;

                intBrksCount = arrClassBrks.Length - 1;
                ILineFillSymbol pLineFillSym = new LineFillSymbolClass();
                pFeat = Cursor.NextFeature();
                dblInstantSep = (dblFromSep - dblToSep) / Convert.ToDouble(intBrksCount - 1);

                int i = 0;
                //Start Loop
                while (pFeat != null)
                {
                    IFeatureDraw pFeatDraw = (IFeatureDraw)pFeat;
                    pLineFillSym = new LineFillSymbolClass();
                    pLineFillSym.Angle = dblLineAngle;
                    pLineFillSym.Color = pLineRgb;
                    pLineFillSym.LineSymbol.Width = dblLineWidth;

                    for (int j = 0; j < intBrksCount; j++)
                    {
                        if (arrValue[i] >= arrClassBrks[j] && arrValue[i] <= arrClassBrks[j + 1])
                            pLineFillSym.Separation = dblToSep + (dblInstantSep * Convert.ToDouble(j));

                    }

                    Display.SetSymbol((ISymbol)pLineFillSym);
                    pFeatDraw.Draw(esriDrawPhase.esriDPGeography, Display, (ISymbol)pLineFillSym, true,
                        null, esriDrawStyle.esriDSNormal);
                    i++;
                    pFeat = Cursor.NextFeature();
                }
            }
        }

        IFeatureIDSet IFeatureRenderer.ExclusionSet
        {
            set { this.m_pExclusionSet = value; }
        }

        void IFeatureRenderer.PrepareFilter(ESRI.ArcGIS.Geodatabase.IFeatureClass fc, ESRI.ArcGIS.Geodatabase.IQueryFilter queryFilter)
        {
            //Must Add OID
            queryFilter.AddField(fc.OIDFieldName);

            m_pFeatureClass = fc;
            m_pQueryFilter = queryFilter;
        }

        bool IFeatureRenderer.get_RenderPhase(ESRI.ArcGIS.esriSystem.esriDrawPhase DrawPhase)
        {
            return (DrawPhase == esriDrawPhase.esriDPGeography) | (DrawPhase == esriDrawPhase.esriDPAnnotation);
        
        }

        ISymbol IFeatureRenderer.get_SymbolByFeature(ESRI.ArcGIS.Geodatabase.IFeature Feature)
        {
            ISymbol pSymbol = m_pLegendGroup.get_Class(0).Symbol;

            return pSymbol;
        }
        #endregion

        #region ILegendInfo Members

        int ILegendInfo.LegendGroupCount
        {
            get { return 1; }
        }

        ILegendItem ILegendInfo.LegendItem
        {
            get { return null; }
        }

        bool ILegendInfo.SymbolsAreGraduated
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        ILegendGroup ILegendInfo.get_LegendGroup(int Index)
        {
            if (m_pLegendGroup.get_Class(0).Symbol != null)
                return m_pLegendGroup;
            else
                return null;
        }
        #endregion


    }
}
