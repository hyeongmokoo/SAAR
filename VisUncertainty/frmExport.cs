using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;

namespace VisUncertainty
{
    public partial class frmExport : Form
    {
        public IActiveView pActiveView;
        public frmExport()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (sfdExportMap.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    IExport docExport = null;
                    string strSaveFile = string.Concat(sfdExportMap.FileName);

                    int intDpi = Convert.ToInt32(nudDpi.Value);

                    IActiveView docActiveView = pActiveView;

                    //Can add other formats in here H.Koo 071515
                    if (sfdExportMap.FilterIndex == 1)
                        docExport = new ExportJPEG() as IExport;
                    else if (sfdExportMap.FilterIndex == 2)
                        docExport = new ExportPDF() as IExport;
                    else if (sfdExportMap.FilterIndex == 3)
                        docExport = new ExportTIFF() as IExport;
                    else if (sfdExportMap.FilterIndex == 4)
                        docExport = new ExportBMP() as IExport;

                    IPrintAndExport docPrintAndExport = new PrintAndExportClass();

                    docExport.ExportFileName = strSaveFile;
                    docPrintAndExport.Export(docActiveView, docExport, intDpi, chkClip.Checked, null);
                    docExport.Cleanup();
                    MessageBox.Show("Complete");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
