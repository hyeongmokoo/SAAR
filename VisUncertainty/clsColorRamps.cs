using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace VisUncertainty
{
    class clsColorRamps
    {
        //ColorRamp names
        public string[] colorRampNames = new string[]{"Blue Light to Dark", "Green Light to Dark",
            "Orange Light to Dark", "Red Light to Dark", "Blue to Red", "Green to Purple", 
            "Green to Red", "Purple to Brown", "Custom"};
        
        //ColorRamps Colors, it is linked with MultiPartColorRamp Functions.
        public List<Color>[] CreateColorList()
        {
            int intColorRampCount = colorRampNames.Length;
            List<Color>[] colorList = new List<Color>[intColorRampCount];

            //Blue Light to Dark
            Color pColor1 = Color.FromArgb(239, 243, 255);
            Color pColor2 = Color.FromArgb(8, 81, 156);
            List<Color> colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);

            colorList[0] = colors;

            //"Green Light to Dark"
            pColor1 = Color.FromArgb(237, 248, 233);
            pColor2 = Color.FromArgb(0, 109, 44);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);

            colorList[1] = colors;
                            
            //"Orange Light to Dark":
            pColor1 = Color.FromArgb(254, 237, 222);
            pColor2 = Color.FromArgb(166, 54, 3);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);

            colorList[2] = colors;
            
           //Red Light to Dark":
            pColor1 = Color.FromArgb(254, 229, 217);
            pColor2 = Color.FromArgb(165, 15, 21);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);

            colorList[3] = colors;
            
            //"Blue to Red":
            pColor1 = Color.FromArgb(49, 54, 149);
            pColor2 = Color.FromArgb(255, 255, 191);
            Color pColor3 = Color.FromArgb(165, 0, 38);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);
            colors.Add(pColor3);

            colorList[4] = colors;
            
            //"Green to Purple":
            pColor1 = Color.FromArgb(0, 68, 27);
            pColor2 = Color.FromArgb(247, 247, 247);
            pColor3 = Color.FromArgb(64, 0, 75);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);
            colors.Add(pColor3);
            
            colorList[5] = colors;

            //"Green to Red":
            pColor1 = Color.FromArgb(0, 104, 55);
            pColor2 = Color.FromArgb(255, 255, 191);
            pColor3 = Color.FromArgb(165, 0, 38);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);
            colors.Add(pColor3);
            
            colorList[6] = colors;
            
            //"Purple to Brown":
            pColor1 = Color.FromArgb(45, 0, 75);
            pColor2 = Color.FromArgb(247, 247, 247);
            pColor3 = Color.FromArgb(127, 59, 8);
            colors = new List<Color>();
            colors.Add(pColor1);
            colors.Add(pColor2);
            colors.Add(pColor3);

            colorList[7] = colors;

            //"Custome":
            pColor1 = Color.White;

            colors = new List<Color>();
            colors.Add(pColor1);

            colorList[8] = colors;

            return colorList;
        }
    }
}
