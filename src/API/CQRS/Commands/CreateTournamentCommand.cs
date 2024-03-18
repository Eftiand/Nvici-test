

using API.Data.Entities;
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record CreateTournamentCommand(string Name) : IRequest<Result<Tournament>>;