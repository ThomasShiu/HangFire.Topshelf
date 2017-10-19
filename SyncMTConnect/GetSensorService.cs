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
  public class GetSensorService : ISyncMTCAction
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
              // 讀取Sensor資料
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
      context($"同步MTConnect Sensor Data完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    public void UnSyncData(Callback context, string connectionstring)
    {
      // todo levi 未測試完成
      context($"取消同步MTConnect Sensor Data");
      return;

    }


    // MTConnect Sensors Data
    #region MTConnect Current 機台現況
    public void CatchMTConnectCurrentXML(string v_machine, string v_url,string connectionstring)
    {
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        try
        {
          // http://192.168.0.183:5000/current]

          string url = "http://" + v_url + ":5000/current";
          String vNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

          XmlDocument xmlDoc = new XmlDocument();
          xmlDoc = GetDataFromUrl(url);

          // 擷取不到XML 資料就跳出
          if (xmlDoc.IsEmpty())
          {
            return;
          }

          XmlNamespaceManager theNameManager = new XmlNamespaceManager(xmlDoc.NameTable);
          theNameManager.AddNamespace("mtS", "urn:mtconnect.org:MTConnectStreams:1.3");
          theNameManager.AddNamespace("m", "urn:mtconnect.org:MTConnectStreams:1.3");
          theNameManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

          // 電表部分
          XmlNodeList EmeterStreams = xmlDoc.SelectNodes("descendant::mtS:Emeter", theNameManager);
          var EmeterNode = from n in EmeterStreams.Cast<XmlNode>()
                           select new
                           {
                             componentType = "Sensors",
                             componentName = "Electricity-Meter",
                             componentId = "e1",
                             dataItemId = n.Attributes["dataItemId"].Value,
                             timestamp = n.Attributes["timestamp"].Value,
                             sequence = n.Attributes["sequence"].Value,
                             name = n.Attributes["name"].Value,
                             value = n.InnerText.Replace("KW", "").Replace("MA", "").Replace("A", "").Replace("V", "")
                           };

          //JArray MixArray = new JArray();

          // 電表
          foreach (var col in EmeterNode)
          {
            var timestamp = col.timestamp;
            var timestamp2 = DateTime.Parse(timestamp, null, DateTimeStyles.RoundtripKind);
            timestamp2 = timestamp2.AddHours(8); //UTC 轉 GMT+8
                                                 //SensorEntity.Timestamp = timestamp2;

            v_sql = string.Format(@"INSERT INTO MES_MTConnectSensor(MachineID,DeviceName,Deviceuuid, ComponentType, ComponentName, ComponentId, DataItemId, DataItemName, Sequence, Value_1, Timestamp,CreatorTime) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}')",
                v_machine,
                "Botnana-Control",
                "device-2",
                col.componentType,
                col.componentName,
                col.componentId,
                col.dataItemId,
                col.name,
                col.sequence,
                col.value,
                timestamp2.ToString("yyyy/MM/dd HH:mm:ss.mmm", CultureInfo.InvariantCulture),
                vNow
                );
            Conn.Execute(v_sql);
            //mtcService.ExecuteSQL(v_sql);

          }

          // 溫度部分
          XmlNodeList TsensorStreams = xmlDoc.SelectNodes("descendant::mtS:Tsensor", theNameManager);
          var TsensorNode = from n in TsensorStreams.Cast<XmlNode>()
                            select new
                            {
                              componentType = "Sensors",
                              componentName = "Temperature-Sensor",
                              componentId = "t1",
                              dataItemId = n.Attributes["dataItemId"].Value,
                              timestamp = n.Attributes["timestamp"].Value,
                              sequence = n.Attributes["sequence"].Value,
                              name = n.Attributes["name"].Value,
                              value = n.InnerText.Replace("C", "")
                            };
          foreach (var col in TsensorNode)
          {
            var timestamp = col.timestamp;
            var timestamp2 = DateTime.Parse(timestamp, null, DateTimeStyles.RoundtripKind);
            timestamp2 = timestamp2.AddHours(8); //UTC 轉 GMT+8
                                                 //SensorEntity.Timestamp = timestamp2;
            v_sql = string.Format(@"INSERT INTO MES_MTConnectSensor(MachineID,DeviceName,Deviceuuid, ComponentType, ComponentName, ComponentId, DataItemId, DataItemName, Sequence, Value_1, Timestamp,CreatorTime) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}')",
                 v_machine,
                 "Botnana-Control",
                 "device-2",
                 col.componentType,
                 col.componentName,
                 col.componentId,
                 col.dataItemId,
                 col.name,
                 col.sequence,
                 col.value,
                 timestamp2.ToString("yyyy/MM/dd HH:mm:ss.mmm", CultureInfo.InvariantCulture),
                 vNow
                 );
            Conn.Execute(v_sql);
            //mtcService.ExecuteSQL(v_sql);
          }

        }
        catch { }

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