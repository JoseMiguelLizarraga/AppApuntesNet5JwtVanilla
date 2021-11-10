using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using DataAccess.Models;
using DataAccess;
using Services;

namespace AppApuntesNet5
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            // SQL Server
            services.AddDbContext<AppApuntesNet5Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
            );

            // MySQL
            //string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContextPool<ApplicationDbContext>(options =>
            //    options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
            //);


            services.AddSingleton<ICategoriaService, CategoriaService>();
            services.AddTransient<ITemaService, TemaService>();
            services.AddTransient<IDetalleTemaService, DetalleTemaService>();

            // Agregar identity para poder usar el sistema de usuarios por defectos de asp.net
            // La clase ApplicationUser del modelo representa a un usuario en la base de datos

            services.AddIdentity<UsuarioAutenticado, IdentityRole>()
                .AddEntityFrameworkStores<AppApuntesNet5Context>()
                .AddDefaultTokenProviders();

            // Configurar servicio de autenticacion con tokens

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "yourdomain.com",
                    ValidAudience = "yourdomain.com",
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])),  // Esta es una variable de ambiente guardada en  launchSettings.json 
                    ClockSkew = TimeSpan.Zero
                });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppApuntesNet5", Version = "v1" });
            });

            /* Asi ignora la referencia ciclica al convertir a json una instancia de una clase
            padre que tiene una referencia cruzada a una clase hija; y cuya clase hija tambien
            hace referencia a la clase padre, formando un loop infinito.
            */

            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.DateFormatString = "yyyy-MM-dd";  // Asi las fechas convertidas a json se muestran formateadas a yyyy-MM-dd
                o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddCors();  // Habilitar que otras aplicaciones usen el web api 
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();  // Esto es para la autenticacion de usuario

            // Habilitar que otras aplicaciones usen el web api 

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppApuntesNet5 v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
