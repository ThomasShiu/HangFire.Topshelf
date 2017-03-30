using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;

//using System.Messaging;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using Autofac.Integration.WebApi;
using Hangfire.Common;
using Hangfire.Console;
using Hangfire.Dashboard;
using Hangfire.RecurringJobExtensions;
using Hangfire.Samples.Framework;
using Hangfire.Samples.Framework.Logging;
using Hangfire.Server;
using Hangfire.SqlServer;
using Owin;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Hangfire.Topshelf.Core
{
    /// <summary>
    /// Hangfire 自訂擴充集.
    /// </summary>
    public static class HangfireExtensions
    {
        /// <summary>
        /// 管理訊息 (送到那里去)
        /// </summary>
        /// <param name="logger">Looger 物件</param>
        /// <param name="message">訊息內容</param>
        /// <param name="context">PerformContext 物件</param>
        /// <param name="loggerEnabled"> 是否要送到Logger <c>true</c> 送Logger <c> 不送 </c></param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static void Console(this ILog logger, string message, PerformContext context, bool loggerEnabled = true)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message));
            if (context == null) throw new ArgumentNullException(nameof(context));

            context.WriteLine(message);

            if (loggerEnabled) logger.Info(message);
        }

        /// <summary>
        /// 使用Owin 功能 (WebApi Service)
        /// </summary>
        /// <param name="configurator">服務設定物件</param>
        /// <param name="baseAddress">服務地址</param>
        /// <returns>HostConfigurator.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static HostConfigurator UseOwin(this HostConfigurator configurator, string baseAddress)
        {
            if (string.IsNullOrEmpty(baseAddress)) throw new ArgumentNullException(nameof(baseAddress));

            configurator.Service(() => new Bootstrap { Address = baseAddress });

            return configurator;
        }

        /// <summary>
        /// 使用Storage
        /// </summary>
        /// <typeparam name="TStorage">JobStorag 設定物件</typeparam>
        /// <param name="app">要設定對像</param>
        /// <param name="storage">設定物件</param>
        /// <returns>IGlobalConfiguration&lt;TStorage&gt;.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IGlobalConfiguration<TStorage> UseStorage<TStorage>(this IAppBuilder app, TStorage storage) where TStorage : JobStorage
        {
            if (storage == null) throw new ArgumentNullException(nameof(storage));

            return GlobalConfiguration.Configuration.UseStorage(storage);
        }

        /// <summary>
        /// 使用 MSMQ (MessageQueue). (必需要在MSSQL 下才可以使用)
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="pathPattern">MSMQ 路徑</param>
        /// <param name="queues">佇列集合</param>
        /// <returns>IGlobalConfiguration&lt;SqlServerStorage&gt;.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static IGlobalConfiguration<SqlServerStorage> UseMsmq(this IGlobalConfiguration<SqlServerStorage> configuration, string pathPattern, params string[] queues)
        {
            if (string.IsNullOrEmpty(pathPattern)) throw new ArgumentNullException(nameof(pathPattern));
            if (queues == null) throw new ArgumentNullException(nameof(queues));

            foreach (var queueName in queues)
            {
                var path = string.Format(pathPattern, queueName);

                if (!MessageQueue.Exists(path))
                    using (var queue = MessageQueue.Create(path, transactional: true))
                        queue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
            }
            return configuration.UseMsmqQueues(pathPattern, queues);
        }

        /// <summary>
        /// 使用那些量測資訊在看板上
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns>IAppBuilder.</returns>
        /// <remarks>在這個方法中設定了要顯示那些項目，就是一進入後第一頁最上方區塊</remarks>
        public static IAppBuilder UseDashboardMetric(this IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseDashboardMetric(DashboardMetrics.ServerCount)
                .UseDashboardMetric(SqlServerStorage.ActiveConnections)
                .UseDashboardMetric(SqlServerStorage.TotalConnections)
                .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                .UseDashboardMetric(DashboardMetrics.RetriesCount)
                .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                .UseDashboardMetric(DashboardMetrics.SucceededCount)
                .UseDashboardMetric(DashboardMetrics.FailedCount)
                .UseDashboardMetric(DashboardMetrics.DeletedCount);

            return app;
        }

        /// <summary>
        /// Uses the hangfire filters.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>IAppBuilder.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IAppBuilder UseHangfireFilters(this IAppBuilder app, params JobFilterAttribute[] filters)
        {
            if (filters == null) throw new ArgumentNullException(nameof(filters));

            foreach (var filter in filters)
                GlobalConfiguration.Configuration.UseFilter(filter);

            return app;
        }

        /// <summary>
        /// 使用 Autofac. (連同設定)
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="config">HttpConfiguration 物件</param>
        /// <returns>IContainer.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IContainer UseAutofac(this IAppBuilder app, HttpConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var builder = new ContainerBuilder();

            var assembly = typeof(Startup).Assembly;

            builder.RegisterAssemblyModules(assembly);

            builder.RegisterApiControllers(assembly);

            var container = builder.Build();
            // 設定WebAPI 的DI
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            // 設定Hangfire 的DI
            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            return container;
        }

        /// <summary>
        /// 使用 Recurring job. (註冊
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="container">The container.</param>
        /// <returns>IGlobalConfiguration.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IGlobalConfiguration UseRecurringJob(this IGlobalConfiguration configuration, IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            var interfaceTypes = container.ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(IDependency)))
                .Select(x => x.Activator)
                .OfType<ReflectionActivator>()
                .Select(x => x.LimitType.GetInterface($"I{x.LimitType.Name}"));

            return GlobalConfiguration.Configuration.UseRecurringJob(() => interfaceTypes);
        }

        /// <summary>
        /// 使用DI中的Job來註冊Recurring Job
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="container">DI 容器</param>
        /// <returns>IAppBuilder.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IAppBuilder UseRecurringJob(this IAppBuilder app, IContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            GlobalConfiguration.Configuration.UseRecurringJob(container);

            return app;
        }

        /// <summary>
        /// 使用類別來註冊 Recurring Job
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="types">The types.</param>
        /// <returns>IAppBuilder.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IAppBuilder UseRecurringJob(this IAppBuilder app, params Type[] types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            GlobalConfiguration.Configuration.UseRecurringJob(types);

            return app;
        }

        /// <summary>
        /// Uses the recurring job.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="typesProvider">The types provider.</param>
        /// <returns>IAppBuilder.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IAppBuilder UseRecurringJob(this IAppBuilder app, Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            GlobalConfiguration.Configuration.UseRecurringJob(typesProvider);

            return app;
        }

        /// <summary>
        /// 使用參數檔來設定 Recurring job.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="jsonFile">The json file.</param>
        /// <returns>IAppBuilder.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static IAppBuilder UseRecurringJob(this IAppBuilder app, string jsonFile)
        {
            if (string.IsNullOrEmpty(jsonFile)) throw new ArgumentNullException(nameof(jsonFile));

            GlobalConfiguration.Configuration.UseRecurringJob(jsonFile);

            return app;
        }
    }
}