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

namespace Client_EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string PPState = "Pause";

        public MainWindow()
        {
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

    }
}
