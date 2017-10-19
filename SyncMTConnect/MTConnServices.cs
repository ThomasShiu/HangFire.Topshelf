using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace Hangfire.Topshelf.Jobs
{
  public class MTConnServices
  {
    public static string v_MESContext = "MESContext";
    

    #region 把DataTable轉成JSON字串
    //把DataTable轉成JSON字串
    public string GetJson(string sql)
    {
      //得到一個DataTable物件
      DataTable dt = this.queryDataTable(sql);
      //將DataTable轉成JSON字串
      string str_json = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
      //string str_json = JsonConvert.SerializeObject(dt);
      return str_json;

    }
    #endregion

    #region 回傳DataTable物件
    /// <summary>
    /// 依據SQL語句，回傳DataTable物件
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public DataTable queryDataTable(string sql)
    {
      DataSet ds = new DataSet();
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[v_MESContext].ConnectionString))
      {
        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
        da.Fill(ds);
      }

      return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();

    }
    #endregion

    #region JSON字串轉成DataTable
    //把JSON字串轉成DataTable或Newtonsoft.Json.Linq.JArray
    public DataTable JSONstrToDataTable(string jsonStr)
    {

      //Newtonsoft.Json.Linq.JArray jArray = 
      //    JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JArray>(li_showData.Text.Trim());
      //或
      DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonStr);

      return dt;
      //GridView1顯示DataTable的資料
      //GridView1.DataSource = jArray; GridView1.DataBind();
      //GridView1.DataSource = dt; GridView1.DataBind();
    }
    #endregion


    #region 執行SQL語法
    public void ExecuteSQL(string SQL)
    {
      //1.引用SqlConnection物件連接資料庫
      using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[v_MESContext].ConnectionString))
      {
        //2.開啟資料庫
        conn.Open();
        //3.引用SqlCommand物件
        using (SqlCommand command = new SqlCommand(SQL, conn))
        {
          try
          {
            command.ExecuteNonQuery();
            //string msg = String.Format("ExecuteSQL() at {0:yyyy/MM/dd HH:mm:ss}", DateTime.Now);
            //Log(msg);

          }
          catch
          {
            //throw ex.GetBaseException();
          }
          finally
          {
            conn.Close();
          }
        }
      }
    }

    #endregion

    public void Log(string msg)
    {
      File.AppendAllText(@"C:\MES_log\tasklog.txt", msg + Environment.NewLine);
    }
  }
}
