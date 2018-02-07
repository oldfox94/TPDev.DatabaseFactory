using DatabaseFactory;
using DbInterface.Helpers;
using DbInterface.Models;
using DbNotifyer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using DbType = DbInterface.Models.DbType;

namespace TestApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbFactory m_dbFactory { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            var connectionData = new DbConnectionData();

            //SQL
            connectionData.ServerName = "localhost";
            connectionData.Name = "";
            connectionData.User = "";
            connectionData.Password = "";
            connectionData.Instance = ""; //Leave empty for default Instance
            //m_dbFactory = new DbFactory(DbType.SQL, connectionData); //Uncomment for SQL

            //SQLite
            connectionData.Path = Environment.CurrentDirectory;
            connectionData.Name = "D_TestApp.db";
            //m_dbFactory = new DbFactory(DbType.SQLite, connectionData); //Uncomment for SQLite

            //MySQL
            connectionData.ServerName = "";
            connectionData.Port = "";
            connectionData.Name = "";
            connectionData.User = "";
            connectionData.Password = "";
            //m_dbFactory = new DbFactory(DbType.MySQL, connectionData); //Uncomment for MySQL

            //Oracle
            connectionData.ServerName = "";
            connectionData.Port = "";
            connectionData.Name = "";
            connectionData.User = "";
            connectionData.Password = "";
            //m_dbFactory = new DbFactory(DbType.Oracle, connectionData); //Uncomment for Oracle

            #region Init Logger
            m_dbFactory.InitLogger("DbFactoryLog");
            m_dbFactory.InitNotifyIcon(new NotifyData { Title = "TestApp", NotifyOnError = true, NotifyOnInfo = true });
            #endregion
        }

        private void CreateTable()
        {
            var columns = GetSampleColumns();
            m_dbFactory.Insert.CreateTable("TestTbl", columns);
        }

        private List<ColumnData> GetSampleColumns()
        {
            var colList = new List<ColumnData>();

            colList.Add(new ColumnData { Name = "Pk", Type = DbDEF.VarchrNotNullPk(50) });
            colList.Add(new ColumnData { Name = "Name", Type = DbDEF.VarchrNull(100) });
            colList.Add(new ColumnData { Name = "Text", Type = DbDEF.TxtNull });

            return colList;
        }

        private void OnCreateSampleTableClick(object sender, RoutedEventArgs e)
        {
            CreateTable();
            RefreshDataTbl();
        }

        private void RefreshDataTbl()
        {
            var tbl = GetTable();
            if (tbl == null) return;

            Grid.ItemsSource = null;
            Grid.ItemsSource = tbl.DefaultView;
        }

        private int m_DataSetCounter;
        private void UpdateDataSet()
        {
            var ds = m_dbFactory.Get.GetDataSet(new List<string> { @"SELECT * FROM TestTbl" }, "TestDs");
            if (ds == null || ds.Tables.Count <= 0) return;

            var tbl = ds.Tables[0];
            if (tbl.Rows.Count <= 0)
            {
                var dr = tbl.NewRow();
                dr["Pk"] = Guid.NewGuid().ToString();
                dr["Name"] = "Created with UpdateDataSet";
                dr["Text"] = "Create a new row";
                tbl.Rows.Add(dr);
            }
            else
            {
                var dr = tbl.Rows[0];
                if (dr == null) return;

                m_DataSetCounter++;
                dr["Name"] = "Updatet with UpdateDataSet Count:" + m_DataSetCounter;
                dr["Text"] = "Updates always the first row";
            }

            m_dbFactory.Update.UpdateDataSet(ds);
        }

        private int m_DataTableCounter;
        private void UpdateDataTable()
        {
            var tbl = m_dbFactory.Get.GetTable(@"SELECT * FROM TestTbl");

            if (tbl.Rows.Count <= 0)
            {
                var dr = tbl.NewRow();
                dr["Pk"] = Guid.NewGuid().ToString();
                dr["Name"] = "Created with UpdateDataTable";
                dr["Text"] = "Create a new row";
                tbl.Rows.Add(dr);
            }
            else
            {
                var dr = tbl.Rows[0];
                if (dr == null) return;

                m_DataTableCounter++;
                dr["Name"] = "Updated with UpdateDataTable Count: " + m_DataTableCounter;
                dr["Text"] = "Updates always the first row";
            }
            m_dbFactory.Update.UpdateTable(tbl);
        }

        private DataTable GetTable()
        {
            return m_dbFactory.Get.GetTable("TestTbl", null);
        }

        private void OnUpdateWithDataSetClick(object sender, RoutedEventArgs e)
        {
            UpdateDataSet();
            RefreshDataTbl();
        }

        private void OnUpdateWithDataTableClick(object sender, RoutedEventArgs e)
        {
            UpdateDataTable();
            RefreshDataTbl();
        }

        private void UpdateOneValue()
        {
            m_dbFactory.Update.UpdateOneValue("TestTbl", "Name", "Update by OneValue", ConvertionHelper.WHERE("Text", "Updates always the first row"));
        }

        private void OnUpdateOneValueClick(object sender, RoutedEventArgs e)
        {
            UpdateOneValue();
            RefreshDataTbl();
        }

        private void OnGetTableClick(object sender, RoutedEventArgs e)
        {
            RefreshDataTbl();
        }
    }
}
