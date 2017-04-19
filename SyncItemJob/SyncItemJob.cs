using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    public class SyncItemJob : IRecurringJob
    {
        [DisplayName("同步料號到各地區")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} Synchronize data running...");
            // Arrange
            // todo 這里要處理密碼加密的問題
            var connString = context.GetJobData<DBConnectionstring>("ConnectionString");
            context.WriteLine($"ConnectionInfo- IP:{connString.ServerIP} DB:{connString.DBNM} User:{connString.User}");
            var useToDay = context.GetJobData<bool>("UseToDay");
            var startDate = useToDay ? DateTime.Today : context.GetJobData<DateTime>("StartDate");
            var toKSCservice = new CCMToKSCService(connString.connectionstring)
            {
                InsertToTemp = context.GetJobData<string>("InsertToTemp"),
                InsertTempToFormal = context.GetJobData<string>("InsertTempToFormal")
            };
            context.WriteLine($"從CCM-->KSC筆數為:{toKSCservice.Execute()}");

            var toSubCservice = new KSCToSubService(connString.connectionstring)
            {
                ScriptFile = context.GetJobData<string>("ToSubSQL")
            };
            context.WriteLine($"從KSC-->Sub筆數為:{toSubCservice.Execute()}");
            context.WriteLine("完成同步料號到各地區任務");
        }
    }
}
