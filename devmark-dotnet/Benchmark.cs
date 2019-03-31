using System.Text;

namespace devmark_dotnet
{
    public class Benchmark
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Tool[] Tools { get; set; }
        public Step[] Steps { get; set; }
    }
}
