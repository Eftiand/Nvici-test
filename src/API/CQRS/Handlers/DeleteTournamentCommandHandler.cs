
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class DeleteTournamentCommandHandler : IRequestHandler<DeleteTournamentCommand, Result>
{
    private readonly IRepository<Tournament> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTournamentCommandHandler(IRepository<Tournament> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _repository.GetByIdAsync(request.TournamentId, cancellationToken);
        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.TournamentId);
        }

        _repository.Delete(tournament);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}