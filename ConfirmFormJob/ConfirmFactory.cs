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
                case ConfirmActionType.Appointment:  // 人事任用 0
                    return new H0451Service();

                case ConfirmActionType.Leave:        // 離職單 1
                    return new H0452Service();

                case ConfirmActionType.Change:       // 人事異動單 2
                    return new H0453Service();

                case ConfirmActionType.Salary:       // 薪資變更單 3
                    return new H0457Service();

                case ConfirmActionType.Health:       // 健保變更單 4
                    return new H0454Service();

                case ConfirmActionType.Labor:        // 勞保變更單 5
                    return new H0455Service();

                case ConfirmActionType.Laborious:    // 勞退變更單 6
                    return new H0456Service();

                case ConfirmActionType.MemberDisable:    // 帳號停用 7
                    return new H0458Service();

                default:
                    return null;
            }
        }
    }
}