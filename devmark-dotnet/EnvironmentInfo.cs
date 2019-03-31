using System;
using System.IO;
using System.Management;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    internal class EnvironmentInfo
    {
        private readonly ProcessRunner _runner;

        public EnvironmentInfo(ProcessRunner runner)
        {
            this._runner = runner;
        }

        public Guid RunId { get; } = Guid.NewGuid();

        public string TempPath { get; set; }

        public string GetTempPath(string directoryName)
        {
            var path = Path.Combine(TempPath, directoryName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public int GetPhysicalMemory()
        {
            var mc = new ManagementClass("Win32_ComputerSystem");
            var moc = mc.GetInstances();
            foreach (ManagementObject item in moc)
            {
                return (int)(Convert.ToInt64(item.Properties["TotalPhysicalMemory"].Value) / (1024 * 1024));
            }

            return 0;
        }

    }
}
