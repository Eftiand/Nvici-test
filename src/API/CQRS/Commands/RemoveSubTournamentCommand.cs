
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record RemoveSubTournamentCommand(int TournamentId, int SubTournamentId) : IRequest<Result>;
