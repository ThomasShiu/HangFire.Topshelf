using System;

namespace Hangfire.Topshelf.Jobs
{
  public delegate void Callback(string line);

  /// <summary>
  /// 確認作業統一介面
  /// </summary>
  internal interface ISyncMTCAction
  {
    /// <summary>
    /// 確認作業
    /// </summary>
    /// <param name="context"></param>
    /// <param name="execDate">執行日期</param>
    void SyncData(Callback context, string connectionstring);

    /// <summary>
    /// 取消確認作業
    /// </summary>
    /// <param name="context"></param>
    /// <param name="execDate">執行日期</param>
    void UnSyncData(Callback context, string connectionstring);
  }
}