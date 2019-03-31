using Newtonsoft.Json;
using System;
using System.IO;

namespace devmark_dotnet
{
    class ResultWriter
    {
        private readonly EnvironmentInfo _env;

        public ResultWriter(EnvironmentInfo env)
        {
            this._env = env;
        }

        public void Write(BenchmarkResult result)
        {
            var path = Path.Combine(_env.TempPath, result.BenchmarkId) + ".json";

            Console.WriteLine("Writing results to " + path);

            using (var streamWriter = new StreamWriter(path))
            using (var textWriter = new JsonTextWriter(streamWriter))
            {
                textWriter.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer();
                serializer.Converters.Add(new StringBuilderConverter());
                serializer.Serialize(textWriter, result);
            }            

        }
    }
}
