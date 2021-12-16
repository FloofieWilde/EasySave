using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Projet.Save
{
    public class Worker
    {
        public int Id { get; set; }
        public BackgroundWorker worker { get; set; }
        public ManualResetEvent WorkEvent { get; set; }
        public string Statut { get; set; }
        public string PreviousStatut { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string CopyType { get; set; }
        public string DateStart { get; set; }
        public long TotalFiles { get; set; }
        public long TotalSize { get; set; }
        public double Progress { get; set; }
        public long RemainingFiles { get; set; }
        public long RemainingFilesSize { get; set; }
    }
}
