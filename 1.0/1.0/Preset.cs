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
            string content = File.ReadAllText(@"\data\preset\preset.json");
            Path preset = JsonConvert.DeserializeObject<Path>(content);
        }
    }
}

        //public static void EditPreset()
        //{
        //    var path = new Path
        //    {
        //        Name = "",
        //        Source = "",
        //        Destination = ""
        //    };

        //    var options = new JsonSerializerOptions { WriteIndented = true };
        //    string jsonString = JsonSerializer.Serialize(path, options);

        //    Console.WriteLine(jsonString);
        //}

        //public static async Task EditPreset()
        //{
            //var path = new Path
            //{
            //    Name = "",
            //    Source = "",
            //    Destination = ""
            //};

            //string fileName = "WeatherForecast.json";
            //using FileStream createStream = File.Create(fileName);
            //await JsonSerializer.SerializeAsync(createStream, path);
            //await createStream.DisposeAsync();

            //Console.WriteLine(File.ReadAllText(fileName));
        //}
