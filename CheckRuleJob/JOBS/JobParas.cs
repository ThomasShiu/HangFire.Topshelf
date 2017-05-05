using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class JobParas.
    /// </summary>
    public class JobParas : IMailConfig
    {        
        /// <summary>
        /// The fsmtpip
        /// </summary>
        private string FSMTPIP;
        /// <summary>
        /// Gets or sets the smtpip.
        /// </summary>
        /// <value>The smtpip.</value>
        /// <exception cref="System.Exception">IP位置格式錯誤</exception>
        public string SMTPIP
        {
            get
            {
                return FSMTPIP;
            }
            set
            {
                if (IsIP(value))
                {
                    FSMTPIP = value;
                }
                else { throw new System.Exception("IP位置格式錯誤"); }
            }

        }
        //= 
        /// <summary>
        /// SMTP port
        /// </summary>
        /// <value>The SMTP port.</value>
        public string SMTPPort { get; set; }  //= 

        /// <summary>
        /// 發送信件位置
        /// </summary>
        /// <value>The mc mail.</value>
        public string MCMail { get; set; }  //= 

        /// <summary>
        /// Mail寄信人名稱
        /// </summary>
        /// <value>The name of the mc.</value>
        public string MCName { get; set; }  //= 

        /// <summary>
        /// 執行時間
        /// </summary>
        /// <value>The MCQ time.</value>
        public string MCQTime { get; set; }  //= 

        /// <summary>
        /// 重新測試時間
        /// </summary>
        /// <value>The try number.</value>
        public int tryNumber { get; set; }  //= 
        /// <summary>
        /// 判斷傳入字串是否待合IP4格式
        /// </summary>
        /// <param name="ip">待檢查IP字串</param>
        /// <returns>True or False</returns>
        public static bool IsIP( string ip )
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

    }
}
