using System.Collections.Generic;

namespace devmark_dotnet
{
    class BenchmarkResult
    {
        public List<(string testName, long durationInMs)> Measures { get; private set; } = new List<(string testName, long durationInMs)>();
        internal void AddMeasure(string testName, long durationInMs)
        {
            Measures.Add((testName, durationInMs));
        }
    }
}
