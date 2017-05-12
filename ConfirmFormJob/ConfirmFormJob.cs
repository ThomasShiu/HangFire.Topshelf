using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs.ConfirmFormJob
{
    public class ConfirmFormJob : IRecurringJob
    {
        [DisplayName("外掛測試使用的任務")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} SampleJobA Running ...");

            var intVal = context.GetJobData<int>("IntVal");        
            context.WriteLine("完成");
        }
    }
}
