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
            Config configuration = JsonConvert.DeserializeObject<Config>(jsonString);



            sr = new StreamReader("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/" + configuration.Lang + ".json");
            jsonString = sr.ReadToEnd();
            Language dictLang = JsonConvert.DeserializeObject<Language>(jsonString);

            return dictLang;
        }

        static public Language GetFiles(Language dictLang)
        {
            string Path = "C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/";
            string[] files = Directory.GetFiles(Path);
            Console.WriteLine(dictLang.LangSelect);

            foreach (string file in files)
            {
                string fileNew = file.Substring(Path.Length);
                int length = fileNew.Length;
                fileNew = fileNew.Remove(length - 5);
                Console.WriteLine("•   " + fileNew);
            }
            bool verifLan = false;

            Console.WriteLine(dictLang.LangChoice);
            Console.WriteLine("");

            string lChoice = Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine("");


            foreach (string file in files)
            {
                string fileNew = file.Substring(66);
                int length = fileNew.Length;
                fileNew = fileNew.Remove(length - 5);

                if (lChoice == fileNew)
                {
                    verifLan = true;

                    StreamReader sr = new StreamReader("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
                    string jsonString = sr.ReadToEnd();
                    Config conf = JsonConvert.DeserializeObject<Config>(jsonString);
                    sr.Dispose();

                    string text = File.ReadAllText("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
                    text = text.Replace(conf.Lang, lChoice);

                    File.WriteAllText("C:/Users/" + Environment.UserName + "/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json", text);
                }
            }
            if (verifLan == false)
            {
                Console.WriteLine(dictLang.LangNo);
                return dictLang;
            } else
            {
                Language newDictLang = GetLang();
                Console.WriteLine(newDictLang.LangChange);
                return newDictLang;
            }

        }


        public class Language
        {
            public string MenuTitle { get; set; }
            public string MenuC1 { get; set; }
            public string MenuC2 { get; set; }
            public string MenuC3 { get; set; }
            public string MenuC4 { get; set; }
            public string MenuBc { get; set; }
            public string OptTitle { get; set; }
            public string OptC1 { get; set; }
            public string OptC2 { get; set; }
            public string OptExit { get; set; }
            public string LangSelect { get; set; }
            public string LangChange { get; set; }
            public string LangNo { get; set; }
            public string LangChoice { get; set; }
            public string PreList { get; set; }
            public string PreMod { get; set; }
            public string PreChoice { get; set; }
            public string PreName { get; set; }
            public string PrepathSource { get; set; }
            public string PrepathDest { get; set; }
            public string PreSave1 { get; set; }
            public string PreSave2 { get; set; }
            public string PreSave3 { get; set; }
            public string PreFbg { get; set; }
        }
    }
}
