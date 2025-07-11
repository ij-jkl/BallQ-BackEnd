﻿namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IStrikerRepository, StrikerRepository>();
            services.AddScoped<IDataLoadRepository, DataLoadRepository>();
            
            services.AddTransient<IFakeStrikerSeederService, FakeStrikerSeederService>();

            return services;
        }
    }
}