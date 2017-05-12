using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Topshelf.Jobs
{
    interface IConfirmAction
    {
        void Confirm();
        void UnConfirm();
    }
}
