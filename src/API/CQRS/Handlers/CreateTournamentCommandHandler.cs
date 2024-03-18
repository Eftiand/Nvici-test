
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class CreateTournamentCommandHandler : IRequestHandler<CreateTournamentCommand, Result<Tournament>>
{
    private readonly IRepository<Tournament> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTournamentCommandHandler(IRepository<Tournament> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    async Task<Result<Tournament>> IRequestHandler<CreateTournamentCommand, Result<Tournament>>.Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
    {
        var result = Tournament.Create(request.Name);

        if (result.IsFailure)
        {
            return Result.Failure<Tournament>(result.Errors);
        }

        await _repository.AddAsync(result.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}