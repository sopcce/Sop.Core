using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Sop.Data.Repository;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(providers: new[]
            {
                new ConsoleLoggerProvider(
                    (category, level) =>
                    {
                        return category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information;
                    }, true) 
            });
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("mysql");

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder
                                                   .AddConsole()
                                                   .AddFilter(level => level >= LogLevel.Information)
            );
            var loggerFactory = serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSopData<EfDbBaseDbContext>(opt =>
            {
                opt.UseMySql(connectionString);
                // opt.UseLoggerFactory(MyLoggerFactory);
                opt.UseLoggerFactory(loggerFactory);
            });

            //获取当前相关的程序集 
            //services.AddScoped<IUserService, UserService>(); 
            var path = AppDomain.CurrentDomain?.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

        #region Debug Tests

            //var referencedAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            //var types1 = assemblies
            //  .SelectMany(a => a.DefinedTypes)
            //  .Select(type => type.AsType())
            //  .Where(x => x != typeof(IUserAboutService) && typeof(IUserAboutService).IsAssignableFrom(x)).ToArray();
            //var implementTypes1 = types1.Where(x => x.IsClass).ToArray();
            //var interfaceTypes1 = types1.Where(x => x.IsInterface).ToArray(); 

        #endregion


            var files = Directory.EnumerateFiles(path, "Sop.*.dll");
            var assemblies = files.Select(n => Assembly.Load(AssemblyName.GetAssemblyName(n))).ToArray();

            var types = assemblies
                       .SelectMany(a => a.DefinedTypes)
                       .Select(type => type.AsType())
                       .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCultureIgnoreCase) &&
                                   !t.Name.Contains("CacheService")).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, implementType);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }


    public  class EfDbBaseDbContext : BaseDbContext
    {
        /// <inheritdoc />
        public EfDbBaseDbContext(DbContextOptions options) : base(options)
        {
            SetOnModelCreatingType(OnModelCreatingType.UseEntityMap);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}