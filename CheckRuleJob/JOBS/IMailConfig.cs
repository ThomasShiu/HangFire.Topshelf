namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Interface IMailConfig
    /// </summary>
    public interface IMailConfig
    {
        /// <summary>
        /// Gets or sets the smtpip.
        /// </summary>
        /// <value>The smtpip.</value>
        string SMTPIP { get; set; }

        /// <summary>
        /// SMTP port
        /// </summary>
        /// <value>The SMTP port.</value>
        string SMTPPort { get; set; }

        /// <summary>
        /// 發送信件位置
        /// </summary>
        /// <value>The mc mail.</value>
        string MCMail { get; set; }

        /// <summary>
        /// Mail寄信人名稱
        /// </summary>
        /// <value>The name of the mc.</value>
        string MCName { get; set; }

        /// <summary>
        /// 執行時間
        /// </summary>
        /// <value>The MCQ time.</value>
        string MCQTime { get; set; }

        /// <summary>
        /// 重新測試時間
        /// </summary>
        /// <value>The try number.</value>
        int tryNumber { get; set; }
    }
}