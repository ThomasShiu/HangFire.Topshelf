using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Hangfire.Framework.Win;

namespace Hangfire.Topshelf.Jobs.Tests
{
    internal class KSCToSubsidiaries
    {
        private string _connstirng;
     
        private IDbConnection Connection;
     
        public KSCToSubsidiaries(string connstirng)
        {
            this._connstirng = connstirng;
            Connection = new SqlConnection(this._connstirng);
            Connection.Open();
        }
        ~KSCToSubsidiaries()
        {
            Connection.Close();
            Connection.Dispose();
        }
        /// <summary>
        /// 來源資料表
        /// </summary>
        /// <value>The sourec table.</value>
        public string SourecTable { get; set; }
        /// <summary>
        /// 目的的資料表
        /// </summary>
        /// <value>The destination table.</value>
        public string DestinationTable { get; set; }

        public string SyncFile { get; set; }
        public string UpDateItemFile { get; set; }
        public int Execute()
        {
            return 0;
        }

        public int Synchronization()
        {
            /* 1. 將來KSC 不在目標資料庫的同步
             * 2. 更新特定欄位 像是倉庫位置 或是特定欄位給等定值
             */
            var cSQL = SQLSyntaxHelper.ReadSQLFile(SyncFile);
            var sSQL = string.Format(cSQL, SourecTable);
            return Connection.Execute(sSQL); 
        }

        public int UpdateItem()
        {
            // 找出有有更新部份
            // 更新有更新的部份
            //   先刪除舊的在新增
            var cSQL = SQLSyntaxHelper.ReadSQLFile(SyncFile);
            var sSQL = string.Format(cSQL, SourecTable);
            return Connection.Execute(sSQL);
        }
    }
}