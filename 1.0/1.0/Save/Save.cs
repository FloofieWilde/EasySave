using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Projet.Save
{
    public class Save
    {
        private bool _complete = false;
        private string _sourceDir;
        private string _targetDir;
        public Save(string source, string target, bool type)
        {
            this._sourceDir = source;
            this._targetDir = target;
        }

        /*private bool CheckFiles()
        {
            DirectoryInfo sourceDir = new DirectoryInfo(_sourceDir);
            DirectoryInfo targetDir = new DirectoryInfo(_targetDir);

            if (sourceDir.Exists && targetDir.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CopyFiles(string sourceDir, string targetDir)
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
        }*/

    }
}