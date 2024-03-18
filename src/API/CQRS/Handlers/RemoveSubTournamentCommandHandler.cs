

using API.CQRS.Commands;
using API.CQRS.Queries;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class RemoveSubTournamentCommandHandler : IRequestHandler<RemoveSubTournamentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public RemoveSubTournamentCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(RemoveSubTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _mediator.Send(new GetTournamentByIdQuery(request.TournamentId), cancellationToken);

        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        var subTournament = tournament.SubTournaments.Find(st => st.Id == request.SubTournamentId);

        if (subTournament is null)
        {
            return TournamentErrors.NotFound(request.SubTournamentId);
        }

        tournament.SubTournaments.Remove(subTournament);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}