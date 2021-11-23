using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Projet.Logs
{
    public abstract class LogBase
    {
        public string Name { get; set; }
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public long TotalFiles { get; set; }
        public long TotalSize { get; set; }

    }

}


