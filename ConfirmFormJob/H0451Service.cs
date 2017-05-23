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
  /// 人事任用作業
  /// </summary>
  public class H0451Service:IConfirmAction
  {
    public void Confirm(Callback context, string connectionstring, DateTime execDate)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        var cSQL = $"Select * From HR_CHGENR where FMSTS ='B' and EFFDT <='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_CHGENR_Query>(cSQL).AsList<HR_CHGENR_Query>();
        if (qry.Count != 0) {
        //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGENR_Cr.sql");            
            foreach (var item in qry)
            {              
            var sql = string.Format(csql, item.FMNO,item.NEMPLYID);
              Conn.Execute(sql);              
            }
          //    tran.Rollback();
          } catch (Exception )
          {
          //     tran.Rollback();            
            throw;
          }
        }
      }
      sw.Stop();
      context($"確認任用單完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    public void UnConfirm(Callback context, string connectionstring, DateTime execDate)
    {
      // todo levi 未測試完成
      context($"取消確認任用單未完成功能");
      return;
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
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGENR_Dl.sql");
            foreach (var item in qry)
            {              
              var sql = string.Format(csql, item.FMNO, item.NEMPLYID);
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
      context($"取消確認任用單完成,花費時間為：{sw.ElapsedMilliseconds}");
    }
  }
}