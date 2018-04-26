using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisUncertainty
{
    class clsRPackageNames
    {
        public bool[] CheckedRequiredPackages(string[] strInstalledPackageName)
        {
            try
            {
                int intReqPackCnts = RequiredPackageNames.Length;
                int intInstalledPackCnts = strInstalledPackageName.Length;
                bool[] blnExist = new bool[intReqPackCnts];

                for (int i = 0; i < intReqPackCnts; i++)
                {
                    string strPackName = RequiredPackageNames[i];
                    blnExist[i] = false;

                    for (int j = 0; j < intInstalledPackCnts; j++)
                    {
                        if(strPackName == strInstalledPackageName[j])
                            blnExist[i] = true;
                    }
                }
                return blnExist;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return null;
            }
        }

        public string[] RequiredPackageNames = new string[] { "MASS", "geoR", "car", "spdep", "maptools","deldir", "rgeos", "e1071"};
    }
}
