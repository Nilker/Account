using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cig.YanFa.Account.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Cig.YanFa.Account.Web.Core.StartupTask
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IMyStartupTask
            => services.AddTransient<IMyStartupTask, T>();
    }

    public static class StartupTaskWebHostExtensions
    {
        public static async Task RunWithTasksAsync(this IWebHost webHost, CancellationToken cancellationToken = default)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var logger = webHost.Services.GetRequiredService<ILogger<Program>>();
                var job = scope.ServiceProvider.GetService<IMyStartupTask>();
                logger.LogInformation("启动时---》只执行一次任务，执行了");

                await job.ExecuteAsync(cancellationToken);
            }

            await webHost.RunAsync(cancellationToken);
        }
    }
}
