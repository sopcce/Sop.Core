using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.StartupConfig;
using WebApi.StartupConfig.Swagger;

namespace WebApi
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
            services.AddControllers();

            services.AddOptions();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("mysql");

            //IServiceCollection serviceCollection = new ServiceCollection();
            //serviceCollection.AddLogging(builder => builder
            //                                       .AddConsole()
            //                                       .AddFilter(level => level >= LogLevel.Information)
            //);
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

        #region Dependency injection

            services.AddSopData<EfDbBaseDbContext>(opt =>
            {
                opt.UseMySql(connectionString);
                // opt.UseLoggerFactory(MyLoggerFactory);
                opt.UseLoggerFactory(loggerFactory);
            });

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

        #endregion


        #region Dynamic Controller Routing

            //services.AddSingleton<TranslationTransformer>();
            //services.AddSingleton<TranslationDatabase>(); 

        #endregion


        #region Jwt

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                     {
                         x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                         x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                     })
                    .AddJwtBearer(x =>
                     {
                         x.RequireHttpsMetadata = false;
                         x.SaveToken = true;
                         x.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(key),
                             ValidateIssuer = false,
                             ValidateAudience = false
                         };
                     });

        #endregion

        #region SwaggerGen

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                             new OpenApiInfo
                             {
                                 Title = "Test API V1",
                                 Version = "v1",
                                 Description = "A sample API for testing Swashbuckle",
                                 TermsOfService = new Uri("http://tempuri.org/terms")
                             }
                );

                c.OperationFilter<AssignOperationVendorExtensions>();

                c.SchemaFilter<ExamplesSchemaFilter>();

                c.DescribeAllParametersInCamelCase();

                c.GeneratePolymorphicSchemas();
                var path = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");

                c.IncludeXmlComments(path);

                //c.EnableAnnotations();
                //c.AddSecurityRequirement();

                //c.AddSecurityDefinition("HuangLiAPP.Web", new ApiKeyScheme
                //{
                //    Description = @"JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������ð�ź���ַ�����Ϣ��token:token�ַ��� ",

                //    Name = "token",//jwtĬ�ϵĲ�������
                //    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                //    Type = "apiKey"
                //});
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2",
                             new OpenApiInfo
                             {
                                 Title = "Test API V1",
                                 Version = "v1",
                                 Description = "A sample API for testing Swashbuckle",
                                 TermsOfService = new Uri("http://tempuri.org/terms")
                             }
                );

                c.OperationFilter<AssignOperationVendorExtensions>();

                c.SchemaFilter<ExamplesSchemaFilter>();

                c.DescribeAllParametersInCamelCase();

                c.GeneratePolymorphicSchemas();
                var path = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");

                c.IncludeXmlComments(path);

                //c.EnableAnnotations();
                //c.AddSecurityRequirement();

                //c.AddSecurityDefinition("HuangLiAPP.Web", new ApiKeyScheme
                //{
                //    Description = @"JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������ð�ź���ַ�����Ϣ��token:token�ַ��� ",

                //    Name = "token",//jwtĬ�ϵĲ�������
                //    In = "header",//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                //    Type = "apiKey"
                //});
            });
            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in - needs to be placed after AddSwaggerGen()

        #endregion
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            // global cors policy
            app.UseCors(x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();


            #region Dynamic Controller Routing

                //endpoints.MapDynamicControllerRoute<TranslationTransformer>("{language}/{controller}/{action}"); 

            #endregion

                //endpoints.MapHub<ChatHub>("/chat");
                //endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });


        #region Swagger

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer {Url = $"{httpReq.Scheme}://{httpReq.Host.Value}"}
                    };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                ; // serve the UI at root
                c.SwaggerEndpoint("swagger/v1/swagger.json", "V1 Docs");
                c.SwaggerEndpoint("swagger/v2/swagger.json", "V2 Docs");
            });

        #endregion
        }
    }
}