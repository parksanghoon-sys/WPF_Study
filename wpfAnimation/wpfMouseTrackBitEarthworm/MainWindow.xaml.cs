using System.Windows;

namespace wpfMouseTrackBitEarthworm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 900; i++)
            {
                this.aa.Items.Add("");
            }
        }
    }
}
