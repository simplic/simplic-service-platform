using System.Collections.Generic;
using System.Windows;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ServiceView : Window
    {
        private readonly IServiceClient serviceClient;

        /// <summary>
        /// Instantiates the view for the given module.
        /// </summary>
        public ServiceView(IServiceClient serviceClient)
        {
            InitializeComponent();
            this.serviceClient = serviceClient;
            DataContext = new ServiceViewModel(serviceClient);
        }

        private void AvailableModulesRadListBox_Drop(object sender, DragEventArgs e)
        {
            //var viewModel = DataContext as ServiceViewModel;
            //if (viewModel.SelectedServiceModule == null) return; //no sufficient fix, need to early return when drag&drop was inside the same container

            //var newAvailableModule = ServiceModuleToModuleDefinition(viewModel.SelectedServiceModule);

            //if (newAvailableModule == null) return;
            //viewModel.SelectedAvailableModule = newAvailableModule;
            //viewModel.AvailableModules.Add(newAvailableModule);
            //viewModel.RaisePropertyChanged(nameof(viewModel.AvailableModules));
        }

        private void UsedModulesRadListBox_Drop(object sender, DragEventArgs e)
        {
            //var viewModel = DataContext as ServiceViewModel;
            //if (viewModel.SelectedAvailableModule == null) return;
            

            //ServiceModule newServiceModule = ModuleDefinitionToServiceModule(viewModel.SelectedAvailableModule);

            //viewModel.SelectedServiceModule = newServiceModule;
            //viewModel.RaisePropertyChanged(nameof(viewModel.SelectedServiceModule));
            //viewModel.RaisePropertyChanged(nameof(viewModel.SelectedServiceModuleConfiguration));

            ////viewModel.Modules.Add(newServiceModule);
            //viewModel.Services[0].AddModule(newServiceModule);
            //viewModel.Services[0].Update();

            //viewModel.RaisePropertyChanged(nameof(viewModel.Modules));
            //viewModel.ObservableModules.Add(newServiceModule);
            //viewModel.RaisePropertyChanged(nameof(viewModel.ObservableModules));
        }

        private ModuleDefinition ServiceModuleToModuleDefinition(ServiceModule serviceModule)
        {
            ModuleDefinition newModuleDefinition = new ModuleDefinition();
            Application.Current.Dispatcher.Invoke(async () =>
            {
                newModuleDefinition = await serviceClient.GetModule(serviceModule.Name);
            });
            return newModuleDefinition;
        }

        private static ServiceModule ModuleDefinitionToServiceModule(ModuleDefinition moduleDefinition)
        {
            var newServiceModule = new ServiceModule
            {
                Name = moduleDefinition.Name,
                Configuration = new List<ServiceModuleConfiguration>()
            };

            foreach (var config in moduleDefinition.ConfigurationDefinition)
                newServiceModule.Configuration.Add(new ServiceModuleConfiguration { Name = config.Name, Value = config.Default });
            return newServiceModule;
        }
    }
}
