using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System;

namespace Simplic.ServicePlatform.UI
{
    //ViewModel Stufe 2
    public class ServiceDefinitionViewModel : ViewModelBase
    {
        private ServiceDefinition model;
        private ServiceModule selectedServiceModule;
        private ServiceViewModel parent;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        public ServiceDefinitionViewModel()
        {
        }

        /// <summary>
        /// Instantiates the view model for given model.
        /// </summary>
        public ServiceDefinitionViewModel(ServiceDefinition model, ServiceViewModel parent)
        {
            Model = model;
            Parent = parent;
            DropCommand = new RelayCommand(o => AddAvailableModule(), o => CanAddAvailableModule());
            UsedModules = model.Modules != null
                ? new ObservableCollection<ServiceModuleViewModel>(model.Modules.Select(m => new ServiceModuleViewModel(m)))
                : new ObservableCollection<ServiceModuleViewModel>();
        }

        /// <summary>
        /// Adds a module to the service definition.
        /// </summary>
        /// <param name="serviceModule">Module</param>
        public void AddModule(ServiceModule serviceModule)
        {
            Model.Modules.Add(serviceModule);
            UsedModules.Add(new ServiceModuleViewModel(serviceModule));
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(UsedModules));
        }

        /// <summary>
        /// Adds the selected available module to this service's module collection.
        /// </summary>
        public void AddAvailableModule()
        {
            var newServiceModule = new ServiceModule
            {
                Name = Parent.SelectedAvailableModule.Name,
                Configuration = new List<ServiceModuleConfiguration>
                    (
                        Parent.SelectedAvailableModule.ConfigurationDefinition.Select(config =>
                        {
                            return new ServiceModuleConfiguration { Name = config.Name, Value = config.Default };
                        })
                    )
            };
            AddModule(newServiceModule);
        }

        /// <summary>
        /// Checks whether the conditions are met for adding a module.
        /// </summary>
        public bool CanAddAvailableModule()
        {
            if (Parent.SelectedAvailableModule != null)
                return true;
            return false;
        }

        /// <summary>
        /// Update the view.
        /// </summary>
        public void Update()
        {
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(UsedModules));
        }

        /// <summary>
        /// Synchronizes the bindings with the model.
        /// </summary>
        public void Synch()
        {
            foreach (var module in UsedModules)
                module.Synch();
            Model.Modules = new List<ServiceModule>(UsedModules.Select(m => m.Model));
        }

        /// <summary>
        /// Updates the configurations of the given module with given ones.
        /// </summary>
        /// <param name="moduleName">Module</param>
        /// <param name="newConfigurations">New configurations</param>
        public void UpdateConfigurations(string moduleName, IEnumerable<ServiceModuleConfiguration> newConfigurations)
        {
            foreach (var module in UsedModules)
            {
                if (module.Model.Name.Equals(moduleName))
                {
                    overwriteConfigValues(module.ConfigurationDefinitions, newConfigurations);
                    module.ConfigurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(newConfigurations);
                }
            }
        }

        /// <summary>
        /// Overwrites target configuration values with native configuration values.
        /// </summary>
        /// <param name="nativeConfigurations">Used configuration values</param>
        /// <param name="targetConfigurations">Configurations that should be overwritten</param>
        private void overwriteConfigValues(IEnumerable<ServiceModuleConfiguration> nativeConfigurations, IEnumerable<ServiceModuleConfiguration> targetConfigurations)
        {
            foreach (var newConfiguration in targetConfigurations)
                foreach (var configuration in nativeConfigurations)
                {
                    if (configuration.Name == null) return;
                    if (configuration.Name.Equals(newConfiguration.Name))
                        configuration.Value = newConfiguration.Value;

                }
        }

        /// <summary>
        /// Gets or sets the collection of used modules.
        /// </summary>
        public ObservableCollection<ServiceModuleViewModel> UsedModules { get; set; }

        /// <summary>
        /// Gets or sets the model for the service definition.
        /// </summary>
        public ServiceDefinition Model { get => model; set => model = value; }

        /// <summary>
        /// Gets or sets the selected module.
        /// </summary>
        public ServiceModule SelectedServiceModule { get => selectedServiceModule; set { selectedServiceModule = value; RaisePropertyChanged(nameof(SelectedServiceModule)); RaisePropertyChanged(nameof(SelectedServiceModuleConfiguration)); } }

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
        /// Gets or sets the command for handling drop events.
        /// </summary>
        public ICommand DropCommand { get; set; }

        /// <summary>
        /// Gets and sets the parent of this viewmodel.
        /// </summary>
        public ServiceViewModel Parent { get => parent; set => parent = value; }
    }
}