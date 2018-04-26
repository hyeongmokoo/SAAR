using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Display;

namespace VisUncertainty
{
    interface IClassSeparabilityRenderer
    {
        //double[] arrValue { get; set; }
        string strValueFldName { get; set; }
        
        //double[] arrClassBrks { get; set; }
        clsClassSeperabilityMembers pCSMembers { get; set; }
        //double[] arrSeparability { get; set; }

        //int[,] arrColors { get; set; }
        //int[,] arrSepLineColors { get; set; }
        
        IRgbColor m_pLineRgb { get; set; }
        double dblLineWidth { get; set; }
        
        int intRoundingDigits { get; set; }
        string strHeading { get; set; }

        void CreateLegend();
    }
}
