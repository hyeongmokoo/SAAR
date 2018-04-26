using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

namespace VisUncertainty
{
    public partial class frmNewTemp : Form
    {
        private ESRI.ArcGIS.Controls.AxMapControl[] axMapControls;

        public frmNewTemp()
        {
            InitializeComponent();
            axMapControls = new AxMapControl[4];
            InitialMapControls(2, 2);


        }
        private void InitialMapControls(int intRCount, int intCCount)
        {
            int intTotal = intRCount * intCCount;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNewTemp));

            int k =0;
            int intXSize = 200;
            int intYSize = 200;
            int intXIni = 21;
            int intYIni = 12;

            for (int i = 0; i < intRCount; i++)
            {
                for (int j = 0; j < intCCount; j++)
                {
                    string strName = "axMapControl"+k.ToString();
                    int intX = intXIni + (intXSize * i);
                    int intY = intYIni + (intYSize * j);

                    this.axMapControls[k] = new ESRI.ArcGIS.Controls.AxMapControl();
                    ((System.ComponentModel.ISupportInitialize)(this.axMapControls[k])).BeginInit();
                   
                    this.SuspendLayout();

                    this.axMapControls[k].Location = new System.Drawing.Point(intX, intY);
                    this.axMapControls[k].Name = strName;
                    this.axMapControls[k].OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject(strName +".OcxState")));
                    this.axMapControls[k].Size = new System.Drawing.Size(intXSize, intYSize);
                    this.axMapControls[k].TabIndex = k;
                    
                    this.Controls.Add(this.axMapControls[k]);

                    ((System.ComponentModel.ISupportInitialize)(this.axMapControls[k])).EndInit();
                    k++;
                }
            }

            this.ResumeLayout(false);

        }
    }
}
