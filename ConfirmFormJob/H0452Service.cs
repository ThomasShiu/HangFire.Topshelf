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
  public class H0452Service:IConfirmAction
  {
    public void Confirm(Callback context, string connectionstring, DateTime execDate)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        var cSQL = $"Select FMNO, EMPLYID, FLDT From HR_CHGTOR where FMSTS ='B' and FLDT <='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGTOR_Query>(cSQL).AsList<HR_CHGTOR_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGTOR_Cr.sql");
            foreach (var item in qry)
            {
              // C_STA(狀態):A:在職;B:留職停薪;C:留職不停薪;D:離職
              var sql = string.Format(csql, item.FLDT, item.FMNO, "D", item.EMPLYID);
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
      context($"確認離職單完成,花費時間為：{sw.ElapsedMilliseconds}");
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
        var cSQL = $"Select FMNO, EMPLYID, FLDT From HR_CHGTOR where FMSTS ='B' and FLDT ='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGTOR_Query>(cSQL).AsList<HR_CHGTOR_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGTOR_Dl.sql");
            foreach (var item in qry)
            {
              var sql = string.Format(csql, item.EMPLYID);
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