using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//The class is made for storing Rendering information to evalute 
namespace VisUncertainty
{
    public class clsRenderedLayers
    {
        public string strLayerName;
        public string strValueFldName;
        public string strUncernFldName;
        public int[,] arrColors;
        public double[] ClassBreaks;
        public string strClassificationType;
    }
}
