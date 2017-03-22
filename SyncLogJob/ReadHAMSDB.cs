using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Topshelf.Jobs.DataModels;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// Class ReadHAMSDB.
    /// </summary>
    internal class ReadHAMSDB
    {
        public IEnumerable<Ticker> Reading(DateTime eventDate)
        {
            HAMSDB baseDb = new HAMSDB("HAMSBase");
            HAMS_2017DB db = new HAMS_2017DB("HAMS");
            var xx = baseDb.Emps.Select(p => new { p.Emp_Id, p.Emp_no }).ToList();
            var ss = db.PubEvents.Where(p => DateTime.Parse(p.eventDate) >= eventDate &&
                                   DateTime.Parse(p.eventDate) <= eventDate)
                        .Select(p => new Ticker
                        {
                            YYYYMMDD = p.eventDate,
                            EMPLYID = xx.Where(x => x.Emp_Id == p.personID).Select(x => x.Emp_no).SingleOrDefault<string>(),
                            Name = p.personName,
                            TKT_HH = DateTime.Parse(p.eventTime).Hour,
                            TKT_NN = DateTime.Parse(p.eventTime).Minute,
                            TKT_SS = DateTime.Parse(p.eventTime).Second,
                            BL_DT = p.eventDate,
                        });
            return ss.ToList();
        } 
    }
}