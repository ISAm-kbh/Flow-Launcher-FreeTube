using Flow.Launcher.Plugin.FreeTube.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Flow.Launcher.Plugin.FreeTube.Views
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private readonly Settings _settings;
        private readonly SettingsViewModel _viewModel;
        
        public SettingsControl(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            _settings = _viewModel.settings;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void SettingsControl_Loaded(object sender, RoutedEventArgs e)
        {
            WindowStrategyComboBox.SelectedItem = _viewModel.favoredWindowStrategy;
        }
    }
}
