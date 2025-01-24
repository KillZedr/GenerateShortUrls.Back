using GenerateShortUrsl.Data.GenerateShortUrls.DAL;
using GenerateShortUrsl.Data.GenerateShortUrls.DAL.Contracts;
using GenerateShortUrsl.Data.GenerateShortUrls.DAL.RealizationInterfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace GenerateShortUrl.API
{
    public class Startup
    {
        internal static void AddServices(WebApplicationBuilder builder)
        {
            AddSerilog(builder);
            RegisterDAL(builder.Services);
            AddErrorLogging(builder);
        }


        public static void RegisterDAL(IServiceCollection services)
        {
            services.AddTransient<DbContextOptions<GenerateUrlShort_DbContext>>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                var builder = new DbContextOptionsBuilder<GenerateUrlShort_DbContext>();

                builder.UseNpgsql(connectionString);

                return builder.Options;
            });

            services.AddScoped<DbContext, GenerateUrlShort_DbContext>();

            services.AddScoped(typeof(IUrlRepository<>), typeof(UrlRepository<>));
        }





        internal static void AddSerilog(WebApplicationBuilder builder)
        {
            var loggerConfig = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}");

            if (builder.Environment.IsDevelopment())
            {
                loggerConfig = loggerConfig.MinimumLevel.Debug();
            }
            else
            {
                loggerConfig = loggerConfig.MinimumLevel.Warning();
            }

            var logger = loggerConfig.CreateLogger();
            builder.Services.AddSingleton<ILogger>(logger);
        }

        internal static void AddErrorLogging(WebApplicationBuilder builder)
        {
            var errorLoggerConfig = new LoggerConfiguration()
                .WriteTo.File("errors.txt", rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Error(); 

          
            if (builder.Environment.IsDevelopment())
            {
                errorLoggerConfig = errorLoggerConfig.MinimumLevel.Debug();
            }
            else
            {
                errorLoggerConfig = errorLoggerConfig.MinimumLevel.Error(); 
            }

            var errorLogger = errorLoggerConfig.CreateLogger();
            builder.Services.AddSingleton<ILogger>(errorLogger);
        }

    }
}
