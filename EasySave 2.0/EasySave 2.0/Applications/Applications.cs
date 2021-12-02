using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.Applications
{
    class Applications
    {
        /// <summary>
        /// Method to get the Json file for application and convert it to a .NET object.
        /// </summary>
        /// <returns>extension</returns>
        public static Appli GetJsonApplication()
        {
            string json = File.ReadAllText("./data/application/application.json");
            Appli application = JsonConvert.DeserializeObject<Appli>(json);
            return application;
        }

        /// <summary>
        /// Method to edit the application.
        /// </summary>
        public static void EditApplication(string newApplication)
        {
            Appli applications = GetJsonApplication();
            applications.Application = newApplication;
            string json = JsonConvert.SerializeObject(applications, Formatting.Indented);
            File.WriteAllText("./data/application/application.json", json);
        }
    }
}