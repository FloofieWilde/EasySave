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
                    Console.WriteLine("Langue");
                    break;
                case "2":
                    Console.WriteLine("Preset");
                    Preset.MenuPreset();
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
            StreamReader sr = new StreamReader("C:/Users/"+Environment.UserName+"/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
            string jsonString = sr.ReadToEnd();
            sr.Dispose();
            config configuration = JsonConvert.DeserializeObject<config>(jsonString);

            

            sr = new StreamReader("C:/Users/"+Environment.UserName+"/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/" + configuration.lang + ".json");
            jsonString = sr.ReadToEnd();
            language dict_lang = JsonConvert.DeserializeObject<language>(jsonString);

            return dict_lang;
        }

        static public void /*language*/ get_files(language dict_lang)
        {
            string Path = "C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/";
            string[] files = Directory.GetFiles(Path);
            Console.WriteLine(dict_lang.langselect);

            foreach (string file in files)
            {
                string file_new = file.Substring(Path.Length);
                int length = file_new.Length;
                file_new = file_new.Remove(length-5);
                Console.WriteLine("•   " + file_new);
            }
            bool verif_lan = false;

            Console.WriteLine(dict_lang.langchoice);
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

                    StreamReader sr = new StreamReader("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
                    string jsonString = sr.ReadToEnd();
                    config conf = JsonConvert.DeserializeObject<config>(jsonString);
                    sr.Dispose();


                    string text = File.ReadAllText("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
                    text = text.Replace(conf.lang, lchoice);

                    File.WriteAllText("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json", text);


                    language new_dict_lang = get_lang();

                    Console.WriteLine(new_dict_lang.langchange);

                    //return new_dict_lang;

                }
            }
            if (verif_lan == false) Console.WriteLine(dict_lang.langno);

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
            public string langselect { get; set; }
            public string langchange { get; set; }
            public string langno { get; set; }
            public string langchoice { get; set; }
        }
    }
}
