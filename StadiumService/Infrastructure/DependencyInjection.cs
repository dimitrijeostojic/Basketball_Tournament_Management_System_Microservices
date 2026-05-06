using Domain.RepositoryInterfaces;
using Infrastracture.RepositoryImplementations;
using Infrastructure.Seed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddScoped<IStadiumRepository, StadiumRepository>();
       services.AddScoped<StadiumSeeder>();
        return services;
    }

}
