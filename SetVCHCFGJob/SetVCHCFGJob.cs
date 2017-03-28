using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 自動化設定單據性質設定任務.
    /// </summary>
    public class SetVCHCFGJob : IRecurringJob
    {
        [DisplayName("自動化設定單據性質設定")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}設定單據性質設定任務開始執行 ...");
            // Arrange
            var connString= context.GetJobData<DBConnectionstring>("ConnectionString");
            var jobdata = context.GetJobData<IList<VchCfgItem>>("ITEM");
            var service = new SetVchCfgService(connString.connectionstring);
            // 在這個任務中強制要給昨天日期
            var yesterday = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");
            // act
            foreach (var pair in jobdata)
            {
                context.WriteLine($"設定單別為{pair.VCHTY},將欄位({pair.Field})設定為{pair.Value}");
                service.SetItem(pair.Field,yesterday , pair.VCHTY);
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