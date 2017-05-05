using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hangfire.Framework.Win;
using Hangfire.Topshelf.Jobs;

namespace Hangfire.Topshelf.Jobs.Tests
{
    [TestClass()]
    public class SyncItemServiceTests
    {
        #region SQL字串

        private static readonly string connstirng1 =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private static readonly string connstirng2 =
            @"Data Source=192.168.100.18;Initial Catalog=KSC_15;Persist Security Info=True;User ID=sa;Password=6937937";

        private static readonly string ccm_createTableFile = @"CCM_CreateTable.sql";
        private static readonly string ksc_createTableFile = @"KSC_CreateTable.sql";
        private static readonly string nbg_createTableFile = @"NBG_CreateTable.sql";

        private static readonly string dropTable = @"
            IF EXISTS( SELECT * FROM sys.tables WHERE name = 'ITEM' )
            Drop Table ITEM;
            IF EXISTS( SELECT * FROM sys.tables WHERE name = 'NBG_ITEM' )
            Drop Table ITEM;
            ";

        private static readonly string ClearTable = @"
            IF EXISTS( SELECT * FROM sys.tables WHERE name = 'NBG_ITEM' )
            Delete from ITEM;
            IF EXISTS( SELECT * FROM sys.tables WHERE name = 'NBG_ITEM' )
            Delete from NBG_ITEM;
            ";

        private static readonly string ccm_Item = @"CCM_dbo_ITEM.sql";
        private static readonly string ksc_Item = @"KSC_dbo_ITEM.sql";
        private static readonly string nbg_Item = @"NBG_dbo_ITEM.sql";

        #endregion SQL字串

        #region 起始化及還原
       // [ClassInitialize()]
        public static void Initialize(TestContext testContext)
        {
            // TestDB
            using (IDbConnection conn = new SqlConnection(connstirng1))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    
                    conn.Execute(SQLSyntaxHelper.ReadSQLFile(ccm_createTableFile), null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            // TestDB2
            using (IDbConnection conn = new SqlConnection(connstirng2))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    // ksc
                    conn.Execute(SQLSyntaxHelper.ReadSQLFile(ksc_createTableFile), null, tran);
                    // nbg
                    conn.Execute(SQLSyntaxHelper.ReadSQLFile(nbg_createTableFile), null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

      //  [ClassCleanup]
        public static void Cleanup()
        {
            // TestDB
            using (IDbConnection conn = new SqlConnection(connstirng1))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(dropTable, null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            // TestDB2
            using (IDbConnection conn = new SqlConnection(connstirng1))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(dropTable, null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

     //   [TestCleanup]
        public void TestCleanup()
        {  // TestDB
            using (IDbConnection conn = new SqlConnection(connstirng1))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(ClearTable, null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            // TestDB2
            using (IDbConnection conn = new SqlConnection(connstirng2))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(ClearTable, null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        #endregion

        [TestMethod()]
        public void 台灣轉到昆山_會轉入1筆資料並會繁簡轉換()
        {
            // Arrangey
            //using (IDbConnection conn = new SqlConnection(connstirng1))
            //{
            //    conn.Open();
            //    var tran = conn.BeginTransaction();
            //    try
            //    {
            //        conn.Execute(SQLSyntaxHelper.ReadSQLFile(ccm_Item), null, tran);
            //        tran.Commit();
            //        conn.Close();
            //    } catch (Exception)
            //    {
            //        tran.Rollback();
            //        throw;
            //    }
            //}
            var target = new CCMToKSCService(connstirng2)
            {
             InsertToTemp = "InsertToTemp.SQL",
             InsertTempToFormal = "InsertTempToFormal.SQL",
            };
            // Act
            var actualCount = target.Execute();
            // Assert
            var count = 1;
            var expected = "保护镜";
            // 判斷筆數是否正確
            Assert.AreEqual(actualCount, count);
            using (IDbConnection conn = new SqlConnection(connstirng2))
            {
                conn.Open();
                var actual =
                    conn.ExecuteScalar<string>(@"Select Rtrim(ITEM_NM) from ITEM Where ITEM_NO ='2AMMU0165'");
                conn.Close();
                Assert.AreEqual(actual, expected);
            }
        }

        //[TestMethod()]
        //public void 昆山轉寧波_原有4筆經更新後變10筆原本4筆也更新成跟昆山一致()
        //{
        //    using (IDbConnection conn = new SqlConnection(connstirng2))
        //    {
        //        conn.Open();
        //        var tran = conn.BeginTransaction();
        //        try
        //        {
        //            // ksc
        //            conn.Execute(SQLSyntaxHelper.ReadSQLFile(ksc_Item), null, tran);
        //            // nbg
        //            conn.Execute(SQLSyntaxHelper.ReadSQLFile(nbg_Item), null, tran);
        //            tran.Commit();
        //            conn.Close();
        //        } catch (Exception)
        //        {
        //            tran.Rollback();
        //            throw;
        //        }
        //    }
        //    // Arrange
        //    var target = new KSCToSubsidiaries(connstirng1, connstirng2);
        //    // Act
        //    target.SourecTable = "ITEM";
        //    target.DestinationTable = "NBG_ITEM";
        //    int actual_Count = target.Execute();
        //    // Assert
        //    var count = 10;
        //    var expected = "PSL-1500-C3+PSE/CKS1009";
        //    // 判斷筆數是否正確
        //    Assert.AreEqual(actual_Count, count);
        //    using (IDbConnection conn = new SqlConnection(connstirng2))
        //    {
        //        conn.Open();
        //        var actual =
        //            conn.ExecuteScalar<string>(@"Select ITEM_SP from ITEM Where ITEM_NO ='7SPSL15-0001'");
        //        conn.Close();
        //        Assert.AreEqual(actual, expected);
        //    }
        //}
    }
}