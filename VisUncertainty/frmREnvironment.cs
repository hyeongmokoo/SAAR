using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RDotNet;
using RDotNet.NativeLibrary;

namespace VisUncertainty
{
    public partial class frmREnvironment : Form
    {
        private MainForm m_pForm;
        private REngine m_pEngine;

        public frmREnvironment()
        {
            InitializeComponent();
            m_pForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            m_pEngine = m_pForm.pEngine;

            txtRHomePath.Lines = m_pForm.LibHome;

            UpdateAvailiability(m_pForm.blnsInstalledPcks);
        }

        private void btnAddLibHome_Click(object sender, EventArgs e)
        {
            if (fbdLibrariesHome.ShowDialog() == DialogResult.OK)
            {
                string strNewPath = fbdLibrariesHome.SelectedPath;
                string strNewLibPath = strNewPath.Replace(@"\", @"/");
                m_pEngine.Evaluate(".libPaths('" + strNewLibPath + "')");
                //m_pEngine.Evaluate(".Library.site <- file.path('" + strNewLibPath + "')"); //Same results with the above
                //m_pEngine.Evaluate(".libPaths(c('" + strNewLibPath + "', .Library.site, .Library))");
                ////m_pEngine.Evaluate(".libPaths(" + strNewLibPath + ")");
                m_pForm.LibHome = m_pEngine.Evaluate(".libPaths()").AsCharacter().ToArray();
                txtRHomePath.Lines = m_pForm.LibHome;
            }
        }

        private void UpdateAvailiability(bool[] blnsInstalledPcks)
        {
            lvReqPacks.BeginUpdate();
            lvReqPacks.Items.Clear();

            clsRPackageNames pPckNames = new clsRPackageNames();
            int intCnt = blnsInstalledPcks.Length;

            for (int i = 0; i < intCnt; i++)
            {
                ListViewItem lvi = new ListViewItem(pPckNames.RequiredPackageNames[i]);
                lvi.SubItems.Add(blnsInstalledPcks[i].ToString());

                lvReqPacks.Items.Add(lvi);
            }
            lvReqPacks.EndUpdate();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            m_pEngine.Evaluate("ip <- installed.packages()").AsCharacter();
            string[] installedPackages = m_pEngine.Evaluate("ip[,1]").AsCharacter().ToArray(); //To Check Installed Packages in R
            clsRPackageNames pPckNames = new clsRPackageNames();
            m_pForm.blnsInstalledPcks = pPckNames.CheckedRequiredPackages(installedPackages);

            UpdateAvailiability(m_pForm.blnsInstalledPcks);
        }
    }
}
