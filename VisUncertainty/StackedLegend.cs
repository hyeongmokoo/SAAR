using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;


namespace VisUncertainty
{
    class StackedLegned: ILegendInfo, IFeatureRenderer
    {
        //Data Members

        //private int m_intUncernIdx;
        //private int m_intOriIdx;
        public ILegendGroup m_pLegendGroup;
        //private int m_intLowerIdx;


        //Requried Members
        public string m_strOriRenderField;
        public string m_strUncernRenderField;
        public double dblMaxValue;
        public double dblMaxEstimate;
        public double dblMaxUncern;
        //public double dblError;
        //public int intValueFldIdx;
        //public int intUncerFldIdx;
        public bool bln3Dfeature;
        //public string m_strLowerRenderField;

        //public IRgbColor m_pSymbolRgb;
        //public IRgbColor m_pLineRgb;

        //public double m_dblMinValue;
        //public double m_dblMaxValue;
        //public double m_dblMinPtSize;
        //public byte m_bytFromTrans;
        //public byte m_bytToTrans;
        //public double m_dblOutlineSize;
        //public double m_dblAdjustedMinValue;
        //public double m_dblConInstance;
        //public IGeoFeatureLayer m_pGeoFeatureLayer;

        public StackedLegned()
        {
        }

        //IfeatureRenderer Interface Members
        public bool CanRender(IFeatureClass featClass, IDisplay Display)
        {
            // only use this renderer if we have polygon
            return (featClass.ShapeType == esriGeometryType.esriGeometryPolygon);
        }

        bool IFeatureRenderer.get_RenderPhase(ESRI.ArcGIS.esriSystem.esriDrawPhase DrawPhase)
        {
            return (DrawPhase == esriDrawPhase.esriDPGeography);
        }

        public void PrepareFilter(IFeatureClass fc, IQueryFilter queryFilter)
        {
            //Not Imp
        }

        //Draw Symbols by features
        public void Draw(IFeatureCursor pFCursor, esriDrawPhase DrawPhase, IDisplay Display, ITrackCancel trackCancel)
        {

            // NOT IMPL
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

        //Create Legend
        public void CreateLegend()
        {
            clsSnippet pSnippet = new clsSnippet();

            //Create Legend Group
            m_pLegendGroup = new LegendGroup();
            m_pLegendGroup.Heading = m_strOriRenderField;
            m_pLegendGroup.Editable = true;
            m_pLegendGroup.Visible = true;

            ILegendClass legendClass = new LegendClass();
            legendClass.Label = Math.Round(dblMaxValue, 0).ToString();

            IStackedChartSymbol stackedChartSymbol = new StackedChartSymbolClass();

            //IBarChartSymbol barChartSymbol = new BarChartSymbolClass();
            stackedChartSymbol.Width = 10;
            if (bln3Dfeature)
            {
                I3DChartSymbol p3DChartSymbol = stackedChartSymbol as I3DChartSymbol;
                p3DChartSymbol.Display3D = true;
                p3DChartSymbol.Thickness = 3;
            }
            //IMarkerSymbol markerSymbol = stackedChartSymbol as IMarkerSymbol;
            //markerSymbol.Size = 50;
            IChartSymbol chartSymbol = stackedChartSymbol as IChartSymbol;
            chartSymbol.MaxValue = dblMaxValue;

            ISymbolArray symbolArray = stackedChartSymbol as ISymbolArray;
            //Stacked Symbol
            IFillSymbol fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = pSnippet.getRGB(255, 0, 0);
            fillSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);
            symbolArray.AddSymbol(fillSymbol as ISymbol);
            chartSymbol.set_Value(0, dblMaxUncern);

            //fillSymbol = new SimpleFillSymbolClass();
            //fillSymbol.Color = pSnippet.getRGB(255, 0, 0);
            //fillSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);
            //symbolArray.AddSymbol(fillSymbol as ISymbol);
            //chartSymbol.set_Value(1, dblMaxUncern);

            fillSymbol = new SimpleFillSymbolClass();
            fillSymbol.Color = pSnippet.getRGB(255, 255, 255);
            fillSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);
            symbolArray.AddSymbol(fillSymbol as ISymbol);
            chartSymbol.set_Value(1, dblMaxEstimate-dblMaxUncern);

            legendClass.Symbol = (ISymbol)chartSymbol;
            m_pLegendGroup.AddClass(legendClass);

            //Bounds
            legendClass = new LegendClass();
            legendClass.Label = "Upper Bound";
            ISimpleFillSymbol pLegendUncerSymbol = new SimpleFillSymbol();
            pLegendUncerSymbol.Color = pSnippet.getRGB(0, 0, 255);
            pLegendUncerSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);

            legendClass.Symbol = (ISymbol)pLegendUncerSymbol;
            m_pLegendGroup.AddClass(legendClass);

            legendClass = new LegendClass();
            legendClass.Label = "Lower Bound";
            pLegendUncerSymbol = new SimpleFillSymbol();
            pLegendUncerSymbol.Color = pSnippet.getRGB(255, 0, 0);
            pLegendUncerSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);

            legendClass.Symbol = (ISymbol)pLegendUncerSymbol;
            m_pLegendGroup.AddClass(legendClass);

        }


        //ILegendInfo Interface members
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

    }
}
