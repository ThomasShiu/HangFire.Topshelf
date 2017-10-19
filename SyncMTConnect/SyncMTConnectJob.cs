using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
  public class SyncMTConnectJob : IRecurringJob
  {
    [DisplayName("MTConnect Data Sync")]
    public void Execute(PerformContext context)
    {
      context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}  SyncMTConnectJob Running ...");
      // Arrange
      // todo 這里要處理密碼加密的問題
      var connString = context.GetJobData<DBConnectionstring>("ConnectionString");
      var action = context.GetJobData<SyncMTCActionType>("Type");
      var today = DateTime.Today;
      ISyncMTCAction service = SyncMTCFactory.GetService(action);
      // 執行     
      service.SyncData(context.WriteLine, connString.connectionstring);
      // 回報
      context.WriteLine("完成");
    }
  }
}

