using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using Projet.Languages;
using System.Xml;

namespace Projet.Logs
{

    public abstract class LogBase
    {
        protected string LogFile;
        public string Name { get; set; }
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
        protected DateTimeOffset DateTimeStamp;
        public string Timestamp;
        public bool IsJson;
        public LogBase()
        {
            LogFile = "./data/Logs/";
        }
        /// <summary>
        /// Create the day's path if it doesn't exists
        /// </summary>
        /// <param name="path">Path's base</param>
        protected void CreatePath(string path)
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        path += "/" + DateTimeStamp.Year.ToString();
                        break;
                    case 1:
                        path += "/" + DateTimeStamp.Month.ToString();
                        break;
                    case 2:
                        path += "/" + DateTimeStamp.Day.ToString();
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

        /// <summary>
        /// Returns folder architecture depending on date
        /// </summary>
        /// <returns>/YEAR/MONTH/DAY</returns>
        protected string GetDay()
        {
            string currentDay = "/" + DateTimeStamp.Year.ToString();
            currentDay += "/" + DateTimeStamp.Month.ToString();
            currentDay += "/" + DateTimeStamp.Day.ToString();
            return currentDay;
        }
        /// <summary>
        /// Return file's architecture depending on hour and minute
        /// </summary>
        /// <returns>/HOURhMINUTE.json</returns>
        protected string GetMinute()
        {
            string currentMin = "/" + DateTimeStamp.Hour.ToString();
            currentMin += "h" + DateTimeStamp.Minute.ToString();
            if (IsJson)
            {
                currentMin += ".json";
            }
            else
            {
                currentMin += ".xml";
            }
            return currentMin;
        }

    }

}


