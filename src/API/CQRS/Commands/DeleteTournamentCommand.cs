

using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record DeleteTournamentCommand(int TournamentId) : IRequest<Result>;