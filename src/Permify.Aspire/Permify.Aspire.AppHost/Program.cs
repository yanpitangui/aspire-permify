using Microsoft.Extensions.Configuration;
using System.Text;

var builder = DistributedApplication.CreateBuilder(args);


var postgresdb = builder.AddPostgresContainer("postgres", port:5432, password:"localdev")
    .WithVolumeMount("pg-data", "/var/lib/postgresql/data", VolumeMountType.Named)
    .AddDatabase("aspire-permify");

string? oltpEndpoint = builder.Configuration.GetValue<string>("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL");
var permify = builder.AddContainer("permify", "permify/permify", "latest")
    .WithArgs("serve")
    // .WithEnvironment("PERMIFY_TRACER_ENABLED", "true")
    // .WithEnvironment("PERMIFY_TRACER_EXPORTER", "otlp")
    // .WithEnvironment("PERMIFY_TRACER_ENDPOINT", "host.docker.internal:16150")
    // .WithEnvironment("PERMIFY_TRACER_INSECURE", "true")
    // Aspire dashboard otlp only works with grpc endpoints
    // .WithEnvironment("PERMIFY_DATABASE_ENGINE", "postgres")
    // .WithEnvironment("PERMIFY_DATABASE_AUTO_MIGRATE", "true")
    // .WithEnvironment("PERMIFY_DATABASE_AUTO_MIGRATE", "true")
    // .WithEnvironment("PERMIFY_DATABASE_URI", () =>
    // {
    //     return $"postgres://{postgresdb.Resource.Parent.Name}:localdev@localhost:5432/aspire-permify";
    // })
    .WithServiceBinding(3478, 3478, name: "permifyGrpc");


var api = builder.AddProject<Projects.Permify_Api>("api")
    .WithLaunchProfile("Api")
    .WithReference(postgresdb)
    .WithReference(permify.GetEndpoint("permifyGrpc"));

builder.Build()
    .Run();