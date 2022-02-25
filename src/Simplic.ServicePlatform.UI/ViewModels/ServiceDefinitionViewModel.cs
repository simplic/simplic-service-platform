﻿using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;

namespace Simplic.ServicePlatform.UI
{
    //ViewModel Stufe 2
    public class ServiceDefinitionViewModel : ViewModelBase
    {
        private ServiceDefinition model;
        private ServiceModule selectedServiceModule;

        /// <summary>
        /// Instantiates the view model.
        /// </summary>
        public ServiceDefinitionViewModel()
        {
        }

        /// <summary>
        /// Instantiates the view model for given model.
        /// </summary>
        public ServiceDefinitionViewModel(ServiceDefinition model)
        {
            Model = model;
            //[old] UsedModules = new ObservableCollection<ServiceModule>(model.Modules);
            DropCommand = new RelayCommand(o => AddFromModuleDefinition(o as ModuleDefinition));
            UsedModules = new ObservableCollection<ServiceModuleViewModel>(model.Modules.Select(m => new ServiceModuleViewModel(m)));
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

        public void AddFromModuleDefinition(ModuleDefinition moduleDefinition)
        {
            var newServiceModule = new ServiceModule
            {
                Name = moduleDefinition.Name,
                Configuration = new List<ServiceModuleConfiguration>
                (
                    moduleDefinition.ConfigurationDefinition.Select(config =>
                    {
                        return new ServiceModuleConfiguration { Name = config.Name, Value = config.Default };
                    })
                )
            };

            AddModule(newServiceModule);
        }

        /// <summary>
        /// Update the view.
        /// </summary>
        public void Update()
        {
            RaisePropertyChanged(nameof(Model));
            RaisePropertyChanged(nameof(UsedModules));
        }

        /* [old]
        /// <summary>
        /// Gets or sets the collection of used modules.
        /// </summary>
        public ObservableCollection<ServiceModule> UsedModules { get; set; }
        */
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

        public ICommand DropCommand { get; set; }
    }
}