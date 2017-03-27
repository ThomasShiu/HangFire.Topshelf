using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hangfire.Topshelf.Jobs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Hangfire.Topshelf.Jobs.Tests
{
    public class VCHCONFG
    {
        public string VCH_TY { get; set; }
        public string VCH_NM { get; set; }
        public DateTime CLS_DT { get; set; }
    }


    [TestClass()]
    public class SetVchCfgServiceTests
    {
        #region SQL字串
        private static readonly string createTable =
            @"
            CREATE TABLE [dbo].[VCHCONFG] (
                    [VCH_TY]     NCHAR (4)      NOT NULL,
                    [VCH_NM]     NCHAR (20)     NULL,
                    [CLS_DT]     SMALLDATETIME  NULL
                );
            ";
        private static readonly string  dropTable = @"
            IF EXISTS( SELECT * FROM sys.tables WHERE name = 'VCHCONFG' )
            Drop Table VCHCONFG;
            ";
        private static readonly string datas =
            @"
            INSERT INTO VCHCONFG (VCH_TY,VCH_NM,CLS_DT) VALUES('M091','組立領料單','2017/02/28');
            INSERT INTO VCHCONFG (VCH_TY,VCH_NM,CLS_DT) VALUES('M092','加工領料單','2017/02/28');
            INSERT INTO VCHCONFG (VCH_TY,VCH_NM,CLS_DT) VALUES('M093','挀動盤領料單','2017/02/28');
            ";
        private static readonly string connstirng =
             @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        #endregion
        // 執行該類別中第一項測試前，使用 ClassInitialize 執行程式碼
        [ClassInitialize()]
        public static void Initialize(TestContext testContext)
        {
            using (IDbConnection conn = new SqlConnection(connstirng))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(dropTable, null, tran);
                    conn.Execute(createTable,null,tran);
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
        [ClassCleanup]
        public static void Cleanup()
        {
            using (IDbConnection conn = new SqlConnection(connstirng))
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    conn.Execute(dropTable,null,tran);
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
        public void 設定單據性質欄位_將關帳日期設到1231_會更新3筆並且所有關帳日都會變成1231()
        {
            // Arrange
            var target = new SetVchCfgService(connstirng);
            // Act
            target.SetItem("CLS_DT", "2017/12/31", "M091");
            target.SetItem("CLS_DT", "2017/12/31", "M092");
            target.SetItem("CLS_DT", "2017/12/31", "M093");
            var actual = target.Execute();
            // Assert
            Assert.AreEqual(actual, 3);
            using (IDbConnection conn = new SqlConnection(connstirng))
            {
                conn.Open();
                try
                {
                    var qry = conn.Query<VCHCONFG>("Select * from VCHCONFG");
                    foreach (var vch in qry)
                    {
                        Assert.AreEqual(DateTime.Parse("2017/12/31"),vch.CLS_DT,$"{vch.VCH_TY}日期未正常更新");
                    }   
                 conn.Close();  
                } catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}