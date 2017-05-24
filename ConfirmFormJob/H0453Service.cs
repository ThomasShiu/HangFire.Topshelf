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
  /// 人事異動作業
  /// </summary>
  public class H0453Service:IConfirmAction
  {
    public void Confirm(Callback context, string connectionstring, DateTime execDate)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        var cSQL = $"Select WBSID , UCOMID , UFA_NO , UDEPID , UJOBID , URANKID , EMPLYID ,FMNO From HR_CHGJOB where FMSTS ='B' and EFFDT <='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGJOB_Query>(cSQL).AsList<HR_CHGJOB_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          { var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGJOB_Cr.sql");
            foreach (var item in qry)
            {             
              var sql = string.Format(csql 
                                          ,item.WBSID
                                          ,item.UCOMID
                                          ,item.UFA_NO
                                          ,item.UDEPID
                                          ,item.UJOBID
                                          ,item.URANKID
                                          ,item.EMPLYID
                                          ,item.FMNO);                      
              Conn.Execute(sql);
            }
            //    tran.Rollback();
          } catch (Exception)
          {
            //     tran.Rollback();            
            throw;
          }
        }
      }
      sw.Stop();
      context($"確認人事異動單完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    /// <summary>
    /// 取消確認離職單
    /// </summary>
    /// <param name="context"></param>
    /// <param name="connectionstring"></param>
    /// <param name="execDate"></param>
    public void UnConfirm(Callback context, string connectionstring, DateTime execDate)
    {
      // todo levi 未測試完成
      context($"取消確認離職單未完成功能");
      return;
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        var cSQL = $"Select WBSID ,PCOMID, PFA_NO, PDEPID, PJOBID, PRANKID , EMPLYID ,FMNO From HR_CHGJOB where FMSTS ='B' and EFFDT ='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGJOB_Query_UnConfirm>(cSQL).AsList<HR_CHGJOB_Query_UnConfirm>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGJOB_Dl.sql");
            foreach (var item in qry)
            {              
              var sql = string.Format(csql, item.WBSID,item.PCOMID,item.PFA_NO,item.PDEPID,item.PJOBID,item.PRANKID,item.EMPLYID,item.FMNO);
              Conn.Execute(sql);
            }
            //    tran.Rollback();
          } catch (Exception)
          {
            //     tran.Rollback();            
            throw;
          }
        }
      }
      sw.Stop();
      context($"取消確認離職單完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

  }
}