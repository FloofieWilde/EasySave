using System;
using System.Collections.Generic;
using System.Text;

namespace Options
{
    public class Path
    {
        public Dictionary<string, NameSourceDest> Preset1 { get; set; }
        public Dictionary<string, NameSourceDest> Preset2 { get; set; }
        public Dictionary<string, NameSourceDest> Preset3 { get; set; }
        public Dictionary<string, NameSourceDest> Preset4 { get; set; }
        public Dictionary<string, NameSourceDest> Preset5 { get; set; }
    }

    public class NameSourceDest
    {
        public string Name { get; set; }
        public string PathSource { get; set; }
        public string PathDestination { get; set; }
    }
}
