﻿using System.Reflection;
using ParkingChecker.InputFromAndrioidApi.Base.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ParkingChecker.InputFromAndrioidApi.Configuration;

namespace Lexily.ManagementWarehouseService.Configuration
{
    public static class DependencyStartup
    {
        private static void ConfigureMigrations(IConfiguration configuration)
        {
            var databaseConfiguration =
                configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>();
            
        }

        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            AddInfrastructure(services);
            ConfigureDatabase(services, configuration);
        }

        private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfiguration>(configuration.GetSection("DatabaseConfiguration"));
            services.AddSingleton<IDatabaseConfiguration>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
        }
        

        private static void AddInfrastructure(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            
        }
    }
}