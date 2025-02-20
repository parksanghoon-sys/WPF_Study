using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace wpfIffUI.Control
{
    public class PropertyModel<T> : INotifyPropertyChanged, IDisposable
    {
        private bool _disposed = false;

        private bool _notifyEnabled = true;

        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        private Dictionary<string, List<PropertyChangedEventHandler>> _bindings = new Dictionary<string, List<PropertyChangedEventHandler>>();

        private Dictionary<string, object> _pendings = new Dictionary<string, object>();

        public Func<object> GetDefaultProvider = null;

        public bool NotifyEnabled
        {
            get
            {
                return _notifyEnabled;
            }
            set
            {
                if (value)
                {
                    _notifyEnabled = value;
                    lock (this)
                    {
                        foreach (KeyValuePair<string, object> pending in _pendings)
                        {
                            Notify(pending.Key);
                        }

                        _pendings.Clear();
                    }

                    Monitor.Exit(this);
                }
                else
                {
                    Monitor.Enter(this);
                    _notifyEnabled = value;
                }
            }
        }

        protected Dictionary<string, object> Properties => _properties;

        public T this[string name]
        {
            get
            {
                return GetValue(name);
            }
            set
            {
                if (!object.Equals(GetValue(name), value))
                {
                    SetValue(name, value);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyModel()
        {
            GetDefaultProvider = GetDefault;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor item in properties)
            {
                if (item.Name != "NotifyEnabled")
                {
                    _properties[item.Name] = GetDefaultProvider();
                }
            }
        }

        public PropertyModel(params string[] names)
            : this()
        {
            GetDefaultProvider = GetDefault;
            foreach (string key in names)
            {
                _properties[key] = GetDefaultProvider();
            }
        }

        public PropertyModel(List<string> names)
            : this()
        {
            GetDefaultProvider = GetDefault;
            foreach (string name in names)
            {
                _properties[name] = GetDefaultProvider();
            }
        }

        public PropertyModel(Func<object> getDefaultFunc)
        {
            GetDefaultProvider = getDefaultFunc;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor item in properties)
            {
                if (item.Name != "NotifyEnabled")
                {
                    _properties[item.Name] = GetDefaultProvider();
                }
            }
        }

        public PropertyModel(Func<object> getDefaultFunc, List<string> names)
            : this()
        {
            GetDefaultProvider = getDefaultFunc;
            foreach (string name in names)
            {
                _properties[name] = GetDefaultProvider();
            }
        }

        public PropertyModel(Func<object> getDefaultFunc, params string[] names)
            : this()
        {
            GetDefaultProvider = getDefaultFunc;
            foreach (string key in names)
            {
                _properties[key] = GetDefaultProvider();
            }
        }

        ~PropertyModel()
        {
            Dispose(disposing: false);
        }

        private object GetDefault()
        {
            return default(T);
        }

        protected void AddProperty(string name, object initValue)
        {
            _properties[name] = initValue;
        }

        public virtual void SetValue(string name, T value)
        {
            Properties[name] = value;
            Notify(name);
        }

        public virtual T GetValue(string name)
        {
            return (T)GetProperty(name);
        }

        protected object GetProperty(string name)
        {
            return Properties[name];
        }

        public Dictionary<string, object> GetAllProperties()
        {
            lock (this)
            {
                return new Dictionary<string, object>(_properties);
            }
        }

        public void Bind(PropertyChangedEventHandler handler, object obj)
        {
            List<string> list = new List<string>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor item in properties)
            {
                if (!item.IsReadOnly)
                {
                    list.Add(item.Name);
                }
            }

            Bind(handler, list);
        }

        public void Bind(PropertyChangedEventHandler handler, params string[] names)
        {
            Bind(handler, names.ToList());
        }

        public void Bind(PropertyChangedEventHandler handler, List<string> names)
        {
            lock (this)
            {
                Unbind(handler);
                foreach (string name in names)
                {
                    if (!_bindings.ContainsKey(name))
                    {
                        _bindings[name] = new List<PropertyChangedEventHandler>();
                    }

                    List<PropertyChangedEventHandler> list = _bindings[name];
                    if (!list.Contains(handler))
                    {
                        list.Add(handler);
                    }
                }
            }
        }

        public void Unbind(PropertyChangedEventHandler handler)
        {
            lock (this)
            {
                foreach (KeyValuePair<string, List<PropertyChangedEventHandler>> binding in _bindings)
                {
                    binding.Value.Remove(handler);
                }
            }
        }

        public void Unbind(object obj)
        {
            lock (this)
            {
                foreach (KeyValuePair<string, List<PropertyChangedEventHandler>> binding in _bindings)
                {
                    List<PropertyChangedEventHandler> value = binding.Value;
                    for (int i = 0; i < value.Count; i++)
                    {
                        if (value[i].Target == obj)
                        {
                            value.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        public void UnbindAll()
        {
            lock (this)
            {
                _bindings.Clear();
            }
        }

        protected void Notify(string name)
        {
            lock (this)
            {
                if (!NotifyEnabled)
                {
                    _pendings[name] = null;
                    return;
                }

                PropertyChangedEventArgs e = new PropertyChangedEventArgs(name);
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, e);
                }

                if (!_bindings.TryGetValue(name, out var value))
                {
                    return;
                }

                foreach (PropertyChangedEventHandler item in value)
                {
                    item(this, e);
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        UnbindAll();
                    }

                    this.PropertyChanged = null;
                    _bindings = null;
                    _properties = null;
                    _disposed = true;
                }
            }
        }
    }
}
