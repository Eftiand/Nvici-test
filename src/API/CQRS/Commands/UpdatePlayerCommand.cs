
using System.Text.Json.Serialization;
using API.Data.Entities;
using API.Helpers;
using MediatR;

namespace API.CQRS.Commands;

public class UpdatePlayerCommand : IRequest<Result<Player>>
{
    [JsonIgnore]
    public int Id { get; set;}
    public string Name { get; }
    public int Age { get; }

    public UpdatePlayerCommand(string name, int age)
    {
        Name = name;
        Age = age;
    }
}