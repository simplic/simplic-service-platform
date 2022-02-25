using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Simplic.ServicePlatform.UI
{
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
            UsedModules = new ObservableCollection<ServiceModule>(model.Modules);
        }

        /// <summary>
        /// Adds a module to the service definition.
        /// </summary>
        /// <param name="serviceModule">Module</param>
        public void AddModule(ServiceModule serviceModule)
        {
            UsedModules.Add(serviceModule);
            Model.Modules.Add(serviceModule);
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
        /// Gets or sets the collection of used modules.
        /// </summary>
        public ObservableCollection<ServiceModule> UsedModules { get; set; }

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
    }
}