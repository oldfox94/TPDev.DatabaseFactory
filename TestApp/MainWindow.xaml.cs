using DatabaseFactory;
using DbInterface;
using DbInterface.Helpers;
using DbInterface.Models;
using DbLogger.Models;
using DbNotifyer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DbType = DbInterface.Models.DbType;

namespace TestApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbConnectionData m_connectionData;
        DbFactory m_dbFactory { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            m_connectionData = new DbConnectionData();

            //SQL
            m_connectionData.ServerName = "";
            m_connectionData.Name = "TestDb";
            m_connectionData.User = "";
            m_connectionData.Password = "";
            m_connectionData.Instance = ""; //Leave empty for default Instance
            m_dbFactory = new DbFactory(DbType.SQL, m_connectionData); //Uncomment for SQL

            ////SQLite
            //m_connectionData.Path = Environment.CurrentDirectory;
            //m_connectionData.Name = "D_TestApp.db";
            //m_dbFactory = new DbFactory(DbType.SQLite, m_connectionData); //Uncomment for SQLite

            ////MySQL
            //m_connectionData.ServerName = "";
            //m_connectionData.Port = "";
            //m_connectionData.Name = "";
            //m_connectionData.User = "";
            //m_connectionData.Password = "";
            //m_dbFactory = new DbFactory(DbType.MySQL, m_connectionData); //Uncomment for MySQL

            ////Oracle
            //m_connectionData.ServerName = "";
            //m_connectionData.Port = "";
            //m_connectionData.Name = "";
            //m_connectionData.User = "";
            //m_connectionData.Password = "";
            //m_dbFactory = new DbFactory(DbType.Oracle, m_connectionData); //Uncomment for Oracle

            #region Init Logger
            m_dbFactory.InitLogger("DbFactoryLog", debugLevel: DebugLevelConstants.VeryHigh);
            m_dbFactory.InitNotifyIcon(new NotifyData { Title = "TestApp", NotifyOnError = true, NotifyOnInfo = true });
            //LogTester();
            #endregion
        }

        private void LogTester()
        {
            SLLog.WriteInfo("LogTester", "TestInfo!");
            SLLog.WriteWarning("LogTester", ToString(), "TestWarning");
            try
            {
                throw new ApplicationException("TestException!");
            }
            catch(Exception ex)
            {
                SLLog.WriteError(new LogData
                {
                    FunctionName = "LogTester",
                    Source = ToString(),
                    Ex = ex,
                });
            }

            Task.Run(() => 
            {
                SLLog.WriteInfo("LogTester", "TestInfo (Thread)!");
                SLLog.WriteWarning("LogTester", ToString(), "TestWarning (Thread)!");
                try
                {
                    throw new ApplicationException("TestException (Thread)!");
                }
                catch (Exception ex)
                {
                    SLLog.WriteError(new LogData
                    {
                        FunctionName = "LogTester",
                        Source = ToString(),
                        Ex = ex,
                    });
                }
            });
        }

        private void CreateDatabase()
        {
            if (!m_dbFactory.Check.DatabaseExists(m_connectionData.Name))
            {
                m_dbFactory.Insert.CreateDatabase(m_connectionData.Name);
            }
        }

        private void CreateTable()
        {
            CreateDatabase();

            var tbl1Columns = GetSampleTable1Columns();
            m_dbFactory.Insert.CreateTable("TestTbl", tbl1Columns);

            var tbl2Columns = GetSampleTable2Columns();
            m_dbFactory.Insert.CreateTable("TestTbl2", tbl2Columns);
        }

        private void RenewDatabase()
        {
            var tbl1Columns = GetSampleTable1Columns();
            m_dbFactory.Execute.RenewTbl("TestTbl", tbl1Columns);

            var tbl2Columns = GetSampleTable2Columns();
            m_dbFactory.Execute.RenewTbl("TestTbl2", tbl2Columns);
        }

        private List<ColumnData> GetSampleTable1Columns()
        {
            var colList = new List<ColumnData>();

            colList.Add(new ColumnData { Name = "Pk", Type = DbDEF.VarchrNotNullPk(50) });
            colList.Add(new ColumnData { Name = "Name", Type = DbDEF.VarchrNull(100) });
            colList.Add(new ColumnData { Name = "Text", Type = DbDEF.TxtNull });

            return colList;
        }

        private List<ColumnData> GetSampleTable2Columns()
        {
            var colList = new List<ColumnData>();

            colList.Add(new ColumnData { Name = "Pk", Type = DbDEF.VarchrNotNullPk(50) });
            colList.Add(new ColumnData { Name = "ParentPk", Type = DbDEF.VarchrNotNull(50), FkList = new List<FkData> { new FkData { RefTable = "TestTbl", RefColumn = "Pk" } } });
            colList.Add(new ColumnData { Name = "Name", Type = DbDEF.VarchrNull(100) });
            colList.Add(new ColumnData { Name = "Text", Type = DbDEF.TxtNull });

            return colList;
        }


        private void OnCreateDbClick(object sender, RoutedEventArgs e)
        {
            CreateDatabase();
        }

        private void OnRenewDbClick(object sender, RoutedEventArgs e)
        {
            RenewDatabase();
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
