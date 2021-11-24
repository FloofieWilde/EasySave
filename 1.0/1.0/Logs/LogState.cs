using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Projet.Logs
{
    public class LogState : LogBase
    {
        public bool Active { get; set; } = true;
        public int Progress { get; set; }
        public long RemainingFiles { get; set; }
        public long RemainingFilesSize { get; set; }

        bool FirstTime = true;

        public LogState(string name, string sourceDir, string targetDir, long totalFiles, long totalFilesSize)
        {
            Progress = 0;
            Name = name;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            Timestamp = now;
            RemainingFiles = totalFiles;
            RemainingFilesSize = totalFilesSize;
            TotalFiles = totalFiles;
            TotalSize = totalFilesSize;
        }
        
        public void Display(bool firstTime = false)
        {
            if (FirstTime)
            {
                string tempTimestamp = Timestamp.ToString("yyyy/MM/dd - HH:mm:ss - fff");
                Console.WriteLine("WORK IN PROGRESS");
                Console.WriteLine("Type : " + Name);
                Console.WriteLine("Source directory : " + SourceDir);
                Console.WriteLine("Target directory : " + TargetDir);
                Console.WriteLine("Start timestamp : " + tempTimestamp);
                Console.WriteLine("Total files : " + TotalFiles);
                Console.WriteLine("Total files size : " + TotalSize);
                FirstTime = false;
            }
            string LogString = "Progress : " + Progress + "% | Remaining files : " + RemainingFiles + " | Remaining files size : " + RemainingFilesSize + "     ";
            Console.Write("\r{0}", LogString);

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
            Save("State");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SAVE SUCCESSFULLY COMPLETED");
            Console.ResetColor();

            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
            TimeSpan totalTime = now - Timestamp;
            double totalMs = totalTime.TotalMilliseconds;

            LogDaily dailyLog = new LogDaily(Name);
            dailyLog.Save("Daily");
        }

    }
}