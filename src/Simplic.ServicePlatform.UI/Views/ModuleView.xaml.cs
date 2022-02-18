using System;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for ModuleView.xaml
    /// </summary>
    public partial class ModuleView : Window
    {

        /// <summary>
        /// Instantiates the module view for given module definition.
        /// </summary>
        /// <param name="moduleDefinitionService">Service for module definitions</param>
        /// <param name="moduleDefinition">Module definition</param>
        public ModuleView(IModuleDefinitionService moduleDefinitionService, ModuleDefinition moduleDefinition)
        {
            InitializeComponent();
            DataContext = new ModuleViewModel(moduleDefinitionService, moduleDefinition);
        }

        private void EnableAutoStart_Changed(object sender, EventArgs e)
        {
            ModuleViewModel viewModel = (ModuleViewModel)DataContext;
            if (viewModel == null)
                return;
            EnableAutoStartText.Content = viewModel.EnableAutoStart ? "ON" : "OFF";
        }

        private void RequiredModulesList_Loaded(object sender, RoutedEventArgs e)
        {
            ModuleViewModel viewModel = (ModuleViewModel)DataContext;
            if (viewModel == null)
                return;

            // maybe this shouldn't be hidden at all
            //RequiredModulesLabel.Visibility = viewModel.Requires.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            //RequiredModulesList.Visibility = viewModel.Requires.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
