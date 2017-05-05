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
        /// <exception cref="System.Exception">IP��m�榡���~</exception>
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
                else { throw new System.Exception("IP��m�榡���~"); }
            }

        }
        //= 
        /// <summary>
        /// SMTP port
        /// </summary>
        /// <value>The SMTP port.</value>
        public string SMTPPort { get; set; }  //= 

        /// <summary>
        /// �o�e�H���m
        /// </summary>
        /// <value>The mc mail.</value>
        public string MCMail { get; set; }  //= 

        /// <summary>
        /// Mail�H�H�H�W��
        /// </summary>
        /// <value>The name of the mc.</value>
        public string MCName { get; set; }  //= 

        /// <summary>
        /// ����ɶ�
        /// </summary>
        /// <value>The MCQ time.</value>
        public string MCQTime { get; set; }  //= 

        /// <summary>
        /// ���s���ծɶ�
        /// </summary>
        /// <value>The try number.</value>
        public int tryNumber { get; set; }  //= 
        /// <summary>
        /// �P�_�ǤJ�r��O�_�ݦXIP4�榡
        /// </summary>
        /// <param name="ip">���ˬdIP�r��</param>
        /// <returns>True or False</returns>
        public static bool IsIP( string ip )
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

    }
}
