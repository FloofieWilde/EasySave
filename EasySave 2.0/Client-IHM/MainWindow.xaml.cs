using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
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

namespace Client_IHM
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendMsgButton_Click(object sender, RoutedEventArgs e)
        {
            //BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            string msg = SendMsg.Text;
            //worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(msg);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string msg = e.Argument as string;
            ControllerClient.EcouterReseau(msg);
        }

        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    ReceivedMsg.Text = "Text";
        //}
    }
}
