using System;
using System.ComponentModel;
using System.Management.Automation;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /* 如何透過 C# 應用程式執行或呼叫 PowerShell 命令
     * 實作部份參考:http://blog.miniasp.com/post/2014/05/09/Call-Windows-PowerShell-Cmdlets-from-CSharp-program.aspx
     * Todo 可以在加入是否使用ps1 檔案
     */

    public class RunPowerShellJob : IRecurringJob
    {
        [DisplayName("執行PowerShell任務")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} RunPowerShell Job Running ...");

             var script = context.GetJobData<string>("script");
           // var script = @"Get-Service";

            using (var ps = PowerShell.Create())
            {
                ps.AddScript(script);

                 foreach (var result in ps.Invoke())
               // foreach (var msg in ps.Invoke<string>())
                {
                    context.WriteLine(result.Members["Name"].Value);
                }
                // 判斷是否有錯誤
                if (ps.Streams.Error.Count != 0)
                {
                    var errors = ps.Streams.Error;
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (var err in errors)
                        {
                            context.WriteLine($"錯誤: {err}");
                        }
                    }
                }
            }
            context.WriteLine("執行Power Shell 完成");
        }
    }
}