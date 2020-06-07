using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //Adding Identity
            services.AddIdentity<StoreUser, IdentityRole>(cfg => {
                cfg.User.RequireUniqueEmail = true;
                //two users can have same email
            })
               .AddEntityFrameworkStores<DutchContext>();
              //wher we get the data from

           services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer( cfg => 
                {
                   cfg.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidIssuer=_config["Tokens:Issuer"],
                       ValidAudience = _config["Tokens:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]))

                   };
               
               }
               
               );

            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IMailService, NullMailService>();
            
            services.AddDbContext<DutchContext>(cfg=> { 
            cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
           
           
            
            
            //Add support for real mail service
            services.AddControllersWithViews();
            
            services.AddTransient<DutchSeeder>();
            
            services.AddScoped<IDutchRepository, DutchRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        [Obsolete]
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
            }
            
            //usesdefaultfiles() function to go into wwwroot folder to run some statuc files which starts with index.html or index.htm,etc
           app.UseDefaultFiles();

           // app.UsePathBase("/app");
            //app.UsePathBase(configuration["pathBase"]);
            app.UseStaticFiles();
            
            app.UseNodeModules();

            app.UseAuthentication();//have identifying the user
            // What they 

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(cfg=> {
                cfg.MapControllerRoute("Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" }
                    );
            });
        }
    }
}
