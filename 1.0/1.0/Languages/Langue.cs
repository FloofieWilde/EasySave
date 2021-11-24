using System;
using System.IO;
using Newtonsoft.Json;

namespace Projet.Languages
{
    class Langue
    {
        public static Language GetLang()
        {
            StreamReader sr = new StreamReader("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
            string jsonString = sr.ReadToEnd();
            sr.Dispose();
            config configuration = JsonConvert.DeserializeObject<config>(jsonString);



            sr = new StreamReader("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/" + configuration.lang + ".json");
            jsonString = sr.ReadToEnd();
            Language dictLang = JsonConvert.DeserializeObject<Language>(jsonString);

            return dictLang;
        }

        static public Language GetFiles(Language dictLang)
        {
            string Path = "C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/";
            string[] files = Directory.GetFiles(Path);
            Console.WriteLine(dictLang.langselect);

            foreach (string file in files)
            {
                string file_new = file.Substring(Path.Length);
                int length = file_new.Length;
                file_new = file_new.Remove(length - 5);
                Console.WriteLine("•   " + file_new);
            }
            bool verif_lan = false;

            Console.WriteLine(dictLang.langchoice);
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


                    

                }
            }
            if (verif_lan == false)
            {
                Console.WriteLine(dictLang.langno);
                return dictLang;
            } else
            {
                Language newDictLang = GetLang();
                Console.WriteLine(newDictLang.langchange);
                return newDictLang;
            }

        }


        public class Language
        {
            public string MenuTitle { get; set; }
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
            public string prelist { get; set; }
            public string premod { get; set; }
            public string prechoice { get; set; }
            public string prename { get; set; }
            public string prepathsource { get; set; }
            public string prepathdest { get; set; }
            public string presave1 { get; set; }
            public string presave2 { get; set; }
            public string presave3 { get; set; }
            public string prefgb { get; set; }
        }
    }
}
