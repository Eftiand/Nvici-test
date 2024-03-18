
namespace API.Helpers;

public record Error(string code, string description)
{
    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static readonly Error NullValue= new("Error.NullValue", "Null value was provided.");
    public static readonly Error NotFound = new("Error.NotFound", "The requested resource was not found.");

    public static implicit operator Result(Error error) => Result.Failure(error);
    public static implicit operator Error(string error) => new Error(error, error);

    public Result ToResult() =>  Result.Failure(this);
}