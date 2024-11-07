using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfDatagridChange;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ObservableCollection<IPayLoad> Datas { get;  set; }
    public Dictionary<int, List<IPayLoad>> pairs = new Dictionary<int, List<IPayLoad>>
    {
        {1, new List<IPayLoad>() { new Mode1Payload { Data ="1" ,DateTime ="1", Discription = "1"},{ new Mode1Payload { Data = "1", DateTime = "1", Discription = "1" } }, { new Mode1Payload { Data = "1", DateTime = "1", Discription = "1" } } } },
        {2, new List<IPayLoad>() { new Mode2Payload { Data ="2" ,DateTime ="2", Discription = "2"} } },
    };
    private int messageIndex = 0;
    public MainWindow()
    {
        InitializeComponent();
        this .DataContext = this;
        Datas = new ObservableCollection<IPayLoad>();        
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
       Random random = new Random();
        var data = random.Next(10, 100);

        Datas[0].Discription = data.ToString();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        Datas.RemoveAt(0);
        Datas.Clear();
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        messageIndex++;
        this.txtMsgId.Text = messageIndex.ToString();
        foreach (var item in pairs[messageIndex])
        {
            Datas.Add(item);
        }
        if(messageIndex == 2)
        {
            messageIndex = 0;
        }
        
    }
}
public class Mode1Payload : NotifyObject, IPayLoad
{
    private string _discription;
    public string Discription { get => _discription; set { _discription = value; OnPropertyChanged(); } }
    public string Data { get; set; }
    public string DateTime { get; set; }
    
}

public class Mode2Payload : IPayLoad
{
    public string Discription { get; set; }
    public string Data { get; set; }
    public string DateTime { get; set; }



}
public interface IPayLoad 
{
    string Discription { get; set; }
    string Data { get; set; }
    string DateTime{ get; set; }
}
public class NotifyObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}