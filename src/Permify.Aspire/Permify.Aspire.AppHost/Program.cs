using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);


var postgresdb = builder.AddPostgresContainer("postgres", port:5432, password:"localdev")
    .WithVolumeMount("pg-data", "/var/lib/postgresql/data", VolumeMountType.Named)
    .AddDatabase("aspire-permify");

string? oltpEndpoint = builder.Configuration.GetValue<string>("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL");
var permify = builder.AddContainer("permify", "ghcr.io/permify/permify", "latest")
    .WithArgs("serve")
    .WithReference(postgresdb)
    .WithEnvironment("PERMIFY_TRACER_ENABLED", "true")
    .WithEnvironment("PERMIFY_TRACER_EXPORTER", "otlp")
    .WithEnvironment("PERMIFY_TRACER_ENDPOINT", oltpEndpoint)
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