using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.Extensions
{
    class Extension
    {
        /// <summary>
        /// Method to get the Json file for extensions and convert it to a .NET object.
        /// </summary>
        /// <returns>extension</returns>
        public static Dictionary<string, string> GetJsonExtension()
        {
            string json = File.ReadAllText("./data/extension/extension.json");
            Dictionary<string, string> extension = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return extension;
        }

        /// <summary>
        /// Method to add a new extension.
        /// </summary>
        public static void AddExtension(string extension)
        {
            Dictionary<string, string> extensions = GetJsonExtension();
            int idExtension = extensions.Count + 1;
            extensions.Add("Extension" + idExtension, extension);
            string json = JsonConvert.SerializeObject(extensions, Formatting.Indented);
            File.WriteAllText(@"./data/extension/extension.json", json);
        }

        /// <summary>
        /// Method to edit the choosen extension.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extension"></param>
        public static void EditExtension(int id, string extension)
        {
            Dictionary<string, string> extensions = GetJsonExtension();
            extensions["Extension" + id] = extension;
            string json = JsonConvert.SerializeObject(extensions, Formatting.Indented);
            File.WriteAllText("./data/extension/extension.json", json);
        }

        /// <summary>
        /// Method to delete an extension from the json file. 
        /// </summary>
        public static void DeleteExtension(int choice)
        {
            Dictionary<string, string> extensions = GetJsonExtension();
            //string choice = MakeChoicePreset(preset);
            //Langue.Language dictLang = Langue.GetLang();
            for (int i = choice; i < extensions.Count; i++)
            {
                extensions["Extension" + i] = extensions["Extension" + (i + 1).ToString()];
            }
            extensions.Remove("Extension" + extensions.Count);
            string json = JsonConvert.SerializeObject(extensions, Formatting.Indented);
            File.WriteAllText("./data/extension/extension.json", json);
        }
    }
}
