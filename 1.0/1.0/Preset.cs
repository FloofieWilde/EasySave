using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Options
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
            string json = File.ReadAllText(@"C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json");
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
            Console.WriteLine("\nVoici la liste des presets:\n");
            for (int i=1; i<=5; i++)
            {
                Console.WriteLine(i.ToString()+$". {preset["Preset"+i.ToString()].Name}");
            }
            Console.WriteLine("\nQuel preset voulez vous modifier?\n");
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
            Console.WriteLine("\nVous avez choisit de modifier le Preset" + choice);
            Console.WriteLine("\nChoisissez un nouveau nom: ");
            preset["Preset" + choice].Name = Console.ReadLine();
            Console.WriteLine("\nChoisissez un nouveau path source: ");
            preset["Preset" + choice].PathSource = Console.ReadLine();
            Console.WriteLine("\nChoisissez un nouveau path destination: ");
            preset["Preset" + choice].PathDestination = Console.ReadLine();

            string json = JsonConvert.SerializeObject(preset, Formatting.Indented);
            File.WriteAllText(@"C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json", json);
            Console.WriteLine("\nEnregistrement du nouveau preset n°"+choice+" avec succès! " +
                "\nVoici les nouvelles valeur: " +
                "\nName: "+ $". {preset["Preset" + choice].Name}"+
                "\nName: "+ $". {preset["Preset" + choice].PathSource}"+
                "\nName: "+ $". {preset["Preset" + choice].PathDestination}"
            );
        }

        /// <summary>
        /// Function to get back to the options menu
        /// </summary>
        public static void BackToMenuOption()
        {
            Console.WriteLine("\nRetour au menu Options...\n");
            langue.language dict_lang = langue.get_lang();
            Opt_Menu.opt_menu(dict_lang);
        }
    }
}