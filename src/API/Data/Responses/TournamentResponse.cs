

namespace API.Data.Responses;

public record TournamentResponse(int Id, string Name, List<PlayerResponse> Players, List<TournamentResponse> SubTournaments);