using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;
using Hangfire.Console;

namespace Hangfire.Topshelf.Jobs
{
    public class CheckWaterNumberJob : IRecurringJob
    {
        [DisplayName("各倉庫的檢查")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} Check Warehouse Running ...");
            // Arrange
            // todo 這里要處理密碼加密的問題
            var connnectionstring = context.GetJobData<string>("ConnectionString");
            WaterNumberSettings.Instance.ConnectionString = connnectionstring;
            // 執行
         
            // 回報
            context.WriteLine("完成");
        }
    }
}
