using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace RefStruct;

public struct Result
{
    public static Result<T> Ok<T>(T value) => new Result<T>(true, value, null);

    public static Result<T> Error<T>(
        Exception reason,
        [CallerMemberName] string callerMemberName = "",
        [CallerLineNumber] int callerLineNumber = 0)
    {
        reason.Data[nameof(CallerMemberNameAttribute)] = callerMemberName;
        reason.Data[nameof(CallerLineNumberAttribute)] = callerLineNumber;
        reason.Data[nameof(Environment.StackTrace)] = Environment.StackTrace.ToString();

        return new Result<T>(false, default!, reason);
    }
}

public readonly ref struct Result<T>(bool hasValue, T value, Exception error)
{
    public bool HasValue { get; } = hasValue;

    public Exception Reason { get; } = error;

    public T Value => this.HasValue ? value : throw new InvalidOperationException("Invalid access to Value", this.Reason);

    [Pure]
    public static implicit operator Result<T>(T i) => Result.Ok(i);

    [Pure]
    public static implicit operator Result<T>(Exception ex) => Result.Error<T>(ex);
}