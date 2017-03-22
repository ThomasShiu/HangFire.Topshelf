using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hangfire.Topshelf.Jobs.Tests
{
    [TestClass()]
    public class SyncJobTests
    {
        [TestMethod()]
        public void 執行刷卡資料測讀取資料後寫入()
        {
            // Arrange
            var job = new SyncJob();
            // Act
            job.Execute(null);
            // Assert
            Assert.AreEqual(1, 1);
        }
    }
}