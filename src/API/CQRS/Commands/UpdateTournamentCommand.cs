
using System.Text.Json.Serialization;
using API.Data.Entities;
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public class UpdateTournamentCommand : IRequest<Result<Tournament>>
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; }

    public UpdateTournamentCommand(string name)
    {
        Name = name;
    }
}