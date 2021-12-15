using System;
using System.Collections.Generic;
using System.Text;

namespace Projet.Calcules
{
    public class Calcule
    {
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
