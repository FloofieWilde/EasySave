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

            Console.WriteLine($"Preset1: {preset["Preset1"].Name}");
            Console.WriteLine($"Preset2: {preset["Preset2"].Name}");
            Console.WriteLine($"Preset3: {preset["Preset3"].Name}");
            Console.WriteLine($"Preset4: {preset["Preset4"].Name}");
            Console.WriteLine($"Preset5: {preset["Preset5"].Name}");
        }
    }
}