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
        private long _filesSize { get; set; }
        private long _transferTime { get; set; }

        public LogDaily(string name, string sourceDir, string targetDir, string timestamp, long filesSize, long transferTime)
        {
            Name = name;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            Timestamp = timestamp;
            _filesSize = filesSize;
            _transferTime = transferTime;
        }

        public void Save(LogDaily logToWrite)
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
                string jsonString = JsonSerializer.Serialize(logToWrite);
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


