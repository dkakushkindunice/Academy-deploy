namespace Kakushkin_NewsFeed.Common.Results;

public class Result
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public string[] Errors { get; init; } = [];

    public static Result Ok(string? message = "OK") =>
        new() { Success = true, Message = message };

    public static Result Fail(string errorMessage) =>
        new() { Success = false, Errors = [errorMessage] };

    public static Result Fail(string[] errors) =>
        new() { Success = false, Errors = errors };
}

public class Result<T> : Result
{
    public T? Data { get; init; }

    public static Result<T> Ok(T data, string? message = "OK") =>
        new() { Success = true, Data = data, Message = message };

    public static new Result<T> Fail(string errorMessage) =>
        new() { Success = false, Errors = [errorMessage] };

    public static new Result<T> Fail(string[] errors) =>
        new() { Success = false, Errors = errors };
}
