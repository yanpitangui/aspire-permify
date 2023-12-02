﻿using Ardalis.Result;
using Permify.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Permify.Application.Features.Heroes.DeleteHero;

public class DeleteHeroHandler : IRequestHandler<DeleteHeroRequest, Result>
{
    private readonly IContext _context;
    public DeleteHeroHandler(IContext context)
    {
        _context = context;
    }
    public async Task<Result> Handle(DeleteHeroRequest request, CancellationToken cancellationToken)
    {
        var hero = await _context.Heroes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (hero is null) return Result.NotFound();
        _context.Heroes.Remove(hero);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}