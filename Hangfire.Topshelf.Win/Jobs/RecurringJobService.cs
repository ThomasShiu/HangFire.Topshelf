﻿using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
	public class RecurringJobService
	{
		[RecurringJob("*/1 * * * *")]
		[DisplayName("InstanceTestJob")]
		[Queue("jobs")]        
		public void InstanceTestJob(PerformContext context)
		{
			context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} InstanceTestJob Running ...");
		}

		[RecurringJob("*/5 * * * *")]
		[DisplayName("JobStaticTest")]
		[Queue("jobs")]
		public static void StaticTestJob(PerformContext context)
		{
			context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} StaticTestJob Running ...");
		}
	}
}
