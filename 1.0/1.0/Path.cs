using System;
using System.Collections.Generic;
using System.Text;

namespace Options
{
    /// <summary>
    /// Class to declare the preset. Each preset have 3 column (name, source, destination)
    /// </summary>
    public class Path
    {
        public Dictionary<string, NameSourceDest> Preset1 { get; set; }
        public Dictionary<string, NameSourceDest> Preset2 { get; set; }
        public Dictionary<string, NameSourceDest> Preset3 { get; set; }
        public Dictionary<string, NameSourceDest> Preset4 { get; set; }
        public Dictionary<string, NameSourceDest> Preset5 { get; set; }
    }
}
