using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirclularGage.Main
{
    public class MessengerKey
    {
        public string Recipient { get; set; }
        public Type MessageType { get; set; }
        public MessengerKey( Type type, string target ) 
        {
            Recipient = target;
            MessageType = type;
        }
        public override bool Equals(object obj)
        {
            var other = obj as MessengerKey;
            return other != null && this.MessageType == other.MessageType && this.Recipient == other.Recipient;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (MessageType?.GetHashCode() ?? 0);
                hash = hash * 23 + (Recipient?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }


    public class Messenger
    {
        private static readonly Dictionary<MessengerKey, List<Action<object>>> _messageHandlers = new Dictionary<MessengerKey, List<Action<object>>>();
        public static void Register<T>(string targetRecipient, Action<T> action)
        {
            var key = new MessengerKey(typeof(T), targetRecipient);
            if (!_messageHandlers.ContainsKey(key)) 
            {
                _messageHandlers.Add(key, new List<Action<object>>());
            }
            _messageHandlers[key].Add(obj => action((T)obj));
        }
        //public static void Send<T>(T message)
        //{
        //    if (_messageHandlers.ContainsKey(typeof(T)))
        //    {
        //        foreach (var handler in _messageHandlers[typeof(T)])
        //        {
        //            handler(message);
        //        }
        //    }
        //}
        public static void Send<T>(string targetRecipient, T message)
        {
            var key = new MessengerKey(typeof(T), targetRecipient);
            if (_messageHandlers.ContainsKey(key))
            {
                foreach (var handler in _messageHandlers[key])
                {
                    handler(message);
                }
            }
        }
        public static void UnRegister<T>(string recipient)
        {
            var keysToRemove = _messageHandlers.Keys.Where(k => k.Recipient == recipient && k.MessageType == typeof(T)).ToList();
            foreach (var key in keysToRemove)
            {
                _messageHandlers.Remove(key);
            }
        }
    }
}
