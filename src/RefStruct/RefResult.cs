namespace RefStruct;

public struct Result
{
    public static Result<T> Ok<T>(T value) => new Result<T>(true, value, null);

    public static Result<T> Error<T>(Exception error) => new Result<T>(false, default!, error);
}

public readonly ref struct Result<T>(bool success, T value, Exception error)
{
    public bool HasValue { get; } = success;

    public Exception Reason { get; } = error;

    public T Value => this.HasValue ? value : throw new InvalidOperationException("Invalid access to Value", this.Reason);
}