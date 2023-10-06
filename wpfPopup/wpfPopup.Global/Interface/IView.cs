namespace wpfPopup.Local.Interface
{
    public interface IView
    {
    }
    public interface IMainView : IView
    {
        public bool? ShowPopupWindow();
    }
}
