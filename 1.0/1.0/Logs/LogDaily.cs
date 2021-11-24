using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;

namespace Projet.Logs
{
    public class LogDaily : LogBase
    {
        public long FilesSize { get; set; }
        public long TransferTime { get; set; }

        public LogDaily(string name)
        {
            Name = name;
        }
    }
}


