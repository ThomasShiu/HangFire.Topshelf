using Dapper;
using Hangfire.Framework.Win;
using Hangfire.Topshelf.Jobs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace Hangfire.Topshelf.Jobs
{
  /// <summary>
  /// 更新 Target 資料
  /// </summary>
  public class GetTargetService : ISyncMTCAction
  {
    //MTConnServices mtcService = new MTConnServices();

    public void SyncData(Callback context, string connectionstring)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();

      try
      {
        // 更新產能資料 Target
        GetTarget(connectionstring);

      }
      catch (Exception)
      {
        throw;
      }

      sw.Stop();
      context($"更新產能資料完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    public void UnSyncData(Callback context, string connectionstring)
    {
      // todo levi 未測試完成
      context($"取消更新產能資料");
      return;

    }

    #region 取得本月目標產量，實際產量 LAEP CFM
    public void GetTarget(string connectionstring)
    {
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        string[] vTarget = { "0", "0", "0", "0" };

        string cSQL = "SELECT LEAP, CFM, LEAP_COUNT, CFM_COUNT  " +
                      " FROM iPS_CHPC.dbo.TARGET ";

        var qry = Conn.Query<MES_Target_Query>(cSQL).AsList<MES_Target_Query>();
        if (qry.Count != 0)
        {
          try
          {
            foreach (var item in qry)
            {
              vTarget[0] = item.LEAP;
              vTarget[1] = item.CFM;
            }
          }
          catch (Exception)
          {
            //throw;
          }
        }

        // 大於20日,取本月,否則取上個月
        DateTime vNow = DateTime.Now;
        var vYear = vNow.Year.ToString();
        var vMonth = vNow.Month.ToString();
        var vDay = vNow.Day.ToString();
        string StartDate, EndDate;

        if (int.Parse(vDay) >= 20)
        {
          vNow = vNow.AddMonths(0);
          vYear = vNow.Year.ToString();
          vMonth = vNow.Month.ToString();
          if (int.Parse(vMonth) < 10)
          {
            vMonth = "0" + vMonth;
          }
          //StartDate = vYear + "-" + vMonth + "-" + "20";
        }
        else
        {
          vNow = vNow.AddMonths(-1);
          vYear = vNow.Year.ToString();
          vMonth = vNow.Month.ToString();
          if (int.Parse(vMonth) < 10)
          {
            vMonth = "0" + vMonth;
          }
          //StartDate = vYear + "-" + vMonth + "-" + "20"; 
        }
        StartDate = vYear + "-" + vMonth + "-" + "20";
        vNow = vNow.AddMonths(1);
        vYear = vNow.Year.ToString();
        vMonth = vNow.Month.ToString();
        EndDate = vYear + "-" + vMonth + "-" + "19";

        // LEAP 產量
        cSQL = " SELECT '0' LEAP,'0' CFM,CAST(COUNT(DISTINCT(MO_LIST)) AS VARCHAR) AS LEAP_CNT,'0' CFM_COUNT " +
                  " FROM iPS_CHPC.dbo.MM_WDATA A JOIN iPS_CHPC.dbo.MM_FACTORY_MACHINE B ON A.MACHINE_CODE = B.MACHINE_CODE " +
                  " WHERE B.MACHINE_TYPE = 'FINE' AND A.ENGINE LIKE '%LEAP%' " +
                  " AND(EDATE >= '" + StartDate + "' AND EDATE <= '" + EndDate + "')";
        qry = Conn.Query<MES_Target_Query>(cSQL).AsList<MES_Target_Query>();
        if (qry.Count != 0)
        {
          try
          {
            foreach (var item in qry)
            {
              vTarget[2] = item.LEAP_COUNT;
            }
          }
          catch (Exception)
          {
            //throw;
          }
        }

        // CFM 產量
        cSQL = " SELECT CAST(COUNT(DISTINCT(MO_LIST)) AS VARCHAR) AS CFM_CNT " +
                " FROM iPS_CHPC.dbo.MM_WDATA A JOIN iPS_CHPC.dbo.MM_FACTORY_MACHINE B ON A.MACHINE_CODE = B.MACHINE_CODE " +
                " WHERE B.MACHINE_TYPE = 'FINE' AND A.ENGINE LIKE '%CFM%' " +
                " AND(EDATE >= '" + StartDate + "' AND EDATE <= '" + EndDate + "')";
        qry = Conn.Query<MES_Target_Query>(cSQL).AsList<MES_Target_Query>();
        if (qry.Count != 0)
        {
          try
          {
            foreach (var item in qry)
            {
              vTarget[3] = item.CFM_COUNT;
            }
          }
          catch (Exception)
          {
            //throw;
          }
        }

        cSQL = string.Format(@"UPDATE MES.dbo.MES_Target SET LEAP = '{0}', CFM = '{1}', LEAP_COUNT = '{2}', CFM_COUNT = '{3}' ",
                 vTarget[0], vTarget[1], vTarget[2], vTarget[3]
             );
        Conn.Execute(cSQL);
      }
    }
    #endregion


  }
}