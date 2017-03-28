using System;
using System.ComponentModel;
using Hangfire.Common;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class MyJob2.
    /// </summary>
    public class MyJob2 : IRecurringJob
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        [DisplayName("MyJob2 Test")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob2 Running ...");

            var intVal = context.GetJobData<int>("IntVal");

            var stringVal = context.GetJobData<string>("StringVal");

            var booleanVal = context.GetJobData<bool>("BooleanVal");

            //var simpleObject = context.GetJobData<SimpleObject>("SimpleObject");

            // context.WriteLine($"IntVal:{intVal},StringVal:{stringVal},BooleanVal:{booleanVal},simpleObject:{JobHelper.ToJson(simpleObject)}");
            context.WriteLine($"IntVal:{intVal},StringVal:{stringVal},BooleanVal:{booleanVal}");
        }
    }
}