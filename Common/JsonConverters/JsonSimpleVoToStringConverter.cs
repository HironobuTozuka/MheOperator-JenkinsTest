using System;
using Newtonsoft.Json;

namespace Common.JsonConverters
{
    public class JsonSimpleVoToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.GetConstructor(new Type[1]{typeof(string)}) != null;
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Activator.CreateInstance(objectType, reader.Value);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(Convert.ToString(value));
        }
        
    }
}