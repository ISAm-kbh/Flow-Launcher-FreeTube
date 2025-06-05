using Flow.Launcher.Plugin.FreeTube.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Flow.Launcher.Plugin.FreeTube.Views
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl(SettingsViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void ExecutablePathTextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (this.DataContext as SettingsViewModel)?.clearExecutablePathWhenDisabled((bool)e.NewValue, (bool)e.OldValue);
        }
    }
}
