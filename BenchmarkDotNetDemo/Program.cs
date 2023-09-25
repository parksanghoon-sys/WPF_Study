using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class FactorialBenchmark
{
    [Benchmark]
    public int CalculateFactorial()
    {
        int number = 5;
        int result = 1;
        for (int i = 1; i <= number; i++)
        {
            result *= i;
        }
        return result;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<FactorialBenchmark>();
    }
}