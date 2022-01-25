using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoposphere.Data;
using Shoposphere.Data.Entities;
using Shoposphere.Services.Concrete;
using Shoposphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoposphere.UI
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
            services.AddDbContext<ShoposphereDbContext>(option =>
            {
                option.UseSqlServer("Server=.;Database=Shoposphere;User Id=sa;Password=123;");
            });

            services.AddScoped<IRepository<Category>, EFRepository<Category>>();
            services.AddScoped<IRepository<Comment>, EFRepository<Comment>>();
            services.AddScoped<IRepository<Order>, EFRepository<Order>>();
            services.AddScoped<IRepository<Product>, EFRepository<Product>>();
            services.AddScoped<IRepository<Role>, EFRepository<Role>>();
            services.AddScoped<IRepository<Shipper>, EFRepository<Shipper>>();
            services.AddScoped<IRepository<Supplier>, EFRepository<Supplier>>();
            services.AddScoped<IRepository<User>, EFRepository<User>>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(option =>
               {
                   option.LoginPath = "/auth/login";
                   option.ExpireTimeSpan = TimeSpan.FromHours(1);
               });


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
