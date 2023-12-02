using Ardalis.Result;
using Permify.Domain.Entities.Common;
using MediatR;

namespace Permify.Application.Features.Heroes.DeleteHero;

public record DeleteHeroRequest(HeroId Id) : IRequest<Result>;