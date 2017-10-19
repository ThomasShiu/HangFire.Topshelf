namespace Hangfire.Topshelf.Jobs
{
  /// <summary>
  /// 確認類別工廠
  /// </summary>
  internal class SyncMTCFactory
  {
    internal static ISyncMTCAction GetService(SyncMTCActionType actionType)
    {
      switch (actionType)
      {
        case SyncMTCActionType.Log:           // 取得MTConnect LOG 資料 0
          return new GetLogService();

        case SyncMTCActionType.Sensor:        // 取得MTConnect Sensor 資料 1
          return new GetSensorService();

        case SyncMTCActionType.Current:       // 取得MTConnect Current 資料 2
          return new GetCurrentService();

        case SyncMTCActionType.Target:        // 更新產能資料 3
          return new GetTargetService();

        default:
          return null;
      }
    }
  }
}