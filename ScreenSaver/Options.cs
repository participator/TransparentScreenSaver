using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSaver
{
    enum Options
    {
        show,
        preview,
        configure
    }

    class OptionProvided
    {
        public Options selected { get; set; }
        public long? pointerArg { get; set; } 
    }
}
