
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public record DeletePlayerCommand(int PlayerId) : IRequest<Result>;