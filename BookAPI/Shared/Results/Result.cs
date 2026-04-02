namespace Shared.Results;

public class Result
{
    public bool Success { get; init; }
    public Error? Error { get; init; }

    public static Result Ok() => new() { Success = true };

    public static Result Fail(string code, string message) => new()
    {
        Success = false,
        Error = new Error(code, message)
    };
}

public sealed class Result<T> : Result
{
    public T? Data { get; init; }
    
    public static Result<T> Ok(T data) => new() { Success = true, Data = data };

    public static new Result<T> Fail(string code, string message) => new()
    {
        Success = false,
        Error = new Error(code, message)
    };
}