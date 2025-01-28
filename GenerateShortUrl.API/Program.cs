
using GenerateShortUrl.API.InfoService;
using GenerateShortUrls.BLL;
using GenerateShortUrls.BLL.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace GenerateShortUrl.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ConfigureHttpsDefaults(httpsOptions =>
                {
                    httpsOptions.ServerCertificate = new X509Certificate2("/https/aspnetcore-dev.pfx", "12345");
                });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication(options =>
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
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var token = context.SecurityToken as JwtSecurityToken;
                        if (token == null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        
                        var userIdString = token.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
                        if (string.IsNullOrEmpty(userIdString))
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        
                        if (!Guid.TryParse(userIdString, out Guid userId))
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var user = await userService.GetUserByIdAsync(userId);

                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        
                        var storedToken = user.Token; 
                        var currentToken = token.RawData;
                        if (storedToken != currentToken)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }
                    }
                };

            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<UserInfoService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter your Bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                options.OperationFilter<AddUserInfoToSwaggerHeader>();

            });
            Startup.AddServices(builder);

            ModuleHead.RegisterModule(builder.Services);
            var app = builder.Build();
            DbInitializer.InitializeDb(app.Services);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();
            
            app.Run();
        }
    }
}
