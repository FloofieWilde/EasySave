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
        static BackgroundWorker workerListen = new BackgroundWorker();
        Socket server;
        public MainWindow()
        {
            server = ControllerClient.SeConnecter();
            workerListen.DoWork += worker_DoWork;
            workerListen.RunWorkerCompleted += worker_RunWorkerCompleted;
            workerListen.WorkerSupportsCancellation = true;
            workerListen.RunWorkerAsync();
            InitializeComponent();
        }

        private void SendMsgButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker workerSend = new BackgroundWorker();
            string msg = SendMsg.Text;
            workerSend.DoWork += worker_DoWorkSend;
            workerSend.RunWorkerCompleted += worker_RunWorkerCompletedSend;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.RunWorkerAsync(msg);

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string msg = e.Argument as string;
            ControllerClient.EcouterReseau(server);
        }

        private void worker_DoWorkSend(object sender, DoWorkEventArgs e)
        {
            string message = e.Argument as string;
            ControllerClient.SendMsg(message, server);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void worker_RunWorkerCompletedSend(object sender, RunWorkerCompletedEventArgs e)
        {
            ReceivedMsg.Text = "Text";
        }
    }
}
