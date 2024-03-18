
using API.Helpers;

namespace API.Data.Entities;

public class Player : Entity<int>
{
    private Player() : base()
    {
    }

    public string Name { get; private set; } = null!;
    public int Age { get; private set; } = 0;
    public List<Tournament> Tournaments { get; private set; } = new();

    public static Result<Player> Create(string name, int age)
    {
        var player = new Player();
        var nameResult = player.UpdateName(name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<Player>(nameResult.Errors);
        }

        var ageResult = player.UpdateAge(age);
        if (ageResult.IsFailure)
        {
            return Result.Failure<Player>(ageResult.Errors);
        }

        return player;
    }

    public Result UpdateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return Error.NullValue;

        Name = name;
        return Result.Success();
    }

    public Result UpdateAge(int age)
    {
        if (age < 0)
            return Result.Failure("GenericErrors.LessThanZero","Age cannot be less than 0");

        Age = age;
        return Result.Success();
    }

}