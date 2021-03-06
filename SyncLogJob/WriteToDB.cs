﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Hangfire.Console;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class WriteToDB.
    /// </summary>
    internal class WriteToDB
    {
        private readonly PerformContext _context;

        public WriteToDB(PerformContext context)
        {
            _context = context;
        }
        public void Write(IEnumerable<Ticker> datas)
        {
            using (IDbConnection conn = this.OpenConnection())
            {
                var progress = _context.WriteProgressBar(datas.Count());
                foreach (var ss in datas)
                {
                    if ((ss.EMPLYID == null) || (ss.EMPLYID.Trim().Length != 0))
                    {
                        this.DeleteTicketLog(conn, ss);
                        this.InsertTicketLog(conn, ss);
                    } else
                    {
                    }
                    //  progress.SetValue();
                }
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns>SqlConnection.</returns>
        private SqlConnection OpenConnection()
        {
            var connectString = SyncLogSettings.Instance.DataBaseConnectString;
            _context.WriteLine($"連接字串：{connectString}");
            var connection = new SqlConnection(connectString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Inserts the ticket log.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="logdata">The logdata.</param>
        private void InsertTicketLog(IDbConnection conn, Ticker logdata)
        {
            conn.Execute(
                        @"INSERT INTO HR_TICKET
                            ( YYMMDD , EMPLYID , TKT_HH , TKT_NN , TKT_SS , BL_DT , C_UPDATE , C_FUNC , SN )
                          VALUES
                            ( @YYMMDD,@EMPLYID,@TKT_HH,@TKT_NN,@TKT_SS,@BL_DT,'N','2','RAC-960PEF' )",
                        new
                        {
                            YYMMDD = logdata.YYYYMMDD,
                            EMPLYID = logdata.EMPLYID,
                            TKT_HH = logdata.TKT_HH,
                            TKT_NN = logdata.TKT_NN,
                            TKT_SS = logdata.TKT_SS,
                            BL_DT = logdata.BL_DT
                        });
        }

        /// <summary>
        /// Deletes the ticket log.
        /// </summary>
        /// <param name="conn">The connection.</param>
        /// <param name="logdata">The logdata.</param>
        private void DeleteTicketLog(IDbConnection conn, Ticker logdata)
        {
            conn.Execute(
                        @"DELETE FROM HR_TICKET
                                WHERE YYMMDD =@YYMMDD
                                  AND EMPLYID =@EMPLYID
                                  AND TKT_HH = @TKT_HH
                                  AND TKT_NN =@TKT_NN
                                  AND TKT_SS =@TKT_SS",
                         new
                         {
                             YYMMDD = logdata.YYYYMMDD,
                             EMPLYID = logdata.EMPLYID,
                             TKT_HH = logdata.TKT_HH,
                             TKT_NN = logdata.TKT_NN,
                             TKT_SS = logdata.TKT_SS
                         });
        }
    }
}