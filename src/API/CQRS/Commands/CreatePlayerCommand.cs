

using API.Data.Entities;
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record CreatePlayerCommand(string Name, int Age) : IRequest<Result<Player>>;