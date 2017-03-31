using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Hangfire.Topshelf.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace CheckWaterNumberJobTests
{
    [Binding]
    public class 檢查單別流水是否連續Steps
    {
        #region SQL字串
        private static readonly string createTable =
            @"   -- 主檔
                CREATE TABLE [dbo].[COMTs]
                (
                  [VCH_TY] [nchar](4) NOT NULL ,
                  [VCH_NO] [nchar](15) NOT NULL ,
                  [VCH_DT] [smalldatetime] NOT NULL
                    CONSTRAINT [PK_COMT]
                    PRIMARY KEY CLUSTERED ( [VCH_TY] ASC , [VCH_NO] ASC )
                    WITH ( PAD_INDEX = OFF , STATISTICS_NORECOMPUTE = OFF ,
                           IGNORE_DUP_KEY = OFF , ALLOW_ROW_LOCKS = ON ,
                           ALLOW_PAGE_LOCKS = ON ) ON [PRIMARY]
                )
                       ON
                [PRIMARY] ;
                CREATE TABLE [dbo].[EXCHTYPE](
	                [EXCH_TY] [nchar](4) NOT NULL,
	                [TABLE_NM] [nchar](30) NULL	
                 CONSTRAINT [PK_EXCHTYPE] PRIMARY KEY CLUSTERED 
                 ([EXCH_TY] ASC) WITH (PAD_INDEX  = OFF, 
                  STATISTICS_NORECOMPUTE  = OFF, 
                  IGNORE_DUP_KEY = OFF, 
                  ALLOW_ROW_LOCKS  = ON, 
                  ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                ) ON [PRIMARY];
             
            ";
        private static readonly string dropTable = 
            @" IF EXISTS( SELECT * FROM sys.tables WHERE name = 'COMTs' )
                  Drop Table COMTs;
               IF EXISTS( SELECT * FROM sys.tables WHERE name = 'EXCHTYPE' )
                  Drop Table EXCHTYPE;
            ";
        private static readonly string datas =
           @"
           Insert into EXCHTYPE (EXCH_TY,TABLE_NM) Values ('01','COMTs');
            ";

          /*
            Insert into COMTs (VCH_TY,VCH_NO,VCH_DT ) Values ('F712','170104001','2017/01/04');
            Insert into COMTs (VCH_TY,VCH_NO,VCH_DT ) Values ('F712','170104002','2017/01/04');
            Insert into COMTs (VCH_TY,VCH_NO,VCH_DT ) Values ('F712','170104004','2017/01/04');
            Insert into COMTs (VCH_TY,VCH_NO,VCH_DT ) Values ('F712','170104005','2017/01/04');
            Insert into COMTs (VCH_TY,VCH_NO,VCH_DT ) Values ('F712','170104006','2017/01/04');
             */

        private static readonly string connstirng =
          @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        #endregion

        #region Before & After
        [BeforeScenario()]
        public void Before()
        {
            using (IDbConnection conn = new SqlConnection(connstirng))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(dropTable, null, tran);
                    conn.Execute(createTable, null, tran);
                    conn.Execute(datas, null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        [AfterScenario()]
        public void After()
        {
            //using (IDbConnection conn = new SqlConnection(connstirng))
            //{
            //    conn.Open();
            //    var tran = conn.BeginTransaction();
            //    try
            //    {
            //        conn.Execute(dropTable, null, tran);
            //        tran.Commit();
            //        conn.Close();
            //    } catch (Exception)
            //    {
            //        tran.Rollback();
            //        throw;
            //    }
            //}
        }
        #endregion

        [Given(@"目前製令單中有單別資料如下")]
        public void 假設目前製令單中有單別資料如下(Table table)
        {
            using (IDbConnection conn = new SqlConnection(connstirng))
            {
                conn.Open();
                var mformdates = table.CreateSet<COMT>();
                conn.Insert<List<COMT>>(mformdates.ToList());
               // conn.Execute(@"INSERT INTO COMTs (VCH_TY , VCH_NO , VCH_DT) VALUES (@VCHTY,@VCHNO,@VCHDT);", new [](VCHTY = )  );
            }
        }

        [When(@"進行檢查時")]
        public void 當進行檢查時()
        {
            var service = new WaterNumberService(connstirng);
            var startDate = DateTime.Parse("2017/01/01");
            var actual = service.Execute(startDate);
            if (actual != null)
                ScenarioContext.Current.Set<ResuleData>(actual, "actual");

        }

        [Then(@"會有以下資料的產生")]
        public void 那麼會有以下資料的產生(Table table)
        {
            var actual = ScenarioContext.Current.Get<ResuleData>("actual");
            var expected = 1;
            Assert.AreEqual(expected, actual.MForms.Count());
            table.CompareToSet(
                actual.MForms.Select(a => new 
                {
                    VCH_TY = a.VCH_TY,
                    VCH_NO = a.expected
                }));
        }
    }
    /// <summary>
    /// Class COMT.
    /// </summary>
    public class COMT
    {
        /// <summary>
        /// 單別代碼自動化設定單據性質設定任務.
        /// </summary>
        /// <value>The vchty.</value>
        public string VCH_TY { get; set; }
        /// <summary>
        /// 單位性質欄位名稱
        /// </summary>
        /// <value>The field.</value>
        public string VCH_NO { get; set; }
        /// <summary>
        /// 要設定的數值
        /// </summary>
        /// <value>The value.</value>
        public DateTime? VCH_DT { get; set; }
    }
}
