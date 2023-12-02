using Permify.Application.Common;
using Permify.Infrastructure;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Permify.Api.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services)
    {
        services.AddScoped<IContext, ApplicationDbContext>();
        NewId.SetProcessIdProvider(new CurrentProcessIdProvider());
        
        return services;
    }
    
    
}