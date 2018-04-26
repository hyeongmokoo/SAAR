using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace VisUncertainty
{
    public partial class frmExportChart : Form
    {
        public Chart thisChart;
        public Form thisForm;
        public Panel thisPanel;

        private int m_intFilterIndex;

        public frmExportChart()
        {
            InitializeComponent();
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            if (sfdExportImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutputPath.Text = string.Concat(sfdExportImage.FileName);
                m_intFilterIndex = sfdExportImage.FilterIndex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string strOutputPath = txtOutputPath.Text;
                int intHeight = Convert.ToInt32(nudHeight.Value);
                int intWidth = Convert.ToInt32(nudWidth.Value);

                //Create clone chart to change the size of original chart. 
                System.IO.MemoryStream myStream = new System.IO.MemoryStream();

                if (thisChart != null)
                {
                    Chart pCloneChart = new Chart();
                    thisChart.Serializer.Save(myStream);
                    pCloneChart.Serializer.Load(myStream);




                    //// save to a bitmap
                    pCloneChart.Size = new System.Drawing.Size(intWidth, intHeight);

                    //////ForAAG
                    //pCloneChart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 50);
                    //pCloneChart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                    //pCloneChart.ChartAreas[0].AxisX.Title = "Median Household Income";
                    //pCloneChart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                    //pCloneChart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 50);
                    //pCloneChart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;



                    Bitmap bmp = new Bitmap(intWidth, intHeight);
                    float fltResolution = Convert.ToSingle(nudResolution.Value);
                    bmp.SetResolution(fltResolution, fltResolution);

                    pCloneChart.DrawToBitmap(bmp, new Rectangle(0, 0, intWidth, intHeight));

                    switch (m_intFilterIndex)
                    {
                        case 1:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 2:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Emf);
                            break;
                        case 3:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case 4:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case 5:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case 6:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                    }
                }
                else
                {
                    thisForm.Size = new System.Drawing.Size(intWidth, intHeight);

                    Bitmap bmp = new Bitmap(intWidth, intHeight);
                    float fltResolution = Convert.ToSingle(nudResolution.Value);
                    bmp.SetResolution(fltResolution, fltResolution);

                    thisForm.DrawToBitmap(bmp, new Rectangle(0, 0, intWidth, intHeight));

                    switch (m_intFilterIndex)
                    {
                        case 1:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 2:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Emf);
                            break;
                        case 3:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case 4:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case 5:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case 6:
                            bmp.Save(strOutputPath, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                    }
                }
                MessageBox.Show("The export completed successfully (" + strOutputPath + ")");
            }
            catch (Exception ex)
            {
                frmErrorLog pfrmErrorLog = new frmErrorLog();pfrmErrorLog.ex = ex; pfrmErrorLog.ShowDialog();
                return;
            }
        }
    }
}
