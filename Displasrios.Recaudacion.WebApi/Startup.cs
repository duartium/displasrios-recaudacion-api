using Displasrios.Recaudacion.Core.Contracts;
using Displasrios.Recaudacion.Core.Contracts.Repositories;
using Displasrios.Recaudacion.Core.Contracts.Services;
using Displasrios.Recaudacion.Core.Models.Security;
using Displasrios.Recaudacion.Infraestructure.MainContext;
using Displasrios.Recaudacion.Infraestructure.Repositories;
using Displasrios.Recaudacion.Infraestructure.Services;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Displasrios.Recaudacion.WebApi.Hubs;

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
            services.AddCors(options => options.AddPolicy("DisplasriosPolicy", builder =>
            {
                //builder.WithOrigins("http://localhost:4200")
                //builder.AllowAnyOrigin()
                builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .SetIsOriginAllowed((Host) => true)
                       .AllowCredentials();
            }));

            services.AddSignalR();
            //services.AddControllers().AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //});
            services.AddControllers().AddJsonOptions(o =>
             {
                 o.JsonSerializerOptions.PropertyNamingPolicy = null;
                 o.JsonSerializerOptions.DictionaryKeyPolicy = null;
             }).AddFluentValidation(cfg =>
             {
                 cfg.ImplicitlyValidateChildProperties = true;
             });

            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";
                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"DISPLASRIOS API {groupName}",
                    Version = groupName,
                    Description = "API GESTIÓN DE RECAUDACIÓN Y VENTAS",
                    Contact = new OpenApiContact
                    {
                        Name = "Byron Duarte",
                        Email = "byronduarte95@gmail.com",
                        Url = new Uri("http://neutrinodevs.com/"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
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

            services.AddApiVersioning(opt => {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-version"),
                    new MediaTypeApiVersionReader("ver"));
            });

            services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddSingleton(Configuration);
            services.AddDbContext<DISPLASRIOSContext>(options =>
            {
                options.UseSqlServer(Encoding.UTF8.GetString(Convert.FromBase64String(Configuration.GetSection("ConnectionStrings:displasrios_db").Value)));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddScoped<IAuthenticationService, TokenAuthenticationService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICatalogueRepository, CatalogueRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICashRegisterRepository, CashRegisterRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISellerRepository, SellerRepository>();
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
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "DISPLASRIOS API");
            });
            app.UseCors("DisplasriosPolicy");
            app.UseAuthentication();
            app.UseRouting();
            
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<OrderHub>("hub");
            });

            app.UseHttpsRedirection();

        }
    }
}
