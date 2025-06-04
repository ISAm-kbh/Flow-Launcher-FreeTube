using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.FreeTube
{
    public class Settings
    {
        public bool favorNewInstance { get; set; } = true;
        public bool manuallySpecifyPath { get; set; } = false;
        public string userSpecifiedPath { get; set; } = string.Empty;
    }
}
