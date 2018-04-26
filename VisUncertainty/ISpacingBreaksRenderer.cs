using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Display;

namespace VisUncertainty
{
    public interface ISpacingBreaksRenderer
    {
        double[] arrValue { get; set; }
        double[] arrClassBrks { get; set; }

        IRgbColor m_pLineRgb { get; set; }
        double dblLineWidth { get; set; }
        double dblLineAngle { get; set; }
        double dblFromSep { get; set; }
        double dblToSep { get; set; }
        int intRoundingDigits { get; set; }
        string strHeading { get; set; }

        void CreateLegend();
    }
}
