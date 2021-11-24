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
        /// Function to navigate between the different function
        /// </summary>
        public static void MenuPreset()
        {
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();
            string choice = MakeChoicePreset(preset);
            EditPreset(choice, preset);
            BackToMenuOption();
        }

        /// <summary>
        /// Function to get the Json file and convert it to a .NET object
        /// </summary>
        /// <returns>preset</returns>
        public static Dictionary<string, NameSourceDest> GetJsonPreset()
        {
            string json = File.ReadAllText(@"C:/Users/"+ Environment.UserName +"/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json");
            Dictionary<string, NameSourceDest> preset = JsonConvert.DeserializeObject<Dictionary<string, NameSourceDest>>(json);
            return preset;
        }

        /// <summary>
        /// Function to choose which preset we want to edit
        /// </summary>
        /// <param name="preset"></param>
        /// <returns>choice</returns>
        public static string MakeChoicePreset(Dictionary<string, NameSourceDest> preset)
        {
            Langue.Language dictLang = Langue.GetLang();

            string choice = "0";
            while (!(choice == "1" | choice == "2" | choice == "3" | choice == "4" | choice == "5"))
            {
                Console.WriteLine("\n" + dictLang.PreList);
                Console.WriteLine(dictLang.PreMod);
                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine(i.ToString() + $". {preset["Preset" + i.ToString()].Name}");
                }
                choice = Console.ReadLine();
            }

            return choice;
        }

        /// <summary>
        /// Function to edit the choosen preset
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="preset"></param>
        public static void EditPreset(string choice, Dictionary<string, NameSourceDest> preset)
        {
            Langue.Language dictLang = Langue.GetLang();

            Console.WriteLine("\n"+ dictLang.PreChoice + choice);
            Console.WriteLine("\n"+dictLang.PreName);
            preset["Preset" + choice].Name = Console.ReadLine();
            Console.WriteLine("\n"+dictLang.PrepathSource);
            preset["Preset" + choice].PathSource = Console.ReadLine();
            Console.WriteLine("\n"+dictLang.PrepathDest);
            preset["Preset" + choice].PathDestination = Console.ReadLine();

            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText(@"C:/Users/"+ Environment.UserName +"/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json", json);
            Console.WriteLine("\n"+dictLang.PreSave1+choice+dictLang.PreSave2 +
                "\n"+ dictLang.PreSave3 +
                "\nName: "+ $"{preset["Preset" + choice].Name}"+
                "\nPathSource: "+ $"{preset["Preset" + choice].PathSource}"+
                "\nPathDestination: "+ $"{preset["Preset" + choice].PathDestination}"
            );
        }

        /// <summary>
        /// Function to get back to the options menu
        /// </summary>
        public static void BackToMenuOption()
        {
            Langue.Language dictLang = Langue.GetLang();
            Console.WriteLine("\n" + dictLang.PreFbg + "\n");
            Option.OptMenu(dictLang);
        }
    }
}