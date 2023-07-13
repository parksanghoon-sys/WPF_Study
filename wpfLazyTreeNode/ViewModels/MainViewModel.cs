using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfLazyTreeNode.Models;
using wpfLazyTreeNode.Utils;
using static wpfLazyTreeNode.Utils.ToggleImageUtils;

namespace wpfLazyTreeNode.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public LazyTreeNode CreateNode(string key, string text,ExploreType exploreType)
        {
            var images = ToggleImageUtils.GetExplorers(exploreType);
            var node = new LazyTreeNode { Key = key, Text = text };
            
            node.OnExpanded += Node_OnExpanded;
            node.OpenedImage = images.openedImage;
            node.ClosedImage = images.closedImage;

            if (DirectoryUtils.IsDirectoryOnFileExists(key))
            {
                node.AddDummyNode();
            }
            return node;
        }

        private void Node_OnExpanded(LazyTreeNode node)
        {
            foreach (var di in DirectoryUtils.GetDirectories(node.Key))
            {
                node.Children.Add(CreateNode(di.FullName, di.Name,ExploreType.Directory));
            }
            foreach(var fi in DirectoryUtils.GetFiles(node.Key))
            {
                node.Children.Add(CreateNode(fi.FullName, fi.Name, ExploreType.File));
            }
        }
        private void AddDriveNodes()
        {
            foreach (var driveInfo in DriveInfo.GetDrives())
            {
                PathNodes.Add(CreateNode(driveInfo.Name, driveInfo.Name, ExploreType.Drive));
            }
        }
        public MainViewModel()
        {
            PathNodes = new();
            //PathNodes.Add(CreateNode("1", "홍길동"));
            //PathNodes.Add(CreateNode("2", "김이박"));
            AddDriveNodes();
        }
        public ObservableCollection<LazyTreeNode> PathNodes { get; set; }
    }
}
