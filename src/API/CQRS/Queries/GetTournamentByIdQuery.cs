
using API.Data.Entities;
using MediatR;

namespace API.CQRS.Queries;

public record GetTournamentByIdQuery(int TournamentId) : IRequest<Tournament?>;
