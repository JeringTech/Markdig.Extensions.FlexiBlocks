using BenchmarkDotNet.Running;

namespace Jering.Markdig.Extensions.FlexiBlocks.Performance
{
    public class Program
    {
        public static void Main(string[] _)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}
