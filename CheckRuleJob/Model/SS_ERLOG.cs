using System;

namespace Hangfire.Topshelf.Jobs.Model
{
    /// <summary>
    /// Class SS_ERLOG.
    /// </summary>
    public class SS_ERLOG
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
    }
}
