using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
  public class ConfirmFormJob:IRecurringJob
  {
    [DisplayName("人事異動作業自動確認作業")]
    public void Execute(PerformContext context)
    {
      context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} 人事異動作業 ConfirmFormJob Running ...");
      // Arrange
      // todo 這里要處理密碼加密的問題
      var connString = context.GetJobData<DBConnectionstring>("ConnectionString");
      var action = context.GetJobData<ConfirmActionType>("Type");
      var today = DateTime.Today;
      IConfirmAction service = ConfirmFactory.GetService(action);    
      // 執行     
      service.Confirm(context.WriteLine, connString.connectionstring, today);
      // 回報
      context.WriteLine("完成");
    }
  }
}
