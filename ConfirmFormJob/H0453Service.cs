using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Hangfire.Framework.Win;

namespace Hangfire.Topshelf.Jobs
{
  /// <summary>
  /// 人事離職作業
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
        var cSQL = $"Select * From HR_CHGENR where FMSTS ='B' and EFFDT ='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGENR_Query>(cSQL).AsList<HR_CHGENR_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            foreach (var item in qry)
            {
              var sql = SQLSyntaxHelper.ReadSQLFile("HR_CHGENR_Cr.sql");
              sql = string.Format(sql, item.FMNO, item.NEMPLYID);
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
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        var cSQL = $"Select * From HR_CHGENR where FMSTS ='B' and EFFDT ='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGENR_Query>(cSQL).AsList<HR_CHGENR_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            foreach (var item in qry)
            {
              var sql = SQLSyntaxHelper.ReadSQLFile("HR_CHGENR_Cr.sql");
              sql = string.Format(sql, item.FMNO, item.NEMPLYID);
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