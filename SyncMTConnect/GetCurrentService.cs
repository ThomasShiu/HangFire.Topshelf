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
  /// 取得MTConnect LOG 資料
  /// 會依據最新的Sequence 去擷取最新區段的LOG
  /// </summary>
  public class GetCurrentService : ISyncMTCAction
  {
    string v_sql;
    MTConnServices mtcService = new MTConnServices();

    public void SyncData(Callback context, string connectionstring)
    {
      // 計時用
      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        // 取得機台資料
        var cSQL = $"SELECT MachineID, MachineName, IPaddress FROM MES.dbo.MES_Machines ORDER BY SortCode DESC";
        var qry = Conn.Query<MES_Machines_Query>(cSQL).AsList<MES_Machines_Query>();
        if (qry.Count != 0)
        {
          try
          {
            //var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGENR_Cr.sql");            
            foreach (var item in qry)
            {
              // 更新機台狀態看板
              CatchMTConnectCurrentXML(item.MachineID, item.IPaddress, connectionstring);
             

            }
            //    tran.Rollback();
          }
          catch (Exception)
          {
            //     tran.Rollback();            
            throw;
          }
        }
      }
      sw.Stop();
      context($"同步MTConnect Current Data完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    public void UnSyncData(Callback context, string connectionstring)
    {
      // todo levi 未測試完成
      context($"取消同步MTConnect Current Data");
      return;

    }


    #region MTConnect Current 機台現況
    public void CatchMTConnectCurrentXML(string v_machine, string v_url, string connectionstring)
    {
      string v_avg = "0", i_avg = "0", ttl_power = "0",
          resistance1 = "0", electronic1 = "0", room1 = "0", liquid1 = "0",
          resistance2 = "0", electronic2 = "0", room2 = "0", liquid2 = "0";
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        try
        {
          // http://192.168.0.183:5000/sample?path=//Log//DataItem[@name=%27log%27]

          string url = "http://" + v_url + ":5000/current";

          XmlDocument xmlDoc = new XmlDocument();
          //xmlDoc = null;
          xmlDoc = GetDataFromUrl(url);

          // 擷取不到XML 資料就歸零並跳出
          if (xmlDoc == null)
          {
            v_sql = string.Format(@"UPDATE MES_MachinesStat SET v_avg = '{1}', i_avg = '{2}', ttl_power = '{3}', resistance1 = '{4}', electronic1 = '{5}', room1 = '{6}', liquid1 = '{7}', resistance2 = '{8}', electronic2 = '{9}', room2 = '{10}', liquid2 = '{11}',ConnStat='N',LastModifyTime=getdate(),LastModifyUserId='WEB_MES' WHERE MachineID = '{0}'",
               v_machine,
               v_avg,
               i_avg,
               ttl_power,
               resistance1, electronic1, room1, liquid1,
               resistance2, electronic2, room2, liquid2
           );
            Conn.Execute(v_sql);
            //mtcService.ExecuteSQL(v_sql);
            return;
          }

          XmlNamespaceManager theNameManager = new XmlNamespaceManager(xmlDoc.NameTable);
          theNameManager.AddNamespace("mtS", "urn:mtconnect.org:MTConnectStreams:1.3");
          theNameManager.AddNamespace("m", "urn:mtconnect.org:MTConnectStreams:1.3");
          theNameManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

          XmlNodeList EmeterStreams = xmlDoc.SelectNodes("descendant::mtS:Emeter", theNameManager);
          var EmeterNode = from n in EmeterStreams.Cast<XmlNode>()
                           select new
                           {
                             name = n.Attributes["name"].Value,
                             value = n.InnerText.Replace("KW", "").Replace("MA", "").Replace("A", "").Replace("V", "")
                           };

          // 電表
          foreach (var col in EmeterNode)
          {
            if (col.name == "V-Avg") { v_avg = col.value; }
            else if (col.name == "I-Avg") { i_avg = col.value; }
            else if (col.name == "Total-Power") { ttl_power = col.value; }

          }

          XmlNodeList TsensorStreams = xmlDoc.SelectNodes("descendant::mtS:Tsensor", theNameManager);
          var TsensorNode = from n in TsensorStreams.Cast<XmlNode>()
                            select new { name = n.Attributes["name"].Value, value = n.InnerText.Replace("C", "") };
          foreach (var col in TsensorNode)
          {
            if (col.name == "resistance.1") { resistance1 = col.value; }
            else if (col.name == "electronic.1") { electronic1 = col.value; }
            else if (col.name == "room.1") { room1 = col.value; }
            else if (col.name == "liquid.1") { liquid1 = col.value; }
            else if (col.name == "resistance.2") { resistance2 = col.value; }
            else if (col.name == "electronic.2") { electronic2 = col.value; }
            else if (col.name == "room.2") { room2 = col.value; }
            else if (col.name == "liquid.2") { liquid2 = col.value; }
          }

          int totalCount = EmeterNode.Count() + TsensorNode.Count();


          //擷取到資料並更新
          v_sql = string.Format(@"UPDATE MES_MachinesStat SET v_avg = '{1}', i_avg = '{2}', ttl_power = '{3}', resistance1 = '{4}', electronic1 = '{5}', room1 = '{6}', liquid1 = '{7}', resistance2 = '{8}', electronic2 = '{9}', room2 = '{10}', liquid2 = '{11}',ConnStat='Y',LastModifyTime=getdate(),LastModifyUserId='WEB_MES' WHERE MachineID = '{0}'",
              v_machine,
              v_avg,
              i_avg,
              ttl_power,
              resistance1, electronic1, room1, liquid1,
              resistance2, electronic2, room2, liquid2
          );
          Conn.Execute(v_sql);
          //mtcService.ExecuteSQL(v_sql);

          // 擷取不到資料就變更狀態為異常 E
          if (v_avg == "ERROR: TIMED OUT" | v_avg == "UNAVAILABLE")
          {
            v_sql = string.Format(@"UPDATE MES_MachinesStat_test SET ConnStat='E',LastModifyTime=getdate(),LastModifyUserId='WEB_MES' WHERE MachineID = '{0}'",
            v_machine);
            mtcService.ExecuteSQL(v_sql);
          }
        }
        catch
        {
          v_sql = string.Format(@"UPDATE MES_MachinesStat SET v_avg = '{1}', i_avg = '{2}', ttl_power = '{3}', resistance1 = '{4}', electronic1 = '{5}', room1 = '{6}', liquid1 = '{7}', resistance2 = '{8}', electronic2 = '{9}', room2 = '{10}', liquid2 = '{11}',ConnStat='N',LastModifyTime=getdate(),LastModifyUserId='WEB_MES' WHERE MachineID = '{0}'",
                     v_machine,
                     v_avg,
                     i_avg,
                     ttl_power,
                     resistance1, electronic1, room1, liquid1,
                     resistance2, electronic2, room2, liquid2
                 );
          Conn.Execute(v_sql);
          //mtcService.ExecuteSQL(v_sql);
        }
      }
    }
    #endregion

    #region 探測 MTConnect XML 是否有資料
    public XmlDocument GetDataFromUrl(string url)
    {
      XmlDocument urlData = new XmlDocument();
      HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);

      rq.Timeout = 2000;

      HttpWebResponse response = rq.GetResponse() as HttpWebResponse;

      using (Stream responseStream = response.GetResponseStream())
      {
        XmlTextReader reader = new XmlTextReader(responseStream);
        urlData.Load(reader);
      }
      return urlData;
    }
    #endregion


  }
}