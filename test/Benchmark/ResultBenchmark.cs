using BenchmarkDotNet.Attributes;

namespace Benchmark;

public class ResultBenchmark
{
    private static int ReturnOne() => 1;

    private static RefStruct.Result<int> ReturnRefStructOne() => RefStruct.Result.Ok(1);

    private static Struct.Result<int> ReturnStructOne() => Struct.Result.Ok(1);

    private static Class.Result<int> ReturnClassOne() => Class.Result.Ok(1);

    [Benchmark]
    public void Measure_ReturnOne()
    {
        var result = ReturnOne();
    }

    [Benchmark]
    public void Measure_ReturnRefStructOne()
    {
        var result = ReturnRefStructOne();
    }

    [Benchmark]
    public void Measure_ReturnStructOne()
    {
        var result = ReturnStructOne();
    }

    [Benchmark]
    public void Measure_ReturnClassOne()
    {
        var result = ReturnClassOne();
    }
}