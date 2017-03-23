using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 計算目標介面
    /// </summary>
    public interface ICalcSpace
    {
        /// <summary>
        /// 取得目標路徑
        /// </summary>
        /// <param name="context">傳入參數內容</param>
        /// <returns>目標路徑</returns>
        string GetTarget(PerformContext context);

        /// <summary>
        /// 判斷目標是否存在
        /// </summary>
        /// <param name="target">目標路徑或名稱</param>
        /// <returns><c>True</c>表示存在 <c>False</c>表示不存在</returns>
        bool TargetExists(string target);

        /// <summary>
        /// 計算目標大小
        /// </summary>
        /// <param name="target">目標路徑或名稱</param>
        /// <param name="searchPattern">過濾模式</param>
        /// <returns>目標大小位元組</returns>
        long Calculate(string target, string searchPattern);

        // void SendTo(string info);
    }
}