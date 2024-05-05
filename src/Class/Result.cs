using System.Diagnostics.Contracts;

namespace Class;

public interface Result
{
    public static Ok<T> Ok<T>(T value) => new Ok<T>(value);

    public static Error<T> Error<T>(Exception reason) => new Error<T>(reason);
}

public interface Result<T> : Result
{
    public static Ok<T> Ok(T value) => new Ok<T>(value);

    public T Value { get; }

    public bool HasValue => this is Ok<T>;
}

public record class Ok<T>(T Value) : Result<T>;

public record class Error<T>(Exception Reason) : Result<T>
{
    public T Value => throw new InvalidOperationException("Invalid access to Value", this.Reason);
}