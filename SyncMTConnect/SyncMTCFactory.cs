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
                case SyncMTCActionType.Appointment:  // 取得MTConnect LOG 資料 0
          return new GetLogService();

                case SyncMTCActionType.Leave:        // 離職單 1
                    return new H0452Service();

                case SyncMTCActionType.Change:       // 人事異動單 2
                    return new H0453Service();

                case SyncMTCActionType.Salary:       // 薪資變更單 3
                    return new H0457Service();

                case SyncMTCActionType.Health:       // 健保變更單 4
                    return new H0454Service();

                case SyncMTCActionType.Labor:        // 勞保變更單 5
                    return new H0455Service();

                case SyncMTCActionType.Laborious:    // 勞退變更單 6
                    return new H0456Service();

                case SyncMTCActionType.MemberDisable:    // 帳號停用 7
                    return new H0458Service();

                default:
                    return null;
            }
        }
    }
}