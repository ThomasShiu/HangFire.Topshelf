using System;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.RecurringJobExtensions;

namespace Hangfire.Topshelf.Jobs
{
    public class MyJob1 : IRecurringJob
	{
		public void Execute(PerformContext context)
		{
			context.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} MyJob1 Running ...");
		}
	}
}
