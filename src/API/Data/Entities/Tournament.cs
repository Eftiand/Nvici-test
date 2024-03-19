
using API.Helpers;
using API.Helpers.Errors;

namespace API.Data.Entities;

public sealed class Tournament : Entity<int>
{
    public string Name { get; private set; } = null!;
    public int? ParentId { get; private set; }
    public Tournament? Parent { get; private set; }
    public List<Tournament> SubTournaments { get; private set; } = new();
    public List<Player> Players { get; private set; } = new();

    private Tournament()
    {
    }

    public static Result<Tournament> Create(string name)
    {
        var tournament = new Tournament();
        var nameResult = tournament.UpdateName(name);

        if (nameResult.IsFailure)
        {
            return Result.Failure<Tournament>(nameResult.Errors);
        }

        return tournament;
    }

    public Result AddPlayer(Player player)
    {
        if (player is null)
            return Error.NullValue;

        if (Players.Contains(player))
            return TournamentErrors.PlayerAlreadyInTournament(player.Id, Id);

        Players.Add(player);
        return Result.Success();
    }

    public Result AddSubTournament(Tournament tournament)
    {
        var depth = CalculateDepth();
        if (depth >= 5)
        {
            return TournamentErrors.DepthTooBig();
        }

        SubTournaments.Add(tournament);
        return Result.Success();
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
    }

    public Result RemoveSubTournament(Tournament tournament)
    {
        if (!SubTournaments.Contains(tournament))
            return TournamentErrors.NotFoundInTournament(tournament.Id, Id);

        SubTournaments.Remove(tournament);
        return Result.Success();
    }

    public Result UpdateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Error.NullValue;

        Name = name;
        return Result.Success();
    }

    private int CalculateDepth()
    {
        if (ParentId == null)
        {
            return 1;
        }

        return 1 + Parent!.CalculateDepth();
    }

}