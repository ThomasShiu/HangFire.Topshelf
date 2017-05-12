using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Framework.Win;

namespace Hangfire.Topshelf.Jobs
{
   public class H0451Service : IConfirmAction
    {
        public delegate void Callback(string line);
        public string ConnString { get; set; }    
       
        public H0451Service(string connectionstring)
        {
            ConnString = connectionstring;          
        }
        public void Confirm(Callback context)
        {  // 計時用
            Stopwatch sw = new Stopwatch();
           
            sw.Reset();
            sw.Start();
            int result = 0;
            using (IDbConnection Conn = new SqlConnection(ConnString))
            {
                throw new NotImplementedException();


                sw.Stop();
                context($"SQL規則執行作業執行完成-觸發器({triggerMapDataValueGID}),花費時間為：{sw.ElapsedMilliseconds}");
            }
        }

        public void UnConfirm()
        {
            throw new NotImplementedException();
        }
    }
}
