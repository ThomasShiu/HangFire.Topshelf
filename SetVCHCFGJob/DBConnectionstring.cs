namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class DBConnectionstring.
    /// </summary>
    public class DBConnectionstring
    {
        /// <summary>
        /// 取得連接字串 
        /// </summary>
        /// <value>The connectionstring.</value>
        public string connectionstring {
            get
            {
                return string.Format(StringFormat, ServerIP, DBNM, User, Pwd);
            }
        }
        /// <summary>
        /// 連接字串格式
        /// </summary>
        /// <value>The string format.</value>
        public string StringFormat { get; set; }
        /// <summary>
        /// 主機位置
        /// </summary>
        /// <value>The server ip.</value>
        public string ServerIP { get; set; }
        /// <summary>
        /// 資料庫
        /// </summary>
        /// <value>The DBNM.</value>
        public string DBNM { get; set; }
        /// <summary>
        /// 使用者帳號 
        /// </summary>
        /// <value>The user.</value>
        public string User { get; set; }
        /// <summary>
        /// 加密後密碼
        /// </summary>
        /// <value>The password.</value>
        public string Pwd { get; set; }
    }
}