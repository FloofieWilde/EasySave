using System;
using Options;
using Projet.Logs;
using Projet.SaveSystem;

namespace Projet
{
    class Menu
    {
        static void Main(string[] args)
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
            string choice = MakeChoice();

            
            while(!(choice == "1" | choice == "2" | choice == "3" | choice == "4"))
            {
                Console.WriteLine("N'est pas un choix valide.");
                Console.WriteLine("");
                choice = MakeChoice();
                
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Copie");
                    string a = @"C:\Users\franc\source\repos\C-hashtag-point-web\1.0\1.0\DossA";
                    string b = @"C:\Users\franc\source\repos\C-hashtag-point-web\1.0\1.0\DossB";
                    Save save = new Save(a, b, true);
                    save.Copy();
                    break;
                case "2":
                    Console.WriteLine("Option");
                    break;
                case "3":
                    Console.WriteLine("Logs");
                    break;
                case "4":
                    Console.WriteLine("Quitter");
                    break;
            }

        }

        static string MakeChoice()
        {
            Console.WriteLine("Veuillez sélectrionner un choix.");
            Console.WriteLine("");
            Console.WriteLine("1. Copier vos fichiers");
            Console.WriteLine("2. Configurer l'application");
            Console.WriteLine("3. Voir les logs");
            Console.WriteLine("4. Quitter l'application");

            Console.WriteLine("");
            string choice = Console.ReadLine();
            Console.WriteLine("");

            return choice;
        }


    }
}
