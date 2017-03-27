using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    public class SetVCHCFGJob : IRecurringJob
    {
        [DisplayName("自動化設定單據性質設定")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}設定單據性質設定任務開始執行 ...");
            // Arrange
            var connString= context.GetJobData<DBConnectionstring>("ConnectionString");
            var jobdata = context.GetJobData();
            var service = new SetVchCfgService(connString.connectionstring);
            var field = "CLS_DT";
            var value = "2017/03/03";
            var vchty = "F712";
            // act
            foreach (var pair in jobdata)
            {
                context.WriteLine($"設定單別為{vchty},將欄位({field})設定為{value}");
                service.SetItem(field, value,vchty);
            }
            var count = service.Execute();
            // Feedback
            context.WriteLine($"共更新{count}筆資料");
            context.WriteLine("完成設定單據性質設定任務");
        }
    }

    internal class VchCfgItem
    {
        public string VCHTY { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}