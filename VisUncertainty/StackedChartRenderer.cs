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
    class StackedChartRenderer : IFeatureRenderer
    {
       // //Data Members
       // private esriGeometryType m_ShapeType;
       // private IFeatureClass m_pFeatureClass;
       // private IQueryFilter m_pQueryFilter;
       // public IFeatureRenderer m_stackedChart;

       ////Requried Members
       // public string m_strOriRenderField;
       // public string m_strUncernRenderField;
       // public double dblMaxValue;

       // public double dblError;
       // public int intValueFldIdx;
       // public int intUncerFldIdx;
       // public bool bln3Dfeature;

        //Data Members
        //private esriGeometryType m_ShapeType;
        private IFeatureClass m_pFeatureClass;
       
        //public IFeatureRenderer m_stackedChart;
        //private int m_intUncernIdx;
        //private int m_intOriIdx;
        //public ILegendGroup m_pLegendGroup;
        //private int m_intLowerIdx;
        

        //Requried Members
        public string m_strOriRenderField;
        public string m_strUncernRenderField;
        public double dblMaxValue;
        public double dblMaxEstimate;
        public double dblMaxUncern;
        public double dblError;
        public int intValueFldIdx;
        public int intUncerFldIdx;
        public bool bln3Dfeature;
        public IQueryFilter m_pQueryFilter;
        public IDisplay m_pDisplay;


        public StackedChartRenderer()
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
            //if (queryFilter == null)
            //    queryFilter = new QueryFilterClass();
            queryFilter = m_pQueryFilter;
            queryFilter.AddField(fc.OIDFieldName);

            //m_ShapeType = fc.ShapeType;
            queryFilter.AddField(m_strOriRenderField);
            queryFilter.AddField(m_strUncernRenderField);

            // save the fc and the query filter so that multiple cursors can be built in DrawSymbols
            m_pFeatureClass = fc;
            m_pQueryFilter = queryFilter;

        }

        //Draw Symbols by features
        public void Draw(IFeatureCursor pFCursor, esriDrawPhase DrawPhase, IDisplay Display, ITrackCancel trackCancel)
        {
            pFCursor = m_pFeatureClass.Search(m_pQueryFilter, true);
            clsSnippet pSnippet = new clsSnippet();

             IFeature pFeature = pFCursor.NextFeature();

            double dblValue = 0;
            double dblUncern = 0;
            double dblInterval = 0;

            IStackedChartSymbol barChartSymbol;
            IChartSymbol chartSymbol;

            while (pFeature != null)
            {
                IFeatureDraw pFeatDraw = (IFeatureDraw)pFeature;

                dblValue = Convert.ToDouble(pFeature.get_Value(intValueFldIdx));
                dblUncern = Convert.ToDouble(pFeature.get_Value(intUncerFldIdx));

                dblInterval = dblError * dblUncern;


                barChartSymbol = new StackedChartSymbolClass();

                barChartSymbol.Width = 10;

                if (bln3Dfeature)
                {
                    I3DChartSymbol p3DChartSymbol = barChartSymbol as I3DChartSymbol;
                    p3DChartSymbol.Display3D = true;
                    p3DChartSymbol.Thickness = 3;
                }

                //IMarkerSymbol markerSymbol = barChartSymbol as IMarkerSymbol;
                //markerSymbol.Size = 50;
                chartSymbol = barChartSymbol as IChartSymbol;
                chartSymbol.MaxValue = dblMaxValue;
                
                
                ISymbolArray symbolArray = barChartSymbol as ISymbolArray;
                //Upper Error Symbol
                IFillSymbol fillSymbol = new SimpleFillSymbolClass();
                fillSymbol.Color = pSnippet.getRGB(0, 0, 255);
                fillSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);
                symbolArray.AddSymbol(fillSymbol as ISymbol);
                chartSymbol.set_Value(0, dblInterval);

                //Lower Error
                fillSymbol = new SimpleFillSymbolClass();
                fillSymbol.Color = pSnippet.getRGB(255, 0, 0);
                fillSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);
                symbolArray.AddSymbol(fillSymbol as ISymbol);
                chartSymbol.set_Value(1, dblInterval);

                //Value -Error to represent mean value
                fillSymbol = new SimpleFillSymbolClass();
                fillSymbol.Color = pSnippet.getRGB(255, 255, 255);
                fillSymbol.Outline.Color = pSnippet.getRGB(0, 0, 0);
                symbolArray.AddSymbol(fillSymbol as ISymbol);
                chartSymbol.set_Value(2, dblValue - dblInterval);
                Display = m_pDisplay;
                Display.SetSymbol((ISymbol)chartSymbol);
                DrawPhase = esriDrawPhase.esriDPGeography;
                pFeatDraw.Draw(DrawPhase, Display, (ISymbol)chartSymbol, true,
                        null, esriDrawStyle.esriDSNormal);
                pFeature = pFCursor.NextFeature();

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


       
    }
}
