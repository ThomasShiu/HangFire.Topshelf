using Hangfire.Samples.Framework.Logging;

namespace Hangfire.Samples.Framework
{
    /// <summary>
    /// 應用程式服務基礎類別
    /// </summary>
    public abstract class BaseAppService : IAppService
    {
        public virtual ILog Logger { get; set; } = new NullLogger();
    }
}
