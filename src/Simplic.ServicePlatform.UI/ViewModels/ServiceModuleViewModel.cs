using Simplic.UI.MVC;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simplic.ServicePlatform.UI
{
    /// <summary>
    /// View model for the service module.
    /// </summary>
    public class ServiceModuleViewModel : ViewModelBase
    {
        public ServiceModuleViewModel(ServiceModule model)
        {
            Model = new ServiceModule { Name = model.Name, Configuration = new List<ServiceModuleConfiguration>(model.Configuration) };
            ConfigurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(Model.Configuration);
        }

        /// <summary>
        /// Synchronize all configurations from the binding with the model.
        /// </summary>
        public void Synch()
        {
            Model.Configuration = ConfigurationDefinitions.ToList();
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public ServiceModule Model { get; set; }

        /// <summary>
        /// Gets or sets the configurations.
        /// </summary>
        public ObservableCollection<ServiceModuleConfiguration> ConfigurationDefinitions { get; set; }
    }
}
