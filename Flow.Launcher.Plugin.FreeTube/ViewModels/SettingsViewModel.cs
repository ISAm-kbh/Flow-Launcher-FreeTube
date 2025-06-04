using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.FreeTube.ViewModels
{
    public class SettingsViewModel : BaseModel, INotifyPropertyChanged
    {
        public SettingsViewModel(Settings _settings)
        {
            settings = _settings;
            windowFavorings = new() { "New Instance", "Existing Instance" };
        }

        public Settings settings { get; init; }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public List<string> windowFavorings { get; set; }

        public string favoredWindowStrategy
        {
            get
            {
                return this.settings.favorNewInstance ? windowFavorings[0] : windowFavorings[1];
            }
            set
            {
                if (value ==  windowFavorings[0])
                {
                    this.settings.favorNewInstance = true;
                }
                else if (value == windowFavorings[1])
                {
                    this.settings.favorNewInstance = false;
                }
            }
        }

        public bool useManuallySpecifiedPath
        {
            get => this.settings.manuallySpecifyPath;
            set
            {
                this.settings.manuallySpecifyPath = value;
                OnPropertyChanged(nameof(useManuallySpecifiedPath));
            }
        }

        public void clearExecutablePathWhenDisabled(bool newValue, bool oldValue)
        {
            if (!newValue && oldValue)
            {
                executablePath = string.Empty;
                OnPropertyChanged(nameof(executablePath));
            }
        }

        public string executablePath
        {
            get => this.settings.userSpecifiedPath;
            set => this.settings.userSpecifiedPath = value;
        }
    }
}
