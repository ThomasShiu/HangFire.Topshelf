using System.ComponentModel;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// SQL 執行作業類別
    /// </summary>
    public enum SQLRuleType
    {
        /// <summary>
        /// The check
        /// </summary>
        [Description("檢查作業")]
        Check = 0,

        /// <summary>
        /// The maintain
        /// </summary>
        [Description("維護作業")]
        Maintain = 1,
        /// <summary>
        /// The maintain
        /// </summary>
        [Description("報表作業")]
        Report = 2
    }
}