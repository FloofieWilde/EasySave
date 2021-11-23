using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Projet.Logs
{
    public class LogState : LogBase
    {
        public bool Active { get; set; } = false;
        public int Progress { get; set; }
        public long RemainingFiles { get; set; }
        public long RemainingFilesSize { get; set; }
        
        public LogState(string name, string sourceDir, string targetDir, long totalFiles, long totalFilesSize)
        {
            Progress = 0;
            Name = name;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            Timestamp = now.ToString("yyyy/MM/dd - HH:mm:ss - fff");
            RemainingFiles = totalFiles;
            RemainingFilesSize = totalFilesSize;
            TotalFiles = totalFiles;
            TotalSize = totalFilesSize;
        }
        public void Save()
        {
            string fileName = @"C:\Users\franc\source\repos\C-hashtag-point-web\1.0\1.0\Logs\LogsData/Current.json";

            if (File.Exists(fileName)) File.Delete(fileName);
            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(fileName, jsonString);
        }
        public void Display()
        {
            if (Active) Console.WriteLine("WORK IN PROGRESS");
            else Console.WriteLine("SAVE ENDED");
            Console.WriteLine("Name : " + Name);
            Console.WriteLine("Source directory : " + SourceDir);
            Console.WriteLine("Target directory : " + TargetDir);
            Console.WriteLine("Start timestamp : " + Timestamp);
            Console.WriteLine("Progress : " + Progress + "%");
            Console.WriteLine("Remaining files : " + RemainingFiles);
            Console.WriteLine("Remaining files size : " + RemainingFilesSize);
            Console.WriteLine("Total files : " + TotalFiles);
            Console.WriteLine("Total files size : " + TotalSize);
        }
        public void Update(long remainingFilesSize)
        {
            RemainingFiles--;
            RemainingFilesSize -= remainingFilesSize;
            float tempRemain = (float)((TotalSize - RemainingFilesSize) / 100);
            float tempTotal = (float)(TotalSize / 100);

            float tempoSize = tempRemain / tempTotal;
            Progress = (int)(tempoSize * 100);
        }

        public void End()
        {
            Active = false;
            Save();
            Console.WriteLine("J'ai FINI (En vrai faudrait que je finisse ça");
        }

    }
}