using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        class MyCustomPanel : Panel
        {
            bool _wrap = false;

            public MyCustomPanel()
            {
            }

            protected override Size MeasureOverride(Size constraint)
            {
                if (this.Children.Count < 2)
                {
                    return base.MeasureOverride(constraint);
                }
                Size finalSize = new Size();

                Children[0].Measure(constraint);
                Children[1].Measure(constraint);

                if (Children[0].DesiredSize.Width + Children[1].DesiredSize.Width <= constraint.Width)
                {
                    _wrap = false;
                    finalSize.Width = Children[0].DesiredSize.Width + Children[1].DesiredSize.Width;
                    finalSize.Height = Math.Max(Children[0].DesiredSize.Height, Children[1].DesiredSize.Height);
                }
                else
                {
                    _wrap = true;
                    finalSize.Height = Children[0].DesiredSize.Height + Children[1].DesiredSize.Height;
                    finalSize.Width = Math.Max(Children[0].DesiredSize.Width, Children[1].DesiredSize.Width);
                }
                return finalSize;
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                if (_wrap)
                {
                    Children[0].Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
                    Children[1].Arrange(new Rect(0, Children[0].RenderSize.Height, finalSize.Width, finalSize.Height - Children[0].RenderSize.Height));
                }
                else
                {
                    Children[0].Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
                    Children[1].Arrange(new Rect(finalSize.Width - Children[1].DesiredSize.Width, 0, Children[1].DesiredSize.Width, finalSize.Height));

                }
                return base.ArrangeOverride(finalSize);
            }
        }
    }
}
