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
using Projet.SaveSystem;
using Projet.Stockages;
using Projet.WorkSoftwares;
using Projet.Languages;
using System.IO;
using Projet.Logs;
using Newtonsoft.Json;
using System.Net;
using System.Data;
using System.ComponentModel;
using Microsoft.Win32;
using Projet.Priority;
using Projet.Size;
using System.Threading;
using Projet.Save;
using System.Net.Sockets;
using Projet.Client;
using System.Diagnostics;

namespace EasySave_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static BackgroundWorker workerListen = new BackgroundWorker();
        static BackgroundWorker workerSend = new BackgroundWorker();
        static Langue.Language dictLang = Langue.GetLang();
        public static List<Worker> Workers = new List<Worker>();
        Socket server;
        Socket client;

        public MainWindow()
        {
            //Checking if the application is already open, and closing it if it is, else we open it.
            bool aIsNewInstance = false;
            Mutex myMutex = new Mutex(true, "MainWindow", out aIsNewInstance);
            if (!aIsNewInstance)
            {
                MessageBox.Show("Already an instance is running...");
                App.Current.Shutdown();
            }
            else
            {
                workerListen.DoWork += worker_DoWorkListen;
                workerListen.RunWorkerCompleted += worker_RunWorkerCompletedListen;
                workerListen.ProgressChanged += worker_ProgressChangedListen;
                workerListen.WorkerReportsProgress = true;
                workerListen.WorkerSupportsCancellation = true;
                workerListen.RunWorkerAsync();

                workerSend.DoWork += worker_DoWorkSend;
                workerSend.RunWorkerCompleted += worker_RunWorkerCompletedSend;
                workerSend.WorkerSupportsCancellation = true;

                InitializeComponent();
            }
        }

        /// <summary>
        /// If we click on the exit button, we close the connexion with the client and close the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExitApp(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                Server.Deconnecter(server);
            }

            Environment.Exit(621);
        }

        /// <summary>
        /// If we click on "copy", we add all presets into the listbox. We also open a connexion with the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Visible;
            OptionsPannel.Visibility = Visibility.Collapsed;
            LogsPannel.Visibility = Visibility.Collapsed;
            InfoCopy.Visibility = Visibility.Collapsed;
            ProgressCopy.Visibility = Visibility.Collapsed;
            ListPresetCopy.Items.Clear();
            Workers.Clear();
            RadioCopyPartial.Content = dictLang.PartialCopy;
            RadioCopyComplet.Content = dictLang.CompletCopy;
            ChooseCopyType.Content = dictLang.ChooseTypeCopy;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPresetCopy.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
                BackgroundWorker workerCopy = new BackgroundWorker();
                workerCopy.DoWork += worker_DoWork;
                workerCopy.RunWorkerCompleted += worker_RunWorkerCompleted;
                workerCopy.ProgressChanged += worker_ProgressChanged;
                workerCopy.WorkerReportsProgress = true;
                workerCopy.WorkerSupportsCancellation = true;
                ManualResetEvent tempWorkEvent = new ManualResetEvent(false);
                Workers.Add(new Worker
                {
                    worker = workerCopy,
                    WorkEvent = tempWorkEvent,
                    Id = i,
                    Statut = "INACTIVE",
                    Name = preset["Preset" + i.ToString()].Name,
                    Source = preset["Preset" + i.ToString()].PathSource,
                    Destination = preset["Preset" + i.ToString()].PathDestination
                });

            }
            if (!workerSend.IsBusy && client != null)
            {
                string msg = JsonConvert.SerializeObject(Workers, Formatting.Indented);
                workerSend.RunWorkerAsync(msg);
            }
        }

        /// <summary>
        /// If we click on "options", the menu options appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OptionsBouton_Click(object sender, RoutedEventArgs e)
        {
            LangButton.Content = dictLang.OptMLang;
            PresetButton.Content = dictLang.OptMPreset;
            ExtensionButton.Content = dictLang.OptMExt;
            ApplicationButton.Content = dictLang.OptMApp;
            StockageButton.Content = dictLang.OptMStoc;
            PriorityButton.Content = dictLang.OptMPrio;
            SizeButton.Content = dictLang.OptMSize;
            CopyPannel.Visibility = Visibility.Collapsed;
            OptionsPannel.Visibility = Visibility.Visible;
            LogsPannel.Visibility = Visibility.Collapsed;
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on logs, we add all days in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Collapsed;
            OptionsPannel.Visibility = Visibility.Collapsed;
            LogsPannel.Visibility = Visibility.Visible;
            LogsListbox.Items.Clear();
            LogsGrid.DataContext = null;
            var dates = LogDaily.GetJsonDates();
            foreach (string date in dates)
            {
                LogsListbox.Items.Add(date);
            }
        }

        /// <summary>
        /// If we double click on the listbox, the logs daily of the selected day appears.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogsListbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedDate = LogsListbox.SelectedItem.ToString();
            var logs = LogDaily.GetJsonLogs();
            List<LogJson> logSelected = new List<LogJson>();
            foreach (var log in logs)
            {
                if (log.Key == selectedDate)
                {
                    logSelected = log.Value;
                }
            }
            LogsGrid.DataContext = logSelected;
        }

        /// <summary>
        /// If we click on the language button, the available language appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LangButton_Click(object sender, RoutedEventArgs e)
        {
            //Texttext.Content = dictLang.MenuTitle;
            //Kckc.Content = GetLangLines().Item1;

            GenerateGridLang();

            LangPannel.Visibility = Visibility.Visible;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on the button "extensions" the list of extension appear (they are the extensions to crypt a file)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            ListExtension.Items.Clear();
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Visible;
            StockagePannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
            Dictionary<string, string> extension = Extension.GetJsonExtension();
            int nbExtension = extension.Count;
            for (int i = 1; i <= nbExtension; i++)
            {
                ListExtension.Items.Add(i.ToString() + $" - {extension["Extension" + i.ToString()]}");
            }
        }

        /// <summary>
        /// If we click on "priority", we add everything in the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityButton_Click(object sender, RoutedEventArgs e)
        {
            ListPriority.Items.Clear();
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Visible;
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
            Dictionary<string, string> priorities = Priority.GetJsonPriority();
            int nbPriority = priorities.Count;
            for (int i = 1; i <= nbPriority; i++)
            {
                ListPriority.Items.Add(i.ToString() + $" - {priorities["Priority" + i.ToString()]}");
            }
        }

        /// <summary>
        /// If we click on "application", we add the current work application in the listbox (by default "calculator").
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            EditApplicationButton.Content = dictLang.OptAppAlter;
            LabelEditApplication.Content = dictLang.OptAppMod;
            LabelEditTheApplication.Content = dictLang.Name;
            ConfirmEditApplication.Content = dictLang.Confirm;
            CancelEditApplication.Content = dictLang.Cancel;
            ListApplication.Items.Clear();
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Visible;
            StockagePannel.Visibility = Visibility.Collapsed;
            EditApplicationPannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
            WorkSoft application = WorkSoftware.GetJsonApplication();
            ListApplication.Items.Add(application.Application);
        }

        /// <summary>
        /// If we click on "stockage", we display the current type of stockage (xml or json).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockageButton_Click(object sender, RoutedEventArgs e)
        {
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Visible;
            PriorityPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
            JsonXml stockage = Stockage.GetJsonStockage();
            LabelCurrentStockage.Content = dictLang.OptStocNow + stockage.TypeStockage;
            EditStockageButton.Content = dictLang.OptStocAlter;
            LabelEditStockage.Content = dictLang.OptStocNew;
            ConfirmEditStockage.Content = dictLang.Confirm;
            CancelEditStockage.Content = dictLang.Cancel;
        }

        /// <summary>
        /// If we click on "size", we display the current maximum size of a file we can copy at the same time. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SizeButton_Click(object sender, RoutedEventArgs e)
        {
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Visible;
            EditSizePannel.Visibility = Visibility.Collapsed;
            JsonSize size = Projet.Size.Size.GetJsonSize();
            LabelCurrentSize.Content = dictLang.OptCurrentSize + size.Size + " Ko";
            EditSizeButton.Content = dictLang.OptSizeAlter;
            LabelEditSize.Text = dictLang.OptSizeNew;
            ConfirmEditSize.Content = dictLang.Confirm;
            CancelEditSize.Content = dictLang.Cancel;
        }

        /// <summary>
        /// if we click on "preset", we add them to the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            ListPreset.Items.Clear();
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Visible;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Collapsed;
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
            PriorityPannel.Visibility = Visibility.Collapsed;
            SizePannel.Visibility = Visibility.Collapsed;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPreset.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
        }

        /// <summary>
        /// If we click on "add preset", the pannel to add a new one appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPresetButton_Click(object sender, RoutedEventArgs e)
        {
            LabelAddPreset.Content = dictLang.OptPreAdd;
            LabelNameAddPreset.Content = dictLang.Name;
            LabelSourceAddPreset.Content = dictLang.Sauce;
            LabelDestinationAddPreset.Content = dictLang.Dest;
            ConfirmAddPreset.Content = dictLang.Confirm;
            CancelAddPreset.Content = dictLang.Cancel;
            AddSourceFileButton.Content = dictLang.OptiPreBrowse;
            AddDestinationFileButton.Content = dictLang.OptiPreBrowse;
            AddNameTextbox.Text = "";
            AddPathSourceTextbox.Text = "";
            AddPathDestinationTextbox.Text = "";
            AddPannel.Visibility = Visibility.Visible;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "edit preset", the pannel appear and the current name/path are showed in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPresetButton_Click(object sender, RoutedEventArgs e)
        {
            LabelEditPreset.Content = dictLang.OptPreEdit;
            LabelNameEditPreset.Content = dictLang.Name;
            LabelSourceEditPreset.Content = dictLang.Sauce;
            LabelDestinationEditPreset.Content = dictLang.Dest;
            ConfirmEditPreset.Content = dictLang.Confirm;
            CancelEditPreset.Content = dictLang.Cancel;
            EditSourceFileButton.Content = dictLang.OptiPreBrowse;
            EditDestinationFileButton.Content = dictLang.OptiPreBrowse;

            AddPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            if (ListPreset.SelectedIndex != -1)
            {
                EditPannel.Visibility = Visibility.Visible;
                string selectedItem = ListPreset.SelectedItem.ToString();
                string id = "";
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        if (i.ToString() == selectedItem.Substring(j, 1))
                        {
                            id += i.ToString();
                        }
                    }
                }
                PresetEditId.Content = id;
                EditNameTextbox.Text = preset["Preset" + id].Name;
                EditPathSourceTextbox.Text = preset["Preset" + id].PathSource;
                EditPathDestinationTextbox.Text = preset["Preset" + id].PathDestination;
            }
        }

        /// <summary>
        /// If we click on "delete preset", the current preset is deleted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePresetButton_Click(object sender, RoutedEventArgs e)
        {
            LabelDeletePreset.Content = dictLang.OptPreDel;
            ConfirmDeletePreset.Content = dictLang.Confirm;
            CancelDeletePreset.Content = dictLang.Cancel;
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            if (ListPreset.SelectedIndex != -1)
            {
                DeletePannel.Visibility = Visibility.Visible;
                string selectedItem = ListPreset.SelectedItem.ToString();
                string id = "";
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        if (i.ToString() == selectedItem.Substring(j, 1))
                        {
                            id += i.ToString();
                        }
                    }
                }
                PresetDeleteId.Content = id;
            }
        }

        /// <summary>
        /// If we click on "confirm to edit a preset", the preset is edited and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// If we click on "cancel edit preset", the pannel to edit disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEditPreset_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "browse" beside "source path", the explorer open so we can choose a path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFolder = new OpenFileDialog(); ;
            openFolder.ValidateNames = false;
            openFolder.CheckFileExists = false;
            openFolder.CheckPathExists = true;
            openFolder.FileName = "Ce dossier";
            openFolder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolder.ShowDialog() == true)
            {
                AddPathSourceTextbox.Text = System.IO.Path.GetDirectoryName(openFolder.FileName);
            }
        }

        /// <summary>
        /// If we click on "browse" beside "destination path", the explorer open so we can choose a path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDestinationFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFolder = new OpenFileDialog();
            openFolder.ValidateNames = false;
            openFolder.CheckFileExists = false;
            openFolder.CheckPathExists = true;
            openFolder.FileName = "Ce dossier";
            openFolder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolder.ShowDialog() == true)
            {
                AddPathDestinationTextbox.Text = System.IO.Path.GetDirectoryName(openFolder.FileName);
            }
        }

        /// <summary>
        /// If we click on "browse" beside "source path", the explorer open so we can choose a path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditSourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFolder = new OpenFileDialog();
            openFolder.ValidateNames = false;
            openFolder.CheckFileExists = false;
            openFolder.CheckPathExists = true;
            openFolder.FileName = "Ce dossier";
            openFolder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolder.ShowDialog() == true)
            {
                EditPathSourceTextbox.Text = System.IO.Path.GetDirectoryName(openFolder.FileName);
            }
        }

        /// <summary>
        /// If we click on "browse" beside "destination path", the explorer open so we can choose a path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDestinationFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFolder = new OpenFileDialog();
            openFolder.ValidateNames = false;
            openFolder.CheckFileExists = false;
            openFolder.CheckPathExists = true;
            openFolder.FileName = "Ce dossier";
            openFolder.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFolder.ShowDialog() == true)
            {
                EditPathDestinationTextbox.Text = System.IO.Path.GetDirectoryName(openFolder.FileName);
            }
        }

        /// <summary>
        /// If we click on confirm add preset, the preset is added and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// If we click on cancel add preset, the pannel to add disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAddPreset_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on confirm delete preset, the preset is deleted and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// If we click on cancel delete preset, the pannel to delete disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDeletePreset_Click(object sender, RoutedEventArgs e)
        {
            AddPannel.Visibility = Visibility.Collapsed;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on add extension, the pannel appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            LabelAddExtension.Content = dictLang.OptExtAdd;
            LabelAddNewExtension.Content = dictLang.Extension;
            ConfirmAddExtension.Content = dictLang.Confirm;
            CancelAddExtension.Content = dictLang.Cancel;

            AddExtensionTextbox.Text = "";
            AddExtensionPannel.Visibility = Visibility.Visible;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on edit extension, the pannel to edit appear and the current extension is displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            LabelEditExtension.Content = dictLang.OptExtEdit;
            LabelEditTheExtension.Content = dictLang.Extension;
            ConfirmEditExtension.Content = dictLang.Confirm;
            CancelEditExtension.Content = dictLang.Cancel;

            AddExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
            Dictionary<string, string> extensions = Extension.GetJsonExtension();
            if (ListExtension.SelectedIndex != -1)
            {
                EditExtensionPannel.Visibility = Visibility.Visible;
                string selectedItem = ListExtension.SelectedItem.ToString();
                string id = "";
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        if (i.ToString() == selectedItem.Substring(j, 1))
                        {
                            id += i.ToString();
                        }
                    }
                }
                ExtensionEditId.Content = id;
                EditExtensionTextbox.Text = extensions["Extension" + id];
            }
        }

        /// <summary>
        /// If we click on "delete extension", the pannel to delete appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            LabelDeleteExtension.Content = dictLang.OptExtDel;
            ConfirmDeleteExtension.Content = dictLang.Confirm;
            CancelDeleteExtension.Content = dictLang.Cancel;

            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            if (ListExtension.SelectedIndex != -1)
            {
                DeleteExtensionPannel.Visibility = Visibility.Visible;
                string selectedItem = ListExtension.SelectedItem.ToString();

                string id = "";
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        if (i.ToString() == selectedItem.Substring(j, 1))
                        {
                            id += i.ToString();
                        }
                    }
                }
                ExtensionDeleteId.Content = id;
            }
        }

        /// <summary>
        /// If we click on "confirm add extension", the extension is added in the json and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// If we cancel adding an extension, the pannel to add disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAddExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "confirm edit extension", the extension is edited in the json, and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        /// <summary>
        /// If we click on cancel, the pannel disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEditExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on delete, the extension is deleted and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// If we click on cancel, the pannel disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDeleteExtension_Click(object sender, RoutedEventArgs e)
        {
            AddExtensionPannel.Visibility = Visibility.Collapsed;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "addpriority" the pannel to add a priority appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            LabelAddPriority.Content = dictLang.OptExtAdd;
            LabelAddNewPriority.Content = dictLang.Extension;
            ConfirmAddPriority.Content = dictLang.Confirm;
            CancelAddPriority.Content = dictLang.Cancel;

            AddPriorityTextbox.Text = "";
            AddPriorityPannel.Visibility = Visibility.Visible;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "edit priority", the pannel appear and the current priority is displayed in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditPriorityButton_Click(object sender, RoutedEventArgs e)
        {
            LabelEditPriority.Content = dictLang.OptExtEdit;
            LabelEditThePriority.Content = dictLang.Extension;
            ConfirmEditPriority.Content = dictLang.Confirm;
            CancelEditPriority.Content = dictLang.Cancel;

            AddPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
            Dictionary<string, string> priorities = Priority.GetJsonPriority();
            if (ListPriority.SelectedIndex != -1)
            {
                EditPriorityPannel.Visibility = Visibility.Visible;
                string selectedItem = ListPriority.SelectedItem.ToString();
                string id = "";
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        if (i.ToString() == selectedItem.Substring(j, 1))
                        {
                            id += i.ToString();
                        }
                    }
                }
                PriorityEditId.Content = id;
                EditPriorityTextbox.Text = priorities["Priority" + id];
            }
        }

        /// <summary>
        /// If we click on "delete priority", the pannel to delete appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePriorityButton_Click(object sender, RoutedEventArgs e)
        {
            LabelDeletePriority.Content = dictLang.OptExtDel;
            ConfirmDeletePriority.Content = dictLang.Confirm;
            CancelDeletePriority.Content = dictLang.Cancel;

            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            if (ListPriority.SelectedIndex != -1)
            {
                DeletePriorityPannel.Visibility = Visibility.Visible;
                string selectedItem = ListPriority.SelectedItem.ToString();

                string id = "";
                for (int i = 0; i <= 9; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        if (i.ToString() == selectedItem.Substring(j, 1))
                        {
                            id += i.ToString();
                        }
                    }
                }
                PriorityDeleteId.Content = id;
            }
        }

        /// <summary>
        /// If we click on confirm add priority, the priority is added and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmAddPriority_Click(object sender, RoutedEventArgs e)
        {
            string priority = AddPriorityTextbox.Text;
            Priority.AddPriority(priority);
            ListPriority.Items.Clear();
            Dictionary<string, string> priorities = Priority.GetJsonPriority();
            int nbPriority = priorities.Count;
            for (int i = 1; i <= nbPriority; i++)
            {
                ListPriority.Items.Add(i.ToString() + $" - {priorities["Priority" + i.ToString()]}");
            }
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we cancel adding the priority, the pannels disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAddPriority_Click(object sender, RoutedEventArgs e)
        {
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we confirm adding the priority, the priority is edited and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmEditPriority_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(PriorityEditId.Content);
            string priority = EditPriorityTextbox.Text;
            Priority.EditPriority(id, priority);
            ListPriority.Items.Clear();
            Dictionary<string, string> priorities = Priority.GetJsonPriority();
            int nbPriority = priorities.Count;
            for (int i = 1; i <= nbPriority; i++)
            {
                ListPriority.Items.Add(i.ToString() + $" - {priorities["Priority" + i.ToString()]}");
            }
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we cancel, the pannel disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEditPriority_Click(object sender, RoutedEventArgs e)
        {
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "confirm delete priority", the priority is deleted and the listbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmDeletePriority_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(PriorityDeleteId.Content);
            Priority.DeletePriority(id);
            ListPriority.Items.Clear();
            Dictionary<string, string> priorities = Priority.GetJsonPriority();
            int nbPriority = priorities.Count;
            for (int i = 1; i <= nbPriority; i++)
            {
                ListPriority.Items.Add(i.ToString() + $" - {priorities["Priority" + i.ToString()]}");
            }
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on "cancel delete priority", the pannel disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDeletePriority_Click(object sender, RoutedEventArgs e)
        {
            AddPriorityPannel.Visibility = Visibility.Collapsed;
            EditPriorityPannel.Visibility = Visibility.Collapsed;
            DeletePriorityPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on edit size button, the pannel appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditSizeButton_Click(object sender, RoutedEventArgs e)
        {
            EditSizePannel.Visibility = Visibility.Visible;
            JsonSize size = Projet.Size.Size.GetJsonSize();
            EditSizeTextbox.Text = size.Size.ToString();
            LabelErrorSize.Content = "";
        }

        /// <summary>
        /// If we click on confirm edit size, the size is edited (we verify if the input is a number).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmEditSize_Click(object sender, RoutedEventArgs e)
        {
            string size = EditSizeTextbox.Text;
            var isNumeric = int.TryParse(size, out int sizeInt);
            if (isNumeric == true)
            {
                Projet.Size.Size.EditSize(sizeInt);
                LabelCurrentSize.Content = dictLang.OptCurrentSize + size + " Ko";
                EditSizePannel.Visibility = Visibility.Collapsed;
            }
            else
            {
                LabelErrorSize.Content = dictLang.OptSizeError;
            }
        }

        /// <summary>
        /// Close the pannel to edit the size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEditSize_Click(object sender, RoutedEventArgs e)
        {
            EditSizePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Start the copy for the selected preset. Verify is the preset and type are valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyStartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((RadioCopyComplet.IsChecked == true || RadioCopyPartial.IsChecked == true) && ListPresetCopy.SelectedIndex != -1)
            {
                Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
                string selectedItem = ListPresetCopy.SelectedItem.ToString();
                int id = Convert.ToInt32(Preset.GetId(selectedItem));
                string name = preset["Preset" + id].Name;
                string source = preset["Preset" + id].PathSource;
                string destination = preset["Preset" + id].PathDestination;
                string copyType = "";
                bool full = false;
                if (RadioCopyComplet.IsChecked == true)
                {
                    full = true;
                    copyType = dictLang.CompletCopy;
                }
                else if (RadioCopyPartial.IsChecked == true)
                {
                    copyType = dictLang.PartialCopy;
                }

                Save save = new Save(source, destination, full);
                var DirInfo = save.Copy();

                if (DirInfo.error != 0)
                {
                    ErrorCopy.Content = DirInfo.error switch
                    {
                        1 => dictLang.ErrorCloseApplication,
                        2 => dictLang.ErrorUnvalidPreset,
                        3 => dictLang.ErrorEmptyFolder,
                        _ => dictLang.ErrorOther,
                    };
                }

                else
                {
                    InfoCopy.Visibility = Visibility.Visible;
                    ProgressCopy.Visibility = Visibility.Visible;
                    CopyIdPreset.Text = id.ToString();
                    CopyType.Text = $"{dictLang.CopyType} {copyType}";
                    CopyNamePreset.Text = $"{dictLang.CopyPreset} {name}";
                    CopySource.Text = $"{dictLang.CopyPathSource} {source}";
                    CopyDestination.Text = $"{dictLang.CopyPathDest} {destination}";

                    var staticLog = save.CurrentStateLog;

                    CopyDate.Text = $"{dictLang.CopyDateStart} {staticLog.Timestamp}";
                    CopyNbFile.Text = $"{dictLang.CopyNbFiles} {staticLog.TotalFiles}";
                    CopySizeFile.Text = $"{dictLang.CopyFileSize} {staticLog.TotalSize}";
                    CopyStatut.Text = dictLang.CopyStillRunning;
                    RadioCopyPartial.IsChecked = false;
                    RadioCopyComplet.IsChecked = false;
                    ErrorCopy.Content = "";
                    ProgressBarCopy.Foreground = Brushes.Green;
                    //string jsonWorker = JsonConvert.SerializeObject(Workers, Formatting.Indented);

                    List<string> param = new List<string>() { full.ToString(), source, destination, id.ToString() };
                    if (Workers[id - 1].worker.IsBusy)
                    {
                        ErrorCopy.Content = dictLang.CopyBusy;
                    }
                    else
                    {
                        Workers[id - 1].Id = id;
                        Workers[id - 1].Statut = "ACTIVE";
                        Workers[id - 1].CopyType = copyType;
                        Workers[id - 1].Name = name;
                        Workers[id - 1].Source = source;
                        Workers[id - 1].Destination = destination;
                        Workers[id - 1].DateStart = staticLog.Timestamp;
                        Workers[id - 1].TotalFiles = staticLog.TotalFiles;
                        Workers[id - 1].TotalSize = staticLog.TotalSize;
                        Workers[id - 1].Progress = 0;
                        Workers[id - 1].worker.RunWorkerAsync(param);
                    }
                }
            }
            else
            {
                ErrorCopy.Content = dictLang.ErrorChooseTypePreset;
            }
        }

        /// <summary>
        /// Called when we click on "start copy" for each save. This is a new thread (backgroundworker). It process the copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            List<string> param = e.Argument as List<string>;
            string copyType = param[0];
            string source = param[1];
            string destination = param[2];
            string jsonWorker = param[3];
            int finalId = Convert.ToInt32(jsonWorker);
            Workers[finalId - 1].WorkEvent.Set();
            bool full = false;
            if (copyType == "True") { full = true; }
            else if (copyType == "False") { full = false; }
            Save save = new Save(source, destination, full);
            var DirInfo = save.Copy();
            save.ProcessCopy(DirInfo.source, DirInfo.target, ProgressBarCopy, sender, e, finalId);
        }

        /// <summary>
        /// Called if the thread stopped (cancelled or finished). Display the information on the IHM.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int idPreset = Convert.ToInt32(CopyIdPreset.Text);
            int idWorker = 0;
            BackgroundWorker currentWorker = sender as BackgroundWorker;
            for (int i = 0; i < Workers.Count; i++)
            {
                if (currentWorker == Workers[i].worker)
                {
                    idWorker = i;
                }
            }
            if (e.Cancelled)
            {
                if (Workers[idPreset - 1].Statut == "CANCELLED")
                {
                    ProgressBarCopy.Foreground = Brushes.Red;
                    CopyStatut.Text = dictLang.CopyCancelled;
                }
                //Workers[idWorker].Statut = "CANCELLED";

                LogStates.UpdateJsonLogState(Workers);

                if (client != null)
                {
                    BackgroundWorker workerSendUpdate = new BackgroundWorker();
                    workerSendUpdate.DoWork += worker_DoWorkSend;
                    string msg = JsonConvert.SerializeObject(Workers, Formatting.Indented);
                    workerSendUpdate.RunWorkerAsync(msg);
                }
            }
            else
            {
                Workers[idWorker].Statut = "FINISHED";
                Workers[idWorker].Progress = 100;
                Workers[idWorker].RemainingFiles = 0;
                Workers[idWorker].RemainingFilesSize = 0;
                LogStates.UpdateJsonLogState(Workers);
                if (idPreset - 1 == idWorker)
                {
                    CopyStatut.Text = dictLang.CopySuccess;
                    ProgressBarCopy.Value = 100;
                    ProgressBarCopy.Foreground = Brushes.Green;
                    CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {0}";
                    CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {0}";
                }
                if (client != null)
                {
                    BackgroundWorker workerSendUpdate = new BackgroundWorker();
                    workerSendUpdate.DoWork += worker_DoWorkSend;
                    string msg = JsonConvert.SerializeObject(Workers, Formatting.Indented);
                    workerSendUpdate.RunWorkerAsync(msg);
                }
            }
        }

        /// <summary>
        /// Called everytime the progress change (after any file was just copied). Display in real time the progress of the copy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int idPreset = Convert.ToInt32(CopyIdPreset.Text);
            int idWorker = 0;
            BackgroundWorker currentWorker = sender as BackgroundWorker;
            for (int i = 0; i < Workers.Count; i++)
            {
                if (currentWorker == Workers[i].worker)
                {
                    idWorker = i;
                }
            }
            List<long> param = e.UserState as List<long>;
            long remainingFiles = param[0];
            long remainingFilesSize = param[1];
            //Workers[idWorker].Statut = "ACTIVE";
            Workers[idWorker].Progress = e.ProgressPercentage;
            Workers[idWorker].RemainingFiles = remainingFiles;
            Workers[idWorker].RemainingFilesSize = remainingFilesSize;
            LogStates.UpdateJsonLogState(Workers);

            if (Workers[idPreset-1].Statut == "ACTIVE")
            {
                ProgressBarCopy.Foreground = Brushes.Green;
                Workers[idPreset - 1].Statut = "ACTIVE";
                CopyStatut.Text = dictLang.CopyStillRunning;
            }
            else if (Workers[idPreset-1].Statut == "PAUSED")
            {
                ProgressBarCopy.Foreground = Brushes.Yellow;
                Workers[idPreset - 1].Statut = "PAUSED";
                CopyStatut.Text = dictLang.CopyPause;
            }

            if (!workerSend.IsBusy && client != null)
            {
                string msg = JsonConvert.SerializeObject(Workers, Formatting.Indented);
                workerSend.RunWorkerAsync(msg);
            }


            if (Workers[idPreset - 1].worker.IsBusy && idPreset - 1 == idWorker)
            {
                CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {remainingFiles}";
                CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {remainingFilesSize}";
                ProgressBarCopy.Value = e.ProgressPercentage;
            }
        }

        /// <summary>
        /// Called when we double click on the listbox. Show the selected preset information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListPresetCopy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = ListPresetCopy.SelectedItem.ToString();
            int id = Convert.ToInt32(Preset.GetId(selectedItem));
            InfoCopy.Visibility = Visibility.Visible;
            CopyIdPreset.Text = id.ToString();
            CopyNamePreset.Text = $"{dictLang.CopyPreset} {Workers[id - 1].Name}";
            CopySource.Text = $"{dictLang.CopyPathSource} {Workers[id - 1].Source}";
            CopyDestination.Text = $"{dictLang.CopyPathDest} {Workers[id - 1].Destination}";
            //Si pas de copie
            if (Workers[id - 1].Statut == "INACTIVE")
            {
                ProgressCopy.Visibility = Visibility.Collapsed;
                CopyType.Text = "";
                CopyDate.Text = "";
                CopyNbFile.Text = "";
                CopySizeFile.Text = "";
                CopyStatut.Text = dictLang.CopyNo;
                ProgressBarCopy.Foreground = Brushes.Gray;
            }
            //Si copie en cours
            else if (Workers[id - 1].Statut == "ACTIVE")
            {
                ProgressCopy.Visibility = Visibility.Visible;
                CopyType.Text = $"{dictLang.CopyType} {Workers[id - 1].CopyType}";
                CopyDate.Text = $"{dictLang.CopyDateStart} {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"{dictLang.CopyNbFiles} {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"{dictLang.CopyFileSize} {Workers[id - 1].TotalSize}";
                CopyStatut.Text = dictLang.CopyStillRunning;
                ProgressBarCopy.Value = Workers[id - 1].Progress;
                ProgressBarCopy.Foreground = Brushes.Green;
                CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }
            //Si copie annulé
            else if (Workers[id - 1].Statut == "CANCELLED")
            {
                ProgressCopy.Visibility = Visibility.Visible;
                CopyType.Text = $"{dictLang.CopyType} {Workers[id - 1].CopyType}";
                CopyDate.Text = $"{dictLang.CopyDateStart} {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"{dictLang.CopyNbFiles} {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"{dictLang.CopyFileSize} {Workers[id - 1].TotalSize}";
                CopyStatut.Text = dictLang.CopyCancelled;
                ProgressBarCopy.Value = Workers[id - 1].Progress;
                ProgressBarCopy.Foreground = Brushes.Red;
                CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }
            //Si copie terminée avec succès
            else if (Workers[id - 1].Statut == "FINISHED")
            {
                ProgressCopy.Visibility = Visibility.Visible;
                CopyType.Text = $"{dictLang.CopyType} {Workers[id - 1].CopyType}";
                CopyDate.Text = $"{dictLang.CopyDateStart} {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"{dictLang.CopyNbFiles} {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"{dictLang.CopyFileSize} {Workers[id - 1].TotalSize}";
                CopyStatut.Text = dictLang.CopySuccess;
                ProgressBarCopy.Foreground = Brushes.Green;
                ProgressBarCopy.Value = 100;
                CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {0}";
                CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {0}";
            }
            //Si copie en pause
            else if (Workers[id - 1].Statut == "PAUSED")
            {
                ProgressCopy.Visibility = Visibility.Visible;
                CopyType.Text = $"{dictLang.CopyType} {Workers[id - 1].CopyType}";
                CopyDate.Text = $"{dictLang.CopyDateStart} {Workers[id - 1].DateStart}";
                CopyNbFile.Text = $"{dictLang.CopyNbFiles} {Workers[id - 1].TotalFiles}";
                CopySizeFile.Text = $"{dictLang.CopyFileSize} {Workers[id - 1].TotalSize}";
                CopyStatut.Text = dictLang.CopyPause;
                ProgressBarCopy.Value = Workers[id - 1].Progress;
                ProgressBarCopy.Foreground = Brushes.Yellow;
                CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {Workers[id - 1].RemainingFiles}";
                CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {Workers[id - 1].RemainingFilesSize}";
            }
        }

        /// <summary>
        /// If we click on application, the pannel appear and the current application is displayed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            EditApplicationPannel.Visibility = Visibility.Visible;
            WorkSoft application = WorkSoftware.GetJsonApplication();
            EditApplicationTextbox.Text = application.Application;
        }

        /// <summary>
        /// If we click on confirm edit application, the application is added and the textbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmEditApplication_Click(object sender, RoutedEventArgs e)
        {
            string newApplication = EditApplicationTextbox.Text;
            WorkSoftware.EditApplication(newApplication);
            ListApplication.Items.Clear();
            WorkSoft application = WorkSoftware.GetJsonApplication();
            ListApplication.Items.Add(application.Application);
            EditApplicationPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Make the pannel to edit an application disappear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEditApplication_Click(object sender, RoutedEventArgs e)
        {
            EditApplicationPannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// If we click on edit a stockage, the pannel appear.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditStockageButton_Click(object sender, RoutedEventArgs e)
        {
            EditStockagePannel.Visibility = Visibility.Visible;
            RadioJson.IsChecked = false;
            RadioXml.IsChecked = false;
        }

        /// <summary>
        /// If we click on confirm edit stockage, the stockage is edited and the textbox is updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmEditStockage_Click(object sender, RoutedEventArgs e)
        {
            JsonXml stockage = Stockage.GetJsonStockage();
            string stock = stockage.TypeStockage;
            if (RadioJson.IsChecked == true)
            {
                stock = ".json";
            }
            else if (RadioXml.IsChecked == true)
            {
                stock = ".xml";
            }
            Stockage.EditStockage(stock);
            LabelCurrentStockage.Content = $"{dictLang.OptStocNow} {stock}";
            EditStockagePannel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Hide the pannel to edit a stockage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelEditStockage_Click(object sender, RoutedEventArgs e)
        {
            EditStockagePannel.Visibility = Visibility.Collapsed;
        }

        public void GenerateGridLang()
        {
            int Lines = GetLangLines().Item1;
            string[] LangList = GetLangLines().Item2;
            int countC = 0;
            int countR = 0;
            string PathLang = "./data/lang/";

            //string PathDataImg = "./data/imgs/";


            foreach (string Lang in LangList)
            {
                var bc = new BrushConverter();

                Button butt = new Button();
                //butt.Background = new Brush.Grey;
                butt.Margin = new Thickness(4);

                string name = Lang.Substring(PathLang.Length);
                int length = name.Length;
                name = name.Remove(length - 5);

                butt.Tag = name;

                butt.Click += new RoutedEventHandler(ChangeLang_Click);

                string PathImg = "imgs/" + name + ".png";

                Image img = new Image();
                BitmapImage ImgBmp = new BitmapImage(new Uri(PathImg, UriKind.Relative));
                img.Stretch = Stretch.Fill;
                img.Source = ImgBmp;
                img.Margin = new Thickness(10);

                StackPanel stackPnl = new StackPanel();
                stackPnl.Orientation = Orientation.Horizontal;
                //stackPnl.Margin = new Thickness(10);
                stackPnl.Children.Add(img);

                butt.Content = stackPnl;

                LangPannel.Children.Add(butt);

                Grid.SetColumn(butt, countC);
                Grid.SetRow(butt, countR);
                countC += 1;
                if (countC == 3)
                {
                    countC = 0;
                    countR += 1;
                }
            }

            //Dans ma fonction : var theValue = button.Attributes["Tag"].ToString()
        }

        public (int, string[]) GetLangLines()
        {
            string[] LangList = Langue.GetFileName();
            int CountLang = LangList.Count();
            int Lines = (CountLang / 3) + 1;
            int ColumnLast = (CountLang % 3);
            if (ColumnLast == 0) Lines -= 1;
            return (Lines, LangList);
        }

        static private void ChangeLang_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button is null) return;
            string NewLang = (string)button.Tag;

            StreamReader sr = Langue.GetConfig();
            string jsonString = sr.ReadToEnd();
            Config conf = JsonConvert.DeserializeObject<Config>(jsonString);
            sr.Dispose();

            string text = File.ReadAllText("./data/config.json");
            text = text.Replace(conf.Lang, NewLang);

            File.WriteAllText("./data/config.json", text);

            MessageBox.Show("Langue changée pour " + NewLang);

            dictLang = Langue.GetLang();

            //return newDictLang;
        }

        public static void CheckProcesses()
        {
            var app = WorkSoftware.GetJsonApplication();
            Process[] pname = Process.GetProcessesByName(app.Application);

            if (pname.Length > 0)
            {
                foreach (var work in Workers)
                {
                    work.PreviousStatut = work.Statut;
                    if (work.Statut == "ACTIVE")
                    {
                        work.WorkEvent.Reset();
                        work.Statut = "PAUSED";
                    }

                }
                bool isLocked = true;
                while (isLocked)
                {
                    int tempSoft = Process.GetProcessesByName(app.Application).Length;
                    if (tempSoft == 0)
                    {
                        isLocked = false;
                    }
                }
                foreach (var work in Workers)
                {
                    if (work.PreviousStatut == "ACTIVE")
                    {
                        work.WorkEvent.Set();
                        work.Statut = "ACTIVE";
                    }
                }
            }
        }

        public static void PauseAllExceptOne(int id, object sender)
        {
            foreach (var work in Workers)
            {
                if (work.Id != id)
                {
                    work.PreviousStatut = work.Statut;
                    if (work.Statut == "ACTIVE")
                    {
                        work.WorkEvent.Reset();
                        work.Statut = "PAUSED";
                    }
                }
            }
        }
        public static void PlayAllExceptOne(int id)
        {
            foreach (var work in Workers)
            {
                if (work.Id != id)
                {
                    //work.Statut = work.PreviousStatut;
                    work.WorkEvent.Set();
                    work.Statut = "ACTIVE";
                }
            }

        }
        private void PlayCopy_Click(object sender, RoutedEventArgs e)
        {
            var app = WorkSoftware.GetJsonApplication();
            Process[] pname = Process.GetProcessesByName(app.Application);
            if (ProgressBarCopy.Foreground == Brushes.Yellow && Process.GetProcessesByName(app.Application).Length == 0)
            {
                int idPreset = Convert.ToInt32(CopyIdPreset.Text);
                Workers[idPreset - 1].Statut = "ACTIVE";
                CopyStatut.Text = dictLang.CopyStillRunning;
                Workers[idPreset - 1].WorkEvent.Set();
                ProgressBarCopy.Foreground = Brushes.Green;
            }
        }

        private void PauseCopy_Click(object sender, RoutedEventArgs e)
        {
            int idPreset = Convert.ToInt32(CopyIdPreset.Text);
            if (Workers[idPreset - 1].worker.IsBusy && ProgressBarCopy.Foreground != Brushes.Red)
            {
                ProgressBarCopy.Foreground = Brushes.Yellow;
                Workers[idPreset - 1].Statut = "PAUSED";
                CopyStatut.Text = dictLang.CopyPause;
                Workers[idPreset - 1].WorkEvent.Reset();
            }
            if (client != null)
            {
                BackgroundWorker workerSendUpdate = new BackgroundWorker();
                workerSendUpdate.DoWork += worker_DoWorkSend;
                string message = JsonConvert.SerializeObject(Workers, Formatting.Indented);
                workerSendUpdate.RunWorkerAsync(message);
            }
        }

        private void StopCopy_Click(object sender, RoutedEventArgs e)
        {
            int idPreset = Convert.ToInt32(CopyIdPreset.Text);
            if (Workers[idPreset - 1].worker.IsBusy)
            {
                ProgressBarCopy.Foreground = Brushes.Red;
                Workers[idPreset - 1].Statut = "CANCELLED";
                CopyStatut.Text = dictLang.CopyCancelled;
                Workers[idPreset - 1].worker.CancelAsync();
            }
        }

        //BACKGROUNDWORKER ACCES DISTANCE

        /// <summary>
        /// Called when we click on "copy", this is a separated thread listening continuously the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWorkListen(object sender, DoWorkEventArgs e)
        {
            server = Server.SeConnecter();
            client = Server.AccepterConnection(server);
            //string msg = e.Argument as string;
            Server.EcouterReseau(client, sender);
        }

        /// <summary>
        /// Called when there is a progress every 0,5 second to send data to the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_DoWorkSend(object sender, DoWorkEventArgs e)
        {
            string message = e.Argument as string;
            Server.SendMsg(message, client);
        }

        /// <summary>
        /// Called when the server stop listening. But it never happens anyways.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompletedListen(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        /// <summary>
        /// Called when the message was sent successfully, but not used so far.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_RunWorkerCompletedSend(object sender, RunWorkerCompletedEventArgs e)
        {
            //ReceivedMsg.Text = "Text";
        }


        private void TestDistance_Click(object sender, RoutedEventArgs e)
        {
            string msg = JsonConvert.SerializeObject(Workers, Formatting.Indented);
            BackgroundWorker workerSend = new BackgroundWorker();
            //string msg = TestDistanceText.Text;
            workerSend.DoWork += worker_DoWorkSend;
            workerSend.RunWorkerCompleted += worker_RunWorkerCompletedSend;
            workerSend.WorkerSupportsCancellation = true;
            workerSend.RunWorkerAsync(msg);
        }

        /// <summary>
        /// Called when the server receive a message from the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_ProgressChangedListen(object sender, ProgressChangedEventArgs e)
        {
            string msg = e.UserState as string;
            List<int> messageComplet = JsonConvert.DeserializeObject<List<int>>(msg);
            TestDistanceText.Text = messageComplet[1].ToString();
            int idPreset = Convert.ToInt32(CopyIdPreset.Text);
            int idMessage = messageComplet[0];

            //Si le client clique sur stop
            if (messageComplet[1] == 0)
            {
                if (Workers[idMessage - 1].worker.IsBusy)
                {
                    Workers[idMessage - 1].Statut = "CANCELLED";
                    CopyStatut.Text = dictLang.CopyCancelled;
                    Workers[idMessage - 1].worker.CancelAsync();
                    if (idMessage == idPreset)
                    {
                        ProgressBarCopy.Foreground = Brushes.Red;
                    }
                }
            }

            //Si le client clique sur pause
            else if (messageComplet[1] == 1)
            {
                if (Workers[idMessage - 1].Statut == "ACTIVE")
                {
                    Workers[idMessage - 1].Statut = "PAUSED";
                    CopyStatut.Text = dictLang.CopyPause;
                    Workers[idMessage - 1].WorkEvent.Reset();
                    if (idMessage == idPreset)
                    {
                        ProgressBarCopy.Foreground = Brushes.Yellow;
                    }
                    if (client != null)
                    {
                        BackgroundWorker workerSendUpdate = new BackgroundWorker();
                        workerSendUpdate.DoWork += worker_DoWorkSend;
                        string message = JsonConvert.SerializeObject(Workers, Formatting.Indented);
                        workerSendUpdate.RunWorkerAsync(message);
                    }
                }
            }

            //Si le client clique sur play
            else if (messageComplet[1] == 2)
            {
                if (Workers[idMessage - 1].Statut == "PAUSED")
                {
                    Workers[idMessage - 1].WorkEvent.Set();
                    CopyStatut.Text = dictLang.CopyStillRunning;
                    Workers[idMessage - 1].Statut = "ACTIVE";
                    if (idMessage == idPreset)
                    {
                        ProgressBarCopy.Foreground = Brushes.Green;
                    }
                }
            }
        }
    }
}
