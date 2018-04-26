using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisUncertainty
{
    class clsBlockNames
    {
        public bool BlockPreDeterminedName(string strFldNM)
        {
            try
            {
                bool blnExist = false;

                for (int i = 0; i < DeterminedName.Length; i++)
                {
                    if (strFldNM == DeterminedName[i])
                    {
                        blnExist = true;
                        MessageBox.Show("The field name is reserved");
                    }
                }
                return blnExist;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
                return true;
            }
        }

        private static string[] DeterminedName = new string[] { "esf_resi", "sfilter", "ii", "Zi", "Pr" };
    }
}
