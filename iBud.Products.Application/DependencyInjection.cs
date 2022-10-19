using iBud.Products.Application.Interfaces.Authentication;
using iBud.Products.Application.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace iBud.Products.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services){
        services.AddScoped<IAuthenticationService,AuthenticationsService>();
        return services;
    }
}