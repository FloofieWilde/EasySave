using System;
using System.IO;
using Projet.Logs;
using System.Diagnostics;
using Projet.Languages;
using EasySave_2._0;
using Projet.WorkSoftwares;
using System.Windows.Controls;
using System.ComponentModel;

namespace Projet.SaveSystem
{/// <summary>
/// The class that manages the save system
/// </summary>
    public class Save
    {
        private readonly string SourceDir;
        private readonly string TargetDir;
        private readonly bool Full;
        public LogState CurrentStateLog;
        private LogDaily CurrentDailyLog;
        private readonly Stopwatch ProcessTime;
        private long CryptTime;

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
        public (DirectoryInfo source, DirectoryInfo target, int error) Copy()
        {
            var app = WorkSoftware.GetJsonApplication();
            Process[] pname = Process.GetProcessesByName(app.Application);

            if (pname.Length > 0) return (null, null, 1);
            string copyType = "Partial";
            if (Full) copyType = "Complete";

            DirectoryInfo sourceDirInfo = new DirectoryInfo(SourceDir);
            if (!sourceDirInfo.Exists) return (null, null, 2 );
            long filesNumber = Directory.GetFiles(SourceDir, "*", SearchOption.AllDirectories).Length;
            long filesSize = DirSize(sourceDirInfo);
            if (filesSize == 0) return (null, null, 3);
            CurrentStateLog = new LogState(copyType, SourceDir, TargetDir, filesNumber, filesSize);
            CurrentDailyLog = new LogDaily(copyType);
            DirectoryInfo targetDirInfo = new DirectoryInfo(TargetDir);

            if (!targetDirInfo.Exists)
            {
                Directory.CreateDirectory(TargetDir);
            }

            return (sourceDirInfo, targetDirInfo, 0);
        }
        /// <summary>
        /// Process the copy, writing logs at the same time
        /// </summary>
        /// <param name="source">Source Directory</param>
        /// <param name="target">Target Directory</param>
        public void ProcessCopy(DirectoryInfo source, DirectoryInfo target, ProgressBar progressBar, object sender)
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
                        CheckException(fi, target);
                    }
                }
                else
                {
                    CheckException(fi, target);
                }
                //progressBar.Value = CurrentStateLog.Progress;
                if ((sender as BackgroundWorker).WorkerReportsProgress == true)
                {
                    (sender as BackgroundWorker).ReportProgress(CurrentStateLog.Progress);
                }
                
                filesSize = fi.Length;
                ProcessTime.Stop();
                CurrentDailyLog.Update(filesSize, ProcessTime.ElapsedMilliseconds, fi.Name, target.Name, CryptTime);
                CurrentStateLog.Update(filesSize);
                ProcessTime.Reset();
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
                ProcessCopy(diSourceSubDir, nextTargetSubDir, progressBar, sender);
                CurrentStateLog.Save();
                CurrentStateLog.Display();
            }
            CurrentStateLog.End(progressBar);
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
        private void CheckException(FileInfo source, DirectoryInfo target)
        {
            try
            {
                CryptTime = Crypt.CryptOrSave(source, target);
            }
            catch (IOException)
            {
                CryptTime = -1;
            }
        }
    }
}