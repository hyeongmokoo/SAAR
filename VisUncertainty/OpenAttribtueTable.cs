using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.GeoDatabaseUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

using ESRI.ArcGIS.ArcMap;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace VisUncertainty
{
    class OpenAttriTable : BaseCommand
    {
        private IMapControl3 m_mapControl;

        public OpenAttriTable()
        {
            base.m_caption = "Open Attribute Table";
        }

        public override void OnClick()
        {
            ILayer pLayer = (ILayer)m_mapControl.CustomProperty;
            //IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            //string strLayerName = pFLayer.Name;
            //IAttributeTable pAttributeTable = pFLayer as IAttributeTable;
            //ITable pTable = pAttributeTable as ITable;
            frmAttributeTable pfrmAttTable = new frmAttributeTable();
            LoadingAttributeTable(pLayer, pfrmAttTable);
            pfrmAttTable.Show();
            //IQueryFilter pQFilter = new QueryFilterClass();
            //pQFilter = null;

            ////pSnippet.funcTableView(pLayer, pTable, pfrmTableView, pQFilter);

            //System.Data.DataTable shpDataTable; 
            //shpDataTable = new DataTable("shpDataTable");

            ////IQueryFilter que = new QueryFilterClass();
            //ICursor pCursor = pTable.Search(pQFilter, true);
            //IRow pRow = pCursor.NextRow();
            //if (pRow != null)
            //{
            //    for (int i = 0; i < pRow.Fields.FieldCount; i++)
            //    {

            //        DataColumn column;
            //        column = new DataColumn();
            //        column.ColumnName = pRow.Fields.get_Field(i).Name;
            //        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeOID) column.DataType = System.Type.GetType("System.Int32");
            //        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeInteger) column.DataType = System.Type.GetType("System.Int32");
            //        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeSingle) column.DataType = System.Type.GetType("System.Single");
            //        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeDouble) column.DataType = System.Type.GetType("System.Double");
            //        if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeSmallInteger) column.DataType = System.Type.GetType("System.Int16");
            //        Debug.WriteLine(i + " " + pRow.Fields.get_Field(i).Name + " " + pRow.Fields.Field[i].Type);
            //        shpDataTable.Columns.Add(column);

            //    }
            //    while (pRow != null)
            //    {
            //        DataRow pDataRow = shpDataTable.NewRow();
            //        for (int j = 0; j < pCursor.Fields.FieldCount; j++) 
            //            pDataRow[j] = pRow.get_Value(j);
            //        shpDataTable.Rows.Add(pDataRow);
            //        pRow = pCursor.NextRow();
            //    }
            //}


            

            //pfrmAttTable.dgvAttTable.DataSource = shpDataTable;
            //pfrmAttTable.Text = "Attribute of " + strLayerName;


            //for (int i = 0; i < shpDataTable.Columns.Count; i++)
            //{
            //    if (i != 1)
            //    {
            //        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Int16")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Int32")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Single")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //        if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Double")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    }
            //}

            //pfrmAttTable.dgvAttTable.Columns["Shape"].Visible = false;
            //pfrmAttTable.dgvAttTable.AllowUserToAddRows = false; 
            //pfrmAttTable.pLayer = pLayer;
            //pfrmAttTable.ShowDialog();

        }

        public override void OnCreate(object hook)
        {
            m_mapControl = (IMapControl3)hook;
        }
        private void LoadingAttributeTable(ILayer pLayer, frmAttributeTable pfrmAttTable)
        {
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            string strLayerName = pFLayer.Name;
            IAttributeTable pAttributeTable = pFLayer as IAttributeTable;
            ITable pTable = pAttributeTable as ITable;

            IQueryFilter pQFilter = new QueryFilterClass();
            pQFilter = null;

            //pSnippet.funcTableView(pLayer, pTable, pfrmTableView, pQFilter);

            System.Data.DataTable shpDataTable;
            shpDataTable = new DataTable("shpDataTable");

            //IQueryFilter que = new QueryFilterClass();
            ICursor pCursor = pTable.Search(pQFilter, true);
            IRow pRow = pCursor.NextRow();
            if (pRow != null)
            {
                for (int i = 0; i < pRow.Fields.FieldCount; i++)
                {

                    DataColumn column;
                    column = new DataColumn();
                    column.ColumnName = pRow.Fields.get_Field(i).Name;
                    if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeOID) column.DataType = System.Type.GetType("System.Int32");
                    if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeInteger) column.DataType = System.Type.GetType("System.Int32");
                    if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeSingle) column.DataType = System.Type.GetType("System.Single");
                    if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeDouble) column.DataType = System.Type.GetType("System.Double");
                    if (pRow.Fields.Field[i].Type == esriFieldType.esriFieldTypeSmallInteger) column.DataType = System.Type.GetType("System.Int16");
                    Debug.WriteLine(i + " " + pRow.Fields.get_Field(i).Name + " " + pRow.Fields.Field[i].Type);
                    shpDataTable.Columns.Add(column);

                }
                while (pRow != null)
                {
                    DataRow pDataRow = shpDataTable.NewRow();
                    for (int j = 0; j < pCursor.Fields.FieldCount; j++)
                        pDataRow[j] = pRow.get_Value(j);
                    shpDataTable.Rows.Add(pDataRow);
                    pRow = pCursor.NextRow();
                }
            }




            pfrmAttTable.dgvAttTable.DataSource = shpDataTable;
            pfrmAttTable.Text = "Attribute of " + strLayerName;


            for (int i = 0; i < shpDataTable.Columns.Count; i++)
            {
                if (i != 1)
                {
                    if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Int16")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Int32")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Single")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (shpDataTable.Columns[i].DataType == System.Type.GetType("System.Double")) pfrmAttTable.dgvAttTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            pfrmAttTable.dgvAttTable.Columns["Shape"].Visible = false;
            pfrmAttTable.dgvAttTable.AllowUserToAddRows = false;
            pfrmAttTable.m_pLayer = pLayer;
            pfrmAttTable.Refresh();
        }
    }
}
