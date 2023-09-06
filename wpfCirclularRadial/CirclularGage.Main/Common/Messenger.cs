using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclularGage.Main
{
    public class Messenger
    {
        private static readonly Dictionary<Type, List<Action<object>>> _messageHandlers = new Dictionary<Type, List<Action<object>>>();
        public static void Register<T>(object recipient, Action<T> action)
        {
            if(!_messageHandlers.ContainsKey(typeof(T))) 
            {
                _messageHandlers.Add(typeof(T), new List<Action<object>>());
            }
            _messageHandlers[typeof(T)].Add(obj => action((T)obj));
        }
        public static void Send<T>(T message)
        {
            if (_messageHandlers.ContainsKey(typeof(T)))
            {
                foreach (var handler in _messageHandlers[typeof(T)])
                {
                    handler(message);
                }
            }
        }
        public static void UnRegister<T>(object recipient) 
        {
            if(_messageHandlers.ContainsKey(typeof(T)))
            {
                _messageHandlers[typeof(T)].RemoveAll(x => x.Target == recipient);
            }
        }
    }
}
