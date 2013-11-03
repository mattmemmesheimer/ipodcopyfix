using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPodFixGui.ipodfix
{
    class ProgressEventArgs
    {
        public bool Completed { get; set; }
        public double PercentCompleted { get; set; }

        public ProgressEventArgs() : this(false, -1)
        {
        }

        public ProgressEventArgs(bool Completed, double PercentCompleted)
        {
            this.Completed = Completed;
            this.PercentCompleted = PercentCompleted;
        }
    }
}
