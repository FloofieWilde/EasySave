using System;
using System.IO;
using Projet.Logs;
using System.Diagnostics;
using Projet.Languages;

namespace Projet.SaveSystem
{/// <summary>
/// The class that manages the save system
/// </summary>
    public class Save
    {
        private readonly string SourceDir;
        private readonly string TargetDir;
        private readonly bool Full;
        private LogState CurrentStateLog;
        private LogDaily CurrentDailyLog;
        private readonly Stopwatch ProcessTime;
        private long EncryptTime;
        private readonly string ExtensionWhitelist = ".txt";

        public Save(string source, string target, bool full)
        {
            SourceDir = source;
            TargetDir = target;
            Full = full;
            ProcessTime = new Stopwatch();
        }
        /// <summary>
        /// Fetches basic data like copy type or directory size then call ProcessCopy
        /// </summary>
        public void Copy()
        {
            Process[] pname = Process.GetProcessesByName("Calculator");
            if (pname.Length > 0)
            {
                Console.WriteLine("Merci de stopper votre logiciel métier avant de lancer une sauvegarde ");
                return;
            }
            string copyType = "Partial";
            if (Full) copyType = "Complete";
            Langue.Language currentLanguage = Langue.GetLang();

            DirectoryInfo sourceDirInfo = new DirectoryInfo(SourceDir);
            if (!sourceDirInfo.Exists)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(currentLanguage.SaveSauceNotExists);
                Console.ResetColor();
                return;
            }
            long filesNumber = Directory.GetFiles(SourceDir, "*", SearchOption.AllDirectories).Length;
            long filesSize = DirSize(sourceDirInfo);

            CurrentStateLog = new LogState(copyType, SourceDir, TargetDir, filesNumber, filesSize);
            CurrentDailyLog = new LogDaily(copyType);
            DirectoryInfo targetDirInfo = new DirectoryInfo(TargetDir);

            if (!targetDirInfo.Exists)
            {
                Directory.CreateDirectory(TargetDir);
            }
            ProcessCopy(sourceDirInfo, targetDirInfo);

            CurrentStateLog.End();
        }
        /// <summary>
        /// Process the copy, writing logs at the same time
        /// </summary>
        /// <param name="source">Source Directory</param>
        /// <param name="target">Target Directory</param>
        private void ProcessCopy(DirectoryInfo source, DirectoryInfo target)
        {
            CurrentStateLog.Display();
            long filesSize;
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                ProcessTime.Start();
                if (Full == false)
                {
                    if (File.GetLastWriteTime(target.FullName) != File.GetLastWriteTime(fi.FullName))
                    {
                        EncryptTime = Crypt.CopyOrCrypt(fi, target, ExtensionWhitelist);
                    }
                }
                else
                {
                    EncryptTime = Crypt.CopyOrCrypt(fi, target, ExtensionWhitelist);
                }
                filesSize = fi.Length;
                ProcessTime.Stop();
                CurrentDailyLog.Update(filesSize, ProcessTime.ElapsedMilliseconds, fi.Name, target.Name, EncryptTime);
                CurrentStateLog.Update(filesSize);
                ProcessTime.Reset();
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
                ProcessCopy(diSourceSubDir, nextTargetSubDir);
                CurrentStateLog.Save();
                CurrentStateLog.Display();
            }
        }
        /// <summary>
        /// Returns directory's size, in bytes
        /// </summary>
        /// <param name="directory">The directory to measure</param>
        /// <returns></returns>
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