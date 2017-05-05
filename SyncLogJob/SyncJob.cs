using System;
using System.ComponentModel;
using System.Linq;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    public class SyncJob : IRecurringJob
    {      
        [DisplayName("同步全盈廠刷卡機資料")]
        public void Execute(PerformContext context)
        {
            context.WriteLine("開始執行同步作業");
            var db = new ReadHAMSDB(context);
            var writer = new WriteToDB(context);
            context.WriteLine("讀取刷卡資料");
            var datas = db.Reading(DateTime.Today);
            context.WriteLine($"資料筆數：{datas.Count()}筆");
            context.WriteLine("寫入刷卡資料");
            writer.Write(datas);
        }
    }
}