using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;

namespace VisUncertainty
{
    [Guid("433ea1c0-9262-4164-b8a7-ee6e1d52eb63")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("VisUncertainty.ClassSeparabilityRenderer")]
    public class ClassSeparabilityRenderer: IClassSeparabilityRenderer, IFeatureRenderer, ILegendInfo
    {
        //ISpacingBrksRenderer variables
        //double[] arrValue = null;
        clsClassSeperabilityMembers pCSMembers = null;
        //double[] arrClassBrks = null;
        //double[] arrSeparability = null;
        IRgbColor pLineRgb = null;
        double dblLineWidth = 0;
        //int[,] arrColors = null;
        //int[,] arrSepLineColors = null;
        int intRoundingDigits = 2;
        string strHeading = string.Empty;
        string strValueFldName = string.Empty;
        //IfeatureRenderer variables
        private IFeatureClass m_pFeatureClass;
        private IQueryFilter m_pQueryFilter;
        private IFeatureIDSet m_pExclusionSet = null;

        //ILegendGroup Variables
        private ILegendGroups m_pTwoLegendGroup;
        private ILegendGroup m_pLegendGroup;

        //Internal variables;
        private int intBrksCount = 0;
        private int intEstIdx = 0;


        #region IClassSeparabilityRenderer Members

        clsClassSeperabilityMembers IClassSeparabilityRenderer.pCSMembers
        {
            get
            {
                return this.pCSMembers;
            }
            set
            {
                this.pCSMembers = value;
            }
        }

        string IClassSeparabilityRenderer.strValueFldName
        {
            get { return this.strValueFldName; }
            set { this.strValueFldName = value; }
        }


        //double[] IClassSeparabilityRenderer.arrClassBrks
        //{
        //    get { return this.arrClassBrks; }
        //    set { this.arrClassBrks = value; }
        //}

        //double[] IClassSeparabilityRenderer.arrSeparability
        //{
        //    get { return this.arrSeparability; }
        //    set { this.arrSeparability = value; }
        //}

        ESRI.ArcGIS.Display.IRgbColor IClassSeparabilityRenderer.m_pLineRgb
        {
            get { return this.pLineRgb; }
            set { this.pLineRgb = value; }
        }

        double IClassSeparabilityRenderer.dblLineWidth
        {
            get { return this.dblLineWidth; }
            set
            { this.dblLineWidth = value; }
        }

        int IClassSeparabilityRenderer.intRoundingDigits
        {
            get { return this.intRoundingDigits; }
            set { this.intRoundingDigits = value; }
        }

        string IClassSeparabilityRenderer.strHeading
        {
            get { return this.strHeading; }
            set { this.strHeading = value; }
        }
        //int[,] IClassSeparabilityRenderer.arrColors
        //{
        //    get { return this.arrColors; }
        //    set { this.arrColors = value; }
        //}

        //int[,] IClassSeparabilityRenderer.arrSepLineColors
        //{
        //    get { return this.arrSepLineColors; }
        //    set { this.arrSepLineColors = value; }
        //}
        void IClassSeparabilityRenderer.CreateLegend()
        {
            double dblLinewidth = 2;
            clsSnippet pSnippet = new clsSnippet();
            m_pTwoLegendGroup = new LegendGroupsClass();
            //Create Legend Group
            ILegendGroup pLegendGroup = new LegendGroup();

            pLegendGroup.Heading = null;
            pLegendGroup.Editable = true;
            pLegendGroup.Visible = true;

            //intBrksCount = (arrClassBrks.Length * 2) - 3;
            intBrksCount = (pCSMembers.ClassBrks.Length * 2) - 3;

            //For Values
            ILegendClass legendClass = new LegendClass();

            ISimpleFillSymbol pSimpleFillSym = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSym = new SimpleLineSymbolClass();
            int intValueIdx = 0;
            int intSepLineIdx = 0;

            int intLastIdx = pCSMembers.ClassBrks.Length - 2;

            legendClass = new LegendClass();
            legendClass.Label = "Estimates";
            pSimpleFillSym = new SimpleFillSymbolClass();
            pSimpleFillSym.Color = (IColor)pSnippet.getRGB(pCSMembers.Colors[intLastIdx, 0], pCSMembers.Colors[intLastIdx, 1], pCSMembers.Colors[intLastIdx, 2]);
            legendClass.Symbol = pSimpleFillSym as ISymbol;
            pLegendGroup.AddClass(legendClass);

            legendClass = new LegendClass();
            //legendClass.Label = "Separability Value";
            legendClass.Label = "-- Separability Value";
            pLineSym = new SimpleLineSymbolClass();
            //pLineSym.Color = (IColor)pSnippet.getRGB(0, 0, 0);
            pLineSym.Color = (IColor)pSnippet.getRGB(255, 255, 255);
            pLineSym.Width = dblLinewidth;
            legendClass.Symbol = (ISymbol)pLineSym;
            pLegendGroup.AddClass(legendClass);
            m_pTwoLegendGroup.Add(pLegendGroup);
            
            //Create Legend Group
            pLegendGroup = new LegendGroup();
            //pLegendGroup.Heading = "-----------------";
            pLegendGroup.Heading = "_____________";
            pLegendGroup.Editable = true;
            pLegendGroup.Visible = true;

            for (int i = 0; i < intBrksCount; i++)
            {

                legendClass = new LegendClass();
                if (i % 2 == 0)
                {

                    intValueIdx = i / 2;
                    if (i == 0)
                    {
                        legendClass.Label = Math.Round(pCSMembers.ClassBrks[intValueIdx], intRoundingDigits).ToString() + " - " + (Math.Round(pCSMembers.ClassBrks[intValueIdx + 1], intRoundingDigits)).ToString();
                    }
                    else
                        legendClass.Label = (Math.Round(pCSMembers.ClassBrks[intValueIdx], intRoundingDigits) + (1 * Math.Pow(0.1, intRoundingDigits))).ToString() + " - " + (Math.Round(pCSMembers.ClassBrks[intValueIdx + 1], intRoundingDigits)).ToString();


                    pSimpleFillSym = new SimpleFillSymbolClass();
                    pSimpleFillSym.Color = (IColor)pSnippet.getRGB(pCSMembers.Colors[intValueIdx, 0], pCSMembers.Colors[intValueIdx, 1], pCSMembers.Colors[intValueIdx, 2]);
                    legendClass.Symbol = pSimpleFillSym as ISymbol;
                }
                else
                {
                    intValueIdx = (i - 1) / 2;
                    legendClass.Label = "-- " + Math.Round(pCSMembers.Serabability[intValueIdx], intRoundingDigits).ToString();

                    intSepLineIdx = Convert.ToInt32(Math.Floor(pCSMembers.Serabability[intValueIdx] * 10));
                    pLineSym = new SimpleLineSymbolClass();
                    //pLineSym.Color = (IColor)pSnippet.getRGB(arrSepLineColors[intSepLineIdx, 0], arrSepLineColors[intSepLineIdx, 1], arrSepLineColors[intSepLineIdx, 2]);
                    pLineSym.Color = (IColor)pSnippet.getRGB(255, 255, 255);
                    pLineSym.Width = dblLinewidth;
                    
                    //ITextSymbol
                    //ITextSymbol textSymbol = new TextSymbol();


                    //legendClass.Format.LabelSymbol = textSymbol;


                    legendClass.Symbol = (ISymbol)pLineSym;

                }

                pLegendGroup.AddClass(legendClass);
            }
            m_pTwoLegendGroup.Add(pLegendGroup);

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
                clsSnippet pSnippet = new clsSnippet();
                IFeature pFeat = null;

                intBrksCount = pCSMembers.ClassBrks.Length - 1;
                ISimpleFillSymbol pSFillSym = new SimpleFillSymbolClass();

                ICartographicLineSymbol pOutLines = new CartographicLineSymbol();
                pOutLines.Width = dblLineWidth;
                pOutLines.Color = (IColor)pLineRgb;

                pFeat = Cursor.NextFeature();
                IRgbColor pRGBColor = null;
                int i = 0;
                double dblValue = 0;

                //Start Loop
                while (pFeat != null)
                {
                    dblValue = Convert.ToDouble(pFeat.get_Value(intEstIdx));
                    IFeatureDraw pFeatDraw = (IFeatureDraw)pFeat;
                    pSFillSym = new SimpleFillSymbolClass();
                    pSFillSym.Outline = pOutLines;
                    pRGBColor = null;

                    for (int j = 0; j < intBrksCount; j++)
                    {
                        if (j == 0)
                        {
                            if (dblValue >= pCSMembers.ClassBrks[0] && dblValue <= pCSMembers.ClassBrks[1])
                            {
                                pRGBColor = pSnippet.getRGB(pCSMembers.Colors[0, 0], pCSMembers.Colors[0, 1], pCSMembers.Colors[0, 2]);
                            }
                        }
                        else
                        {
                            if (dblValue > pCSMembers.ClassBrks[j] && dblValue <= pCSMembers.ClassBrks[j + 1])
                            {

                                pRGBColor = pSnippet.getRGB(pCSMembers.Colors[j, 0], pCSMembers.Colors[j, 1], pCSMembers.Colors[j, 2]);
                            }
                        }
                    }
                    pSFillSym.Color = (IColor)pRGBColor;
                    Display.SetSymbol((ISymbol)pSFillSym);
                    pFeatDraw.Draw(esriDrawPhase.esriDPGeography, Display, (ISymbol)pSFillSym, true,
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
            if (queryFilter == null)
                queryFilter = new QueryFilterClass();
            queryFilter.AddField(fc.OIDFieldName);

            intEstIdx = fc.FindField(strValueFldName);

            queryFilter.AddField(strValueFldName);
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

        #region LegendInfo Member
        int ILegendInfo.LegendGroupCount
        {
            get { return 2; }
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

            ILegendGroup pLegendGroup = m_pTwoLegendGroup.get_Element(Index);


            m_pLegendGroup = pLegendGroup;
            return m_pLegendGroup;
        }
        #endregion

    }
        
    
}
