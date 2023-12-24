using Microsoft.Extensions.Configuration;
using System.Text;

var builder = DistributedApplication.CreateBuilder(args);


var postgresdb = builder.AddPostgresContainer("postgres", port:5432, password:"localdev")
    .WithVolumeMount("pg-data", "/var/lib/postgresql/data", VolumeMountType.Named)
    .AddDatabase("aspire-permify");

var otelCollector = builder.AddContainer("otel-collector", "otel/opentelemetry-collector-contrib")
    .WithServiceBinding(4318, 4318, name: "http")
    .WithVolumeMount("../../../etc/otelcol-config.yaml", "/etc/otelcol-config.yml")
    .WithArgs("--config=/etc/otelcol-config.yml");

string? oltpEndpoint = builder.Configuration.GetValue<string>("DOTNET_DASHBOARD_OTLP_ENDPOINT_URL");
var permify = builder.AddContainer("permify", "permify/permify", "latest")
    .WithArgs("serve")
    .WithEnvironment("PERMIFY_TRACER_ENABLED", "true")
    .WithEnvironment("PERMIFY_TRACER_EXPORTER", "otlp")
    // Aspire dashboard otlp only works with grpc endpoints, so we use the collector to send the traces to the dashboard
    .WithEnvironment("PERMIFY_TRACER_ENDPOINT", "host.docker.internal:4318")
    .WithEnvironment("PERMIFY_TRACER_INSECURE", "true")
    
    .WithEnvironment("PERMIFY_METER_ENABLED", "true")
    .WithEnvironment("PERMIFY_METER_EXPORTER", "otlp")
    // Aspire dashboard otlp only works with grpc endpoints, so we use the collector to send the metrics to the dashboard
    .WithEnvironment("PERMIFY_METER_ENDPOINT", "host.docker.internal:4318")
    .WithEnvironment("PERMIFY_METER_INSECURE", "true")
    .WithEnvironment("PERMIFY_DATABASE_ENGINE", "postgres")
    .WithEnvironment("PERMIFY_DATABASE_AUTO_MIGRATE", "true")
    .WithEnvironment("PERMIFY_DATABASE_URI", () =>
    {
        return $"postgres://{postgresdb.Resource.Parent.Name}:localdev@host.docker.internal:5432/aspire-permify";
    })
    .WithServiceBinding(3478, 3478, name: "permifyGrpc");


var api = builder.AddProject<Projects.Permify_Api>("api")
    .WithLaunchProfile("Api")
    .WithReference(postgresdb)
    .WithReference(permify.GetEndpoint("permifyGrpc"));

builder.Build()
    .Run();