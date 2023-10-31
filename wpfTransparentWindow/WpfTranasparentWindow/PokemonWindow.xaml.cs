using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using WpfTranasparentWindow.Enums;
using WpfTranasparentWindow.Services;
using WpfTranasparentWindow.Utils;

namespace WpfTranasparentWindow
{
    /// <summary>
    /// PokemonWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PokemonWindow : Window
    {
        public PokemonWindow()
        {
            InitializeComponent();
            this.TargetUpdated += this.PokemonWindow_TargetUpdated;
            this.MouseMove += this.Window_MouseMove;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
            this.DragMove();
        }
     

        private void PokemonWindow_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if(this.Topmost)
                this.Activate();
        }

        public void SetPokemon(PokemonType type)
        {
            ImageHelper.SetImageForImage(this.xPokemon, $"/WpfTranasparentWindow;component/Images/{type.ToString().Remove(0, 1)}.png", false, 390);
        }

        public void SetWindowExTransparent()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
        public void Dispose()
        {
            this.TargetUpdated -= this.PokemonWindow_TargetUpdated;
        }
    }
}
