using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Flow.Launcher.Plugin.FreeTube.ViewModels
{
    public class SettingsViewModel : BaseModel, INotifyPropertyChanged
    {
        public SettingsViewModel(Settings _settings, PluginInitContext _context)
        {
            settings = _settings;
            context = _context;
            windowFavorings = new() { context.API.GetTranslation("flowlauncher_plugin_freetube_new_instance"),
                    context.API.GetTranslation("flowlauncher_plugin_freetube_existing_instance") };
        }

        public Settings settings { get; init; }
        private readonly PluginInitContext context;

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
