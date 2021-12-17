using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Projet.Languages;

namespace Projet.Stockages
{
    public class Stockage
    {
        /// <summary>
        /// Method to get the type of stockage from a .json file.
        /// </summary>
        /// <returns>stockage</returns>
        public static JsonXml GetJsonStockage()
        {
            string json = File.ReadAllText(@"./data/stockage/stockage.json");
            JsonXml stockage = JsonConvert.DeserializeObject<JsonXml>(json);
            return stockage;
        }

        /// <summary>
        /// Method to choose a new stockage type for logs.
        /// </summary>
        /// <param name="choice"></param>
        public static void EditStockage(string choice)
        {
            JsonXml stockage = GetJsonStockage();
            if (choice == ".json")
            {
                stockage.TypeStockage = ".json";
            }
            else if (choice == ".xml")
            {
                stockage.TypeStockage = ".xml";
            }
            string json = JsonConvert.SerializeObject(stockage, Formatting.Indented);
            File.WriteAllText(@"./data/stockage/stockage.json", json);
        }
    }
}