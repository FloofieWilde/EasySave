using System.Diagnostics;
using System.IO;
using Projet.Extensions;

namespace Projet.SaveSystem
{
    class Crypt
    {
        /// <summary>
        /// Determines if the file should be crypted and copied or just copied
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static long CryptOrSave(FileInfo source, DirectoryInfo target)
        {
            bool crypt = false;
            var oui = Extension.GetJsonExtension();
            foreach (var f in oui)
            {
                if (source.Extension == f.Value) crypt = true;
            }
            if (crypt)
            {
                long time = CryptDecrypt(source, target);
                return time;
            }
            else
            {
                source.CopyTo(Path.Combine(target.FullName, source.Name), true);
                return 0;
            }

        }
        /// <summary>
        /// Starts the files's encryption
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static long CryptDecrypt(FileInfo source, DirectoryInfo target)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string contenu = File.ReadAllText(source.FullName);
            string keyPath = @"./data/save/key.txt";
            string key = File.ReadAllText(keyPath);
            string text = Xor(contenu, key);
            string finalPath = target.FullName + @"\" + source.Name;
            File.WriteAllText(finalPath, text);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// Encrypt the file
        /// </summary>
        /// <param name="contenu"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Xor(string contenu, string key)
        {
            string sortie = "";
            int i = 0;
            foreach (char character in contenu)
            {
                sortie += (char)(character ^ key[i % key.Length]);
                i++;
            }
            return sortie;
        }
    }
}