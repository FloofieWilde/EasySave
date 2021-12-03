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
        /// Method to navigate between the differents methods
        /// </summary>
        public static void MenuStockage()
        {
            string choice = MakeChoiceStockage();
            EditStockage(choice);
            BackToMenuOption();
        }

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
        /// Method to choose between .json and .xml
        /// </summary>
        /// <returns>choice</returns>
        public static string MakeChoiceStockage()
        {
            JsonXml stockage = GetJsonStockage();
            Console.WriteLine($"Vous utilisez actuellement{stockage.TypeStockage}");
            string choice = "0";
            while (!(choice == "1" | choice == "2" || choice == "3"))
            {
                Console.WriteLine("Choisissez un type de stockage:");
                Console.WriteLine("1 - .json");
                Console.WriteLine("2 - .xml");
                choice = Console.ReadLine();
            }
            return choice;
        }

        /// <summary>
        /// Method to choose a new stockage type for logs.
        /// </summary>
        /// <param name="choice"></param>
        public static void EditStockage(string choice)
        {
            JsonXml stockage = GetJsonStockage();

            if (choice == "1")
            {
                stockage.TypeStockage = ".json";
            }
            else if (choice == "2")
            {
                stockage.TypeStockage = ".xml";
            }
            string json = JsonConvert.SerializeObject(stockage, Formatting.Indented);
            File.WriteAllText(@"./data/stockage/stockage.json", json);
            Console.WriteLine($"Modification en {stockage.TypeStockage} avec succès!");
        }

        /// <summary>
        /// Method to get back to the options menu
        /// </summary>
        public static void BackToMenuOption()
        {
            Langue.Language dictLang = Langue.GetLang();
            Console.WriteLine("\n" + dictLang.PreFbg + "\n\n\n");
            Option.OptMenu(dictLang);
        }
    }
}
