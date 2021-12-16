using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.Size
{
    public class Size
    {
        /// <summary>
        /// Returns the minimum size to pause all the others processes
        /// </summary>
        /// <returns></returns>
        public static JsonSize GetJsonSize()
        {
            string json = File.ReadAllText(@"./data/size/size.json");
            JsonSize size = JsonConvert.DeserializeObject<JsonSize>(json);
            return size;
        }
        /// <summary>
        /// Edits the minimum size
        /// </summary>
        /// <param name="size"></param>
        public static void EditSize(int size)
        {
            JsonSize sizes = GetJsonSize();
            sizes.Size = size;
            string json = JsonConvert.SerializeObject(sizes, Formatting.Indented);
            File.WriteAllText(@"./data/size/size.json", json);
        }
    }
}
