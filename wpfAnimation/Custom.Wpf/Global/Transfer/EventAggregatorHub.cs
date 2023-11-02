using Prism.Events;
using System;
using System.Diagnostics;

namespace Custom.Wpf.Global.Event
{
    public class EventAggregatorHub : IEventHub
    {
        private IEventAggregator _eventAggregator;
        public EventAggregatorHub(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public Action<StackTrace> Publising { get; set; }

        public void Publish<T1, T2>(T2 value) where T1 : PubSubEvent<T2>, new()
        {
            StackTrace stackTrace = new StackTrace();

            Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
            Publising?.Invoke(stackTrace);
            _eventAggregator.GetEvent<T1>().Publish(value);
        }

        public void Subscribe<T1, T2>(Action<T2> action) where T1 : PubSubEvent<T2>, new()
        {
            _eventAggregator.GetEvent<T1>().Subscribe(action);

        }
    }
}