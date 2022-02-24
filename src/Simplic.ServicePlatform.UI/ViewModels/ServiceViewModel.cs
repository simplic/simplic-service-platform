using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Simplic.ServicePlatform.UI
{
    public class ServiceViewModel : SSPViewModelBase
    {
        private readonly IServiceClient serviceClient;

        private ServiceDefinition serviceDefinition;
        private ModuleDefinition selectedAvailableModule;
        private ServiceModule selectedServiceModule;
        private ObservableCollection<ServiceModule> observableModules;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">service</param>
        public ServiceViewModel(IServiceClient serviceClient, ServiceDefinition serviceDefinition)
        {
            this.serviceClient = serviceClient;
            this.serviceDefinition = serviceDefinition;

            LoadAvailableServices();
            LoadAvailableModules();
            ObservableModules = new ObservableCollection<ServiceModule>(Modules);

            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
        }

        private void LoadAvailableServices()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                AvailableServices = new ObservableCollection<ServiceDefinition>(await serviceClient.GetAllServices());
            });
        }

        private void LoadAvailableModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                AvailableModules = new ObservableCollection<ModuleDefinition>(await serviceClient.GetAllModules());
            });
        }
        private void Save()
        {
            serviceClient.SaveService(serviceDefinition);
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(ServiceName);
        }

        /// <summary>
        /// Command for saving.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the selected available module.
        /// </summary>
        public ModuleDefinition SelectedAvailableModule { get => selectedAvailableModule; set { selectedAvailableModule = value; OnPropertyChanged(); } }

        /// <summary>
        /// Gets or sets the selected module.
        /// </summary>
        public ServiceModule SelectedServiceModule { get => selectedServiceModule; set { selectedServiceModule = value; OnPropertyChanged(); OnPropertyChanged(nameof(SelectedServiceModuleConfiguration)); } }

        /// <summary>
        /// Gets the configuration of the selected service module.
        /// </summary>
        public IList<ServiceModuleConfiguration> SelectedServiceModuleConfiguration
        {
            get => (SelectedServiceModule != null && SelectedServiceModule.Configuration != null)
                ? SelectedServiceModule.Configuration
                : new List<ServiceModuleConfiguration>();
        }

        /// <summary>
        /// Gets a list of available modules.
        /// </summary>
        public IList<ModuleDefinition> AvailableModules { get; set; }

        /// <summary>
        /// Gets or sets the collection that represents the modules of the service.
        /// </summary>
        public ObservableCollection<ModuleDefinition> ModulesBridge { get ; set; }

        /// <summary>
        /// Gets or sets the id of the service.
        /// </summary>
        public Guid Id { get => serviceDefinition.Id; set => serviceDefinition.Id = value; }

        /// <summary>
        /// Gets or sets the service name of the service.
        /// </summary>
        public string ServiceName { get => serviceDefinition.ServiceName; set => serviceDefinition.ServiceName = value; }

        /// <summary>
        /// Gets or sets the modules of the service.
        /// </summary>
        public IList<ServiceModule> Modules { get => serviceDefinition.Modules; set => serviceDefinition.Modules = value; }

        /// <summary>
        /// Gets or sets the observable collection of the modules.
        /// </summary>
        public ObservableCollection<ServiceModule> ObservableModules { get => observableModules; set { observableModules = value; RaisePropertyChanged(nameof(ObservableModules)); } }

        /// <summary>
        /// Gets or sets available services.
        /// </summary>
        public ObservableCollection<ServiceDefinition> AvailableServices { get; set; }
    }
}
