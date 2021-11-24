using System;
using System.IO;
using Newtonsoft.Json;
using Projet.Logs;
using Projet.SaveSystem;
using Projet.Languages;
using Projet.Presets;

namespace Projet
{
    class Menu
    {
        static public void Main()
        {
            Langue.Language dictLang = Langue.GetLang();

            Title();
            string choice = GetMainChoice(dictLang);
            RedirFromMain(choice, dictLang);
        }

        static string MakeChoice(Langue.Language dictLang)
        {
            Console.WriteLine(dictLang.MenuTitle);
            Console.WriteLine("");
            Console.WriteLine(dictLang.MenuC1);
            Console.WriteLine(dictLang.MenuC2);
            Console.WriteLine(dictLang.MenuC3);
            Console.WriteLine(dictLang.MenuC4);

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

        static string GetMainChoice(Langue.Language dictLang)
        {
            string choice = MakeChoice(dictLang);

            while (!(choice == "1" | choice == "2" | choice == "3" | choice == "4" | choice == "621"))
            {
                Console.WriteLine(dictLang.MenuBc);
                Console.WriteLine("");
                choice = MakeChoice(dictLang);
            }

            return choice;
        }

        static void RedirFromMain(string choice, Langue.Language dictLang)
        {
            switch (choice)
            {
                case "1":
                    string presetId = GetPresetChoice();
                    var presets = Preset.GetJsonPreset();

                    string choosenSourcePath = presets["Preset" + presetId].PathSource;
                    string choosenTargetPath = presets["Preset" + presetId].PathDestination;
                    string copyChoice = GetFullCopy();
                    bool full = false;
                    if (copyChoice == "1") full = true;
                    Save save = new Save(choosenSourcePath, choosenTargetPath, full);
                    save.Copy();
                    break;
                case "2":
                    //Console.WriteLine("Option");
                    Option.OptMenu(dictLang);
                    break;
                case "3":
                    LogDaily oui = new LogDaily("Full");
                    oui.Load();
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


        private static string PresetChoice()
        {
            var presets = Preset.GetJsonPreset();
            if (presets == null) Console.WriteLine("You don't have presets saved");
            
            var choosenPreset = Preset.MakeChoicePreset(presets);
            Console.WriteLine(choosenPreset);
            
            return choosenPreset;
        }
        private static string GetPresetChoice()
        {
            string choice = PresetChoice();

            while (!(choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5"))
            {
                Console.WriteLine("Please select an existing preset");
                choice = PresetChoice();
            }

            return choice;
        }

        private static string FullCopy()
        {
            Console.WriteLine("Please choose between theses save modes : ");
            Console.WriteLine("1. Full");
            Console.WriteLine("2. Differential");
            string choosenCopy = Console.ReadLine();

            return choosenCopy;
        }

        private static string GetFullCopy()
        {
            string choice = FullCopy();

            while (!(choice == "1" || choice == "2"))
            {
                Console.WriteLine("Please choose between theses save modes only");
                choice = FullCopy();
            }

            return choice;
        }

    }


}
