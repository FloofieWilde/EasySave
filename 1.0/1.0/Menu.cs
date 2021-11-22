using System;
using System.IO;
using Newtonsoft.Json;
using Options;

namespace Menu
{
    class Menu
    {
        static void Main(string[] args)
        {
            langue.language dict_lang = langue.get_lang();

            Console.WriteLine("  ______                 _____                                      _               __   ___  ");
            Console.WriteLine(" |  ____|               / ____|                                    (_)             /_ | / _ \\ ");
            Console.WriteLine(" | |__   __ _ ___ _   _| (___   __ ___   _____  __   _____ _ __ ___ _  ___  _ __    | || | | |");
            Console.WriteLine(" |  __| / _` / __| | | |\\___ \\ / _` \\ \\ / / _ \\ \\ \\ / / _ \\ '__/ __| |/ _ \\| '_ \\   | || | | |");
            Console.WriteLine(" | |___| (_| \\__ \\ |_| |____) | (_| |\\ V /  __/  \\ V /  __/ |  \\__ \\ | (_) | | | |  | || |_| |");
            Console.WriteLine(" |______\\__,_|___/\\__, |_____/ \\__,_| \\_/ \\___|   \\_/ \\___|_|  |___/_|\\___/|_| |_|  |_(_)___/ ");
            Console.WriteLine("                   __/ |                                                                      ");
            Console.WriteLine("                  |___/                                                                       ");
            Console.WriteLine("");

            string choice = MakeChoice(dict_lang);

            while(!(choice == "1" | choice == "2" | choice == "3" | choice == "4"))
            {
                Console.WriteLine(dict_lang.menubc);
                Console.WriteLine("");
                choice = MakeChoice(dict_lang);
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Copie");
                    break;
                case "2":
                    Console.WriteLine("Option");
                    break;
                case "3":
                    Console.WriteLine("Logs");
                    break;
                case "4":
                    Console.WriteLine("Quitter");
                    System.Environment.Exit(1);
                    break;
            }

        }

        static string MakeChoice(langue.language dict_lang)
        {
            Console.WriteLine(dict_lang.menutitle);
            Console.WriteLine("");
            Console.WriteLine(dict_lang.menuc1);
            Console.WriteLine(dict_lang.menuc2);
            Console.WriteLine(dict_lang.menuc3);
            Console.WriteLine(dict_lang.menuc4);

            Console.WriteLine("");
            string choice = Console.ReadLine();
            Console.WriteLine("");

            return choice;
        }
    }
    class config
    {
        public string lang { get; set; }
    }

    class language
    {
        public string menutitle { get; set; }
        public string menuc1 { get; set; }
        public string menuc2 { get; set; }
        public string menuc3 { get; set; }
        public string menuc4 { get; set; }
        public string menubc { get; set; }
    }
}
