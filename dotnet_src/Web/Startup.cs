using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.StartupConfig;

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
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();


            services.AddOptions();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("mysql"); 

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                   .AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                   .AddConsole()
                   .AddEventLog();
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Example log message");


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

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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


            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}