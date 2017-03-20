using System.IO;
using System.Linq;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    internal class CaleFolderSpace : ICalcSpace
    {
        /// <summary>
        /// 判斷目標是否存在
        /// </summary>
        /// <param name="target">目標路徑或名稱是否存在</param>
        /// <returns><c>True</c>表示存在 <c>False</c>表示不存在</returns>
        public bool TargetExists(string target)
        {
            return Directory.Exists(target);
        }

        /// <summary>
        /// 計算目標大小
        /// </summary>
        /// <param name="target">目標路徑或名稱</param>
        /// <param name="searchPattern">過濾模式</param>
        /// <returns>目標大小位元組</returns>
        public long Calculate(string target, string searchPattern)
        {
            var dir = new DirectoryInfo(target);
            return dir.GetFiles(searchPattern, SearchOption.AllDirectories)
                      .Sum(x => x.Length);
        }

        public string GetTarget(PerformContext context)
        {
            return context.GetJobData<string>("Path");
        }
    }
}