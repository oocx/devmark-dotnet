using System.IO;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    class BuildAngularAppBenchmark
    {
        private readonly EnvironmentInfo _env;
        private readonly ProcessRunner _runner;

        public BuildAngularAppBenchmark(EnvironmentInfo env, ProcessRunner runner)
        {
            this._env = env;
            this._runner = runner;            
        }

        public async Task<BenchmarkResult> Run()
        {
            var result = new BenchmarkResult();
            var tmp = _env.GetTempPath("NodeInstall");
            var npm = _env.PathToNpm;
            var git = _env.PathToGit;
            var app = Path.Combine(tmp, "angular7-example-app");

            await Run(git, "clone https://github.com/oocx/angular7-example-app.git", tmp);                        
            await Run(npm, "install", app);
            await Run(npm, "run build:prod:en", app);
            await Run(npm, "run build:server:prod", app);
            await Run(npm, "run build:ssr", app);
            await Run(npm, "run lint", app);
            await Run(npm, "run test", app);
            await Run(npm, "run e2e", app);

            return result;            

            async Task Run(string filePath, string arguments, string workingDirectory)
            {
                result.AddMeasure(filePath + " " + arguments, await _runner.ExecAndMeasureAsync(filePath, arguments, workingDirectory));
            }
        }
        
    }
}
