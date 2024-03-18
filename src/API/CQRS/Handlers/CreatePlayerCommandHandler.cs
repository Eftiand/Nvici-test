
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Result<Player>>
{
    private readonly IRepository<Player> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerCommandHandler(IRepository<Player> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Player>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var result = Player.Create(request.Name, request.Age);

        if(result.IsFailure)
            return result;

        await _repository.AddAsync(result.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}