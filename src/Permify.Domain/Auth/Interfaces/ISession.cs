using Permify.Domain.Entities.Common;
using System;

namespace Permify.Domain.Auth.Interfaces;

public interface ISession
{
    public UserId UserId { get; }

    public DateTime Now { get; }
}