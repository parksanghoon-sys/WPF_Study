using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Custom.Wpf.Global.Controls;
using Custom.Wpf.Global.Mvvm;
using System.Collections.Generic;
using wpfSiderBarMenu.Local.ViewModels;

namespace wpfSiderBarMenu
{
    public class DataModel
    {
        public IconType IconType { get; set; }
        public string Name { get; set; }

        public DataModel(IconType type, string name)
        {
            this.IconType = type;
            this.Name = name;
        }
    }
    public partial class MainViewModel : ObservableBase
    {
        [ObservableProperty] List<DataModel> _sampleDatas;
        [ObservableProperty] string _contentName;
        [ObservableProperty] object _contentobj;

        public MainViewModel()
        {
            this.SampleDatas = new List<DataModel>();
            this.SampleDatas.Add(new DataModel(IconType.Home, "Home"));
            this.SampleDatas.Add(new DataModel(IconType.Account, "Profile"));
            this.SampleDatas.Add(new DataModel(IconType.Comment, "Messages"));
            this.SampleDatas.Add(new DataModel(IconType.Cog, "Settings"));
            this.SampleDatas.Add(new DataModel(IconType.Youtube, "Help"));
            this.SampleDatas.Add(new DataModel(IconType.Security, "Password"));
            this.SampleDatas.Add(new DataModel(IconType.WhiteBalanceSunny, "Sign Out"));
        }
        [RelayCommand]
        private void TabItemSelected(DataModel model)
        {
            //this.ContentName = model.Name;
            
            switch (model.IconType)
            {
                case IconType.Home:
                    {

                    Contentobj = new Navigation1ViewModel();
                    }
                    break;
                case IconType.Account:
                    {
                        Contentobj = new Navigation2ViewModel();
                    }
                    
                    break;
                case IconType.Comment:
                    {

                    Contentobj = new Navigation3ViewModel();
                    }
                    break;
            }
        }
    }
}
