using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangfire.Topshelf.Jobs.Model
{
    /// <summary>
    /// Class SS_TRGLST.
    /// </summary>
    public class SS_TRGLST
    {
        /// <summary>
        /// Gets or sets the trgid.
        /// </summary>
        /// <value>The trgid.</value>
        public string TRGID { get; set; }
        /// <summary>
        /// Gets or sets the TRGNM.
        /// </summary>
        /// <value>The TRGNM.</value>
        public string TRGNM { get; set; }
        /// <summary>
        /// Gets or sets the cron.
        /// </summary>
        /// <value>The cron.</value>
        public string CRON { get; set; }
        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        /// <value>The act.</value>
        public string ACT { get; set; }
        /// <summary>
        /// Gets the is active.
        /// </summary>
        /// <value>The is active.</value>
        public Boolean IsActive {
          get{
              if (this.ACT == "Y")
                  return true;
              else
                  return false;
        }
        }

    } 
}
