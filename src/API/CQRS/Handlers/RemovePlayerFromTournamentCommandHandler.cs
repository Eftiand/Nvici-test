

using API.CQRS.Commands;
using API.CQRS.Queries;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class RemovePlayerFromTournamentCommandHandler : IRequestHandler<RemovePlayerFromTournamentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public RemovePlayerFromTournamentCommandHandler(IUnitOfWork unitOfWork,IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(RemovePlayerFromTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _mediator.Send(new GetTournamentByIdQuery(request.TournamentId), cancellationToken);

        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        var player = tournament.Players.Find(p => p.Id == request.PlayerId);

        if (player is null)
        {
            return PlayerErrors.NotFound(request.PlayerId);
        }

        tournament.Players.Remove(player);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}