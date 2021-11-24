using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace Projet.Logs
{
    public abstract class LogBase
    {
        protected string LogFile;
        public string Name { get; set; }
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long TotalFiles { get; set; }
        public long TotalSize { get; set; }
        public LogBase()
        {
            LogFile = @"C:/Users/franc/source/repos/C-hashtag-point-web/1.0/1.0/Logs/LogsData/";
        }
        protected void CreatePath(string path)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        path += "/" + Timestamp.Year.ToString();
                        break;
                    case 1:
                        path += "/" + Timestamp.Month.ToString();
                        break;
                    case 2:
                        path += "/" + Timestamp.Day.ToString();
                        break;
                    default:
                        return;
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

        }
        public void Save(string folder)
        {
            string testPath = LogFile + folder;
            string usedPath = testPath + GetDay() + GetMinute();
            
            if (!File.Exists(usedPath)) CreatePath(testPath);

            string jsonString = JsonSerializer.Serialize(this);
            if (folder == "State") File.WriteAllText(usedPath, jsonString);
            else File.AppendAllText(usedPath, jsonString);

        }

        private string GetDay()
        {
            string currentDay = "/" + Timestamp.Year.ToString();
            currentDay += "/" + Timestamp.Month.ToString();
            currentDay += "/" + Timestamp.Day.ToString();
            return currentDay;
        }
        private string GetMinute()
        {
            string currentMin = "/" + Timestamp.Hour.ToString();
            currentMin += "_" + Timestamp.Minute.ToString();
            currentMin += ".json";
            return currentMin;
        }

    }

}


