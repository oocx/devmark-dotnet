using System;
using System.Linq;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var runner = new ProcessRunner();
            var env = new EnvironmentInfo(runner);

            await env.InitAsync();

            var npmInstall = new BuildAngularAppBenchmark(env, runner);

            Console.WriteLine("Starte Benchmark...");
            Console.WriteLine("Temp Ordner: " + env.TempPath);
            Console.WriteLine("Npm Ordner:  " + env.PathToNpm);

            var result = await npmInstall.Run();

            var totalMs = result.Measures.Sum(m => m.durationInMs);
            Console.WriteLine($"Overal test duration: {totalMs:N0} ms");
        }
    }
}
