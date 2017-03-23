using Hangfire.Samples.Framework.Logging;

namespace Hangfire.Samples.Framework
{
    /// <summary>
    /// 應用程式服務基礎介面
    /// </summary>
    public interface IAppService : IDependency
    {
        /// <summary>
        /// 記錄器
        /// </summary>
        /// <value>The logger.</value>
        ILog Logger { get; set; }
    }
}