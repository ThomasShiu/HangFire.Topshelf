using System;

namespace Hangfire.Topshelf.Jobs
{
   /// <summary>
   /// 全域參數DTO
   /// </summary>
    public class WaterNumberSettings
    {
        private static readonly Lazy<WaterNumberSettings> _instance = new Lazy<WaterNumberSettings>(() => new WaterNumberSettings());

        /// <summary>
        /// 取得HangfireSettings 實體
        /// </summary>
        /// <value>The instance.</value>
        public static WaterNumberSettings Instance => _instance.Value;

        /// <summary>
        /// 取得參數物件
        /// </summary>
        /// <value>The configuration.</value>
        public WaterNumberSettings Configuration { get; }

        private WaterNumberSettings()
        {
            //// 取得應用程式參數檔
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("SynclogSettings.json", optional: true, reloadOnChange: true);

            //  Configuration = builder.Build();
        }

        /// <summary>
        /// 資料庫連接字串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}