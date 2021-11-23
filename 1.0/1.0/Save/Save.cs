using System;
using System.IO;
using Projet.Logs;


namespace Projet.SaveSystem
{
    public class Save
    {
        readonly string SourceDir;
        readonly string TargetDir;
        readonly bool Full;
        LogState CurrentLog;
        public Save(string source, string target, bool full)
        {
            SourceDir = source;
            TargetDir = target;
            Full = full;
        }
        public void Copy()
        {
            string copyType = "Partial";
            if (Full) copyType = "Complete";

            DirectoryInfo sourceDirInfo = new DirectoryInfo(SourceDir);
            long filesNumber = Directory.GetFiles(SourceDir, "*", SearchOption.AllDirectories).Length;
            long filesSize = DirSize(sourceDirInfo);

            CurrentLog = new LogState(copyType, SourceDir, TargetDir, filesNumber, filesSize);
            DirectoryInfo targetDirInfo = new DirectoryInfo(TargetDir);

            if (!targetDirInfo.Exists)
            {
                Directory.CreateDirectory(TargetDir);
            }
            FullCopy(sourceDirInfo, targetDirInfo);
            CurrentLog.End();
        }
        private void FullCopy(DirectoryInfo source, DirectoryInfo target)
        {
            CurrentLog.Display(true);
            long filesSize;
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                filesSize = fi.Length;
                CurrentLog.Update(filesSize);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                FullCopy(diSourceSubDir, nextTargetSubDir);
                CurrentLog.Save();
                CurrentLog.Display();

            }
            
        }

        private void DiffCopy(DirectoryInfo source, DirectoryInfo target)
        {

        }
        private static long DirSize(DirectoryInfo directory)
        {
            long size = 0;
            FileInfo[] fis = directory.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            DirectoryInfo[] dis = directory.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

    }
}