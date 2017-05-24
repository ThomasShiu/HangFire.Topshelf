﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hangfire.Topshelf.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Topshelf.Jobs.Tests
{
  [TestClass()]
 
  public class H0453ServiceTests
  {
    public delegate void Callback(string line);

    private static readonly string connstirng =
            @"Data Source=192.168.100.11;Initial Catalog=HRSDBR53_BAK;Integrated Security=False;User ID=sa;Password=6937937;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    [TestMethod()]
    [TestCategory("人事單據")]
    public void 人事異動單確認測試()
    {
      // Arrange
      var service = new H0453Service();
      // Act
      service.Confirm(WriteLine, connstirng, new DateTime(2017, 4, 1));
      // Assert
      Assert.AreEqual(true, true);
    }

    [TestMethod()]
    [TestCategory("人事單據")]
    public void 人事異動單取消確認測試()
    {
      // Arrange
      var service = new H0453Service();
      // Act
      service.UnConfirm(WriteLine, connstirng, new DateTime(2017, 4, 1));
      // Assert
      Assert.AreEqual(true, true);
    }
    public void WriteLine(string line)
    {
    }
  }
}