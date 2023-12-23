using Microsoft.Extensions.DependencyInjection;
using System;

namespace Permify.Api.Configurations;

public static class PermifySetup
{
    public static void AddPermifySetup(this IServiceCollection services)
    {
        services.AddGrpcClient<Base.V1.Permission.PermissionClient>(o =>
        {
            o.Address = new Uri("http://_permifyGrpc.permify");
        });

        services.AddGrpcClient<Base.V1.Schema.SchemaClient>(o =>
        {
            o.Address = new Uri("http://_permifyGrpc.permify");
        });
        services.AddGrpcClient<Base.V1.Data.DataClient>(o =>
        {
            o.Address = new Uri("http://_permifyGrpc.permify");
        });

        services.AddGrpcClient<Base.V1.Watch.WatchClient>(o =>
        {
            o.Address = new Uri("http://_permifyGrpc.permify");
        });

        services.AddGrpcClient<Base.V1.Bundle.BundleClient>(o =>
        {
            o.Address = new Uri("http://_permifyGrpc.permify");
        });


        services.AddGrpcClient<Base.V1.Tenancy.TenancyClient>(o =>
        {
            o.Address = new Uri("http://_permifyGrpc.permify");
        });
    }
}