using System;
using System.Collections.Generic;
using System.Text;

namespace Projet.Logs
{
    public class LogJson
    {
        public long FileSize { get; set; }
        public double TransferTime { get; set; }
        public long EncryptTime { get; set; }
        public string Name { get; set; }
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
    }
}
