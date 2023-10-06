using CommunityToolkit.Mvvm.ComponentModel;
using wpfPopup.Local.Interface;

namespace wpfPopup.Global
{
    public abstract class ViewModelBase : ObservableObject
    {
        private readonly IView _view;
        public ViewModelBase()
        {
            
        }
        public ViewModelBase(IView view)
        {
            _view = view;
        }
        public IView View { get; private set; }
    }
    public abstract class ViewModelBase<T> : ObservableObject 
        where T : IView
    {
        public T View { get; private set; }
        public ViewModelBase(T view)
        {
            View = view;
        }
    }
}