using Newtonsoft.Json;
using Projet.Server;
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

namespace Client_EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string PPState = "Pause";
        static BackgroundWorker workerListen = new BackgroundWorker();
        Socket server;
        public MainWindow()
        {
            workerListen.DoWork += worker_DoWork;
            workerListen.RunWorkerCompleted += worker_RunWorkerCompleted;
            workerListen.ProgressChanged += worker_ProgressChangedListen;
            workerListen.WorkerReportsProgress = true;
            workerListen.WorkerSupportsCancellation = true;
            workerListen.RunWorkerAsync();
            InitializeComponent();
            
        }
        public void ExitApp(object sender, RoutedEventArgs e)
        {
            Environment.Exit(621);
        }

        public void PlayPauseFct(object sender, RoutedEventArgs e)
        {

            if (PPState == "Pause")
            {
                PlayPause.Source = new BitmapImage(new Uri("Play.png", UriKind.Relative));
                PPState = "Play";
                Play();
                UpdateInfos();

            }
            else if (PPState == "Play"){
                PlayPause.Source = new BitmapImage(new Uri("Pause.png", UriKind.Relative));
                PPState = "Pause";
                Pause();
                UpdateInfos();
            }
        }

        public void UpdateInfos()
        {
            Tets.Text = 
                "Type de sauvegarde : " + "Var\n" +
                "Statut : " + "Var\n" +
                "Chemin source : " + "Var\n" +
                "Chemin de destination : " + "Var\n" +
                "Date de début : " + "Var\n" +
                "Fichier : " + "Var/Var " + "(Var restants)\n" +
                "Taille : " + "Var/Var " + "(Var restants)\n"
                ;
        }

        public void Play()
        {
            
        }

        public void Pause()
        {

        }



        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            server = ClientServer.SeConnecter();
            string msg = e.Argument as string;
            ClientServer.EcouterReseau(server, sender);
        }

        private void worker_DoWorkSend(object sender, DoWorkEventArgs e)
        {
            string message = e.Argument as string;
            ClientServer.SendMsg(message, server);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void worker_RunWorkerCompletedSend(object sender, RunWorkerCompletedEventArgs e)
        {
            //ReceivedMsg.Text = "Text";
        }
        void worker_ProgressChangedListen(object sender, ProgressChangedEventArgs e)
        {
            string msg = e.UserState as string;
            List<Worker> Workers = JsonConvert.DeserializeObject<List<Worker>>(msg);
            string text = Workers[8].Progress.ToString();
            TestDistanceText.Text = text;
        }


        private void TestDistance_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker workerSend = new BackgroundWorker();
            string msg = TestDistanceText.Text;
            workerSend.DoWork += worker_DoWorkSend;
            workerSend.RunWorkerCompleted += worker_RunWorkerCompletedSend;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.RunWorkerAsync(msg);
        }
    }
}
