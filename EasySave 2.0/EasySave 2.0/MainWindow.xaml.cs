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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Visible;
            OptionsPannel.Visibility = Visibility.Collapsed;
            LogsPannel.Visibility = Visibility.Collapsed;
        }

        private void OptionsBouton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Collapsed;
            OptionsPannel.Visibility = Visibility.Visible;
            LogsPannel.Visibility = Visibility.Collapsed;
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Collapsed;
            OptionsPannel.Visibility = Visibility.Collapsed;
            LogsPannel.Visibility = Visibility.Visible;
        }

        private void LangButton_Click(object sender, RoutedEventArgs e)
        {
            LangPannel.Visibility = Visibility.Visible;
            PresetPannel.Visibility = Visibility.Collapsed;
        }

        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Visible;
        }
    }
}
