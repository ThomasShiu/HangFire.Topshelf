using Dapper;
using Hangfire.Framework.Win;
using Hangfire.Topshelf.Jobs.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;

namespace Hangfire.Topshelf.Jobs
{
  /// <summary>
  /// 取得MTConnect LOG 資料
  /// 會依據最新的Sequence 去擷取最新區段的LOG
  /// </summary>
  public class GetLogService: ISyncMTCAction
  {   
    string v_sql, v_component, v_itemname;
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
        if (qry.Count != 0) {
          try
          {
            //var csql = SQLSyntaxHelper.ReadSQLFile("HR_CHGENR_Cr.sql");            
            foreach (var item in qry)
            {
              v_component = "Log";
              v_itemname = "log";
              // 依據序號讀取LOG
              CatchMTConnectLogHead(item.MachineID, item.IPaddress, v_component, v_itemname, connectionstring);
              // 直接讀取LOG
              CatchMTConnectLogXML(item.MachineID, item.IPaddress, v_component, v_itemname, connectionstring);
            
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
      context($"同步MTConnect Log Data完成,花費時間為：{sw.ElapsedMilliseconds}");
    }

    public void UnSyncData(Callback context, string connectionstring)
    {
      // todo levi 未測試完成
      context($"取消同步MTConnect Log Data");
      return;

    }


    #region MTConnect XML Catch
    // 取得最新SEQUENCE
    public void CatchMTConnectLogHead(string v_machine, string v_url, string v_component, string v_itemname,string connectionstring)
    {

        try
        {
          // http://192.168.0.183:5000/sample?path=//Log//DataItem[@name="log"]
          string url = "http://" + v_url + ":5000/sample?path=//" + v_component + "//DataItem[@name='" + v_itemname + "']";

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
          // HEAD : creationTime,sender,instanceId,version,bufferSize,nextSequence,firstSequence , lastSequence
          XmlElement DeviceStreams = (XmlElement)xmlDoc.SelectSingleNode("descendant::mtS:Header ", theNameManager);

          // 最新的序號
          var LastSeq = int.Parse(DeviceStreams.Attributes["lastSequence"].Value);
          if (LastSeq > 10000)
          {
            LastSeq -= 10000;
          }
          else if (LastSeq > 1000)
          {
            LastSeq -= 1000;
          }
          else if (LastSeq > 100)
          {
            LastSeq -= 100;
          }

          CatchMTConnectLogXML(v_machine, v_url, v_component, v_itemname, LastSeq, connectionstring);

        }
        catch
        {
        }
      
    }


    // MTConnect Log Data
    public void CatchMTConnectLogXML(string v_machine, string v_url, string v_component, string v_itemname, int v_LastSeq,string connectionstring)
    {
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        try
        {
          // http://192.168.0.183:5000/sample?path=//Log//DataItem[@name="log"]
          string url = "http://" + v_url + ":5000/sample?path=//" + v_component + "//DataItem[@name='" + v_itemname + "']&from=" + v_LastSeq + "&count=1000";
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

          XmlElement DeviceStreams = (XmlElement)xmlDoc.SelectSingleNode("descendant::mtS:DeviceStream", theNameManager);
          XmlNodeList theStreams = DeviceStreams.SelectNodes("descendant::mtS:ComponentStream", theNameManager);

          foreach (XmlNode CompStream in theStreams)
          {
            if (CompStream.Attributes["component"].Value == v_component)
            {

              XmlElement EventElement = (XmlElement)CompStream.SelectSingleNode("descendant::mtS:Events", theNameManager);

              foreach (XmlNode EventStream in EventElement.ChildNodes)
              {
                var timestamp = EventStream.Attributes["timestamp"].Value;
                var timestamp2 = DateTime.Parse(timestamp, null, DateTimeStyles.RoundtripKind);
                timestamp2 = timestamp2.AddHours(8); //UTC 轉 GMT+8
                                                     //LogEntity.Timestamp = timestamp2;

                if (!EventStream.InnerText.Contains("UNAVAILABLE"))
                {
                  string[] vValues = EventStream.InnerText.Split(',');

                  v_sql = string.Format(@"INSERT INTO MES_MTConnectLog(MachineID,DeviceName,Deviceuuid, ComponentType, ComponentName, ComponentId, DataItemId, DataItemName, Sequence, Value_1,Value_2,Value_3,Value_4, Timestamp,CreatorTime) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}','{12}','{13}','{14}')",
                   v_machine,
                   DeviceStreams.Attributes["name"].Value,
                   DeviceStreams.Attributes["uuid"].Value,
                   CompStream.Attributes["component"].Value,
                   CompStream.Attributes["name"].Value,
                   CompStream.Attributes["componentId"].Value,
                   EventStream.Attributes["dataItemId"].Value,
                   EventStream.Attributes["name"].Value,
                   EventStream.Attributes["sequence"].Value,
                   vValues[0], vValues[1], vValues[2], vValues[3],
                   timestamp2.ToString("yyyy/MM/dd HH:mm:ss.mmm", CultureInfo.InvariantCulture),
                   vNow
                   );

                  Conn.Execute(v_sql);
                  //mtcService.ExecuteSQL(v_sql);
                }
                else
                {
                  v_sql = string.Format(@"INSERT INTO MES_MTConnectLog(MachineID,DeviceName,Deviceuuid, ComponentType, ComponentName, ComponentId, DataItemId, DataItemName, Sequence, Value_1, Timestamp,CreatorTime) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}')",
                   v_machine,
                   DeviceStreams.Attributes["name"].Value,
                   DeviceStreams.Attributes["uuid"].Value,
                   CompStream.Attributes["component"].Value,
                   CompStream.Attributes["name"].Value,
                   CompStream.Attributes["componentId"].Value,
                   EventStream.Attributes["dataItemId"].Value,
                   EventStream.Attributes["name"].Value,
                   EventStream.Attributes["sequence"].Value,
                   EventStream.InnerText,
                   timestamp2.ToString("yyyy/MM/dd HH:mm:ss.mmm", CultureInfo.InvariantCulture),
                   vNow
                   );
                  Conn.Execute(v_sql);
                  //mtcService.ExecuteSQL(v_sql);
                }

              }
            }
          }

        }
        catch
        {
        }
      }
    }

    public void CatchMTConnectLogXML(string v_machine, string v_url, string v_component, string v_itemname, string connectionstring)
    {
      using (IDbConnection Conn = new SqlConnection(connectionstring))
      {
        try
        {
          // http://192.168.0.183:5000/sample?path=//Log//DataItem[@name="log"]
          string url = "http://" + v_url + ":5000/sample?path=//" + v_component + "//DataItem[@name='" + v_itemname + "']";
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

          XmlElement DeviceStreams = (XmlElement)xmlDoc.SelectSingleNode("descendant::mtS:DeviceStream", theNameManager);
          XmlNodeList theStreams = DeviceStreams.SelectNodes("descendant::mtS:ComponentStream", theNameManager);

          foreach (XmlNode CompStream in theStreams)
          {
            if (CompStream.Attributes["component"].Value == v_component)
            {

              XmlElement EventElement = (XmlElement)CompStream.SelectSingleNode("descendant::mtS:Events", theNameManager);

              foreach (XmlNode EventStream in EventElement.ChildNodes)
              {
                var timestamp = EventStream.Attributes["timestamp"].Value;
                var timestamp2 = DateTime.Parse(timestamp, null, DateTimeStyles.RoundtripKind);
                timestamp2 = timestamp2.AddHours(8); //UTC 轉 GMT+8
                                                     //LogEntity.Timestamp = timestamp2;

                if (!EventStream.InnerText.Contains("UNAVAILABLE"))
                {
                  string[] vValues = EventStream.InnerText.Split(',');

                  v_sql = string.Format(@"INSERT INTO MES_MTConnectLog(MachineID,DeviceName,Deviceuuid, ComponentType, ComponentName, ComponentId, DataItemId, DataItemName, Sequence, Value_1,Value_2,Value_3,Value_4, Timestamp,CreatorTime) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}','{12}','{13}','{14}')",
                   v_machine,
                   DeviceStreams.Attributes["name"].Value,
                   DeviceStreams.Attributes["uuid"].Value,
                   CompStream.Attributes["component"].Value,
                   CompStream.Attributes["name"].Value,
                   CompStream.Attributes["componentId"].Value,
                   EventStream.Attributes["dataItemId"].Value,
                   EventStream.Attributes["name"].Value,
                   EventStream.Attributes["sequence"].Value,
                   vValues[0], vValues[1], vValues[2], vValues[3],
                   timestamp2.ToString("yyyy/MM/dd HH:mm:ss.mmm", CultureInfo.InvariantCulture),
                   vNow
                   );
                  Conn.Execute(v_sql);
                  //mtcService.ExecuteSQL(v_sql);
                }
                else
                {

                  v_sql = string.Format(@"INSERT INTO MES_MTConnectLog(MachineID,DeviceName,Deviceuuid, ComponentType, ComponentName, ComponentId, DataItemId, DataItemName, Sequence, Value_1, Timestamp,CreatorTime) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}')",
                   v_machine,
                   DeviceStreams.Attributes["name"].Value,
                   DeviceStreams.Attributes["uuid"].Value,
                   CompStream.Attributes["component"].Value,
                   CompStream.Attributes["name"].Value,
                   CompStream.Attributes["componentId"].Value,
                   EventStream.Attributes["dataItemId"].Value,
                   EventStream.Attributes["name"].Value,
                   EventStream.Attributes["sequence"].Value,
                   EventStream.InnerText,
                   timestamp2.ToString("yyyy/MM/dd HH:mm:ss.mmm", CultureInfo.InvariantCulture),
                   vNow
                   );
                  Conn.Execute(v_sql);
                  //mtcService.ExecuteSQL(v_sql);
                }

              }
            }
          }

        }
        catch
        {
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