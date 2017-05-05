using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class WaterNumberService.
    /// </summary>
    public class RuleService : IRuleService
    {
       
        private readonly string _connectionstring;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleService"/> class.
        /// </summary>
        /// <param name="connnstring">The connnstring.</param>
        public RuleService(string connnstring)
        {
            _connectionstring = connnstring;
        }
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <returns>IEnumerable&lt;COMT&gt;.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Execute(DateTime startDate)
        {           
        }
    }

    

    

 
}
