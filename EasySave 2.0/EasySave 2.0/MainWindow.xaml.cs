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

namespace EasySave_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Langue.Language dictLang = Langue.GetLang();

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
            ListPresetCopy.Items.Clear();
            RadioCopyPartial.Content = dictLang.PartialCopy;
            RadioCopyComplet.Content = dictLang.CompletCopy;
            ChooseCopyType.Content = dictLang.ChooseTypeCopy;
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPresetCopy.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
        }

        public void OptionsBouton_Click(object sender, RoutedEventArgs e)
        {
            LangButton.Content = dictLang.OptMLang;
            PresetButton.Content = dictLang.OptMPreset;
            ExtensionButton.Content = dictLang.OptMExt;
            ApplicationButton.Content = dictLang.OptMApp;
            StockageButton.Content = dictLang.OptMStoc;
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
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Collapsed;
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            CopyPannel.Visibility = Visibility.Collapsed;
            OptionsPannel.Visibility = Visibility.Collapsed;
            LogsPannel.Visibility = Visibility.Visible;
            //LogsGrid.Items.Add("test");

            //LogDaily dailyLog = new LogDaily("");
            //string ad = @"./data/Logs/Daily/2021/12/6";
            //DirectoryInfo dirInfo = dailyLog.GetFiles(ad);
            //if (dirInfo == null) return;
            //List<LogDaily> logs = dailyLog.Load(dirInfo.FullName);
            //LogsGrid.Items.Add(logs.Count);
            ////dgr.ItemsSource = logs.response.ResultSet;
            //foreach (LogDaily log in logs)
            //{
            //    LogsGrid.Items.Add("Test");
            //    LogsGrid.Items.Add(log.Name);
            //}

            var logs = LogDaily.GetJsonLogs();
            LogsGrid.DataContext = logs;
            //LogsGrid.DataBind();


            //List<LogJson> logs = JsonConvert.DeserializeObject<List<LogJson>>(temp);


        }

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
        }
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
            Dictionary<string, string> extension = Extension.GetJsonExtension();
            int nbExtension = extension.Count;
            for (int i = 1; i <= nbExtension; i++)
            {
                ListExtension.Items.Add(i.ToString() + $" - {extension["Extension" + i.ToString()]}");
            }
        }

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
            WorkSoft application = WorkSoftware.GetJsonApplication();
            ListApplication.Items.Add(application.Application);
        }

        private void StockageButton_Click(object sender, RoutedEventArgs e)
        {
            LangPannel.Visibility = Visibility.Collapsed;
            PresetPannel.Visibility = Visibility.Collapsed;
            ExtensionPannel.Visibility = Visibility.Collapsed;
            ApplicationPannel.Visibility = Visibility.Collapsed;
            StockagePannel.Visibility = Visibility.Visible;
            JsonXml stockage = Stockage.GetJsonStockage();
            LabelCurrentStockage.Content = dictLang.OptStocNow + stockage.TypeStockage;
            EditStockageButton.Content = dictLang.OptStocAlter;
            LabelEditStockage.Content = dictLang.OptStocNew;
            ConfirmEditStockage.Content = dictLang.Confirm;
            CancelEditStockage.Content = dictLang.Cancel;
        }

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
            Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
            int nbPreset = preset.Count;
            for (int i = 1; i <= nbPreset; i++)
            {
                ListPreset.Items.Add(i.ToString() + $" - {preset["Preset" + i.ToString()].Name}");
            }
        }

        private void AddPresetButton_Click(object sender, RoutedEventArgs e)
        {
            LabelAddPreset.Content = dictLang.OptPreAdd;
            LabelNameAddPreset.Content = dictLang.Name;
            LabelSourceAddPreset.Content = dictLang.Sauce;
            LabelDestinationAddPreset.Content = dictLang.Dest;
            ConfirmAddPreset.Content = dictLang.Confirm;
            CancelAddPreset.Content = dictLang.Cancel;
            AddNameTextbox.Text = "";
            AddPathSourceTextbox.Text = "";
            AddPathDestinationTextbox.Text = "";
            AddPannel.Visibility = Visibility.Visible;
            EditPannel.Visibility = Visibility.Collapsed;
            DeletePannel.Visibility = Visibility.Collapsed;
        }

        private void EditPresetButton_Click(object sender, RoutedEventArgs e)
        {
            LabelEditPreset.Content = dictLang.OptPreEdit;
            LabelNameEditPreset.Content = dictLang.Name;
            LabelSourceEditPreset.Content = dictLang.Sauce;
            LabelDestinationEditPreset.Content = dictLang.Dest;
            ConfirmEditPreset.Content = dictLang.Confirm;
            CancelEditPreset.Content = dictLang.Cancel;
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
            LabelAddExtension.Content = dictLang.OptExtAdd;
            LabelAddNewExtension.Content = dictLang.Extension;
            ConfirmAddExtension.Content = dictLang.Confirm;
            CancelAddExtension.Content = dictLang.Cancel;

            AddExtensionTextbox.Text = "";
            AddExtensionPannel.Visibility = Visibility.Visible;
            EditExtensionPannel.Visibility = Visibility.Collapsed;
            DeleteExtensionPannel.Visibility = Visibility.Collapsed;
        }

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
            ErrorCopy.Content = "VOMI";

            if ((RadioCopyComplet.IsChecked == true || RadioCopyPartial.IsChecked == true) && ListPresetCopy.SelectedIndex != -1)
            {
                Dictionary<string, NameSourceDest> preset = Preset.GetJsonPreset();
                string presetId = ListPresetCopy.SelectedItem.ToString().Substring(0, 1);
                string name = preset["Preset" + presetId].Name;
                string source = preset["Preset" + presetId].PathSource;
                string destination = preset["Preset" + presetId].PathDestination;
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

                    //ErrorCopy.Visibility = Visibility.Visible;
                }
                else
                {
                    InfoCopy.Visibility = Visibility.Visible;
                    ProgressCopy.Visibility = Visibility.Visible;
                    CopyType.Text = $"{dictLang.CopyType} {copyType}";
                    CopyNamePreset.Text = $"{dictLang.CopyPreset} {name}";
                    CopySource.Text = $"{dictLang.CopyPathSource} {source}";
                    CopyDestination.Text = $"{dictLang.CopyPathDest} {destination}";

                    var staticLog = save.CurrentStateLog;

                    CopyDate.Text = $"{dictLang.CopyDateStart} {staticLog.Timestamp}";
                    CopyNbFile.Text = $"{dictLang.CopyNbFiles} {staticLog.TotalFiles}";
                    CopySizeFile.Text = $"{dictLang.CopyFileSize} {staticLog.TotalSize}";


                    save.ProcessCopy(DirInfo.source, DirInfo.target, ProgressBarCopy);

                    /*while (staticLog.Progress < 100)
                    {
                        CopyFileRemaining.Content = $"{dictLang.CopyFileRemaining} {staticLog.RemainingFiles}";
                        CopySizeRemaining.Content = $"{dictLang.CopyFileSizeRemaining} {staticLog.RemainingFilesSize}";

                    }*/
                    CopyEnd.Text = dictLang.CopySuccess;
                }
            }
            else
            {
                //ErrorCopy.Visibility = Visibility.Visible;
                ErrorCopy.Content = dictLang.ErrorChooseTypePreset;
            }
        }

        private void EditApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            EditApplicationPannel.Visibility = Visibility.Visible;
            WorkSoft application = WorkSoftware.GetJsonApplication();
            EditApplicationTextbox.Text = application.Application;
        }

        private void ConfirmEditApplication_Click(object sender, RoutedEventArgs e)
        {
            string newApplication = EditApplicationTextbox.Text;
            WorkSoftware.EditApplication(newApplication);
            ListApplication.Items.Clear();
            WorkSoft application = WorkSoftware.GetJsonApplication();
            ListApplication.Items.Add(application.Application);
            EditApplicationPannel.Visibility = Visibility.Collapsed;
        }

        private void CancelEditApplication_Click(object sender, RoutedEventArgs e)
        {
            EditApplicationPannel.Visibility = Visibility.Collapsed;
        }

        private void EditStockageButton_Click(object sender, RoutedEventArgs e)
        {
            EditStockagePannel.Visibility = Visibility.Visible;
            RadioJson.IsChecked = false;
            RadioXml.IsChecked = false;
        }

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
            LabelCurrentStockage.Content = $"Stockage actuelle: {stock}";
            EditStockagePannel.Visibility = Visibility.Collapsed;
        }

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

        //TODO: Finir le changement de Langue

    }
}
