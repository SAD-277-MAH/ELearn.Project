using AutoMapper;
using Banking.Services.Upload.Service;
using ELearn.Common.Extentions;
using ELearn.Common.Filters;
using ELearn.Common.Helpers.Interface;
using ELearn.Common.Helpers.Service;
using ELearn.Common.Utilities;
using ELearn.Data.Context;
using ELearn.Data.Models;
using ELearn.Repo.Infrastructure;
using ELearn.Services.Seed.Service;
using ELearn.Services.Site.Interface;
using ELearn.Services.Site.Service;
using ELearn.Services.Upload.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Presentation
{
    public class Startup
    {
        private readonly int? _httpsPort;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("Properties\\launchSettings.json").Build();
                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 2147383648; });

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders().AddErrorDescriber<PersianIdentityErrorDescriber>();

            var tokenSettings = Configuration.GetSection("TokenSettings").Get<TokenSettings>();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Secret)),
                        ValidateIssuer = true,
                        ValidIssuer = tokenSettings.Site,
                        ValidateAudience = true,
                        ValidAudience = tokenSettings.Audience,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.AddAuthorization(option =>
            {
                option.AddPolicy("RequireSystemRole", policy => policy.RequireRole("System"));
                option.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                option.AddPolicy("RequireTeacherRole", policy => policy.RequireRole("Teacher"));
                option.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student"));

                option.AddPolicy("RequireTeacherOrStudentRole", policy => policy.RequireRole("Teacher", "Student"));

                option.AddPolicy("Access", policy => policy.RequireRole("System", "Admin", "Teacher", "Student"));
            });

            var mapperConfig = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.ReturnHttpNotAcceptable = true;

                option.SslPort = _httpsPort;
                option.Filters.Add(typeof(RequireHttpsAttribute));
            })
               .AddNewtonsoftJson(option =>
               {
                   option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
               });

            services.AddHsts(option =>
            {
                option.MaxAge = TimeSpan.FromDays(180);
                option.IncludeSubDomains = true;
                option.Preload = true;
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("EnableCors", builder =>
            //    {
            //        builder
            //          //.AllowAnyOrigin()
            //          .WithOrigins(new string[] { "http://localhost:3000", "https://localhost:3000" })
            //          .AllowAnyHeader()
            //          .AllowAnyMethod()
            //          .Build();
            //    });
            //});
            services.AddCors();

            services.AddOpenApiDocument(document =>
            {
                document.DocumentName = "v1_Site";
                document.ApiGroupNames = new[] { "v1_Site" };

                document.PostProcess = d =>
                {
                    d.Info.Title = "Website Api Document";
                };

                document.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("JWT",
                        new OpenApiSecurityScheme
                        {
                            Type = OpenApiSecuritySchemeType.ApiKey,
                            Name = "Authorization",
                            In = OpenApiSecurityApiKeyLocation.Header,
                            Description = "Type into the textbox: Bearer {your JWT token}."
                        }));

                document.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork<DatabaseContext>, UnitOfWork<DatabaseContext>>();
            services.AddTransient<SeedService>();
            services.AddScoped<IUtilities, Utilities>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddTransient<IOrderService, OrderService>();

            services.AddScoped<UserCheckIdFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SeedService seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddAppError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });

                app.UseHsts();
            }

            seeder.SeedRoles();
            seeder.SeedUsers();
            seeder.SeedCategories();

            //app.UseCors("EnableCors");
            app.UseCors(p => p
            .AllowAnyOrigin()
            //.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            //.WithExposedHeaders("ejUrl")
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseStaticFiles(new StaticFileOptions()
            {
                RequestPath = new PathString("/wwwroot")
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Swagger}/{action=Index}/{id?}"
                );
            });
        }
    }
}
