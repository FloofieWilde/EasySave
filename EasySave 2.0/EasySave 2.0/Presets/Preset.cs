using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Projet.Languages;

namespace Projet.Presets
{
    /// <summary>
    /// Class to choose a preset, and edit it
    /// </summary>
    public class Preset
    {
        /// <summary>
        /// Method to get the Json file and convert it to a .NET object
        /// </summary>
        /// <returns>preset</returns>
        public static Dictionary<string, NameSourceDest> GetJsonPreset()
        {
            string json = File.ReadAllText("./data/preset/preset.json");
            Dictionary<string, NameSourceDest> preset = JsonConvert.DeserializeObject<Dictionary<string, NameSourceDest>>(json);
            return preset;
        }

        /// <summary>
        /// Method to edit the choosen preset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void EditPreset(int id, string name, string source, string destination)
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            preset["Preset" + id].Name = name;
            preset["Preset" + id].PathSource = source;
            preset["Preset" + id].PathDestination = destination;

            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText("./data/preset/preset.json", json);
        }

        /// <summary>
        /// Method to add a new preset
        /// </summary>
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void AddPreset(string name, string source, string destination)
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            int idPreset = preset.Count + 1;
            NameSourceDest newPreset = new NameSourceDest
            {
                Name = name,
                PathSource = source,
                PathDestination = destination
            };
            preset.Add("Preset" + idPreset, newPreset);
            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText("./data/preset/preset.json", json);
        }

        /// <summary>
        /// Method to delete the choosen preset
        /// </summary>
        /// <param name="choice"></param>
        public static void DeletePreset(int choice)
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            for (int i = choice; i < preset.Count; i++)
            {
                preset["Preset" + i] = preset["Preset" + (i + 1).ToString()];
            }
            preset.Remove("Preset" + preset.Count);
            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText("./data/preset/preset.json", json);
        }

        /// <summary>
        /// Method to get the ID of the selected preset in the listbox (first few caracters)
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public static string GetId(string selectedItem)
        {
            string id = "";
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i.ToString() == selectedItem.Substring(j, 1))
                    {
                        id += i.ToString();
                    }
                }
            }
            return id;
        }
    }
}