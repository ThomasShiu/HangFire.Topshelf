using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// SqlHelper操作類
    /// </summary>
    public sealed partial class SqlHelper
    {
        /// <summary>
        /// 連接字串樣版
        /// </summary>
        private string ConnStrFmt = "Data Source={0};Initial Catalog ={1};Persist Security Info=True; User ID={2}; Password={3}";

        /// <summary>
        /// 批量操作每批次記錄數
        /// </summary>
        public static int BatchSize = 2000;

        /// <summary>
        /// 超時時間
        /// </summary>
        public static int CommandTimeOut = 600;

        /// <summary>
        ///初始化SqlHelper實例
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        public SqlHelper( string connectionString )
        {
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// 起始化一個<see cref="SqlHelper"/>實例物件
        /// </summary>
        /// <param name="SrvIP">資料庫主機位置</param>
        /// <param name="DBNM">資料庫名稱</param>
        /// <param name="USR">資料庫登入使用者代碼</param>
        /// <param name="PWD">登入密碼(明碼)</param>
        public SqlHelper( string SrvIP, string DBNM, string USR, string PWD )
        {
            this.ConnectionString = String.Format(ConnStrFmt, SrvIP, DBNM, USR, PWD);
        }

        /// <summary>
        /// 數據庫連接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        #region 實例方法

        #region ExecuteNonQuery

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        public int ExecuteNonQuery( string commandText, params SqlParameter[] parms )
        {
            return ExecuteNonQuery(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        public int ExecuteNonQuery( CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteNonQuery(ConnectionString, commandType, commandText, parms);
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T">返回對像類型</typeparam>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public T ExecuteScalar<T>( string commandText, params SqlParameter[] parms )
        {
            return ExecuteScalar<T>(ConnectionString, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public object ExecuteScalar( string commandText, params SqlParameter[] parms )
        {
            return ExecuteScalar(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public object ExecuteScalar( CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteScalar(ConnectionString, commandType, commandText, parms);
        }

        #endregion ExecuteScalar

        #region ExecuteDataReader

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private SqlDataReader ExecuteDataReader( string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataReader(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private SqlDataReader ExecuteDataReader( CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataReader(ConnectionString, commandType, commandText, parms);
        }

        #endregion ExecuteDataReader

        #region ExecuteDataRow

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行
        /// </summary>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行</returns>
        public DataRow ExecuteDataRow( string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataRow(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行
        /// </summary>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行</returns>
        public DataRow ExecuteDataRow( CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataRow(ConnectionString, commandType, commandText, parms);
        }

        #endregion ExecuteDataRow

        #region ExecuteDataTable

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public DataTable ExecuteDataTable( string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataTable(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public DataTable ExecuteDataTable( CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(ConnectionString, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        ///  執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <param name="order">排序SQL,如"ORDER BY ID DESC"</param>
        /// <param name="pageSize">每頁記錄數</param>
        /// <param name="pageIndex">頁索引</param>
        /// <param name="parms">查詢參數</param>
        /// <param name="query">查詢SQL</param>
        /// <param name="cte">查詢SQL</param>
        /// <returns></returns>
        public DataTable ExecutePageDataTable( string sql, string order, int pageSize, int pageIndex, SqlParameter[] parms = null, string query = null, string cte = null )
        {
            return ExecutePageDataTable(sql, order, pageSize, pageIndex, parms, query, cte);
        }

        #endregion ExecuteDataTable

        #region ExecuteDataSet

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        public DataSet ExecuteDataSet( string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(ConnectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        public DataSet ExecuteDataSet( CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(ConnectionString, commandType, commandText, parms);
        }

        #endregion ExecuteDataSet

        #region 批量操作

        /// <summary>
        /// 大批量數據插入
        /// </summary>
        /// <param name="table">數據表</param>
        public void BulkInsert( DataTable table )
        {
            BulkInsert(ConnectionString, table);
        }

        /// <summary>
        /// 使用MySqlDataAdapter批量更新數據
        /// </summary>
        /// <param name="table">數據表</param>
        public void BatchUpdate( DataTable table )
        {
            BatchUpdate(ConnectionString, table);
        }

        /// <summary>
        /// 分批次批量刪除數據
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <param name="batchSize">每批次刪除記錄行數</param>
        /// <param name="interval">批次執行間隔(秒)</param>
        public void BatchDelete( string sql, int batchSize = 1000, int interval = 1 )
        {
            BatchDelete(ConnectionString, sql, batchSize, interval);
        }

        /// <summary>
        /// 分批次批量更新數據
        /// </summary>
        /// <param name="sql">SQL語句</param>
        /// <param name="batchSize">每批次更新記錄行數</param>
        /// <param name="interval">批次執行間隔(秒)</param>
        public void BatchUpdate( string sql, int batchSize = 1000, int interval = 1 )
        {
            BatchUpdate(ConnectionString, sql, batchSize, interval);
        }

        #endregion 批量操作

        #endregion 實例方法

        #region 靜態方法

        private static void PrepareCommand( SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] parms )
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();

            command.Connection = connection;
            command.CommandTimeout = CommandTimeOut;
            // 設置命令文本(存儲過程名或SQL語句)
            command.CommandText = commandText;
            // 分配事務
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            // 設置命令類型.
            command.CommandType = commandType;
            if (parms != null && parms.Length > 0)
            {
                //預處理SqlParameter參數數組，將為NULL的參數賦值為DBNull.Value;
                foreach (SqlParameter parameter in parms)
                {
                    if (( parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input ) && ( parameter.Value == null ))
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                command.Parameters.AddRange(parms);
            }
        }

        #region ExecuteNonQuery

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        public static int ExecuteNonQuery( string connectionString, string commandText, params SqlParameter[] parms )
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteNonQuery(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        public static int ExecuteNonQuery( string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteNonQuery(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        public static int ExecuteNonQuery( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteNonQuery(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        public static int ExecuteNonQuery( SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteNonQuery(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回影響的行數
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回影響的行數</returns>
        private static int ExecuteNonQuery( SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            int retval = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteNonQuery

        #region ExecuteScalar

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <typeparam name="T">返回對像類型</typeparam>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public static T ExecuteScalar<T>( string connectionString, string commandText, params SqlParameter[] parms )
        {
            object result = ExecuteScalar(connectionString, commandText, parms);
            if (result != null)
            {
                return (T)Convert.ChangeType(result, typeof(T));
                ;
            }
            return default(T);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public static object ExecuteScalar( string connectionString, string commandText, params SqlParameter[] parms )
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteScalar(connection, CommandType.Text, commandText, parms);
            }
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public static object ExecuteScalar( string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteScalar(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public static object ExecuteScalar( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteScalar(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        public static object ExecuteScalar( SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteScalar(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行第一列
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一行第一列</returns>
        private static object ExecuteScalar( SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            object retval = command.ExecuteScalar();
            command.Parameters.Clear();
            return retval;
        }

        #endregion ExecuteScalar

        #region ExecuteDataReader

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="connectionString">數據庫連接</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private static SqlDataReader ExecuteDataReader( string connectionString, string commandText, params SqlParameter[] parms )
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return ExecuteDataReader(connection, null, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="connectionString">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private static SqlDataReader ExecuteDataReader( string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            SqlConnection connection = new SqlConnection(connectionString);
            return ExecuteDataReader(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private static SqlDataReader ExecuteDataReader( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataReader(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private static SqlDataReader ExecuteDataReader( SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataReader(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回只讀數據集
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回只讀數據集</returns>
        private static SqlDataReader ExecuteDataReader( SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            SqlCommand command = new SqlCommand();
            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        #endregion ExecuteDataReader

        #region ExecuteDataRow

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>,返回結果集中的第一行</returns>
        public static DataRow ExecuteDataRow( string connectionString, string commandText, params SqlParameter[] parms )
        {
            DataTable dt = ExecuteDataTable(connectionString, CommandType.Text, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>,返回結果集中的第一行</returns>
        public static DataRow ExecuteDataRow( string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            DataTable dt = ExecuteDataTable(connectionString, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>,返回結果集中的第一行</returns>
        public static DataRow ExecuteDataRow( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            DataTable dt = ExecuteDataTable(connection, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一行
        /// </summary>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>,返回結果集中的第一行</returns>
        public static DataRow ExecuteDataRow( SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            DataTable dt = ExecuteDataTable(transaction, commandType, commandText, parms);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        #endregion ExecuteDataRow

        #region ExecuteDataTable

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public static DataTable ExecuteDataTable( string connectionString, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(connectionString, CommandType.Text, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public static DataTable ExecuteDataTable( string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(connectionString, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public static DataTable ExecuteDataTable( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(connection, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public static DataTable ExecuteDataTable( SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(transaction, commandType, commandText, parms).Tables[0];
        }

        /// <summary>
        /// 獲取空表結構
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="tableName">數據表名稱</param>
        /// <returns>返回結果集中的第一個數據表</returns>
        public static DataTable ExecuteEmptyDataTable( string connectionString, string tableName )
        {
            return ExecuteDataSet(connectionString, CommandType.Text, string.Format("select * from {0} where 1=-1", tableName)).Tables[0];
        }

        /// <summary>
        ///  執行SQL語句,返回結果集中的第一個數據表
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="sql">SQL語句</param>
        /// <param name="order">排序SQL,如"ORDER BY ID DESC"</param>
        /// <param name="pageSize">每頁記錄數</param>
        /// <param name="pageIndex">頁索引</param>
        /// <param name="parms">查詢參數</param>
        /// <param name="query">查詢SQL</param>
        /// <param name="cte">CTE表達式</param>
        /// <returns></returns>
        public static DataTable ExecutePageDataTable( string connectionString, string sql, string order, int pageSize, int pageIndex, SqlParameter[] parms = null, string query = null, string cte = null )
        {
            string psql = string.Format(@"
                                        {3}
                                        SELECT  *
                                        FROM    (
                                                 SELECT ROW_NUMBER() OVER (ORDER BY {1}) RowNumber,*
                                                 FROM   (
                                                         {0}
                                                        ) t
                                                 WHERE  1 = 1 {2}
                                                ) t
                                        WHERE   RowNumber BETWEEN @RowNumber_Begin
                                                          AND     @RowNumber_End", sql, order, query, cte);

            List<SqlParameter> paramlist = new List<SqlParameter>()
            {
                new SqlParameter("@RowNumber_Begin", SqlDbType.Int){ Value = (pageIndex - 1) * pageSize + 1 },
                new SqlParameter("@RowNumber_End", SqlDbType.Int){ Value = pageIndex * pageSize }
            };
            if (parms != null)
                paramlist.AddRange(parms);
            return ExecuteDataTable(connectionString, psql, paramlist.ToArray());
        }

        #endregion ExecuteDataTable

        #region ExecuteDataSet

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandText">SQL語句</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        public static DataSet ExecuteDataSet( string connectionString, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(connectionString, CommandType.Text, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        public static DataSet ExecuteDataSet( string connectionString, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return ExecuteDataSet(connection, commandType, commandText, parms);
            }
        }

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        public static DataSet ExecuteDataSet( SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(connection, null, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        public static DataSet ExecuteDataSet( SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            return ExecuteDataSet(transaction.Connection, transaction, commandType, commandText, parms);
        }

        /// <summary>
        /// 執行SQL語句,返回結果集
        /// </summary>
        /// <param name="connection">數據庫連接</param>
        /// <param name="transaction">事務</param>
        /// <param name="commandType">命令類型(存儲過程,命令文本, 其它.)</param>
        /// <param name="commandText">SQL語句或存儲過程名稱</param>
        /// <param name="parms">查詢參數</param>
        /// <returns>返回結果集</returns>
        private static DataSet ExecuteDataSet( SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parms )
        {
            SqlCommand command = new SqlCommand();

            PrepareCommand(command, connection, transaction, commandType, commandText, parms);
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet ds = new DataSet();
            adapter.Fill(ds);
            if (commandText.IndexOf("@") > 0)
            {
                commandText = commandText.ToLower();
                int index = commandText.IndexOf("where ");
                if (index < 0)
                {
                    index = commandText.IndexOf("\nwhere");
                }
                if (index > 0)
                {
                    ds.ExtendedProperties.Add("SQL", commandText.Substring(0, index - 1));  //將獲取的語句保存在表的一個附屬數組裡，方便更新時生成CommandBuilder
                }
                else
                {
                    ds.ExtendedProperties.Add("SQL", commandText);  //將獲取的語句保存在表的一個附屬數組裡，方便更新時生成CommandBuilder
                }
            }
            else
            {
                ds.ExtendedProperties.Add("SQL", commandText);  //將獲取的語句保存在表的一個附屬數組裡，方便更新時生成CommandBuilder
            }

            foreach (DataTable dt in ds.Tables)
            {
                dt.ExtendedProperties.Add("SQL", ds.ExtendedProperties["SQL"]);
            }

            command.Parameters.Clear();
            return ds;
        }

        #endregion ExecuteDataSet

        #region 批量操作

        /// <summary>
        /// 大批量數據插入
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="table">數據表</param>
        public static void BulkInsert( string connectionString, DataTable table )
        {
            if (string.IsNullOrEmpty(table.TableName))
                throw new Exception("DataTable.TableName屬性不能為空");
            using (SqlBulkCopy bulk = new SqlBulkCopy(connectionString))
            {
                bulk.BatchSize = BatchSize;
                bulk.BulkCopyTimeout = CommandTimeOut;
                bulk.DestinationTableName = table.TableName;
                foreach (DataColumn col in table.Columns)
                {
                    bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulk.WriteToServer(table);
                bulk.Close();
            }
        }

        /// <summary>
        /// 使用MySqlDataAdapter批量更新數據
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="table">數據表</param>
        public static void BatchUpdate( string connectionString, DataTable table )
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = CommandTimeOut;
            command.CommandType = CommandType.Text;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            SqlCommandBuilder commandBulider = new SqlCommandBuilder(adapter);
            commandBulider.ConflictOption = ConflictOption.OverwriteChanges;

            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                //設置批量更新的每次處理條數
                adapter.UpdateBatchSize = BatchSize;
                //設置事物
                adapter.SelectCommand.Transaction = transaction;

                if (table.ExtendedProperties["SQL"] != null)
                {
                    adapter.SelectCommand.CommandText = table.ExtendedProperties["SQL"].ToString();
                }
                adapter.Update(table);
                transaction.Commit();/////提交事務
            }
            catch (SqlException ex)
            {
                if (transaction != null)
                    transaction.Rollback();
                throw ex;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        /// <summary>
        /// 分批次批量刪除數據
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="sql">SQL語句</param>
        /// <param name="batchSize">每批次更新記錄行數</param>
        /// <param name="interval">批次執行間隔(秒)</param>
        public static void BatchDelete( string connectionString, string sql, int batchSize = 1000, int interval = 1 )
        {
            sql = sql.ToLower();

            if (batchSize < 1000)
                batchSize = 1000;
            if (interval < 1)
                interval = 1;
            while (ExecuteScalar(connectionString, sql.Replace("delete", "select top 1 1")) != null)
            {
                ExecuteNonQuery(connectionString, CommandType.Text, sql.Replace("delete", string.Format("delete top ({0})", batchSize)));
                System.Threading.Thread.Sleep(interval * 1000);
            }
        }

        /// <summary>
        /// 分批次批量更新數據
        /// </summary>
        /// <param name="connectionString">數據庫連接字符串</param>
        /// <param name="sql">SQL語句</param>
        /// <param name="batchSize">每批次更新記錄行數</param>
        /// <param name="interval">批次執行間隔(秒)</param>
        public static void BatchUpdate( string connectionString, string sql, int batchSize = 1000, int interval = 1 )
        {
            if (batchSize < 1000)
                batchSize = 1000;
            if (interval < 1)
                interval = 1;
            string existsSql = Regex.Replace(sql, @"[\w\sc.=,']*from", "select top 1 1 from", RegexOptions.IgnoreCase);
            existsSql = Regex.Replace(existsSql, @"set[\w\sc.=,']* where", "where", RegexOptions.IgnoreCase);
            existsSql = Regex.Replace(existsSql, @"update", "select top 1 1 from", RegexOptions.IgnoreCase);
            while (ExecuteScalar<int>(connectionString, existsSql) != 0)
            {
                ExecuteNonQuery(connectionString, CommandType.Text, Regex.Replace(sql, "update", string.Format("update top ({0})", batchSize), RegexOptions.IgnoreCase));
                System.Threading.Thread.Sleep(interval * 1000);
            }
        }

        #endregion 批量操作

        #endregion 靜態方法
    }
}