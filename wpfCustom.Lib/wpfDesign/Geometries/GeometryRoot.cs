using Newtonsoft.Json;
using System.Collections.Generic;

namespace Design.Geometries
{
    internal class GeometryRoot
    {
        [JsonProperty("geometries")]
        public List<GeometryItem> Items { get; set; }
    }
}
