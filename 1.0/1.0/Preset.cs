using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;

namespace Options
{
    public class Preset
    {
        public static void MenuPreset()
        {
            //Call a function to get the file
            Dictionary<string, NameSourceDest> preset = GetJsonPreset();

            //Call a function to choose which preset we want to edit
            MakeChoicePreset(preset);
        }

        public static Dictionary<string, NameSourceDest> GetJsonPreset()
        {
            // read file into a string and deserialize JSON to a type
            string json = File.ReadAllText(@"C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json");
            Dictionary<string, NameSourceDest> preset = JsonConvert.DeserializeObject<Dictionary<string, NameSourceDest>>(json);
            return preset;
        }

        public static void MakeChoicePreset(Dictionary<string, NameSourceDest> preset)
        {
            Console.WriteLine("\nVoici la liste des presets:\n");
            Console.WriteLine($"1. {preset["Preset1"].Name}");
            Console.WriteLine($"2. {preset["Preset2"].Name}");
            Console.WriteLine($"3. {preset["Preset3"].Name}");
            Console.WriteLine($"4. {preset["Preset4"].Name}");
            Console.WriteLine($"5. {preset["Preset5"].Name}");
            Console.WriteLine("\nQuel preset voulez vous modifier?\n");

            string choice = Console.ReadLine();
            EditPreset(choice);
        }

        public static void EditPreset(string choice)
        {
            Console.WriteLine("\nVous avez choisit de modifier le Preset" + choice + "\n");
        }
    }
}