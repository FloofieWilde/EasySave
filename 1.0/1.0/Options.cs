using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Projet.Languages;
using Projet.Logs;
using Projet.Presets;
using Projet.Stockages;

namespace Projet
{
    class Option
    {
        static public void OptMenu(Langue.Language dictLang)
        {

            string choice = "0";
            while (!(choice == "1" | choice == "2" | choice == "3" | choice == "4"))
            {
                Console.WriteLine(dictLang.OptTitle);
                Console.WriteLine("");
                Console.WriteLine("1. "+dictLang.OptC1);
                Console.WriteLine("2. "+dictLang.OptC2);
                Console.WriteLine("3. Stockage");
                Console.WriteLine("4. "+dictLang.OptExit);

                Console.WriteLine("");

                choice = Console.ReadLine();
            }

            switch (choice)
            {
                case "1":
                    dictLang = Langue.GetFiles(dictLang);
                    OptMenu(dictLang);
                    break;
                case "2":
                    Preset.MenuPreset();
                    OptMenu(dictLang);
                    break;
                case "3":
                    Stockage.MenuStockage();
                    break;
                case "4":
                    Console.WriteLine("\n\n\n");
                    Menu.Main();
                    break;
            }
        }
    }


}
