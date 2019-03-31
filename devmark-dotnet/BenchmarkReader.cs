using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace devmark_dotnet
{
    public class BenchmarkReader
    {
        public async Task<Benchmark> ReadAsync(string fileName)
        {
            try
            {
                using (var reader = File.OpenText(fileName))
                {
                    var config = await JObject.ReadFromAsync(new JsonTextReader(reader));
                    var benchmark = new Benchmark
                    {
                        Id = config.Value<string>("id"),
                        Description = config.Value<string>("description"),
                        Tools = config.SelectTokens("tools.*").Select(ReadTool).ToArray(),
                        Steps = config.SelectTokens("steps.*").Select(ReadStep).ToArray()
                    };
                    return benchmark;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error reading Benachmark configuration file " + fileName);
                Console.Error.WriteLine(ex.ToString());
                throw;
            }
     
        }

        private Tool ReadTool(JToken token)
        {
            return new Tool()
            {
                Id = ((JProperty)token.Parent).Name,
                Executable = token.Value<string>("executable"),
                GetVersionCommand = token.Value<string>("getVersionCommand")
            };
        }

        private Step ReadStep(JToken token)
        {
            return new Step()
            {
                Id = ((JProperty)token.Parent).Name,
                Description = token.Value<string>("description"),
                Command = token.Value<string>("command"),
                Arguments = token.Value<string>("arguments"),
                Path = token.Value<string>("path"),
            };
        }
    }
}
