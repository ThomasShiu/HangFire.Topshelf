using System.IO;
using System.Linq;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    internal class CaleFileSpace : ICalcSpace
    {
        public long Calculate(string target, string searchPattern)
        {
            var dir = new DirectoryInfo(Path.GetDirectoryName(target));
            var sp = Path.GetFileName(target);
            if (sp == string.Empty)
                sp = searchPattern;
            return dir.GetFiles(sp, SearchOption.AllDirectories)
                      .Sum(x => x.Length);
        }

        public string GetTarget(PerformContext context)
        {
            return context.GetJobData<string>("FileName");
        }

        public bool TargetExists(string target)
        {
            var path = Path.GetDirectoryName(target);
            var searchPattenrn = Path.GetFileName(target);
            return Directory.GetFiles(path, searchPattenrn, SearchOption.AllDirectories)
                             .Any();
        }
    }
}