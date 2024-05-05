using BenchmarkDotNet.Attributes;
using Class;

namespace Benchmark;

public class ResultEvaluationBenchmark
{
    private static int ReturnOne() => 1;

    private static DotNext.Result<int> ReturnOneDotNext() => 1;

    private static RefStruct.Result<int> ReturnRefStructOne() => 1;

    private static Struct.Result<int> ReturnStructOne() => Struct.Result.Ok(1);

    private static Class.Result<int> ReturnClassOne() => Class.Result.Ok(1);

    [Benchmark]
    public void Measure_ReturnOne()
    {
        var value = 0;

        var result = ReturnOne();
        if (result == 1)
            value = result;
    }

    [Benchmark]
    public void Measure_ReturnOneDotNext()
    {
        var value = 0;

        var result = ReturnOneDotNext();
        if (result.IsSuccessful)
            value = result.Value;
    }

    [Benchmark]
    public void Measure_ReturnRefStructOne()
    {
        var value = 0;

        var result = ReturnRefStructOne();
        if (result.HasValue)
            value = result.Value;
    }

    [Benchmark]
    public void Measure_ReturnStructOne()
    {
        var value = 0;

        var result = ReturnStructOne();
        if (result.HasValue)
            value = result.Value;
    }

    [Benchmark]
    public void Measure_ReturnClassOne()
    {
        var value = 0;

        var result = ReturnClassOne();
        if (result.HasValue)
            value = result.Value;
    }
}