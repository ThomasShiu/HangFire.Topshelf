using System;
using System.IO;
using Hangfire.Topshelf.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hangfire.Topshelf.tests
{
    [TestClass]
    public class CalcSpaceJobTest
    {
        [TestMethod]
        [TestCategory("CalcSpaceJob")]
        public void 執行計算空間大小_()
        {
            // Substitute mock 部份 在處理PerformContext 類別有問題
            //// Arrange
            //const int prcSize = 10240000;
            //var root = System.AppDomain.CurrentDomain.BaseDirectory;
            //var dir = Directory.CreateDirectory(root + @"\TestX");
            //var rd = new Random();
            //var iCount = rd.Next(1, 10);
            //for (var i = 1; i <= iCount; i++)
            //{
            //    FakeFileUtility.CreateFakeFile(dir.FullName + @"\Tempfile" + i, prcSize);
            //}
            //var context = Substitute.For<PerformContext>();
            //context.GetJobData<string>("searchPattern");
            //context.Received().Wr
            //var job = new CalcSpaceJob();
            //// Act

            //dir.Delete(true);
            //// Assert
        }

        [TestMethod]
        [TestCategory("計算目錄")]
        public void 執行計算目錄大小_以亂數產生檔案數_每個10MB_並計算出該目錄大小()
        {
            // Arrange
            const int prcSize = 10240000;
            var root = System.AppDomain.CurrentDomain.BaseDirectory;
            var dir = Directory.CreateDirectory(root + @"\TestX");
            var rd = new Random();
            var iCount = rd.Next(1, 10);
            for (var i = 1; i <= iCount; i++)
            {
                FakeFileUtility.CreateFakeFile(dir.FullName + @"\Tempfile" + i, prcSize);
            }
            var sfs = new CaleFolderSpace();
            // Act
            var dirSize = sfs.Calculate(dir.FullName, "*.*");

            dir.Delete(true);
            // Assert
            Assert.AreEqual<long>(dirSize, prcSize * iCount);
        }

        [TestMethod]
        [TestCategory("計算目錄")]
        public void 執行計算目錄大小_以亂數產生檔案數_每個10MB_只計算log檔_並計算出該目錄大小()
        {
            // Arrange
            const int prcSize = 10240000;
            var root = System.AppDomain.CurrentDomain.BaseDirectory;
            var dir = Directory.CreateDirectory(root + @"\TestX");
            var rd = new Random();
            var iCount = rd.Next(1, 10);
            for (var i = 1; i <= iCount; i++)
            {
                FakeFileUtility.CreateFakeFile(dir.FullName + @"\Tempfile" + i + ".tmp", prcSize);
            }
            var iCount2 = rd.Next(1, 10);
            for (var i = 1; i <= iCount2; i++)
            {
                FakeFileUtility.CreateFakeFile(dir.FullName + @"\Tempfile" + i + ".log", prcSize);
            }
            var sfs = new CaleFolderSpace();

            // Act
            var dirSize = sfs.Calculate(dir.FullName, "*.log");
            dir.Delete(true);

            // Assert
            Assert.AreEqual<long>(dirSize, prcSize * iCount2);
        }

        [TestMethod]
        [TestCategory("計算檔案")]
        public void 執行計算檔案大小_並計算大小()
        {
            // Arrange
            var rd = new Random();
            var prcSize = rd.Next(1024000, 10240000);
            var root = System.AppDomain.CurrentDomain.BaseDirectory;
            var dir = Directory.CreateDirectory(root + @"\TestX");
            var fileName = dir.FullName + @"\fakeFile.txt";
            FakeFileUtility.CreateFakeFile(fileName, prcSize);
            var sfs = new CaleFileSpace();
            // Act
            var expected = sfs.Calculate(fileName, "*.*");
            dir.Delete(true);
            // Assert
            Assert.AreEqual<long>(expected, prcSize);
        }

        [TestMethod]
        [TestCategory("計算檔案")]
        public void 執行計算檔案大小_無指名特定檔案_以副檔名條件_並計算大小()
        {
            // Arrange
            var rd = new Random();
            var fileCount = rd.Next(1, 10);
            var root = System.AppDomain.CurrentDomain.BaseDirectory;
            var dir = Directory.CreateDirectory(root + @"\TestX");
            var fileNameTemplate = dir.FullName + @"\logt{0}.txt";
            long fileSizeToTotalSpace = 0;
            for (var i = 0; i < fileCount; i++)
            {
                var prcSize = rd.Next(10240, 91260);
                fileSizeToTotalSpace += prcSize;
                FakeFileUtility.CreateFakeFile(
                    string.Format(fileNameTemplate, i), prcSize);
            }
            fileNameTemplate = dir.FullName + @"\logt{0}.tmp";
            for (var i = 0; i < fileCount; i++)
            {
                var prcSize = rd.Next(10240, 91260);
                FakeFileUtility.CreateFakeFile(
                    string.Format(fileNameTemplate, i), prcSize);
            }

            var sfs = new CaleFileSpace();
            // Act
            var expected = sfs.Calculate(dir.FullName + @"\logt*.txt", "logt*.txt");
            dir.Delete(true);
            // Assert
            Assert.AreEqual<long>(expected, fileSizeToTotalSpace);
        }

        [TestMethod]
        [TestCategory("計算磁區")]
        public void 執行計算磁區大小_計算C槽_並計算出大小()
        {
            // 目前這個方法無法實測
            // Arrange
            //var sds = new CaleDriveSpace();
            //var expected = 172037775360;
            //// Act
            //var  actual  = sds.Calculate(@"C:\", "");
            //// Assert
            //Assert.AreEqual<long>(expected, actual);
        }
    }
}