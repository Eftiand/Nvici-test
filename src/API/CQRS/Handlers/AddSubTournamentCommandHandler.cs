
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class AddSubTournamentCommandHandler : IRequestHandler<AddSubTournamentCommand, Result>
{
    private readonly IRepository<Tournament> _repository;
    private readonly IUnitOfWork _unitOfWork;
    public AddSubTournamentCommandHandler(IRepository<Tournament> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddSubTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _repository.GetByIdAsync(request.TournamentId, cancellationToken);
        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        var subTournament = await _repository.GetByIdAsync(request.SubTournamentId, cancellationToken);
        if (subTournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        //Validate so there isn't 5 layers in the tree
        var depth = await ValidateTournamentDepth(tournament, cancellationToken);
        if (depth >= 5)
        {
            return TournamentErrors.DepthTooBig();
        }

        tournament.AddSubTournament(subTournament);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<int> ValidateTournamentDepth(Tournament tournament, CancellationToken cancellationToken)
    {
        if (tournament.ParentId == null)
        {
            return 1;
        }

        var parentTournament = await _repository.GetByIdAsync(tournament.ParentId.Value, cancellationToken);
        return 1 + await ValidateTournamentDepth(parentTournament!, cancellationToken); // Recursively calculate depth
    }
}