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
            string optchoice = Console.ReadLine();

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Langue");
                    break;
                case "2":
                    Console.WriteLine("Preset");
                    Preset.EditPreset();
                    break;
                case "3":
                    Console.WriteLine("Quitter");
                    break;
            }
        }
    }

    class langue
    {
        public static language get_lang()
        {
            StreamReader sr = new StreamReader("C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
            string jsonString = sr.ReadToEnd();
            config configuration = JsonConvert.DeserializeObject<config>(jsonString);

            //Console.WriteLine(configuration.lang);

            sr = new StreamReader("C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/" + configuration.lang + ".json");
            jsonString = sr.ReadToEnd();
            language dict_lang = JsonConvert.DeserializeObject<language>(jsonString);

            return dict_lang;
        }

        static public void get_files()
        {
            string[] files = Directory.GetFiles("C:/Users/loicm/source/repos/C-hashtag-point-web/1.0/1.0/data/lang");
            Console.WriteLine("Langues disponibles :");
            foreach (string file in files)
            {
                string file_new = file.Substring(66);
                int length = file_new.Length;
                file_new = file_new.Remove(length-5);
                Console.WriteLine("• " + file_new);
            }
            bool verif_lan = false;

            Console.WriteLine("Faites votre choix");
            Console.WriteLine("");

            string lchoice = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("");

            verif_lan = false;

            foreach (string file in files)
            {
                string file_new = file.Substring(66);
                int length = file_new.Length;
                file_new = file_new.Remove(length - 5);
                if (lchoice == file_new)
                {
                    verif_lan = true;

                    //TODO: Ajouter édit de 

                    language dict_lang = get_lang();
                }
            }

        }


        public class language
        {
            public string menutitle { get; set; }
            public string menuc1 { get; set; }
            public string menuc2 { get; set; }
            public string menuc3 { get; set; }
            public string menuc4 { get; set; }
            public string menubc { get; set; }
            public string opttitle { get; set; }
            public string optc1 { get; set; }
            public string optc2 { get; set; }
            public string optexit { get; set; }

        }
    }
}
