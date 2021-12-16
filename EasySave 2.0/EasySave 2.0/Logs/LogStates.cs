using Newtonsoft.Json;
using Projet.Save;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.Logs
{
    public class LogStates
    {
        /// <summary>
        /// Method to get the logstate json (unused so far, but might be useful for later use)
        /// </summary>
        /// <returns>logstates</returns>
        public static List<Worker> GetJsonLogState()
        {
            string json = File.ReadAllText("./data/preset/preset.json");
            List<Worker> logstates = JsonConvert.DeserializeObject<List<Worker>>(json);
            return logstates;
        }

        /// <summary>
        /// Write in the logstate json file in real time
        /// </summary>
        /// <param name="Workers"></param>
        public static void UpdateJsonLogState(List<Worker> Workers)
        {
            string json = JsonConvert.SerializeObject(Workers, Formatting.Indented);
            try
            {
                File.WriteAllText("./data/Logs/logstate.json", json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
