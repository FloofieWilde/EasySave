using System;
using System.Collections.Generic;
using System.Text;

namespace Projet.Calcules
{
    public class Calcule
    {
        /// <summary>
        /// Return the ID of the selected preset.
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public static string GetId(string selectedItem)
        {
            string id = "";
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i.ToString() == selectedItem.Substring(j, 1))
                    {
                        id += i.ToString();
                    }
                }
            }
            return id;
        }
    }
}
