
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class UpdateTournamentCommandHandler : IRequestHandler<UpdateTournamentCommand, Result<Tournament>>
{
    private readonly IRepository<Tournament> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTournamentCommandHandler(IRepository<Tournament> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Tournament>> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tournament is null)
        {
            return TournamentErrors.NotFound(request.Id);
        }

        tournament.UpdateName(request.Name);

        _repository.Update(tournament);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(tournament);
    }
}