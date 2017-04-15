namespace Hangfire.Topshelf.Jobs.Tests
{
    internal class CCMToKSC
    {
        private string _connstirng1;
        private string _connstirng2;

        public CCMToKSC(string connstirng1, string connstirng2)
        {
            this._connstirng1 = connstirng1;
            this._connstirng2 = connstirng2;
        }

        public string SourecTable { get; set; }
        public string DestinationTable { get; set; }

        public int Execute()
        {
            return 0;
        }
    }
}