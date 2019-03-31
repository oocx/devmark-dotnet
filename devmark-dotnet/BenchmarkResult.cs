using System;
using System.Collections.Generic;
using System.Text;

namespace devmark_dotnet
{
    class BenchmarkResult
    {
        public string BenchmarkId { get; private set; }

        public Guid RunId { get; private set; }

        public DateTime StartedAt { get; set; }

        public string MachineName { get; set; }

        public Dictionary<string, string> EnvironmentInfo { get; } = new Dictionary<string, string>();

        public Dictionary<string, long> Measures { get; } = new Dictionary<string, long>();

        public Dictionary<string, StringBuilder> Output { get; } = new Dictionary<string, StringBuilder>();
     
        public BenchmarkResult(string id, Guid runId)
        {
            this.BenchmarkId = id;
            this.RunId = runId;
        }

        private object _outputDataLock = new object();
        
        public void AddOutputData(string stepId, string data)
        {
            lock (_outputDataLock)
            {
                if (!Output.ContainsKey(stepId)) {
                    Output[stepId] = new StringBuilder();
                }

                Output[stepId].AppendLine(data);
            }
        }
    }
}
