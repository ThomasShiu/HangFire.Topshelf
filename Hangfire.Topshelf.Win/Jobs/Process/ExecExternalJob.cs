using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 執行外部程式等待方式
    /// </summary>
    internal enum WaitForType
    {
        /// <summary>
        /// 不等待,只呼叫
        /// </summary>
        DonotCare = 0,
        /// <summary>
        /// 等待 但只限定於 Time Out 時間
        /// </summary>
        WaitForTimeOut = 1,
        /// <summary>
        /// 一直等待到執行完成
        /// </summary>
        WaitFor = 2
    }

    /// <summary>
    /// 執行外部程的呼叫
    /// </summary>
    public class ExecExternalJob : IRecurringJob
    {
        /// <summary>
        /// Execute the <see cref="T:Hangfire.RecurringJob" />.
        /// </summary>
        /// <param name="context">The context to <see cref="T:Hangfire.Server.PerformContext" />.</param>
        [DisplayName("執行呼叫外部程式")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyycontext.WriteLine(y/MM/dd HH:mm:ss} ExecExternalJob 開始執行 ...");
            var wft = context.GetJobData<WaitForType>("WaitFor");
            var program = context.GetJobData<string>("Program");
            var cmd = context.GetJobData<string>("command");
            var timeOut = context.GetJobData<int>("TimeOut");
            switch (wft)
            {
                case WaitForType.DonotCare:
                    context.WriteLine(CmdHelper.RunProgram(program, cmd));
                    break;
                case WaitForType.WaitForTimeOut:
                   context.WriteLine( CmdHelper.RunProgram(program, cmd, timeOut));
                    break;
                case WaitForType.WaitFor:
                    context.WriteLine(CmdHelper.RunProgramWaitFor(program, cmd));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} ExecExternalJob 完成 ...");
        }
    }
}