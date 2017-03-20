using System;
using Hangfire.Topshelf.Core;
using Serilog;
using Topshelf;

namespace Hangfire.Topshelf
{
	class Program
	{
		static int Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.LiterateConsole()                
				.WriteTo.RollingFile(HangfireSettings.Instance.LogFilePath)
				.CreateLogger();

			return (int)HostFactory.Run(x =>
			{
                x.UseSerilog(Log.Logger); // 將Topshelf log 傳給Serilog
                x.RunAsLocalSystem();   // Service 使用授權模式
                x.StartAutomatically(); // 設為開機後自動起啟用
				x.SetServiceName(HangfireSettings.Instance.ServiceName);
				x.SetDisplayName(HangfireSettings.Instance.ServiceDisplayName);
				x.SetDescription(HangfireSettings.Instance.ServiceDescription);

                // Service 認証使用者方式
				x.UseOwin(baseAddress: HangfireSettings.Instance.ServiceAddress);

				x.SetStartTimeout(TimeSpan.FromMinutes(5));
				//https://github.com/Topshelf/Topshelf/issues/165
				x.SetStopTimeout(TimeSpan.FromMinutes(35));

				x.EnableServiceRecovery(r => { r.RestartService(1); });
			});
		}
	}
}
