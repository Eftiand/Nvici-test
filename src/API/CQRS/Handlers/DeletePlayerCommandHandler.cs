
using API.CQRS.Commands;
using API.Data.Entities;
using API.Helpers;
using API.Helpers.Errors;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class DeletePlayerCommandHandler : IRequestHandler<DeletePlayerCommand, Result>
{
    private readonly IRepository<Player> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePlayerCommandHandler(IRepository<Player> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = await _repository.GetByIdAsync(request.PlayerId, cancellationToken);

        if(player is null)
        {
            return PlayerErrors.NotFound(request.PlayerId);
        }

        _repository.Delete(player);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}