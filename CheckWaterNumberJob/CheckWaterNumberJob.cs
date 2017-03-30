using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    public class CheckWaterNumberJob : IRecurringJob
    {
        [DisplayName("單別流水號是否有欠缺")]
        public void Execute(PerformContext context)
        {
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} Check Warehouse Running ...");
            // Arrange
            // todo 這里要處理密碼加密的問題
            var connString = context.GetJobData<DBConnectionstring>("ConnectionString");
            context.WriteLine($"ConnectionInfo- IP:{connString.ServerIP} DB:{connString.DBNM} User:{connString.User}");
            var useToDay = context.GetJobData<bool>("UseToDay");
            var startDate = useToDay ? DateTime.Today : context.GetJobData<DateTime>("StartDate");
            var jobdata = context.GetJobData<IList<VchItem>>("ITEM");
            var service = new WaterNumberService(connString.connectionstring);
            var result = new List<COMT>();
            // 執行
            foreach (var pair in jobdata)
            {
                context.WriteLine($"檢查單別為{pair.VCH_TY},是否為為主從表：({pair.MD}) 主表為：{pair.MTable} 子表為：{pair.DTable}");
                result.AddRange(service.Execute(pair.MTable, startDate));
            }

            // 回報
            context.WriteLine($"共更新{result.Count()}筆資料");
            result.ForEach(r => context.WriteLine($"發現欠缺單號{r.VCH_TY}-{r.VCH_NO},日期為{r.VCH_DT:yyyy/MM/dd}"));
            context.WriteLine("完成單別流水號是否有欠缺任務");
        }
    }

    /// <summary>
    ///  參數類別
    /// </summary>
    public class VchItem
    {
        /// <summary>
        /// Gets or sets the vc h_ ty.
        /// </summary>
        /// <value>The vc h_ ty.</value>
        public string VCH_TY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VchItem"/> is md.
        /// </summary>
        /// <value><c>true</c> if md; otherwise, <c>false</c>.</value>
        public bool MD { get; set; }

        /// <summary>
        /// Gets or sets the m table.
        /// </summary>
        /// <value>The m table.</value>
        public string MTable { get; set; }

        /// <summary>
        /// Gets or sets the d table.
        /// </summary>
        /// <value>The d table.</value>
        public string DTable { get; set; }
    }
}