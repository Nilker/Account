using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cig.YanFa.Account.Web.Core.StartupTask
{
    /// <summary>
    /// 启动事件
    /// </summary>
    public interface IMyStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
