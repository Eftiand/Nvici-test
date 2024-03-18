
namespace API.Helpers.Errors;

public record TournamentErrors()
{
    public static Error NotFound(int TournamentId) =>  new($"Tournaments.NotFound", $"Tournament with id {TournamentId} not found.");
    public static Error NotFoundInTournament(int TournamentId, int ParentId) =>  new($"Tournaments.NotFoundInTournament", $"Tournament with id {TournamentId} not found inside of parent tournament with id {ParentId}");
    public static Error DepthTooBig() =>  new("Tournaments.DepthTooBig", "Tournament tree is too deep");
    public static Error PlayerAlreadyInTournament(int PlayerId, int TournamentId) =>  new("Tournaments.PlayerAlreadyInTournament", $"Player with id {PlayerId} is already in tournament with id {TournamentId}");

}