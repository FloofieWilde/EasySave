using Newtonsoft.Json;
using Projet.Calcules;
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
        static List<Worker> Workers = new List<Worker>();
        static BackgroundWorker workerListen = new BackgroundWorker();
        Socket server;
        public MainWindow()
        {
            workerListen.DoWork += worker_DoWork;
            workerListen.RunWorkerCompleted += worker_RunWorkerCompletedListen;
            workerListen.ProgressChanged += worker_ProgressChangedListen;
            workerListen.WorkerReportsProgress = true;
            workerListen.WorkerSupportsCancellation = true;
            workerListen.RunWorkerAsync();
            InitializeComponent();
            
        }
        public void ExitApp(object sender, RoutedEventArgs e)
        {
            //ClientServer.Deconnecter(server);
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
            /*Tets.Text = 
                "Type de sauvegarde : " + "Var\n" +
                "Statut : " + "Var\n" +
                "Chemin source : " + "Var\n" +
                "Chemin de destination : " + "Var\n" +
                "Date de début : " + "Var\n" +
                "Fichier : " + "Var/Var " + "(Var restants)\n" +
                "Taille : " + "Var/Var " + "(Var restants)\n"
                ;*/
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

        private void worker_RunWorkerCompletedListen(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void worker_RunWorkerCompletedSend(object sender, RunWorkerCompletedEventArgs e)
        {
            //ReceivedMsg.Text = "Text";
        }
        void worker_ProgressChangedListen(object sender, ProgressChangedEventArgs e)
        {
            string msg = e.UserState as string;
            Workers = JsonConvert.DeserializeObject<List<Worker>>(msg);

            ListPreset.Items.Clear();
            for (int i = 1; i <= Workers.Count; i++)
            {
                ListPreset.Items.Add($"{i} - {Workers[i-1].Name}");
            }
            if (CopyIdPreset.Text != "")
            {
                PanelInfo.Visibility = Visibility.Visible;
                int id = Convert.ToInt32(CopyIdPreset.Text);
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
               /* Workers[id - 1].RemainingFilesSize = Workers[id - 1].RemainingFilesSize/1024;
                Workers[id - 1].TotalSize = Workers[id - 1].TotalSize/1024;*/
                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} Ko";

                if (Workers[id - 1].Statut == "ACTIVE")
                {
                    CopyStatut.Text = $"{Workers[id - 1].Statut}";
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Green;
                    //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                    //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
                }
                else if (Workers[id - 1].Statut == "CANCELLED")
                {
                    CopyStatut.Text = $"{Workers[id - 1].Statut}";
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Red;
                    //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                    //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
                }
                else if (Workers[id - 1].Statut == "FINISHED")
                {
                    CopyStatut.Text = $"{Workers[id - 1].Statut}";
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Green;
                    //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                    //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
                }
                else if (Workers[id - 1].Statut == "PAUSED")
                {
                    CopyStatut.Text = $"{Workers[id - 1].Statut}";
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Yellow;
                    //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                    //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
                }
            }
        }

        private void ListPreset_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = ListPreset.SelectedItem.ToString();
            int id = Convert.ToInt32(Calcule.GetId(selectedItem));
            PanelInfo.Visibility = Visibility.Visible;
            CopyIdPreset.Text = id.ToString();
            CopyNamePreset.Text = $"Nom: {Workers[id-1].Name}";
            CopySource.Text = $"Source: {Workers[id-1].Source}";
            CopyDestination.Text = $"Destination: {Workers[id-1].Destination}";

            if (Workers[id - 1].Statut == "INACTIVE")
            {
                Progressbar.Visibility = Visibility.Collapsed;
                CopyType.Text = "";
                CopyDate.Text = "";
                CopyNbFile.Text = "";
                CopySizeFile.Text = "";
                CopyStatut.Text = Workers[id - 1].Statut;
                Progressbar.Foreground = Brushes.Green;
            }
            else if (Workers[id-1].Statut == "ACTIVE")
            {
                Progressbar.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} Ko";
                CopyStatut.Text = $"{Workers[id - 1].Statut}";
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Green;
                //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }

            else if (Workers[id - 1].Statut == "CANCELLED")
            {
                Progressbar.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} Ko";
                CopyStatut.Text = $"{Workers[id - 1].Statut}";
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Red;
                //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }

            else if (Workers[id - 1].Statut == "FINISHED")
            {
                Progressbar.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} Ko";
                CopyStatut.Text = $"{Workers[id - 1].Statut}";
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Green;
                //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }

            else if (Workers[id - 1].Statut == "PAUSED")
            {
                Progressbar.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} Ko";
                CopyStatut.Text = $"{Workers[id - 1].Statut}";
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Red;
                //CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                //CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }
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

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
