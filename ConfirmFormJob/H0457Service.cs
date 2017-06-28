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
  /// 薪資變更作業
  /// </summary>
  public class H0457Service:IConfirmAction
  {
        /// <summary>
        /// 薪資變更單確認
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="execDate"></param>
        public void Confirm(Callback context, string connectionstring, DateTime execDate)
        {
            // 計時用
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            using (IDbConnection Conn = new SqlConnection(connectionstring))
            {

                #region HR_PAYPMS 員工津貼扣款設定

                var cSQL1 = $"Select M.EMPLYID, FITEM, PMS_NO, AMT, D.C_OVT, C_FRL, C_TAX ,CHTY From HR_PAYCHM M ,HR_PAYCHD D WHERE M.FMNO = D.FMNO AND M.C_STA ='CF' and M.CRDT <='{execDate:yyyy/MM/dd}'";
                var qry2 = Conn.Query<HR_PAYCHD_Query>(cSQL1).AsList<HR_PAYCHD_Query>();
                if (qry2.Count != 0)
                {
                    //  var tran = Conn.BeginTransaction();
                    try
                    {
                        var csql = SQLSyntaxHelper.ReadSQLFile("HR_PAYPMS_Cr.sql");
                        foreach (var item in qry2)
                        {
                            var sql = string.Format(csql, item.CHTY
                                                         , item.EMPLYID
                                                         , item.FITEM
                                                         , item.PMS_NO
                                                         , item.AMT
                                                         , item.C_OVT
                                                         , item.C_FRL
                                                         , item.C_TAX);
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

                #endregion HR_PAYPMS 員工津貼扣款設定

                #region HR_PAYSET 員工薪資設定檔
                var cSQL = $"Select EMPLYID, BAS_SLY, ALP_RWD, C_OVT, C_LPS_TY, IID_NO, PAYTP, PAYROLLTP, OT_TY, FMNO From HR_PAYCHM where C_STA ='CF' and CRDT <='{execDate:yyyy/MM/dd}'";
                var qry = Conn.Query<HR_PAYCHM_Query>(cSQL).AsList<HR_PAYCHM_Query>();
                if (qry.Count != 0)
                {
                    //  var tran = Conn.BeginTransaction();
                    try
                    {
                        var csql = SQLSyntaxHelper.ReadSQLFile("HR_PAYCHM_Cr.sql");
                        foreach (var item in qry)
                        {
                            var sql = string.Format(csql, item.EMPLYID
                                                         , item.BAS_SLY
                                                         , item.ALP_RWD
                                                         , item.C_OVT
                                                         , item.C_LPS_TY
                                                         , item.IID_NO
                                                         , item.PAYTP
                                                         , item.PAYROLLTP
                                                         , item.OT_TY
                                                         , item.FMNO);
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

                #endregion HR_PAYSET 員工薪資設定檔
            }
            sw.Stop();
            context($"確認薪資變更單完成,花費時間為：{sw.ElapsedMilliseconds}");
        }

        /// <summary>
        /// 取消確認薪資變更單
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="execDate"></param>
        public void UnConfirm(Callback context, string connectionstring, DateTime execDate)
        {
            context($"取消確認薪資變更單未完成功能");
        }
  }
}