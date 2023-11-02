using System.Runtime.CompilerServices;

namespace Design.Geometries
{
    public static class GeometryConverter
    {
        public static string GetData([CallerMemberName] string name = null)
        {
            return GeometryContainer._items[name].Data;
        }
    }
}