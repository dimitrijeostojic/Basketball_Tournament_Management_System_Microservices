using Domain.Abstraction;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Data;
using Infrastructure.RepositoryImplementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastracture(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
         options.UseSqlServer(
             configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityCore<User>() //konfiguracija identity servisa
           .AddRoles<IdentityRole>() //dodavanje podrske za role
                                     //.AddTokenProvider<DataProtectorTokenProvider<User>>("") //dodavanje token provajdera
           .AddEntityFrameworkStores<AuthDbContext>() //podesavanje entity framework skladista
           .AddDefaultTokenProviders(); //dodavanje podrazumevanih token provajdera


        services.TryAddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AuthDbContext>());
        services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        return services;
    }
}
