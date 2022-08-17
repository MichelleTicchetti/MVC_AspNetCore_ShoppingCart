using Carrito_A.Data;
using Carrito_A.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carrito_A
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region Tipo de DB Provider a usar
            try
            {
                _dbInMemory = Configuration.GetValue<bool>("DbInMem");
            }
            catch
            {
                //Dejamos el tratamiento que le queremos dar. En este caso asumimos que si falla la lectura del appsettings.json tomamos en memoria.
                _dbInMemory = true;
            }
            #endregion
        }

        public IConfiguration Configuration { get; }

        private bool _dbInMemory = false;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Tipo de BD Provider a usar
            //EN MEMORIA
            if (_dbInMemory)
            {
                services.AddDbContext<CarritoContext>(options => options.UseInMemoryDatabase("CarritoDB"));
            }
            else
            {
                services.AddDbContext<CarritoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Carrito"))//Obtenemos el Connection String a partir de la Clave CarritoCS
                );
            }
            #endregion

            #region Identity

            services.AddIdentity<Persona, Rol>().AddEntityFrameworkStores<CarritoContext>();

            services.Configure<IdentityOptions>(opciones =>
                {
                    opciones.Password.RequiredLength = 5;
                }
            
            );

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
               opciones =>
               {
                   opciones.LoginPath = "/Account/IniciarSesion";
                   opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                   opciones.Cookie.Name = "CarritoCookie";
               });

            #endregion
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CarritoContext carritoContext)
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

            if (!_dbInMemory)
            {
                //si la bd no existe la crea
                carritoContext.Database.Migrate();
            }

            app.UseRouting();

            /*
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto = serviceScope.ServiceProvider.GetRequiredService<CarritoContext>();

                contexto.Database.Migrate();
            }
            */

            app.UseAuthentication();
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
