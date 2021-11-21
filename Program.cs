using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace Projet
{

    public class Program
    {

        //Main class which has the role of ViewModel
        public static void Main(string[] args)
        {
            Console.WriteLine("Please type your command \nreminder: [Command name] [Source directory] [Target directory] [Type]\n All fields are required");
            string argument = Console.ReadLine();
            args = argument.Split(' ');
            string result = ArgumentsCheck(args);
            Console.WriteLine(result);
            if (result == "Validated")
            {
                bool type = false;
                if (args[3] == "full") type = true;
                Console.WriteLine("Bravo le vo " + type);
                //new Save(args[1], args[2], type);
            }
        }

        static string ArgumentsCheck(string[] args)
        {
            // all
            if (args.Length != 4) return ("Not enought arguments, the command: [Command name] [Source directory] [Target directory] [Type]");
            //args 0
            if (args[0] != "save") return("Command not recognized, type 'save' to use this program");

            // args 1
            DirectoryInfo sourceDir = new(@args[1]);
            if (!sourceDir.Exists)
            {
                return "Source directory does not exist or could not be found";
            }
            // args 2
            DirectoryInfo targetDir = new(@args[2]);
            if (!targetDir.Exists)
            {
                return "Target directory does not exist or could not be found";
            }
            // args 3
            if (args[3] == "full" || args[3] == "partial")
            {
                return "Validated";

            }
            else
            {
                return "Please indicate a valid save type (full or partial)";

            }

        }


        /*public static void LogSave()
        {
            var FileName = "Test.json";
            var LogTest = new LogDaily("Nom", "Source", "Cible", 183, 981, "OUI");
            Console.WriteLine(LogTest.Name);
            var stream = File.Create(FileName);
            JsonSerializer.Serialize<LogDaily>(stream, LogTest);
            stream.DisposeAsync();
        }*/
    }
}
