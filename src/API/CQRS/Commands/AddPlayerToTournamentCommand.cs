
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record AddPlayerToTournamentCommand(int TournamentId, int PlayerId) : IRequest<Result>;