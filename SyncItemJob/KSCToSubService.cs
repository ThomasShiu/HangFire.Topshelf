using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Hangfire.Framework.Win;

namespace Hangfire.Topshelf.Jobs
{
    internal class KSCToSubService
    {
        public string Connstirng { get; }

        public KSCToSubService(string connstirng)
        {
            Connstirng = connstirng;
        }

        public string ScriptFile { get; set; }  
        
        public int Execute()
        {
            using (IDbConnection conn = new SqlConnection(Connstirng))
            {
                var sSQL = SQLSyntaxHelper.ReadSQLFile(ScriptFile);
                return conn.Execute(sSQL);
            }
        }
    }
}