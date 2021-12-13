using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.IO;
using System.Text.Json;
using Projet.Languages;
using Projet.Stockages;

namespace Projet.Logs
{
    /// <summary>
    ///  This class manages operations related to daily logs
    /// </summary>
    public class LogDaily : LogBase
    {
        public long FileSize { get; set; }
        public double TransferTime { get; set; }
        public long EncryptTime { get; set; }
        private Langue.Language CurrentLanguage;
        XmlDocument XmlDoc = new XmlDocument();
        long XmlCount = 0;

        public LogDaily(string name)
        {
            Name = name;

            DateTimeStamp = (DateTimeOffset)DateTime.UtcNow;
            Timestamp = DateTimeStamp.ToString("yyyy/MM/dd - HH:mm:ss - fff");
            CurrentLanguage = Langue.GetLang();
            var extensionType = Stockage.GetJsonStockage();
            if (extensionType.TypeStockage == ".json") IsJson = true;
            else IsJson = false;

        }
        /// <summary>
        /// Updates object's datas and calls Save() to save them into a json file
        /// </summary>
        /// <param name="filesize">Size of the file</param>
        /// <param name="transferTime">Total time taken to process the file</param>
        /// <param name="sourceDir">Source directory</param>
        /// <param name="targetDir">Target directory</param>
        public void Update(long filesize, double transferTime, string sourceDir, string targetDir, long cryptTime)
        {
            FileSize = filesize;
            TransferTime = transferTime;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            EncryptTime = cryptTime;
            Save();

        }
        /// <summary>
        /// Save object's data into a json file
        /// </summary>
        public void Save()
        {

            string testPath = LogFile + "Daily";
            string FilePath = testPath + GetDay() + GetMinute();

            if (!File.Exists(FilePath))
            {
                CreatePath(testPath);
                if (!IsJson) XmlDoc.LoadXml("<Root></Root>");

            }

            JsonSerializerOptions oui = new JsonSerializerOptions();

            oui.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            oui.WriteIndented = true;
            string jsonString = System.Text.Json.JsonSerializer.Serialize<LogDaily>(this, oui);

            if (!IsJson)
            {

                XmlNode Root = XmlDoc.DocumentElement;

                XmlNode saveName = XmlDoc.CreateElement($"Save{XmlCount}");
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

                XmlNode fileSize = XmlDoc.CreateElement("FileSize");
                fileSize.InnerText = FileSize.ToString();
                saveName.InsertAfter(fileSize, saveName.LastChild);

                XmlNode transferTime = XmlDoc.CreateElement("TransferTime");
                transferTime.InnerText = TransferTime.ToString();
                saveName.InsertAfter(transferTime, saveName.LastChild);

                XmlNode cryptTime = XmlDoc.CreateElement("CryptTime");
                cryptTime.InnerText = EncryptTime.ToString();
                saveName.InsertAfter(cryptTime, saveName.LastChild);

                XmlDoc.Save(FilePath);
            }
            else
            {
                File.AppendAllText(FilePath, jsonString);

            }
            XmlCount++;
        }
        public List<LogDaily> Load(string path)
        {

            var fullLog = new List<LogDaily>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (path.EndsWith(".json"))
                {
                    string json = File.ReadAllText(file);
                    var logList = JsonConvert.DeserializeObject<List<LogDaily>>(json);
                    foreach (LogDaily logDaily in logList)
                    {
                        fullLog.Add(logDaily);
                        fullLog.Append<LogDaily>(logDaily);
                    }
                }
                else if(path.EndsWith(".xml"))
                {
                    XmlDoc.Load(path);
                    foreach(XmlNode xnode in XmlDoc.DocumentElement.ChildNodes)
                    {
                        string xstring = JsonConvert.SerializeXmlNode(xnode);
                        LogDaily finalLog = JsonConvert.DeserializeObject<LogDaily>(xstring);
                        fullLog.Append<LogDaily>(finalLog);
                        
                    }
                }

            }
            return fullLog;
        }

        


        public DirectoryInfo GetFiles(string path)
        {
            //string path = LogFile + "Daily" + GetDay();
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!Directory.Exists(path))
            {
                return null;
            }
            else
            {
                return dirInfo;
            }
        }
    }
}


