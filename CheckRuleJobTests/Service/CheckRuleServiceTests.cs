using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hangfire.Topshelf.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Topshelf.Jobs.Tests
{
   
    
    [TestClass()]
    public class CheckRuleServiceTests
    {
        internal void Message(string line)   { }
        private static readonly string connstirng =
             @"Data Source=192.168.100.11;Initial Catalog=SysManR8;Integrated Security=False;User ID=sa;Password=6937937;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [TestMethod()]
        public void CheckRuleServiceTest()
        {
            // Arrange
            var jp = new JobParas()
            {
                SMTPIP = "192.168.100.103",
                SMTPPort ="25",
                MCMail = "ErpSys@ccm3s.com",
                MCName = "Mail Center 代理人",
                MCQTime ="10000",
                tryNumber =3
            };

            var service = new CheckRuleService(connstirng, jp)
            {
                triggerMapDataValueGID = "DAY020"
            };
            // Act
            var actual = service.Execute(Message);
            // Assert
            var expected = 24;
            Assert.AreEqual(actual, expected);
        }
    }
}