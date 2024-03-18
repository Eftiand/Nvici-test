
using API.Data.Entities;
using MediatR;

namespace API.CQRS.Queries;

public record GetPlayerByIdQuery(int PlayerId) : IRequest<Player?>;