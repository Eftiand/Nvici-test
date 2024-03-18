
using API.Data.Entities;
using MediatR;

namespace API.CQRS.Queries;

public record GetAllTournamentsQuery() : IRequest<IEnumerable<Tournament>>;