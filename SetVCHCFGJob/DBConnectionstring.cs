namespace Hangfire.Topshelf.Jobs
{
    public class DBConnectionstring
    {
        public string connectionstring {
            get
            {
                return string.Format(StringFormat, ServerIP, DBNM, User, Pwd);
            }
        }
        public string StringFormat { get; set; }
        public string ServerIP { get; set; }
        public string DBNM { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
    }
}