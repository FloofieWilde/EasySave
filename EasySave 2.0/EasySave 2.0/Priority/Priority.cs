using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projet.Priority
{
    public class Priority
    {
        /// <summary>
        /// Method to get the Json file for priorities and convert it to a .NET object.
        /// </summary>
        /// <returns>extension</returns>
        public static Dictionary<string, string> GetJsonPriority()
        {
            string json = File.ReadAllText("./data/priority/priority.json");
            Dictionary<string, string> priorities = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return priorities;
        }

        /// <summary>
        /// Method to add a new priority.
        /// </summary>
        public static void AddPriority(string priority)
        {
            Dictionary<string, string> priorities = GetJsonPriority();
            int idPriority = priorities.Count + 1;
            priorities.Add("Priority" + idPriority, priority);
            string json = JsonConvert.SerializeObject(priorities, Formatting.Indented);
            File.WriteAllText(@"./data/priority/priority.json", json);
        }

        /// <summary>
        /// Method to edit the choosen priority.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="priority"></param>
        public static void EditPriority(int id, string priority)
        {
            Dictionary<string, string> priorities = GetJsonPriority();
            priorities["Priority" + id] = priority;
            string json = JsonConvert.SerializeObject(priorities, Formatting.Indented);
            File.WriteAllText("./data/priority/priority.json", json);
        }

        /// <summary>
        /// Method to delete a priority from the json file. 
        /// </summary>
        public static void DeletePriority(int choice)
        {
            Dictionary<string, string> priorities = GetJsonPriority();
            for (int i = choice; i < priorities.Count; i++)
            {
                priorities["Priority" + i] = priorities["Priority" + (i + 1).ToString()];
            }
            priorities.Remove("Priority" + priorities.Count);
            string json = JsonConvert.SerializeObject(priorities, Formatting.Indented);
            File.WriteAllText("./data/priority/priority.json", json);
        }
    }
}
