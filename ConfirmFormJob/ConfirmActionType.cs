namespace Hangfire.Topshelf.Jobs
{
  public enum ConfirmActionType
  {
    /// <summary>
    /// 任用
    /// </summary>
    Appointment = 0,
    /// <summary>
    /// 離職
    /// </summary>
    Leave = 1,
    /// <summary>
    /// 異動
    /// </summary>
    Change = 2,
    /// <summary>
    /// 薪資
    /// </summary>
    Salary = 3,
    /// <summary>
    /// 健保
    /// </summary>
    Health = 4,
    /// <summary>
    /// 勞保
    /// </summary>
    Labor = 5,
    /// <summary>
    /// 勞退
    /// </summary>
    Laborious =6
  }
}
