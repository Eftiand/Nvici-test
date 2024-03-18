using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record AddSubTournamentCommand(int TournamentId, int SubTournamentId) : IRequest<Result>;
