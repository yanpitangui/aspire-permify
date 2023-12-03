var builder = DistributedApplication.CreateBuilder(args);


var postgresdb = builder.AddPostgresContainer("postgres", port:5432)
    .WithEnvironment("POSTGRES_USER", "permify")
    .AddDatabase("aspire-permify");

var permify = builder.AddContainer("permify", "ghcr.io/permify/permify", "latest")
    .WithReference(postgresdb)
    //.WithOtlpExporter()
    .WithServiceBinding(3476, 3476)
    .WithServiceBinding(3478, 3478);


var api = builder.AddProject<Projects.Permify_Api>("api")
    .WithLaunchProfile("Api")
    .WithReference(postgresdb);
    //.WithReference(permify);



builder.Build().Run();