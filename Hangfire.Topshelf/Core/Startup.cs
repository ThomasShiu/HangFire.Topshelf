using System;
using System.Web.Http;
using Hangfire.Dashboard;
using Hangfire.Console;
using Hangfire.Topshelf.Jobs;
using Owin;
using Swashbuckle.Application;

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

            // 產生序列集項目 在MSMQ 中會變作 hangfire-XXXX
            var queues = new[] { "default", "apis", "jobs" };

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

            //global hangfire filters
            app.UseHangfireFilters(new AutomaticRetryAttribute { Attempts = 0 });

			app.UseHangfireServer(new BackgroundJobServerOptions
			{
				//wait all jobs performed when BackgroundJobServer shutdown.
				ShutdownTimeout = TimeSpan.FromMinutes(30),
				Queues = queues,
                //該Server 最大Job 執行數量
				WorkerCount = Math.Max(Environment.ProcessorCount, 20)
			});

			var options = new DashboardOptions
			{
				AppPath = HangfireSettings.Instance.AppWebSite,
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
			};
			app.UseHangfireDashboard("", options);

			app.UseDashboardMetric();

			app.UseRecurringJob(typeof(RecurringJobService));

			app.UseRecurringJob(container);

			app.UseRecurringJob("recurringjob.json");
		}

	}
}
