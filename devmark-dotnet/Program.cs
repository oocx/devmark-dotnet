using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("devmark-dotnet [temp-path] list-of-benchmark-files");
                    return;
                }

                var processRunner = new ProcessRunner();
                var env = new EnvironmentInfo(processRunner);                

                if (!args[0].EndsWith(".json"))
                {
                    env.TempPath = Path.Combine(args[0], env.RunId.ToString());
                }
                else
                {
                    env.TempPath = Path.Combine(Path.GetTempPath(), "DevMark", env.RunId.ToString());
                }                                

                var benachmarkReader = new BenchmarkReader();
                var resultWriter = new ResultWriter(env);

                foreach (var benchmarkPath in args.Where(a => a.EndsWith(".json")))
                {
                    Console.WriteLine("staring benchmark " + benchmarkPath);
                    var benchmark = await benachmarkReader.ReadAsync(benchmarkPath);
                    var benchmarkRunner = new BenchmarkRunner(processRunner, env);
                    var result = await benchmarkRunner.RunAsync(benchmark);
                    var totalMs = result.Measures.Sum(m => m.Value);
                    Console.WriteLine($"Overal test duration: {totalMs:N0} ms");
                    resultWriter.Write(result);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}
