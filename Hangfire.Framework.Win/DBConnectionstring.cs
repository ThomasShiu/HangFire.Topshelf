namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class DBConnectionstring.
    /// </summary>
    public class DBConnectionstring
    {
        /// <summary>
        /// ���o�s���r�� 
        /// </summary>
        /// <value>The connectionstring.</value>
        public string connectionstring {
            get
            {
                return string.Format(StringFormat, ServerIP, DBNM, User, Pwd);
            }
        }
        /// <summary>
        /// �s���r��榡
        /// </summary>
        /// <value>The string format.</value>
        public string StringFormat { get; set; }
        /// <summary>
        /// �D����m
        /// </summary>
        /// <value>The server ip.</value>
        public string ServerIP { get; set; }
        /// <summary>
        /// ��Ʈw
        /// </summary>
        /// <value>The DBNM.</value>
        public string DBNM { get; set; }
        /// <summary>
        /// �ϥΪ̱b�� 
        /// </summary>
        /// <value>The user.</value>
        public string User { get; set; }
        /// <summary>
        /// �[�K��K�X
        /// </summary>
        /// <value>The password.</value>
        public string Pwd { get; set; }
    }
}