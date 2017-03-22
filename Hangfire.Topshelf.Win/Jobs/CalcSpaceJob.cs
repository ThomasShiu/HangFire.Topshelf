using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    internal enum CaleSpaceActoinType
    {
        /// <summary>
        /// 磁區
        /// </summary>
        Drive,

        /// <summary>
        /// 目錄
        /// </summary>
        Folder,

        /// <summary>
        /// 檔案
        /// </summary>
        Files
    }

    /// <summary>
    /// 計算空間大小作業
    /// </summary>
    public class CalcSpaceJob : IRecurringJob
    {
        private CaleSpaceActoinType _action = CaleSpaceActoinType.Folder;
        private ICalcSpace _calc;

        /// <summary>
        /// 執行計算作業
        /// </summary>
        /// <param name="context">The context to <see cref="T:Hangfire.Server.PerformContext" />.</param>
        [DisplayName("計算目標使用空間")]
        public void Execute(PerformContext context)
        {
            /*
             *   1. 顯示訊息
             *   2. 檢查目標是否存在
             *   3. 執行動作
             */
            // Arrange
            context.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} CalcSpaceJob 開始執行 ...");
            _action = context.GetJobData<CaleSpaceActoinType>("Type");
            context.WriteLine($"執行類別為：{_action}");
            _calc = GetCale(_action);
            // 取得目標位置
            var target = _calc.GetTarget(context);
            context.WriteLine($"目標位置：{target}");

            //Checking
            if (!_calc.TargetExists(target))
            {
                context.SetTextColor(ConsoleTextColor.Red);
                context.WriteLine($"目標位置不存在，位置為：{target}");
                context.ResetTextColor();
            }
            // Act
            context.WriteLine($"計算目標{target}");
            var searchPattern = context.GetJobData<string>("searchPattern");
            context.WriteLine($"搜尋模式字串：{searchPattern}");
            var result = _calc.Calculate(target, searchPattern);
            context.WriteLine("目標大小為：{0}", result);
            WriteTo("41DCF8A5ACB74AA38DD3DB8CB1EE5462", DateTime.Now, result);
        }

        private async Task<bool> WriteTo(string serial, DateTime occurrence, long targetSize)
        {
            const string baseUrl = "http://localhost:49988/api/V1.0/";
            var url = $"{baseUrl}/{serial}/{occurrence:yyyyMMddHHmmss}/{targetSize}";
            var client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody == "0";
        }

        private ICalcSpace GetCale(CaleSpaceActoinType actionType)
        {
            switch (actionType)
            {
                case CaleSpaceActoinType.Drive:
                    return new CaleDriveSpace();

                case CaleSpaceActoinType.Folder:
                    return new CaleFolderSpace();

                case CaleSpaceActoinType.Files:
                    return new CaleFileSpace();

                default:
                    return new CaleFolderSpace();
            }
        }
    }
}