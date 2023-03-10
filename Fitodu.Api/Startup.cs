using System.Collections.Generic;
using System.Text;
using Fitodu.Service.Infrastructure.Middlewares;
using Fitodu.Service.Infrastructure.Swagger;
using Fitodu.Core.Enums;
using Fitodu.Core.Extensions;
using Fitodu.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using System;

namespace Fitodu.Api
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
            services.RegisterDatabase(Configuration);
            services.RegisterServices();

            services
                .AddCors()
                .AddMvc()

                // https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/issues/17
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })

                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddMvcLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(o =>
                {
                    var factory = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create("Validation", "Fitodu.Resource");
                    o.DataAnnotationLocalizerProvider = (t, f) => localizer;
                });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info
                {
                    Title = "Fitodu API",
                    Version = "v1"
                });
                x.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                x.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {
                        "Bearer", new string[] { }
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                x.IncludeXmlComments(xmlPath);
                //x.IncludeXmlComments(string.Format(@"\Fitodu.Api\Fitodu.Api.xml",
                           //System.AppDomain.CurrentDomain.BaseDirectory));
                x.DescribeAllEnumsAsStrings();
                x.OperationFilter<LanugageHeaderParameter>();
            });

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization(x =>
            {
                //x.AddPolicy(UserRole.User.GetName(), policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireRole(UserRole.User.GetName(), UserRole.CompanyAdmin.GetName(), UserRole.SuperAdmin.GetName());
                //});
                x.AddPolicy(UserRole.Client.GetName(), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(UserRole.Client.GetName(), UserRole.Coach.GetName());
                });
                x.AddPolicy(UserRole.Coach.GetName(), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(UserRole.Coach.GetName());
                });
            });

            //.AddHangfire(x => x.UseSqlServerStorage(Configuration["ConnectionStrings:LogConnection"]));
            //services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(options => options
                .WithExposedHeaders("Content-Disposition")
                //.WithOrigins(Configuration["Environment:ClientUrl"])
                .AllowAnyOrigin()
                .AllowAnyHeader().AllowAnyMethod());//.AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Fitodu API V1");
                x.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<RequestCultureMiddleware>();
            app.UseMvc();

            //app.UseHangfireDashboard();
        }
    }
}