using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RentACar.API.Auth;
using RentACar.Core.Repositories;
using RentACar.Core.Services;
using RentACar.Core.UnitOfWorks;
using RentACar.Data;
using RentACar.Data.Repositories;
using RentACar.Data.UnitOfWorks;
using HealthChecks.UI.Client;
using HealthChecks.UI.Configuration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;

namespace RentACar.API
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
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service.Services.Service<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IDapperService<>), typeof(Service.Services.DapperService<>));
            services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SqlConnectionString"].ToString(), o =>
                {
                    o.MigrationsAssembly("RentACar.Data");
                });
            });
            services.AddControllers();
            services.AddHealthChecks()
                .AddSqlServer(
                    Configuration.GetConnectionString("SqlConnectionString"),
                    "SELECT 1;",
                    "Veritabani",
                    HealthStatus.Degraded,
                    timeout: TimeSpan.FromSeconds(30),
                    tags: new[] { "db", "sql", "sqlServer", });

            services.AddHealthChecksUI().AddInMemoryStorage();
    
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentACar.API", Version = "v1" });
                c.EnableAnnotations();
                #region Auth
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
                #endregion
            });

            #region Auth
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RentACar.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            #region Auth
            app.UseAuthentication();
            #endregion

            app.UseAuthorization();

        
            app.UseHealthChecks("/HealthCheck-api", new HealthCheckOptions
            {
            Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/HealthCheck";
            });
     
        }
    }
}
