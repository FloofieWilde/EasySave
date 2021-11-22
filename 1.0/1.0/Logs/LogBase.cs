using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Projet.Logs
{
    public class LogBase
    {
        protected string Name { get; set; }
        protected string SourceDir { get; set; }
        protected string TargetDir { get; set; }
        protected string Timestamp { get; set; }

    }

}


