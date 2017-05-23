namespace Hangfire.Topshelf.Jobs
{
  /// <summary>
  /// 確認類別工廠
  /// </summary>
  internal class ConfirmFactory
  {
    internal static IConfirmAction GetService(ConfirmActionType actionType)
    {
      switch (actionType)
      {
        case ConfirmActionType.Appointment:  // 人事任用
          return new H0451Service();

        case ConfirmActionType.Leave:        // 離職單
          return new H0451Service();

        case ConfirmActionType.Change:       // 人事異動單
          return new H0451Service();

        case ConfirmActionType.Salary:       // 薪資變更單
          return new H0451Service();

        case ConfirmActionType.Health:       // 健保變更單
          return new H0451Service();

        case ConfirmActionType.Labor:        // 勞保變更單
          return new H0451Service();

        case ConfirmActionType.Laborious:    // 勞退變更單
          return new H0451Service();

        default:
          return null;
      }
    }
  }
}