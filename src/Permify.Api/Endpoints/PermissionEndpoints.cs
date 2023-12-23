using Base.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace Permify.Api.Endpoints;

public static class PermissionEndpoints
{
    public static void MapPermissionEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/Permission")
            .WithTags("Permission");
        
        group.MapPost("/", async (Schema.SchemaClient client, [FromBody] string schema) => await client.WriteAsync(new SchemaWriteRequest
        {
            TenantId = "t1",
            Schema = schema
        }));
    }
}