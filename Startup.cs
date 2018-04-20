using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using project;
using project_vega.Controllers;
using project_vega.Core;
using project_vega.Core.Models;
using project_vega.Persistence;

namespace project_vega
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //point to the appsettings.json
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container. Dependency injection!
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //Inject DbContext for using EF
            //setup connectionString in appsettings.json file
            //default lifetime is scoped
            services.AddDbContext<VegaDbContext>(options => { options.UseSqlServer(Configuration["ConnectionString:Default"]); });

            //inject automapper
            services.AddAutoMapper();

            //inject VehicleRepository and UnitOfWork
            //Transient: A separate instance of repository for every use
            //Singleton: A single instance of repository during application lifecycle
            //Scoped: A single instance of repository for each request (EF initiate DbContext as Scoped)
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IPhotoStorage, FileSystemPhotoStorage>();

            //inject photosetting class. Configuration in appsetting.json
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));

            #region Authentication Services
            //on client side, should use AuthHttp instead of Http in Creat(), Delete() and Update() method.
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(options =>
                {
                    options.Authority = "https://foxulee.auth0.com/";
                    options.Audience = "https://api.vega.com";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.RequireAdminRole, policy => policy.RequireClaim("https://vega.com/roles", "Admin"));
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline in the middleware.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //add middlerwares
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //only work on client side changes, not on server side
                //to watch server side, need to install dotnet.watcher.tools, add referrence in project-vega.csproj file, run command: "dotnet watch run" to start
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    //any changes in the code will automatically refresh browser                
                    HotModuleReplacement = true,

                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();


            #region Enable authentication middleware
            app.UseAuthentication();

            #endregion



            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
