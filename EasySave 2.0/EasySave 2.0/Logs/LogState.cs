using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Projet.Languages;
using Projet;

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

        bool FirstTime = true;
        public long TotalFiles { get; set; }
        public long TotalSize { get; set; }
        private Langue.Language CurrentLanguage;

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
        }
        /// <summary>
        /// Displays current status on the console
        /// </summary>
        /// <param name="firstTime"></param>
        public void Display()
        {

            if (FirstTime)
            {
                Console.WriteLine(CurrentLanguage.SaveWip);
                Console.WriteLine(CurrentLanguage.SaveType + " : " + Name);
                Console.WriteLine(CurrentLanguage.SaveSauce + " : " + SourceDir);
                Console.WriteLine(CurrentLanguage.SaveTarget + " : " + TargetDir);
                Console.WriteLine(CurrentLanguage.SaveStarttime + " : " + Timestamp);
                Console.WriteLine(CurrentLanguage.SaveTotalFile + " : " + TotalFiles);
                Console.WriteLine(CurrentLanguage.SaveTotalSize + " : " + TotalSize);
                FirstTime = false;
            }
            string LogString = CurrentLanguage.SaveProgress1+ " : " + Progress + "% | " + CurrentLanguage.SaveProgress2 + " : " + RemainingFiles + " | " + CurrentLanguage.SaveProgress3 + " : " + RemainingFilesSize + "     ";
            Console.Write("\r{0}", LogString);

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
            string usedPath = testPath + GetDay() + GetMinute();

            if (!File.Exists(usedPath)) CreatePath(testPath);

            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(usedPath, jsonString);
        }
        /// <summary>
        /// Called when save process is finished, saves a last time object's datas then display the sucessfull save message
        /// </summary>
        public void End()
        {
            Active = false;
            Save();
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(CurrentLanguage.SaveSuccess);
            Console.ResetColor();
            Menu.MenuPrincipal();
        }
    }
}