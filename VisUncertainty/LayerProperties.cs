

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace VisUncertainty
{
    class LayerProperty : BaseCommand
    {
        private IMapControl3 m_mapControl;

        public LayerProperty()
        {
            base.m_caption = "Layer Properties";
        }

        public override void OnClick()
        {
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            frmProperties2 pfrmPropeties = new frmProperties2();
            pfrmPropeties.mlayer = pLayer;
            pfrmPropeties.ShowDialog();

        }

        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl3)hook;
        }
    }
}
