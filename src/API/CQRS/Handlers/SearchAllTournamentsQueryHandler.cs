
using API.CQRS.Queries;
using API.Data.Entities;
using API.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.CQRS.Handlers;

public class SearchAllTournamentsQueryHandler : IRequestHandler<SearchAllTournamentsQuery, IEnumerable<Tournament>>
{
    private readonly IRepository<Tournament> _repository;

    public SearchAllTournamentsQueryHandler(IRepository<Tournament> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Tournament>> Handle(SearchAllTournamentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetQuery()
            .Include(t=>t.SubTournaments)
            .ToListAsync(cancellationToken);

        var filteredResult = result.Where(request.Predicate.Compile());

        return filteredResult;
    }
}