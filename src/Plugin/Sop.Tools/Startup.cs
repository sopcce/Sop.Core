using System.IO;
using System.Security.AccessControl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Sop.Data;
using Sop.Tools.Models;
using Sop.Tools.Services;
using Sop.Tools.Services.Interfaces;

namespace Sop.Tools
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
          
            //
            services.AddSingleton<INavMenuService, NavMenuService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<SopContext>(options =>
             options.UseMySQL(Configuration.GetConnectionString("SopContext")));
            //InitAppConfig(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Production/ Development /
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see 
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            string pathRoot = Path.Combine(Directory.GetCurrentDirectory(), @"Uploads/");
            if (!Directory.Exists(pathRoot))
            {
                Directory.CreateDirectory(pathRoot);
                addpathPower(pathRoot, "ASPNET", "FullControl");
            }

            var options = new FileServerOptions();
            options.FileProvider = new PhysicalFileProvider(pathRoot);
            options.RequestPath = new Microsoft.AspNetCore.Http.PathString("/Uploads");
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.StaticFileOptions.DefaultContentType = "application/x-msdownload";
            options.EnableDirectoryBrowsing = true;
            app.UseFileServer(options);




            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=About}/{id?}");

                routes.MapRoute(
                    name: "default1",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });


        }
        private void InitAppConfig(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Datas/Config/NavBarMenus.json");

            var config = builder.Build();
            //services.Configure<NavBarMenus>(config.GetSection("NavBarMenus"));
        }
        public void addpathPower(string pathname, string username, string power)
        {

            DirectoryInfo dirinfo = new DirectoryInfo(pathname);

            if ((dirinfo.Attributes & FileAttributes.ReadOnly) != 0)
            {
                dirinfo.Attributes = FileAttributes.Normal;
            }

            //取得访问控制列表   
            DirectorySecurity dirsecurity = dirinfo.GetAccessControl();

            switch (power)
            {
                case "FullControl":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
                    break;
                case "ReadOnly":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));
                    break;
                case "Write":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Write, AccessControlType.Allow));
                    break;
                case "Modify":
                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Modify, AccessControlType.Allow));
                    break;
            }
            dirinfo.SetAccessControl(dirsecurity);
        }
    }
}
