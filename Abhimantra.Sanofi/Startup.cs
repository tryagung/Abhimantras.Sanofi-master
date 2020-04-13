using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Abhimantra.Sanofi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sanofi.Core;
using Sanofi.Core.EntitiesModel.IdentityCore;
using Sanofi.Infrastructure.DbContext;

namespace Abhimantra.Sanofi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            environment = env;
        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 100 * 1024 * 1024;

            });
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new AuthorizationPageRequirement())
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
                o.Filters.Add(typeof(MyActionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider()//Prevent request too long
                .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                //options.LogoutPath = new PathString("/[your-path]");
            });

            var host = Configuration["DocumentManagementSettings:Host"];
            var stringServerContextLogin = Configuration["DocumentManagementSettings:ServerContextLogin"];

            var serverContextLogin = stringServerContextLogin.Split(";");

            if (serverContextLogin.Length < 2)
                throw new Exception("Destination Server must be set first");

            //set global variable param model
            var globalVariable = new GlobalVariableParamModel()
            {
                ApplicationDomain = Configuration["GlobalVariable:ApplicationDomain"],
                ApplicationName = Configuration["GlobalVariable:ApplicationName"],
                ConnectionString = Configuration["ConnectionStrings:DefaultConnection"],
                ContentRootPath = environment.ContentRootPath,
                ReportBaseUrl = Configuration["GlobalVariable:ReportDomain"],
                EmailNotificationUsername = Configuration["GlobalVariable:EmailNotificationUsername"],
                EmailNotificationPassword = Configuration["GlobalVariable:EmailNotificationPassword"],
                EmailNotificationHost = Configuration["GlobalVariable:EmailNotificationHost"],
                EmailNotificationPort = Convert.ToInt16(Configuration["GlobalVariable:EmailNotificationPort"]),
                SensenetBaseUrl = Configuration["GlobalVariable:SensenetBaseUrl"],
                EnvironmentVariable = !string.IsNullOrEmpty(Configuration["GlobalVariable:EnvironmentVariable"]) ? Configuration["GlobalVariable:EnvironmentVariable"] : "Development"
            };
            services.AddSingleton(globalVariable);

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(AppSettingsJson.GetConnectionString()));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:PetroseaConnection"]));

            //Get from APPSETTING.JSON connectionString
            //add identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //services.AddKendo();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient<IAuthorizationHandler, AuthorizationPageHandler>();

            services.AddScoped<IPrincipal>(sp => sp.GetService<IHttpContextAccessor>().HttpContext.User);


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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
