using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Hangfire.Topshelf.Jobs
{
    using Samples.Framework;


    public class SetVchCfgService : BaseAppService
    {
        private readonly string _connectionstring;
        private readonly string _updateString = "Update VCHCONFG set {0}='{1}' where VCH_TY ='{2}'";
        private readonly IList<string> _sqls = new List<string>();

        public SetVchCfgService(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public void SetItem(string field, string value,string vchty)
        {
            _sqls.Add(string.Format(_updateString, field, value, vchty));
        }

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