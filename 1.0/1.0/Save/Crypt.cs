using System.IO;
using System.Diagnostics;

namespace Projet.SaveSystem
{
    class Crypt
    {
        public static long CopyOrCrypt(FileInfo source, DirectoryInfo target, string extensionwl)
        {
            if(source.Extension == extensionwl)
            {
                long encryptTime = CryptDecrypt(source, target);
                return encryptTime;
            }
            else
            {
                source.CopyTo(Path.Combine(target.FullName, source.Name), true);
                return 0;
            }
        }
        public static long CryptDecrypt(FileInfo sourceFilePath, DirectoryInfo destFilePath)
        {
            Stopwatch EncryptTime = new Stopwatch();
            EncryptTime.Start();
            string contenu = File.ReadAllText(sourceFilePath.FullName);
            string keyPath = @"C:\Users\franc\Source\Repos\C-hashtag-point-web\1.0\1.0\data\save\key.txt";
            string key = File.ReadAllText(keyPath);
            string text = Xor(contenu, key);
            string finalPath = destFilePath.FullName + "/" + sourceFilePath.Name;
            File.WriteAllText(finalPath, text);
            EncryptTime.Stop();
            return EncryptTime.ElapsedMilliseconds;
        }

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