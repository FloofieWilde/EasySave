﻿using Newtonsoft.Json;
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
            workerListen.ProgressChanged += worker_ProgressChangedListen;
            workerListen.WorkerReportsProgress = true;
            workerListen.WorkerSupportsCancellation = true;
            workerListen.RunWorkerAsync();
            InitializeComponent();
            
        }

        /// <summary>
        /// Exit the client application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExitApp(object sender, RoutedEventArgs e)
        {
            //ClientServer.Deconnecter(server);
            Environment.Exit(621);
        }

        /// <summary>
        /// Determine if the button is for pause or for play.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlayPauseFct(object sender, RoutedEventArgs e)
        {

            if (PPState == "Pause")
            {
                PlayPause.Source = new BitmapImage(new Uri("Play.png", UriKind.Relative));
                PPState = "Play";
                Pause();
            }
            else if (PPState == "Play"){
                PlayPause.Source = new BitmapImage(new Uri("Pause.png", UriKind.Relative));
                PPState = "Pause";
                Play();
            }
        }

        /// <summary>
        /// If we clicked on play, we send the information to the server, so the server will play again the paused choosen copy.
        /// </summary>
        public void Play()
        {
            int id = Convert.ToInt32(CopyIdPreset.Text);
            BackgroundWorker workerSend = new BackgroundWorker();
            string msg = $"[{id},{2}]";
            workerSend.DoWork += worker_DoWorkSend;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.RunWorkerAsync(msg);
        }

        /// <summary>
        /// If we clicked on pause, we send the information to the server, so the server will pause the active choosen copy.
        /// </summary>
        public void Pause()
        {
            int id = Convert.ToInt32(CopyIdPreset.Text);
            BackgroundWorker workerSend = new BackgroundWorker();
            string msg = $"[{id},{1}]";
            workerSend.DoWork += worker_DoWorkSend;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.RunWorkerAsync(msg);
        }

        /// <summary>
        /// If we click on stop, we send the information to the server, so the server will stop the choosen copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(CopyIdPreset.Text);
            BackgroundWorker workerSend = new BackgroundWorker();
            string msg = $"[{id},{0}]";
            workerSend.DoWork += worker_DoWorkSend;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.RunWorkerAsync(msg);
        }


        //Backgroundworker
        
        /// <summary>
        /// Called when we start the client application. Listen continuously the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            server = ClientServer.SeConnecter();
            string msg = e.Argument as string;
            ClientServer.EcouterReseau(server, sender);
        }

        /// <summary>
        /// Called when there is an information to send to the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //ReceivedMsg.Text = "Text";worker_ProgressChangedListen()
        }

        /// <summary>
        /// Update the informations in real time (every 0,5 second).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                /* Workers[id - 1].RemainingFilesSize = Workers[id - 1].RemainingFilesSize/1024;
                 Workers[id - 1].TotalSize = Workers[id - 1].TotalSize/1024;*/

                if (Workers[id - 1].Statut == "ACTIVE")
                {
                    CopyStatut.Text = Workers[id - 1].Statut;
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Green;
                    CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                    CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id - 1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                    PPState = "Pause";
                    PlayPause.Source = new BitmapImage(new Uri("Pause.png", UriKind.Relative));
                }
                else if (Workers[id - 1].Statut == "CANCELLED")
                {
                    CopyStatut.Text = Workers[id - 1].Statut;
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Red;
                    CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                    CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id - 1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                }
                else if (Workers[id - 1].Statut == "FINISHED")
                {
                    CopyStatut.Text = Workers[id - 1].Statut;
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Green;
                    CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                    CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id - 1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                }
                else if (Workers[id - 1].Statut == "PAUSED")
                {
                    CopyStatut.Text = Workers[id - 1].Statut;
                    Progressbar.Value = Workers[id - 1].Progress;
                    Progressbar.Foreground = Brushes.Yellow;
                    CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                    CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id - 1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                    PPState = "Play";
                    PlayPause.Source = new BitmapImage(new Uri("Play.png", UriKind.Relative));
                }
            }
        }

        /// <summary>
        /// If we double click on the listbox, show the informations of the choosen preset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                BoutonPannel.Visibility = Visibility.Collapsed;
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
                BoutonPannel.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                CopyStatut.Text = Workers[id - 1].Statut;
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Green;
                PPState = "Pause";
                PlayPause.Source = new BitmapImage(new Uri("Pause.png", UriKind.Relative));
            }

            else if (Workers[id - 1].Statut == "CANCELLED")
            {
                Progressbar.Visibility = Visibility.Visible;
                BoutonPannel.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                CopyStatut.Text = Workers[id - 1].Statut;
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Red;
            }

            else if (Workers[id - 1].Statut == "FINISHED")
            {
                Progressbar.Visibility = Visibility.Visible;
                BoutonPannel.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                CopyStatut.Text = Workers[id - 1].Statut;
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Green;
            }

            else if (Workers[id - 1].Statut == "PAUSED")
            {
                Progressbar.Visibility = Visibility.Visible;
                BoutonPannel.Visibility = Visibility.Visible;
                CopyType.Text = $"Type de copie: {Workers[id - 1].CopyType}";
                CopyDate.Text = $"Date de début: {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"Fichier restants: {Workers[id - 1].RemainingFiles} / {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"Taille des fichiers restants: {Workers[id-1].RemainingFilesSize} / {Workers[id - 1].TotalSize} octets";
                CopyStatut.Text = Workers[id - 1].Statut;
                Progressbar.Value = Workers[id - 1].Progress;
                Progressbar.Foreground = Brushes.Yellow;
                PPState = "Play";
                PlayPause.Source = new BitmapImage(new Uri("Play.png", UriKind.Relative));
            }
        } 
    }
}
