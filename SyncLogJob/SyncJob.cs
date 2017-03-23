using System;
using System.ComponentModel;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    public class SyncJob : IRecurringJob
    {
        private ReadHAMSDB db;

        [DisplayName("同步全盈廠刷卡機資料")]
        public void Execute(PerformContext context)
        {
            context.WriteLine("開始執行同步作業");
            db = new ReadHAMSDB();
            var writer = new WriteToDB();
            context.WriteLine("讀取刷卡資料");
            var datas = db.Reading(DateTime.Today);
            context.WriteLine("寫入刷卡資料");
            writer.Write(datas);
        }
    }
}