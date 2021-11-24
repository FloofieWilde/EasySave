using System;
using System.IO;
using System.Text.Json;

namespace Projet.Logs
{
    /// <summary>
    ///  This class manages operations related to daily logs
    /// </summary>
    public class LogDaily : LogBase
    {
        public long FileSize { get; set; }
        public double TransferTime { get; set; }

        public LogDaily(string name)
        {
            Name = name;

            DateTimeStamp = (DateTimeOffset)DateTime.UtcNow;
            Timestamp = DateTimeStamp.ToString("yyyy/MM/dd - HH:mm:ss - fff");
            Save();

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

            string jsonString = JsonSerializer.Serialize(this);
            File.AppendAllText(usedPath, jsonString);

        }
    }
}


