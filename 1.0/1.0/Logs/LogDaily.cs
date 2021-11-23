using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;

namespace Projet.Logs
{
    public class LogDaily : LogBase
    {
        long FilesSize { get; set; }
        long TransferTime { get; set; }

        public LogDaily(string name, string sourceDir, string targetDir, DateTimeOffset timestamp, long filesSize, long transferTime)
        {
            Name = name;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            Timestamp = timestamp;
            FilesSize = filesSize;
            TransferTime = transferTime;
        }

        public void Save()
        {
            DateTime localTime = DateTime.Now;

            string _currentDay = localTime.Day.ToString() + "/";
            _currentDay += localTime.Month.ToString() + "/";
            _currentDay += localTime.Year.ToString();
            string fileName = "./Daily/" + _currentDay;

            if (File.Exists(fileName))
            {
                Console.WriteLine("Faut faire le système de sauvegarde sur un fichier existant");
            }
            else
            {
                
                CreatePath(localTime);
                string jsonString = JsonSerializer.Serialize(this);
                File.WriteAllText(fileName, jsonString);
            }
        }
        private void CreatePath(DateTime localTime, bool first = true)
        {
            string dirName = "./Daily/";
            dirName += localTime.Year.ToString();
            if (first) dirName += "/" + localTime.Month.ToString();

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            if (first) CreatePath(localTime, false);

        }

    }
}


