using Newtonsoft.Json;

namespace Design.Images
{
    public class ImageItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}