using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text.Json;
using Projet.Languages;

namespace Projet.Logs
{
    /// <summary>
    ///  This class manages operations related to daily logs
    /// </summary>
    public class LogDaily : LogBase
    {
        public long FileSize { get; set; }
        public double TransferTime { get; set; }
        private Langue.Language CurrentLanguage;

        public LogDaily(string name)
        {
            Name = name;

            DateTimeStamp = (DateTimeOffset)DateTime.UtcNow;
            Timestamp = DateTimeStamp.ToString("yyyy/MM/dd - HH:mm:ss - fff");
            CurrentLanguage = Langue.GetLang();

        }
        /// <summary>
        /// Updates object's datas and calls Save() to save them into a json file
        /// </summary>
        /// <param name="filesize">Size of the file</param>
        /// <param name="transferTime">Total time taken to process the file</param>
        /// <param name="sourceDir">Source directory</param>
        /// <param name="targetDir">Target directory</param>
        public void Update(long filesize, double transferTime, string sourceDir, string targetDir)
        {
            FileSize = filesize;
            TransferTime = transferTime;
            SourceDir = sourceDir;
            TargetDir = targetDir;
            Save();

        }
        /// <summary>
        /// Save object's data into a json file
        /// </summary>
        public void Save()
        {

            string testPath = LogFile + "Daily";
            string usedPath = testPath + GetDay() + GetMinute();

            if (!File.Exists(usedPath)) CreatePath(testPath);

            JsonSerializerOptions oui = new JsonSerializerOptions();

            oui.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            oui.WriteIndented = true;
            string jsonString = System.Text.Json.JsonSerializer.Serialize<LogDaily>(this, oui);
            File.AppendAllText(usedPath, jsonString);

        }
        public void Load()
        {
            string path = LogFile + "Daily" + GetDay();
            if (!Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(CurrentLanguage.LogNoDaily);
                Console.ResetColor();
            }
            else
            {
                string[] files = Directory.GetFiles(path);
                int it = 0;
                foreach (string file in files)
                {
                    it++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(CurrentLanguage.LogDailyFilen + " : " + it + " / " + files.Length);
                    Console.ResetColor();
                    string[] json = File.ReadAllLines(file);
                    int i = 0;
                    int j = 0;
                    foreach (string line in json)
                    {
                        if (line.StartsWith("}"))
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine(CurrentLanguage.LogNumber + " : " + i);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(line);

                        }
                        j++;
                        if (j >= 6)
                        {
                            i++;
                            j = 0;
                        }
                    }
                }
            }
            Menu.MenuPrincipal();
        }
    }
}


