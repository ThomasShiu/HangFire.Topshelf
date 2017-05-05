using System;

namespace Hangfire.Topshelf.Jobs.Model
{
    /// <summary>
    /// Class SS_CDLOG.
    /// </summary>
    public class SS_CDLOG
    {
        /// <summary>
        /// Gets or sets the dt.
        /// </summary>
        /// <value>The dt.</value>
        public DateTime? DT { get; set; }
        /// <summary>
        /// Gets or sets the MSG.
        /// </summary>
        /// <value>The MSG.</value>
        public string MSG { get; set; }
        /// <summary>
        /// Gets or sets the mtype.
        /// </summary>
        /// <value>The mtype.</value>
        public string MTYPE { get; set; }
        /// <summary>
        /// Gets or sets the runtm.
        /// </summary>
        /// <value>The runtm.</value>
        public decimal? RUNTM { get; set; }
    }
}
