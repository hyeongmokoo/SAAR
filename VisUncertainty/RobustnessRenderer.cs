using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;

namespace VisUncertainty
{
    class RobustnessRenderer
                :  IFeatureRenderer, ILegendInfo
    {
        #region private Members
        private IQueryFilter m_pQueryFilter;
        private ILegendGroup m_pLegendGroup;

        #endregion
        #region Public Members
        //RequireMebers
        //public string m_strUncernRenderField;
        public IFeatureClass m_pFeatureClass;

        public double dblFromSep;
        public double dblToSep;
        public double dblAngle;
        public double dblLinewidth;

        public IRgbColor pLineColor;
        public int intUncernBreakCount;
        public double[] arrRobustBrks;
        public double[] arrRobustness;

        public int intRoundingDigits = 2; // Not connected yet 102915 HK

        #endregion


        #region IFeatureRenderer Interface Members
        //IfeatureRenderer Interface Members
        public bool CanRender(IFeatureClass featClass, IDisplay Display)
        {

            return (featClass.ShapeType == esriGeometryType.esriGeometryPolygon || featClass.ShapeType == esriGeometryType.esriGeometryPoint);
        }

        bool IFeatureRenderer.get_RenderPhase(ESRI.ArcGIS.esriSystem.esriDrawPhase DrawPhase)
        {
            return (DrawPhase == esriDrawPhase.esriDPGeography);
        }

        public void PrepareFilter(IFeatureClass fc, IQueryFilter queryFilter)
        {
            if (queryFilter == null)
                queryFilter = new QueryFilterClass();

            queryFilter.AddField(fc.OIDFieldName);

            //queryFilter.AddField(m_strUncernRenderField);

            //int UncernIdx = fc.FindField(m_strUncernRenderField);
            m_pFeatureClass = fc;
            m_pQueryFilter = queryFilter;

        }

        //Draw Symbols by features
        public void Draw(IFeatureCursor pFCursor, esriDrawPhase DrawPhase, IDisplay Display, ITrackCancel trackCancel)
        {
            IFeatureCursor pOriCursor = pFCursor;

            IFeature pFeat = null;

            // do not draw features if no display
            if (Display == null)
                return;

            ILineFillSymbol pLineFillSym = new LineFillSymbolClass();
            pFeat = pFCursor.NextFeature();
            double dblInstantSep = (dblFromSep - dblToSep) / Convert.ToDouble(intUncernBreakCount - 2);
            
            int i = 0;
            //Start Loop
            while (pFeat != null)
            {
                IFeatureDraw pFeatDraw = (IFeatureDraw)pFeat;
                pLineFillSym = new LineFillSymbolClass();
                pLineFillSym.Angle = dblAngle;
                pLineFillSym.Color = pLineColor;
                pLineFillSym.LineSymbol.Width = dblLinewidth;

                if (Math.Round(arrRobustness[i], intRoundingDigits)==1)
                    pLineFillSym.Separation = double.MaxValue;
                else
                {
                    for (int j = 0; j < intUncernBreakCount - 1; j++)
                    {
                        if (arrRobustness[i] >= arrRobustBrks[j] && arrRobustness[i] <= arrRobustBrks[j + 1])
                            pLineFillSym.Separation = dblToSep + (dblInstantSep * Convert.ToDouble(j));

                    }
                }
                

                Display.SetSymbol((ISymbol)pLineFillSym);
                pFeatDraw.Draw(esriDrawPhase.esriDPGeography, Display, (ISymbol)pLineFillSym, true,
                    null, esriDrawStyle.esriDSNormal);
                i++;
                pFeat = pFCursor.NextFeature();
            }
            
        }

        public IFeatureIDSet ExclusionSet
        {
            set
            {
                // NOT IMPL    
            }
        }
        ESRI.ArcGIS.Display.ISymbol IFeatureRenderer.get_SymbolByFeature(IFeature pFeature)
        {

            ISymbol pSymbol = null;

            return pSymbol;
        }

        #endregion

        #region ILegendInfo Members
        //Create Legend
        public void CreateLegend()
        {
            clsSnippet pSnippet = new clsSnippet();

            //Create Legend Group
            m_pLegendGroup = new LegendGroup();

            m_pLegendGroup.Heading = "Robustness";
            m_pLegendGroup.Editable = true;
            m_pLegendGroup.Visible = true;

            //For Values
            ILegendClass legendClass = new LegendClass();
            
            ILineFillSymbol pLineFillSym = new LineFillSymbolClass();
            


            double dblInstantSep = (dblFromSep - dblToSep) / Convert.ToDouble(intUncernBreakCount - 2);
            
            for(int i=0; i < intUncernBreakCount; i++)
            {
                
                legendClass = new LegendClass();
                if (i == intUncernBreakCount - 1)
                    legendClass.Label = "1";
                else if(i==intUncernBreakCount-2)
                    legendClass.Label = arrRobustBrks[i].ToString()+ " - " + (1 - (1*Math.Pow(0.1, intRoundingDigits))).ToString();
                else
                    legendClass.Label = arrRobustBrks[i].ToString() + " - " + (arrRobustBrks[i + 1] - (1 * Math.Pow(0.1, intRoundingDigits))).ToString();

                pLineFillSym = new LineFillSymbolClass();
                
                pLineFillSym.Angle = dblAngle;
                pLineFillSym.Color = pLineColor;
                pLineFillSym.LineSymbol.Width = dblLinewidth;
                if(i==(intUncernBreakCount-1))
                    pLineFillSym.Separation = double.MaxValue;
                else
                    pLineFillSym.Separation = dblToSep + (dblInstantSep * Convert.ToDouble(i));
                legendClass.Symbol = pLineFillSym as ISymbol;
                m_pLegendGroup.AddClass(legendClass);
            }
        }

        public bool SymbolsAreGraduated
        {
            get
            {
                return false;
            }
            set
            {
                // NOT IMPL
            }
        }
        public ILegendItem LegendItem
        {
            get
            {
                return null;
            }
        }
        public int LegendGroupCount
        {
            get
            {
                int n = 0;
                if (m_pLegendGroup != null)
                {
                    n = n + 1;
                }
                return n;
            }
        }
        ESRI.ArcGIS.Carto.ILegendGroup ILegendInfo.get_LegendGroup(int intLegendIdx)
        {

            if (m_pLegendGroup.get_Class(0).Symbol != null)
                return m_pLegendGroup;
            else
                return null;

        }
        #endregion
    }
}
