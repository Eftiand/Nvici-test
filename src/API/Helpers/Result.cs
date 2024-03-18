
namespace API.Helpers;

public class Result
{

    protected Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Invalid error");
        }

        IsSuccess = isSuccess;
        Errors = new List<Error> { error };
    }

    protected Result(bool isSuccess, List<Error> errors)
    {
        if(isSuccess && errors.Any() || !isSuccess && !errors.Any())
        {
            throw new InvalidOperationException("Invalid error");
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public List<Error> Errors { get; }

    public static Result Success() => new(true, Error.None);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result Failure(string code, string description) => new(false, new Error(code, description));
    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);
    public static Result<TValue> Failure<TValue>(string code, string description) => new(default!, false, new Error(code, description));
    public static Result<TValue> Failure<TValue>(List<Error> errors) => new(default!, false, errors);
}

public class Result<TValue> : Result
{
    protected internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        Value = value;
    }

    protected internal Result(TValue value, bool isSuccess, List<Error> error) : base(isSuccess, error)
    {
        Value = value;
    }


    public TValue Value { get; }

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);

}