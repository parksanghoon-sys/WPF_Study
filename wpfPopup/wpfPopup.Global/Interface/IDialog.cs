namespace wpfPopup.Global.Interface
{
    public interface IDialog
    {
        object DataContext { get; set; }
        void Show();
        bool? ShowDialog();
        void Close();
    }
    public interface IDialogService
    {
        IDialog Dialog { get; }
        void SetVM(ViewModelBase vm);
    }
}
