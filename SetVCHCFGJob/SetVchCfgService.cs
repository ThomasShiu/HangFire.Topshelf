using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Hangfire.Samples.Framework;
using Hangfire.Samples.Framework.Logging;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 自動化設定單據性質設定服務
    /// </summary>
    public class SetVchCfgService // : ISetVchCfgService //: BaseAppService
    {
        private readonly string _connectionstring;
        private readonly string _updateString = @"Update VCHCONFG set {0}='{1}' where VCH_TY ='{2}'";
        private readonly IList<string> _sqls = new List<string>();

        /// <summary>
        /// 記錄器
        /// </summary>
        /// <value>The logger.</value>
        public virtual ILog Logger { get; set; } = new NullLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="SetVchCfgService"/> class.
        /// </summary>
        /// <param name="connectionstring">The connectionstring.</param>
        public SetVchCfgService(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        /// <summary>
        /// 設定項目
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="vchty">The vchty.</param>
        public void SetItem(string field, string value,string vchty)
        {
            _sqls.Add(string.Format(_updateString, field, value, vchty));
        }

        /// <summary>
        /// 執行更新內容
        /// </summary>
        /// <returns>傳回執行筆數</returns>
        public int Execute()
        {
            var result = 0;
            using (IDbConnection conn = new SqlConnection(_connectionstring))
            {
               conn.Open();
               var tran = conn.BeginTransaction();
                try
                {
                    result += _sqls.Sum(sql => conn.Execute(sql, null, tran));
                    tran.Commit();
                }
                catch (Exception)
                {
                   tran.Rollback();
                    throw;
                }
                finally
                {
                    if(conn.State == ConnectionState.Connecting)
                    conn.Close();
                }
            }
            return result;
        }
    }

   
}