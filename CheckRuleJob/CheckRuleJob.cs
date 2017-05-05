using System;
using System.ComponentModel;
using System.Linq;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class CheckWaterNumberJob.
    /// </summary>
    public class CheckRuleJob : IRecurringJob
    {
        /// <summary>
        /// Execute the <see cref="T:Hangfire.RecurringJob" />.
        /// </summary>
        /// <param name="context">The context to <see cref="T:Hangfire.Server.PerformContext" />.</param>
        [DisplayName("執行項目檢查")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} Check Rule Running ...");
            // Arrange
            // todo 這里要處理密碼加密的問題
            var connString = context.GetJobData<DBConnectionstring>("ConnectionString");
            string TRGID = context.GetJobData<string>("GID");
            context.WriteLine($"ConnectionInfo - IP:{connString.ServerIP} DB:{connString.DBNM} User:{connString.User}");
            context.WriteLine($"TriggerID - {TRGID}");

            var jp = context.GetJobData<JobParas>("Paras");            
            var service = new CheckRuleService(connString.connectionstring, jp)
            {
                triggerMapDataValueGID = TRGID
            };

            // 執行
            var result = service.Execute(context.WriteLine);
            // 回報
            context.WriteLine($"共執行了{result}項目");
            context.WriteLine("完成規則檢查任務");      
        }
    }  
}