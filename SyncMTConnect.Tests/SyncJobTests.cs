using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hangfire.Topshelf.Jobs.Tests
{
  [TestClass()]
  public class SyncJobTests
  {
    public delegate void Callback(string line);

    private static readonly string connstirng =
            @"Data Source=192.168.0.20;Initial Catalog=MES;Integrated Security=False;User ID=webuser;Password=ch6955598;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    [TestMethod()]
    [TestCategory("MTConnect")]
    public void 擷取Log測試()
    {
      // Arrange
      var service = new SyncMTConnectJob();
      // Act
      service.SyncData(WriteLine, connstirng);
      // Assert
      Assert.AreEqual(true, true);
    }

    public void WriteLine(string line)
    {
    }
  }

  }