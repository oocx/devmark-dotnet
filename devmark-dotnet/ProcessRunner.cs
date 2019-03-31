using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    internal class ProcessRunner
    {
        public async Task<long> ExecAndMeasureAsync(string fileName, string arguments, string workingDirectory, Action<string> outputCallback)
        {
            Console.Write(fileName + " " + arguments); ;

            var tcs = new TaskCompletionSource<long>();

            var watch = new Stopwatch();
            watch.Start();

            var processInfo = new ProcessStartInfo(fileName, arguments)
            {

            };

            await Task.Run(() =>
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo(fileName, arguments)
                    {
                        WorkingDirectory = workingDirectory,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    },
                    EnableRaisingEvents = true
                };

                process.OutputDataReceived += (sender, args) => outputCallback(args.Data);
                process.ErrorDataReceived += (sender, args) => outputCallback(args.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();
                watch.Stop();

                Console.WriteLine($" \t{watch.ElapsedMilliseconds:N0} ms");

                tcs.TrySetResult(watch.ElapsedMilliseconds);
            });

            return await tcs.Task;
        }

        public async Task<string> ExecAndGetOutputAsync(string fileName, string arguments)
        {
            var tcs = new TaskCompletionSource<string>();

            var processInfo = new ProcessStartInfo(fileName, arguments)
            {
                RedirectStandardOutput = true,
            };

            await Task.Run(() =>
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo(fileName, arguments)
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = false,
                        UseShellExecute = false
                    },
                    EnableRaisingEvents = true
                };

                var waitForData = new ManualResetEvent(false);

                process.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args?.Data))
                    {
                        tcs.TrySetResult(args.Data.Split(Environment.NewLine)[0]);
                        waitForData.Set();
                    }
                };
                process.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args?.Data)) throw new Exception(args.Data);
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                waitForData.WaitOne();


            });

            return await tcs.Task;
        }
    }
}
