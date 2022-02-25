using Simplic.UI.MVC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ServicePlatform.UI
{
    //Viewmodel Stufe 3
    public class ServiceModuleViewModel : ViewModelBase
    {
        private ObservableCollection<ServiceModuleConfiguration> configurationDefinitions;
        private ServiceModule model;

        public ServiceModuleViewModel(ServiceModule model)
        {
            Model = model;
            configurationDefinitions = new ObservableCollection<ServiceModuleConfiguration>(model.Configuration);
            configurationDefinitions.Add(new ServiceModuleConfiguration() { Name = "Config1", Value = "Einstellung1"});
            configurationDefinitions.Add(new ServiceModuleConfiguration() { Name = "Config2", Value = "Einstellung2"});
            configurationDefinitions.Add(new ServiceModuleConfiguration() { Name = "Config3", Value = "Einstellung3"});
        }

        /// <summary>
        /// Synchronize all configurations from the binding with the model.
        /// </summary>
        public void Synch()
        {
            Model.Configuration = ConfigurationDefinitions;
        }

        public ServiceModule Model { get => model; set => model = value; }
        public ObservableCollection<ServiceModuleConfiguration> ConfigurationDefinitions { get => configurationDefinitions; set => configurationDefinitions = value; }
    }
}
