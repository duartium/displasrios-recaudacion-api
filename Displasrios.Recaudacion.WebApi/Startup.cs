using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Models.Security;
using Displasrios.Recaudacion.Infraestructure.Repositories;
using Displasrios.Recaudacion.Infraestructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace Displasrios.Recaudacion.WebApi
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
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";
                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"NEO {groupName}",
                    Version = groupName,
                    Description = "API GESTIÓN DE RECAUDACIÓN Y VENTAS",
                    Contact = new OpenApiContact
                    {
                        Name = "NEO",
                        Email = "neutrinodevs.com",
                        Url = new Uri("http://neutrinodevs.com/"),
                    }
                });
            });

            services.Configure<TokenManagement>(Configuration.GetSection("TokenManagement"));
            var tokenManagement = Configuration.GetSection("TokenManagement").Get<TokenManagement>();
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenManagement.Secret)),
                    ValidIssuer = tokenManagement.Issuer,
                    ValidAudience = tokenManagement.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                
            });

            services.AddScoped<IAuthenticationService, TokenAuthenticationService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddFile("Logs/api_{Date}.txt");

            app.UseSwagger();
            app.UseSwaggerUI(setup => {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "NEO API");
            });

            app.UseAuthentication();
            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
