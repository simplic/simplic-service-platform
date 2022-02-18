using System.Collections.Generic;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceView : Window
    {
        private IModuleDefinitionService moduleDefinitionService;

        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IModuleDefinitionService moduleDefinitionService, ServiceDefinition serviceDefinition)
        {
            InitializeComponent();
            this.moduleDefinitionService = moduleDefinitionService;
            DataContext = new ServiceViewModel(moduleDefinitionService, serviceDefinition);
        }

        private void AvailableModulesRadListBox_Drop(object sender, DragEventArgs e)
        {
            var viewModel = DataContext as ServiceViewModel;
            if (viewModel.SelectedServiceModule == null) return; //no sufficient fix, need to early return when drag&drop was inside the same container

            Application.Current.Dispatcher.Invoke(async () =>
            {
                ModuleDefinition newAvailableModule = await moduleDefinitionService.GetByName(viewModel.SelectedServiceModule.Name);
                viewModel.SelectedAvailableModule = newAvailableModule;
                viewModel.AvailableModules.Add(newAvailableModule);
                viewModel.RaisePropertyChanged(nameof(viewModel.AvailableModules));
            });
        }

        private void UsedModulesRadListBox_Drop(object sender, DragEventArgs e)
        {
            var viewModel = DataContext as ServiceViewModel;
            if (viewModel.SelectedAvailableModule == null) return;

            var newServiceModule = new ServiceModule
            {
                Name = viewModel.SelectedAvailableModule.Name,
                Configuration = new List<ServiceModuleConfiguration>()
            };

            foreach (var config in viewModel.SelectedAvailableModule.ConfigurationDefinition)
                newServiceModule.Configuration.Add(new ServiceModuleConfiguration { Name = config.Name, Value = config.Default });

            viewModel.SelectedServiceModule = newServiceModule;
            viewModel.RaisePropertyChanged(nameof(viewModel.SelectedServiceModule));

            viewModel.RaisePropertyChanged(nameof(viewModel.SelectedServiceModuleConfiguration));

            viewModel.ModulesDummy.Add(newServiceModule);
            viewModel.RaisePropertyChanged(nameof(viewModel.ModulesDummy));
        }
    }
}
