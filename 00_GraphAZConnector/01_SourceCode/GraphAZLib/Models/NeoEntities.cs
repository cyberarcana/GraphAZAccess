using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphAZLib.Models
{
    public class NodeEntity
    {        
        public int Id { get; set; } = 0;

        public virtual string Type { get; set; } = "node";

        [JsonConverter(typeof(RawJsonConverter))]
        public string Labels { get; set; } = "";

        [JsonConverter(typeof(RawJsonConverter))]
        public string Properties { get; set; } = "";
    }

    public class RelationshipEntity : NodeEntity
    {      
        public override string Type { get; set; } = "relationship";
        public string Label { get; set; } = "";

        [JsonConverter(typeof(RawJsonConverter))]
        public string Start { get; set; } = "";

        [JsonConverter(typeof(RawJsonConverter))]
        public string End { get; set; } = "";
    }

    public class RawJsonConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.TokenType == JsonToken.StartObject)
            {
                return JObject.Load(reader).ToString();
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                return JArray.Load(reader).ToString();
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue((string)value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
