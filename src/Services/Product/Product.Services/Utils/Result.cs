namespace Product.Services.Utils;

public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, string? error)
    {
        switch (isSuccess)
        {
            case true when error != null:
                throw new InvalidOperationException();
            case false when error == null:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    public static Result Success() => new(true, null);
    public static Result<T> Success<T>(T value) => new(value, true, null);
    public static Result Failure(string error) => new(false, error);
    public static Result<T> Failure<T>(string error) => new(default, false, error);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(T? value, bool isSuccess, string? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T? value) => new(value, true, null);
}