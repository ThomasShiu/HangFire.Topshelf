using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Dapper;
using Hangfire.Samples.Framework;
using Hangfire.Server;
using Hangfire.Topshelf.Jobs.Model;
using Hangfire.Console;
using Hangfire.Topshelf.Jobs.Code;

namespace Hangfire.Topshelf.Jobs
{ 

    /// <summary>
    ///  執行SQL作業
    /// </summary>
    public class SQLRuleRun
    {
        public delegate void Callback(string line);
        /// <summary>
        /// 連線字串
        /// </summary>
        protected string _connectionstring;

        /// <summary>
        /// 類型 RLTP(型態):M:資料庫維護;C:資料檢查;R:報表作業
        /// </summary>
        public SQLRuleType RuleType { get; set; } = SQLRuleType.Check;      
               
        /// <summary>
        /// 依行類型傳回表示字串
        /// </summary>
        public string MTYPE
        {
            get
            {
                string sMty = "M";
                switch (RuleType)
                {
                    case SQLRuleType.Check:
                        sMty = "C";
                        break;
                    case SQLRuleType.Maintain:
                        sMty = "M";
                        break;
                    case SQLRuleType.Report:
                        sMty = "R";
                        break;
                }
                return sMty;
            }
        }

        #region SQL 字串部份

        // 讀取在TRGID 中所有的DBID
        private const string getDBList = @" 
               SELECT DBID,HOST ,DBNM ,USR ,PWD
                 From SS_DBLS D
                WHERE ACT ='Y' 
                  and DBID IN (SELECT D.DBID 
                                 From SS_RCDB D,SS_RULECK R
                                Where D.SRNO =R.SRNO
                                  AND R.TRGID =@TRGID)";

        // 取回所指定的DBID 中的規則
        private const string getRuleList = @"
              SELECT
                     R.SRNO,R.TITL,
                     R.CKSQL,
                     R.LSSQL,
                     D.DBID,
                     R.REMARK
               FROM SS_RCDB D ,SS_RULECK R
              WHERE D.SRNO = R.SRNO 
                AND R.RLTP =@RLTP 
                AND R.ACT ='Y' 
                AND D.DBID = @DBID  
                AND R.TRGID =@TRGID ";

        #endregion SQL 字串部份

        /// <summary>
        /// 群組代碼
        /// </summary>
        /// <value>
        /// 目前要執行的群組代碼，用於區分群組使用
        /// </value>
        public string triggerMapDataValueGID { get; set; }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public int Execute(Callback context)
        {  
                // 計時用
                Stopwatch sw = new Stopwatch();
                Stopwatch swSQL = new Stopwatch();
                sw.Reset();
                sw.Start();
                int result = 0;
                using (IDbConnection Conn = new SqlConnection(_connectionstring))
                {              
                    context($"開始執行作業(TRGID={triggerMapDataValueGID})....." );                   
                   // 找出該TRGID 中所有的DB 並依DB 來執行SQL
                   var DBLS = Conn.Query<Model.SS_DBLS_Mod>(getDBList, new { TRGID = triggerMapDataValueGID });
                
                    foreach (var dbitem in DBLS)
                    {
                        var rules = Conn.Query<Model.CheckRULE>(getRuleList, new { DBID = dbitem.DBID, RLTP = MTYPE, TRGID= triggerMapDataValueGID });
                        // 取出所有管控碼清單
                        foreach (ICheckRule rule in rules)
                        {
                        result++;
                            swSQL.Reset();
                            swSQL.Start();
                            rule.DB_Mod = dbitem;
                            int iCon = 0;
                            try
                            {
                                // 執行檢查或維護
                                if (DoRunSQL(rule, ref iCon))
                                {
                                    // 檢查執行後結果
                                    OnCheckResult(iCon, rule);
                                    swSQL.Stop();
                                    context($"{rule.TITL}({rule.SRNO})完成作業,花費時間為：{swSQL.ElapsedMilliseconds}");
                                }
                            }
                            catch (Exception e)
                            {
                                context($"{rule.TITL}({rule.SRNO})作業失敗-例外為：{e.Message}");
                            }
                        }
                    }
                }
                sw.Stop();
                context($"SQL規則執行作業執行完成-觸發器({triggerMapDataValueGID}),花費時間為：{sw.ElapsedMilliseconds}");
            return result;
        }

        /// <summary>
        /// 檢查執行後結果 會依照模式不同而有不同的處理方式
        /// </summary>
        /// <param name="ResultCount">The result count.</param>
        /// <param name="aRule">a rule.</param>
        /// <remarks>主要給後續在實作內容</remarks>
        protected virtual void OnCheckResult( int ResultCount, ICheckRule aRule )
        { }

        /// <summary>
        /// 執行SQL 作業Does the run SQL.
        /// </summary>
        /// <remarks>
        /// 執行以下項目
        /// 1. 執行SQL
        ///    a.若傳回值為0 表示無問題--> 結束
        ///    b.若傳回值大於0 表示有問題 --> 產生報表 --> 發MAIL 通知
        /// </remarks>
        private bool DoRunSQL( ICheckRule aRule, ref int RecordCount )
        {
            try
            {
                aRule.CKSQL = ParaTrans.Format(aRule.CKSQL);
                switch (RuleType)
                {
                    case SQLRuleType.Check:
                        SqlParameter[] px = { };
                        RecordCount = SqlHelper.ExecuteScalar<int>(aRule.DB_Mod.DBConnString, aRule.CKSQL, px);
                        break;

                    case SQLRuleType.Maintain:
                        RecordCount = SqlHelper.ExecuteNonQuery(aRule.DB_Mod.DBConnString, aRule.CKSQL);
                        break;

                    default:
                        RecordCount = -1;
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                RecordCount = -1;
                return false;
                throw ex;
            }
        }

        /// <summary>
        /// SQL字串中將 ' 取代為''
        /// </summary>
        /// <param name="str">待處理字串</param>
        /// <returns>處理後字串</returns>
        public string sqlstrproc( string str )
        {
            if (str.Contains("'"))
            {
                return str.Replace("'", "''");
            }
            else
            {
                return str;
            }
        }
    }
}