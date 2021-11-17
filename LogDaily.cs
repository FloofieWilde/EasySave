using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;


namespace Projet
{
    public class LogDaily
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public Int64 FileSize { get; set; }
        public Int64 FileTransferTime { get; set; }
        public string Time { get; set; }
        public LogDaily(string name, string source, string target, Int64 size, Int64 transferTime, string time)
        {
            Name = name;
            FileSource = source;
            FileTarget = target;
            FileSize = size;
            FileTransferTime = transferTime;
            Time = time;
        }

        
    }
}


