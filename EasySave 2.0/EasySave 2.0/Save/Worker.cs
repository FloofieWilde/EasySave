using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Projet.Save
{
    public class Worker
    {
        public BackgroundWorker worker { get; set; }
        public int Id { get; set; }
        public int Statut { get; set; }
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
