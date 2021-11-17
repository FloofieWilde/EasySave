using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Projet
{
    public class Save
    {
        public Save(string source, string target, bool type)
        {
            DirectoryCopy(source, target, type);
        }
        public void DirectoryCopy(string sourceDir, string destDir, bool type)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // Créer le dossier si il n'existe pas      
            Directory.CreateDirectory(destDir);

            // Copier coller les fichiers
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            // Copier coller les sous dossiers

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDir, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, true);
            }


        }


    }

}