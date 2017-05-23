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
  /// 勞保變更作業
  /// </summary>
  public class H0455Service:IConfirmAction
  {
    public void Confirm(Callback context, string connectionstring, DateTime execDate)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        
        var cSQL = $"SELECT FMNO, EMPLYID, DDT  FROM HR_LISCHM WHERE FMSTS = 'CF' AND DDT ='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_LISCHM_Query>(cSQL).AsList<HR_LISCHM_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_LISCHM_Cr.sql");
            foreach (var item in qry)
            {
             //  勞保變更檔(HR_LISCHM)
             
             var sql = string.Format(csql
                                     , item.EMPLYID
                                     , item.FMNO
                                     , item.DDT.Value.ToString("yyyy/MM/dd"));
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
      context($"確認勞保變更單完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    /// <summary>
    /// 取消確認勞退變更單
    /// </summary>
    /// <param name="context"></param>
    /// <param name="connectionstring"></param>
    /// <param name="execDate"></param>
    public void UnConfirm(Callback context, string connectionstring, DateTime execDate)
    {
      context($"取消確認勞保變更單未完成功能");
    }
  }
}