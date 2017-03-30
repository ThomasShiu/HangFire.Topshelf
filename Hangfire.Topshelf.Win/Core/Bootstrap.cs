using System;
using Hangfire.Samples.Framework.Logging;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace Hangfire.Topshelf.Core
{
    /// <summary>
    /// OWIN host
    /// </summary>
    public class Bootstrap : ServiceControl
    {
        private static readonly ILog _logger = LogProvider.For<Bootstrap>();

        private IDisposable webApp;

        /// <summary>
        /// 主機地址
        /// </summary>
        /// <value>主機地址字串</value>
        public string Address { get; set; }

        /// <summary>
        /// 啟動主機服務
        /// </summary>
        /// <param name="hostControl">主機控制項</param>
        /// <returns>是否成功啟動 <c>True</c> 啟動成功  <c>發生錯誤未成功啟動</c></returns>
        public bool Start(HostControl hostControl)
        {
            try
            {
                webApp = WebApp.Start<Startup>(Address);
                return true;
            } catch (Exception ex)
            {
                _logger.ErrorException("Topshelf 啟動時發生錯誤", ex);
                return false;
            }
        }

        /// <summary>
        /// 停止主機服務
        /// </summary>
        /// <param name="hostControl">主機控制項</param>
        /// <returns>是否成功停止 <c>True</c> 停止成功  <c>發生錯誤未成功停止</c>  </returns>
        public bool Stop(HostControl hostControl)
        {
            try
            {
                webApp?.Dispose();
                return true;
            } catch (Exception ex)
            {
                _logger.ErrorException("Topshelf 停止時生錯誤", ex);
                return false;
            }
        }
    }
}