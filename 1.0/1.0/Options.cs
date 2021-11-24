using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Menu;

namespace Options
{
    class Opt_Menu
    {
        static public void opt_menu(langue.language dict_lang)
        {
            Console.WriteLine(dict_lang.opttitle);
            Console.WriteLine("");
            Console.WriteLine(dict_lang.optc1);
            Console.WriteLine(dict_lang.optc2);
            Console.WriteLine(dict_lang.optexit);

            Console.WriteLine("");
            //string optchoice = Console.ReadLine();

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    //Console.WriteLine("Langue");
                    dict_lang = langue.get_files(dict_lang);
                    opt_menu(dict_lang);
                    break;
                case "2":
                    //Console.WriteLine("Preset");
                    Preset.MenuPreset();
                    opt_menu(dict_lang);
                    break;
                case "3":
                    //Console.WriteLine("Retour");
                    Console.WriteLine("\n\n\n");
                    Menu.Menu.Main();
                    break;
            }
        }
    }


}
