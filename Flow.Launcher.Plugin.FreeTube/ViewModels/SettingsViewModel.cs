using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.FreeTube.ViewModels
{
    public class SettingsViewModel : BaseModel
    {
        public SettingsViewModel(Settings _settings)
        {
            settings = _settings;
            windowFavorings = new() { "New Instance", "Existing Instance" };
        }

        public Settings settings { get; init; }

        public List<string> windowFavorings { get; set; }

        public string favoredWindowStrategy
        {
            get
            {
                return settings.favorNewInstance ? windowFavorings[0] : windowFavorings[1];
            }
            set
            {
                if (value ==  windowFavorings[0])
                {
                    settings.favorNewInstance = true;
                }
                else if (value == windowFavorings[1])
                {
                    settings.favorNewInstance = false;
                }
            }
        }
    }
}
