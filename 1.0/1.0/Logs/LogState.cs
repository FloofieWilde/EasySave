using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet.Logs
{
    public class LogState : LogBase
    {
        private bool _active = false;
        private int _progress = 0;
        private long _remainingFiles;
        private long _remainingFilesSize;
        private long _totalFiles;
        private long _totalSize;

 
    }
}