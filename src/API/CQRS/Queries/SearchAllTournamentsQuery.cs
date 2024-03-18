
using System.Linq.Expressions;
using API.Data.Entities;
using MediatR;

namespace API.CQRS.Queries;

public record SearchAllTournamentsQuery(Expression<Func<Tournament, bool>> Predicate) : IRequest<IEnumerable<Tournament>>;
