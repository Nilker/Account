using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cig.YanFa.Account.Repository
{
    public static class DapperSysRightsManagerContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDapperSysRightsManagerContext<T>(this IServiceCollection services, Action<DapperDBContextOptions> setupAction) where T : DapperSysRightsManagerContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            services.AddOptions();
            services.Configure(setupAction);
            services.AddScoped<DapperSysRightsManagerContext, T>();
            services.AddScoped(typeof(IUnitOfWorkFactory<DapperSysRightsManagerContext>),typeof(DapperUnitOfWorkFactory<DapperSysRightsManagerContext>));// <IUnitOfWorkFactory, DapperUnitOfWorkFactory<>>();
            return services;
        }
    }
}
