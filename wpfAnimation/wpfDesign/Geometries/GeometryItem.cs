using Newtonsoft.Json;

namespace Design.Geometries
{
    internal class GeometryItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
