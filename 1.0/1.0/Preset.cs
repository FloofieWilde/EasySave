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
        public static void EditPreset()
        {
            // read file into a string and deserialize JSON to a type
            string json = File.ReadAllText(@"C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json");
            Dictionary<string, NameSourceDest> preset = JsonConvert.DeserializeObject<Dictionary<string, NameSourceDest>>(json);

            Console.WriteLine("\nVoici la liste des presets:\n");

            Console.WriteLine($"1. {preset["Preset1"].Name}");
            Console.WriteLine($"2. {preset["Preset2"].Name}");
            Console.WriteLine($"3. {preset["Preset3"].Name}");
            Console.WriteLine($"4. {preset["Preset4"].Name}");
            Console.WriteLine($"5. {preset["Preset5"].Name}");

            Console.WriteLine("\nQuel preset voulez vous modifier?\n");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nVous avez choisit le Preset1\n");
                    break;
                case "2":
                    Console.WriteLine("\nVous avez choisit le Preset2\n");
                    Preset.EditPreset();
                    break;
                case "3":
                    Console.WriteLine("\nVous avez choisit le Preset3\n");
                    break;
                case "4":
                    Console.WriteLine("\nVous avez choisit le Preset4\n");
                    break;
                case "5":
                    Console.WriteLine("\nVous avez choisit le Preset5\n");
                    break;
            }
        }
    }
}