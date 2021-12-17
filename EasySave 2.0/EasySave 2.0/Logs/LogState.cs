using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Projet.Languages;
using Projet;
using System.Xml;
using Projet.Stockages;
using System.Windows.Controls;

namespace Projet.Logs
{
    /// <summary>
    ///  This class manages operations related to state logs
    /// </summary>
    public class LogState : LogBase
    {
        public bool Active { get; set; } = true;
        public int Progress { get; set; }
        public long RemainingFiles { get; set; }
        public long RemainingFilesSize { get; set; }

        public long TotalFiles { get; set; }
        public long TotalSize { get; set; }
        private Langue.Language CurrentLanguage;
        XmlDocument XmlDoc = new XmlDocument();

        public LogState(string name, string sourceDir, string targetDir, long totalFiles, long totalFilesSize)
        {
            Progress = 0;
            Name = name;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            DateTimeStamp = (DateTimeOffset)DateTime.UtcNow;
            Timestamp = DateTimeStamp.ToString("yyyy/MM/dd - HH:mm:ss - fff");
            RemainingFiles = totalFiles;
            RemainingFilesSize = totalFilesSize;
            TotalFiles = totalFiles;
            TotalSize = totalFilesSize;
            CurrentLanguage = Langue.GetLang();
            var extensionType = Stockage.GetJsonStockage();
            if (extensionType.TypeStockage == ".json") IsJson = true;
            else IsJson = false;
        }
        /// <summary>
        /// Displays current status on the console
        /// </summary>
        /// <param name="firstTime"></param>
        public (int Progress, long RemainingFiles, long RemainingFilesSize) Display()
        {
            return (Progress, RemainingFiles, RemainingFilesSize);
        }

        /// <summary>
        /// Update object's infos, without saving in json
        /// </summary>
        /// <param name="remainingFilesSize">Remaining files size</param>
        public void Update(long remainingFilesSize)
        {
            RemainingFiles--;
            RemainingFilesSize -= remainingFilesSize;
            float tempRemain = (float)((TotalSize - RemainingFilesSize) / 100);
            float tempTotal = (float)(TotalSize / 100);

            float tempoSize = tempRemain / tempTotal;
            Progress = (int)(tempoSize * 100);
        }
        /// <summary>
        /// Save object's data into a json
        /// </summary>
        public void Save()
        {
            string testPath = LogFile + "State";
            string FilePath = testPath + GetDay() + GetMinute();

            if (!File.Exists(FilePath))
            {
                CreatePath(testPath);
                if (!IsJson) XmlDoc.LoadXml("<Root></Root>");

            }
            if (!IsJson)
            {

                XmlNode Root = XmlDoc.DocumentElement;

                XmlNode saveName = XmlDoc.CreateElement($"Save");
                Root.InsertAfter(saveName, Root.LastChild);

                XmlNode name = XmlDoc.CreateElement("Name");
                name.InnerText = Name;
                saveName.InsertAfter(name, saveName.LastChild);

                XmlNode sourceDir = XmlDoc.CreateElement("SourceDir");
                sourceDir.InnerText = SourceDir;
                saveName.InsertAfter(sourceDir, saveName.LastChild);

                XmlNode targetDir = XmlDoc.CreateElement("TargetDir");
                targetDir.InnerText = TargetDir;
                saveName.InsertAfter(targetDir, saveName.LastChild);

                XmlNode totalSize = XmlDoc.CreateElement("TotalSize");
                totalSize.InnerText = TotalSize.ToString();
                saveName.InsertAfter(totalSize, saveName.LastChild);

                XmlNode totalFiles = XmlDoc.CreateElement("TotalFiles");
                totalFiles.InnerText = TotalFiles.ToString();
                saveName.InsertAfter(totalFiles, saveName.LastChild);

                XmlNode active = XmlDoc.CreateElement("Active");
                active.InnerText = Active.ToString();
                saveName.InsertAfter(active, saveName.LastChild);

                XmlNode progress = XmlDoc.CreateElement("Progress");
                progress.InnerText = Progress.ToString();
                saveName.InsertAfter(progress, saveName.LastChild);

                XmlNode remainingFiles = XmlDoc.CreateElement("RemainingFiles");
                remainingFiles.InnerText = RemainingFiles.ToString();
                saveName.InsertAfter(remainingFiles, saveName.LastChild);

                XmlNode remainingFilesSize = XmlDoc.CreateElement("RemainingFilesSize");
                remainingFilesSize.InnerText = Progress.ToString();
                saveName.InsertAfter(remainingFilesSize, saveName.LastChild);

                XmlDoc.Save(FilePath);
            }
            else
            {
                string jsonString = JsonSerializer.Serialize(this);
                File.WriteAllText(FilePath, jsonString);

            }

        }
        /// <summary>
        /// Called when save process is finished, saves a last time object's datas then display the sucessfull save message
        /// </summary>
        public void End()
        {
            Active = false;
            Save();
            //progressBar.Value = 100;
        }
    }
}