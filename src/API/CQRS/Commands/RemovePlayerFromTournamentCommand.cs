
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record RemovePlayerFromTournamentCommand(int TournamentId, int PlayerId) : IRequest<Result>;
