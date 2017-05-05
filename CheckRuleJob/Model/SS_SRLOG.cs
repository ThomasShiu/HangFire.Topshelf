// ***********************************************************************
// 程式集(Assembly)         : CallServer
// 作者(Author)             : levi
// 建立日期(Created)          : 02-04-2016
//
// Last Modified By : levi
// Last Modified On : 01-29-2016
// ***********************************************************************
// <copyright file="SS_SRLOG.cs" company="CCM">
//     Copyright ?  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;


namespace Hangfire.Topshelf.Jobs.Model
{
    /// <summary>
    /// Class SS_SRLOG.
    /// </summary>
    public class SS_SRLOG
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
        /// Gets or sets the fmno.
        /// </summary>
        /// <value>The fmno.</value>
        public string FMNO { get; set; }

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>The host.</value>
        public string HOST { get; set; }

        /// <summary>
        /// Gets or sets the DBNM.
        /// </summary>
        /// <value>The DBNM.</value>
        public string DBNM { get; set; }

        /// <summary>
        /// Gets or sets the SQL.
        /// </summary>
        /// <value>The SQL.</value>
        public string SQL { get; set; }

        /// <summary>
        /// Gets or sets the acflag.
        /// </summary>
        /// <value>The acflag.</value>
        public string ACFLAG { get; set; }

        /// <summary>
        /// Gets or sets the runtm.
        /// </summary>
        /// <value>The runtm.</value>
        public decimal? RUNTM { get; set; }

    }
}
