using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    public class SampleJobA : IRecurringJob
    {
        [DisplayName("外掛測試使用的任務")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} SampleJobA Running ...");

            var intVal = context.GetJobData<int>("IntVal");
            var stringVal = context.GetJobData<string>("StringVal");
            var booleanVal = context.GetJobData<bool>("BooleanVal");
            var simpleObject = context.GetJobData<PamanObject>("SimpleObject");
            context.WriteLine("完成");
        }
    }

    internal class PamanObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}