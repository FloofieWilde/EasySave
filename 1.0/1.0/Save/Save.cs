using System;
using System.IO;
using Projet.Logs;
using System.Diagnostics;


namespace Projet.SaveSystem
{
    public class Save
    {
        private readonly string SourceDir;
        private readonly string TargetDir;
        private readonly bool Full;
        private LogState CurrentStateLog;
        private LogDaily CurrentDailyLog;
        private Stopwatch ProcessTime;
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

            CurrentStateLog = new LogState(copyType, SourceDir, TargetDir, filesNumber, filesSize);
            CurrentDailyLog = new LogDaily(copyType);
            DirectoryInfo targetDirInfo = new DirectoryInfo(TargetDir);

            if (!targetDirInfo.Exists)
            {
                Directory.CreateDirectory(TargetDir);
            }
            FullCopy(sourceDirInfo, targetDirInfo);
            CurrentStateLog.End();
        }
        private void FullCopy(DirectoryInfo source, DirectoryInfo target)
        {
            CurrentStateLog.Display(true);
            long filesSize;
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                filesSize = fi.Length;
                CurrentDailyLog.Save("Daily");
                CurrentStateLog.Update(filesSize);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                FullCopy(diSourceSubDir, nextTargetSubDir);
                CurrentStateLog.Save("State");
                CurrentStateLog.Display();
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