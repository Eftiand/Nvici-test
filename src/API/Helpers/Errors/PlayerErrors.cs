
namespace API.Helpers.Errors;

public record PlayerErrors()
{
    public static Error NotFound(int PlayerId) =>  new($"Players.NotFound", $"Tournament with id {PlayerId} not found.");
    public static Error NotRegisteredInParentTournament(int PlayerId, int ParentTournament) =>  new("Players.NotRegisteredInParentTournament", $"Player with id {PlayerId} is not registered in parent tournament with id {ParentTournament}");

}