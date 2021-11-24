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
            langue.language dict_lang = langue.get_lang();

            Console.WriteLine("\n"+dict_lang.prelist+"\n");
            for (int i=1; i<=5; i++)
            {
                Console.WriteLine(i.ToString()+$". {preset["Preset"+i.ToString()].Name}");
            }
            Console.WriteLine("\n"+dict_lang.premod+"\n");
            string choice = Console.ReadLine();

            return choice;
        }

        /// <summary>
        /// Function to edit the choosen preset
        /// </summary>
        /// <param name="choice"></param>
        /// <param name="preset"></param>
        public static void EditPreset(string choice, Dictionary<string, NameSourceDest> preset)
        {
            langue.language dict_lang = langue.get_lang();

            Console.WriteLine("\n"+ dict_lang.prechoice + choice);
            Console.WriteLine("\n"+dict_lang.prename);
            preset["Preset" + choice].Name = Console.ReadLine();
            Console.WriteLine("\n"+dict_lang.prepathsource);
            preset["Preset" + choice].PathSource = Console.ReadLine();
            Console.WriteLine("\n"+dict_lang.prepathdest);
            preset["Preset" + choice].PathDestination = Console.ReadLine();

            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText(@"C:/Users/"+ Environment.UserName +"/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json", json);
            Console.WriteLine("\n"+dict_lang.presave1+choice+dict_lang.presave2 +
                "\n"+ dict_lang.presave3 +
                "\nName: "+ $". {preset["Preset" + choice].Name}"+
                "\nPathSource: "+ $". {preset["Preset" + choice].PathSource}"+
                "\nPathDestination: "+ $". {preset["Preset" + choice].PathDestination}"
            );
        }

        /// <summary>
        /// Function to get back to the options menu
        /// </summary>
        public static void BackToMenuOption()
        {
            langue.language dict_lang = langue.get_lang();
            Console.WriteLine("\n" + dict_lang.prefgb + "\n");
            Opt_Menu.opt_menu(dict_lang);
        }
    }
}