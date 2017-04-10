using Hangfire.Samples.Framework;
using Hangfire.Samples.Framework.Logging;

namespace Hangfire.Topshelf.Jobs
{
    public class RunPowerShellService
    {
        /// <summary>
        /// 記錄器
        /// </summary>
        /// <value>The logger.</value>
        public virtual ILog Logger { get; set; } = new NullLogger();

        public int Execute()
        {
            return 0;
        }
    }
}
