using Permify.Api.Common;
using Permify.Api.Configurations;
using Permify.Api.Endpoints;
using Permify.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();
// Controllers
builder.AddValidationSetup();

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddSwaggerSetup();

// Persistence
builder.AddPersistenceSetup();

// Application layer setup
builder.Services.AddApplicationSetup();

builder.Services.AddPermifySetup();

// Add identity stuff
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Request response compression
builder.Services.AddCompressionSetup();

// HttpContextAcessor
builder.Services.AddHttpContextAccessor();

// Mediator
builder.Services.AddMediatRSetup();

// Exception handler
builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Logging.ClearProviders();

// Add serilog
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Host.UseLoggingSetup(builder.Configuration);
    
    // Add opentelemetry
    builder.AddOpenTemeletrySetup();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseSwaggerSetup();
app.UseHsts();

app.UseResponseCompression();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapHeroEndpoints();
app.MapPermissionEndpoints();   

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();



await app.Migrate();

await app.RunAsync();