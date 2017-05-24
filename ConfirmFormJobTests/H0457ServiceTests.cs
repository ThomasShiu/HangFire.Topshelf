﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hangfire.Topshelf.Jobs.Tests
{
  [TestClass()]
  public class H0457ServiceTests
  {
    public delegate void Callback(string line);

    private static readonly string connstirng =
            @"Data Source=192.168.100.11;Initial Catalog=HRSDBR53_BAK;Integrated Security=False;User ID=sa;Password=6937937;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    [TestMethod()]
    [TestCategory("人事單據")]
    public void 薪資異動單確認測試()
    {
      // Arrange
      var service = new H0457Service();
      // Act
      service.Confirm(WriteLine, connstirng, new DateTime(2017, 3, 30));
      // Assert
      Assert.AreEqual(true, true);
    }

    public void WriteLine(string line)
    {
    }
  }
}