using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Projet
{
    public class Save
    {
        //Allow the transfer of files. Has the role of model.
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

            //Create a folder if it doesn't exist yet
            Directory.CreateDirectory(destDir);

            //Copy-paste the folders
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            //Copy past the sub folders
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDir, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, true);
            }


        }


    }

}