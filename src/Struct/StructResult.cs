namespace Struct;

public interface Result
{
    public static Ok<T> Ok<T>(T value) => new Ok<T> { Value = value };

    public static Error<T> Error<T>(Exception reason) => new Error<T> { Reason = reason };
}

public interface Result<T> : Result
{
    public static Ok<T> Ok(T value) => new Ok<T> { Value = value };

    public T Value { get; }

    /// <summary>
    /// Use to inspect OK or error with conversion
    /// </summary>
    public bool HasValue => this is Ok<T>;
}

public record struct Ok<T> : Result<T>
{
    public T Value { get; init; }
}

public record struct Error<T> : Result<T>
{
    public Exception Reason { get; init; }

    public T Value => throw new InvalidOperationException("Invalid access to Value", this.Reason);
}