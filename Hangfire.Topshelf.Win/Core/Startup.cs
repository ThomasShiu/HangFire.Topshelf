using System;
using System.IO;
using System.Web.Http;
using Hangfire.Console;
using Hangfire.Dashboard;
using Owin;
using Swashbuckle.Application;
#pragma warning disable 618

namespace Hangfire.Topshelf.Core
{
    /// <summary>
    /// 起始化狀態類別
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 設定應用程式
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            #region Web API Config

            // Configure Web API for self-host.
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "Hangfire Topshelf Apis");
                c.IncludeXmlComments($@"{typeof(Startup).Assembly.GetName().Name}.xml");
            })
            .EnableSwaggerUi();

            var container = app.UseAutofac(config);

            app.UseWebApi(config);

            #endregion Web API Config

            // 產生序列集項目 在MSMQ 中會變作 hangfire-XXXX
            var queues = new[] { "default", "apis", "jobs" };

            #region Storage Options

#if Redis
            app.UseStorage(new Hangfire.Redis.RedisStorage(HangfireSettings.Instance.HangfireRedisConnectionString))
			   .UseConsole();
            //it seems a bug that progress bar in Hangfire.Console cannot dispaly for a long time when using redis storage.
#else
            app.UseStorage(new Hangfire.SqlServer.SqlServerStorage(HangfireSettings.Instance.HangfireSqlserverConnectionString))
              .UseMsmq(HangfireSettings.Instance.HangfireMSMQConnectionString, queues)
              //.UseMsmq(@".\private$\hangfire-{0}", queues)  // 使用本地
              //.UseMsmq(@"FormatName: DIRECT = TCP:192.168.1.1\private$\myqueue"); // 使用遠程IP指定訊息佇列位置"
              .UseConsole();
#endif

            #endregion Storage Options

            //global hangfire filters
            app.UseHangfireFilters(new AutomaticRetryAttribute { Attempts = 0 });

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                // wait all jobs performed when BackgroundJobServer shutdown.
                ShutdownTimeout = TimeSpan.FromMinutes(30),
                // 設定 佇列集
                Queues = queues,
                //該Server 最大Job 執行數量
                WorkerCount = Math.Max(Environment.ProcessorCount, 20)
            });

            #region Dashboard Options

            var options = new DashboardOptions
            {
                AppPath = HangfireSettings.Instance.AppWebSite,
                #region 權限設定
                AuthorizationFilters = new[]
                {
                    new BasicAuthAuthorizationFilter ( new BasicAuthAuthorizationFilterOptions
                    {
                        SslRedirect = false,
                        RequireSsl = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = HangfireSettings.Instance.LoginUser,
								// Password as plain text
								PasswordClear = HangfireSettings.Instance.LoginPwd
                            }
                        }
                    } )
                }
                #endregion
            };
            app.UseHangfireDashboard("", options);
            app.UseDashboardMetric();

            #endregion Dashboard Options

            #region 註冊Job資料

            // app.UseRecurringJob(typeof(RecurringJobService));

            app.UseRecurringJob(container);
            if (File.Exists(HangfireSettings.Instance.RecurringJobFile))
                 app.UseRecurringJob(HangfireSettings.Instance.RecurringJobFile);

            #endregion 註冊Job資料
        }
    }
}