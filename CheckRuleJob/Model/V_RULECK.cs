namespace Hangfire.Topshelf.Jobs.Model
{
    /// <summary>
    /// Interface IRuleCK
    /// </summary>
    public interface ICheckRule
    {
        /// <summary>
        /// Gets or sets the srno.
        /// </summary>
        /// <value>The srno.</value>
        string SRNO { get; set; }

        /// <summary>
        /// Gets or sets the titl.
        /// </summary>
        /// <value>The titl.</value>
        string TITL { get; set; }

        /// <summary>
        /// Gets or sets the CKSQL.
        /// </summary>
        /// <value>The CKSQL.</value>
        string CKSQL { get; set; }

        /// <summary>
        /// Gets or sets the LSSQL.
        /// </summary>
        /// <value>The LSSQL.</value>
        string LSSQL { get; set; }

        /// <summary>
        /// Gets or sets the dbid.
        /// </summary>
        /// <value>The dbid.</value>
        string DBID { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        string REMARK { get; set; }

        /// <summary>
        /// Gets or sets the d b_ mod.
        /// </summary>
        /// <value>The d b_ mod.</value>
        IDBLS_Mod DB_Mod { get; set; }
    }

    /// <summary>
    /// Class RULECK.
    /// </summary>
    public partial class CheckRULE : ICheckRule
    {
        /// <summary>
        /// Gets or sets the srno.
        /// </summary>
        /// <value>The srno.</value>
        public string SRNO { get; set; }

        /// <summary>
        /// Gets or sets the titl.
        /// </summary>
        /// <value>The titl.</value>
        public string TITL { get; set; }

        /// <summary>
        /// Gets or sets the CKSQL.
        /// </summary>
        /// <value>The CKSQL.</value>
        public string CKSQL { get; set; }

        /// <summary>
        /// Gets or sets the LSSQL.
        /// </summary>
        /// <value>The LSSQL.</value>
        public string LSSQL { get; set; }

        /// <summary>
        /// Gets or sets the dbid.
        /// </summary>
        /// <value>The dbid.</value>
        public string DBID { get; set; }

        /// <summary>
        /// Gets or sets the remark.
        /// </summary>
        /// <value>The remark.</value>
        public string REMARK { get; set; }

        /// <summary>
        /// Gets or sets the d b_ mod.
        /// </summary>
        /// <value>The d b_ mod.</value>
        public IDBLS_Mod DB_Mod { get; set; }
    }
}