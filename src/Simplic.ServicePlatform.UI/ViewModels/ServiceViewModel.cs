using Simplic.PlugIn.ServicePlatform.Server;
using Simplic.ServicePlatform.Data.DB;
using Simplic.ServicePlatform.Service;
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
        private readonly IServiceDefinitionService serviceDefinitionService;
        private readonly IModuleDefinitionService moduleDefinitionService;
        private ServiceDefinition serviceDefinition;
        private ModuleDefinition selectedAvailableModule;
        private ServiceModule selectedServiceModule;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        /// <param name="serviceDefinition">service</param>
        public ServiceViewModel(IServiceDefinitionService serviceDefinitionService, IModuleDefinitionService moduleDefinitionService, ServiceDefinition serviceDefinition)
        {
            this.serviceDefinitionService = serviceDefinitionService;
            this.moduleDefinitionService = moduleDefinitionService;
            this.serviceDefinition = serviceDefinition;

            SaveCommand = new RelayCommand(o => Save(), o => CanSave());
            //LoadAvailableModules();
            LoadDummies();
        }

        private void LoadAvailableModules()
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                AvailableModules = await moduleDefinitionService.GetAll();
            });
        }

        private void LoadDummies()
        {
            AvailableModules = new ObservableCollection<ModuleDefinition>
            {
                new ModuleDefinition
                {
                    Name = "example.module",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition>
                    {
                        new ModuleConfigurationDefinition { Name = "some config", Default = "v" }
                    }
                },
                new ModuleDefinition
                {
                    Name = "example.module.two",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition> { new ModuleConfigurationDefinition { Name = "some config2", Default = "v2" }
                    }
                },
                new ModuleDefinition
                {
                    Name = "example.module.three",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition> { new ModuleConfigurationDefinition { Name = "some config3", Default = "v3" }
                    }
                },
                new ModuleDefinition
                {
                    Name = "example.module.four",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition> { new ModuleConfigurationDefinition { Name = "some config4", Default = "v4" }
                    }
                },
                new ModuleDefinition
                {
                    Name = "example.module.five",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition> { new ModuleConfigurationDefinition { Name = "some config5", Default = "v5" }
                    }
                },
                new ModuleDefinition
                {
                    Name = "example.module.six",
                    ConfigurationDefinition = new List<ModuleConfigurationDefinition>{ new ModuleConfigurationDefinition { Name = "some config6", Default = "v6" }
                    }
                }
            };

            ModulesDummy = new ObservableCollection<ServiceModule>() { new ServiceModule { Name = "some.service.module" } };
        }

        private void Save()
        {
            serviceDefinitionService.Save(serviceDefinition);
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
                : new List<ServiceModuleConfiguration> { new ServiceModuleConfiguration { Name = "congrats you found", Value = "nothing." } };
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
        public IList<ServiceModule> ModulesDummy { get; set; } //TBR
    }
}
