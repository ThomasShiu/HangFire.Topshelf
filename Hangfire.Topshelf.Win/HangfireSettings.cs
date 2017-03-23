using System;
using Microsoft.Extensions.Configuration;

namespace Hangfire.Topshelf
{
    /// <summary>
    /// 全域參數DTO
    /// </summary>
    public class HangfireSettings
    {
        private static readonly Lazy<HangfireSettings> _instance = new Lazy<HangfireSettings>(() => new HangfireSettings());

        /// <summary>
        /// 取得HangfireSettings 實體
        /// </summary>
        /// <value>The instance.</value>
        public static HangfireSettings Instance => _instance.Value;

        /// <summary>
        /// 取得參數物件
        /// </summary>
        /// <value>The configuration.</value>
        public IConfigurationRoot Configuration { get; }

        private HangfireSettings()
        {
            // 取得應用程式參數檔
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        /// <summary>
        /// Windows 服務名稱
        /// </summary>
        public string ServiceName => Configuration["hangfire.server.serviceName"];

        /// <summary>
        /// Windows 服務顯示名稱
        /// </summary>
        public string ServiceDisplayName => Configuration["hangfire.server.serviceDisplayName"];

        /// <summary>
        /// Windows 服務描述
        /// </summary>
        public string ServiceDescription => Configuration["hangfire.server.serviceDescription"];

        /// <summary>
        /// Windows 服務位址
        /// </summary>
        public string ServiceAddress => Configuration["hangfire.server.serviceAddress"];

        /// <summary>
        /// App Web Site 位址
        /// </summary>
        public string AppWebSite => Configuration["hangfire.server.website"];

        /// <summary>
        /// 登入使用者帳號
        /// </summary>
        public string LoginUser => Configuration["hangfire.login.user"];

        /// <summary>
        /// 登入使用者密碼
        /// </summary>
        public string LoginPwd => Configuration["hangfire.login.pwd"];

        /// <summary>
        /// 記錄檔路徑位置
        /// </summary>
        public string LogFilePath => Configuration["hangfire.logfile.path"];

        /// <summary>
        /// MSSQL連接字串
        /// </summary>
        public string HangfireSqlserverConnectionString => Configuration.GetConnectionString("hangfire.sqlserver");

        /// <summary>
        ///  MSMQ 序列服務器連接字串
        /// </summary>
        public string HangfireMSMQConnectionString => Configuration["hangfire.MSMQ"];

        /// <summary>
        ///  Redis server連接字串
        /// </summary>
        public string HangfireRedisConnectionString => Configuration.GetConnectionString("hangfire.redis");
    }

    // TODO levi 170318 可以加入使用SQLite , 記憶體方式來作為Storage
}