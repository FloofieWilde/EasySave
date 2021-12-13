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
        /// Method to choose which preset we want to edit
        /// </summary>
        /// <param name="preset"></param>
        /// <returns>choice</returns>
        public static string MakeChoicePreset(Dictionary<string, NameSourceDest> preset)
        {
            Langue.Language dictLang = Langue.GetLang();
            int nbPreset = preset.Count;
            Console.WriteLine("Nombre de travail de sauvegarde: " + nbPreset);
            string choice = "0";
            List<string> listOfChoice = new List<string>();
            for (int i = 1; i <= nbPreset; i++)
            {
                listOfChoice.Add(i.ToString());
            }
            while (!listOfChoice.Contains(choice))
            {
                Console.WriteLine("\n" + dictLang.PreList);
                Console.WriteLine(dictLang.PreMod);
                for (int i = 1; i <= nbPreset; i++)
                {
                    Console.WriteLine(i.ToString() + $". {preset["Preset" + i.ToString()].Name}");
                }
                choice = Console.ReadLine();
            }
            return choice;
        }

        /// <summary>
        /// Method to edit the choosen preset
        /// </summary>
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
        /// Method to delete a preset from the json file. 
        /// </summary>
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