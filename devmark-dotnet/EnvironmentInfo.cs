using System;
using System.IO;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    class EnvironmentInfo
    {
        private readonly ProcessRunner _runner;

        public EnvironmentInfo(ProcessRunner runner)
        {
            this._runner = runner;
        }

        public Guid RunId { get; } = Guid.NewGuid();

        public string TempPath
        {
            get { return Path.Combine(Path.GetTempPath(), "DevMark", RunId.ToString()); }
        }

        public string GetTempPath(string directoryName)
        {
            var path = Path.Combine(TempPath, directoryName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        
        public string PathToNpm { get; private set; }

        public string PathToGit { get; set; }

        public async Task InitAsync()
        {
            PathToNpm = await _runner.ExecAndGetOutputAsync("where", "npm.cmd");
            PathToGit = await _runner.ExecAndGetOutputAsync("where", "git.exe");
        }

    }
}
