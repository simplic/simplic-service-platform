using Simplic.PlugIn.ServicePlatform.Server;
using Simplic.ServicePlatform.Data.DB;
using Simplic.ServicePlatform.Service;
using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Simplic.ServicePlatform.UI
{
    internal class ServiceViewModel : SSPViewModelBase
    {
        private IModuleDefinitionService moduleDefinitionService;
        private ServiceDefinition serviceDefinition;
        private ModuleDefinition selectedAvailableModule;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">service</param>
        public ServiceViewModel(IModuleDefinitionService moduleDefinitionService, ServiceDefinition serviceDefinition)
        {
            this.moduleDefinitionService = moduleDefinitionService;
            this.serviceDefinition = serviceDefinition;

            SaveCommand = new RelayCommand(o => Save());
        }

        private void Save()
        {
            MessageBox.Show("not implemented");
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
        /// Gets a list of available modules.
        /// </summary>
        public IList<ModuleDefinition> AvailableModules
        {
            get
            {
                return new List<ModuleDefinition>
                {
                    new ModuleDefinition { Name = "example.module" },
                    new ModuleDefinition { Name = "example.module.two" }
                };
                return (IList<ModuleDefinition>)Application.Current.Dispatcher.Invoke(async () =>
                {
                    return await moduleDefinitionService.GetAll();
                });
            }
        }

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
    }
}
