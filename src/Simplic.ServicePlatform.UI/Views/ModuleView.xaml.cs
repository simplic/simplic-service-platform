using System;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for ModuleView.xaml
    /// </summary>
    public partial class ModuleView : Window
    {
        private readonly IServiceClient serviceClient;

        /// <summary>
        /// Instantiates the module view for given module definition.
        /// </summary>
        /// <param name="serviceClient">Client for managing requests</param>
        /// <param name="moduleDefinition">Module definition</param>
        public ModuleView(IServiceClient serviceClient, ModuleDefinition moduleDefinition)
        {
            InitializeComponent();
            this.serviceClient = serviceClient;
            DataContext = new ModuleViewModel(serviceClient, moduleDefinition);
        }

        private void RequiredModulesList_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ModuleViewModel;
            if (viewModel == null)
                return;
        }

        private void AvailableModulesRadListBox_Drop(object sender, DragEventArgs e)
        {
            var viewModel = DataContext as ModuleViewModel;
            if (string.IsNullOrEmpty(viewModel.SelectedRequiredModule)) return; //no sufficient fix, need to early return when drag&drop was inside the same container

            RestockAvailableModuleByName(viewModel.SelectedRequiredModule);
            viewModel.RaisePropertyChanged(nameof(viewModel.AvailableModules));
        }

        private void RequiredModulesRadListBox_Drop(object sender, DragEventArgs e)
        {
            var viewModel = DataContext as ModuleViewModel;
            if (viewModel.SelectedAvailableModule == null) return;
            AddModuleToRequired(viewModel.SelectedAvailableModule);
        }

        /// <summary>
        /// Adds given module back to available modules, if exists.
        /// </summary>
        /// <param name="targetModule">Module to be added back to available modules</param>
        private void RestockAvailableModuleByName(string targetModuleName)
        {
            var viewModel = DataContext as ModuleViewModel;

            Application.Current.Dispatcher.Invoke(async () =>
            {
                var availableModule = await serviceClient.GetModule(targetModuleName);
                if (availableModule != null)
                {
                    viewModel.AvailableModules.Add(availableModule);
                    viewModel.SelectedAvailableModule = availableModule;
                }
            });
        }

        private void AddModuleToRequired(ModuleDefinition moduleDefinition)
        {
            var viewModel = DataContext as ModuleViewModel;
            if (string.IsNullOrEmpty(moduleDefinition.Name)) return;

            viewModel.Requires.Add(moduleDefinition.Name);
            viewModel.ObservableRequires.Add(moduleDefinition.Name);
            viewModel.SelectedRequiredModule = moduleDefinition.Name;
            viewModel.RaisePropertyChanged(nameof(viewModel.Requires));
            viewModel.RaisePropertyChanged(nameof(viewModel.ObservableRequires));
        }

    }
}
