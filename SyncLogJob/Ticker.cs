// ReSharper disable InconsistentNaming
namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 刷卡記錄
    /// </summary>
    internal class Ticker
    {
        /// <summary>
        /// 刷卡日期
        /// </summary>
        /// <value>The yyyymmdd.</value>
        public string YYYYMMDD { get; set; }

        /// <summary>
        /// 員工編號
        /// </summary>
        /// <value>The emplyid.</value>
        public string EMPLYID { get; set; }

        /// <summary>
        /// 員工姓名
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// 刷卡時
        /// </summary>
        /// <value>The tk t_ hh.</value>
        public int TKT_HH { get; set; }

        /// <summary>
        /// 刷卡分
        /// </summary>
        /// <value>The tk t_ nn.</value>
        public int TKT_NN { get; set; }

        /// <summary>
        /// 刷卡秒
        /// </summary>
        /// <value>The tk t_ ss.</value>
        public int TKT_SS { get; set; }

        /// <summary>
        /// 刷卡日期
        /// </summary>
        /// <value>The b l_ dt.</value>
        public string BL_DT { get; set; }
    }
}