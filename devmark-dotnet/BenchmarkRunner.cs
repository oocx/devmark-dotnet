using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    public class BenchmarkRunner
    {
        private readonly ProcessRunner _processRunner;
        private readonly EnvironmentInfo _env;

        internal BenchmarkRunner(ProcessRunner processRunner, EnvironmentInfo env)
        {
            this._processRunner = processRunner;
            this._env = env;
        }

        public BenchmarkRunner()
        {
        }

        internal async Task<BenchmarkResult> RunAsync(Benchmark benchmark)
        {
            Console.WriteLine("Executing Benchmark " + benchmark.Id);
            Console.WriteLine(benchmark.Description);

            var tempPath = _env.GetTempPath(benchmark.Id);
            var result = InitResult(benchmark);
            var tools = await GetTools(benchmark);

            await ExecuteSteps(benchmark, tempPath, result, tools);

            return result;
        }

        private BenchmarkResult InitResult(Benchmark benchmark)
        {
            var result = new BenchmarkResult(benchmark.Id, _env.RunId);
            result.MachineName = Environment.MachineName;
            result.StartedAt = DateTime.Now;
            result.EnvironmentInfo.Add("cpu_count", Environment.ProcessorCount.ToString());
            result.EnvironmentInfo.Add("os_version", Environment.OSVersion.ToString());
            result.EnvironmentInfo.Add("total_physical_memory", _env.GetPhysicalMemory().ToString());

            Console.WriteLine($"{result.MachineName}, {result.StartedAt.ToShortDateString()} {result.StartedAt.ToShortTimeString()}");
            foreach (var info in result.EnvironmentInfo)
            {
                Console.WriteLine($"{info.Key}:\t {info.Value}");
            }

            return result;
        }

        private async Task ExecuteSteps(Benchmark benchmark, string tempPath, BenchmarkResult result, Dictionary<string, string> tools)
        {
            foreach (var step in benchmark.Steps)
            {                
                var workingDirectory = step.Path != null ? Path.Combine(tempPath, step.Path) : tempPath;
                var command = tools.ContainsKey(step.Command) ? tools[step.Command] : Path.Combine(workingDirectory, step.Command);
                var durationInMs = await _processRunner.ExecAndMeasureAsync(command, step.Arguments, workingDirectory, s => result.AddOutputData(step.Id, s));
                result.Measures.Add(step.Id, durationInMs);
            }
        }

        private async Task<Dictionary<string, string>> GetTools(Benchmark benchmark)
        {
            var tools = new Dictionary<string, string>();
            foreach (var tool in benchmark.Tools)
            {
                var toolPath = await ResolveToolAsync(tool.Executable);
                tools.Add(tool.Id, toolPath);
                var version = await _processRunner.ExecAndGetOutputAsync(toolPath, tool.GetVersionCommand);
                Console.WriteLine($"{tool.Id}: \t{version}");
            }

            return tools;
        }

        private async Task<string> ResolveToolAsync(string executable)
        {
            return await _processRunner.ExecAndGetOutputAsync("where", executable);
        }
    }
}
