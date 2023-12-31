﻿using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Permify.Application.Auth;
using Permify.Domain.Auth.Interfaces;
using Permify.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Permify.Api.Configurations;

public static class PersistanceSetup
{
    public static void AddPersistenceSetup(this WebApplicationBuilder builder)
    {

        builder.Services.AddScoped<ISession, Session>();
        builder.AddNpgsqlDbContext<ApplicationDbContext>("aspire-permify", configureDbContextOptions: db =>
        {
            db.UseExceptionProcessor();
        });

    }
}