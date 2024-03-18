
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Result<Player>>
{
    private readonly IRepository<Player> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePlayerCommandHandler(IRepository<Player> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Player>> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if(player is null)
        {
            return PlayerErrors.NotFound(request.Id);
        }

        player.UpdateName(request.Name);
        player.UpdateAge(request.Age);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(player);
    }
}