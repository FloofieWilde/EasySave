using System;
using System.IO;
using Newtonsoft.Json;
using Options;

namespace Menu
{
    class Menu
    {
        static public void Main()
        {
            langue.language dict_lang = langue.get_lang();

            Title();
            string choice = GetMainChoice(dict_lang);
            RedirFromMain(choice, dict_lang);

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
        static void Title()
        {
            Console.WriteLine("  ______                 _____                                      _               __   ___  ");
            Console.WriteLine(" |  ____|               / ____|                                    (_)             /_ | / _ \\ ");
            Console.WriteLine(" | |__   __ _ ___ _   _| (___   __ ___   _____  __   _____ _ __ ___ _  ___  _ __    | || | | |");
            Console.WriteLine(" |  __| / _` / __| | | |\\___ \\ / _` \\ \\ / / _ \\ \\ \\ / / _ \\ '__/ __| |/ _ \\| '_ \\   | || | | |");
            Console.WriteLine(" | |___| (_| \\__ \\ |_| |____) | (_| |\\ V /  __/  \\ V /  __/ |  \\__ \\ | (_) | | | |  | || |_| |");
            Console.WriteLine(" |______\\__,_|___/\\__, |_____/ \\__,_| \\_/ \\___|   \\_/ \\___|_|  |___/_|\\___/|_| |_|  |_(_)___/ ");
            Console.WriteLine("                   __/ |                                                                      ");
            Console.WriteLine("                  |___/                                                                       ");
            Console.WriteLine("");
        }

        static string GetMainChoice(langue.language dict_lang)
        {
            string choice = MakeChoice(dict_lang);

            while (!(choice == "1" | choice == "2" | choice == "3" | choice == "4" | choice == "621"))
            {
                Console.WriteLine(dict_lang.menubc);
                Console.WriteLine("");
                choice = MakeChoice(dict_lang);
            }

            return choice;
        }

        static void RedirFromMain(string choice, langue.language dict_lang)
        {
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Copie");
                    break;
                case "2":
                    //Console.WriteLine("Option");
                    Opt_Menu.opt_menu(dict_lang);
                    break;
                case "3":
                    Console.WriteLine("Logs");
                    break;
                case "4":
                    //Console.WriteLine("Quitter");
                    System.Environment.Exit(621);
                    break;
                case "621":
                    Console.WriteLine("Test");
                   //playtest
                    break;
            }
        }
    }
    

}
