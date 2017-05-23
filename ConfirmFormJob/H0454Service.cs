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
  /// 健保變更作業 
  /// </summary>
  public class H0454Service:IConfirmAction
  {
    public void Confirm(Callback context, string connectionstring, DateTime execDate)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        #region 健保變更檔(HR_HISCHM)
        var cSQL = $"Select FMNO,EMPLYID,DDT From HR_HISCHM  where FMSTS ='CF' and DDT <='{execDate:yyyy/MM/dd}'";
        var qry = Conn.Query<HR_HISCHM_Query>(cSQL).AsList<HR_HISCHM_Query>();
        if (qry.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_HISCHM_Cr.sql");
            foreach (var item in qry)
            {             
             var sql = string.Format(csql 
                                         ,item.EMPLYID
                                         ,item.FMNO
                                         ,item.DDT.Value);
              Conn.Execute(sql);
            }
            //    tran.Rollback();
          } catch (Exception)
          {
            //     tran.Rollback();            
            throw;
          }
        }
        #endregion
        #region 眷屬健保變更明細檔(HR_HISCHD)
        var cSQL2 = @"SELECT M.EMPLYID,D.IDNO,
                            (Select CHGDT from HR_FAHIS where EMPLYID =M.EMPLYID AND IDNO =D.IDNO) AS CHGDT,
                             M.DDT , M.FMNO
                       FROM HR_HISCHM M ,HR_HISCHD D WHERE M.FMNO = D.FMNO AND M.FMSTS ='CF' ";
        cSQL2 += $" and M.DDT = '{execDate:yyyy/MM/dd}'";
        var qry2 = Conn.Query<HR_HISCHD_Query>(cSQL2).AsList<HR_HISCHD_Query>();
        if (qry2.Count != 0)
        {
          //  var tran = Conn.BeginTransaction();
          try
          {
            var csql = SQLSyntaxHelper.ReadSQLFile("HR_HISCHD_Cr.sql");
            foreach (var item in qry2)
            {
              var sql = string.Format(csql, item.EMPLYID
                                          , item.IDNO
                                          , item.CHGDT.Value
                                          , item.DDT.Value
                                          , item.FMNO);
              Conn.Execute(sql);
            }
            //    tran.Rollback();
          } catch (Exception)
          {
            //     tran.Rollback();            
            throw;
          }
        }
        #endregion
      }
      sw.Stop();
      context($"確認健保變更單完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    /// <summary>
    /// 取消確認離職單
    /// </summary>
    /// <param name="context"></param>
    /// <param name="connectionstring"></param>
    /// <param name="execDate"></param>
    public void UnConfirm(Callback context, string connectionstring, DateTime execDate)
    {
      context($"取消確認健保變更單未完成功能");
    }
  }
}