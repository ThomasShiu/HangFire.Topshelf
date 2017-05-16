using System;

namespace Hangfire.Topshelf.Jobs.ConfirmFormJob
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
        case ConfirmActionType.Appointment:
          return new H0451Service();        
        case ConfirmActionType.Leave:
          break;
        case ConfirmActionType.Change:
          break;
        case ConfirmActionType.Salary:
          break;
        case ConfirmActionType.Health:
          break;
        case ConfirmActionType.Labor:
          break;
        case ConfirmActionType.Laborious:
          break;
        default:
          break;
      }
      return null;
    }
  }
}