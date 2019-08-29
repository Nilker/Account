using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Repository.ContentOptions;
using Microsoft.Extensions.DependencyInjection;

namespace Cig.YanFa.Account.Repository
{
    public static class DapperDBContextServiceCollectionExtensions
    {
        public static IServiceCollection AddDapperDBContext<T>(this IServiceCollection services, Action<DapperDBContextOptions> setupAction) where T : DapperDBContext
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
            if (typeof(T).Name.ToLower() == "CrmDBContext".ToLower())
            {
                services.Configure<CRMContextOptions>(setupAction);
            }
            if (typeof(T).Name.ToLower() == "AcDBContext".ToLower())
            {
                services.Configure<AcContextOptions>(setupAction);
            }
            if (typeof(T).Name.ToLower() == "BudgetDBContext".ToLower())
            {
                services.Configure<BudgetContextOptions>(setupAction);
            }
            if (typeof(T).Name.ToLower()== "SysRightsManagerDBContext".ToLower())
            {
                services.Configure<SysRightsManagerContextOptions>(setupAction);
            }

            services.AddScoped<DapperDBContext, T>();
            services.AddScoped<T>();
            //services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddScoped(typeof(IUnitOfWorkFactory<>), typeof(UnitOfWorkFactory<>));
            return services;
        }
    }
}
