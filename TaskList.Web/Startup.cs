using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TaskList.DAL.DbContexts;
using TaskList.DAL.Models;
using TaskList.Web.Infrastructure;
using TaskList.DAL.Repositories;
using TaskList.DAL.Interfaces;
using TaskList.Web.Attribute.RequiresRole;
using System.Net;

namespace TaskList.Web
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AspNetUser, AspNetRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IRoleProvider, RoleProvider>();
            //services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();
            services.AddTransient<IPasswordValidator<AspNetUser>, CustomPasswordValidator>();

            services.AddTransient<IIdentityRepository, IdentityRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IStatusRepository, StatusRepository>();
            services.AddTransient<ITaskListRepository, TaskListRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();

            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            app.UseStatusCodePages(async context => {
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.Unauthorized || response.StatusCode == (int)HttpStatusCode.Forbidden)
                    response.Redirect("/Account/AccessDenied");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area}/{controller=Tasks}/{action=List}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    "pageNotFound",
                    "{*.}",
                     new { controller = "Home", action = "PageNotFound" }
                );
            });
        }
    }
}
