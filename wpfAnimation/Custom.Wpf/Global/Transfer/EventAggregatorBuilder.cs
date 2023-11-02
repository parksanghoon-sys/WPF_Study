using Prism.Events;

namespace Custom.Wpf.Global.Event
{
    public class EventAggregatorBuilder
    {
        public static IEventHub Get(IEventAggregator ea) => new EventAggregatorHub(ea);
    }
}
