

using API.CQRS.Queries;
using API.Data;
using API.Data.Entities;
using API.Data.Responses;
using API.Repositories;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.CQRS.Handlers;

public class GetAllTournamentsQueryHandler : IRequestHandler<GetAllTournamentsQuery, IEnumerable<Tournament>>
{
    private readonly IRepository<Tournament> _repository;

    public GetAllTournamentsQueryHandler(IRepository<Tournament> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Tournament>> Handle(GetAllTournamentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetQuery()
            .Include(t=>t.SubTournaments)
            .ToListAsync(cancellationToken);

        return result;
    }
}