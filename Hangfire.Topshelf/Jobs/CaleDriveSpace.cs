using System.IO;
using System.Linq;
using Hangfire.RecurringJobExtensions;
using Hangfire.Server;

namespace Hangfire.Topshelf.Jobs
{
    internal class CaleDriveSpace : ICalcSpace
    {
        public long Calculate(string target, string searchPattern)
        {
            var drive = DriveInfo.GetDrives()
                                 .FirstOrDefault(d => d.Name == target);
            var useSpace = (drive?.TotalSize ?? 0) - (drive?.TotalFreeSpace ?? 0);
            return useSpace;
        }

        public string GetTarget(PerformContext context)
        {
            return context.GetJobData<string>("Drive");
        }

        public bool TargetExists(string target)
        {
            return DriveInfo.GetDrives().Count(d => d.Name == target) == 1;
        }
    }
}