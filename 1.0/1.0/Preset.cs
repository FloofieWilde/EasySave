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
            for (int i=1; i<=5; i++)
            {
                Console.WriteLine(i.ToString()+$". {preset["Preset"+i.ToString()].Name}");
            }
            Console.WriteLine("\nQuel preset voulez vous modifier?\n");
            string choice = Console.ReadLine();
            EditPreset(choice, preset);
        }

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
            //Console.WriteLine(json);
            File.WriteAllText(@"C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/preset/preset.json", json);
            Console.WriteLine("$\nEnregistrement du nouveau preset n°"+choice+" avec succès! " +
                "\nVoici les nouvelles valeur: " +
                "\nName: "+ $". {preset["Preset" + choice].Name}"+
                "\nName: "+ $". {preset["Preset" + choice].PathSource}"+
                "\nName: "+ $". {preset["Preset" + choice].PathDestination}"
            );
        }
    }
}