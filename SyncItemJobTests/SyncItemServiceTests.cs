using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hangfire.Framework.Win;

namespace Hangfire.Topshelf.Jobs.Tests
{
    [TestClass()]
    public class SyncItemServiceTests
    {
        #region SQL字串

        private static readonly string connstirng1 =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private static readonly string connstirng2 =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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

      

        [ClassInitialize()]
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

        [ClassCleanup]
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

        [TestCleanup]
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

        [TestMethod()]
        public void 台灣轉到昆山_會轉入10筆資料並會繁簡轉換()
        {
            // Arrangey
            using (IDbConnection conn = new SqlConnection(connstirng1))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(SQLSyntaxHelper.ReadSQLFile(ccm_Item), null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            var target = new CCMToKSC(connstirng1, connstirng2)
            {
                SourecTable = "ITEM",
                DestinationTable = "ITEM"
            };
            // Act
            var actual_Count = target.Execute();
            // Assert
            var count = 10;
            var expected = "PS-1000简易型分度盘影像筛选机";
            // 判斷筆數是否正確
            Assert.AreEqual(actual_Count, count);
            using (IDbConnection conn = new SqlConnection(connstirng2))
            {
                conn.Open();
                var actual =
                    conn.ExecuteScalar<string>(@"Select ITEM_NM from ITEM Where ITEM_NO ='6SPS010-0001'");
                conn.Close();
                Assert.AreEqual(actual, expected);
            }
        }

        [TestMethod()]
        public void 昆山轉寧波_原有4筆經更新後變10筆原本4筆也更新成跟昆山一致()
        {
            using (IDbConnection conn = new SqlConnection(connstirng2))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    // ksc
                    conn.Execute(SQLSyntaxHelper.ReadSQLFile(ksc_Item), null, tran);
                    // nbg
                    conn.Execute(SQLSyntaxHelper.ReadSQLFile(nbg_Item), null, tran);
                    tran.Commit();
                    conn.Close();
                } catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            // Arrange
            var target = new KSCToSubsidiaries(connstirng1, connstirng2);
            // Act
            target.SourecTable = "ITEM";
            target.DestinationTable = "NBG_ITEM";
            int actual_Count = target.Execute();
            // Assert
            var count = 10;
            var expected = "PSL-1500-C3+PSE/CKS1009";
            // 判斷筆數是否正確
            Assert.AreEqual(actual_Count, count);
            using (IDbConnection conn = new SqlConnection(connstirng2))
            {
                conn.Open();
                var actual =
                    conn.ExecuteScalar<string>(@"Select ITEM_SP from ITEM Where ITEM_NO ='7SPSL15-0001'");
                conn.Close();
                Assert.AreEqual(actual, expected);
            }
        }
    }
}