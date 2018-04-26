using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisUncertainty
{
    class clsClassSeperabilityMembers
    {
        private double[] arrClassBrks; 
        public double[] ClassBrks
        { 
            get { return arrClassBrks;}
            set { arrClassBrks = value;}
        }

        private double[] arrSeparability;
        public double[] Serabability
        {
            get { return arrSeparability; }
            set { arrSeparability = value; }
        }

        private int[,] arrColors;
        public int[,] Colors
        {
            get { return arrColors; }
            set { arrColors = value; }
        }

        //private int[,] arrSepLineColors;
        ////public int[,] SepLineColors
        ////{
        ////    get { return arrSepLineColors; }
        ////    set { arrSepLineColors = value; }
        ////}
    }
}
