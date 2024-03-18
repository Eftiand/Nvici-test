
using API.Data.Entities;
using MediatR;

namespace API.CQRS.Queries;

public record GetAllPlayersQuery : IRequest<IEnumerable<Player>>;