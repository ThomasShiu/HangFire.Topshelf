using System;
using System.Threading;
using Hangfire.Topshelf.Core;
using Serilog;
using Topshelf;

namespace Hangfire.Topshelf.Win
{
    internal class Program
    {
        private static int Main(string[] args)
        {
#if DEBUG
            // 檢查是否有重複執行程式
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                System.Console.Write("Cant boot another Hangfire.TopShelf for running state");
                Thread.Sleep(5000);
                return -1;
            }
#endif

            // 建立 Logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.LiterateConsole()
                .WriteTo.RollingFile(HangfireSettings.Instance.LogFilePath)
                .CreateLogger();
            // 起動Service
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

/* 增加項目清單
 * 1. 程式參數的應用
 * 2.
 */