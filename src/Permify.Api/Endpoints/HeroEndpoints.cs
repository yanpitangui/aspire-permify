using Ardalis.Result.AspNetCore;
using Permify.Application.Features.Heroes.CreateHero;
using Permify.Application.Features.Heroes.DeleteHero;
using Permify.Application.Features.Heroes.GetAllHeroes;
using Permify.Application.Features.Heroes.GetHeroById;
using Permify.Application.Features.Heroes.UpdateHero;
using Permify.Domain.Entities.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Permify.Api.Endpoints;

public static class HeroEndpoints
{
    public static void MapHeroEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/Hero")
            .WithTags("Hero");
        
        group.MapGet("/", async (IMediator mediator, [AsParameters] GetAllHeroesRequest request) =>
        {
            var result = await mediator.Send(request);
            return result;
        });

        group.MapGet("{id}", async (IMediator mediator, HeroId id) =>
        {
            var result = await mediator.Send(new GetHeroByIdRequest(id));
            return result.ToMinimalApiResult();
        });

        group.MapPost("/", async (IMediator mediator, CreateHeroRequest request) =>
        {
            var result = await mediator.Send(request);
            return result.ToMinimalApiResult();
        });

        group.MapPut("{id}", async (IMediator mediator, HeroId id, UpdateHeroRequest request) =>
        {
            var result = await mediator.Send(request with { Id = id });
            return result.ToMinimalApiResult();
        });

        group.MapDelete("{id}", async (IMediator mediator, HeroId id) =>
        {
            var result = await mediator.Send(new DeleteHeroRequest(id));
            return result.ToMinimalApiResult();
        });
    }
}