
using API.CQRS.Queries;
using API.Data.Entities;
using API.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.CQRS.Handlers;

public class GetTournamentByIdQueryHandler : IRequestHandler<GetTournamentByIdQuery, Tournament?>
{
    private readonly IRepository<Tournament> _repository;

    public GetTournamentByIdQueryHandler(IRepository<Tournament> repository)
    {
        _repository = repository;
    }

    public async Task<Tournament?> Handle(GetTournamentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetQuery().ToListAsync(cancellationToken);

        return result.Find(t => t.Id == request.TournamentId);
    }
}