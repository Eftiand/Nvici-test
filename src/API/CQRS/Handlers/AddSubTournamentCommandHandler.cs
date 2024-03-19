
using API.CQRS.Commands;
using API.CQRS.Queries;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class AddSubTournamentCommandHandler : IRequestHandler<AddSubTournamentCommand, Result>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    public AddSubTournamentCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddSubTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _mediator.Send(new GetTournamentByIdQuery(request.TournamentId), cancellationToken);
        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        var subTournament = await _mediator.Send(new GetTournamentByIdQuery(request.SubTournamentId), cancellationToken);
        if (subTournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        var result = tournament.AddSubTournament(subTournament);

        if (result.IsFailure)
            return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}