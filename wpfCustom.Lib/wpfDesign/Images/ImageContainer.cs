using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;

namespace Design.Images
{
    public class ImageContainer
    {
        internal static ImageRoot _data;
        internal static Dictionary<string, ImageItem> _items;
        static ImageContainer()
        {
            Build();
        }

        private static void Build()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Design.Properties.Resources.images.yaml";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            StringReader r = new StringReader(reader.ReadToEnd());
            Deserializer deserializer = new();
            var yamlObject = deserializer.Deserialize<object>(r);

            JsonSerializer js = new();
            StringWriter w = new();
            js.Serialize(w,yamlObject);
            string jsonText = w.ToString();
            _data = JsonConvert.DeserializeObject<ImageRoot>(jsonText);
            _items = new();

            foreach (var item in _data.Items)
            {
                _items.Add(item.Name, item);
            }
        }
    }
}