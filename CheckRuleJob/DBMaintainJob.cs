using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class CheckWaterNumberJob.
    /// </summary>
    public class DBMaintainJob : IRecurringJob
    {
        /// <summary>
        /// Execute the <see cref="T:Hangfire.RecurringJob" />.
        /// </summary>
        /// <param name="context">The context to <see cref="T:Hangfire.Server.PerformContext" />.</param>
        [DisplayName("執行維護項目")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} Maintain Rule Running ...");
            // Arrange
            // todo 這里要處理密碼加密的問題
            var connString = context.GetJobData<DBConnectionstring>("ConnectionString");
            string TRGID = context.GetJobData<string>("GID");
            context.WriteLine($"ConnectionInfo - IP:{connString.ServerIP} DB:{connString.DBNM} User:{connString.User}");
            context.WriteLine($"TriggerID - {TRGID}");
            var service = new DBMaintainService(connString.connectionstring)
            {
                triggerMapDataValueGID = TRGID
            };
            // 執行
            var result = service.Execute(context.WriteLine);
            // 回報
            context.WriteLine($"共執行了{result}項目");
            context.WriteLine("完成規則維護任務");
        }
    }
}