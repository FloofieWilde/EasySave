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
        /// Method to navigate between the different Methods
        /// </summary>
        public static void MenuPreset()
        {
            string action = MakeChoiceAction();
            switch (action)
            {
                case "1":
                    AddPreset();
                    break;
                case "2":
                    EditPreset();
                    break;
                case "3":
                    DeletePreset();
                    break;
                case "4":
                    BackToMenuOption();
                    break;
            }
            BackToMenuOption();
        }

        /// <summary>
        /// Method to get the Json file and convert it to a .NET object
        /// </summary>
        /// <returns>preset</returns>
        public static Dictionary<string, NameSourceDest> GetJsonPreset()
        {
            string json = File.ReadAllText(@"./data/preset/preset.json");
            Dictionary<string, NameSourceDest> preset = JsonConvert.DeserializeObject<Dictionary<string, NameSourceDest>>(json);
            return preset;
        }

        /// <summary>
        /// Method to choose if we want to edit, add or delete a preset.
        /// </summary>
        /// <returns>action</returns>
        public static string MakeChoiceAction()
        {
            string action = "0";
            while (!(action == "1" | action == "2" | action == "3" | action == "4"))
            {
                Console.WriteLine("Quel action voulez vous faire?");
                Console.WriteLine("1. Ajouter un preset");
                Console.WriteLine("2. Modifier un preset");
                Console.WriteLine("3. Supprimer un preset");
                Console.WriteLine("4. Retour");
                action = Console.ReadLine();
            }
            return action;
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
        public static void EditPreset()
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            string choice = MakeChoicePreset(preset);
            Langue.Language dictLang = Langue.GetLang();

            Console.WriteLine("\n" + dictLang.PreChoice + choice);
            Console.WriteLine("\n" + dictLang.PreName);
            preset["Preset" + choice].Name = Console.ReadLine();
            Console.WriteLine("\n" + dictLang.PrepathSource);
            preset["Preset" + choice].PathSource = Console.ReadLine();
            Console.WriteLine("\n" + dictLang.PrepathDest);
            preset["Preset" + choice].PathDestination = Console.ReadLine();

            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText(@"./data/preset/preset.json", json);
            Console.WriteLine("\n" + dictLang.PreSave1 + choice + dictLang.PreSave2 +
                "\n" + dictLang.PreSave3 +
                "\nName: " + $"{preset["Preset" + choice].Name}" +
                "\nPathSource: " + $"{preset["Preset" + choice].PathSource}" +
                "\nPathDestination: " + $"{preset["Preset" + choice].PathDestination}"
            );
        }

        /// <summary>
        /// Method to add a new preset
        /// </summary>
        public static void AddPreset()
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            int idPreset = preset.Count + 1;
            Langue.Language dictLang = Langue.GetLang();

            Console.WriteLine("\n" + dictLang.PreName);
            string name = Console.ReadLine();
            Console.WriteLine("\n" + dictLang.PrepathSource);
            string source = Console.ReadLine();
            Console.WriteLine("\n" + dictLang.PrepathDest);
            string destination = Console.ReadLine();

            NameSourceDest newPreset = new NameSourceDest
            {
                Name = name,
                PathSource = source,
                PathDestination = destination
            };
            preset.Add("Preset" + idPreset, newPreset);

            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText(@"./data/preset/preset.json", json);

            Console.WriteLine("Preset créé avec succès!");
        }

        /// <summary>
        /// Method to delete a preset from the json file. 
        /// </summary>
        public static void DeletePreset()
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            string choice = MakeChoicePreset(preset);
            for (int i = Convert.ToInt32(choice); i < preset.Count; i++)
            {
                preset["Preset" + i] = preset["Preset" + (i + 1).ToString()];
            }
            preset.Remove("Preset" + preset.Count);
            //preset.Remove("Preset" + (preset.Count+1).ToString());
            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText(@"./data/preset/preset.json", json);
        }

        /// <summary>
        /// Function to get back to the options menu
        /// </summary>
        public static void BackToMenuOption()
        {
            Langue.Language dictLang = Langue.GetLang();
            Console.WriteLine("\n" + dictLang.PreFbg + "\n\n\n");
            Option.OptMenu(dictLang);
        }
    }
}