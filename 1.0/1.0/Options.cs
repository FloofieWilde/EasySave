using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Menu;

namespace Options
{
    class Menu
    {
        
    }

    class langue
    {
        public static language get_lang()
        {
            StreamReader sr = new StreamReader("C:/Users/jennm/source/repos/C-hashtag-point-web/1.0/1.0/data/config.json");
            string jsonString = sr.ReadToEnd();
            config configuration = JsonConvert.DeserializeObject<config>(jsonString);

            //Console.WriteLine(configuration.lang);

            sr = new StreamReader("C:/Users/jennm/source/repos/C-hashtag-point-web/1.0/1.0/data/lang/" + configuration.lang + ".json");
            jsonString = sr.ReadToEnd();
            language dict_lang = JsonConvert.DeserializeObject<language>(jsonString);

            return dict_lang;
        }

        public class language
        {
            public string menutitle { get; set; }
            public string menuc1 { get; set; }
            public string menuc2 { get; set; }
            public string menuc3 { get; set; }
            public string menuc4 { get; set; }
            public string menubc { get; set; }
        }
    }
}
