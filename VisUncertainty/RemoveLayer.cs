
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;


namespace VisUncertainty
{
    class RemoveLayer : BaseCommand
    {
        private IMapControl3 m_mapControl;

        public RemoveLayer()
        {
            base.m_caption = "Remove Layer";
        }

        public override void OnClick()
        {
            ILayer layer = (ILayer)m_mapControl.CustomProperty;
            IFeatureLayer pFlayer = (IFeatureLayer)layer;
            //MainForm mForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            //clsSnippet pSnippet = new clsSnippet();
            clsBrusingLinking pBL = new clsBrusingLinking();
            string strWarning = pBL.RemoveWarningByBrushingTechnique(pFlayer);
            if (strWarning == string.Empty)
                m_mapControl.Map.DeleteLayer(layer);
            else
            {

                DialogResult dialogResult = MessageBox.Show("The layer is related with plots below:" + Environment.NewLine + strWarning
                    , "Do you want to remove the layer?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    m_mapControl.Map.DeleteLayer(layer);
                    pBL.CloseAllRelatedPlots(pFlayer);
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl3)hook;
        }

    }
}
