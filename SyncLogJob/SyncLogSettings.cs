using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Hangfire.Topshelf.Jobs
{
    /// <summary>
    /// 全域參數DTO
    /// </summary>
    public class SyncLogSettings
    {

        private static readonly Lazy<SyncLogSettings> _instance = new Lazy<SyncLogSettings>(() => new SyncLogSettings());

        /// <summary>
        /// 取得HangfireSettings 實體
        /// </summary>
        /// <value>The instance.</value>
        public static SyncLogSettings Instance => _instance.Value;

        /// <summary>
        /// 取得參數物件
        /// </summary>
        /// <value>The configuration.</value>
        public IConfigurationRoot Configuration { get; }

        private SyncLogSettings()
        {
            // 取得應用程式參數檔
            var builder = new ConfigurationBuilder()
                .AddJsonFile("SynclogSettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        /// <summary>
        /// MSSQL連接字串
        /// </summary>
        public string ConnectionString => Configuration.GetConnectionString("hangfire.sqlserver");
        /// <summary>
        /// 資料庫連接字串
        /// </summary>
        /// <value>主要連接到資料庫所使用的字串</value>        
        public string DataBaseConnectString => Configuration.GetConnectionString("HRDBR");
        
        /// <summary>
        /// HAMS 當年度出勤資料的資料庫
        /// </summary>
        /// <value>The hamsdb.</value>
        public string HAMSDB => Configuration.GetConnectionString("HAMS.ThisYearDB");

        /// <summary>
        /// HAMS 基本資料 的資料庫
        /// </summary>
        /// <value>The hams base database.</value>
        public string HAMSBaseDB => Configuration.GetConnectionString("HAMS.DB");

    }
}
