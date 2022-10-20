using iBud.Products.Infrastructure.Common.Authentication;
using iBud.Products.Infrastructure.Common.Configuration;
using iBud.Products.Infrastructure.Interfaces;
using iBud.Products.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iBud.Products.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection(JwtConfig.SectionName));
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IFirstRepository, FirstRepository>();
        return services;
    }
}