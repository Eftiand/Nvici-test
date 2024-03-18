
using API.CQRS.Queries;
using API.Data.Entities;
using API.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.CQRS.Handlers;

public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<Player>>
{
    private readonly IRepository<Player> _repository;

    public GetAllPlayersQueryHandler(IRepository<Player> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Player>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}