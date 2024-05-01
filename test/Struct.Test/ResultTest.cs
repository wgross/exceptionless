namespace Struct.Test;

public class ResultTest
{
    [Fact]
    public void Return_Ok()
    {
        Result<int> func() => Result.Ok(1);

        var result = func();

        Assert.True(result is { HasValue: true, Value: 1 });
    }

    [Fact]
    public void Return_error()
    {
        Result<int> func() => Result.Error<int>(new Exception("fail"));

        var result = func();

        Assert.False(result.HasValue);
        Assert.True(result is Error<int> { Reason: { Message: "fail" } });
    }

    [Fact]
    public void Throws_error_if_unchecked()
    {
        Result<int> func() => Result.Error<int>(new Exception("fail"));

        var result = func();

        try
        {
            var _ = result.Value;

            Assert.Fail("hasn't thrown");
        }
        catch (Exception ex)
        {
            Assert.Equal("Invalid access to Value", ex.Message);
            Assert.Equal("fail", ex.InnerException!.Message);
        }
    }
}