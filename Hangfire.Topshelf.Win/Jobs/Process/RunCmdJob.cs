using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 執行 Windows Command 指令
    /// </summary>
    public class RunCmdJob : IRecurringJob
    {
        /// <summary>
        /// Execute the <see cref="T:Hangfire.RecurringJob" />.
        /// </summary>
        /// <param name="context">The context to <see cref="T:Hangfire.Server.PerformContext" />.</param>
        [DisplayName("執行Win Command")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} RunCmdJob 開始執行 ...");

            var cmd = context.GetJobData<string>("command");
            CmdHelper.Run(cmd, context.WriteLine);

            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} RunCmdJob 完成 ...");
        }

        /// <summary>
        /// Commands the output.  原本寫法 後來改使用CmdHelper
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>System.String.</returns>
        public string CommandOutput(string commandText)
        {
            var p = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    //不跳出cmd視窗
                    CreateNoWindow = true
                }
            };
            string strOutput = null;

            try
            {
                p.Start();

                p.StandardInput.WriteLine(commandText);
                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();//匯出整個執行過程
                p.WaitForExit();
                p.Close();
            } catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
    }
}