using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Dapper;
using Hangfire.Topshelf.Jobs.Model;
using Hangfire.Topshelf.Jobs.Code;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 檢查規則物件
    /// </summary>
    public class CheckRuleService : SQLRuleRun
    {
        /// <summary>
        /// 取得要通知人員名單 SQL 字串
        /// </summary>
        private const string SS_MUR_SS_MUSR = @"
                   select b.MUSR,b.EMAIL,b.NAME
                     from SS_MUR a 
                left join SS_MUSR b 
                       on a.MUSR=b.MUSR
                    where a.SRNO=@SRNO
                      AND ACT=@ACT";   

        /// <summary>
        /// Gets or sets the mail configuration.
        /// </summary>
        /// <value>The mail configuration.</value>
        public IMailConfig MailConfig { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRule" /> class.
        /// </summary>
        /// <param name="connnstring">The connnstring.</param>
        /// <param name="aConfig">a configuration.</param>
        public CheckRuleService(string connnstring, IMailConfig aConfig)
        {
            MailConfig = aConfig;
            _connectionstring = connnstring;
            RuleType = SQLRuleType.Check; //RLTP(型態):M:資料庫維護;C:資料檢查
        }

        /// <summary>
        /// 檢查執行後結果 會依照模式不同而有不同的處理方式
        /// </summary>
        /// <param name="ResultCount">檢查後筆數</param>
        /// <param name="aRule">a rule.</param>
        /// <remarks>主要給後續在實作內容</remarks>
        protected override void OnCheckResult(int ResultCount, ICheckRule aRule)
        {
                if (ResultCount != 0) //當筆數大於0時表示檢查結果需要輸出報表
                {
                    aRule.LSSQL = ParaTrans.Format(aRule.LSSQL);
                    SqlParameter[] px = { };
                    DataTable dt4 = SqlHelper.ExecuteDataTable(aRule.DB_Mod.DBConnString, aRule.LSSQL, px);
                    DoSendMail(aRule, GenerateHTMLReport.GenerateBody(dt4, aRule).ToString());
                };           
        }

        /// <summary>
        /// Does the send mail.
        /// </summary>
        /// <param name="aRule">a rule.</param>
        /// <param name="aBody">a body.</param>
        /// <returns><c>true</c> 表示成功, <c>false</c> 反然之</returns>
        protected bool DoSendMail(ICheckRule aRule, string aBody)
        {
            // 從資料庫找出所有 SRNO中要通知的人員
            using (var conn = new SqlConnection(this._connectionstring))
            {
                List<SS_MUR> Murs = ((List<SS_MUR>)conn.Query<SS_MUR>(SS_MUR_SS_MUSR, new { SRNO = aRule.SRNO, ACT = 'Y' }));
                ThreadStart starter = () => mailProc(aRule, Murs, aBody);
                Thread Td = new Thread(starter);
                Td.Start();
            }
            return true;
        }

        /// <summary>
        /// 呼叫發送信件
        /// </summary>
        /// <param name="aRule">a rule.</param>
        /// <param name="Murs">The murs.</param>
        /// <param name="aBody">a body.</param>
        //    protected void mailProc( string aSRNO, string AMUSR, string ANAME, string AEMAIL, string aTitle,string aBody)
        protected void mailProc(ICheckRule aRule, List<SS_MUR> Murs, string aBody)
        {
            MC mc1 = new MC(MailConfig)
            {
                SRNO = aRule.SRNO,
                NTYPE = "MA",
                MTYPE = MTYPE,
                ToUsers = Murs,
                SRNOTITL = aRule.TITL,
                MailBody = aBody
            };
            mc1.Execute();
        }
    }
}