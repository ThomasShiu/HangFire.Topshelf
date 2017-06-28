using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Hangfire.Framework.Win;
using Hangfire.Topshelf.Jobs.Model;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 人事離職作業
    /// </summary>
    public class H0458Service : IConfirmAction
    {
        public void Confirm(Callback context, string connectionstring, DateTime execDate)
        {
            // 計時用
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            using (IDbConnection Conn = new SqlConnection(connectionstring))
            {

                #region 人員離職帳號失效作業
                var cSQL1 = $"Select FMNO, EMPLYID, FLDT From HR_CHGTOR where FMSTS ='A' and FLDT ='{execDate:yyyy/MM/dd}'";
                var qry1 = Conn.Query<HR_CHGTOR_Query>(cSQL1).AsList<HR_CHGTOR_Query>();
                if (qry1.Count != 0)
                {
                    //  var tran = Conn.BeginTransaction();
                    try
                    {
                        var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGMEMBER.sql");
                        foreach (var item in qry1)
                        {
                            var sql = string.Format(csql, item.FLDT, item.EMPLYID);
                            Conn.Execute(sql);
                        }
                        //    tran.Rollback();
                    }
                    catch (Exception)
                    {
                        //     tran.Rollback();
                        throw;
                    }
                }
                #endregion
            }
            sw.Stop();
            context($"人員帳號停用完成,花費時間為：{sw.ElapsedMilliseconds}");
        }

        /// <summary>
        /// 取消確認
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="execDate"></param>
        public void UnConfirm(Callback context, string connectionstring, DateTime execDate)
        {
            context($"取消確認未完成功能");
        }
    }
}