using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace wpfDragableCanvas.UI.Units
{
    public class DragableCanvasTreeView : TreeView
    {
        static DragableCanvasTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragableCanvasTreeView), new FrameworkPropertyMetadata(typeof(DragableCanvasTreeView)));
        }
    }
}
