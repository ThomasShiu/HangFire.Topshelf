using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Core;
using Hangfire.Samples.Framework;
using Hangfire.Samples.Framework.Logging;
using Hangfire.Topshelf.Jobs;

namespace Hangfire.Topshelf.Core
{
    /// <summary>
    /// Hangfire Autofac Module
    /// </summary>
    public class HangfireModule : Autofac.Module
    {
        /// <summary>
        /// Override to attach module-specific functionality to a
        /// component registration.
        /// 附加到组件注册
        /// </summary>
        /// <param name="componentRegistry">The component registry.</param>
        /// <param name="registration">The registration to attach functionality to.</param>
        /// <remarks>This method will be called for all existing <i>and future</i> component
        /// registrations - ordering is not important.</remarks>
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
                                                              IComponentRegistration registration)
        {
            base.AttachToComponentRegistration(componentRegistry, registration);

            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }

        private void InjectLoggerProperties(object instance)
        { 
            // 注入記錄器屬性
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType
              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(p => p.PropertyType == typeof(ILog) && p.CanWrite && p.GetIndexParameters().Length == 0);

            // Set the properties located.
            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, LogProvider.GetLogger(instanceType), null);
            }
        }

        /// <summary>
        /// 事件 在組件準備時
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PreparingEventArgs"/> instance containing the event data.</param>
        private void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(new[]
                 {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(ILog),
                        (p, i) => LogProvider.GetLogger(p.Member.DeclaringType)
                    ),
                 });
        }

        /// <summary>
        /// Auto register
        /// </summary>
        /// <param name="builder"></param>
        /// <remarks>主要處理註冊所有的類別</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            LoadPlugIn(builder);

            //register 註冊所實作的介面
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => typeof(IDependency).IsAssignableFrom(t)
                       && t != typeof(IDependency) && !t.IsInterface)
                .AsImplementedInterfaces();

            //register 註冊指定的類別
            RegisterspecifiedType(builder);
        }

        private void RegisterspecifiedType(ContainerBuilder builder)
        {
            // 註冊 Service類
            // builder.Register(x => new RecurringJobService() { });
            // 註冊 特定類別
            // builder.Register(x => new MyJob1());
             builder.Register(x => new MyJob2());
            // builder.Register(x => new LongRunningJob());
            builder.Register(x => new CalcSpaceJob());
        }

        /// <summary>
        /// 載入外部Job 集
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void LoadPlugIn(ContainerBuilder builder)
        {
            string[] assemblyScanerPattern = new[] { @"CCM.Hangfire.Jobs.*.dll" };

            // Make sure process paths are sane...
            var plugInFolder = AppDomain.CurrentDomain.BaseDirectory;
            // Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jobs");
            Directory.SetCurrentDirectory(plugInFolder);

            // 1. Scan for assemblies containing autofac modules in the bin folder
            List<Assembly> assemblies = new List<Assembly>();
            assemblies.AddRange(
                Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.AllDirectories)
                         .Where(filename => assemblyScanerPattern.Any(pattern => Regex.IsMatch(filename, pattern)))
                         .Select(Assembly.LoadFrom)
                );

            foreach (var assembly in assemblies)
            {
                builder.RegisterAssemblyTypes(assembly)
                    .AsSelf();
            }
        }
    }
}