using Ardalis.Result;
using Permify.Domain.Entities.Common;
using MediatR;

namespace Permify.Application.Features.Heroes.GetHeroById;

public record GetHeroByIdRequest(HeroId Id) : IRequest<Result<GetHeroResponse>>;