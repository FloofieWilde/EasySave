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
using Projet.Extensions;
using Projet.Presets;

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

        public void ExitApp(object sender, RoutedEventArgs e)
        {
            Environment.Exit(621);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Visible;
            OptionsPannel.Visibility = Visibility.Collapsed;
            LogsPannel.Visibility = Visibility.Collapsed;
            InfoCopy.Visibility = Visibility.Collapsed;
            ProgressCopy.Visibility = Visibility.Collapsed;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPresetCopy.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
        }

        private void OptionsBouton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Collapsed;
            OptionsPannel.Visibility = Visibility.Visible;
            LogsPannel.Visibility = Visibility.Collapsed;
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
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
            ExtensionPannel.Visibility = Visibility.Collapsed;
        }
        private void ExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            ListExtension.Items.Clear();
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Visible;
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            Dictionary<string, string> extension = Extension.GetJsonExtension();
            int nbExtension = extension.Count;
            for (int i = 1; i <= nbExtension; i++)
            {
                ListExtension.Items.Add(i.ToString() + $" - {extension["Extension" + i.ToString()]}");
            }
        }

        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            ListPreset.Items.Clear();
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Visible;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPreset.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
        }

        private void AddPresetButton_Click(object sender, RoutedEventArgs e)
        {
            AddNameTextbox.Text = "";
            AddPathSourceTextbox.Text = "";
            AddPathDestinationTextbox.Text = "";
            AddPannel.Visibility = Visibility.Visible;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void EditPresetButton_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            if (ListPreset.SelectedIndex != -1)
            {
                EditPannel.Visibility = Visibility.Visible;
                string selectedItem = ListPreset.SelectedItem.ToString();
                int id = Convert.ToInt32(selectedItem.Substring(0, 1));
                PresetEditId.Content = id.ToString();
                EditNameTextbox.Text = preset["Preset" + id].Name;
                EditPathSourceTextbox.Text = preset["Preset" + id].PathSource;
                EditPathDestinationTextbox.Text = preset["Preset" + id].PathDestination;
            }
        }

        private void DeletePresetButton_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            if (ListPreset.SelectedIndex != -1)
            {
                DeletePannel.Visibility = Visibility.Visible;
                string selectedItem = ListPreset.SelectedItem.ToString();
                int id = Convert.ToInt32(selectedItem.Substring(0, 1));
                PresetDeleteId.Content = id.ToString();
            }
        }

        private void ConfirmEditPreset_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(PresetEditId.Content);
            string name = EditNameTextbox.Text;
            string pathSource = EditPathSourceTextbox.Text;
            string pathDestination = EditPathDestinationTextbox.Text;
            Preset.EditPreset(id, name, pathSource, pathDestination);
            ListPreset.Items.Clear();
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPreset.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void CancelEditPreset_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void ConfirmAddPreset_Click(object sender, RoutedEventArgs e)
        {
            string name = AddNameTextbox.Text;
            string pathSource = AddPathSourceTextbox.Text;
            string pathDestination = AddPathDestinationTextbox.Text;
            Preset.AddPreset(name, pathSource, pathDestination);
            ListPreset.Items.Clear();
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPreset.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void CancelAddPreset_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void ConfirmDeletePreset_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(PresetDeleteId.Content);
            Preset.DeletePreset(id);
            ListPreset.Items.Clear();
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPreset.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void CancelDeletePreset_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void AddExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionTextbox.Text = "";
            AddExtensionPannel.Visibility = Visibility.Visible;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void EditExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            Dictionary<string, string> extensions = Extension.GetJsonExtension();
            if (ListExtension.SelectedIndex != -1)
            {
                EditExtensionPannel.Visibility = Visibility.Visible;
                string selectedItem = ListExtension.SelectedItem.ToString();
                int id = Convert.ToInt32(selectedItem.Substring(0, 1));
                ExtensionEditId.Content = id;
                EditExtensionTextbox.Text = extensions["Extension" + id];
            }
        }

        private void DeleteExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            if (ListExtension.SelectedIndex != -1)
            {
                DeleteExtensionPannel.Visibility = Visibility.Visible;
                string selectedItem = ListExtension.SelectedItem.ToString();
                int id = Convert.ToInt32(selectedItem.Substring(0, 1));
                ExtensionDeleteId.Content = id;
            }
        }

        private void ConfirmAddExtension_Click(object sender, RoutedEventArgs e)
        {
            string extension = AddExtensionTextbox.Text;
            Extension.AddExtension(extension);
            ListExtension.Items.Clear();
            Dictionary<string, string> extensions = Extension.GetJsonExtension();
            int nbExtension = extensions.Count;
            for (int i = 1; i <= nbExtension; i++)
            {
                ListExtension.Items.Add(i.ToString() + $" - {extensions["Extension" + i.ToString()]}");
            }
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void CancelAddExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void ConfirmEditExtension_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(ExtensionEditId.Content);
            string extension = EditExtensionTextbox.Text;
            Extension.EditExtension(id, extension);
            ListExtension.Items.Clear();
            Dictionary<string, string> extensions = Extension.GetJsonExtension();
            int nbExtension = extensions.Count;
            for (int i = 1; i <= nbExtension; i++)
            {
                ListExtension.Items.Add(i.ToString() + $" - {extensions["Extension" + i.ToString()]}");
            }
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void CancelEditExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void ConfirmDeleteExtension_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(ExtensionDeleteId.Content);
            Extension.DeleteExtension(id);
            ListExtension.Items.Clear();
            Dictionary<string, string> extensions = Extension.GetJsonExtension();
            int nbExtension = extensions.Count;
            for (int i = 1; i <= nbExtension; i++)
            {
                ListExtension.Items.Add(i.ToString() + $" - {extensions["Extension" + i.ToString()]}");
            }
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void CancelDeleteExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        private void CopyStartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((RadioCopyComplet.IsChecked == true || RadioCopyPartial.IsChecked == true) && ListPresetCopy.SelectedIndex != -1)
            {
                Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
                string presetId = ListPresetCopy.SelectedItem.ToString().Substring(0,1);
                string name = preset["Preset" + presetId].Name;
                string source = preset["Preset" + presetId].PathSource;
                string destination = preset["Preset" + presetId].PathDestination;
                string copyType = "";
                if (RadioCopyComplet.IsChecked == true)
                {
                    copyType = "complet";
                }
                else if (RadioCopyPartial.IsChecked == true)
                {
                    copyType = "partielle";
                }
                InfoCopy.Visibility = Visibility.Visible;
                ProgressCopy.Visibility = Visibility.Visible;
                ErrorCopy.Visibility = Visibility.Hidden;
                CopyType.Text = $"Type de sauvegarde: {copyType}";
                CopyNamePreset.Text = $"Nom sauvegarde: {name}";
                CopySource.Text = $"Path Source: {source}";
                CopyDestination.Text = $"Path Destination: {destination}";
                CopyDate.Text = "Date de début: ";
                CopyNbFile.Text = "Nombre total des fichiers: 1";
                CopySizeFile.Text = "Taille total des fichiers: ";

                CopyFileRemaining.Content = "Fichier restants: ";
                CopySizeRemaining.Content = "Taille des fichiers restant: ";
                //CopyEnd.Text = "Copie terminée!";
            }
            else
            {
                ErrorCopy.Visibility = Visibility.Visible;
            }
        }
    }
}
