using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using Hangfire.Framework.Win;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class CCMToKSC.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal class CCMToKSCService
    {
        public string Connstirng { get; }

        public CCMToKSCService(string connstirng)
        {
            Connstirng = connstirng;
        }
        public string InsertTempToFormal { get; set; }
        public string InsertToTemp { get; set; }

        /// <summary>
        /// 繁體轉簡體
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>System.String.</returns>
        private static string ConvertToGb(string str)
        {
            return ChineseConverter.Convert(str,
                ChineseConversionDirection.TraditionalToSimplified);
        }

        public int Execute()
        {
            using (IDbConnection conn = new SqlConnection(Connstirng))
            {   
                   
                var sSQL = SQLSyntaxHelper.ReadSQLFile(InsertToTemp);
                var query = conn.Query<string>(sSQL).ToList();
                if (query.Count == 0)
                {
                    return -1;
                }
                // 2. 繁簡轉換
                var sb = new StringBuilder();
                query.ForEach(p => sb.Append(p));
                sSQL = ConvertToGb(sb.ToString());
                if(sSQL.Length !=0)
                  conn.Execute(sSQL);
                // 3. 新增料號到正式區
                sSQL = SQLSyntaxHelper.ReadSQLFile(InsertTempToFormal);
                return conn.Execute(sSQL);
            }
        }

       
    }
}