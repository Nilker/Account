using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Cig.YanFa.Account.Repository.Repository.Interface;
using Cig.YanFa.Account.Web.Core.Common;
using Cig.YanFa.Account.Web.Core.Common.Aop;
using Cig.YanFa.Account.Web.Core.Common.AutoRegisterDi;
using Cig.YanFa.Account.Web.Core.Common.Email;
using Cig.YanFa.Account.Web.Core.StartupTask;
using Cig.YanFa.Configuration;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace Cig.YanFa.Account.Web.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(s =>
            {
                s.Filters.Add(typeof(GlobalExceptionsFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region 注册DbContext

            //注册邮件配置
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddDapperDBContext<CrmDBContext>(options => { options.Configuration = Configuration["AllConnection:Crm2009"]; });
            services.AddDapperDBContext<AcDBContext>(options => { options.Configuration = Configuration["AllConnection:AC2010"]; });
            services.AddDapperDBContext<BudgetDBContext>(options => { options.Configuration = Configuration["AllConnection:Budget2017"]; });
            services.AddDapperDBContext<SysRightsManagerDBContext>(options => { options.Configuration = Configuration["AllConnection:SysRightsManager"]; });


            //services.AddTransient<ITest2Repository, Test2Repository>();
            services.AddTransient(typeof(IDapperRepositoryBase<,>), typeof(DapperRepositoryBase<,>));


            //DI 注入 DependencyInjection  单个注册
            //services.AddScoped(typeof(IProductService), typeof(ProductService));

            //var assemblyToScan = Assembly.GetExecutingAssembly(); //..or whatever assembly you need
            //var assemblyToScan = Assembly.Load("Cig.YanFa.Account.Application");
            //var assemblyToScanRepository = Assembly.Load("Cig.YanFa.Account.Repository");

            //services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan, assemblyToScanRepository)
            //    .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Repository"))
            //    .AsPublicImplementedInterfaces(); 
            #endregion

            #region 模拟登陆
            //模拟登陆
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.AddScheme<DemoHandle>("cookie", null);
            });
            #endregion

            #region Swagger
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CIG", Version = "v1" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "Cig.YanFa.Account.Web.Core.xml");
                c.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region hangFire
            //hangfire的任务需要数据库持久化
            //Hangfire.AspNetCore
            //Hangfire.MySql.Core  mysql引用 大小写敏感
            //Hangfire.SqlServer   sqlserver引用 大小写敏感

            //hangfire必须需要绑定一个持久化的连接数据。 官方推荐的是sqlserver,还有mg,mssql,pgsql,redis都是个人封装的
            //连接字符串必须加 Allow User Variables=true
            services.AddHangfire(x => x.UseStorage(new SqlServerStorage(
                Configuration["AllConnection:Crm2009"],
                new SqlServerStorageOptions
                {
                    TransactionIsolationLevel = IsolationLevel.ReadCommitted, // 事务隔离级别。默认是读取已提交。
                    QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作业队列轮询间隔。默认值为15秒。
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作业到期检查间隔（管理过期记录）。默认值为1小时。
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合计数器的间隔。默认为5分钟。
                    PrepareSchemaIfNecessary = true,                          //- 如果设置为true，则创建数据库表。默认是true。
                    DashboardJobListLimit = 50000,                            //- 仪表板作业列表限制。默认值为50000。
                    TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超时。默认为1分钟。
                    //TablePrefix = "Hangfire"                                  //- 数据库中表的前缀。默认为none
                    SchemaName = "HangfireAccount"
                }
            )));
            #endregion

            #region AutoMapper
            //注册 autoMapper服务
            //services.AddAutoMapper();
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region 启动时只执行一次
            services.AddStartupTask<MyStartupTask>();//TODO:lhl 启动时执行一次，但成功， 
            #endregion

            #region AutoFac

            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();

            //注册要通过反射创建的组件
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            builder.RegisterType<YanFaLogAop>();//可以直接替换其他拦截器！一定要把拦截器进行注册


            var assemblyToScan = Assembly.Load("Cig.YanFa.Account.Application");
            var assemblyToScanRepository = Assembly.Load("Cig.YanFa.Account.Repository");

            builder.RegisterAssemblyTypes(assemblyToScan, assemblyToScanRepository)
                .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(typeof(YanFaLogAop));//可以直接替换拦截器


            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            #endregion

            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            //中文乱码问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //app.UseCookiePolicy();

            //启用认证
            app.UseAuthentication();

            app.UseHangfireDashboard(); //使用hangfire面板
            var options = new BackgroundJobServerOptions
            {
                Queues = new[] { "account" },
                ServerName = "Account-Service"
            };
            app.UseHangfireServer(options); //启动hangfire服务
            //hangfire di
            GlobalConfiguration.Configuration.UseActivator<HangFireActivator>(new HangFireActivator(serviceProvider));



            app.Map("/login", app2 => app2.Run(async context =>
            {
                var Identity = new ClaimsIdentity();
                Identity.AddClaim(new Claim(ClaimTypes.Name, "假永振"));
                await context.SignInAsync(new ClaimsPrincipal(Identity));
                context.Response.Redirect("/");
            }));

            //app.Run(context =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";
            //    return context.Response.WriteAsync(
            //            context.User?.Identity?.IsAuthenticated ?? false ? context.User.Identity.Name : "没有登陆");
            //});


            //loggerFactory.AddLog4Net(); // << Add this line  转移到Program 下了，添加过滤

            //启用中间件服务生成Swagger作为JSON终结点
#if DEBUG
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
#endif
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
