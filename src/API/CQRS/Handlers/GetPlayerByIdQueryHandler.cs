
using API.CQRS.Queries;
using API.Data.Entities;
using API.Repositories;
using MediatR;

namespace API.CQRS.Handlers;

public class GetPlayerByIdQueryHandler : IRequestHandler<GetPlayerByIdQuery, Player?>
{
    private readonly IRepository<Player> _repository;
    public GetPlayerByIdQueryHandler(IRepository<Player> repository)
    {
        _repository = repository;
    }

    public async Task<Player?> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.PlayerId, cancellationToken);
    }
}