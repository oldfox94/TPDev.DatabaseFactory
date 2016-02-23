using DatabaseFactory;
using DbInterface.Models;
using System;
using System.Collections.Generic;
using System.Windows;

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
            connectionData.ServerName = "";
            connectionData.Name = "";
            connectionData.User = "";
            connectionData.Password = "";
            //m_dbFactory = new DbFactory.DbFactory(DbType.SQL, connectionData);

            //SQLite
            connectionData.Path = Environment.CurrentDirectory;
            connectionData.Name = "D_TestApp.db";
            m_dbFactory = new DbFactory(DbType.SQLite, connectionData);

            //MySQL
            connectionData.ServerName = "";
            connectionData.Port = "";
            connectionData.Name = "";
            connectionData.User = "";
            connectionData.Password = "";
            //m_dbFactory = new DbFactory.DbFactory(DbType.MySQL, connectionData);

            m_dbFactory.InitLogger("DbFactoryLog");
        }

        private void CreateTable()
        {
            var columns = GetSampleColumns();
            m_dbFactory.Insert.CreateTable("TestTbl", columns);
        }

        private List<ColumnData> GetSampleColumns()
        {
            var colList = new List<ColumnData>();

            colList.Add(new ColumnData { Name = "Pk", Type = DbDEF.TxtUniNotNullPk});
            colList.Add(new ColumnData { Name = "Name", Type = DbDEF.TxtNull });
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
            var tbl = m_dbFactory.Get.GetTable("SELECT * FROM TestTbl");

            Grid.ItemsSource = null;
            Grid.ItemsSource = tbl.DefaultView;
        }

        private void UpdateDataSet()
        {
            var ds = m_dbFactory.Get.GetDataSet(new List<string> { @"SELECT * FROM TestTbl" }, "TestDs");
            if (ds == null || ds.Tables.Count <= 0) return;

            var tbl = ds.Tables[0];
            if (tbl.Rows.Count <= 0)
            {
                var dr = tbl.NewRow();
                dr["Pk"] = Guid.NewGuid().ToString();
                tbl.Rows.Add(dr);
            };

            var row = tbl.Rows[0];
            if (row == null) return;

            row["Name"] = "Updatet with DataSet";

            m_dbFactory.Update.UpdateDataSet(ds);
        }

        private void OnUpdateWithDataSetClick(object sender, RoutedEventArgs e)
        {
            UpdateDataSet();
            RefreshDataTbl();
        }
    }
}
