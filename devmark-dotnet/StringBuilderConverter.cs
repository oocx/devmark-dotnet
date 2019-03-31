using Newtonsoft.Json;
using System;
using System.Text;

namespace devmark_dotnet
{
    class StringBuilderConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(StringBuilder);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) { writer.WriteValue(string.Empty); };
            writer.WriteValue(value.ToString());
        }
    }
}
