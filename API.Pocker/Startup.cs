using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using API.Pocker.Data;
using API.Pocker.Mapping;
using API.Pocker.Models;
using API.Pocker.Services;
using API.Pocker.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Pocker.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API.Pocker.Services.ManageAccounts;
using API.Pocker.Support;
using API.Pocker.Hubs;

namespace API.Pocker
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
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            services.AddDbContext<ApplicationDbContext>(options =>
           options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
           .UseLoggerFactory(loggerFactory));

            services.AddIdentity<IdentityUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireDigit = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.Password.RequiredLength = 5;

            });

            services.Configure<AmqpSettings>(Configuration.GetSection("amqp"));

            services.Configure<JwtSettings>(Configuration.GetSection("JWTSettings"));
            var jwtSettings = Configuration.GetSection("JWTSettings").Get<JwtSettings>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
                option.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        await context.Response.WriteJsonAsync(
                                 401,
                                 ResponseAPI.Error("You are not authorized")
                             );
                    },
                    OnForbidden = async context =>
                    {
                        await context.Response.WriteJsonAsync(
                            403,
                            ResponseAPI.Error("You are not authorized to access this resource")
                        );
                    }
                };
            }
           );

            services.AddCors(setupAction =>
            {
                setupAction.AddPolicy("CorsDefaultPolicy", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().Build();
                });
            });


            services.AddControllers();
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Put only your JWT bearer on the textbox",
                BearerFormat = "JWT",
                Scheme = "bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Planning Pocker",
                    Description = " ",
                    Version = "v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Maikel Blanco Dieguez",
                        Email = "mbdieguez@uci.cu",

                    }
                });
                swaggerGenOptions.AddSecurityDefinition(
                      jwtSecurityScheme.Reference.Id,
                      jwtSecurityScheme
                  );

                swaggerGenOptions.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                            { jwtSecurityScheme, Array.Empty<string>() }
                    }
                );
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swaggerGenOptions.IncludeXmlComments(xmlPath);
            }
           );

            services.AddSingleton<AmqpService>();

            services.AddScoped<ManageAccountService>();
            services.AddScoped<CardsService>();
            services.AddScoped<UserHistoryService>();
            services.AddScoped<UserProfileService>();
            services.AddScoped<VotesService>();


            services.AddAutoMapper(config =>
            {
                config.AddProfile<MapProfile>();
            },
               Assembly.GetExecutingAssembly()
           );

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseCors("CorsDefaultPolicy");

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(SwaggerUIOption =>
            {
                SwaggerUIOption.SwaggerEndpoint("/swagger/v1/swagger.json", "Planning Poker");
            });

            app.UseRouting();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<VotesHub>("/hubs/votes");
            });
        }
    }
}
